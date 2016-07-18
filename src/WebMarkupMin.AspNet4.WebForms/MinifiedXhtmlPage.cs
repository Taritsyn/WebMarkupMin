using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet4.Common;
using WebMarkupMin.AspNet4.WebForms.Components;

namespace WebMarkupMin.AspNet4.WebForms
{
	/// <summary>
	/// Web Forms page with support of XHTML minification
	/// </summary>
	public class MinifiedXhtmlPage : MinifiedPageBase
	{
		/// <summary>
		/// Constructs a instance of Web Forms page with support of XHTML minification
		/// </summary>
		public MinifiedXhtmlPage()
			: this(WebMarkupMinConfiguration.Instance, null)
		{ }

		/// <summary>
		/// Constructs a instance of Web Forms page with support of XHTML minification
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="minificationManager">XHTML minification manager</param>
		public MinifiedXhtmlPage(WebMarkupMinConfiguration configuration,
			IXhtmlMinificationManager minificationManager)
			: base(new MinifiedXhtmlComponent(configuration, minificationManager))
		{ }
	}
}