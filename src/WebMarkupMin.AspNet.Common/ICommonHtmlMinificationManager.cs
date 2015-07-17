using WebMarkupMin.Core;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Defines a interface of common HTML minification manager
	/// </summary>
	public interface ICommonHtmlMinificationManager<TSettings> : IMarkupMinificationManager<TSettings>
		where TSettings : class, new()
	{
		/// <summary>
		/// Gets or sets a JS minifier factory
		/// </summary>
		IJsMinifierFactory JsMinifierFactory
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a CSS minifier factory
		/// </summary>
		ICssMinifierFactory CssMinifierFactory
		{
			get;
			set;
		}
	}
}