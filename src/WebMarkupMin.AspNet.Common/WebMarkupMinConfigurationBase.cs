namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Base class of WebMarkupMin configuration
	/// </summary>
	public abstract class WebMarkupMinConfigurationBase
	{
		/// <summary>
		/// Gets or sets a flag for whether to disable markup minification
		/// </summary>
		public bool DisableMinification
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to disable HTTP compression of content
		/// </summary>
		public bool DisableCompression
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a maximum size of the response (in bytes), in excess of which
		/// disables the minification of markup
		/// </summary>
		public int MaxResponseSize
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to disable the <c>*-Minification-Powered-By</c> HTTP headers
		/// (e.g. <c>"X-HTML-Minification-Powered-By: WebMarkupMin"</c>)
		/// </summary>
		public bool DisablePoweredByHttpHeaders
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs a instance of WebMarkupMin configuration
		/// </summary>
		protected WebMarkupMinConfigurationBase()
		{
			DisableMinification = false;
			DisableCompression = false;
			MaxResponseSize = -1;
			DisablePoweredByHttpHeaders = false;
		}
	}
}