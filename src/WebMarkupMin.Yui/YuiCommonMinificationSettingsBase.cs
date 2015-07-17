namespace WebMarkupMin.Yui
{
	/// <summary>
	/// Common settings of YUI Minifier
 	/// </summary>
	public abstract class YuiCommonMinificationSettingsBase
	{
		/// <summary>
		/// Gets or sets a column number, after which must be inserted a line break.
		/// Specify 0 to get a line break after each semi-colon in JavaScript,
		/// and after each rule in CSS.
		/// </summary>
		public int LineBreakPosition
		{
			get;
			set;
		}
	}
}