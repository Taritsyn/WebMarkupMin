namespace WebMarkupMin.Core
{
	/// <summary>
	/// Defines a interface of JS minifier factory
	/// </summary>
	public interface IJsMinifierFactory
	{
		/// <summary>
		/// Creates a instance of JS minifier
		/// </summary>
		/// <returns>Instance of JS minifier</returns>
		IJsMinifier CreateMinifier();
	}
}