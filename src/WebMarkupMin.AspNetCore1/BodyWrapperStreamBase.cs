using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AdvancedStringBuilder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet.Common.Compressors;
using WebMarkupMin.Core;
using WebMarkupMin.Core.Utilities;
using AspNetCommonStrings = WebMarkupMin.AspNet.Common.Resources.Strings;

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2
#elif ASPNETCORE3
namespace WebMarkupMin.AspNetCore3
#elif ASPNETCORE5
namespace WebMarkupMin.AspNetCore5
#else
#error No implementation for this target
#endif
{
	/// <summary>
	/// Base class of stream wrapper that apply a markup minification and compression only if necessary
	/// </summary>
	internal abstract class BodyWrapperStreamBase : Stream
	{
		/// <summary>
		/// HTTP context
		/// </summary>
		protected readonly HttpContext _context;

		/// <summary>
		/// Original stream
		/// </summary>
		private readonly Stream _originalStream;

		/// <summary>
		/// Stream that original content is read into
		/// </summary>
		private MemoryStream _cachedStream;

		/// <summary>
		/// Compression stream
		/// </summary>
		private Stream _compressionStream;

		/// <summary>
		/// Flag for whether to do automatically flush the compression stream
		/// </summary>
		protected bool _autoFlushCompressionStream = false;

		/// <summary>
		/// WebMarkupMin configuration
		/// </summary>
		private readonly WebMarkupMinOptions _options;

		/// <summary>
		/// List of markup minification managers
		/// </summary>
		private readonly IList<IMarkupMinificationManager> _minificationManagers;

		/// <summary>
		/// HTTP compression manager
		/// </summary>
		private readonly IHttpCompressionManager _compressionManager;

		/// <summary>
		/// Flag indicating whether the stream wrapper is initialized
		/// </summary>
		private InterlockedStatedFlag _wrapperInitializedFlag = new InterlockedStatedFlag();

		/// <summary>
		/// Flag indicating whether a markup minification is enabled
		/// </summary>
		protected bool _minificationEnabled = false;

		/// <summary>
		/// Flag indicating whether a HTTP compression is enabled
		/// </summary>
		protected bool _compressionEnabled = false;

		/// <summary>
		/// Current URL
		/// </summary>
		private string _currentUrl;

		/// <summary>
		/// Text encoding
		/// </summary>
		private Encoding _encoding;

		/// <summary>
		/// Current markup minification manager
		/// </summary>
		private IMarkupMinificationManager _currentMinificationManager;

		/// <summary>
		/// Current HTTP compressor
		/// </summary>
		protected ICompressor _currentCompressor;

		/// <summary>
		/// Flag indicating whether the current HTTP compressor is initialized
		/// </summary>
		private InterlockedStatedFlag _currentCompressorInitializedFlag = new InterlockedStatedFlag();

		/// <summary>
		/// Flag that indicates if the HTTP headers is modified for compression
		/// </summary>
		private InterlockedStatedFlag _httpHeadersModifiedForCompressionFlag = new InterlockedStatedFlag();

		/// <summary>
		/// Flag that the stream wrapper is destroyed
		/// </summary>
		private InterlockedStatedFlag _disposedFlag = new InterlockedStatedFlag();


		/// <summary>
		/// Constructs an instance of the stream wrapper
		/// </summary>
		/// <param name="context">HTTP context</param>
		/// <param name="originalStream">Original stream</param>
		/// <param name="options">WebMarkupMin configuration</param>
		/// <param name="minificationManagers">List of markup minification managers</param>
		/// <param name="compressionManager">HTTP compression manager</param>
		protected BodyWrapperStreamBase(HttpContext context, Stream originalStream,
			WebMarkupMinOptions options, IList<IMarkupMinificationManager> minificationManagers,
			IHttpCompressionManager compressionManager)
		{
			_context = context;
			_originalStream = originalStream;
			_options = options;
			_minificationManagers = minificationManagers;
			_compressionManager = compressionManager;
		}


		protected void Initialize()
		{
			if (_wrapperInitializedFlag.Set())
			{
				HttpRequest request = _context.Request;
				HttpResponse response = _context.Response;

				if (response.StatusCode == 200)
				{
					string httpMethod = request.Method;
					string contentType = response.ContentType;
					string mediaType = null;
					Encoding encoding = null;

					if (contentType != null)
					{
						MediaTypeHeaderValue mediaTypeHeader;

						if (MediaTypeHeaderValue.TryParse(contentType, out mediaTypeHeader))
						{
							mediaType = mediaTypeHeader.MediaType
#if ASPNETCORE2 || ASPNETCORE3 || ASPNETCORE5
								.Value
#endif
								.ToLowerInvariant()
								;
							encoding = mediaTypeHeader.Encoding;
						}
					}

					string currentUrl = GetCurrentUrl(request);
					IHeaderDictionary responseHeaders = response.Headers;
					bool isEncodedContent = responseHeaders.IsEncodedContent();

					int minificationManagerCount = _minificationManagers.Count;
					if (minificationManagerCount > 0)
					{
						for (int managerIndex = 0; managerIndex < minificationManagerCount; managerIndex++)
						{
							IMarkupMinificationManager minificationManager = _minificationManagers[managerIndex];
							if (minificationManager.IsSupportedHttpMethod(httpMethod)
								&& mediaType != null && minificationManager.IsSupportedMediaType(mediaType)
								&& minificationManager.IsProcessablePage(currentUrl))
							{
								if (isEncodedContent)
								{
									throw new InvalidOperationException(
										string.Format(
											AspNetCommonStrings.MarkupMinificationIsNotApplicableToEncodedContent,
											responseHeaders[HeaderNames.ContentEncoding]
										)
									);
								}

								_currentMinificationManager = minificationManager;
								_cachedStream = new MemoryStream();
								_minificationEnabled = true;

								break;
							}
						}
					}

					if (_compressionManager != null && !isEncodedContent
						&& _compressionManager.IsSupportedHttpMethod(httpMethod)
						&& _compressionManager.IsSupportedMediaType(mediaType)
						&& _compressionManager.IsProcessablePage(currentUrl))
					{
						string acceptEncoding = request.Headers[HeaderNames.AcceptEncoding];
						ICompressor compressor = InitializeCurrentCompressor(acceptEncoding);

						if (compressor != null)
						{
							_compressionStream = compressor.Compress(_originalStream);
							_compressionEnabled = true;
						}
					}

					_currentUrl = currentUrl;
					_encoding = encoding;
				}
			}
		}

		private static string GetCurrentUrl(HttpRequest request)
		{
			PathString pathBase = request.PathBase;
			PathString path = request.Path;
			QueryString queryString = request.QueryString;

			if (!queryString.HasValue)
			{
				if (pathBase.HasValue && !path.HasValue)
				{
					return pathBase.Value;
				}

				if (!pathBase.HasValue && path.HasValue)
				{
					return path.Value;
				}
			}

			var stringBuilderPool = StringBuilderPool.Shared;
			StringBuilder urlBuilder = stringBuilderPool.Rent();

			if (pathBase.HasValue)
			{
				urlBuilder.Append(pathBase.Value);
			}

			if (path.HasValue)
			{
				urlBuilder.Append(path.Value);
			}

			if (urlBuilder.Length > 0 && queryString.HasValue)
			{
				urlBuilder.Append(queryString.Value);
			}

			string currentUrl = urlBuilder.ToString();
			stringBuilderPool.Return(urlBuilder);

			return currentUrl;
		}

		protected ICompressor InitializeCurrentCompressor(string acceptEncoding)
		{
			if (_currentCompressorInitializedFlag.Set())
			{
				_compressionManager?.TryCreateCompressor(acceptEncoding, out _currentCompressor);
			}

			return _currentCompressor;
		}

		private void ModifyHttpHeadersForCompressionOnce()
		{
			if (_httpHeadersModifiedForCompressionFlag.Set())
			{
				IHeaderDictionary responseHeaders = _context.Response.Headers;
				_currentCompressor.AppendHttpHeaders((key, value) =>
				{
					responseHeaders.Append(key, new StringValues(value));
				});
				responseHeaders.Remove(HeaderNames.ContentMD5);
				responseHeaders.Remove(HeaderNames.ContentLength);
			}
		}
#if NET451 || ASPNETCORE2 || ASPNETCORE3 || ASPNETCORE5

		private async void InternalWriteAsync(byte[] buffer, int offset, int count, AsyncCallback callback,
			TaskCompletionSource<object> tcs)
		{
			try
			{
				await WriteAsync(buffer, offset, count);
				tcs.TrySetResult(null);
			}
			catch (Exception ex)
			{
				tcs.TrySetException(ex);
			}

			if (callback != null)
			{
				// Offload callbacks to avoid stack dives on sync completions
				var ignored = Task.Run(() =>
				{
					try
					{
						callback(tcs.Task);
					}
					catch (Exception)
					{
						// Suppress exceptions on background threads
					}
				});
			}
		}
#endif

		protected async Task InternalFinishAsync()
		{
			if (_minificationEnabled)
			{
				bool isMinified = false;
				int cachedByteCount = (int)_cachedStream.Length;
				IHeaderDictionary responseHeaders = _context.Response.Headers;
				Action<string, string> appendHttpHeader = (key, value) =>
				{
					responseHeaders.Append(key, new StringValues(value));
				};

				if (cachedByteCount > 0 && _options.IsAllowableResponseSize(cachedByteCount))
				{
					Encoding encoding = _encoding ?? _options.DefaultEncoding;
#if NETSTANDARD1_3
					byte[] cachedBytes = _cachedStream.ToArray();
					string content = encoding.GetString(cachedBytes);
#else
					byte[] cachedBytes = _cachedStream.GetBuffer();
					string content = encoding.GetString(cachedBytes, 0, cachedByteCount);
#endif

					IMarkupMinifier minifier = _currentMinificationManager.CreateMinifier();
					MarkupMinificationResult minificationResult = minifier.Minify(content, _currentUrl,
						_encoding, _currentMinificationManager.GenerateStatistics);

					if (minificationResult.Errors.Count == 0)
					{
						if (_options.IsPoweredByHttpHeadersEnabled())
						{
							_currentMinificationManager.AppendPoweredByHttpHeader(appendHttpHeader);
						}
						responseHeaders.Remove(HeaderNames.ContentMD5);

						string processedContent = minificationResult.MinifiedContent;
						var byteArrayPool = ArrayPool<byte>.Shared;
						int processedByteCount = encoding.GetByteCount(processedContent);
						byte[] processedBytes = byteArrayPool.Rent(processedByteCount);

						try
						{
							encoding.GetBytes(processedContent, 0, processedContent.Length, processedBytes, 0);

							if (_compressionEnabled)
							{
								_currentCompressor.AppendHttpHeaders(appendHttpHeader);
								responseHeaders.Remove(HeaderNames.ContentLength);
								await _compressionStream.WriteAsync(processedBytes, 0, processedByteCount);
							}
							else
							{
								responseHeaders[HeaderNames.ContentLength] = processedByteCount.ToString();
								await _originalStream.WriteAsync(processedBytes, 0, processedByteCount);
							}
						}
						finally
						{
							byteArrayPool.Return(processedBytes);
						}

						isMinified = true;
					}
				}

				if (!isMinified)
				{
					Stream outputStream;

					if (_compressionEnabled)
					{
						outputStream = _compressionStream;
						_currentCompressor.AppendHttpHeaders(appendHttpHeader);
					}
					else
					{
						outputStream = _originalStream;
					}

					_cachedStream.Seek(0, SeekOrigin.Begin);
					await _cachedStream.CopyToAsync(outputStream);
				}

				_cachedStream.Clear();
			}
		}

		public virtual async Task FinishAsync()
		{
			await Task.Run(() => throw new NotImplementedException());
		}

		#region Stream overrides

		public override long Position
		{
			get { throw new NotSupportedException(); }
			set { throw new NotSupportedException(); }
		}

		public override long Length
		{
			get { throw new NotSupportedException(); }
		}

		public override bool CanWrite
		{
			get { return _originalStream.CanWrite; }
		}

		public override bool CanSeek
		{
			get { return false; }
		}

		public override bool CanRead
		{
			get { return false; }
		}


#if NET451 || ASPNETCORE2 || ASPNETCORE3 || ASPNETCORE5
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback,
			object state)
		{
			var tcs = new TaskCompletionSource<object>(state);
			InternalWriteAsync(buffer, offset, count, callback, tcs);

			return tcs.Task;
		}

		public override void EndWrite(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException(nameof(asyncResult));
			}

			var task = (Task)asyncResult;
			task.GetAwaiter().GetResult();
		}

