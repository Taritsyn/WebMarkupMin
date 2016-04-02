using System;

using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// The exception that is thrown when a processing of code is failed
	/// </summary>
	public abstract class CodeProcessingException : Exception
	{
		/// <summary>
		/// Gets or sets a line number
		/// </summary>
		public int LineNumber
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a column number
		/// </summary>
		public int ColumnNumber
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a source fragment
		/// </summary>
		public string SourceFragment
		{
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeProcessingException"/> class
		/// with a specified error message
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		protected CodeProcessingException(string message)
			: this(message, 0, 0, string.Empty, null)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeProcessingException"/> class
		/// with a specified error message and reference to the inner exception that is
		/// the cause of this exception
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		/// <param name="innerException">Exception that is the cause of the current exception</param>
		protected CodeProcessingException(string message, Exception innerException)
			: this(message, 0, 0, string.Empty, innerException)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeProcessingException"/> class
		/// with a specified error message, node coordinates and source fragment
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		/// <param name="nodeCoordinates">Node coordinates</param>
		/// <param name="sourceFragment">Source fragment</param>
		protected CodeProcessingException(string message, SourceCodeNodeCoordinates nodeCoordinates, string sourceFragment)
			: this(message, nodeCoordinates, sourceFragment, null)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeProcessingException"/> class
		/// with a specified error message, line number, column number and source fragment
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		/// <param name="lineNumber">Line number</param>
		/// <param name="columnNumber">Column number</param>
		/// <param name="sourceFragment">SourceFragment</param>
		protected CodeProcessingException(string message, int lineNumber, int columnNumber, string sourceFragment)
			: this(message, lineNumber, columnNumber, sourceFragment, null)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeProcessingException"/> class
		/// with a specified error message, node coordinates, source fragment
		/// and reference to the inner exception that is the cause of this exception
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		/// <param name="nodeCoordinates">Node coordinates</param>
		/// <param name="sourceFragment">Source fragment</param>
		/// <param name="innerException">Exception that is the cause of the current exception</param>
		protected CodeProcessingException(string message, SourceCodeNodeCoordinates nodeCoordinates,
			string sourceFragment, Exception innerException)
			: this(message, nodeCoordinates.LineNumber, nodeCoordinates.ColumnNumber,
				sourceFragment, innerException)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeProcessingException"/> class
		/// with a specified error message, line number, column number, source fragment
		/// and reference to the inner exception that is the cause of this exception
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		/// <param name="lineNumber">Line number</param>
		/// <param name="columnNumber">Column number</param>
		/// <param name="sourceFragment">Source fragment</param>
		/// <param name="innerException">Exception that is the cause of the current exception</param>
		protected CodeProcessingException(string message, int lineNumber, int columnNumber,
			string sourceFragment, Exception innerException) : base(message, innerException)
		{
			LineNumber = lineNumber;
			ColumnNumber = columnNumber;
			SourceFragment = sourceFragment;
		}
	}
}