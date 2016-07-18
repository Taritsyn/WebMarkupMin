using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet4.Common;
using WebMarkupMin.AspNet4.WebForms.Components;

namespace WebMarkupMin.AspNet4.WebForms
{
	/// <summary>
	/// Master page with support of HTML minification and HTTP compression
	/// </summary>
	public class MinifiedAndCompressedHtmlMasterPage : MinifiedAndCompressedMasterPageBase
	{
		/// <summary>
		/// Constructs a instance of master page with of HTML minification and HTTP compression
		/// </summary>
		protected MinifiedAndCompressedHtmlMasterPage()
			: this(WebMarkupMinConfiguration.Instance, null, null)
		{ }

		/// <summary>
		/// Constructs a instance of master page with of HTML minification and HTTP compression
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="minificationManager">HTML minification manager</param>
		/// <param name="compressionManager">HTTP compression manager</param>
		protected MinifiedAndCompressedHtmlMasterPage(WebMarkupMinConfiguration configuration,
			IHtmlMinificationManager minificationManager,
			IHttpCompressionManager compressionManager)
			: base(new MinifiedHtmlComponent(configuration, minificationManager),
				new CompressedComponent(configuration, compressionManager))
		{ }
	}
}