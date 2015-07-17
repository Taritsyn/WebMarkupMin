using System;
using System.Collections.Generic;
using System.Text;

using Yahoo.Yui.Compressor;

using WebMarkupMin.Core;
using WebMarkupMin.Core.Resources;

namespace WebMarkupMin.Yui
{
	/// <summary>
	/// Minifier, which produces minifiction of CSS-code
	/// by using YUI CSS Compressor for .NET
	/// </summary>
	public sealed class YuiCssMinifier : YuiMinifierBase, ICssMinifier
	{
		/// <summary>
		/// Settings of YUI CSS Minifier
		/// </summary>
		private YuiCssMinificationSettings _settings;

		/// <summary>
		/// Gets a value indicating the minifier supports inline code minification
		/// </summary>
		public bool IsInlineCodeMinificationSupported
		{
			get { return true; }
		}


		/// <summary>
		/// Constructs instance of YUI CSS Minifier
		/// </summary>
		public YuiCssMinifier() : this(new YuiCssMinificationSettings())
		{ }

		/// <summary>
		/// Constructs instance of YUI CSS Minifier
		/// </summary>
		/// <param name="settings">Settings of YUI CSS Minifier</param>
		public YuiCssMinifier(YuiCssMinificationSettings settings)
		{
			_settings = settings;
		}


		/// <summary>
		/// Produces code minifiction of CSS content by using
		/// YUI CSS Compressor for .NET
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
		/// YUI CSS Compressor for .NET
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

			var errors = new List<MinificationErrorInfo>();

			var cssCompressor = isInlineCode ? CreateInlineCssCompressorInstance() : CreateEmbeddedCssCompressorInstance();

			string newContent = string.Empty;

			try
			{
				newContent = cssCompressor.Compress(content);
			}
			catch (ArgumentOutOfRangeException)
			{
				errors.Add(new MinificationErrorInfo(Strings.ErrorMessage_UnknownError, 0, 0, string.Empty));
			}

			return new CodeMinificationResult(newContent, errors);
		}

		/// <summary>
		/// Creates a instance of embedded CSS-code compressor
		/// </summary>
		/// <returns>Embedded CSS-code compressor</returns>
		private CssCompressor CreateEmbeddedCssCompressorInstance()
		{
			var embeddedCssCompressor = new CssCompressor();
			ApplyCssSettingsToCssCompressor(embeddedCssCompressor, _settings);

			return embeddedCssCompressor;
		}

		/// <summary>
		/// Creates a instance of inline CSS-code compressor
		/// </summary>
		/// <returns>Inline CSS-code compressor</returns>
		private CssCompressor CreateInlineCssCompressorInstance()
		{
			var inlineCssCompressor = new CssCompressor();
			ApplyCssSettingsToCssCompressor(inlineCssCompressor, _settings);
			inlineCssCompressor.LineBreakPosition = -1;

			return inlineCssCompressor;
		}

		/// <summary>
		/// Applies a settings to CSS-code compressor
		/// </summary>
		/// <param name="cssCompressor">CSS-code compressor</param>
		/// <param name="cssSettings">Settings of YUI CSS Minifier</param>
		private static void ApplyCssSettingsToCssCompressor(CssCompressor cssCompressor,
			YuiCssMinificationSettings cssSettings)
		{
			ApplyCommonSettingsToCompressor(cssCompressor, cssSettings);

			cssCompressor.RemoveComments = cssSettings.RemoveComments;
		}
	}
}