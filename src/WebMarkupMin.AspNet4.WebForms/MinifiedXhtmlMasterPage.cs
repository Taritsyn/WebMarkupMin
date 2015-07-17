using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet4.Common;

namespace WebMarkupMin.AspNet4.WebForms
{
	/// <summary>
	/// Master page with support of XHTML minification
	/// </summary>
	public class MinifiedXhtmlMasterPage : MinifiedMasterPageBase
	{
		/// <summary>
		/// Constructs a instance of master page with support of XHTML minification
		/// </summary>
		public MinifiedXhtmlMasterPage()
			: this(WebMarkupMinConfiguration.Instance, XhtmlMinificationManager.Current)
		{ }

		/// <summary>
		/// Constructs a instance of master page with support of XHTML minification
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="minificationManager">XHTML minification manager</param>
		public MinifiedXhtmlMasterPage(WebMarkupMinConfiguration configuration,
			IXhtmlMinificationManager minificationManager)
			: base(configuration, minificationManager)
		{ }
	}
}