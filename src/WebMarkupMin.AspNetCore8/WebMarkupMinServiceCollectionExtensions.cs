using System;

using Microsoft.Extensions.DependencyInjection;

using OriginalOptions = WebMarkupMin.AspNetCoreLatest.WebMarkupMinOptions;
using OriginalServicesBuilder = WebMarkupMin.AspNetCoreLatest.WebMarkupMinServicesBuilder;
using OriginalServiceCollectionExtensions = WebMarkupMin.AspNetCoreLatest.WebMarkupMinServiceCollectionExtensions;

namespace WebMarkupMin.AspNetCore8
{
	public static class WebMarkupMinServiceCollectionExtensions
	{
		/// <summary>
		/// Configures a set of <see cref="OriginalOptions"/> for the application
		/// </summary>
		/// <param name="services">The services available in the application</param>
		/// <param name="configure">The <see cref="OriginalOptions"/> which need to be configured</param>
		[Obsolete("Use a WebMarkupMin.AspNetCoreLatest package")]
		public static IServiceCollection ConfigureWebMarkupMin(this IServiceCollection services,
			Action<OriginalOptions> configure)
		{
			return OriginalServiceCollectionExtensions.ConfigureWebMarkupMin(services, configure);
		}

		/// <summary>
		/// Adds a services required by WebMarkupMin to <see cref="IServiceCollection"/>
		/// </summary>
		/// <param name="services">The services available in the application</param>
		/// <returns>A builder that allows further WebMarkupMin specific setup of <see cref="IServiceCollection"/></returns>
		[Obsolete("Use a WebMarkupMin.AspNetCoreLatest package")]
		public static OriginalServicesBuilder AddWebMarkupMin(this IServiceCollection services)
		{
			return OriginalServiceCollectionExtensions.AddWebMarkupMin(services);
		}

		/// <summary>
		/// Adds a services required by WebMarkupMin to <see cref="IServiceCollection"/>
		/// </summary>
		/// <param name="services">The services available in the application</param>
		/// <param name="configure">The <see cref="OriginalOptions"/> which need to be configured</param>
		/// <returns>A builder that allows further WebMarkupMin specific setup of <see cref="IServiceCollection"/></returns>
		[Obsolete("Use a WebMarkupMin.AspNetCoreLatest package")]
		public static OriginalServicesBuilder AddWebMarkupMin(this IServiceCollection services,
			Action<OriginalOptions> configure)
		{
			return OriginalServiceCollectionExtensions.AddWebMarkupMin(services, configure);
		}
	}
}