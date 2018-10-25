using System;
using System.Collections.Generic;
using System.Text;

using Yahoo.Yui.Compressor;

using WebMarkupMin.Core;
using CoreStrings = WebMarkupMin.Core.Resources.Strings;

namespace WebMarkupMin.Yui
{
	/// <summary>
	/// Minifier, which produces minifiction of CSS-code by using the YUI CSS Compressor for .NET
	/// </summary>
	public sealed class YuiCssMinifier : YuiMinifierBase, ICssMinifier
	{
		/// <summary>
		/// Settings of the YUI CSS Minifier
		/// </summary>
		private readonly YuiCssMinificationSettings _settings;

		/// <summary>
		/// Original CSS minifier
		/// </summary>
		private CssCompressor _originalCssMinifier;

		/// <summary>
		/// Synchronizer of minification
		/// </summary>
		private readonly object _minificationSynchronizer = new object();


		/// <summary>
		/// Constructs an instance of the YUI CSS Minifier
		/// </summary>
		public YuiCssMinifier()
			: this(new YuiCssMinificationSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the YUI CSS Minifier
		/// </summary>
		/// <param name="settings">Settings of the YUI CSS Minifier</param>
		public YuiCssMinifier(YuiCssMinificationSettings settings)
		{
			_settings = settings;
		}


		/// <summary>
		/// Creates a instance of original CSS minifier
		/// </summary>
		/// <param name="settings">CSS minifier settings</param>
		/// <returns>Instance of original CSS minifier</returns>
		private static CssCompressor CreateOriginalCssMinifierInstance(YuiCssMinificationSettings settings)
		{
			var originalMinifier = new CssCompressor();
			ApplyCommonSettingsToOriginalMinifier(originalMinifier, settings);
			originalMinifier.RemoveComments = settings.RemoveComments;

			return originalMinifier;
		}


		#region ICssMinifier implementation

		/// <summary>
		/// Gets a value indicating the minifier supports inline code minification
		/// </summary>
		public bool IsInlineCodeMinificationSupported
		{
			get { return true; }
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

			string newContent = string.Empty;
			var errors = new List<MinificationErrorInfo>();

			try
			{
				lock (_minificationSynchronizer)
				{
					if (_originalCssMinifier == null)
					{
						_originalCssMinifier = CreateOriginalCssMinifierInstance(_settings);
					}

					newContent = _originalCssMinifier.Compress(content);
				}
			}
			catch (ArgumentOutOfRangeException)
			{
				errors.Add(new MinificationErrorInfo(CoreStrings.ErrorMessage_UnknownError, 0, 0, string.Empty));
			}

			return new CodeMinificationResult(newContent, errors);
		}

		#endregion
	}
}