namespace WebMarkupMin.Core
{
	/// <summary>
	/// Null CSS minifier factory
	/// </summary>
	public sealed class NullCssMinifierFactory : ICssMinifierFactory
	{
		/// <summary>
		/// Creates a instance of Null CSS minifier
		/// </summary>
		/// <returns>Instance of Null CSS minifier</returns>
		public ICssMinifier CreateMinifier()
		{
			return new NullCssMinifier();
		}
	}
}