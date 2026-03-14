namespace WebMarkupMin.Core
{
	/// <summary>
	/// Mads Kristensen's CSS minifier factory
	/// </summary>
	public sealed class KristensenCssMinifierFactory : ICssMinifierFactory
	{
		/// <summary>
		/// Gets or sets a minification settings used to configure the Mads Kristensen's CSS Minifier
		/// </summary>
		public KristensenCssMinificationSettings MinificationSettings
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs an instance of the Mads Kristensen's CSS Minifier factory
		/// </summary>
		public KristensenCssMinifierFactory()
			: this(new KristensenCssMinificationSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the Mads Kristensen's CSS Minifier factory
		/// </summary>
		/// <param name="settings">Minification settings used to configure the Mads Kristensen's CSS Minifier</param>
		public KristensenCssMinifierFactory(KristensenCssMinificationSettings settings)
		{
			MinificationSettings = settings;
		}


		#region ICssMinifierFactory implementation

		/// <summary>
		/// Creates a instance of Mads Kristensen's CSS minifier
		/// </summary>
		/// <returns>Instance of Mads Kristensen's CSS minifier</returns>
		public ICssMinifier CreateMinifier()
		{
			return new KristensenCssMinifier(MinificationSettings);
		}

		#endregion
	}
}