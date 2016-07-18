using System;
using System.Collections.Generic;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet.Common.UrlMatchers;
using WebMarkupMin.Core;
using WebMarkupMin.Core.Loggers;

namespace WebMarkupMin.AspNet4.Common
{
	/// <summary>
	/// HTML minification manager
	/// </summary>
	public sealed class HtmlMinificationManager : HtmlMinificationManagerBase
	{
		/// <summary>
		/// Default instance of HTML minification manager
		/// </summary>
		private static readonly Lazy<HtmlMinificationManager> _default
			= new Lazy<HtmlMinificationManager>(() => new HtmlMinificationManager());

		/// <summary>
		/// Current instance of HTML minification manager
		/// </summary>
		private static IHtmlMinificationManager _current;

		/// <summary>
		/// Logger
		/// </summary>
		private ILogger _logger;

		/// <summary>
		/// CSS minifier factory
		/// </summary>
		private ICssMinifierFactory _cssMinifierFactory;

		/// <summary>
		/// JS minifier factory
		/// </summary>
		private IJsMinifierFactory _jsMinifierFactory;

		/// <summary>
		/// Gets or sets a instance of HTML minification manager
		/// </summary>
		public static IHtmlMinificationManager Current
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
		/// Gets or sets a logger
		/// </summary>
		protected override ILogger Logger
		{
			get
			{
				return _logger ?? DefaultLogger.Current;
			}
			set
			{
				_logger = value;
			}
		}

		/// <summary>
		/// Gets or sets a CSS minifier factory
		/// </summary>
		public override ICssMinifierFactory CssMinifierFactory
		{
			get
			{
				return _cssMinifierFactory ?? DefaultCssMinifierFactory.Current;
			}
			set
			{
				_cssMinifierFactory = value;
			}
		}

		/// <summary>
		/// Gets or sets a JS minifier factory
		/// </summary>
		public override IJsMinifierFactory JsMinifierFactory
		{
			get
			{
				return _jsMinifierFactory ?? DefaultJsMinifierFactory.Current;
			}
			set
			{
				_jsMinifierFactory = value;
			}
		}


		/// <summary>
		/// Constructs a instance of HTML minification manager
		/// </summary>
		public HtmlMinificationManager()
			: this(new HtmlMinificationSettings(), null, null, null)
		{ }

		/// <summary>
		/// Constructs a instance of HTML minification manager
		/// </summary>
		/// <param name="settings">HTML minification settings</param>
		/// <param name="cssMinifierFactory">CSS minifier factory</param>
		/// <param name="jsMinifierFactory">JS minifier factory</param>
		/// <param name="logger">Logger</param>
		public HtmlMinificationManager(HtmlMinificationSettings settings,
			ICssMinifierFactory cssMinifierFactory,
			IJsMinifierFactory jsMinifierFactory,
			ILogger logger)
		{
			MinificationSettings = settings;
			SupportedMediaTypes = new HashSet<string>(MediaTypeGroupConstants.Html);
			IncludedPages = new List<IUrlMatcher>();
			ExcludedPages = new List<IUrlMatcher>();
			CssMinifierFactory = cssMinifierFactory;
			JsMinifierFactory = jsMinifierFactory;

			Logger = logger;
		}
	}
}