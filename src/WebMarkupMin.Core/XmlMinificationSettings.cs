namespace WebMarkupMin.Core
{
	/// <summary>
	/// XML minification settings
	/// </summary>
	public sealed class XmlMinificationSettings : MarkupMinificationSettingsBase
	{
		/// <summary>
		/// Gets or sets a flag for whether to minify whitespace
		/// </summary>
		public bool MinifyWhitespace
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove all XML comments
		/// </summary>
		public bool RemoveXmlComments
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to allow the inserting space
		/// before slash in empty tag
		/// </summary>
		public bool RenderEmptyTagsWithSpace
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to collapse tags without content
		/// </summary>
		public bool CollapseTagsWithoutContent
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs instance of XML minification settings
		/// </summary>
		public XmlMinificationSettings()
			: this(false)
		{ }

		/// <summary>
		/// Constructs instance of XML minification settings
		/// </summary>
		/// <param name="useEmptyMinificationSettings">Initiates the creation of empty XML minification settings</param>
		public XmlMinificationSettings(bool useEmptyMinificationSettings)
		{
			if (!useEmptyMinificationSettings)
			{
				MinifyWhitespace = true;
				RemoveXmlComments = true;
			}
			else
			{
				MinifyWhitespace = false;
				RemoveXmlComments = false;
			}
			PreserveNewLines = false;
			CollapseTagsWithoutContent = false;
			RenderEmptyTagsWithSpace = false;
		}
	}
}