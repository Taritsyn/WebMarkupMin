using WebMarkupMin.Core;

namespace WebMarkupMin.NUglify
{
	/// <summary>
	/// NUglify CSS Minifier factory
	/// </summary>
	public sealed class NUglifyCssMinifierFactory : ICssMinifierFactory
	{
		/// <summary>
		/// Gets or sets a minification settings used to configure the NUglify CSS Minifier
		/// </summary>
		public NUglifyCssMinificationSettings MinificationSettings
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs an instance of the NUglify CSS Minifier factory
		/// </summary>
		public NUglifyCssMinifierFactory() : this(new NUglifyCssMinificationSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the NUglify CSS Minifier factory
		/// </summary>
		/// <param name="settings">Minification settings used to configure the NUglify CSS Minifier</param>
		public NUglifyCssMinifierFactory(NUglifyCssMinificationSettings settings)
		{
			MinificationSettings = settings;
		}


		#region ICssMinifierFactory implementation

		/// <summary>
		/// Creates a instance of the NUglify CSS Minifier
		/// </summary>
		/// <returns>Instance of the NUglify CSS Minifier</returns>
		public ICssMinifier CreateMinifier()
		{
			return new NUglifyCssMinifier(MinificationSettings);
		}

		#endregion
	}
}