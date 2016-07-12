namespace WebMarkupMin.NUglify
{
	/// <summary>
	/// Common settings of the NUglify Minifier
	/// </summary>
	public abstract class NUglifyCommonMinificationSettingsBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether the opening curly brace for blocks is
		/// on its own line (<code>NewLine</code>) or on the same line as
		/// the preceding code (<code>SameLine</code>)
		/// or taking a hint from the source code position (<code>UseSource</code>).
		/// Only relevant when OutputMode is set to <code>MultipleLines</code>.
		/// </summary>
		public BlockStart BlocksStartOnSameLine
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to ignore all errors found in the input code
		/// </summary>
		public bool IgnoreAllErrors
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a string representation of the list of
		/// debug lookups (comma-separated)
		/// </summary>
		public string IgnoreErrorList
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a number of spaces per indent level when in
		/// <code>MultipleLines</code> output mode
		/// </summary>
		public int IndentSize
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a column position at which the line
		/// will be broken at the next available opportunity
		/// </summary>
		public int LineBreakThreshold
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a output mode:
		/// <code>SingleLine</code> - output all code on a single line;
		/// <code>MultipleLines</code> - break the output into multiple lines to be more human-readable
		/// </summary>
		public OutputMode OutputMode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a string representation of the list
		/// of names defined for the preprocessor (comma-separated)
		/// </summary>
		public string PreprocessorDefineList
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to add a semicolon
		/// at the end of the parsed code
		/// </summary>
		public bool TermSemicolons
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs an instance of common settings of the NUglify Minifier
		/// </summary>
		protected NUglifyCommonMinificationSettingsBase()
		{
			BlocksStartOnSameLine = BlockStart.NewLine;
			IgnoreAllErrors = false;
			IgnoreErrorList = string.Empty;
			IndentSize = 4;
			LineBreakThreshold = int.MaxValue;
			OutputMode = OutputMode.SingleLine;
			PreprocessorDefineList = string.Empty;
			TermSemicolons = false;
		}
	}
}