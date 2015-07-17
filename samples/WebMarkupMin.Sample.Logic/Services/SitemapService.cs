using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

using WebMarkupMin.Sample.Logic.Models;

namespace WebMarkupMin.Sample.Logic.Services
{
	public sealed class SitemapService
	{
		private static readonly XNamespace _sitemapNs = "http://www.sitemaps.org/schemas/sitemap/0.9";


		/// <summary>
		/// Generates a XML Sitemap
		/// </summary>
		/// <param name="items">List of the sitemap item</param>
		/// <returns>XML Sitemap</returns>
		public XDocument GenerateXmlSiteMap(IEnumerable<SitemapItem> items)
		{
			var sitemap = new XDocument(
				new XDeclaration("1.0", "utf-8", "yes"),
				new XElement(_sitemapNs + "urlset", items.Select(CreateItemElement))
			);

			return sitemap;
		}

		private static XElement CreateItemElement(SitemapItem item)
		{
			var itemElem = new XElement(_sitemapNs + "url", new XElement(_sitemapNs + "loc", item.Url.ToLowerInvariant()));

			if (item.LastModified.HasValue)
			{
				itemElem.Add(new XElement(_sitemapNs + "lastmod", item.LastModified.Value
					.ToUniversalTime()
					.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")));
			}

			if (item.ChangeFrequency.HasValue)
			{
				itemElem.Add(new XElement(_sitemapNs + "changefreq", item.ChangeFrequency.Value.ToString().ToLower()));
			}

			if (item.Priority.HasValue)
			{
				itemElem.Add(new XElement(_sitemapNs + "priority", item.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)));
			}

			return itemElem;
		}
	}
}