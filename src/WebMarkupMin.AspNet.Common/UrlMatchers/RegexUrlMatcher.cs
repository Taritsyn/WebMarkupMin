using System;
using System.Text.RegularExpressions;

using CoreStrings = WebMarkupMin.Core.Resources.Strings;

namespace WebMarkupMin.AspNet.Common.UrlMatchers
{
	/// <summary>
	/// Regular expression URL matcher
	/// </summary>
	public sealed class RegexUrlMatcher : IUrlMatcher
	{
		/// <summary>
		/// Regular expression
		/// </summary>
		private readonly Regex _regex;


		/// <summary>
		/// Constructs a instance of regular expression URL matcher
		/// </summary>
		/// <param name="pattern">Regular expression pattern (ECMAScript standard compliant)</param>
		/// <param name="caseSensitive">Flag for whether pattern matching for the condition
		/// should be case-sensitive</param>
		public RegexUrlMatcher(string pattern, bool caseSensitive = false)
		{
			if (pattern == null)
			{
				throw new ArgumentNullException("pattern");
			}

			if (string.IsNullOrWhiteSpace(pattern))
			{
				throw new ArgumentException(CoreStrings.Common_ValueIsEmpty, "pattern");
			}

			RegexOptions options = RegexOptions.ECMAScript;
			if (!caseSensitive)
			{
				options = options | RegexOptions.IgnoreCase;
			}

			_regex = new Regex(pattern, options);
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