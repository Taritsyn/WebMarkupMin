namespace WebMarkupMin.Sample.Logic.Models
{
	/// <summary>
	/// XHTML minification view model
	/// </summary>
	public sealed class XhtmlMinificationViewModel : MarkupMinificationViewModelBase
	{
		/// <summary>
		/// XHTML minification settings
		/// </summary>
		public XhtmlMinificationSettingsViewModel Settings
		{
			get;
			set;
		}
	}
}