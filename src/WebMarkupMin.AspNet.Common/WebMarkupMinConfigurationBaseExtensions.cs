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
		/// <returns>Result of check (true - size is allowable; false - size is not allowable)</returns>
		public static bool IsAllowableResponseSize(this WebMarkupMinConfigurationBase configuration, long responseSize)
		{
			bool isAllowableResponseSize = (configuration.MaxResponseSize == -1
				|| responseSize <= configuration.MaxResponseSize);

			return isAllowableResponseSize;
		}

		/// <summary>
		/// Checks whether the adding of <code>*-Minification-Powered-By</code> HTTP headers in the response is enabled
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <returns>Result of check (true - is enabled; false - is disabled)</returns>
		public static bool IsPoweredByHttpHeadersEnabled(this WebMarkupMinConfigurationBase configuration)
		{
			bool isPoweredByHttpHeadersEnabled = !configuration.DisablePoweredByHttpHeaders;

			return isPoweredByHttpHeadersEnabled;
		}
	}
}