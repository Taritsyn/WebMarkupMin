using System.Collections.Generic;

using WebMarkupMin.AspNet.Common.UrlMatchers;
using WebMarkupMin.Core;
using WebMarkupMin.Core.Loggers;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Base class of markup minification manager
	/// </summary>
	/// <typeparam name="TSettings">The type of markup minification settings</typeparam>
	public abstract class MarkupMinificationManagerBase<TSettings> : IMarkupMinificationManager<TSettings>
		where TSettings : class, new()
	{
		/// <summary>
		/// Gets or sets a logger
		/// </summary>
		protected virtual ILogger Logger
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a markup minification settings used to configure the HTML minifier
		/// </summary>
		public TSettings MinificationSettings
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
		/// Gets or sets a list of supported media-types
		/// </summary>
		public ISet<string> SupportedMediaTypes
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
		/// Gets a <code>*-Minification-Powered-By</code> HTTP-header
		/// </summary>
		public KeyValuePair<string, string> PoweredByHttpHeader
		{
			get;
			protected set;
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
		/// Constructs a instance of markup minification manager
		/// </summary>
		protected MarkupMinificationManagerBase()
		{
			SupportedHttpMethods = new HashSet<string> { "GET" };
			IncludedPages = new List<IUrlMatcher>();
			ExcludedPages = new List<IUrlMatcher>();
			GenerateStatistics = false;
		}


		/// <summary>
		/// Creates a instance of markup minifier
		/// </summary>
		/// <returns>Instance of markup minifier</returns>
		public abstract IMarkupMinifier CreateMinifier();
	}
}