using System.Collections.Generic;

using WebMarkupMin.AspNet.Common.UrlMatchers;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Base class of content processing manager
	/// </summary>
	public abstract class ContentProcessingManagerBase : IContentProcessingManager
	{
		/// <summary>
		/// Gets or sets a list of supported HTTP status codes
		/// </summary>
		public ISet<int> SupportedHttpStatusCodes
		{
			get;
			set;
		}

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
		/// by markup minifier or compressor
		/// </summary>
		public IList<IUrlMatcher> IncludedPages
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a list of URL matchers, which is used to exclude pages from processing
		/// by markup minifier or compressor
		/// </summary>
		public IList<IUrlMatcher> ExcludedPages
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs a instance of content processing manager
		/// </summary>
		protected ContentProcessingManagerBase()
		{
			SupportedHttpStatusCodes = new HashSet<int> { 200 };
			SupportedHttpMethods = new HashSet<string> { "GET" };
			IncludedPages = new List<IUrlMatcher>();
			ExcludedPages = new List<IUrlMatcher>();
		}
	}
}