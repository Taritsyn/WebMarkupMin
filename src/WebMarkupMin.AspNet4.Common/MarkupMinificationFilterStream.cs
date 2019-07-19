using System;
using System.IO;
using System.Text;
using System.Web;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet4.Common.Helpers;
using WebMarkupMin.Core;
using WebMarkupMin.Core.Utilities;
using AspNetCommonStrings = WebMarkupMin.AspNet.Common.Resources.Strings;

namespace WebMarkupMin.AspNet4.Common
{
	/// <summary>
	/// Markup minification response filter
	/// </summary>
	public sealed class MarkupMinificationFilterStream : Stream
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
		/// Stream that original content is read into
		/// </summary>
		private MemoryStream _cachedStream = new MemoryStream();

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
		/// <param name="response">HTTP response</param>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="minificationManager">Markup minification manager</param>
		public MarkupMinificationFilterStream(HttpResponseBase response,
			WebMarkupMinConfiguration configuration,
			IMarkupMinificationManager minificationManager)
			: this(response, configuration, minificationManager, string.Empty, TextEncodingShortcuts.Default)
		{ }

		/// <summary>
		/// Constructs a instance of markup minification response filter
		/// </summary>
		/// <param name="response">HTTP response</param>
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
			_originalStream = response.Filter;
			_configuration = configuration;
			_minificationManager = minificationManager;
			_currentUrl = currentUrl;
			_encoding = encoding;
		}


		public override int Read(byte[] buffer, int offset, int count)
		{
			return _originalStream.Read(buffer, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return _originalStream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			_originalStream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			_cachedStream.Write(buffer, 0, count);
		}

		public override void Flush()
		{
			_originalStream.Flush();
		}

		public override void Close()
		{
			bool isEncodedContent = HttpHeadersHelpers.IsEncodedContent(_response.Headers);
			if (!isEncodedContent)
			{
				byte[] cachedBytes = _cachedStream.ToArray();
				int cachedByteCount = cachedBytes.Length;

				bool isMinified = false;

				if (_configuration.IsAllowableResponseSize(cachedByteCount))
				{
					string content = _encoding.GetString(cachedBytes);
					IMarkupMinifier minifier = _minificationManager.CreateMinifier();

					MarkupMinificationResult minificationResult = minifier.Minify(content,
						_currentUrl, _encoding, _minificationManager.GenerateStatistics);
					if (minificationResult.Errors.Count == 0)
					{
						if (_configuration.IsPoweredByHttpHeadersEnabled())
						{
							_minificationManager.AppendPoweredByHttpHeader((key, value) =>
							{
								_response.Headers[key] = value;
							});
						}

						using (var writer = new StreamWriter(_originalStream, _encoding))
						{
							writer.Write(minificationResult.MinifiedContent);
						}

						isMinified = true;
					}
				}

				if (!isMinified)
				{
					_cachedStream.Seek(0, SeekOrigin.Begin);
					_cachedStream.CopyTo(_originalStream);
				}
			}

			_cachedStream.Clear();
			_originalStream.Close();

			if (isEncodedContent)
			{
				throw new InvalidOperationException(
					string.Format(
						AspNetCommonStrings.MarkupMinificationIsNotApplicableToEncodedContent,
						_response.Headers["Content-Encoding"]
					)
				);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_cachedStream != null)
				{
					_cachedStream.Dispose();
					_cachedStream = null;
				}

				_originalStream.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}