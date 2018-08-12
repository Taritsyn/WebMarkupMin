using System.Collections.Generic;
using System.IO.Compression;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using WebMarkupMin.AspNet.Brotli;
using WebMarkupMin.AspNet.Common.Compressors;
using WebMarkupMin.AspNet.Common.UrlMatchers;
using WebMarkupMin.AspNetCore1;
using WebMarkupMin.Core;
using WebMarkupMin.NUglify;
using WebMarkupMin.Sample.Logic.Services;

namespace WebMarkupMin.Sample.AspNetCore1.Mvc1
{
	public class Startup
	{
		/// <summary>
		/// Gets or sets a instance of hosting environment
		/// </summary>
		public IHostingEnvironment HostingEnvironment
		{
			get;
			set;
		}

		public IConfigurationRoot Configuration
		{
			get;
			set;
		}


		public Startup(IHostingEnvironment env)
		{
			HostingEnvironment = env;

			// Set up configuration sources.
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables()
				;
			Configuration = builder.Build();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton(Configuration);

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

					options.CssMinifierFactory = new NUglifyCssMinifierFactory();
					options.JsMinifierFactory = new NUglifyJsMinifierFactory();
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

					options.CssMinifierFactory = new KristensenCssMinifierFactory();
					options.JsMinifierFactory = new CrockfordJsMinifierFactory();
				})
				.AddXmlMinification(options =>
				{
					XmlMinificationSettings settings = options.MinificationSettings;
					settings.CollapseTagsWithoutContent = true;
				})
				.AddHttpCompression(options =>
				{
					options.CompressorFactories = new List<ICompressorFactory>
					{
						new BrotliCompressorFactory(new BrotliCompressionSettings
						{
							Level = 1
						}),
						new DeflateCompressorFactory(new DeflateCompressionSettings
						{
							Level = CompressionLevel.Fastest
						}),
						new GZipCompressorFactory(new GZipCompressionSettings
						{
							Level = CompressionLevel.Fastest
						})
					};
				})
				;

			// Add framework services.
			services.AddMvc(options =>
			{
				options.CacheProfiles.Add("CacheCompressedContent5Minutes",
					new CacheProfile
					{
						NoStore = HostingEnvironment.IsDevelopment(),
						Duration = 300,
						Location = ResponseCacheLocation.Client,
						VaryByHeader = "Accept-Encoding"
					}
				);
			});

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
				app.UseExceptionHandler("/error");
			}

			app.UseStatusCodePages();

			app.UseStaticFiles();

			app.UseWebMarkupMin();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
