using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.OptionsModel;

namespace WebMarkupMin.AspNet5
{
	/// <summary>
	/// Sets up default options for <see cref="WebMarkupMinOptions"/>
	/// </summary>
	public class WebMarkupMinOptionsSetup : ConfigureOptions<WebMarkupMinOptions>
	{
		/// <summary>
		/// Hosting environment
		/// </summary>
		private readonly IHostingEnvironment _hostingEnvironment;


		/// <summary>
		/// Constructs a instance of <see cref="WebMarkupMinOptionsSetup"/>
		/// </summary>
		public WebMarkupMinOptionsSetup(IHostingEnvironment hostingEnvironment)
			: base(ConfigureWebMarkupMinOptions)
		{
			_hostingEnvironment = hostingEnvironment;
		}


		/// <summary>
		/// Sets a default options
		/// </summary>
		public static void ConfigureWebMarkupMinOptions(WebMarkupMinOptions options)
		{ }

		public override void Configure(
			[NotNull] WebMarkupMinOptions options)
		{
			options.HostingEnvironment = _hostingEnvironment;

			base.Configure(options);
		}
	}
}
