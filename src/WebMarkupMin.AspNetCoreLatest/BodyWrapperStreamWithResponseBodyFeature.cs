using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet.Common.Compressors;
using WebMarkupMin.Core.Utilities;

#if ASPNETCORE3
namespace WebMarkupMin.AspNetCore3
#elif ASPNETCORE6
namespace WebMarkupMin.AspNetCore6
#elif ASPNETCORE_LATEST
namespace WebMarkupMin.AspNetCoreLatest
#else
#error No implementation for this target
#endif
{
	/// <summary>
	/// Stream wrapper with HTTP response body feature that apply a markup minification and compression only if necessary
	/// </summary>
	internal sealed class BodyWrapperStreamWithResponseBodyFeature : BodyWrapperStreamBase, IHttpResponseBodyFeature
	{
		/// <summary>
		/// HTTP response body feature
		/// </summary>
		private readonly IHttpResponseBodyFeature _responseBodyFeature;

		/// <summary>
		/// Pipe adapter
		/// </summary>
		private PipeWriter _pipeAdapter = null;

		/// <summary>
		/// Flag indicating whether the modification of HTTP headers is required
		/// </summary>
		private bool _httpHeadersModificationRequired = true;

		/// <summary>
		/// Flag indicating whether the processing is finished
		/// </summary>
		private StatedFlag _finishedFlag = new StatedFlag();


		/// <summary>
		/// Constructs an instance of the stream wrapper with HTTP response body feature
		/// </summary>
		/// <param name="context">HTTP context</param>
		/// <param name="options">WebMarkupMin configuration</param>
		/// <param name="minificationManagers">List of markup minification managers</param>
		/// <param name="compressionManager">HTTP compression manager</param>
		/// <param name="responseBodyFeature">HTTP response body feature</param>
		internal BodyWrapperStreamWithResponseBodyFeature(HttpContext context,
			WebMarkupMinOptions options, IList<IMarkupMinificationManager> minificationManagers,
			IHttpCompressionManager compressionManager, IHttpResponseBodyFeature responseBodyFeature)
			: base(context, responseBodyFeature.Stream, options, minificationManagers, compressionManager)
		{
			_responseBodyFeature = responseBodyFeature;
		}


		public override async Task FinishAsync()
		{
			if (_finishedFlag.Set())
			{
				await InternalFinishAsync();
				_httpHeadersModificationRequired = false;

				if (_pipeAdapter is not null)
				{
					await _pipeAdapter.CompleteAsync();
				}
				await DisposeAsync();
			}
		}

		#region IHttpResponseBodyFeature implementation

		public Stream Stream
		{
			get { return this; }
		}

		public PipeWriter Writer
		{
			get
			{
				if (_pipeAdapter is null)
				{
					_pipeAdapter = PipeWriter.Create(Stream, new StreamPipeWriterOptions(leaveOpen: true));
				}

				return _pipeAdapter;
			}
		}


		public void DisableBuffering()
		{
			string acceptEncoding = _context.Request.Headers[HeaderNames.AcceptEncoding];
			ICompressor currentCompressor = ResolveCurrentCompressor(acceptEncoding);

			if (currentCompressor?.SupportsFlush == false)
			{
				// Some of the compressors don't support flushing which would block real-time
				// responses like SignalR.
				ResetCurrentCompressor();
			}
			else
			{
				_autoFlushCompressionStream = true;
			}

			_responseBodyFeature.DisableBuffering();
		}

		public Task StartAsync(CancellationToken token = default)
		{
			Initialize();

			if (_httpHeadersModificationRequired && (_minificationEnabled || _compressionEnabled))
			{
				// Prevent start of the response so that the headers can be modified
				return Task.CompletedTask;
			}

			return _responseBodyFeature.StartAsync(token);
		}

		public Task SendFileAsync(string path, long offset, long? count, CancellationToken cancellationToken)
		{
			Initialize();

			if (_minificationEnabled || _compressionEnabled)
			{
				return SendFileFallback.SendFileAsync(Stream, path, offset, count, cancellationToken);
			}

			return _responseBodyFeature.SendFileAsync(path, offset, count, cancellationToken);
		}

		public async Task CompleteAsync()
		{
			if (_finishedFlag.IsSet())
			{
				return;
			}

			await FinishAsync(); // Sets a `_finishedFlag` field
			await _responseBodyFeature.CompleteAsync();
		}

		#endregion
	}
}