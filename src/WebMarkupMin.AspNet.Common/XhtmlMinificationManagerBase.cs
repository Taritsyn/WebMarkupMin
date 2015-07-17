using System.Collections.Generic;

using WebMarkupMin.Core;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Base class of XHTML minification manager
	/// </summary>
	public abstract class XhtmlMinificationManagerBase
		: CommonHtmlMinificationManagerBase<XhtmlMinificationSettings>, IXhtmlMinificationManager
	{
		/// <summary>
		/// Constructs a instance of XHTML minification manager
		/// </summary>
		protected XhtmlMinificationManagerBase()
		{
			PoweredByHttpHeader = new KeyValuePair<string, string>(
				"X-XHTML-Minification-Powered-By", "WebMarkupMin");
		}


		/// <summary>
		/// Creates a instance of XHTML minifier
		/// </summary>
		/// <returns>Instance of XHTML minifier</returns>
		public override IMarkupMinifier CreateMinifier()
		{
			XhtmlMinificationSettings settings = MinificationSettings;
			ICssMinifier cssMinifier = CssMinifierFactory.CreateMinifier();
			IJsMinifier jsMinifier = JsMinifierFactory.CreateMinifier();

			var minifier = new XhtmlMinifier(settings, cssMinifier, jsMinifier, _logger);

			return minifier;
		}
	}
}