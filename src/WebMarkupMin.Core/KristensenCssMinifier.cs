/* This minifier based on the code of Efficient stylesheet minifier
 * (https://madskristensen.net/blog/Efficient-stylesheet-minification-in-C)
 */

/* Feb 28, 2010
 *
 * Copyright (c) 2010 Mads Kristensen (http://madskristensen.net)
 */

using System.Text;
using System.Text.RegularExpressions;

using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// Minifier, which produces minifiction of CSS-code
	/// by using Mads Kristensen's CSS minifier
	/// </summary>
	public sealed class KristensenCssMinifier : ICssMinifier
	{
		private static readonly char[] _space = new char[] { ' ' };
		private static readonly char[] _semicolon = new char[] { ';' };

		private static readonly Regex _commentRegex = new Regex(@"/\*[\s\S]*?\*/",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _separatingChars = new Regex(@" ?([:,;{}]) ?",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _redundantSelectorRegex = new Regex(@"[a-zA-Z]+#",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _zeroValue = new Regex(@"(?<=[ :])0(?:px|em|ex|cm|mm|in|pt|pc|%|ch|rem|vh|vm(?:ax|in)?|vw)",
			TargetFrameworkShortcuts.PerformanceRegexOptions);

		#region ICssMinifier implementation

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
			return Minify(content, isInlineCode, TargetFrameworkShortcuts.DefaultTextEncoding);
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

			// Remove comments
			newContent = _commentRegex.Replace(newContent, string.Empty);

			// Minify whitespace
			newContent = newContent.CollapseWhitespace();
			newContent = _separatingChars.Replace(newContent, "$1");
			newContent = newContent.Trim(_space);

			// Removing last semicolons
			if (isInlineCode)
			{
				newContent = newContent.TrimEnd(_semicolon);
			}
			else
			{
				newContent = newContent.Replace(";}", "}");
			}

			if (!isInlineCode)
			{
				// Remove redundant selectors
				newContent = _redundantSelectorRegex.Replace(newContent, "#");
			}

			// Remove units from zero values
			newContent = _zeroValue.Replace(newContent, "0");

			return new CodeMinificationResult(newContent);
		}

		#endregion
	}
}