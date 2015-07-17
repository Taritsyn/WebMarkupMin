using System.IO;
using System.Text;
using System.Web;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.Core;

namespace WebMarkupMin.AspNet4.Common
{
	/// <summary>
	/// Markup minification response filters
	/// </summary>
	public sealed class MarkupMinificationFilterStream : Stream
	{
		/// <summary>
		/// HTTP-response
		/// </summary>
		private readonly HttpResponseBase _response;

		/// <summary>
		/// Original stream
		/// </summary>
		private readonly Stream _stream;

		/// <summary>
		/// Stream that original content is read into
		/// and then passed to TransformStream function
		/// </summary>
		private readonly MemoryStream _cacheStream = new MemoryStream();

		/// <summary>
		/// WebMarkupMin configuration
		/// </summary>
		private readonly WebMarkupMinConfiguration _configuration;

		/// <summary>
		/// Markup minification manager
		/// </summary>
		private readonly IMarkupMinificationManager _minificationManager;

		/// <summary>
		/// Current URL
		/// </summary>
		private readonly string _currentUrl;

		/// <summary>
		/// Text encoding
		/// </summary>
		private readonly Encoding _encoding;

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanSeek
		{
			get { return true; }
		}

		public override bool CanWrite
		{
			get { return true; }
		}

		public override long Length
		{
			get { return 0; }
		}

		public override long Position
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs a instance of markup minification response filter
		/// </summary>
		/// <param name="response">HTTP-response</param>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="minificationManager">Markup minification manager</param>
		public MarkupMinificationFilterStream(HttpResponseBase response,
			WebMarkupMinConfiguration configuration,
			IMarkupMinificationManager minificationManager)
			: this(response, configuration, minificationManager, string.Empty, Encoding.Default)
		{ }

		/// <summary>
		/// Constructs a instance of markup minification response filter
		/// </summary>
		/// <param name="response">HTTP-response</param>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="minificationManager">Markup minification manager</param>
		/// <param name="currentUrl">Current URL</param>
		/// <param name="encoding">Text encoding</param>
		public MarkupMinificationFilterStream(HttpResponseBase response,
			WebMarkupMinConfiguration configuration,
			IMarkupMinificationManager minificationManager,
			string currentUrl,
			Encoding encoding)
		{
			_response = response;
			_stream = response.Filter;
			_configuration = configuration;
			_minificationManager = minificationManager;
			_currentUrl = currentUrl;
			_encoding = encoding;
		}


		public override int Read(byte[] buffer, int offset, int count)
		{
			return _stream.Read(buffer, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return _stream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			_stream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			_cacheStream.Write(buffer, 0, count);
		}

		public override void Flush()
		{
			_stream.Flush();
		}

		public override void Close()
		{
			byte[] cacheBytes = _cacheStream.ToArray();
			int cacheSize = cacheBytes.Length;

			bool isMinified = false;

			if (_configuration.IsAllowableResponseSize(cacheSize))
			{
				string content = _encoding.GetString(cacheBytes);
				IMarkupMinifier minifier = _minificationManager.CreateMinifier();

				MarkupMinificationResult minificationResult = minifier.Minify(content,
					_currentUrl, _encoding, false);
				if (minificationResult.Errors.Count == 0)
				{
					if (_configuration.IsPoweredByHttpHeadersEnabled())
					{
						_minificationManager.AppendPoweredByHttpHeader((key, value) =>
						{
							_response.Headers[key] = value;
						});
					}

					byte[] output = _encoding.GetBytes(minificationResult.MinifiedContent);
					_stream.Write(output, 0, output.GetLength(0));

					isMinified = true;
				}
			}

			if (!isMinified)
			{
				_stream.Write(cacheBytes, 0, cacheSize);
			}

			_cacheStream.SetLength(0);
			_stream.Close();
		}
	}
}