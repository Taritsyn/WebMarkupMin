using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WebMarkupMin.Sample.Logic.Models
{
	/// <summary>
	/// A class that serializes a <see cref="Sitemap" /> instance to XML sitemap format
	/// </summary>
	public sealed class XmlSitemapFormatter : IXmlSerializable
	{
		/// <summary>
		/// Sitemap namespace (protocol standard)
		/// </summary>
		private const string SitemapNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";

		/// <summary>
		/// Gets a <see cref="Sitemap" /> associated with the formatter
		/// </summary>
		public Sitemap Sitemap
		{
			get;
			private set;
		}


		/// <summary>
		/// Constructs an instance of the <see cref="XmlSitemapFormatter" /> class with the specified <see cref="Sitemap" /> instance
		/// </summary>
		/// <param name="sitemap">The <see cref="Sitemap" /> to serialize</param>
		public XmlSitemapFormatter(Sitemap sitemap)
		{
			if (sitemap is null)
			{
				throw new ArgumentNullException(nameof(sitemap));
			}

			Sitemap = sitemap;
		}


		#region IXmlSerializable implementation

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Writes the <see cref="Sitemap" /> to the specified <see cref="XmlWriter" /> instance
		/// </summary>
		/// <param name="writer">The <see cref="XmlWriter" /> to write the <see cref="Sitemap" /> to</param>
		public void WriteXml(XmlWriter writer)
		{
			if (writer is null)
			{
				throw new ArgumentNullException(nameof(writer));
			}

			writer.WriteStartDocument();

			writer.WriteStartElement("urlset", SitemapNamespace);

			foreach (SitemapItem item in Sitemap.Items)
			{
				writer.WriteStartElement("url");

				writer.WriteElementString("loc", item.Url.ToString());
				if (item.LastModified.HasValue)
				{
					writer.WriteStartElement("lastmod");
					writer.WriteValue(item.LastModified.Value.ToUniversalTime());
					writer.WriteEndElement();
				}
				if (item.ChangeFrequency.HasValue)
				{
					writer.WriteElementString("changefreq", item.ChangeFrequency.Value.ToString().ToLower());
				}
				if (item.Priority.HasValue)
				{
					writer.WriteElementString("priority", item.Priority.Value.ToString("F1", CultureInfo.InvariantCulture));
				}

				writer.WriteEndElement();
			}

			writer.WriteEndElement();

			writer.WriteEndDocument();
		}

		#endregion
	}
}