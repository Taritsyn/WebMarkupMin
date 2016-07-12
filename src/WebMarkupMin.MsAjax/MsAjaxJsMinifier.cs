using System.Collections.Generic;
using System.Text;

using Microsoft.Ajax.Utilities;
using MsEvalTreatment = Microsoft.Ajax.Utilities.EvalTreatment;
using MsLocalRenaming = Microsoft.Ajax.Utilities.LocalRenaming;

using WebMarkupMin.Core;
using WebMarkupMin.Core.Utilities;
using WmmEvalTreatment = WebMarkupMin.MsAjax.EvalTreatment;
using WmmLocalRenaming = WebMarkupMin.MsAjax.LocalRenaming;

namespace WebMarkupMin.MsAjax
{
	/// <summary>
	/// Minifier, which produces minifiction of JS-code by using the Microsoft Ajax JS Minifier
	/// </summary>
	public sealed class MsAjaxJsMinifier : MsAjaxMinifierBase, IJsMinifier
	{
		/// <summary>
		/// Original JS minifier settings for embedded code
		/// </summary>
		private readonly CodeSettings _originalEmbeddedJsSettings;

		/// <summary>
		/// Original JS minifier settings for inline code
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
			_originalEmbeddedJsSettings = CreateOriginalJsMinifierSettings(settings, false);
			_originalInlineJsSettings = CreateOriginalJsMinifierSettings(settings, true);
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

			CodeSettings originalJsSettings = isInlineCode ?
				_originalInlineJsSettings : _originalEmbeddedJsSettings;
			var originalMinifier = new Minifier
			{
				WarningLevel = 2
			};

			string newContent = originalMinifier.MinifyJavaScript(content, originalJsSettings);
			ICollection<ContextError> originalErrors = originalMinifier.ErrorList;

			var errors = new List<MinificationErrorInfo>();
			var warnings = new List<MinificationErrorInfo>();
			MapErrors(originalErrors, errors, warnings);

			return new CodeMinificationResult(newContent, errors, warnings);
		}

		/// <summary>
		/// Creates a original JS minifier settings
		/// </summary>
		/// <param name="settings">JS minifier settings</param>
		/// <param name="isInlineCode">Flag for whether to create a settings for inline code</param>
		/// <returns>Original JS minifier settings</returns>
		private static CodeSettings CreateOriginalJsMinifierSettings(MsAjaxJsMinificationSettings settings,
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
		private static void MapJsSettings(CodeSettings originalSettings, MsAjaxJsMinificationSettings settings)
		{
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
			originalSettings.StrictMode = settings.StrictMode;
			originalSettings.StripDebugStatements = settings.StripDebugStatements;
		}
	}
}