using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using WebMarkupMin.AspNet.Common;

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2
#elif ASPNETCORE3
namespace WebMarkupMin.AspNetCore3
#elif ASPNETCORE6
namespace WebMarkupMin.AspNetCore6
#elif ASPNETCORE_LATEST
namespace WebMarkupMin.AspNetCoreLatest
#else
#error No implementation for this target
#endif
{
	public class WebMarkupMinServicesBuilder
	{
		private readonly IServiceCollection _services;

		/// <summary>
		/// Gets a <see cref="IServiceCollection"/> where essential WebMarkupMin services are configured
		/// </summary>
		public IServiceCollection Services
		{
			get { return _services; }
		}


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

		public WebMarkupMinServicesBuilder ConfigureHttpCompression(Action<HttpCompressionOptions> configure)
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

			_services.TryAddSingleton<IHtmlMinificationManager, HtmlMinificationManager>();

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

			_services.TryAddSingleton<IXhtmlMinificationManager, XhtmlMinificationManager>();

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

			_services.TryAddSingleton<IXmlMinificationManager, XmlMinificationManager>();

			return this;
		}

		public WebMarkupMinServicesBuilder AddHttpCompression()
		{
			return AddHttpCompression(configure: null);
		}

		public WebMarkupMinServicesBuilder AddHttpCompression(Action<HttpCompressionOptions> configure)
		{
			if (configure != null)
			{
				_services.Configure(configure);
			}

			_services.TryAddSingleton<IHttpCompressionManager, HttpCompressionManager>();

			return this;
		}
	}
}