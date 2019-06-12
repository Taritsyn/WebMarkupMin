using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using Microsoft.Ajax.Utilities;
using MsEvalTreatment = Microsoft.Ajax.Utilities.EvalTreatment;
using MsLocalRenaming = Microsoft.Ajax.Utilities.LocalRenaming;

using WebMarkupMin.Core;
using WebMarkupMin.Core.Utilities;
using WebMarkupMin.MsAjax.Reporters;
using WmmEvalTreatment = WebMarkupMin.MsAjax.EvalTreatment;
using WmmLocalRenaming = WebMarkupMin.MsAjax.LocalRenaming;
using WmmStringBuilderPool = WebMarkupMin.Core.Utilities.StringBuilderPool;

namespace WebMarkupMin.MsAjax
{
	/// <summary>
	/// Minifier, which produces minifiction of JS-code by using the Microsoft Ajax JS Minifier
	/// </summary>
	public sealed class MsAjaxJsMinifier : MsAjaxMinifierBase, IJsMinifier
	{
		/// <summary>
		/// Microsoft Ajax JS Minifier settings
		/// </summary>
		private readonly MsAjaxJsMinificationSettings _settings;

		/// <summary>
		/// Error reporter
		/// </summary>
		private MsAjaxErrorReporter _errorReporter;

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
		private readonly object _minificationSynchronizer = new object();


		/// <summary>
		/// Constructs an instance of the Microsoft Ajax JS Minifier
		/// </summary>
		public MsAjaxJsMinifier() : this(new MsAjaxJsMinificationSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the Microsoft Ajax JS Minifier
		/// </summary>
		/// <param name="settings">Microsoft Ajax JS Minifier settings</param>
		public MsAjaxJsMinifier(MsAjaxJsMinificationSettings settings)
		{
			_settings = settings;
		}


		/// <summary>
		/// Creates a instance of original JS parser
		/// </summary>
		/// <param name="settings">JS minifier settings</param>
		/// <param name="isInlineCode">Flag for whether to create a JS parser for inline code</param>
		/// <returns>Instance of original JS parser</returns>
		private static JSParser CreateOriginalJsParserInstance(MsAjaxJsMinificationSettings settings,
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
			originalSettings.EvalTreatment = Utils.GetEnumFromOtherEnum<WmmEvalTreatment, MsEvalTreatment>(
				settings.EvalTreatment);
			originalSettings.IgnoreConditionalCompilation = settings.IgnoreConditionalCompilation;
			originalSettings.IgnorePreprocessorDefines = settings.IgnorePreprocessorDefines;
			originalSettings.InlineSafeStrings = settings.InlineSafeStrings;
			originalSettings.KnownGlobalNamesList = settings.KnownGlobalNamesList;
			originalSettings.LocalRenaming = Utils.GetEnumFromOtherEnum<WmmLocalRenaming, MsLocalRenaming>(
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
		/// Produces a code minifiction of JS content by using the Microsoft Ajax JS Minifier
		/// </summary>
		/// <param name="content">JS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <returns>Minification result</returns>
		public CodeMinificationResult Minify(string content, bool isInlineCode)
		{
			return Minify(content, isInlineCode, Encoding.Default);
		}

		/// <summary>
		/// Produces a code minifiction of JS content by using the Microsoft Ajax JS Minifier
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
				if (_errorReporter == null)
				{
					_errorReporter = new MsAjaxErrorReporter(_settings.WarningLevel);
				}

				JSParser originalJsParser = isInlineCode ?
					_originalInlineJsParser : _originalEmbeddedJsParser;
				if (originalJsParser == null)
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

				StringBuilder contentBuilder = WmmStringBuilderPool.GetBuilder(content.Length);
				var documentContext = new DocumentContext(content)
				{
					FileContext = string.Empty
				};

				try
				{
					using (var stringWriter = new StringWriter(contentBuilder, CultureInfo.InvariantCulture))
					{
						// Parse the input
						Block scriptBlock = originalJsParser.Parse(documentContext);
						if (scriptBlock != null)
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
					WmmStringBuilderPool.ReleaseBuilder(contentBuilder);

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