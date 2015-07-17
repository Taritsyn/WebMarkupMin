using WebMarkupMin.Core;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Base class of common HTML minification manager
	/// </summary>
	/// <typeparam name="TSettings">The type of generic HTML settings</typeparam>
	public abstract class CommonHtmlMinificationManagerBase<TSettings>
		: MarkupMinificationManagerBase<TSettings>, ICommonHtmlMinificationManager<TSettings>
		where TSettings : class, new()
	{
		/// <summary>
		/// Gets or sets a JS minifier factory
		/// </summary>
		public virtual IJsMinifierFactory JsMinifierFactory
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a CSS minifier factory
		/// </summary>
		public virtual ICssMinifierFactory CssMinifierFactory
		{
			get;
			set;
		}
	}
}