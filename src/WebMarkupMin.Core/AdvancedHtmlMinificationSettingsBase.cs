namespace WebMarkupMin.Core
{
	/// <summary>
	/// Advanced HTML minification settings
	/// </summary>
	public abstract class AdvancedHtmlMinificationSettingsBase : CommonHtmlMinificationSettingsBase
	{
		/// <summary>
		/// Gets or sets a flag for whether to remove CDATA sections from scripts and styles
		/// </summary>
		public bool RemoveCdataSectionsFromScriptsAndStyles
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to preserve case of tag and attribute names
		/// (useful for Angular 2 templates)
		/// </summary>
		public bool PreserveCase
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a render mode of HTML empty tag
		/// </summary>
		public HtmlEmptyTagRenderMode EmptyTagRenderMode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove optional end tags
		/// </summary>
		public bool RemoveOptionalEndTags
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove values from boolean attributes
		/// </summary>
		public bool CollapseBooleanAttributes
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a removal mode of HTML attribute quotes
		/// </summary>
		public HtmlAttributeQuotesRemovalMode AttributeQuotesRemovalMode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove <code>type="text/javascript"</code> from <code>script</code> tags
		/// </summary>
		public bool RemoveJsTypeAttributes
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove <code>type="text/css"</code> from
		/// <code>style</code> and <code>link</code> tags
		/// </summary>
		public bool RemoveCssTypeAttributes
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs instance of advanced HTML minification settings
		/// </summary>
		/// <param name="useEmptyMinificationSettings">Initiates the creation of
		/// empty advanced HTML minification settings</param>
		protected AdvancedHtmlMinificationSettingsBase(bool useEmptyMinificationSettings)
			: base(useEmptyMinificationSettings)
		{ }
	}
}