using System;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebMarkupMin.Sample.AspNetCore.Infrastructure.Extensions
{
	public static class ControllerExtensions
	{
		public static Uri GetSiteUrl(this Controller controller)
		{
			HttpRequest request = controller.HttpContext.Request;
			HostString host = request.Host;

			var uriBuilder = new UriBuilder();
			uriBuilder.Scheme = request.Scheme;
			uriBuilder.Host = host.Host;
			if (host.Port.HasValue)
			{
				uriBuilder.Port = host.Port.Value;
			}

			return uriBuilder.Uri;
		}

		public static Uri GetAbsoluteActionUrl(this Controller controller, Uri siteUrl, string controllerName, string actionName)
		{
			string relativeUrl = controller.Url.Action(actionName, controllerName);
			var absoluteUrl = new Uri(siteUrl, relativeUrl);

			return absoluteUrl;
		}
	}
}