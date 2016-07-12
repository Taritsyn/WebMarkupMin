namespace WebMarkupMin.Yui
{
	/// <summary>
	/// YUI CSS Minifier settings
	/// </summary>
	public sealed class YuiCssMinificationSettings : YuiCommonMinificationSettingsBase
	{
		/// <summary>
		/// Gets or sets a flag for whether to remove all comments
		/// except "important" comments
		/// </summary>
		public bool RemoveComments
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs an instance of the YUI CSS Minifier settings
		/// </summary>
		public YuiCssMinificationSettings()
		{
			LineBreakPosition = -1;
			RemoveComments = true;
		}
	}
}