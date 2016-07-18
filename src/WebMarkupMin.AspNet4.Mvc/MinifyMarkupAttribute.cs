using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet4.Common;

namespace WebMarkupMin.AspNet4.Mvc
{
	/// <summary>
	/// Base class of attribute, that applies markup minification to the action result
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public abstract class MinifyMarkupAttribute : ActionFilterAttribute
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
		/// Constructs a instance of markup minification attribute
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="minificationManager">Markup minification manager</param>
		protected MinifyMarkupAttribute(
			WebMarkupMinConfiguration configuration,
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

		public override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			if (!_configuration.IsMinificationEnabled())
			{
				return;
			}

			IMarkupMinificationManager minificationManager =
				_minificationManager ?? GetDefaultMinificationManager();
			HttpContextBase context = filterContext.HttpContext;
			HttpRequestBase request = context.Request;
			HttpResponseBase response = context.Response;
			Encoding encoding = response.ContentEncoding;
			string mediaType = response.ContentType;
			string currentUrl = request.RawUrl;

			if (response.Filter != null
				&& response.StatusCode == 200
				&& minificationManager.IsSupportedMediaType(mediaType)
				&& minificationManager.IsProcessablePage(currentUrl))
			{
				response.Filter = new MarkupMinificationFilterStream(response, _configuration,
					minificationManager, currentUrl, encoding);
			}
		}
	}
}