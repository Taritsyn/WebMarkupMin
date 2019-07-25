using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using WebMarkupMin.AspNet.Common;

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2
#elif ASPNETCORE3
namespace WebMarkupMin.AspNetCore3
#else
#error No implementation for this target
#endif
{
	/// <summary>
	/// WebMarkupMin middleware
	/// </summary>
	public class WebMarkupMinMiddleware
	{
		/// <summary>
		/// The next middleware in the pipeline
		/// </summary>
		private readonly RequestDelegate _next;

		/// <summary>
		/// WebMarkupMin configuration
		/// </summary>
		private readonly WebMarkupMinOptions _options;

		/// <summary>
		/// List of markup minification manager
		/// </summary>
		private readonly IList<IMarkupMinificationManager> _minificationManagers;

		/// <summary>
		/// HTTP compression manager
		/// </summary>
		private readonly IHttpCompressionManager _compressionManager;


		/// <summary>
		/// Constructs a instance of WebMarkupMin middleware
		/// </summary>
		/// <param name="next">The next middleware in the pipeline</param>
		/// <param name="options">WebMarkupMin options</param>
		/// <param name="services">The list of services</param>
		public WebMarkupMinMiddleware(RequestDelegate next,
			IOptions<WebMarkupMinOptions> options,
			IServiceProvider services)
		{
			if (next == null)
			{
				throw new ArgumentNullException(nameof(next));
			}

			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			_next = next;
			_options = options.Value;

			var minificationManagers = new List<IMarkupMinificationManager>();

			var htmlMinificationManager = services.GetService<IHtmlMinificationManager>();
			if (htmlMinificationManager != null)
			{
				minificationManagers.Add(htmlMinificationManager);
			}

			var xhtmlMinificationManager = services.GetService<IXhtmlMinificationManager>();
			if (xhtmlMinificationManager != null)
			{
				minificationManagers.Add(xhtmlMinificationManager);
			}

			var xmlMinificationManager = services.GetService<IXmlMinificationManager>();
			if (xmlMinificationManager != null)
			{
				minificationManagers.Add(xmlMinificationManager);
			}

			_minificationManagers = minificationManagers;

			var compressionManager = services.GetService<IHttpCompressionManager>();
			if (compressionManager != null)
			{
				_compressionManager = compressionManager;
			}
		}


		public async Task Invoke(HttpContext context)
		{
			bool useMinification = _options.IsMinificationEnabled() && _minificationManagers.Count > 0;
			bool useCompression = _options.IsCompressionEnabled() && _compressionManager != null;

			if (!useMinification && !useCompression)
			{
				await _next.Invoke(context);
				return;
			}
			else
			{
				HttpResponse response = context.Response;
				IFeatureCollection features = context.Features;

				Stream originalStream = response.Body;
				IHttpBufferingFeature originalBufferFeature = features.Get<IHttpBufferingFeature>();
				BodyWrapperStream bodyWrapperStream = new BodyWrapperStream(context, originalStream, _options,
					useMinification ? _minificationManagers : new List<IMarkupMinificationManager>(),
					useCompression ? _compressionManager : null,
					originalBufferFeature);

				response.Body = bodyWrapperStream;
				features.Set<IHttpBufferingFeature>(bodyWrapperStream);

				try
				{
					await _next(context);
					await bodyWrapperStream.Finish();
				}
				finally
				{
#if NETCOREAPP3_0
					await bodyWrapperStream.DisposeAsync();
#else
					bodyWrapperStream.Dispose();
#endif

					response.Body = originalStream;
					features.Set(originalBufferFeature);
				}
			}
		}
	}
}