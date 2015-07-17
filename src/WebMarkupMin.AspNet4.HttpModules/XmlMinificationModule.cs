using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet4.Common;

namespace WebMarkupMin.AspNet4.HttpModules
{
	/// <summary>
	/// HTTP module for XML minification
	/// </summary>
	public sealed class XmlMinificationModule : MarkupMinificationModuleBase
	{
		/// <summary>
		/// Constructs a instance of HTTP module for XML minification
		/// </summary>
		public XmlMinificationModule()
			: this(WebMarkupMinConfiguration.Instance, XmlMinificationManager.Current)
		{ }

		/// <summary>
		/// Constructs a instance of HTTP module for XML minification
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="minificationManager">XML minification manager</param>
		public XmlMinificationModule(
			WebMarkupMinConfiguration configuration,
			IXmlMinificationManager minificationManager)
			: base(configuration, minificationManager)
		{ }
	}
}