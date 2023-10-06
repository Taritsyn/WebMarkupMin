using System.Collections.Generic;

using WebMarkupMin.AspNet.Common.UrlMatchers;

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2
#elif ASPNETCORE3
namespace WebMarkupMin.AspNetCore3
#elif ASPNETCORE5
namespace WebMarkupMin.AspNetCore5
#elif ASPNETCORE6
namespace WebMarkupMin.AspNetCore6
#elif ASPNETCORE7
namespace WebMarkupMin.AspNetCore7
#elif ASPNETCORE8
namespace WebMarkupMin.AspNetCore8
#else
#error No implementation for this target
#endif
{
	/// <summary>
	/// Base class of content processing options
	/// </summary>
	public abstract class ContentProcessingOptionsBase
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
		/// Constructs a instance of content processing options
		/// </summary>
		protected ContentProcessingOptionsBase()
		{
			SupportedHttpStatusCodes = new HashSet<int> { 200 };
			SupportedHttpMethods = new HashSet<string> { "GET" };
			IncludedPages = new List<IUrlMatcher>();
			ExcludedPages = new List<IUrlMatcher>();
		}
	}
}