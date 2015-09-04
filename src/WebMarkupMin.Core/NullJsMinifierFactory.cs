namespace WebMarkupMin.Core
{
	/// <summary>
	/// Null JS minifier factory
	/// </summary>
	public sealed class NullJsMinifierFactory : IJsMinifierFactory
	{
		/// <summary>
		/// Creates a instance of Null JS minifier
		/// </summary>
		/// <returns>Instance of Null JS minifier</returns>
		public IJsMinifier CreateMinifier()
		{
			return new NullJsMinifier();
		}
	}
}