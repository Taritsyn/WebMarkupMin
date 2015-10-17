using System.Collections.Generic;

using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

using WebMarkupMin.AspNet.Common.UrlMatchers;
using WebMarkupMin.AspNet5;
using WebMarkupMin.Core;
using WebMarkupMin.MsAjax;
using WebMarkupMin.Sample.Logic.Services;
using WebMarkupMin.Yui;

namespace WebMarkupMin.Sample.AspNet5.Mvc6
{
	public class Startup
	{
		public IConfigurationRoot Configuration
		{
			get;
			set;
		}


		public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
		{
			// Setup configuration sources.
			var builder = new ConfigurationBuilder()
				.SetBasePath(appEnv.ApplicationBasePath)
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables()
				;

			Configuration = builder.Build();
		}


		// This method gets called by the runtime.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddInstance(Configuration);

			// Add WebMarkupMin services to the services container.
			services.AddWebMarkupMin(options => {
					options.AllowMinificationInDevelopmentEnvironment = true;
					options.AllowCompressionInDevelopmentEnvironment = true;
				})
				.AddHtmlMinification(options =>
				{
					options.ExcludedPages = new List<IUrlMatcher>
					{
						new WildcardUrlMatcher("/minifiers/x*ml-minifier"),
						new ExactUrlMatcher("/contact")
					};

					HtmlMinificationSettings settings = options.MinificationSettings;
					settings.RemoveRedundantAttributes = true;
					settings.RemoveHttpProtocolFromAttributes = true;
					settings.RemoveHttpsProtocolFromAttributes = true;

					options.CssMinifierFactory = new MsAjaxCssMinifierFactory();
					options.JsMinifierFactory = new MsAjaxJsMinifierFactory();
				})
				.AddXhtmlMinification(options =>
				{
					options.IncludedPages = new List<IUrlMatcher>
					{
						new WildcardUrlMatcher("/minifiers/x*ml-minifier"),
						new ExactUrlMatcher("/contact")
					};

					XhtmlMinificationSettings settings = options.MinificationSettings;
					settings.RemoveRedundantAttributes = true;
					settings.RemoveHttpProtocolFromAttributes = true;
					settings.RemoveHttpsProtocolFromAttributes = true;

					options.CssMinifierFactory = new YuiCssMinifierFactory();
					options.JsMinifierFactory = new YuiJsMinifierFactory();
				})
				.AddXmlMinification(options => {
					XmlMinificationSettings settings = options.MinificationSettings;
					settings.CollapseTagsWithoutContent = true;
				})
				.AddHttpCompression()
				;

			// Add MVC services to the services container.
			services.AddMvc();

			// Add WebMarkupMin sample services to the services container.
			services.AddSingleton<SitemapService>();
			services.AddSingleton<CssMinifierFactory>();
			services.AddSingleton<JsMinifierFactory>();
			services.AddSingleton<HtmlMinificationService>();
			services.AddSingleton<XhtmlMinificationService>();
			services.AddSingleton<XmlMinificationService>();
		}

		// Configure is called after ConfigureServices is called.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.MinimumLevel = LogLevel.Information;
			loggerFactory.AddConsole();
			loggerFactory.AddDebug();

			// Configure the HTTP request pipeline.

			// Add the platform handler to the request pipeline.
			app.UseIISPlatformHandler();

			// Add the following to the request pipeline only in development environment.
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// Add Error handling middleware which catches all application specific errors and
				// send the request to the following path or controller action.
				app.UseExceptionHandler("/Home/Error");
			}

			// Add static files to the request pipeline.
			app.UseStaticFiles();

			app.UseWebMarkupMin();

			// Add MVC to the request pipeline.
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");

				// Uncomment the following line to add a route for porting Web API 2 controllers.
				// routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
			});
		}
	}
}