using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.OptionsModel;

using WebMarkupMin.AspNet5.Internal;
using WebMarkupMin.Core;
using WebMarkupMin.Core.Loggers;

namespace WebMarkupMin.AspNet5
{
	public static class WebMarkupMinServiceCollectionExtensions
	{
		/// <summary>
		/// Configures a set of <see cref="WebMarkupMinOptions"/> for the application
		/// </summary>
		/// <param name="services">The services available in the application</param>
		/// <param name="configure">The <see cref="WebMarkupMinOptions"/> which need to be configured</param>
		public static IServiceCollection ConfigureWebMarkupMin(
			[NotNull] this IServiceCollection services,
			[NotNull] Action<WebMarkupMinOptions> configure)
		{
			return services.Configure(configure);
		}

		/// <summary>
		/// Adds a services required by WebMarkupMin to <see cref="IServiceCollection"/>
		/// </summary>
		/// <param name="services">The services available in the application</param>
		/// <returns>A builder that allows further WebMarkupMin specific setup of <see cref="IServiceCollection"/></returns>
		public static WebMarkupMinServicesBuilder AddWebMarkupMin(
			[NotNull] this IServiceCollection services)
		{
			return services.AddWebMarkupMin(configure: null);
		}

		/// <summary>
		/// Adds a services required by WebMarkupMin to <see cref="IServiceCollection"/>
		/// </summary>
		/// <param name="services">The services available in the application</param>
		/// <param name="configure">The <see cref="WebMarkupMinOptions"/> which need to be configured</param>
		/// <returns>A builder that allows further WebMarkupMin specific setup of <see cref="IServiceCollection"/></returns>
		public static WebMarkupMinServicesBuilder AddWebMarkupMin(
			[NotNull] this IServiceCollection services,
			Action<WebMarkupMinOptions> configure)
		{
			services.AddSingleton<IConfigureOptions<WebMarkupMinOptions>, WebMarkupMinOptionsSetup>();

			if (configure != null)
			{
				services.Configure(configure);
			}

			services.AddSingleton<ILogger, ThrowExceptionLogger>();
			services.AddSingleton<ICssMinifierFactory, KristensenCssMinifierFactory>();
			services.AddSingleton<IJsMinifierFactory, CrockfordJsMinifierFactory>();

			// We use the `WebMarkupMinMarkerService` to make sure if all the services were added
			services.AddTransient<WebMarkupMinMarkerService, WebMarkupMinMarkerService>();

			return new WebMarkupMinServicesBuilder(services);
		}
	}
}