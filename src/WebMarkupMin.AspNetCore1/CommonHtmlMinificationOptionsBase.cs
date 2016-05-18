using WebMarkupMin.Core;

namespace WebMarkupMin.AspNetCore1
{
	/// <summary>
	/// Base class of common HTML minification options
	/// </summary>
	/// <typeparam name="TSettings">The type of generic HTML settings</typeparam>
	public abstract class CommonHtmlMinificationOptionsBase<TSettings>
		: MarkupMinificationOptionsBase<TSettings>
		where TSettings : class, new()
	{
		/// <summary>
		/// Gets or sets a CSS minifier factory
		/// </summary>
		public virtual ICssMinifierFactory CssMinifierFactory
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a JS minifier factory
		/// </summary>
		public virtual IJsMinifierFactory JsMinifierFactory
		{
			get;
			set;
		}
	}
}