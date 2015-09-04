using System;
using System.Text.RegularExpressions;

using CoreStrings = WebMarkupMin.Core.Resources.Strings;

namespace WebMarkupMin.AspNet.Common.UrlMatchers
{
	/// <summary>
	/// Wildcard URL matcher
	/// </summary>
	public sealed class WildcardUrlMatcher : IUrlMatcher
	{
		/// <summary>
		/// Regular expression
		/// </summary>
		private readonly Regex _regex;


		/// <summary>
		/// Constructs a instance of wildcard URL matcher
		/// </summary>
		/// <param name="pattern">Pattern with wildcards</param>
		/// <param name="caseSensitive">Flag for whether pattern matching for the condition
		/// should be case-sensitive</param>
		public WildcardUrlMatcher(string pattern, bool caseSensitive = false)
		{
			if (pattern == null)
			{
				throw new ArgumentNullException("pattern");
			}

			if (string.IsNullOrWhiteSpace(pattern))
			{
				throw new ArgumentException(CoreStrings.Common_ValueIsEmpty, "pattern");
			}

			string regexPattern = WildcardPatternToRegexPattern(pattern);
			RegexOptions options = RegexOptions.CultureInvariant | RegexOptions.Singleline;
			if (!caseSensitive)
			{
				options = options | RegexOptions.IgnoreCase;
			}

			_regex = new Regex(regexPattern, options);
		}


		/// <summary>
		/// Converts a pattern with wildcards to regular expression pattern
		/// </summary>
		/// <param name="pattern">Pattern with wildcards</param>
		/// <returns>Regular expression pattern</returns>
		private static string WildcardPatternToRegexPattern(string pattern)
		{
			return "^" + Regex.Escape(pattern)
				.Replace("\\*", ".*")
				.Replace("\\?", ".") + "$"
				;
		}


		/// <summary>
		/// Indicates whether the matching rule finds a match in the specified URL
		/// </summary>
		/// <param name="url">URL</param>
		/// <returns>true if the matching rule finds a match; otherwise, false.</returns>
		public bool IsMatch(string url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}

			if (string.IsNullOrWhiteSpace(url))
			{
				throw new ArgumentException(CoreStrings.Common_ValueIsEmpty, "url");
			}

			string processedUrl = url.TrimEnd('/');
			bool result = _regex.IsMatch(processedUrl);

			return result;
		}
	}
}