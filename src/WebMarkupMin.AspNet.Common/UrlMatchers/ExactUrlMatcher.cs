using System;

using CoreStrings = WebMarkupMin.Core.Resources.Strings;

namespace WebMarkupMin.AspNet.Common.UrlMatchers
{
	/// <summary>
	/// Exact URL matcher
	/// </summary>
	public sealed class ExactUrlMatcher : IUrlMatcher
	{
		/// <summary>
		/// Exemplary URL
		/// </summary>
		private readonly string _exemplaryUrl;

		/// <summary>
		/// String comparison type
		/// </summary>
		private readonly StringComparison _comparisonType;


		/// <summary>
		/// Constructs a instance of exact URL matcher
		/// </summary>
		/// <param name="pattern">Exemplary URL</param>
		/// <param name="caseSensitive">Flag for whether pattern matching for the condition
		/// should be case-sensitive</param>
		public ExactUrlMatcher(string pattern, bool caseSensitive = false)
		{
			if (pattern == null)
			{
				throw new ArgumentNullException(nameof(pattern));
			}

			if (string.IsNullOrWhiteSpace(pattern))
			{
				throw new ArgumentException(CoreStrings.Common_ValueIsEmpty, nameof(pattern));
			}

			_exemplaryUrl = pattern.TrimEnd('/');
			_comparisonType = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
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
				throw new ArgumentNullException(nameof(url));
			}

			if (string.IsNullOrWhiteSpace(url))
			{
				throw new ArgumentException(CoreStrings.Common_ValueIsEmpty, nameof(url));
			}

			string processedUrl = url.TrimEnd('/');
			bool result = string.Equals(processedUrl, _exemplaryUrl, _comparisonType);

			return result;
		}
	}
}