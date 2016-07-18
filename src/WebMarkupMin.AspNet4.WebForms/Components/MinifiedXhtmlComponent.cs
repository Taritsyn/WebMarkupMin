using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet4.Common;

namespace WebMarkupMin.AspNet4.WebForms.Components
{
	/// <summary>
	/// XHTML minified component
	/// </summary>
	public sealed class MinifiedXhtmlComponent : MinifiedComponentBase
	{
		/// <summary>
		/// Constructs a instance of XHTML minified component
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="minificationManager">XHTML minification manager</param>
		public MinifiedXhtmlComponent(WebMarkupMinConfiguration configuration,
			IXhtmlMinificationManager minificationManager)
			: base(configuration, minificationManager)
		{ }


		/// <summary>
		/// Gets a instance of default XHTML minification manager
		/// </summary>
		/// <returns>Instance of default XHTML minification manager</returns>
		protected override IMarkupMinificationManager GetDefaultMinificationManager()
		{
			return XhtmlMinificationManager.Current;
		}
	}
}