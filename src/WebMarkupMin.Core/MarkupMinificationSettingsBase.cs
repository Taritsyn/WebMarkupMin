namespace WebMarkupMin.Core
{
	/// <summary>
	/// Markup minification settings
	/// </summary>
	public abstract class MarkupMinificationSettingsBase
	{
		/// <summary>
		/// Gets or sets a flag for whether to collapse whitespace to one newline string when whitespace
		/// contains a newline
		/// </summary>
		public bool PreserveNewLines
		{
			get;
			set;
		}
	}
}