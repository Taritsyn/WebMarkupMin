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

		/// <summary>
		/// Gets or sets a style of the newline
		/// </summary>
		public NewLineStyle NewLineStyle
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs instance of markup minification settings
		/// </summary>
		protected MarkupMinificationSettingsBase()
		{
			PreserveNewLines = false;
			NewLineStyle = NewLineStyle.Auto;
		}
	}
}