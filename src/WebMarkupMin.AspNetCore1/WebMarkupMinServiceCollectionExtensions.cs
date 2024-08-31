using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

#if ASPNETCORE1
using WebMarkupMin.AspNetCore1.Internal;
#elif ASPNETCORE2
using WebMarkupMin.AspNetCore2.Internal;
#elif ASPNETCORE3
using WebMarkupMin.AspNetCore3.Internal;
#elif ASPNETCORE5
using WebMarkupMin.AspNetCore5.Internal;
#elif ASPNETCORE6
using WebMarkupMin.AspNetCore6.Internal;
#elif ASPNETCORE7
using WebMarkupMin.AspNetCore7.Internal;
#elif ASPNETCORE8
using WebMarkupMin.AspNetCore8.Internal;
#else
#error No implementation for this target
#endif
using WebMarkupMin.Core;
using WebMarkupMin.Core.Loggers;

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2
#elif ASPNETCORE3
namespace WebMarkupMin.AspNetCore3
#elif ASPNETCORE5
namespace WebMarkupMin.AspNetCore5
#elif ASPNETCORE6
namespace WebMarkupMin.AspNetCore6
#elif ASPNETCORE7
namespace WebMarkupMin.AspNetCore7
#elif ASPNETCORE8
namespace WebMarkupMin.AspNetCore8
#else
#error No implementation for this target
#endif
{
	public static class WebMarkupMinServiceCollectionExtensions
	{
		/// <summary>
		/// Configures a set of <see cref="WebMarkupMinOptions"/> for the application
		/// </summary>
		/// <param name="services">The services available in the application</param>
		/// <param name="configure">The <see cref="WebMarkupMinOptions"/> which need to be configured</param>
		public static IServiceCollection ConfigureWebMarkupMin(this IServiceCollection services,
			Action<WebMarkupMinOptions> configure)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			if (configure == null)
			{
				throw new ArgumentNullException(nameof(configure));
			}

			return services.Configure(configure);
		}

		/// <summary>
		/// Adds a services required by WebMarkupMin to <see cref="IServiceCollection"/>
		/// </summary>
		/// <param name="services">The services available in the application</param>
		/// <returns>A builder that allows further WebMarkupMin specific setup of <see cref="IServiceCollection"/></returns>
		public static WebMarkupMinServicesBuilder AddWebMarkupMin(this IServiceCollection services)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			return services.AddWebMarkupMin(configure: null);
		}

		/// <summary>
		/// Adds a services required by WebMarkupMin to <see cref="IServiceCollection"/>
		/// </summary>
		/// <param name="services">The services available in the application</param>
		/// <param name="configure">The <see cref="WebMarkupMinOptions"/> which need to be configured</param>
		/// <returns>A builder that allows further WebMarkupMin specific setup of <see cref="IServiceCollection"/></returns>
		public static WebMarkupMinServicesBuilder AddWebMarkupMin(this IServiceCollection services,
			Action<WebMarkupMinOptions> configure)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			services.AddSingleton<IConfigureOptions<WebMarkupMinOptions>, WebMarkupMinOptionsSetup>();

			if (configure != null)
			{
				services.Configure(configure);
			}

			services.TryAddSingleton<ILogger, NullLogger>();
			services.TryAddSingleton<ICssMinifierFactory, KristensenCssMinifierFactory>();
			services.TryAddSingleton<IJsMinifierFactory, CrockfordJsMinifierFactory>();

			// We use the `WebMarkupMinMarkerService` to make sure if all the services were added
			services.TryAddSingleton<WebMarkupMinMarkerService, WebMarkupMinMarkerService>();

			return new WebMarkupMinServicesBuilder(services);
		}
	}
}