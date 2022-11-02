using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;

using WebMarkupMin.AspNet.Common;

#if ASPNETCORE3
namespace WebMarkupMin.AspNetCore3
#elif ASPNETCORE5
namespace WebMarkupMin.AspNetCore5
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
			bool useCompression = _options.IsCompressionEnabled() && _compressionManager != null;

			if (!useMinification && !useCompression)
			{
				await _next(context);
			}
			else
			{
				await InvokeCore(context, useMinification, useCompression);
			}
		}

		protected override async Task InvokeCore(HttpContext context, bool useMinification, bool useCompression)
		{
			IFeatureCollection features = context.Features;
			IHttpResponseBodyFeature originalBodyFeature = features.Get<IHttpResponseBodyFeature>();
			var bodyWrapperStream = new BodyWrapperStreamWithResponseBodyFeature(context, _options,
				useMinification ? _minificationManagers : new List<IMarkupMinificationManager>(),
				useCompression ? _compressionManager : null,
				originalBodyFeature);

			features.Set<IHttpResponseBodyFeature>(bodyWrapperStream);

			try
			{
				await _next(context);
				await bodyWrapperStream.FinishAsync();
			}
			finally
			{
				await bodyWrapperStream.DisposeAsync();
				features.Set(originalBodyFeature);
			}
		}
	}
}