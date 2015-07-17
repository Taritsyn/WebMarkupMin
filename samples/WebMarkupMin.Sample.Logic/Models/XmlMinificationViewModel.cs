namespace WebMarkupMin.Sample.Logic.Models
{
	/// <summary>
	/// XML minification view model
	/// </summary>
	public sealed class XmlMinificationViewModel : MarkupMinificationViewModelBase
	{
		/// <summary>
		/// XML minification settings
		/// </summary>
		public XmlMinificationSettingsViewModel Settings
		{
			get;
			set;
		}
	}
}