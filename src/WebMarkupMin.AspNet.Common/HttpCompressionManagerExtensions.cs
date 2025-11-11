using System;

using WebMarkupMin.AspNet.Common.Helpers;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// HTTP compression manager extensions
	/// </summary>
	public static class HttpCompressionManagerExtensions
	{
		/// <summary>
		/// Checks whether the media-type is supported
		/// </summary>
		/// <param name="compressionManager">HTTP compression manager</param>
		/// <param name="mediaType">Media-type</param>
		/// <returns>Result of check (<c>true</c> - supported; <c>false</c> - not supported)</returns>
		public static bool IsSupportedMediaType(this IHttpCompressionManager compressionManager, string mediaType)
		{
			if (string.IsNullOrWhiteSpace(mediaType))
			{
				return false;
			}

			Func<string, bool> supportedMediaTypePredicate = compressionManager.SupportedMediaTypePredicate;
			if (supportedMediaTypePredicate is not null)
			{
				return supportedMediaTypePredicate(mediaType);
			}

			return MediaTypeHelpers.IsTextBasedMediaType(mediaType);
		}
	}
}