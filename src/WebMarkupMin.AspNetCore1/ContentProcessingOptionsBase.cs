using System.Collections.Generic;

using WebMarkupMin.AspNet.Common.UrlMatchers;

namespace WebMarkupMin.AspNetCore1
{
	/// <summary>
	/// Base class of content processing options
	/// </summary>
	public abstract class ContentProcessingOptionsBase
	{
		/// <summary>
		/// Gets or sets a list of supported HTTP methods
		/// </summary>
		public ISet<string> SupportedHttpMethods
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a list of URL matchers, which is used to include pages to processing
		/// by markup minifier
		/// </summary>
		public IList<IUrlMatcher> IncludedPages
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a list of URL matchers, which is used to exclude pages from processing
		/// by markup minifier
		/// </summary>
		public IList<IUrlMatcher> ExcludedPages
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs a instance of content processing options
		/// </summary>
		protected ContentProcessingOptionsBase()
		{
			SupportedHttpMethods = new HashSet<string> { "GET" };
			IncludedPages = new List<IUrlMatcher>();
			ExcludedPages = new List<IUrlMatcher>();
		}
	}
}