namespace WebMarkupMin.Core
{
	/// <summary>
	/// XHTML minification settings
	/// </summary>
	public sealed class XhtmlMinificationSettings : CommonHtmlMinificationSettingsBase
	{
		/// <summary>
		/// Gets or sets a Flag for whether to allow the inserting space
		/// before slash in empty tag
		/// </summary>
		public bool RenderEmptyTagsWithSpace
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs instance of XHTML minification settings
		/// </summary>
		public XhtmlMinificationSettings() : this(false)
		{ }

		/// <summary>
		/// Constructs instance of XHTML minification settings
		/// </summary>
		/// <param name="useEmptyMinificationSettings">Initiates the creation of
		/// empty XHTML minification settings</param>
		public XhtmlMinificationSettings(bool useEmptyMinificationSettings)
			: base(useEmptyMinificationSettings)
		{
			UseShortDoctype = false;
			UseMetaCharsetTag = false;
			RenderEmptyTagsWithSpace = true;
		}
	}
}