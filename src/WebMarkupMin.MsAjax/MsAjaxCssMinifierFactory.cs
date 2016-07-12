using WebMarkupMin.Core;

namespace WebMarkupMin.MsAjax
{
	/// <summary>
	/// Microsoft Ajax CSS Minifier factory
	/// </summary>
	public sealed class MsAjaxCssMinifierFactory : ICssMinifierFactory
	{
		/// <summary>
		/// Gets or sets a minification settings used to configure the Microsoft Ajax CSS Minifier
		/// </summary>
		public MsAjaxCssMinificationSettings MinificationSettings
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs an instance of the Microsoft Ajax CSS Minifier factory
		/// </summary>
		public MsAjaxCssMinifierFactory() : this(new MsAjaxCssMinificationSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the Microsoft Ajax CSS Minifier factory
		/// </summary>
		/// <param name="settings">Minification settings used to configure the Microsoft Ajax CSS Minifier</param>
		public MsAjaxCssMinifierFactory(MsAjaxCssMinificationSettings settings)
		{
			MinificationSettings = settings;
		}


		/// <summary>
		/// Creates a instance of the Microsoft Ajax CSS Minifier
		/// </summary>
		/// <returns>Instance of the Microsoft Ajax CSS Minifier</returns>
		public ICssMinifier CreateMinifier()
		{
			return new MsAjaxCssMinifier(MinificationSettings);
		}
	}
}