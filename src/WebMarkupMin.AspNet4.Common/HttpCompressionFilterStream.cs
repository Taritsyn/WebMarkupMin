using System.IO;
using System.Web;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet.Common.Compressors;
using WebMarkupMin.AspNet4.Common.Helpers;
using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.AspNet4.Common
{
	/// <summary>
	/// HTTP compression response filter
	/// </summary>
	public sealed class HttpCompressionFilterStream : Stream
	{
		/// <summary>
		/// HTTP response
		/// </summary>
		private readonly HttpResponseBase _response;

		/// <summary>
		/// Original stream
		/// </summary>
		private readonly Stream _originalStream;

		/// <summary>
		/// Output stream
		/// </summary>
		private Stream _outputStream;

		/// <summary>
		/// Flag that indicates if the output stream is initialized
		/// </summary>
		private InterlockedStatedFlag _outputStreamInitializedFlag = new InterlockedStatedFlag();

		/// <summary>
		/// HTTP compression manager
		/// </summary>
		private readonly IHttpCompressionManager _compressionManager;

		/// <summary>
		/// HTTP compressor
		/// </summary>
		private ICompressor _compressor;

		/// <summary>
		/// Value of the Accept-Encoding HTTP header
		/// </summary>
		private readonly string _acceptEncoding;

		/// <summary>
		/// Flag that indicates if the HTTP headers is appended
		/// </summary>
		private InterlockedStatedFlag _httpHeadersAppendedFlag = new InterlockedStatedFlag();

		public override bool CanRead
		{
			get { return GetOutputStream().CanRead; }
		}

		public override bool CanSeek
		{
			get { return GetOutputStream().CanSeek; }
		}

		public override bool CanWrite
		{
			get { return GetOutputStream().CanWrite; }
		}

		public override long Length
		{
			get { return GetOutputStream().Length; }
		}

		public override long Position
		{
			get
			{
				return GetOutputStream().Position;
			}
			set
			{
				GetOutputStream().Position = value;
			}
		}


		/// <summary>
		/// Constructs a instance of HTTP compression response filter
		/// </summary>
		/// <param name="response">HTTP response</param>
		/// <param name="compressionManager">HTTP compression manager</param>
		/// <param name="acceptEncoding">Value of the Accept-Encoding HTTP header</param>
		public HttpCompressionFilterStream(HttpResponseBase response,
			IHttpCompressionManager compressionManager,
			string acceptEncoding)
		{
			_response = response;
			_originalStream = response.Filter;
			_compressionManager = compressionManager;
			_acceptEncoding = acceptEncoding;
		}


		private Stream GetOutputStream()
		{
			if (_outputStreamInitializedFlag.Set())
			{
				if (HttpHeadersHelpers.IsEncodedContent(_response.Headers)
					|| !_compressionManager.IsSupportedMediaType(_response.ContentType))
				{
					_outputStream = _originalStream;
				}
				else
				{
					if (!_compressionManager.TryCreateCompressor(_acceptEncoding, out _compressor))
					{
						_compressor = new NullCompressor();
					}
					_outputStream = _compressor.Compress(_originalStream);
				}
			}

			return _outputStream;
		}

		private void AppendHttpHeadersOnce()
		{
			if (_httpHeadersAppendedFlag.Set())
			{
				if (_compressor != null)
				{
					_compressor.AppendHttpHeaders((key, value) =>
					{
						_response.Headers[key] = value;
					});
				}
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return GetOutputStream().Read(buffer, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return GetOutputStream().Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			GetOutputStream().SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			Stream outputStream = GetOutputStream();

			AppendHttpHeadersOnce();
			outputStream.Write(buffer, offset, count);
		}

		public override void Flush()
		{
			GetOutputStream().Flush();
		}

		public override void Close()
		{
			GetOutputStream().Close();
		}
	}
}