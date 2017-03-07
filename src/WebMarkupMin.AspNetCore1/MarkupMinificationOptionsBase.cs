using System.Collections.Generic;

using WebMarkupMin.AspNet.Common.UrlMatchers;

namespace WebMarkupMin.AspNetCore1
{
	/// <summary>
	/// Base class of markup minification options
	/// </summary>
	/// <typeparam name="TSettings">The type of markup minification settings</typeparam>
	public abstract class MarkupMinificationOptionsBase<TSettings>
		where TSettings : class, new()
	{
		/// <summary>
		/// Gets or sets a markup minification settings used to configure the HTML minifier
		/// </summary>
		public virtual TSettings MinificationSettings
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a list of supported media-types
		/// </summary>
		public virtual ISet<string> SupportedMediaTypes
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
		/// Gets or sets a flag for whether to allow generate minification statistics
		/// (available through the logger)
		/// </summary>
		public bool GenerateStatistics
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs a instance of markup minification options
		/// </summary>
		protected MarkupMinificationOptionsBase()
		{
			IncludedPages = new List<IUrlMatcher>();
			ExcludedPages = new List<IUrlMatcher>();
		}
	}
}