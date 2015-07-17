using System;

namespace WebMarkupMin.Sample.Logic.Models
{
	/// <summary>
	/// Sitemap item
	/// </summary>
	public sealed class SitemapItem
	{
		/// <summary>
		/// Gets a URL of the page
		/// </summary>
		public string Url
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
		/// Constructs instance of <see cref="SitemapItem"/>
		/// </summary>
		/// <param name="url">URL of the page</param>
		/// <param name="lastModified">The date of last modification of the file</param>
		/// <param name="changeFrequency">How frequently the page is likely to change</param>
		/// <param name="priority">The priority of this URL relative to other URLs on your site</param>
		public SitemapItem(string url, DateTime? lastModified = null, SitemapChangeFrequency? changeFrequency = null,
			double? priority = null)
		{
			Url = url;
			LastModified = lastModified;
			ChangeFrequency = changeFrequency;
			Priority = priority;
		}
	}
}