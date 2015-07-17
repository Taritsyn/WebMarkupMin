using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet4.Common;

namespace WebMarkupMin.AspNet4.WebForms
{
	/// <summary>
	/// Master page with support of XHTML minification and HTTP compression
	/// </summary>
	public class MinifiedAndCompressedXhtmlMasterPage : MinifiedAndCompressedMasterPageBase
	{
		/// <summary>
		/// Constructs a instance of master page with support of XHTML minification and HTTP compression
		/// </summary>
		public MinifiedAndCompressedXhtmlMasterPage()
			: this(WebMarkupMinConfiguration.Instance, XhtmlMinificationManager.Current, HttpCompressionManager.Current)
		{ }

		/// <summary>
		/// Constructs a instance of master page with support of XHTML minification and HTTP compression
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="minificationManager">XHTML minification manager</param>
		/// <param name="compressionManager">HTTP compression manager</param>
		public MinifiedAndCompressedXhtmlMasterPage(WebMarkupMinConfiguration configuration,
			IXhtmlMinificationManager minificationManager,
			IHttpCompressionManager compressionManager)
			: base(configuration, minificationManager, compressionManager)
		{ }
	}
}