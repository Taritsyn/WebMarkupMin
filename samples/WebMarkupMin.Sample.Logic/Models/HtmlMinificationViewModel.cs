namespace WebMarkupMin.Sample.Logic.Models
{
	/// <summary>
	/// HTML minification view model
	/// </summary>
	public sealed class HtmlMinificationViewModel : MarkupMinificationViewModelBase
	{
		/// <summary>
		/// HTML minification settings
		/// </summary>
		public HtmlMinificationSettingsViewModel Settings
		{
			get;
			set;
		}
	}
}