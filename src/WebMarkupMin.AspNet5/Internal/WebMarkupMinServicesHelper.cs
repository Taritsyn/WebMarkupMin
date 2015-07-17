using System;

using WebMarkupMin.AspNet5.Resources;

namespace WebMarkupMin.AspNet5.Internal
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
					Strings.UnableToFindServices,
					"IServiceCollection.AddWebMarkupMin()",
					"IApplicationBuilder.ConfigureServices(...)",
					"IApplicationBuilder.UseWebMarkupMin(...)"
				));
			}
		}
	}
}