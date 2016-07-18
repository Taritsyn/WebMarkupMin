using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet4.Common;
using WebMarkupMin.AspNet4.WebForms.Components;

namespace WebMarkupMin.AspNet4.WebForms
{
	/// <summary>
	/// Master page with support of HTML minification
	/// </summary>
	public class MinifiedHtmlMasterPage : MinifiedMasterPageBase
	{
		/// <summary>
		/// Constructs a instance of master page with support of HTML minification
		/// </summary>
		public MinifiedHtmlMasterPage()
			: this(WebMarkupMinConfiguration.Instance, null)
		{ }

		/// <summary>
		/// Constructs a instance of master page with support of HTML minification
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="minificationManager">HTML minification manager</param>
		public MinifiedHtmlMasterPage(WebMarkupMinConfiguration configuration,
			IHtmlMinificationManager minificationManager)
			: base(new MinifiedHtmlComponent(configuration, minificationManager))
		{ }
	}
}