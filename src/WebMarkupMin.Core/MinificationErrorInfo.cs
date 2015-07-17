namespace WebMarkupMin.Core
{
	/// <summary>
	/// Minification error information
	/// </summary>
	public class MinificationErrorInfo
	{
		/// <summary>
		/// Gets a category name
		/// </summary>
		public string Category
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a message
		/// </summary>
		public string Message
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a line number on which the error occurred
		/// </summary>
		public int LineNumber
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a column number on which the error occurred
		/// </summary>
		public int ColumnNumber
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a source fragment
		/// </summary>
		public string SourceFragment
		{
			get;
			private set;
		}


		/// <summary>
		/// Constructs instance of error details
		/// </summary>
		/// <param name="message">Message</param>
		public MinificationErrorInfo(string message)
			: this(string.Empty, message, 0, 0, string.Empty)
		{ }

		/// <summary>
		/// Constructs instance of error details
		/// </summary>
		/// <param name="category">Category name</param>
		/// <param name="message">Message</param>
		public MinificationErrorInfo(string category, string message)
			: this(category, message, 0, 0, string.Empty)
		{ }

		/// <summary>
		/// Constructs instance of error details
		/// </summary>
		/// <param name="message">Message</param>
		/// <param name="lineNumber">Line number</param>
		/// <param name="columnNumber">Column number</param>
		/// <param name="sourceFragment">Fragment of source code</param>
		public MinificationErrorInfo(string message, int lineNumber, int columnNumber, string sourceFragment)
			: this(string.Empty, message, lineNumber, columnNumber, sourceFragment)
		{ }

		/// <summary>
		/// Constructs instance of error details
		/// </summary>
		/// <param name="category">Category name</param>
		/// <param name="message">Message</param>
		/// <param name="lineNumber">Line number</param>
		/// <param name="columnNumber">Column number</param>
		/// <param name="sourceFragment">Fragment of source code</param>
		public MinificationErrorInfo(string category, string message, int lineNumber, int columnNumber, string sourceFragment)
		{
			Category = category;
			Message = message;
			LineNumber = lineNumber;
			ColumnNumber = columnNumber;
			SourceFragment = sourceFragment;
		}
	}
}