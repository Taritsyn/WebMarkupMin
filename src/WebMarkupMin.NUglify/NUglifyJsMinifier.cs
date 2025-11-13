using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
#if NET9_0_OR_GREATER
using Lock = System.Threading.Lock;
#else
using Lock = System.Object;
#endif

using AdvancedStringBuilder;
using NUglify.JavaScript;
using NUglify.JavaScript.Syntax;
using NUglify.JavaScript.Visitors;
using NuEvalTreatment = NUglify.JavaScript.EvalTreatment;
using NuLocalRenaming = NUglify.JavaScript.LocalRenaming;

using WebMarkupMin.Core;
using WebMarkupMin.Core.Utilities;
using WebMarkupMin.NUglify.Reporters;
using WmmEvalTreatment = WebMarkupMin.NUglify.EvalTreatment;
using WmmLocalRenaming = WebMarkupMin.NUglify.LocalRenaming;

namespace WebMarkupMin.NUglify
{
	/// <summary>
	/// Minifier, which produces minifiction of JS-code by using the NUglify JS Minifier
	/// </summary>
	public sealed class NUglifyJsMinifier : NUglifyMinifierBase, IJsMinifier
	{
		/// <summary>
		/// NUglify JS Minifier settings
		/// </summary>
		private readonly NUglifyJsMinificationSettings _settings;

		/// <summary>
		/// Error reporter
		/// </summary>
		private NUglifyErrorReporter _errorReporter;

		/// <summary>
		/// Original JS parser for embedded code
		/// </summary>
		private JSParser _originalEmbeddedJsParser;

		/// <summary>
		/// Original JS parser for inline code
		/// </summary>
		private JSParser _originalInlineJsParser;

		/// <summary>
		/// Synchronizer of minification
		/// </summary>
		private readonly Lock _minificationSynchronizer = new Lock();


