#if ASPNETCORE1 || ASPNETCORE2
using Microsoft.AspNetCore.Hosting;
#elif ASPNETCORE3
using Microsoft.Extensions.Hosting;
#else
#error No implementation for this target
#endif

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2
#elif ASPNETCORE3
namespace WebMarkupMin.AspNetCore3
#else
#error No implementation for this target
#endif
{
	/// <summary>
	/// WebMarkupMin options extensions
	/// </summary>
	public static class WebMarkupMinOptionsExtensions
	{
		/// <summary>
		/// Checks whether the markup minification is enabled
		/// </summary>
		/// <param name="options">WebMarkupMin options</param>
		/// <returns>Result of check (true - minification is enabled; false - minification is disabled)</returns>
		public static bool IsMinificationEnabled(this WebMarkupMinOptions options)
		{
			bool isMinificationEnabled = false;

			if (!options.DisableMinification)
			{
				isMinificationEnabled = !options.HostingEnvironment.IsDevelopment()
					|| options.AllowMinificationInDevelopmentEnvironment;
			}

			return isMinificationEnabled;
		}

		/// <summary>
		/// Checks whether the HTTP compression is enabled
		/// </summary>
		/// <param name="options">WebMarkupMin options</param>
		/// <returns>Result of check (true - compression is enabled; false - compression is disabled)</returns>
		public static bool IsCompressionEnabled(this WebMarkupMinOptions options)
		{
			bool isCompressionEnabled = false;

			if (!options.DisableCompression)
			{
				isCompressionEnabled = !options.HostingEnvironment.IsDevelopment()
					|| options.AllowCompressionInDevelopmentEnvironment;
			}

			return isCompressionEnabled;
		}
	}
}