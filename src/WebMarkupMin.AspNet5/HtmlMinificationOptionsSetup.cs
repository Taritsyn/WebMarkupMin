using Microsoft.Framework.Internal;
using Microsoft.Framework.OptionsModel;

using WebMarkupMin.Core;

namespace WebMarkupMin.AspNet5
{
	/// <summary>
	/// Sets up default options for <see cref="HtmlMinificationOptions"/>
	/// </summary>
	public class HtmlMinificationOptionsSetup : ConfigureOptions<HtmlMinificationOptions>
	{
		/// <summary>
		/// CSS minifier factory
		/// </summary>
		private readonly ICssMinifierFactory _cssMinifierFactory;

		/// <summary>
		/// JS minifier factory
		/// </summary>
		private readonly IJsMinifierFactory _jsMinifierFactory;


		/// <summary>
		/// Constructs a instance of <see cref="HtmlMinificationOptionsSetup"/>
		/// </summary>
		/// <param name="cssMinifierFactory">CSS minifier factory</param>
		/// <param name="jsMinifierFactory">JS minifier factory</param>
		public HtmlMinificationOptionsSetup(
			ICssMinifierFactory cssMinifierFactory,
			IJsMinifierFactory jsMinifierFactory)
			: base(ConfigureHtmlMinificationOptions)
		{
			_cssMinifierFactory = cssMinifierFactory;
			_jsMinifierFactory = jsMinifierFactory;
		}


		/// <summary>
		/// Sets a default options
		/// </summary>
		public static void ConfigureHtmlMinificationOptions(HtmlMinificationOptions options)
		{ }

		public override void Configure([NotNull] HtmlMinificationOptions options)
		{
			options.CssMinifierFactory = _cssMinifierFactory;
			options.JsMinifierFactory = _jsMinifierFactory;

			base.Configure(options);
		}
	}
}