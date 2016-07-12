namespace WebMarkupMin.NUglify
{
	/// <summary>
	/// NUglify CSS Minifier settings
	/// </summary>
	public sealed class NUglifyCssMinificationSettings : NUglifyCommonMinificationSettingsBase
	{
		/// <summary>
		/// Gets or sets ColorNames setting
		/// </summary>
		public CssColor ColorNames
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets CommentMode setting
		/// </summary>
		public CssComment CommentMode
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
			ColorNames = CssColor.Hex;
			CommentMode = CssComment.Important;
			MinifyExpressions = true;
			RemoveEmptyBlocks = true;
		}
	}
}