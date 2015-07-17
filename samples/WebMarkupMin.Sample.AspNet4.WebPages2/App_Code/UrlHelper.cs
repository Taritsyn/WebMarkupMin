using System;
using System.Web;

namespace WebMarkupMin.Sample.AspNet4.WebPages2
{
	public static class UrlHelper
	{
		public static string GetAbsoluteUrl(string relativeUrl)
		{
			HttpRequest request = HttpContext.Current.Request;

			string absoluteUrl = string.Empty;
			if (relativeUrl != null)
			{
				Uri baseUri = request.Url;
				Uri absoluteUri = new Uri(baseUri, relativeUrl);
				absoluteUrl = absoluteUri.Scheme + Uri.SchemeDelimiter +
					absoluteUri.Host +
					(absoluteUri.IsDefaultPort ? string.Empty : ":" + absoluteUri.Port) +
					absoluteUri.PathAndQuery;
			}

			return absoluteUrl;
		}
	}
}