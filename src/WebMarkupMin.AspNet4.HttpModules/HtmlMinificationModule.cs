using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet4.Common;

namespace WebMarkupMin.AspNet4.HttpModules
{
	/// <summary>
	/// HTTP module for HTML minification
	/// </summary>
	public sealed class HtmlMinificationModule : MarkupMinificationModuleBase
	{
		/// <summary>
		/// Constructs a instance of HTTP module for HTML minification
		/// </summary>
		public HtmlMinificationModule()
			: this(WebMarkupMinConfiguration.Instance, HtmlMinificationManager.Current)
		{ }

		/// <summary>
		/// Constructs a instance of HTTP module for HTML minification
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="minificationManager">HTML minification manager</param>
		public HtmlMinificationModule(
			WebMarkupMinConfiguration configuration,
			IHtmlMinificationManager minificationManager)
			: base(configuration, minificationManager)
		{ }
	}
}