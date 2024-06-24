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
		/// Blazor marker prefix common part
		/// </summary>
		const string MARKER_PREFIX_COMMON_PART = "Blazor";

		/// <summary>
		/// Blazor component marker prefix
		/// </summary>
		const string COMPONENT_MARKER_PREFIX = "Blazor:";

		/// <summary>
		/// Blazor server persisted state marker prefix
		/// </summary>
		const string SERVER_STATE_MARKER_PREFIX = "Blazor-Server-Component-State:";

		/// <summary>
		/// Blazor WebAssembly persisted state marker prefix
		/// </summary>
		const string WEBASSEMBLY_STATE_MARKER_PREFIX = "Blazor-WebAssembly-Component-State:";

		/// <summary>
		/// Blazor web initializer marker prefix
		/// </summary>
		const string WEB_INITIALIZER_MARKER_PREFIX = "Blazor-Web-Initializers:";

		/// <summary>
		/// List of Blazor marker prefixes
		/// </summary>
		private static readonly string[] _markerPrefixes = new string[]
		{
			COMPONENT_MARKER_PREFIX,
			SERVER_STATE_MARKER_PREFIX,
			WEBASSEMBLY_STATE_MARKER_PREFIX,
			WEB_INITIALIZER_MARKER_PREFIX
		};

		/// <summary>
		/// Regular expression for working with the Blazor component data
		/// </summary>
		private static readonly Regex _componentDataRegex = new Regex(@"[^{]*.*$",
			TargetFrameworkShortcuts.PerformanceRegexOptions);

		/// <summary>
		/// Regular expression for working with the Base64 encoded data
		/// </summary>
		private static readonly Regex _base64EncodedDataRegex = new Regex(@"[a-zA-Z0-9+/=]+$",
			TargetFrameworkShortcuts.PerformanceRegexOptions);


		/// <summary>
		/// Checks whether the comment is the Blazor marker
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <returns>Result of check (<c>true</c> - is marker;
		/// <c>false</c> - is not marker)</returns>
		public static bool IsMarker(string commentText)
		{
			int commentTextLength = commentText.Length;
			if (commentTextLength == 0)
			{
				return false;
			}

			int firstNonWhitespaceCharPosition = SourceCodeNavigator.FindNextNonWhitespaceChar(commentText,
				0, commentTextLength);
			if (firstNonWhitespaceCharPosition == -1)
			{
				return false;
			}

			int markerPrefixCommonPartPosition = commentText.IndexOf(MARKER_PREFIX_COMMON_PART,
				firstNonWhitespaceCharPosition, StringComparison.Ordinal);
			int colonPosition = commentText.IndexOf(':', firstNonWhitespaceCharPosition);

			if (markerPrefixCommonPartPosition == -1 || colonPosition == -1)
			{
				return false;
			}

			foreach (string prefix in _markerPrefixes)
			{
				if (commentText.CustomStartsWith(prefix, firstNonWhitespaceCharPosition, StringComparison.Ordinal))
				{
					int dataPosition = firstNonWhitespaceCharPosition + prefix.Length;

					if (dataPosition < commentTextLength)
					{
						bool isCorrectData;

						if (prefix == COMPONENT_MARKER_PREFIX)
						{
							isCorrectData = _componentDataRegex.IsMatch(commentText, dataPosition);
						}
						else
						{
							isCorrectData = _base64EncodedDataRegex.IsMatch(commentText, dataPosition);
						}

						return isCorrectData;
					}
					else
					{
						break;
					}
				}
			}

			return false;
		}
	}
}