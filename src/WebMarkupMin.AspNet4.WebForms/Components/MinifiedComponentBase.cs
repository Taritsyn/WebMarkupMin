using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet4.Common;
using WebMarkupMin.Core;

namespace WebMarkupMin.AspNet4.WebForms.Components
{
	/// <summary>
	/// Base class of minified component
	/// </summary>
	public abstract class MinifiedComponentBase
	{
		/// <summary>
		/// WebMarkupMin configuration
		/// </summary>
		private readonly WebMarkupMinConfiguration _configuration;

		/// <summary>
		/// Markup minification manager
		/// </summary>
		private readonly IMarkupMinificationManager _minificationManager;

		/// <summary>
		/// Flag for whether to disable markup minification
		/// </summary>
		private bool _disableMinification;
		private bool _disableMinificationSet;

		/// <summary>
		/// Gets or sets a flag for whether to disable markup minification
		/// </summary>
		public bool DisableMinification
		{
			get
			{
				if (!_disableMinificationSet)
				{
					return !_configuration.IsMinificationEnabled();
				}

				return _disableMinification;
			}
			set
			{
				_disableMinification = value;
				_disableMinificationSet = true;
			}
		}


		/// <summary>
		/// Constructs a instance of minified component
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="minificationManager">Markup minification manager</param>
		protected MinifiedComponentBase(WebMarkupMinConfiguration configuration,
			IMarkupMinificationManager minificationManager)
		{
			_configuration = configuration;
			_minificationManager = minificationManager;
		}


		/// <summary>
		/// Gets a instance of default markup minification manager
		/// </summary>
		/// <returns>Instance of default markup minification manager</returns>
		protected abstract IMarkupMinificationManager GetDefaultMinificationManager();

		public void Render(HtmlTextWriter writer, Action<HtmlTextWriter> renderHandler)
		{
			if (!DisableMinification)
			{
				IMarkupMinificationManager minificationManager =
					_minificationManager ?? GetDefaultMinificationManager();
				HttpContext context = HttpContext.Current;
				HttpResponse response = context.Response;
				HttpRequest request = context.Request;

				using (var htmlwriter = new HtmlTextWriter(new StringWriter()))
				{
					renderHandler(htmlwriter);

					string content = htmlwriter.InnerWriter.ToString();
					bool isMinified = false;

					Encoding encoding = response.ContentEncoding;
					int responseSize = encoding.GetByteCount(content);
					string httpMethod = request.HttpMethod;
					string mediaType = response.ContentType;
					string currentUrl = request.RawUrl;

					if (minificationManager.IsSupportedHttpStatusCode(response.StatusCode)
						&& _configuration.IsAllowableResponseSize(responseSize)
						&& minificationManager.IsSupportedHttpMethod(httpMethod)
						&& minificationManager.IsSupportedMediaType(mediaType)
						&& minificationManager.IsProcessablePage(currentUrl))
					{
						IMarkupMinifier minifier = minificationManager.CreateMinifier();
						MarkupMinificationResult minificationResult = minifier.Minify(content,
							currentUrl, encoding, minificationManager.GenerateStatistics);
						if (minificationResult.Errors.Count == 0)
						{
							if (_configuration.IsPoweredByHttpHeadersEnabled())
							{
								minificationManager.AppendPoweredByHttpHeader((key, value) =>
								{
									response.Headers[key] = value;
								});
							}

							writer.Write(minificationResult.MinifiedContent);

							isMinified = true;
						}
					}

					if (!isMinified)
					{
						writer.Write(content);
					}
				}
			}
			else
			{
				renderHandler(writer);
			}
		}
	}
}