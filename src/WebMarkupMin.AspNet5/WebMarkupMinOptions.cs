using Microsoft.AspNet.Hosting;

using WebMarkupMin.AspNet.Common;

namespace WebMarkupMin.AspNet5
{
	/// <summary>
	/// WebMarkupMin options
	/// </summary>
	public class WebMarkupMinOptions : WebMarkupMinConfigurationBase
	{
		/// <summary>
		/// Gets or sets a flag for whether to allow markup minification
		/// if the current hosting environment name is development
		/// </summary>
		public bool AllowMinificationInDevelopmentEnvironment
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to allow HTTP compression of content
		/// if the current hosting environment name is development
		/// </summary>
		public bool AllowCompressionInDevelopmentEnvironment
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a instance of hosting environment
		/// </summary>
		public IHostingEnvironment HostingEnvironment
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs a instance of WebMarkupMin options
		/// </summary>
		public WebMarkupMinOptions()
		{
			AllowMinificationInDevelopmentEnvironment = false;
			AllowCompressionInDevelopmentEnvironment = false;
		}
	}
}