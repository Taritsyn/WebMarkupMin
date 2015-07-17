namespace WebMarkupMin.Core
{
	/// <summary>
	/// Defines a interface of CSS minifier factory
	/// </summary>
	public interface ICssMinifierFactory
    {
		/// <summary>
		/// Creates a instance of CSS minifier
		/// </summary>
		/// <returns>Instance of CSS minifier</returns>
		ICssMinifier CreateMinifier();
	}
}