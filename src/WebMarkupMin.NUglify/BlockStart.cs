namespace WebMarkupMin.NUglify
{
	/// <summary>
	/// Describes how to output the opening curly-brace for blocks when the OutputMode
	/// is set to MultipleLines
	/// </summary>
	public enum BlockStart
	{
		/// <summary>
		/// Output the opening curly-brace block-start character on its own new line.
		/// Example:
		/// <code>if (condition)
		/// {
		///		...
		/// }</code>
		/// </summary>
		NewLine = 0,

		/// <summary>
		/// Output the opening curly-brace block-start character at the end of the previous line.
		/// Example:
		/// <code>if (condition) {
		///		...
		/// }</code>
		/// </summary>
		SameLine,

		/// <summary>
		/// Output the opening curly-brace block-start character on the same line or a new line
		/// depending on how it was specified in the sources
		/// </summary>
		UseSource
	}
}