namespace WebMarkupMin.Core
{
	/// <summary>
	/// Generic HTML minification settings
	/// </summary>
	internal sealed class GenericHtmlMinificationSettings : AdvancedHtmlMinificationSettingsBase
	{
		/// <summary>
		/// Gets or sets a flag for whether to use XHTML syntax
		/// </summary>
		public bool UseXhtmlSyntax
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs instance of generic HTML minification settings
		/// </summary>
		public GenericHtmlMinificationSettings()
			: this(false)
		{ }


		/// <summary>
		/// Constructs instance of generic HTML minification settings
		/// </summary>
		/// <param name="useEmptyMinificationSettings">Initiates the creation of
		/// empty generic HTML minification settings</param>
		public GenericHtmlMinificationSettings(bool useEmptyMinificationSettings)
			: base(useEmptyMinificationSettings)
		{ }
	}
}