using WebMarkupMin.Core;

namespace WebMarkupMin.Yui
{
	/// <summary>
	/// YUI JS Minifier factory
	/// </summary>
	public sealed class YuiJsMinifierFactory : IJsMinifierFactory
	{
		/// <summary>
		/// Gets or sets a minification settings used to configure the YUI JS Minifier
		/// </summary>
		public YuiJsMinificationSettings MinificationSettings
		{
			get;
			set;
		}


		/// <summary>
		/// Creates a instance of YUI JS Minifier factory
		/// </summary>
		public YuiJsMinifierFactory() : this(new YuiJsMinificationSettings())
		{ }

		/// <summary>
		/// Creates a instance of YUI JS Minifier factory
		/// </summary>
		/// <param name="settings">Minification settings used to configure the YUI JS Minifier</param>
		public YuiJsMinifierFactory(YuiJsMinificationSettings settings)
		{
			MinificationSettings = settings;
		}


		/// <summary>
		/// Creates a instance of YUI JS Minifier
		/// </summary>
		/// <returns>Instance of YUI JS Minifier</returns>
		public IJsMinifier CreateMinifier()
		{
			return new YuiJsMinifier(MinificationSettings);
		}
	}
}