using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;

using WebMarkupMin.AspNet.Common;

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2
#else
#error No implementation for this target
#endif
{
	/// <summary>
	/// WebMarkupMin middleware
	/// </summary>
	public sealed class WebMarkupMinMiddleware : WebMarkupMinMiddlewareBase
	{
		/// <summary>
		/// Constructs a instance of WebMarkupMin middleware
		/// </summary>
		/// <param name="next">The next middleware in the pipeline</param>
		/// <param name="options">WebMarkupMin options</param>
		/// <param name="services">The list of services</param>
		public WebMarkupMinMiddleware(RequestDelegate next,
			IOptions<WebMarkupMinOptions> options,
			IServiceProvider services)
			: base(next, options, services)
		{ }


		public async Task Invoke(HttpContext context)
		{
			bool useMinification = _options.IsMinificationEnabled() && _minificationManagers.Count > 0;
			bool useCompression = _options.IsCompressionEnabled() && _compressionManager is not null;

			if (!useMinification && !useCompression)
			{
				await _next.Invoke(context);
			}
			else
			{
				await InvokeCore(context, useMinification, useCompression);
			}
		}

		protected override async Task InvokeCore(HttpContext context, bool useMinification, bool useCompression)
		{
			HttpResponse response = context.Response;
			IFeatureCollection features = context.Features;

			Stream originalStream = response.Body;
			IHttpBufferingFeature originalBufferFeature = features.Get<IHttpBufferingFeature>();
			var bodyWrapperStream = new BodyWrapperStreamWithBufferingFeature(context, 	originalStream, _options,
				useMinification ? _minificationManagers : new List<IMarkupMinificationManager>(),
				useCompression ? _compressionManager : null,
				originalBufferFeature);

			response.Body = bodyWrapperStream;
			features.Set<IHttpBufferingFeature>(bodyWrapperStream);

			try
			{
				await _next(context);
				await bodyWrapperStream.FinishAsync();
			}
			finally
			{
				bodyWrapperStream.Dispose();

				response.Body = originalStream;
				features.Set(originalBufferFeature);
			}
		}
	}
}