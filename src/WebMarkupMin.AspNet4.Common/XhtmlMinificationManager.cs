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
		/// Constructs a instance of XHTML minification manager
		/// </summary>
		public XhtmlMinificationManager()
			: this(new XhtmlMinificationSettings(), null, null, null)
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

			Logger = logger;
		}
	}
}