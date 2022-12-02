using System;
using System.Web;

namespace WebMarkupMin.Sample.AspNet4.WebPages2
{
	public static class UrlHelper
	{
		public static Uri GetSiteUrl()
		{
			HttpRequest request = HttpContext.Current.Request;
			Uri currrentUrl = request.Url;

			var uriBuilder = new UriBuilder();
			uriBuilder.Scheme = currrentUrl.Scheme;
			uriBuilder.Host = currrentUrl.Host;
			if (!currrentUrl.IsDefaultPort)
			{
				uriBuilder.Port = currrentUrl.Port;
			}

			return uriBuilder.Uri;
		}

		public static Uri GetAbsolutePageUrl(Uri siteUrl, string relativeUrl)
		{
			var absoluteUrl = new Uri(siteUrl, relativeUrl);

			return absoluteUrl;
		}
	}
}