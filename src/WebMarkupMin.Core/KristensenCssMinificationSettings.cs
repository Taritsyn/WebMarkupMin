namespace WebMarkupMin.Core
{
	/// <summary>
	/// Mads Kristensen's CSS Minifier settings
	/// </summary>
	public sealed class KristensenCssMinificationSettings
	{
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
			RemoveUnitsFromZeroValues = false;
		}
	}
}