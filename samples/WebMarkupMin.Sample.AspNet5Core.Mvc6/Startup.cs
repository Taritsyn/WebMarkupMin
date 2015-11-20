using System.Collections.Generic;

using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using WebMarkupMin.AspNet.Common.UrlMatchers;
using WebMarkupMin.AspNet5;
using WebMarkupMin.Core;
using WebMarkupMin.Sample.Logic.Services;

namespace WebMarkupMin.Sample.AspNet5Core.Mvc6
{
	public class Startup
	{
		public IConfigurationRoot Configuration
		{
			get;
			set;
		}


		public Startup(IHostingEnvironment env)
		{
			// Set up configuration sources.
			var builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables()
				;

			Configuration = builder.Build();
		}


		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddInstance(Configuration);

			// Add WebMarkupMin services to the services container.
			services.AddWebMarkupMin(options =>
				{
					options.AllowMinificationInDevelopmentEnvironment = true;
					options.AllowCompressionInDevelopmentEnvironment = true;
				})
				.AddHtmlMinification(options =>
				{
					options.ExcludedPages = new List<IUrlMatcher>
					{
						new RegexUrlMatcher(@"^/minifiers/x(?:ht)?ml-minifier$"),
						new ExactUrlMatcher("/contact")
					};

					HtmlMinificationSettings settings = options.MinificationSettings;
					settings.RemoveRedundantAttributes = true;
					settings.RemoveHttpProtocolFromAttributes = true;
					settings.RemoveHttpsProtocolFromAttributes = true;
				})
				.AddXhtmlMinification(options =>
				{
					options.IncludedPages = new List<IUrlMatcher>
					{
						new RegexUrlMatcher(@"^/minifiers/x(?:ht)?ml-minifier$"),
						new ExactUrlMatcher("/contact")
					};

					XhtmlMinificationSettings settings = options.MinificationSettings;
					settings.RemoveRedundantAttributes = true;
					settings.RemoveHttpProtocolFromAttributes = true;
					settings.RemoveHttpsProtocolFromAttributes = true;
				})
				.AddXmlMinification(options => {
					XmlMinificationSettings settings = options.MinificationSettings;
					settings.CollapseTagsWithoutContent = true;
				})
				.AddHttpCompression()
				;

			// Add framework services.
			services.AddMvc();

			// Add WebMarkupMin sample services to the services container.
			services.AddSingleton<SitemapService>();
			services.AddSingleton<CssMinifierFactory>();
			services.AddSingleton<JsMinifierFactory>();
			services.AddSingleton<HtmlMinificationService>();
			services.AddSingleton<XhtmlMinificationService>();
			services.AddSingleton<XmlMinificationService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseIISPlatformHandler();

			app.UseStaticFiles();

			app.UseWebMarkupMin();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}

		// Entry point for the application.
		public static void Main(string[] args) => Microsoft.AspNet.Hosting.WebApplication.Run<Startup>(args);
	}
}