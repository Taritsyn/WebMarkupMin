using System.Text;

using NUglify;
using NUglify.JavaScript;
using NuEvalTreatment = NUglify.JavaScript.EvalTreatment;
using NuLocalRenaming = NUglify.JavaScript.LocalRenaming;

using WebMarkupMin.Core;
using WebMarkupMin.Core.Utilities;
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
		/// Original JS minifier settings for embedded JS-code
		/// </summary>
		private readonly CodeSettings _originalEmbeddedJsSettings;

		/// <summary>
		/// Original JS minifier settings for inline JS-code
		/// </summary>
		private readonly CodeSettings _originalInlineJsSettings;

		/// <summary>
		/// Gets a value indicating the minifier supports inline code minification
		/// </summary>
		public bool IsInlineCodeMinificationSupported
		{
			get { return true; }
		}


		/// <summary>
		/// Constructs an instance of the NUglify JS Minifier
		/// </summary>
		public NUglifyJsMinifier() : this(new NUglifyJsMinificationSettings())
		{ }

		/// <summary>
		/// Constructs instance of NUglify JS Minifier
		/// </summary>
		/// <param name="settings">NUglify JS Minifier settings</param>
		public NUglifyJsMinifier(NUglifyJsMinificationSettings settings)
		{
			_originalEmbeddedJsSettings = CreateOriginalJsMinifierSettings(settings, false);
			_originalInlineJsSettings = CreateOriginalJsMinifierSettings(settings, true);
		}


		/// <summary>
		/// Produces a code minifiction of JS content by using the NUglify JS Minifier
		/// </summary>
		/// <param name="content">JS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <returns>Minification result</returns>
		public CodeMinificationResult Minify(string content, bool isInlineCode)
		{
			return Minify(content, isInlineCode, Encoding.GetEncoding(0));
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

			CodeSettings originalJsSettings = isInlineCode ?
				_originalInlineJsSettings : _originalEmbeddedJsSettings;
			UglifyResult originalResult = Uglify.Js(content, originalJsSettings);
			CodeMinificationResult result = GetCodeMinificationResult(originalResult);

			return result;
		}

		/// <summary>
		/// Creates a original JS minifier settings
		/// </summary>
		/// <param name="settings">JS minifier settings</param>
		/// <param name="isInlineCode">Flag for whether to create a settings for inline code</param>
		/// <returns>Original JS minifier settings</returns>
		private static CodeSettings CreateOriginalJsMinifierSettings(NUglifyJsMinificationSettings settings,
			bool isInlineCode)
		{
			var originalSettings = new CodeSettings();
			MapJsSettings(originalSettings, settings);
			originalSettings.SourceMode = isInlineCode ?
				JavaScriptSourceMode.EventHandler : JavaScriptSourceMode.Program;

			return originalSettings;
		}

		/// <summary>
		/// Maps a JS minifier settings
		/// </summary>
		/// <param name="originalSettings">Original JS minifier settings</param>
		/// <param name="settings">JS minifier settings</param>
		private static void MapJsSettings(CodeSettings originalSettings, NUglifyJsMinificationSettings settings)
		{
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
			originalSettings.StrictMode = settings.StrictMode;
			originalSettings.StripDebugStatements = settings.StripDebugStatements;
		}
	}
}