namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// WebMarkupMin configuration extensions
	/// </summary>
	public static class WebMarkupMinConfigurationBaseExtensions
	{
		/// <summary>
		/// Checks whether the response size is not exceeded the limit
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="responseSize">Response size in bytes</param>
		/// <returns>Result of check (<c>true</c> - size is allowable; <c>false</c> - size is not allowable)</returns>
		public static bool IsAllowableResponseSize(this WebMarkupMinConfigurationBase configuration, long responseSize)
		{
			bool isAllowableResponseSize = (configuration.MaxResponseSize == -1
				|| responseSize <= configuration.MaxResponseSize);

			return isAllowableResponseSize;
		}

		/// <summary>
		/// Checks whether the adding of <c>*-Minification-Powered-By</c> HTTP headers in the response is enabled
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <returns>Result of check (<c>true</c> - is enabled; <c>false</c> - is disabled)</returns>
		public static bool IsPoweredByHttpHeadersEnabled(this WebMarkupMinConfigurationBase configuration)
		{
			bool isPoweredByHttpHeadersEnabled = !configuration.DisablePoweredByHttpHeaders;

			return isPoweredByHttpHeadersEnabled;
		}
	}
}