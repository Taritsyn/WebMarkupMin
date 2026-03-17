/* This minifier based on the code of Efficient stylesheet minifier
 * (https://www.madskristensen.net/blog/efficient-stylesheet-minification-in-c/)
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
		private static readonly Regex _redundantCompoundSelectorRegex = new Regex(@"(?<=[,;}]|^)[a-zA-Z][a-zA-Z0-9]*#",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _zeroValue = new Regex(
			@"(?<=[ :])0(?:px|pt|pc|cm|mm|in|em|ex|ch|rem|vw|vh|vm(?:in|ax))(?=[ ;}]|$)",
			TargetFrameworkShortcuts.PerformanceRegexOptions);

		/// <summary>
		/// Mads Kristensen's CSS Minifier settings
		/// </summary>
		private readonly KristensenCssMinificationSettings _settings;


		/// <summary>
		/// Constructs an instance of the Mads Kristensen's CSS Minifier
		/// </summary>
		public KristensenCssMinifier()
			: this(new KristensenCssMinificationSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the Mads Kristensen's CSS Minifier
		/// </summary>
		/// <param name="settings">Mads Kristensen's CSS Minifier settings</param>
		public KristensenCssMinifier(KristensenCssMinificationSettings settings)
		{
			_settings = settings;
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
			if (_settings.RemoveTrailingSemicolons)
			{
				newContent = isInlineCode ?
					newContent.TrimEnd(_semicolon)
					:
					newContent.Replace(";}", "}")
					;
			}

			// Remove redundant selectors
			if (_settings.RemoveRedundantSelectors && !isInlineCode)
			{
				newContent = _redundantCompoundSelectorRegex.Replace(newContent, "#");
			}

			// Remove units from zero values
			if (_settings.RemoveUnitsFromZeroValues)
			{
				newContent = _zeroValue.Replace(newContent, "0");
			}

			return new CodeMinificationResult(newContent);
		}

		#endregion
	}
}