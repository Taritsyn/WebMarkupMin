using System;
using System.Collections.Generic;
using System.Text;

using Yahoo.Yui.Compressor;

using WebMarkupMin.Core;
using CoreStrings = WebMarkupMin.Core.Resources;

namespace WebMarkupMin.Yui
{
	/// <summary>
	/// Minifier, which produces minifiction of CSS-code by using the YUI CSS Compressor for .NET
	/// </summary>
	public sealed class YuiCssMinifier : YuiMinifierBase, ICssMinifier
	{
		/// <summary>
		/// Original CSS minifier for embedded code
		/// </summary>
		private readonly CssCompressor _originalEmbeddedCssMinifier;

		/// <summary>
		/// Original CSS minifier for inline code
		/// </summary>
		private readonly CssCompressor _originalInlineCssMinifier;

		/// <summary>
		/// Synchronizer of minification
		/// </summary>
		private readonly object _minificationSynchronizer = new object();

		/// <summary>
		/// Gets a value indicating the minifier supports inline code minification
		/// </summary>
		public bool IsInlineCodeMinificationSupported
		{
			get { return true; }
		}


		/// <summary>
		/// Constructs an instance of the YUI CSS Minifier
		/// </summary>
		public YuiCssMinifier() : this(new YuiCssMinificationSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the YUI CSS Minifier
		/// </summary>
		/// <param name="settings">Settings of YUI CSS Minifier</param>
		public YuiCssMinifier(YuiCssMinificationSettings settings)
		{
			_originalEmbeddedCssMinifier = CreateOriginalCssMinifierInstance(settings, false);
			_originalInlineCssMinifier = CreateOriginalCssMinifierInstance(settings, true);
		}


		/// <summary>
		/// Produces a code minifiction of CSS content by using the YUI CSS Compressor for .NET
		/// </summary>
		/// <param name="content">CSS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <returns>Minification result</returns>
		public CodeMinificationResult Minify(string content, bool isInlineCode)
		{
			return Minify(content, isInlineCode, Encoding.Default);
		}

		/// <summary>
		/// Produces a code minifiction of CSS content by using the YUI CSS Compressor for .NET
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

			CssCompressor originalCssMinifier = isInlineCode ?
				_originalInlineCssMinifier : _originalEmbeddedCssMinifier;

			string newContent = string.Empty;
			var errors = new List<MinificationErrorInfo>();

			try
			{
				lock (_minificationSynchronizer)
				{
					newContent = originalCssMinifier.Compress(content);
				}
			}
			catch (ArgumentOutOfRangeException)
			{
				errors.Add(new MinificationErrorInfo(CoreStrings.ErrorMessage_UnknownError, 0, 0, string.Empty));
			}

			return new CodeMinificationResult(newContent, errors);
		}

		/// <summary>
		/// Creates a instance of original CSS minifier
		/// </summary>
		/// <param name="settings">CSS minifier settings</param>
		/// <param name="isInlineCode">Flag for whether to create a settings for inline code</param>
		/// <returns>Instance of original CSS minifier</returns>
		private static CssCompressor CreateOriginalCssMinifierInstance(YuiCssMinificationSettings settings,
			bool isInlineCode)
		{
			var originalMinifier = new CssCompressor();
			ApplyCssSettingsToOriginalCssMinifier(originalMinifier, settings);
			if (isInlineCode)
			{
				originalMinifier.LineBreakPosition = -1;
			}

			return originalMinifier;
		}

		/// <summary>
		/// Applies a CSS settings to original CSS minifier
		/// </summary>
		/// <param name="originalMinifier">Original CSS minifier</param>
		/// <param name="settings">CSS minifier settings</param>
		private static void ApplyCssSettingsToOriginalCssMinifier(CssCompressor originalMinifier,
			YuiCssMinificationSettings settings)
		{
			ApplyCommonSettingsToOriginalMinifier(originalMinifier, settings);

			originalMinifier.RemoveComments = settings.RemoveComments;
		}
	}
}