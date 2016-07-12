using System.Collections.Generic;
using System.Text;

using Microsoft.Ajax.Utilities;
using MsCssColor = Microsoft.Ajax.Utilities.CssColor;
using MsCssComment = Microsoft.Ajax.Utilities.CssComment;

using WebMarkupMin.Core;
using WebMarkupMin.Core.Utilities;
using WmmCssColor = WebMarkupMin.MsAjax.CssColor;
using WmmCssComment = WebMarkupMin.MsAjax.CssComment;

namespace WebMarkupMin.MsAjax
{
	/// <summary>
	/// Minifier, which produces minifiction of CSS-code by using the Microsoft Ajax CSS Minifier
	/// </summary>
	public sealed class MsAjaxCssMinifier : MsAjaxMinifierBase, ICssMinifier
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
		/// Constructs an instance of the Microsoft Ajax CSS Minifier
		/// </summary>
		public MsAjaxCssMinifier() : this(new MsAjaxCssMinificationSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the Microsoft Ajax CSS Minifier
		/// </summary>
		/// <param name="settings">Microsoft Ajax CSS Minifier settings</param>
		public MsAjaxCssMinifier(MsAjaxCssMinificationSettings settings)
		{
			_originalEmbeddedCssSettings = CreateOriginalCssMinifierSettings(settings, false);
			_originalInlineCssSettings = CreateOriginalCssMinifierSettings(settings, true);
			_originalJsSettings = new CodeSettings();
		}


		/// <summary>
		/// Produces a code minifiction of CSS content by using the Microsoft Ajax CSS Minifier
		/// </summary>
		/// <param name="content">CSS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <returns>Minification result</returns>
		public CodeMinificationResult Minify(string content, bool isInlineCode)
		{
			return Minify(content, isInlineCode, Encoding.Default);
		}

		/// <summary>
		/// Produces a code minifiction of CSS content by using the Microsoft Ajax CSS Minifier
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
			var originalMinifier = new Minifier
			{
				WarningLevel = 2
			};

			string newContent = originalMinifier.MinifyStyleSheet(content, originalCssSettings, _originalJsSettings);
			ICollection<ContextError> originalErrors = originalMinifier.ErrorList;

			var errors = new List<MinificationErrorInfo>();
			var warnings = new List<MinificationErrorInfo>();
			MapErrors(originalErrors, errors, warnings);

			return new CodeMinificationResult(newContent, errors, warnings);
		}

		/// <summary>
		/// Creates a original CSS minifier settings
		/// </summary>
		/// <param name="settings">CSS minifier settings</param>
		/// <param name="isInlineCode">Flag for whether to create a settings for inline code</param>
		/// <returns>Original CSS minifier settings</returns>
		private static CssSettings CreateOriginalCssMinifierSettings(MsAjaxCssMinificationSettings settings,
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
		private static void MapCssSettings(CssSettings originalSettings, MsAjaxCssMinificationSettings settings)
		{
			MapCommonSettings(originalSettings, settings);

			originalSettings.ColorNames = Utils.GetEnumFromOtherEnum<WmmCssColor, MsCssColor>(
				settings.ColorNames);
			originalSettings.CommentMode = Utils.GetEnumFromOtherEnum<WmmCssComment, MsCssComment>(
				settings.CommentMode);
			originalSettings.MinifyExpressions = settings.MinifyExpressions;
			originalSettings.RemoveEmptyBlocks = settings.RemoveEmptyBlocks;
		}
	}
}