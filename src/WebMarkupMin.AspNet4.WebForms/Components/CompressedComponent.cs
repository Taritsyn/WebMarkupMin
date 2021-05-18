using System;
using System.IO;
using System.Web;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet4.Common;

namespace WebMarkupMin.AspNet4.WebForms.Components
{
	/// <summary>
	/// Compressed component
	/// </summary>
	public sealed class CompressedComponent
	{
		/// <summary>
		/// WebMarkupMin configuration
		/// </summary>
		private readonly WebMarkupMinConfiguration _configuration;

		/// <summary>
		/// HTTP compression manager
		/// </summary>
		private readonly IHttpCompressionManager _compressionManager;

		/// <summary>
		/// Flag for whether to disable HTTP compression of content
		/// </summary>
		private bool _disableCompression;
		private bool _disableCompressionSet;

		/// <summary>
		/// Gets or sets a flag for whether to disable HTTP compression of content
		/// </summary>
		public bool DisableCompression
		{
			get
			{
				if (!_disableCompressionSet)
				{
					return !_configuration.IsCompressionEnabled();
				}

				return _disableCompression;
			}
			set
			{
				_disableCompression = value;
				_disableCompressionSet = true;
			}
		}


		/// <summary>
		/// Constructs a instance of compressed component
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="compressionManager">HTTP compression manager</param>
		public CompressedComponent(WebMarkupMinConfiguration configuration,
			IHttpCompressionManager compressionManager)
		{
			_configuration = configuration;
			_compressionManager = compressionManager;
		}


		public void OnLoad(EventArgs e)
		{
			if (DisableCompression)
			{
				return;
			}

			IHttpCompressionManager compressionManager =
				_compressionManager ?? HttpCompressionManager.Current;
			HttpContext context = HttpContext.Current;
			HttpRequest request = context.Request;
			HttpResponse response = context.Response;
			string httpMethod = request.HttpMethod;
			string currentUrl = request.RawUrl;

			if (compressionManager.IsSupportedHttpStatusCode(response.StatusCode)
				&& compressionManager.IsSupportedHttpMethod(httpMethod)
				&& compressionManager.IsProcessablePage(currentUrl))
			{
				context.Items["originalResponseFilter"] = response.Filter;

				string acceptEncoding = request.Headers["Accept-Encoding"];
				response.Filter = new HttpCompressionFilterStream(new HttpResponseWrapper(response),
					compressionManager, acceptEncoding);
			}
		}

		public void OnError(EventArgs e)
		{
			HttpContext context = HttpContext.Current;
			if (context.Items.Contains("originalResponseFilter"))
			{
				var originalResponseFilter = context.Items["originalResponseFilter"] as Stream;
				if (originalResponseFilter != null)
				{
					context.Response.Filter = originalResponseFilter;
				}
			}
		}
	}
}