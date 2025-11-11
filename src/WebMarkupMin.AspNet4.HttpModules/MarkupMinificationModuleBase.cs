using System;
using System.Text;
using System.Web;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet4.Common;

namespace WebMarkupMin.AspNet4.HttpModules
{
	/// <summary>
	/// Base class of HTTP module for markup minification
	/// </summary>
	public abstract class MarkupMinificationModuleBase : IHttpModule
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
		/// Constructs a instance of HTTP module for markup minification
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="minificationManager">Markup minification manager</param>
		protected MarkupMinificationModuleBase(
			WebMarkupMinConfiguration configuration,
			IMarkupMinificationManager minificationManager)
		{
			_configuration = configuration;
			_minificationManager = minificationManager;
		}

		/// <summary>
		/// Initializes a module and prepares it to handle requests.
		/// </summary>
		/// <param name="context">HTTP context</param>
		public void Init(HttpApplication context)
		{
			context.PostRequestHandlerExecute += ProcessResponse;
		}

		/// <summary>
		/// Gets a instance of default markup minification manager
		/// </summary>
		/// <returns>Instance of default markup minification manager</returns>
		protected abstract IMarkupMinificationManager GetDefaultMinificationManager();

		/// <summary>
		/// Processes the response and sets a XML minification response filter
		/// </summary>
		/// <param name="sender">The source of the event (HTTP application)</param>
		/// <param name="e">An System.EventArgs that contains no event data</param>
		private void ProcessResponse(object sender, EventArgs e)
		{
			if (!_configuration.IsMinificationEnabled())
			{
				return;
			}

			IMarkupMinificationManager minificationManager =
				_minificationManager ?? GetDefaultMinificationManager();
			HttpContext context = ((HttpApplication) sender).Context;
			HttpRequest request = context.Request;
			HttpResponse response = context.Response;
			Encoding encoding = response.ContentEncoding;
			string httpMethod = request.HttpMethod;
			string mediaType = response.ContentType;
			string currentUrl = request.RawUrl;

			if (context.CurrentHandler is not null
				&& minificationManager.IsSupportedHttpStatusCode(response.StatusCode)
				&& minificationManager.IsSupportedHttpMethod(httpMethod)
				&& minificationManager.IsSupportedMediaType(mediaType)
				&& minificationManager.IsProcessablePage(currentUrl))
			{
				response.Filter = new MarkupMinificationFilterStream(new HttpResponseWrapper(response),
					_configuration, minificationManager, currentUrl, encoding);
			}
		}

		/// <summary>
		/// Destroys object
		/// </summary>
		public void Dispose()
		{
			// Nothing to destroy
		}
	}
}