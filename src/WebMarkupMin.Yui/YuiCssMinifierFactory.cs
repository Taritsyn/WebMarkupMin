using WebMarkupMin.Core;

namespace WebMarkupMin.Yui
{
	/// <summary>
	/// YUI CSS Minifier factory
	/// </summary>
	public sealed class YuiCssMinifierFactory : ICssMinifierFactory
	{
		/// <summary>
		/// Gets or sets a minification settings used to configure the YUI CSS Minifier
		/// </summary>
		public YuiCssMinificationSettings MinificationSettings
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs an instance of the YUI CSS Minifier factory
		/// </summary>
		public YuiCssMinifierFactory() : this(new YuiCssMinificationSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the YUI CSS Minifier factory
		/// </summary>
		/// <param name="settings">Minification settings used to configure the YUI CSS Minifier</param>
		public YuiCssMinifierFactory(YuiCssMinificationSettings settings)
		{
			MinificationSettings = settings;
		}


		#region ICssMinifierFactory implementation

		/// <summary>
		/// Creates a instance of the YUI CSS Minifier
		/// </summary>
		/// <returns>Instance of the YUI CSS Minifier</returns>
		public ICssMinifier CreateMinifier()
		{
			return new YuiCssMinifier(MinificationSettings);
		}

		#endregion
	}
}