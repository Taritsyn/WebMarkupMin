namespace WebMarkupMin.MsAjax
{
	public enum CssColor
	{
		/// <summary>
		/// Convert strict names to hex values if shorter; hex values to strict names if shorter. Leave all other
		/// color names or hex values as-specified.
		/// </summary>
		Strict = 0,

		/// <summary>
		/// Always use hex values; do not convert any hex values to color names
		/// </summary>
		Hex,

		/// <summary>
		/// Convert known hex values to major-browser color names if shorter; and known major-browser color
		/// names to hex if shorter
		/// </summary>
		Major,

		/// <summary>
		/// Don't swap names for hex or hex for names, whether or not one is shorter
		/// </summary>
		NoSwap
	}
}