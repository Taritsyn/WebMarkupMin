using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet4.Common;

namespace WebMarkupMin.AspNet4.WebForms.Components
{
	/// <summary>
	/// HTML minified component
	/// </summary>
	public sealed class MinifiedHtmlComponent : MinifiedComponentBase
	{
		/// <summary>
		/// Constructs a instance of HTML minified component
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="minificationManager">HTML minification manager</param>
		public MinifiedHtmlComponent(WebMarkupMinConfiguration configuration,
			IHtmlMinificationManager minificationManager)
			: base(configuration, minificationManager)
		{ }


		/// <summary>
		/// Gets a instance of default HTML minification manager
		/// </summary>
		/// <returns>Instance of default HTML minification manager</returns>
		protected override IMarkupMinificationManager GetDefaultMinificationManager()
		{
			return HtmlMinificationManager.Current;
		}
	}
}