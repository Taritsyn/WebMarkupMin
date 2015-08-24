using System.Text;

using Microsoft.Ajax.Utilities;
using MsCssColor = Microsoft.Ajax.Utilities.CssColor;
using MsCssComment = Microsoft.Ajax.Utilities.CssComment;

using WebMarkupMin.Core;
using WebMarkupMin.Core.Utilities;
using WebMarkupMin.MsAjax.Reporters;
using WmmCssColor = WebMarkupMin.MsAjax.CssColor;
using WmmCssComment = WebMarkupMin.MsAjax.CssComment;

namespace WebMarkupMin.MsAjax
{
	/// <summary>
	/// Minifier, which produces minifiction of CSS-code
	/// by using Microsoft Ajax CSS Minifier
	/// </summary>
	public sealed class MsAjaxCssMinifier : MsAjaxMinifierBase, ICssMinifier
	{
		/// <summary>
		/// Microsoft CSS Minifier settings
		/// </summary>
		private readonly MsAjaxCssMinificationSettings _settings;

		/// <summary>
		/// Gets a value indicating the minifier supports inline code minification
		/// </summary>
		public bool IsInlineCodeMinificationSupported
		{
			get { return true; }
		}


		/// <summary>
		/// Constructs instance of Microsoft Ajax CSS Minifier
		/// </summary>
		public MsAjaxCssMinifier() : this(new MsAjaxCssMinificationSettings())
		{ }

		/// <summary>
		/// Constructs instance of Microsoft Ajax CSS Minifier
		/// </summary>
		/// <param name="settings">Microsoft CSS Minifier settings</param>
		public MsAjaxCssMinifier(MsAjaxCssMinificationSettings settings)
		{
			_settings = settings;
		}


		/// <summary>
		/// Produces code minifiction of CSS content by using
		/// Microsoft Ajax CSS Minifier
		/// </summary>
		/// <param name="content">CSS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <returns>Minification result</returns>
		public CodeMinificationResult Minify(string content, bool isInlineCode)
		{
			return Minify(content, isInlineCode, Encoding.Default);
		}

		/// <summary>
		/// Produces code minifiction of CSS content by using
		/// Microsoft Ajax CSS Minifier
		/// </summary>
		/// <param name="content">CSS content</param>
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
			var errorReporter = new MsAjaxCssErrorReporter();

			var cssParser = new CssParser
			{
				Settings = isInlineCode ? GetInlineCssParserSettings() : GetEmbeddedCssParserSettings()
			};
			cssParser.CssError += errorReporter.CssMinificationErrorHandler;

			try
			{
				newContent = cssParser.Parse(content);
			}
			finally
			{
				cssParser.CssError -= errorReporter.CssMinificationErrorHandler;
			}

			return new CodeMinificationResult(newContent, errorReporter.Errors, errorReporter.Warnings);
		}

		/// <summary>
		/// Gets a embedded CSS-code parser settings
		/// </summary>
		/// <returns>Embedded CSS-code parser settings</returns>
		private CssSettings GetEmbeddedCssParserSettings()
		{
			var embeddedCssParserSettings = new CssSettings();
			MapCssSettings(embeddedCssParserSettings, _settings);
			embeddedCssParserSettings.CssType = CssType.FullStyleSheet;

			return embeddedCssParserSettings;
		}

		/// <summary>
		/// Gets a inline CSS-code parser settings
		/// </summary>
		/// <returns>Inline CSS-code parser settings</returns>
		private CssSettings GetInlineCssParserSettings()
		{
			var inlineCssParserSettings = new CssSettings();
			MapCssSettings(inlineCssParserSettings, _settings);
			inlineCssParserSettings.CssType = CssType.DeclarationList;

			return inlineCssParserSettings;
		}

		/// <summary>
		/// Maps a CSS settings
		/// </summary>
		/// <param name="cssParserSettings">CSS-code parser settings</param>
		/// <param name="cssMinifierSettings">Microsoft CSS Minifier settings</param>
		private static void MapCssSettings(CssSettings cssParserSettings,
			MsAjaxCssMinificationSettings cssMinifierSettings)
		{
			MapCommonSettings(cssParserSettings, cssMinifierSettings);

			cssParserSettings.ColorNames = Utils.GetEnumFromOtherEnum<WmmCssColor, MsCssColor>(
				cssMinifierSettings.ColorNames);
			cssParserSettings.CommentMode = Utils.GetEnumFromOtherEnum<WmmCssComment, MsCssComment>(
				cssMinifierSettings.CommentMode);
			cssParserSettings.MinifyExpressions = cssMinifierSettings.MinifyExpressions;
			cssParserSettings.RemoveEmptyBlocks = cssMinifierSettings.RemoveEmptyBlocks;
		}
	}
}