using System;

using Microsoft.AspNetCore.Builder;

#if ASPNETCORE1
using WebMarkupMin.AspNetCore1.Internal;

namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
using WebMarkupMin.AspNetCore2.Internal;

namespace WebMarkupMin.AspNetCore2
#else
#error No implementation for this target
#endif
{
	/// <summary>
	/// Extension methods for <see cref="IApplicationBuilder"/> to add
	/// WebMarkupMin optimization features to the request execution pipeline
	/// </summary>
	public static class BuilderExtensions
	{
		/// <summary>
		/// Adds a WebMarkupMin optimization features to the <see cref="IApplicationBuilder"/> request execution pipeline
		/// </summary>
		/// <param name="app">The <see cref="IApplicationBuilder"/></param>
		/// <returns>The <paramref name="app"/></returns>
		public static IApplicationBuilder UseWebMarkupMin(this IApplicationBuilder app)
		{
			if (app == null)
			{
				throw new ArgumentNullException(nameof(app));
			}

			// Verify if `AddWebMarkupMin` was done before calling `UseWebMarkupMin`.
			// We use the `WebMarkupMinMarkerService` to make sure if all the services were added.
			IServiceProvider services = app.ApplicationServices;
			WebMarkupMinServicesHelper.ThrowIfWebMarkupMinNotRegistered(services);

			return app.UseMiddleware<WebMarkupMinMiddleware>();
		}
	}
}