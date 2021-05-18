using System.Collections.Generic;

using WebMarkupMin.AspNet.Common.UrlMatchers;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Defines a interface of content processing manager
	/// </summary>
	public interface IContentProcessingManager
	{
		/// <summary>
		/// Gets or sets a list of supported HTTP status codes
		/// </summary>
		ISet<int> SupportedHttpStatusCodes
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a list of supported HTTP methods
		/// </summary>
		ISet<string> SupportedHttpMethods
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a list of URL matchers, which is used to include pages to processing
		/// by markup minifier or compressor
		/// </summary>
		IList<IUrlMatcher> IncludedPages
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a list of URL matchers, which is used to exclude pages from processing
		/// by markup minifier or compressor
		/// </summary>
		IList<IUrlMatcher> ExcludedPages
		{
			get;
			set;
		}
	}
}