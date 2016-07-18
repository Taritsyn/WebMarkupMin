using System.Collections.Generic;

using WebMarkupMin.Core;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Base class of XML minification manager
	/// </summary>
	public abstract class XmlMinificationManagerBase
		: MarkupMinificationManagerBase<XmlMinificationSettings>, IXmlMinificationManager
	{
		/// <summary>
		/// Constructs a instance of XML minification manager
		/// </summary>
		protected XmlMinificationManagerBase()
		{
			PoweredByHttpHeader = new KeyValuePair<string, string>(
				"X-XML-Minification-Powered-By", "WebMarkupMin");
		}


		/// <summary>
		/// Creates a instance of XML minifier
		/// </summary>
		/// <returns>Instance of XML minifier</returns>
		public override IMarkupMinifier CreateMinifier()
		{
			XmlMinificationSettings settings = MinificationSettings;

			var minifier = new XmlMinifier(settings, Logger);

			return minifier;
		}
	}
}