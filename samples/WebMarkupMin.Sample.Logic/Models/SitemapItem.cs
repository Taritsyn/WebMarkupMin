using System;

namespace WebMarkupMin.Sample.Logic.Models
{
	/// <summary>
	/// Represents a sitemap item
	/// </summary>
	public sealed class SitemapItem
	{
		/// <summary>
		/// Gets a URI of the page
		/// </summary>
		public Uri Url
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a date of last modification of the file
		/// </summary>
		public DateTime? LastModified
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a how frequently the page is likely to change
		/// </summary>
		public SitemapChangeFrequency? ChangeFrequency
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a priority of this URL relative to other URLs on your site.
		/// Valid values range from 0.0 to 1.0.
		/// </summary>
		public double? Priority
		{
			get;
			private set;
		}


		/// <summary>
		/// Constructs an instance of the <see cref="SitemapItem"/>
		/// </summary>
		/// <param name="url">URI of the page</param>
		/// <param name="lastModified">The date of last modification of the file</param>
		/// <param name="changeFrequency">How frequently the page is likely to change</param>
		/// <param name="priority">The priority of this URL relative to other URLs on your site</param>
		public SitemapItem(Uri url, DateTime? lastModified = null, SitemapChangeFrequency? changeFrequency = null,
			double? priority = null)
		{
			Url = url;
			LastModified = lastModified;
			ChangeFrequency = changeFrequency;
			Priority = priority;
		}
	}
}