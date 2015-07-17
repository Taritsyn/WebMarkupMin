namespace WebMarkupMin.MsAjax
{
	/// <summary>
	/// Microsoft CSS Minifier settings
	/// </summary>
	public sealed class MsAjaxCssMinificationSettings : MsAjaxCommonMinificationSettingsBase
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
		/// Constructs instance of Microsoft CSS Minifier settings
		/// </summary>
		public MsAjaxCssMinificationSettings()
		{
			ColorNames = CssColor.Hex;
			CommentMode = CssComment.Important;
			MinifyExpressions = true;
			RemoveEmptyBlocks = true;
		}
	}
}