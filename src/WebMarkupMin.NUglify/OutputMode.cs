namespace WebMarkupMin.NUglify
{
	/// <summary>
	/// Output mode setting
	/// </summary>
	public enum OutputMode
	{
		/// <summary>
		/// Output the minified code on a single line for maximum minification.
		/// LineBreakThreshold may still break the single line into multiple lines
		/// at a syntactically correct point after the given line length is reached.
		/// Not easily human-readable.
		/// </summary>
		SingleLine,

		/// <summary>
		/// Output the minified code on multiple lines to increase readability
		/// </summary>
		MultipleLines
	}
}