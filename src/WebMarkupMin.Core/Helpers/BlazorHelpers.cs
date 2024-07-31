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
		const string COMPONENT_MARKER_PREFIX = MARKER_PREFIX_COMMON_PART + ":";

		/// <summary>
		/// Blazor server persisted state marker prefix
		/// </summary>
		const string SERVER_STATE_MARKER_PREFIX = MARKER_PREFIX_COMMON_PART + "-Server-Component-State:";

		/// <summary>
		/// Blazor WebAssembly persisted state marker prefix
		/// </summary>
		const string WEBASSEMBLY_STATE_MARKER_PREFIX = MARKER_PREFIX_COMMON_PART + "-WebAssembly-Component-State:";

		/// <summary>
		/// Blazor web initializer marker prefix
		/// </summary>
		const string WEB_INITIALIZER_MARKER_PREFIX = MARKER_PREFIX_COMMON_PART + "-Web-Initializers:";

		/// <summary>
		/// Blazor streaming boundary marker prefix common part
		/// </summary>
		const string STREAMING_BOUNDARY_MARKER_PREFIX_COMMON_PART = "bl:";

		/// <summary>
		/// Blazor start streaming boundary marker prefix
		/// </summary>
		const string START_STREAMING_BOUNDARY_MARKER_PREFIX = STREAMING_BOUNDARY_MARKER_PREFIX_COMMON_PART;

		/// <summary>
		/// Blazor end streaming boundary marker prefix
		/// </summary>
		const string END_STREAMING_BOUNDARY_MARKER_PREFIX = "/" + STREAMING_BOUNDARY_MARKER_PREFIX_COMMON_PART;

		/// <summary>
		/// List of Blazor ordinary marker prefixes
		/// </summary>
		private static readonly string[] _ordinaryMarkerPrefixes = new string[]
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
		/// Regular expression for working with the Globally Unique Identifier (GUID)
		/// </summary>
		private static readonly Regex _guidRegex = new Regex(
			@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$",
			TargetFrameworkShortcuts.PerformanceRegexOptions);


		/// <summary>
		/// Checks whether the comment is the Blazor marker
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <returns>Result of check (<c>true</c> - is marker;
		/// <c>false</c> - is not marker)</returns>
		public static bool IsMarker(string commentText)
		{
			return IsOrdinaryMarker(commentText)
				|| IsStartStreamingBoundaryMarker(commentText)
				|| IsEndStreamingBoundaryMarker(commentText)
				|| IsStreamingFramingMarker(commentText)
				;
		}

		/// <summary>
		/// Checks whether the comment is the Blazor ordinary marker
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <returns>Result of check (<c>true</c> - is ordinary marker;
		/// <c>false</c> - is not ordinary marker)</returns>
		private static bool IsOrdinaryMarker(string commentText)
		{
			int commentTextLength = commentText.Length;
			if (commentTextLength == 0)
			{
				return false;
			}

			int firstNonWhitespaceCharPosition = commentText.IndexOfNonWhitespace();
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

			foreach (string prefix in _ordinaryMarkerPrefixes)
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

		/// <summary>
		/// Checks whether the comment is the Blazor start streaming boundary marker
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <returns>Result of check (<c>true</c> - is start streaming boundary marker;
		/// <c>false</c> - is not start streaming boundary marker)</returns>
		private static bool IsStartStreamingBoundaryMarker(string commentText)
		{
			return commentText.StartsWith(START_STREAMING_BOUNDARY_MARKER_PREFIX, StringComparison.Ordinal)
				&& commentText.Length > START_STREAMING_BOUNDARY_MARKER_PREFIX.Length
				;
		}

		/// <summary>
		/// Checks whether the comment is the Blazor end streaming boundary marker
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <returns>Result of check (<c>true</c> - is end streaming boundary marker;
		/// <c>false</c> - is not end streaming boundary marker)</returns>
		private static bool IsEndStreamingBoundaryMarker(string commentText)
		{
			return commentText.StartsWith(END_STREAMING_BOUNDARY_MARKER_PREFIX, StringComparison.Ordinal)
				&& commentText.Length > END_STREAMING_BOUNDARY_MARKER_PREFIX.Length
				;
		}

		/// <summary>
		/// Checks whether the comment is the Blazor streaming framing marker
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <returns>Result of check (<c>true</c> - is streaming framing marker;
		/// <c>false</c> - is not streaming framing marker)</returns>
		private static bool IsStreamingFramingMarker(string commentText)
		{
			return _guidRegex.IsMatch(commentText);
		}
	}
}