using System.Collections.Generic;
using System.IO.Compression;

using WebMarkupMin.AspNet.Brotli;
using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet.Common.Compressors;
using WebMarkupMin.AspNet4.Common;
using WebMarkupMin.Core;
using WebMarkupMin.Core.Loggers;
using WebMarkupMin.MsAjax;

namespace WebMarkupMin.Sample.AspNet45.WebForms
{
	public class WebMarkupMinConfig
	{
		public static void Configure(WebMarkupMinConfiguration configuration)
		{
			configuration.AllowMinificationInDebugMode = true;
			configuration.AllowCompressionInDebugMode = true;

			DefaultLogger.Current = new ThrowExceptionLogger();
			DefaultCssMinifierFactory.Current = new MsAjaxCssMinifierFactory();
			DefaultJsMinifierFactory.Current = new MsAjaxJsMinifierFactory();

			IHtmlMinificationManager htmlMinificationManager = HtmlMinificationManager.Current;
			HtmlMinificationSettings htmlMinificationSettings = htmlMinificationManager.MinificationSettings;
			htmlMinificationSettings.RemoveRedundantAttributes = true;
			htmlMinificationSettings.RemoveHttpProtocolFromAttributes = true;
			htmlMinificationSettings.RemoveHttpsProtocolFromAttributes = true;

			IXhtmlMinificationManager xhtmlMinificationManager = XhtmlMinificationManager.Current;
			XhtmlMinificationSettings xhtmlMinificationSettings = xhtmlMinificationManager.MinificationSettings;
			xhtmlMinificationSettings.RemoveRedundantAttributes = true;
			xhtmlMinificationSettings.RemoveHttpProtocolFromAttributes = true;
			xhtmlMinificationSettings.RemoveHttpsProtocolFromAttributes = true;

			IHttpCompressionManager httpCompressionManager = HttpCompressionManager.Current;
			httpCompressionManager.CompressorFactories = new List<ICompressorFactory>
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
		}
	}
}