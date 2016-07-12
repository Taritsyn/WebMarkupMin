using WebMarkupMin.Core;

namespace WebMarkupMin.NUglify
{
	/// <summary>
	/// NUglify JS Minifier factory
	/// </summary>
	public sealed class NUglifyJsMinifierFactory : IJsMinifierFactory
	{
		/// <summary>
		/// Gets or sets a minification settings used to configure the NUglify JS Minifier
		/// </summary>
		public NUglifyJsMinificationSettings MinificationSettings
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs an instance of the NUglify JS Minifier factory
		/// </summary>
		public NUglifyJsMinifierFactory() : this(new NUglifyJsMinificationSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the NUglify JS Minifier factory
		/// </summary>
		/// <param name="settings">Minification settings used to configure the NUglify JS Minifier</param>
		public NUglifyJsMinifierFactory(NUglifyJsMinificationSettings settings)
		{
			MinificationSettings = settings;
		}


		/// <summary>
		/// Creates a instance of the NUglify JS Minifier
		/// </summary>
		/// <returns>Instance of the NUglify JS Minifier</returns>
		public IJsMinifier CreateMinifier()
		{
			return new NUglifyJsMinifier(MinificationSettings);
		}
	}
}