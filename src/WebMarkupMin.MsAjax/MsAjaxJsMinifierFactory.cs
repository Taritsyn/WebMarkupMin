using WebMarkupMin.Core;

namespace WebMarkupMin.MsAjax
{
	/// <summary>
	/// Microsoft Ajax JS Minifier factory
	/// </summary>
	public sealed class MsAjaxJsMinifierFactory : IJsMinifierFactory
	{
		/// <summary>
		/// Gets or sets a minification settings used to configure the Microsoft Ajax JS Minifier
		/// </summary>
		public MsAjaxJsMinificationSettings MinificationSettings
		{
			get;
			set;
		}


		/// <summary>
		/// Creates a instance of Microsoft Ajax JS Minifier factory
		/// </summary>
		public MsAjaxJsMinifierFactory() : this(new MsAjaxJsMinificationSettings())
		{ }

		/// <summary>
		/// Creates a instance of Microsoft Ajax JS Minifier factory
		/// </summary>
		/// <param name="settings">Minification settings used to configure the Microsoft Ajax JS Minifier</param>
		public MsAjaxJsMinifierFactory(MsAjaxJsMinificationSettings settings)
		{
			MinificationSettings = settings;
		}


		/// <summary>
		/// Creates a instance of Microsoft Ajax JS Minifier
		/// </summary>
		/// <returns>Instance of Microsoft Ajax JS Minifier</returns>
		public IJsMinifier CreateMinifier()
		{
			return new MsAjaxJsMinifier(MinificationSettings);
		}
	}
}