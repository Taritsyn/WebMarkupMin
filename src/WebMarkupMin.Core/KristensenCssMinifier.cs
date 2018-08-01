/* This minifier based on the code of Efficient stylesheet minifier
 * (https://madskristensen.net/blog/Efficient-stylesheet-minification-in-C)
 */

/* Feb 28, 2010
 *
 * Copyright (c) 2010 Mads Kristensen (http://madskristensen.net)
 */

using System.Text;
using System.Text.RegularExpressions;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// Minifier, which produces minifiction of CSS-code
	/// by using Mads Kristensen's CSS minifier
	/// </summary>
	public sealed class KristensenCssMinifier : ICssMinifier
	{
		private static readonly Regex _redundantIdSelectorRegex = new Regex(@"[a-zA-Z]+#");
		private static readonly Regex _lineBreakRegex = new Regex(@"[\n\r]+\s*");
		private static readonly Regex _multipleSpacesRegex = new Regex(@"\s+");
		private static readonly Regex _separatingCharacters = new Regex(@"\s?([:,;{}])\s?");
		private static readonly Regex _zeroValue = new Regex(@"([\s:]0)(px|%|em|in|cm|mm|ex|pt|pc|ch|rem|vh|vm|vmin|vmax|vw)");
		private static readonly Regex _commentRegex = new Regex(@"/\*[\s\S]*?\*/");

		/// <summary>
		/// Gets a value indicating the minifier supports inline code minification
		/// </summary>
		public bool IsInlineCodeMinificationSupported
		{
			get { return true; }
		}


		/// <summary>
		/// Produces code minifiction of CSS content by using
		/// Mads Kristensen's CSS minifier
		/// </summary>
		/// <param name="content">CSS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <returns>Minification result</returns>
		public CodeMinificationResult Minify(string content, bool isInlineCode)
		{
			return Minify(content, isInlineCode, Encoding.GetEncoding(0));
		}

		/// <summary>
		/// Produces code minifiction of CSS content by using
		/// Mads Kristensen's CSS minifier
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

			string newContent = content;
			if (!isInlineCode)
			{
				newContent = _redundantIdSelectorRegex.Replace(newContent, "#");
				newContent = _lineBreakRegex.Replace(newContent, string.Empty);
			}
			newContent = _multipleSpacesRegex.Replace(newContent, " ");
			newContent = _separatingCharacters.Replace(newContent, "$1");
			if (!isInlineCode)
			{
				newContent = newContent.Replace(";}", "}");
			}
			newContent = _zeroValue.Replace(newContent, "$1");

			// Remove comments from CSS
			newContent = _commentRegex.Replace(newContent, string.Empty);

			return new CodeMinificationResult(newContent);
		}
	}
}