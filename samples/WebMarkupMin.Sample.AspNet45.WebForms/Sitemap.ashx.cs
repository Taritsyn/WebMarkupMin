using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;

using WebMarkupMin.Sample.Logic.Models;

namespace WebMarkupMin.Sample.AspNet45.WebForms
{
	public class Sitemap : IHttpHandler
	{
		private HttpContext _context;

		public bool IsReusable
		{
			get { return false; }
		}


		public void ProcessRequest(HttpContext context)
		{
			_context = context;

			const string contentType = "text/xml";
			Encoding contentEncoding = Encoding.UTF8;

			IList<SitemapItem> items = GetSitemapItems();
			var sitemap = new WebMarkupMin.Sample.Logic.Models.Sitemap(items);

			var response = context.Response;
			response.ContentType = contentType;
			response.ContentEncoding = contentEncoding;

			var settings = new XmlWriterSettings()
			{
				Encoding = contentEncoding,
				// Add indents in order to test the XML minification
				Indent = true
			};

			using (XmlWriter writer = XmlWriter.Create(response.Output, settings))
			{
				sitemap.GetXmlSitemapFormatter().WriteXml(writer);
			}
		}

		private IList<SitemapItem> GetSitemapItems()
		{
			Uri siteUrl = GetSiteUrl();
			var sitemapItems = new List<SitemapItem>
			{
				new SitemapItem(GetAbsolutePageUrl(siteUrl, "/"), null, SitemapChangeFrequency.Hourly, 0.9),
				new SitemapItem(GetAbsolutePageUrl(siteUrl, "/ChangeLog"), null, SitemapChangeFrequency.Daily, 0.8),
				new SitemapItem(GetAbsolutePageUrl(siteUrl, "/Contact"), null, SitemapChangeFrequency.Weekly, 0.4)
			};

			return sitemapItems;
		}

		private Uri GetSiteUrl()
		{
			HttpRequest request = _context.Request;
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

		private Uri GetAbsolutePageUrl(Uri siteUrl, string relativeUrl)
		{
			var absoluteUrl = new Uri(siteUrl, relativeUrl);

			return absoluteUrl;
		}
	}
}