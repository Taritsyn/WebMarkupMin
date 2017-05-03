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
		/// Checks whether the HTTP method is supported
		/// </summary>
		/// <param name="compressionManager">HTTP compression manager</param>
		/// <param name="method">HTTP method</param>
		/// <returns>Result of check (true - supported; false - not supported)</returns>
		public static bool IsSupportedHttpMethod(this IHttpCompressionManager compressionManager,
			string method)
		{
			return compressionManager.SupportedHttpMethods.Contains(method);
		}

		/// <summary>
		/// Checks whether the media-type is supported
		/// </summary>
		/// <param name="compressionManager">HTTP compression manager</param>
		/// <param name="mediaType">Media-type</param>
		/// <returns>Result of check (true - supported; false - not supported)</returns>
		public static bool IsSupportedMediaType(this IHttpCompressionManager compressionManager, string mediaType)
		{
			if (string.IsNullOrWhiteSpace(mediaType))
			{
				return false;
			}

			Func<string, bool> supportedMediaTypePredicate = compressionManager.SupportedMediaTypePredicate;
			if (supportedMediaTypePredicate != null)
			{
				return supportedMediaTypePredicate(mediaType);
			}

			return MediaTypeHelpers.IsTextBasedMediaType(mediaType);
		}
	}
}