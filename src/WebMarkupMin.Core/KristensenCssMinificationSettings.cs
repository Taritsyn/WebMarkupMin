namespace WebMarkupMin.Core
{
	/// <summary>
	/// Mads Kristensen's CSS Minifier settings
	/// </summary>
	public sealed class KristensenCssMinificationSettings
	{
		/// <summary>
		/// Gets or sets a flag for whether to remove redundant selectors
		/// (e.g. <c>a#btnResetPassword</c> → <c>#btnResetPassword</c>)
		/// </summary>
		public bool RemoveRedundantSelectors
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove trailing semicolons after the last declarations
		/// </summary>
		public bool RemoveTrailingSemicolons
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove units from zero values
		/// </summary>
		/// <remarks>
		/// <para>Units to be removed: <c>px</c>, <c>pt</c>, <c>pc</c>, <c>cm</c>, <c>mm</c>, <c>in</c>, <c>em</c>,
		/// <c>ex</c>, <c>ch</c>, <c>rem</c>, <c>vw</c>, <c>vh</c>, <c>vmin</c> and <c>vmax</c>.</para>
		/// <para>If in your code declares custom properties (variables) or calls the following functions:
		/// <c>min</c>, <c>max</c>, <c>clamp</c> and <c>calc</c>, then it is recommended to disable this setting.</para>
		/// </remarks>
		public bool RemoveUnitsFromZeroValues
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs an instance of the Mads Kristensen's CSS Minifier settings
		/// </summary>
		public KristensenCssMinificationSettings()
		{
			RemoveRedundantSelectors = false;
			RemoveTrailingSemicolons = true;
			RemoveUnitsFromZeroValues = false;
		}
	}
}