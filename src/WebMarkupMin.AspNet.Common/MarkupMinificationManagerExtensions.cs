using System;
using System.Collections.Generic;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Markup minification manager extensions
	/// </summary>
	public static class MarkupMinificationManagerExtensions
	{
		/// <summary>
		/// Checks whether the media-type is supported
		/// </summary>
		/// <param name="minificationManager">Markup minification manager</param>
		/// <param name="mediaType">Media-type</param>
		/// <returns>Result of check (<c>true</c> - supported; <c>false</c> - not supported)</returns>
		public static bool IsSupportedMediaType(this IMarkupMinificationManager minificationManager,
			string mediaType)
		{
			return minificationManager.SupportedMediaTypes.Contains(mediaType);
		}

		/// <summary>
		/// Appends a <c>*-Minification-Powered-By</c> HTTP-header
		/// </summary>
		/// <param name="minificationManager">Markup minification manager</param>
		/// <param name="append">HTTP header appending delegate</param>
		public static void AppendPoweredByHttpHeader(this IMarkupMinificationManager minificationManager,
			Action<string, string> append)
		{
			KeyValuePair<string, string> poweredByHttpHeader = minificationManager.PoweredByHttpHeader;

			append(poweredByHttpHeader.Key, poweredByHttpHeader.Value);
		}
	}
}