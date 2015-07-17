using System;
using System.Collections.Generic;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet.Common.UrlMatchers;
using WebMarkupMin.Core;
using WebMarkupMin.Core.Loggers;

namespace WebMarkupMin.AspNet4.Common
{
	/// <summary>
	/// XHTML minification manager
	/// </summary>
	public sealed class XhtmlMinificationManager : XhtmlMinificationManagerBase
	{
		/// <summary>
		/// Default instance of XHTML minification manager
		/// </summary>
		private static readonly Lazy<XhtmlMinificationManager> _default
			= new Lazy<XhtmlMinificationManager>(() => new XhtmlMinificationManager());

		/// <summary>
		/// Current instance of XHTML minification manager
		/// </summary>
		private static IXhtmlMinificationManager _current;

		/// <summary>
		/// Gets or sets a instance of XHTML minification manager
		/// </summary>
		public static IXhtmlMinificationManager Current
		{
			get
			{
				return _current ?? _default.Value;
			}
			set
			{
				_current = value;
			}
		}


		/// <summary>
		/// Constructs a instance of XHTML minification manager
		/// </summary>
		public XhtmlMinificationManager()
			: this(new XhtmlMinificationSettings(), DefaultCssMinifierFactory.Current,
				DefaultJsMinifierFactory.Current, DefaultLogger.Current)
		{ }

		/// <summary>
		/// Constructs a instance of XHTML minification manager
		/// </summary>
		/// <param name="settings">XHTML minification settings</param>
		/// <param name="cssMinifierFactory">CSS minifier factory</param>
		/// <param name="jsMinifierFactory">JS minifier factory</param>
		/// <param name="logger">Logger</param>
		public XhtmlMinificationManager(XhtmlMinificationSettings settings,
			ICssMinifierFactory cssMinifierFactory,
			IJsMinifierFactory jsMinifierFactory,
			ILogger logger)
		{
			MinificationSettings = settings;
			SupportedMediaTypes = new HashSet<string>(MediaTypeGroupConstants.Xhtml);
			IncludedPages = new List<IUrlMatcher>();
			ExcludedPages = new List<IUrlMatcher>();
			CssMinifierFactory = cssMinifierFactory;
			JsMinifierFactory = jsMinifierFactory;

			_logger = logger;
		}
	}
}