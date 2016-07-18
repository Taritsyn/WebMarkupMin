using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet4.Common;
using WebMarkupMin.AspNet4.WebForms.Components;

namespace WebMarkupMin.AspNet4.WebForms
{
	/// <summary>
	/// Web Forms page with support of HTML minification
	/// </summary>
	public class MinifiedHtmlPage : MinifiedPageBase
	{
		/// <summary>
		/// Constructs a instance of Web Forms page with support of HTML minification
		/// </summary>
		public MinifiedHtmlPage()
			: this(WebMarkupMinConfiguration.Instance, null)
		{ }

		/// <summary>
		/// Constructs a instance of Web Forms page with support of HTML minification
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="minificationManager">HTML minification manager</param>
		public MinifiedHtmlPage(WebMarkupMinConfiguration configuration,
			IHtmlMinificationManager minificationManager)
			: base(new MinifiedHtmlComponent(configuration, minificationManager))
		{ }
	}
}