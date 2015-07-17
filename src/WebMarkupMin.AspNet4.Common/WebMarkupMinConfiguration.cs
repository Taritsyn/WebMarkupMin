using System;

using WebMarkupMin.AspNet.Common;

namespace WebMarkupMin.AspNet4.Common
{
	/// <summary>
	/// WebMarkupMin configuration
	/// </summary>
	public sealed class WebMarkupMinConfiguration : WebMarkupMinConfigurationBase
	{
		/// <summary>
		/// Instance of WebMarkupMin configuration
		/// </summary>
		private static readonly Lazy<WebMarkupMinConfiguration> _instance =
			new Lazy<WebMarkupMinConfiguration>(() => new WebMarkupMinConfiguration());

		/// <summary>
		/// Gets instance of WebMarkupMin configuration
		/// </summary>
		public static WebMarkupMinConfiguration Instance
		{
			get { return _instance.Value; }
		}

		/// <summary>
		/// Gets or sets a flag for whether to allow markup minification during debugging
		/// </summary>
		public bool AllowMinificationInDebugMode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to allow HTTP compression
		/// of content during debugging
		/// </summary>
		public bool AllowCompressionInDebugMode
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs a instance of WebMarkupMin configuration
		/// </summary>
		private WebMarkupMinConfiguration()
		{
			AllowMinificationInDebugMode = false;
			AllowCompressionInDebugMode = false;
		}
	}
}