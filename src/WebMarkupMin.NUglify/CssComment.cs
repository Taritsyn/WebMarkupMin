namespace WebMarkupMin.NUglify
{
	public enum CssComment
	{
		/// <summary>
		/// Remove all comments except those marked as important (<code>//!</code> or <code>/*!</code>)
		/// </summary>
		Important = 0,

		/// <summary>
		/// Remove all source comments from the output
		/// </summary>
		None,

		/// <summary>
		/// Keep all source comments in the output
		/// </summary>
		All,

		/// <summary>
		/// Remove all source comments except those for approved comment-based hacks
		/// </summary>
		Hacks
	}
}