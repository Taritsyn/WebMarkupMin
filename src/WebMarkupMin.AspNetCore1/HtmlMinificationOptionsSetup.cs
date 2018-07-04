using System;

using Microsoft.Extensions.Options;

using WebMarkupMin.Core;

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2
#else
#error No implementation for this target
#endif
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

		public override void Configure(HtmlMinificationOptions options)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			options.CssMinifierFactory = _cssMinifierFactory;
			options.JsMinifierFactory = _jsMinifierFactory;

			base.Configure(options);
		}
	}
}