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
		/// Constructs a instance of XML minification manager
		/// </summary>
		public XmlMinificationManager()
			: this(new XmlMinificationSettings(), DefaultLogger.Current)
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

			_logger = logger;
		}
	}
}