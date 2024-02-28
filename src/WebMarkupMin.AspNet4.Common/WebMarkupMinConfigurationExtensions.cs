﻿using System.Web;

namespace WebMarkupMin.AspNet4.Common
{
	/// <summary>
	/// WebMarkupMin configuration extensions
	/// </summary>
	public static class WebMarkupMinConfigurationExtensions
	{
		/// <summary>
		/// Checks whether the markup minification is enabled
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <returns>Result of check (<c>true</c> - minification is enabled; <c>false</c> - minification is disabled)</returns>
		public static bool IsMinificationEnabled(this WebMarkupMinConfiguration configuration)
		{
			bool isMinificationEnabled = false;

			if (!configuration.DisableMinification)
			{
				isMinificationEnabled = !HttpContext.Current.IsDebuggingEnabled
					|| configuration.AllowMinificationInDebugMode;
			}

			return isMinificationEnabled;
		}

		/// <summary>
		/// Checks whether the HTTP compression is enabled
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <returns>Result of check (<c>true</c> - compression is enabled; <c>false</c> - compression is disabled)</returns>
		public static bool IsCompressionEnabled(this WebMarkupMinConfiguration configuration)
		{
			bool isCompressionEnabled = false;

			if (!configuration.DisableCompression)
			{
				isCompressionEnabled = !HttpContext.Current.IsDebuggingEnabled
					|| configuration.AllowCompressionInDebugMode;
			}

			return isCompressionEnabled;
		}
	}
}