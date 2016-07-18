using System;
using System.Collections.Generic;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet.Common.UrlMatchers;
using WebMarkupMin.Core;
using WebMarkupMin.Core.Loggers;

namespace WebMarkupMin.AspNet4.Common
{
	/// <summary>
	/// XML minification manager
	/// </summary>
	public sealed class XmlMinificationManager : XmlMinificationManagerBase
	{
		/// <summary>
		/// Default instance of XML minification manager
		/// </summary>
		private static readonly Lazy<XmlMinificationManager> _default
			= new Lazy<XmlMinificationManager>(() => new XmlMinificationManager());

		/// <summary>
		/// Current instance of XML minification manager
		/// </summary>
		private static IXmlMinificationManager _current;

		/// <summary>
		/// Logger
		/// </summary>
		private ILogger _logger;

		/// <summary>
		/// Gets or sets a instance of XML minification manager
		/// </summary>
		public static IXmlMinificationManager Current
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
		/// Constructs a instance of XML minification manager
		/// </summary>
		public XmlMinificationManager()
			: this(new XmlMinificationSettings(), null)
		{ }

		/// <summary>
		/// Constructs a instance of XML minification manager
		/// </summary>
		/// <param name="settings">XML minification settings</param>
		/// <param name="logger">Logger</param>
		public XmlMinificationManager(XmlMinificationSettings settings, ILogger logger)
		{
			MinificationSettings = settings;
			SupportedMediaTypes = new HashSet<string>(MediaTypeGroupConstants.Xml);
			IncludedPages = new List<IUrlMatcher>();
			ExcludedPages = new List<IUrlMatcher>();

			Logger = logger;
		}
	}
}