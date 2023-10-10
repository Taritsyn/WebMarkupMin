namespace WebMarkupMin.NUglify
{
	/// <summary>
	/// NUglify CSS Minifier settings
	/// </summary>
	public sealed class NUglifyCssMinificationSettings : NUglifyCommonMinificationSettingsBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether to abbreviate hex colors to #rgb(a) format
		/// </summary>
		public bool AbbreviateHexColor
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a <see cref="CssColor"/> setting
		/// </summary>
		public CssColor ColorNames
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a <see cref="CssComment"/> setting
		/// </summary>
		public CssComment CommentMode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether unicode escape strings (e.g. <code>\ff0e</code>)
		/// would be replaced by it's actual character or not
		/// </summary>
		public bool DecodeEscapes
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether to minify the
		/// JavaScript within expression functions
		/// </summary>
		public bool MinifyExpressions
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether empty blocks removes
		/// the corresponding rule or directive
		/// </summary>
		public bool RemoveEmptyBlocks
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs an instance of the NUglify CSS Minifier settings
		/// </summary>
		public NUglifyCssMinificationSettings()
		{
			AbbreviateHexColor = true;
			ColorNames = CssColor.Hex;
			CommentMode = CssComment.Important;
			DecodeEscapes = true;
			MinifyExpressions = true;
			RemoveEmptyBlocks = true;
		}
	}
}