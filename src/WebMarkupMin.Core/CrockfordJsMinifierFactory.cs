namespace WebMarkupMin.Core
{
	/// <summary>
	/// Douglas Crockford's JS minifier factory
	/// </summary>
	public sealed class CrockfordJsMinifierFactory : IJsMinifierFactory
    {
		/// <summary>
		/// Creates a instance of Douglas Crockford's JS minifier
		/// </summary>
		/// <returns>Instance of Douglas Crockford's JS minifier</returns>
		public IJsMinifier CreateMinifier()
		{
			return new CrockfordJsMinifier();
		}
	}
}