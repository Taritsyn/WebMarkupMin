using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Web;

using WebMarkupMin.Sample.Logic.Models;
using WebMarkupMin.Sample.Logic.Services;

namespace WebMarkupMin.Sample.AspNet45.WebForms
{
	public class Sitemap : IHttpHandler
	{
		private readonly SitemapService _sitemapService;

		private HttpContext _context;

		public bool IsReusable
		{
			get { return false; }
		}


		public Sitemap() : this(new SitemapService())
		{ }

		public Sitemap(SitemapService sitemapService)
		{
			_sitemapService = sitemapService;
		}


		public void ProcessRequest(HttpContext context)
		{
			_context = context;

			var response = context.Response;
			response.ContentType = "text/xml";
			response.ContentEncoding = Encoding.UTF8;

			using (var writer = new XmlTextWriter(response.Output))
			{
				// Add indents in order to test the XML minification
				writer.Formatting = Formatting.Indented;

				IList<SitemapItem> items = GetSitemapItems();

				XDocument sitemap = _sitemapService.GenerateXmlSiteMap(items);
				sitemap.WriteTo(writer);
			}
		}

		private IList<SitemapItem> GetSitemapItems()
		{
			var sitemapItems = new List<SitemapItem>
			{
				new SitemapItem(GetAbsoluteUrl("/"), null, SitemapChangeFrequency.Hourly, 0.9),
				new SitemapItem(GetAbsoluteUrl("/ChangeLog"), null, SitemapChangeFrequency.Daily, 0.8),
				new SitemapItem(GetAbsoluteUrl("/Contact"), null, SitemapChangeFrequency.Weekly, 0.4)
			};

			return sitemapItems;
		}

		private string GetAbsoluteUrl(string relativeUrl)
		{
			HttpRequest request = _context.Request;

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