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