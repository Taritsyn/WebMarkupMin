using System;
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
		protected ILogger _logger
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
		/// Gets or sets a list of supported media types
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
		/// Creates a instance of markup minifier
		/// </summary>
		/// <returns>Instance of markup minifier</returns>
		public abstract IMarkupMinifier CreateMinifier();
	}
}