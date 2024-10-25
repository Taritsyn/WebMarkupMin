using System.Collections.Generic;
using System.IO.Compression;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WebMarkupMin.AspNet.Common.Compressors;
using WebMarkupMin.AspNetCore5;
using WebMarkupMin.Core;
using WebMarkupMin.NUglify;

using IWmmLogger = WebMarkupMin.Core.Loggers.ILogger;
using WmmAspNetCoreLogger = WebMarkupMin.AspNetCore5.AspNetCoreLogger;

namespace WebMarkupMin.Sample.AspNetCore5.Mvc5
{
	public class Startup
	{
		public IConfiguration Configuration
		{
			get;
		}


		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}


		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Add response caching service.
			services.AddResponseCaching();

			// Add WebMarkupMin services to the services container.
			services.AddWebMarkupMin(options =>
			{
				options.AllowMinificationInDevelopmentEnvironment = true;
				options.AllowCompressionInDevelopmentEnvironment = true;
			})
				.AddHtmlMinification(options =>
				{
					HtmlMinificationSettings settings = options.MinificationSettings;
					settings.RemoveRedundantAttributes = true;
					settings.RemoveHttpProtocolFromAttributes = true;
					settings.RemoveHttpsProtocolFromAttributes = true;

					options.CssMinifierFactory = new NUglifyCssMinifierFactory();
					options.JsMinifierFactory = new NUglifyJsMinifierFactory();
				})
				.AddHttpCompression(options =>
				{
					options.CompressorFactories = new List<ICompressorFactory>
					{
						new BuiltInBrotliCompressorFactory(new BuiltInBrotliCompressionSettings
						{
							Level = CompressionLevel.Fastest
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

			// Override the default logger for WebMarkupMin.
			services.AddSingleton<IWmmLogger, WmmAspNetCoreLogger>();

			services.AddControllersWithViews();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseResponseCaching();

			app.UseWebMarkupMin();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}