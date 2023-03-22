using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet.Common.Compressors;

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2
#else
#error No implementation for this target
#endif
{
	/// <summary>
	/// Stream wrapper with HTTP buffering feature that apply a markup minification and compression only if necessary
	/// </summary>
	internal sealed class BodyWrapperStreamWithBufferingFeature : BodyWrapperStreamBase, IHttpBufferingFeature
	{
		/// <summary>
		/// HTTP buffering feature
		/// </summary>
		private readonly IHttpBufferingFeature _bufferingFeature;


		/// <summary>
		/// Constructs an instance of the stream wrapper with HTTP buffering feature
		/// </summary>
		/// <param name="context">HTTP context</param>
		/// <param name="originalStream">Original stream</param>
		/// <param name="options">WebMarkupMin configuration</param>
		/// <param name="minificationManagers">List of markup minification managers</param>
		/// <param name="compressionManager">HTTP compression manager</param>
		/// <param name="bufferingFeature">HTTP buffering feature</param>
		internal BodyWrapperStreamWithBufferingFeature(HttpContext context, Stream originalStream,
			WebMarkupMinOptions options, IList<IMarkupMinificationManager> minificationManagers,
			IHttpCompressionManager compressionManager, IHttpBufferingFeature bufferingFeature)
			: base(context, originalStream, options, minificationManagers, compressionManager)
		{
			_bufferingFeature = bufferingFeature;
		}


		public override async Task FinishAsync()
		{
			await InternalFinishAsync();
			Dispose();
		}

		#region IHttpBufferingFeature implementation

		public void DisableRequestBuffering()
		{
			_bufferingFeature?.DisableRequestBuffering();
		}

		public void DisableResponseBuffering()
		{
			string acceptEncoding = _context.Request.Headers[HeaderNames.AcceptEncoding];
			ICompressor currentCompressor = ResolveCurrentCompressor(acceptEncoding);

			if (currentCompressor?.SupportsFlush == false)
			{
				// Some of the compressors don't support flushing which would block real-time
				// responses like SignalR.
				_compressionEnabled = false;
				_currentCompressor = null;
			}
			else
			{
				_autoFlushCompressionStream = true;
			}

			_bufferingFeature?.DisableResponseBuffering();
		}

		#endregion
	}
}