using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;

using WebMarkupMin.AspNet.Common;

namespace WebMarkupMin.AspNet5
{
	public class WebMarkupMinServicesBuilder
	{
		/// <summary>
		/// Collection of service descriptors
		/// </summary>
		private readonly IServiceCollection _services;


		/// <summary>
		/// Constructs a instance of WebMarkupMin builder
		/// </summary>
		/// <param name="services"></param>
		public WebMarkupMinServicesBuilder(IServiceCollection services)
		{
			_services = services;
		}


		public WebMarkupMinServicesBuilder ConfigureWebMarkupMin(Action<WebMarkupMinOptions> configure)
		{
			_services.Configure(configure);

			return this;
		}

		public WebMarkupMinServicesBuilder ConfigureHtmlMinification(Action<HtmlMinificationOptions> configure)
		{
			_services.Configure(configure);

			return this;
		}

		public WebMarkupMinServicesBuilder ConfigureXhtmlMinification(Action<XhtmlMinificationOptions> configure)
		{
			_services.Configure(configure);

			return this;
		}

		public WebMarkupMinServicesBuilder ConfigureXmlMinification(Action<XmlMinificationOptions> configure)
		{
			_services.Configure(configure);

			return this;
		}

		public WebMarkupMinServicesBuilder AddHtmlMinification()
		{
			return AddHtmlMinification(configure: null);
		}

		public WebMarkupMinServicesBuilder AddHtmlMinification(Action<HtmlMinificationOptions> configure)
		{
			_services.AddSingleton<IConfigureOptions<HtmlMinificationOptions>, HtmlMinificationOptionsSetup>();

			if (configure != null)
			{
				_services.Configure(configure);
			}

			_services.AddSingleton<IHtmlMinificationManager, HtmlMinificationManager>();

			return this;
		}

		public WebMarkupMinServicesBuilder AddXhtmlMinification()
		{
			return AddXhtmlMinification(configure: null);
		}

		public WebMarkupMinServicesBuilder AddXhtmlMinification(Action<XhtmlMinificationOptions> configure)
		{
			_services.AddSingleton<IConfigureOptions<XhtmlMinificationOptions>, XhtmlMinificationOptionsSetup>();

			if (configure != null)
			{
				_services.Configure(configure);
			}

			_services.AddSingleton<IXhtmlMinificationManager, XhtmlMinificationManager>();

			return this;
		}

		public WebMarkupMinServicesBuilder AddXmlMinification()
		{
			return AddXmlMinification(configure: null);
		}

		public WebMarkupMinServicesBuilder AddXmlMinification(Action<XmlMinificationOptions> configure)
		{
			if (configure != null)
			{
				_services.Configure(configure);
			}

			_services.AddSingleton<IXmlMinificationManager, XmlMinificationManager>();

			return this;
		}

		public WebMarkupMinServicesBuilder AddHttpCompression()
		{
			_services.AddSingleton<IHttpCompressionManager, HttpCompressionManager>();

			return this;
		}
	}
}