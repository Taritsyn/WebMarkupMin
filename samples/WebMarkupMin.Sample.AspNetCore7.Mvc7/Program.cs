using System.IO.Compression;

using WebMarkupMin.AspNet.Common.Compressors;
using WebMarkupMin.AspNetCore7;
using WebMarkupMin.Core;
using WebMarkupMin.NUglify;

using IWmmLogger = WebMarkupMin.Core.Loggers.ILogger;
using WmmAspNetCoreLogger = WebMarkupMin.AspNetCore7.AspNetCoreLogger;

var builder = WebApplication.CreateBuilder(args);

#region Configure services

IServiceCollection services = builder.Services;

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

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();