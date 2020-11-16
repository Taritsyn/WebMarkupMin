using System;

#if ASPNETCORE1 || ASPNETCORE2
using HostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
#elif ASPNETCORE3 || ASPNETCORE5
using HostingEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;
#else
#error No implementation for this target
#endif
using Microsoft.Extensions.Options;

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2
#elif ASPNETCORE3
namespace WebMarkupMin.AspNetCore3
#elif ASPNETCORE5
namespace WebMarkupMin.AspNetCore5
#else
#error No implementation for this target
#endif
{
	/// <summary>
	/// Sets up default options for <see cref="WebMarkupMinOptions"/>
	/// </summary>
	public class WebMarkupMinOptionsSetup : ConfigureOptions<WebMarkupMinOptions>
	{
		/// <summary>
		/// Hosting environment
		/// </summary>
		private readonly HostingEnvironment _hostingEnvironment;


		/// <summary>
		/// Constructs a instance of <see cref="WebMarkupMinOptionsSetup"/>
		/// </summary>
		public WebMarkupMinOptionsSetup(HostingEnvironment hostingEnvironment)
			: base(ConfigureWebMarkupMinOptions)
		{
			_hostingEnvironment = hostingEnvironment;
		}


		/// <summary>
		/// Sets a default options
		/// </summary>
		public static void ConfigureWebMarkupMinOptions(WebMarkupMinOptions options)
		{ }

		public override void Configure(WebMarkupMinOptions options)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			options.HostingEnvironment = _hostingEnvironment;

			base.Configure(options);
		}
	}
}