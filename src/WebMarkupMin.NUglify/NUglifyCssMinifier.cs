using System.Text;

using NUglify;
using NUglify.Css;
using NUglify.JavaScript;
using NuCssColor = NUglify.Css.CssColor;
using NuCssComment = NUglify.Css.CssComment;

using WebMarkupMin.Core;
using WebMarkupMin.Core.Utilities;
using WmmCssColor = WebMarkupMin.NUglify.CssColor;
using WmmCssComment = WebMarkupMin.NUglify.CssComment;

namespace WebMarkupMin.NUglify
{
	/// <summary>
	/// Minifier, which produces minifiction of CSS-code by using the NUglify CSS Minifier
	/// </summary>
	public sealed class NUglifyCssMinifier : NUglifyMinifierBase, ICssMinifier
	{
		/// <summary>
		/// Original CSS minifier settings for embedded code
		/// </summary>
		private readonly CssSettings _originalEmbeddedCssSettings;

		/// <summary>
		/// Original CSS minifier settings for inline code
		/// </summary>
		private readonly CssSettings _originalInlineCssSettings;

		/// <summary>
		/// Original JS minifier settings
		/// </summary>
		private readonly CodeSettings _originalJsSettings;

		/// <summary>
		/// Gets a value indicating the minifier supports inline code minification
		/// </summary>
		public bool IsInlineCodeMinificationSupported
		{
			get { return true; }
		}


		/// <summary>
		/// Constructs an instance of the NUglify CSS Minifier
		/// </summary>
		public NUglifyCssMinifier() : this(new NUglifyCssMinificationSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the NUglify CSS Minifier
		/// </summary>
		/// <param name="settings">NUglify CSS Minifier settings</param>
		public NUglifyCssMinifier(NUglifyCssMinificationSettings settings)
		{
			_originalEmbeddedCssSettings = CreateOriginalCssMinifierSettings(settings, false);
			_originalInlineCssSettings = CreateOriginalCssMinifierSettings(settings, true);
			_originalJsSettings = new CodeSettings();
		}


		/// <summary>
		/// Produces a code minifiction of CSS content by using the NUglify CSS Minifier
		/// </summary>
		/// <param name="content">CSS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <returns>Minification result</returns>
		public CodeMinificationResult Minify(string content, bool isInlineCode)
		{
			return Minify(content, isInlineCode, Encoding.GetEncoding(0));
		}

		/// <summary>
		/// Produces a code minifiction of CSS content by using the NUglify CSS Minifier
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

			CssSettings originalCssSettings = isInlineCode ?
				_originalInlineCssSettings : _originalEmbeddedCssSettings;
			UglifyResult originalResult = Uglify.Css(content, originalCssSettings, _originalJsSettings);
			CodeMinificationResult result = GetCodeMinificationResult(originalResult);

			return result;
		}

		/// <summary>
		/// Creates a original CSS minifier settings
		/// </summary>
		/// <param name="settings">CSS minifier settings</param>
		/// <param name="isInlineCode">Flag for whether to create a settings for inline code</param>
		/// <returns>Original CSS minifier settings</returns>
		private static CssSettings CreateOriginalCssMinifierSettings(NUglifyCssMinificationSettings settings,
			bool isInlineCode)
		{
			var originalSettings = new CssSettings();
			MapCssSettings(originalSettings, settings);
			originalSettings.CssType = isInlineCode ? CssType.DeclarationList : CssType.FullStyleSheet;

			return originalSettings;
		}

		/// <summary>
		/// Maps a CSS minifier settings
		/// </summary>
		/// <param name="originalSettings">Original CSS minifier settings</param>
		/// <param name="settings">CSS minifier settings</param>
		private static void MapCssSettings(CssSettings originalSettings, NUglifyCssMinificationSettings settings)
		{
			MapCommonSettings(originalSettings, settings);

			originalSettings.ColorNames = Utils.GetEnumFromOtherEnum<WmmCssColor, NuCssColor>(
				settings.ColorNames);
			originalSettings.CommentMode = Utils.GetEnumFromOtherEnum<WmmCssComment, NuCssComment>(
				settings.CommentMode);
			originalSettings.MinifyExpressions = settings.MinifyExpressions;
			originalSettings.RemoveEmptyBlocks = settings.RemoveEmptyBlocks;
		}
	}
}