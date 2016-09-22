using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet.Common.Compressors;
using WebMarkupMin.Core;
using AspNetCommonStrings = WebMarkupMin.AspNet.Common.Resources;

namespace WebMarkupMin.AspNetCore1
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
				throw new ArgumentNullException("next");
			}

			if (options == null)
			{
				throw new ArgumentNullException("options");
			}

			if (services == null)
			{
				throw new ArgumentNullException("services");
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

			HttpRequest request = context.Request;
			HttpResponse response = context.Response;

			using (var cacheStream = new MemoryStream())
			{
				Stream originalStream = response.Body;
				response.Body = cacheStream;

				try
				{
					await _next.Invoke(context);
				}
				catch (Exception)
				{
					response.Body = originalStream;
					cacheStream.SetLength(0);

					throw;
				}

				byte[] cacheBytes = cacheStream.ToArray();
				int cacheSize = cacheBytes.Length;
				bool isProcessed = false;

				response.Body = originalStream;

				if (request.Method == "GET" && response.StatusCode == 200
					&& _options.IsAllowableResponseSize(cacheSize))
				{
					string contentType = response.ContentType;
					string mediaType = null;
					Encoding encoding = null;

					if (contentType != null)
					{
						MediaTypeHeaderValue mediaTypeHeader;

						if (MediaTypeHeaderValue.TryParse(contentType, out mediaTypeHeader))
						{
							mediaType = mediaTypeHeader.MediaType.ToLowerInvariant();
							encoding = mediaTypeHeader.Encoding;
						}
					}

					encoding = encoding ?? Encoding.GetEncoding(0);

					string currentUrl = request.Path.Value;
					QueryString queryString = request.QueryString;
					if (queryString.HasValue)
					{
						currentUrl += queryString.Value;
					}

					string content = encoding.GetString(cacheBytes);
					string processedContent = content;
					bool isEncodedContent = response.Headers.IsEncodedContent();
					Action<string, string> appendHttpHeader = (key, value) =>
					{
						response.Headers.Append(key, new StringValues(value));
					};

					if (useMinification)
					{
						foreach (IMarkupMinificationManager minificationManager in _minificationManagers)
						{
							if (mediaType != null && minificationManager.IsSupportedMediaType(mediaType)
								&& minificationManager.IsProcessablePage(currentUrl))
							{
								if (isEncodedContent)
								{
									throw new InvalidOperationException(
										string.Format(
											AspNetCommonStrings.MarkupMinificationIsNotApplicableToEncodedContent,
											response.Headers["Content-Encoding"]
										)
									);
								}

								IMarkupMinifier minifier = minificationManager.CreateMinifier();

								MarkupMinificationResult minificationResult = minifier.Minify(processedContent, currentUrl, encoding, false);
								if (minificationResult.Errors.Count == 0)
								{
									processedContent = minificationResult.MinifiedContent;
									if (_options.IsPoweredByHttpHeadersEnabled())
									{
										minificationManager.AppendPoweredByHttpHeader(appendHttpHeader);
									}

									isProcessed = true;
								}
							}

							if (isProcessed)
							{
								break;
							}
						}
					}

					if (useCompression && !isEncodedContent
						&& _compressionManager.IsSupportedMediaType(mediaType))
					{
						string acceptEncoding = request.Headers["Accept-Encoding"];

						ICompressor compressor = _compressionManager.CreateCompressor(acceptEncoding);
						Stream compressedStream = compressor.Compress(originalStream);
						compressor.AppendHttpHeaders(appendHttpHeader);

						using (var writer = new StreamWriter(compressedStream, encoding))
						{
							await writer.WriteAsync(processedContent);
						}

						isProcessed = true;
					}
					else
					{
						if (isProcessed)
						{
							using (var writer = new StreamWriter(originalStream, encoding))
							{
								await writer.WriteAsync(processedContent);
							}
						}
					}
				}

				if (!isProcessed)
				{
					cacheStream.Seek(0, SeekOrigin.Begin);
					await cacheStream.CopyToAsync(originalStream);
				}

				cacheStream.SetLength(0);
			}
		}
	}
}