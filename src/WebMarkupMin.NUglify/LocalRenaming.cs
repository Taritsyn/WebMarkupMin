namespace WebMarkupMin.NUglify
{
	public enum LocalRenaming
	{
		/// <summary>
		/// Keep all names; don't rename anything
		/// </summary>
		KeepAll,

		/// <summary>
		/// Rename all local variables and functions that do not begin with "L_"
		/// </summary>
		KeepLocalizationVars,

		/// <summary>
		/// Rename all local variables and functions
		/// </summary>
		CrunchAll
	}
}