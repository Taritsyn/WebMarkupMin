using System;
using System.Collections.Generic;

namespace WebMarkupMin.Sample.Logic.Models
{
	/// <summary>
	/// Represents a sitemap
	/// </summary>
	public sealed class Sitemap
	{
		/// <summary>
		/// Gets a collection of the items contained in the sitemap
		/// </summary>
		public IList<SitemapItem> Items
		{
			get;
			private set;
		}


		/// <summary>
		/// Constructs an instance of the <see cref="Sitemap" /> class with the specified collection of
		/// <see cref="SitemapItem" /> objects
		/// </summary>
		/// <param name="items">A collection of <see cref="SitemapItem" /> objects</param>
		public Sitemap(IList<SitemapItem> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException(nameof(items));
			}

			Items = items;
		}


		/// <summary>
		/// Gets an <see cref="XmlSitemapFormatter" /> instance
		/// </summary>
		/// <returns>An <see cref="XmlSitemapFormatter" /> instance</returns>
		public XmlSitemapFormatter GetXmlSitemapFormatter()
		{
			return new XmlSitemapFormatter(this);
		}
	}
}