using System;

using Microsoft.Extensions.Options;

using WebMarkupMin.Core;

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2
#elif ASPNETCORE3
namespace WebMarkupMin.AspNetCore3
#elif ASPNETCORE5
namespace WebMarkupMin.AspNetCore5
#elif ASPNETCORE6
namespace WebMarkupMin.AspNetCore6
#elif ASPNETCORE7
namespace WebMarkupMin.AspNetCore7
#else
#error No implementation for this target
#endif
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

		public override void Configure(XhtmlMinificationOptions options)
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