using Microsoft.Extensions.Internal;
using Microsoft.Extensions.OptionsModel;

using WebMarkupMin.Core;

namespace WebMarkupMin.AspNet5
{
	/// <summary>
	/// Sets up default options for <see cref="XhtmlMinificationOptions"/>
	/// </summary>
	public class XhtmlMinificationOptionsSetup : ConfigureOptions<XhtmlMinificationOptions>
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
		/// Constructs a instance of <see cref="XhtmlMinificationOptionsSetup"/>
		/// </summary>
		/// <param name="cssMinifierFactory">CSS minifier factory</param>
		/// <param name="jsMinifierFactory">JS minifier factory</param>
		public XhtmlMinificationOptionsSetup(
			ICssMinifierFactory cssMinifierFactory,
			IJsMinifierFactory jsMinifierFactory)
			: base(ConfigureXhtmlMinificationOptions)
		{
			_cssMinifierFactory = cssMinifierFactory;
			_jsMinifierFactory = jsMinifierFactory;
		}


		/// <summary>
		/// Sets a default options
		/// </summary>
		public static void ConfigureXhtmlMinificationOptions(XhtmlMinificationOptions options)
		{ }

		public override void Configure([NotNull] XhtmlMinificationOptions options)
		{
			options.CssMinifierFactory = _cssMinifierFactory;
			options.JsMinifierFactory = _jsMinifierFactory;

			base.Configure(options);
		}
	}
}