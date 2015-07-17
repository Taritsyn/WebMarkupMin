namespace WebMarkupMin.Core
{
	/// <summary>
	/// Mads Kristensen's CSS minifier factory
	/// </summary>
	public sealed class KristensenCssMinifierFactory : ICssMinifierFactory
    {
		/// <summary>
		/// Creates a instance of Mads Kristensen's CSS minifier
		/// </summary>
		/// <returns>Instance of Mads Kristensen's CSS minifier</returns>
		public ICssMinifier CreateMinifier()
		{
			return new KristensenCssMinifier();
        }
	}
}