using System;

#if ASPNETCORE1
using AspNetCoreStrings = WebMarkupMin.AspNetCore1.Resources.Strings;

namespace WebMarkupMin.AspNetCore1.Internal
#elif ASPNETCORE2
using AspNetCoreStrings = WebMarkupMin.AspNetCore2.Resources.Strings;

namespace WebMarkupMin.AspNetCore2.Internal
#elif ASPNETCORE3
using AspNetCoreStrings = WebMarkupMin.AspNetCore3.Resources.Strings;

namespace WebMarkupMin.AspNetCore3.Internal
#else
#error No implementation for this target
#endif
{
	/// <summary>
	/// Helper class which contains WebMarkupMinServices related helpers
	/// </summary>
	internal static class WebMarkupMinServicesHelper
	{
		/// <summary>
		/// Throws <see cref="InvalidOperationException"/> when <see cref="WebMarkupMinMarkerService"/>
		/// is not present in the list of services
		/// </summary>
		/// <param name="services">The list of services</param>
		public static void ThrowIfWebMarkupMinNotRegistered(IServiceProvider services)
		{
			if (services.GetService(typeof(WebMarkupMinMarkerService)) == null)
			{
				throw new InvalidOperationException(string.Format(
					AspNetCoreStrings.UnableToFindServices,
					"IServiceCollection.AddWebMarkupMin()",
					"IApplicationBuilder.ConfigureServices(...)",
					"IApplicationBuilder.UseWebMarkupMin(...)"
				));
			}
		}
	}
}