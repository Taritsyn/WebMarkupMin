namespace WebMarkupMin.Core
{
	/// <summary>
	/// Removal mode of HTML attribute quotes
	/// </summary>
	public enum HtmlAttributeQuotesRemovalMode
	{
		/// <summary>
		/// Keep quotes
		/// </summary>
		KeepQuotes = 0,

		/// <summary>
		/// Removes a quotes in accordance with standard HTML 4.X
		/// </summary>
		Html4 = 1,

		/// <summary>
		/// Removes a quotes in accordance with standard HTML5
		/// </summary>
		Html5 = 2
	}
}