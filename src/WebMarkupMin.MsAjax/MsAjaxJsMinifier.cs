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
using MsAjaxStrings = WebMarkupMin.MsAjax.Resources;
using WmmEvalTreatment = WebMarkupMin.MsAjax.EvalTreatment;
using WmmLocalRenaming = WebMarkupMin.MsAjax.LocalRenaming;

namespace WebMarkupMin.MsAjax
{
	/// <summary>
	/// Minifier, which produces minifiction of JS-code
	/// by using Microsoft Ajax JS Minifier
	/// </summary>
	public sealed class MsAjaxJsMinifier : MsAjaxMinifierBase, IJsMinifier
	{
		/// <summary>
		/// Microsoft JS Minifier settings
		/// </summary>
		private readonly MsAjaxJsMinificationSettings _settings;

		/// <summary>
		/// Gets a value indicating the minifier supports inline code minification
		/// </summary>
		public bool IsInlineCodeMinificationSupported
		{
			get { return true; }
		}


		/// <summary>
		/// Constructs instance of Microsoft Ajax JS Minifier
		/// </summary>
		public MsAjaxJsMinifier() : this(new MsAjaxJsMinificationSettings())
		{ }

		/// <summary>
		/// Constructs instance of Microsoft Ajax JS Minifier
		/// </summary>
		/// <param name="settings">Microsoft JS Minifier settings</param>
		public MsAjaxJsMinifier(MsAjaxJsMinificationSettings settings)
		{
			_settings = settings;
		}


		/// <summary>
		/// Produces code minifiction of JS content by using
		/// Microsoft Ajax JS Minifier
		/// </summary>
		/// <param name="content">JS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <returns>Minification result</returns>
		public CodeMinificationResult Minify(string content, bool isInlineCode)
		{
			return Minify(content, isInlineCode, Encoding.Default);
		}

		/// <summary>
		/// Produces code minifiction of JS content by using
		/// Microsoft Ajax JS Minifier
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

			string newContent;

			var errorReporter = new MsAjaxJsErrorReporter();
			var errors = new List<MinificationErrorInfo>();
			var warnings = new List<MinificationErrorInfo>();

			var jsParserConfiguration = isInlineCode ? GetInlineJsParserSettings() : GetEmbeddedJsParserSettings();

			var jsParser = new JSParser
			{
				Settings = jsParserConfiguration
			};
			jsParser.CompilerError += errorReporter.JsMinificationErrorHandler;

			try
			{
				var stringBuilder = new StringBuilder();

				using (var stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
				{
					Block block = jsParser.Parse(content);
					if (block != null)
					{
						if (jsParserConfiguration.Format == JavaScriptFormat.JSON)
						{
							// Use a JSON output visitor
							if (!JSONOutputVisitor.Apply(stringWriter, block, jsParserConfiguration))
							{
								errors.Add(new MinificationErrorInfo(MsAjaxStrings.ErrorMessage_InvalidJsonOutput));
							}
						}
						else
						{
							// Use normal output visitor
							OutputVisitor.Apply(stringWriter, block, jsParserConfiguration);
						}
					}
				}

				newContent = stringBuilder.ToString();
			}
			finally
			{
				jsParser.CompilerError -= errorReporter.JsMinificationErrorHandler;
			}

			errors.AddRange(errorReporter.Errors);
			warnings.AddRange(errorReporter.Warnings);

			return new CodeMinificationResult(newContent, errors, warnings);
		}

		/// <summary>
		/// Gets a embedded JS-code parser settings
		/// </summary>
		/// <returns>Embedded JS-code parser settings</returns>
		private CodeSettings GetEmbeddedJsParserSettings()
		{
			var embeddedJsParserSettings = new CodeSettings();
			MapJsSettings(embeddedJsParserSettings, _settings);
			embeddedJsParserSettings.SourceMode = JavaScriptSourceMode.Program;

			return embeddedJsParserSettings;
		}

		/// <summary>
		/// Gets a inline JS-code parser settings
		/// </summary>
		/// <returns>Inline JS-code parser settings</returns>
		private CodeSettings GetInlineJsParserSettings()
		{
			var inlineJsParserSettings = new CodeSettings();
			MapJsSettings(inlineJsParserSettings, _settings);
			inlineJsParserSettings.SourceMode = JavaScriptSourceMode.EventHandler;

			return inlineJsParserSettings;
		}

		/// <summary>
		/// Maps a JS settings
		/// </summary>
		/// <param name="jsParserSettings">JS-code parser settings</param>
		/// <param name="jsMinifierSettings">Microsoft JS Minifier settings</param>
		private static void MapJsSettings(CodeSettings jsParserSettings, MsAjaxJsMinificationSettings jsMinifierSettings)
		{
			MapCommonSettings(jsParserSettings, jsMinifierSettings);

			jsParserSettings.AlwaysEscapeNonAscii = jsMinifierSettings.AlwaysEscapeNonAscii;
			jsParserSettings.AmdSupport = jsMinifierSettings.AmdSupport;
			jsParserSettings.CollapseToLiteral = jsMinifierSettings.CollapseToLiteral;
			jsParserSettings.ConstStatementsMozilla = jsMinifierSettings.ConstStatementsMozilla;
			jsParserSettings.DebugLookupList = jsMinifierSettings.DebugLookupList;
			jsParserSettings.ErrorIfNotInlineSafe = jsMinifierSettings.ErrorIfNotInlineSafe;
			jsParserSettings.EvalLiteralExpressions = jsMinifierSettings.EvalLiteralExpressions;
			jsParserSettings.EvalTreatment = Utils.GetEnumFromOtherEnum<WmmEvalTreatment, MsEvalTreatment>(
				jsMinifierSettings.EvalTreatment);
			jsParserSettings.IgnoreConditionalCompilation = jsMinifierSettings.IgnoreConditionalCompilation;
			jsParserSettings.IgnorePreprocessorDefines = jsMinifierSettings.IgnorePreprocessorDefines;
			jsParserSettings.InlineSafeStrings = jsMinifierSettings.InlineSafeStrings;
			jsParserSettings.KnownGlobalNamesList = jsMinifierSettings.KnownGlobalNamesList;
			jsParserSettings.LocalRenaming = Utils.GetEnumFromOtherEnum<WmmLocalRenaming, MsLocalRenaming>(
				jsMinifierSettings.LocalRenaming);
			jsParserSettings.MacSafariQuirks = jsMinifierSettings.MacSafariQuirks;
			jsParserSettings.ManualRenamesProperties = jsMinifierSettings.ManualRenamesProperties;
			jsParserSettings.NoAutoRenameList = jsMinifierSettings.NoAutoRenameList;
			jsParserSettings.PreserveFunctionNames = jsMinifierSettings.PreserveFunctionNames;
			jsParserSettings.PreserveImportantComments = jsMinifierSettings.PreserveImportantComments;
			jsParserSettings.QuoteObjectLiteralProperties = jsMinifierSettings.QuoteObjectLiteralProperties;
			jsParserSettings.RemoveFunctionExpressionNames = jsMinifierSettings.RemoveFunctionExpressionNames;
			jsParserSettings.RemoveUnneededCode = jsMinifierSettings.RemoveUnneededCode;
			jsParserSettings.RenamePairs = jsMinifierSettings.RenamePairs;
			jsParserSettings.ReorderScopeDeclarations = jsMinifierSettings.ReorderScopeDeclarations;
			jsParserSettings.StrictMode = jsMinifierSettings.StrictMode;
			jsParserSettings.StripDebugStatements = jsMinifierSettings.StripDebugStatements;
		}
	}
}