namespace WebMarkupMin.Yui
{
	/// <summary>
	/// YUI JS Minifier settings
	/// </summary>
	public sealed class YuiJsMinificationSettings : YuiCommonMinificationSettingsBase
	{
		/// <summary>
		/// Gets or sets a flag for whether to allow obfuscation of code
		/// </summary>
		public bool ObfuscateJavascript
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to preserve unnecessary
		/// semicolons (such as right before a '}')
		/// </summary>
		public bool PreserveAllSemicolons
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to disable all the built-in micro optimizations
		/// </summary>
		public bool DisableOptimizations
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to ignore when processing code, that
		/// executed in eval operator
		/// </summary>
		public bool IgnoreEval
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a warning level threshold for reporting errors
		///		0 - error;
		///		1 - warning.
		/// </summary>
		public int WarningLevel
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs an instance of the YUI JS Minifier settings
		/// </summary>
		public YuiJsMinificationSettings()
		{
			LineBreakPosition = -1;
			ObfuscateJavascript = true;
			PreserveAllSemicolons = false;
			DisableOptimizations = false;
			IgnoreEval = false;
			WarningLevel = 0;
		}
	}
}