		/// <summary>
		/// Constructs an instance of the NUglify JS Minifier
		/// </summary>
		public NUglifyJsMinifier()
			: this(new NUglifyJsMinificationSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the NUglify JS Minifier
		/// </summary>
		/// <param name="settings">NUglify JS Minifier settings</param>
		public NUglifyJsMinifier(NUglifyJsMinificationSettings settings)
		{
			_settings = settings;
		}


		/// <summary>
		/// Creates a instance of original JS parser
		/// </summary>
		/// <param name="settings">JS minifier settings</param>
		/// <param name="isInlineCode">Flag for whether to create a JS parser for inline code</param>
		/// <returns>Instance of original JS parser</returns>
		private static JSParser CreateOriginalJsParserInstance(NUglifyJsMinificationSettings settings,
			bool isInlineCode)
		{
			var originalSettings = new CodeSettings();
			MapCommonSettings(originalSettings, settings);
			originalSettings.AlwaysEscapeNonAscii = settings.AlwaysEscapeNonAscii;
			originalSettings.AmdSupport = settings.AmdSupport;
			originalSettings.CollapseToLiteral = settings.CollapseToLiteral;
			originalSettings.ConstStatementsMozilla = settings.ConstStatementsMozilla;
			originalSettings.DebugLookupList = settings.DebugLookupList;
			originalSettings.ErrorIfNotInlineSafe = settings.ErrorIfNotInlineSafe;
			originalSettings.EvalLiteralExpressions = settings.EvalLiteralExpressions;
			originalSettings.EvalTreatment = Utils.GetEnumFromOtherEnum<WmmEvalTreatment, NuEvalTreatment>(
				settings.EvalTreatment);
			originalSettings.IgnoreConditionalCompilation = settings.IgnoreConditionalCompilation;
			originalSettings.IgnorePreprocessorDefines = settings.IgnorePreprocessorDefines;
			originalSettings.InlineSafeStrings = settings.InlineSafeStrings;
			originalSettings.KnownGlobalNamesList = settings.KnownGlobalNamesList;
			originalSettings.LocalRenaming = Utils.GetEnumFromOtherEnum<WmmLocalRenaming, NuLocalRenaming>(
				settings.LocalRenaming);
			originalSettings.MacSafariQuirks = settings.MacSafariQuirks;
			originalSettings.ManualRenamesProperties = settings.ManualRenamesProperties;
			originalSettings.NoAutoRenameList = settings.NoAutoRenameList;
			originalSettings.PreserveFunctionNames = settings.PreserveFunctionNames;
			originalSettings.PreserveImportantComments = settings.PreserveImportantComments;
			originalSettings.QuoteObjectLiteralProperties = settings.QuoteObjectLiteralProperties;
			originalSettings.RemoveFunctionExpressionNames = settings.RemoveFunctionExpressionNames;
			originalSettings.RemoveUnneededCode = settings.RemoveUnneededCode;
			originalSettings.RenamePairs = settings.RenamePairs;
			originalSettings.ReorderScopeDeclarations = settings.ReorderScopeDeclarations;
			originalSettings.SourceMode = isInlineCode ?
				JavaScriptSourceMode.EventHandler : JavaScriptSourceMode.Program;
			originalSettings.StrictMode = settings.StrictMode;
			originalSettings.StripDebugStatements = settings.StripDebugStatements;

			var originalParser = new JSParser()
			{
				Settings = originalSettings
			};

			return originalParser;
		}

		#region IJsMinifier implementation

		/// <summary>
		/// Gets a value indicating the minifier supports inline code minification
		/// </summary>
		public bool IsInlineCodeMinificationSupported
		{
			get { return true; }
		}


		/// <summary>
		/// Produces a code minifiction of JS content by using the NUglify JS Minifier
		/// </summary>
		/// <param name="content">JS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <returns>Minification result</returns>
		public CodeMinificationResult Minify(string content, bool isInlineCode)
		{
			return Minify(content, isInlineCode, TargetFrameworkShortcuts.DefaultTextEncoding);
		}

		/// <summary>
		/// Produces a code minifiction of JS content by using the NUglify JS Minifier
		/// </summary>
		/// <param name="content">JS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <param name="encoding">Text encoding</param>
		/// <returns>Minification result</returns>
		public CodeMinificationResult Minify(string content, bool isInlineCode, Encoding encoding)
		{
			if (string.IsNullOrWhiteSpace(content))
			{
				return new CodeMinificationResult(string.Empty);
			}

			string newContent = string.Empty;
			var errors = new List<MinificationErrorInfo>();
			var warnings = new List<MinificationErrorInfo>();

			lock (_minificationSynchronizer)
			{
				if (_errorReporter is null)
				{
					_errorReporter = new NUglifyErrorReporter(_settings.WarningLevel);
				}

				JSParser originalJsParser = isInlineCode ?
					_originalInlineJsParser : _originalEmbeddedJsParser;
				if (originalJsParser is null)
				{
					originalJsParser = CreateOriginalJsParserInstance(_settings, isInlineCode);
					if (isInlineCode)
					{
						_originalInlineJsParser = originalJsParser;
					}
					else
					{
						_originalEmbeddedJsParser = originalJsParser;
					}
				}

				originalJsParser.CompilerError += _errorReporter.ParseErrorHandler;

				var stringBuilderPool = StringBuilderPool.Shared;
				StringBuilder contentBuilder = stringBuilderPool.Rent(content.Length);
				var documentContext = new DocumentContext(content)
				{
					FileContext = string.Empty
				};

				try
				{
					using (var stringWriter = new StringWriter(contentBuilder, CultureInfo.InvariantCulture))
					{
						// Parse the input
						BlockStatement scriptBlock = originalJsParser.Parse(documentContext);
						if (scriptBlock is not null)
						{
							// Use normal output visitor
							OutputVisitor.Apply(stringWriter, scriptBlock, originalJsParser.Settings);
						}
					}

					newContent = contentBuilder.ToString();
				}
				finally
				{
					originalJsParser.CompilerError -= _errorReporter.ParseErrorHandler;
					stringBuilderPool.Return(contentBuilder);

					errors.AddRange(_errorReporter.Errors);
					warnings.AddRange(_errorReporter.Warnings);

					_errorReporter.Clear();
				}
			}

			return new CodeMinificationResult(newContent, errors, warnings);
		}

		#endregion
	}
}