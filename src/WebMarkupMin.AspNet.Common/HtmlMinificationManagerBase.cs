using System.Collections.Generic;

using WebMarkupMin.Core;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Base class of HTML minification manager
	/// </summary>
	public abstract class HtmlMinificationManagerBase
		: CommonHtmlMinificationManagerBase<HtmlMinificationSettings>, IHtmlMinificationManager
	{
		/// <summary>
		/// Constructs a instance of HTML minification manager
		/// </summary>
		protected HtmlMinificationManagerBase()
		{
			PoweredByHttpHeader = new KeyValuePair<string, string>(
				"X-HTML-Minification-Powered-By", "WebMarkupMin");
		}


		/// <summary>
		/// Creates a instance of HTML minifier
		/// </summary>
		/// <returns>Instance of HTML minifier</returns>
		public override IMarkupMinifier CreateMinifier()
		{
			HtmlMinificationSettings settings = MinificationSettings;
			ICssMinifier cssMinifier = CssMinifierFactory.CreateMinifier();
			IJsMinifier jsMinifier = JsMinifierFactory.CreateMinifier();

			var minifier = new HtmlMinifier(settings, cssMinifier, jsMinifier, Logger);

			return minifier;
		}
	}
}