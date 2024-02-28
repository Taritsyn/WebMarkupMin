using System;
using System.Collections.Generic;

namespace WebMarkupMin.AspNet.Common.Helpers
{
	/// <summary>
	/// Media-type helpers
	/// </summary>
	public static class MediaTypeHelpers
	{
		/// <summary>
		/// Set of remaining text-based media-types
		/// </summary>
		private static readonly HashSet<string> _remainingTextBasedMediaTypes = new HashSet<string>
		{
			// JavaScript-based media-types
			"application/javascript",
			"application/x-javascript",
			"application/ecmascript",

			// JSON-based media-types
			"application/json",

			// XML-based media-types
			"application/xml",
			"application/xml-dtd"
		};


		/// <summary>
		/// Checks whether the media-type is text-based
		/// </summary>
		/// <param name="mediaType">Media-type</param>
		/// <returns>Result of check (<c>true</c> - is text-based; <c>false</c> - is not text-based)</returns>
		public static bool IsTextBasedMediaType(string mediaType)
		{
			if (string.IsNullOrWhiteSpace(mediaType))
			{
				return false;
			}

			if (mediaType.StartsWith("text/", StringComparison.Ordinal)
				|| mediaType.EndsWith("+json", StringComparison.Ordinal)
				|| mediaType.EndsWith("+xml", StringComparison.Ordinal))
			{
				return true;
			}

			bool result = _remainingTextBasedMediaTypes.Contains(mediaType);

			return result;
		}
	}
}