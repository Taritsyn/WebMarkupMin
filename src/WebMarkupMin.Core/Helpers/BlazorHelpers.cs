using System;
using System.Text.RegularExpressions;

using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Helpers
{
	/// <summary>
	/// Blazor helpers
	/// </summary>
	internal static class BlazorHelpers
	{
		/// <summary>
		/// Blazor prefix
		/// </summary>
		const string BLAZOR_PREFIX = "Blazor";

		/// <summary>
		/// Base64 encoded data regular expression pattern
		/// </summary>
		const string BASE64_ENCODED_DATA_REGEX_PATTERN = "[a-zA-Z0-9+/=]+";

		/// <summary>
		/// Regular expression for working with the Blazor component marker
		/// </summary>
		private static readonly Regex _blazorComponentMarkerRegex = new Regex(
			@"^\s*" + BLAZOR_PREFIX + ":[^{]*.*$",
			TargetFrameworkShortcuts.PerformanceRegexOptions);

		/// <summary>
		/// Regular expression for working with the Blazor server persisted state marker
		/// </summary>
		private static readonly Regex _blazorServerStateMarkerRegex = new Regex(
			@"^\s*" + BLAZOR_PREFIX + "-Server-Component-State:" + BASE64_ENCODED_DATA_REGEX_PATTERN + "$",
			TargetFrameworkShortcuts.PerformanceRegexOptions);

		/// <summary>
		/// Regular expression for working with the Blazor WebAssembly persisted state marker
		/// </summary>
		private static readonly Regex _blazorWebAssemblyStateMarkerRegex = new Regex(
			@"^\s*" + BLAZOR_PREFIX + "-WebAssembly-Component-State:" + BASE64_ENCODED_DATA_REGEX_PATTERN + "$",
			TargetFrameworkShortcuts.PerformanceRegexOptions);

		/// <summary>
		/// Regular expression for working with the Blazor web initializer marker
		/// </summary>
		private static readonly Regex _blazorWebInitializerMarkerRegex = new Regex(
			@"^\s*" + BLAZOR_PREFIX + "-Web-Initializers:" + BASE64_ENCODED_DATA_REGEX_PATTERN + "$",
			TargetFrameworkShortcuts.PerformanceRegexOptions);


		/// <summary>
		/// Checks whether the comment is the Blazor marker
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <returns>Result of check (<c>true</c> - is marker;
		/// <c>false</c> - is not marker)</returns>
		public static bool IsMarker(string commentText)
		{
			if (commentText.IndexOf(BLAZOR_PREFIX, StringComparison.Ordinal) == -1
				|| commentText.IndexOf(":", StringComparison.Ordinal) == -1)
			{
				return false;
			}

			return _blazorComponentMarkerRegex.IsMatch(commentText)
				|| _blazorServerStateMarkerRegex.IsMatch(commentText)
				|| _blazorWebAssemblyStateMarkerRegex.IsMatch(commentText)
				|| _blazorWebInitializerMarkerRegex.IsMatch(commentText)
				;
		}
	}
}