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
		/// Constructs an instance of the Microsoft Ajax JS Minifier factory
		/// </summary>
		public MsAjaxJsMinifierFactory() : this(new MsAjaxJsMinificationSettings())
		{ }

		/// <summary>
		/// Constructs a instance of the Microsoft Ajax JS Minifier factory
		/// </summary>
		/// <param name="settings">Minification settings used to configure the Microsoft Ajax JS Minifier</param>
		public MsAjaxJsMinifierFactory(MsAjaxJsMinificationSettings settings)
		{
			MinificationSettings = settings;
		}


		#region IJsMinifierFactory implementation

		/// <summary>
		/// Creates a instance of the Microsoft Ajax JS Minifier
		/// </summary>
		/// <returns>Instance of the Microsoft Ajax JS Minifier</returns>
		public IJsMinifier CreateMinifier()
		{
			return new MsAjaxJsMinifier(MinificationSettings);
		}

		#endregion
	}
}