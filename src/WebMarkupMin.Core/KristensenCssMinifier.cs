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
		private static readonly Regex _separatingCharsInInlineCodeRegex = new Regex(@" ?([:,;]) ?",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _separatingCharsInEmbeddedCodeRegex = new Regex(@" ?([,;{}]) ?",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex declarationSeparatingCharInEmbeddedCodeRegex = new Regex(
			@"(?<=[{;][a-zA-Z-_][a-zA-Z0-9-_]*) ?: ?(?=[^{]+?[;}])",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _redundantSelectorRegex = new Regex(@"(?<=[,;}]|^)[a-zA-Z][a-zA-Z0-9]*#",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _zeroValueRegex = new Regex(
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


		private static string RemoveComments(string content)
		{
			string processedContent = _commentRegex.Replace(content, string.Empty);

			return processedContent;
		}

		private static string MinifyWhitespace(string content, bool isInlineCode)
		{
			if (string.IsNullOrWhiteSpace(content))
			{
				return string.Empty;
			}

			string processedContent = content;
			processedContent = processedContent.CollapseWhitespace();
			if (isInlineCode)
			{
				processedContent = _separatingCharsInInlineCodeRegex.Replace(processedContent, "$1");
			}
			else
			{
				processedContent = _separatingCharsInEmbeddedCodeRegex.Replace(processedContent, "$1");

				// The colon in a declaration (`property : value`) serves as a separator between the property name
				// (preceded by a opening curly brace or semicolon) and its value (followed by a semicolon or closing
				// curly brace).
				// In a declaration, the spaces before and after the colon are insignificant and can be removed.
				// A space before a colon that starts a pseudo-class or pseudo-element (e.g. `a :where(b)`,
				// `.x :not(.y)`, `p :first-child`, `div ::before`) is a descendant combinator and must be preserved.
				processedContent = declarationSeparatingCharInEmbeddedCodeRegex.Replace(processedContent, ":");

				// The space after any colon (e.g. `@media (min-width: 100px)`) is insignificant and can be removed
				processedContent = processedContent.Replace(": ", ":");
			}
			processedContent = processedContent.Trim(_space);

			return processedContent;
		}

		private static string RemoveUnitsFromZeroValues(string content)
		{
			if (content.IndexOf('0') == -1)
			{
				return content;
			}

			string processedContent = _zeroValueRegex.Replace(content, "0");

			return processedContent;
		}

		private static string RemoveRedundantSelectors(string content)
		{
			if (content.IndexOf('#') == -1)
			{
				return content;
			}

			string processedContent = _redundantSelectorRegex.Replace(content, "#");

			return processedContent;
		}

		private static string RemoveTrailingSemicolons(string content, bool isInlineCode)
		{
			string processedContent = isInlineCode ?
				content.TrimEnd(_semicolon)
				:
				content.Replace(";}", "}")
				;

			return processedContent;
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

			string processedContent = content;
			processedContent = RemoveComments(processedContent);
			processedContent = MinifyWhitespace(processedContent, isInlineCode);
			if (processedContent.Length > 0)
			{
				if (_settings.RemoveUnitsFromZeroValues)
				{
					processedContent = RemoveUnitsFromZeroValues(processedContent);
				}
				if (_settings.RemoveRedundantSelectors && !isInlineCode)
				{
					processedContent = RemoveRedundantSelectors(processedContent);
				}
				if (_settings.RemoveTrailingSemicolons)
				{
					processedContent = RemoveTrailingSemicolons(processedContent, isInlineCode);
				}
			}

			return new CodeMinificationResult(processedContent);
		}

		#endregion
	}
}