#endif
		public override void Flush()
		{
			Initialize();

			if (_minificationEnabled)
			{
				_cachedStream.Flush();
			}
			else if (_compressionEnabled)
			{
				_compressionStream.Flush();
			}
			else
			{
				_originalStream.Flush();
			}
		}

		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			Initialize();

			if (_minificationEnabled)
			{
				return _cachedStream.FlushAsync(cancellationToken);
			}
			else if (_compressionEnabled)
			{
				return _compressionStream.FlushAsync(cancellationToken);
			}

			return _originalStream.FlushAsync(cancellationToken);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			Initialize();

			if (_minificationEnabled)
			{
				_cachedStream.Write(buffer, offset, count);
			}
			else if (_compressionEnabled)
			{
				ModifyHttpHeadersForCompressionOnce();
				_compressionStream.Write(buffer, offset, count);
				if (_autoFlushCompressionStream)
				{
					_compressionStream.Flush();
				}
			}
			else
			{
				_originalStream.Write(buffer, offset, count);
			}
		}

		public override async Task WriteAsync(byte[] buffer, int offset, int count,
			CancellationToken cancellationToken)
		{
			Initialize();

			if (_minificationEnabled)
			{
				await _cachedStream.WriteAsync(buffer, offset, count, cancellationToken);
			}
			else if (_compressionEnabled)
			{
				ModifyHttpHeadersForCompressionOnce();
				await _compressionStream.WriteAsync(buffer, offset, count, cancellationToken);
				if (_autoFlushCompressionStream)
				{
					await _compressionStream.FlushAsync(cancellationToken);
				}
			}
			else
			{
				await _originalStream.WriteAsync(buffer, offset, count, cancellationToken);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (_disposedFlag.Set())
			{
				if (disposing)
				{
					if (_compressionStream != null)
					{
						_compressionStream.Dispose();
						_compressionStream = null;
					}

					_currentCompressor = null;

					if (_cachedStream != null)
					{
						_cachedStream.Dispose();
						_cachedStream = null;
					}

					_currentMinificationManager = null;
				}

				base.Dispose(disposing);
			}
		}
#if ASPNETCORE3 || ASPNETCORE5

		public override async ValueTask DisposeAsync()
		{
			if (_disposedFlag.Set())
			{
				if (_compressionStream != null)
				{
					await _compressionStream.DisposeAsync();
					_compressionStream = null;
				}

				_currentCompressor = null;

				if (_cachedStream != null)
				{
					await _cachedStream.DisposeAsync();
					_cachedStream = null;
				}

				_currentMinificationManager = null;

				await base.DisposeAsync();
			}
		}
#endif

		#endregion
	}
}