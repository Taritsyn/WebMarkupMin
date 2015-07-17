using System;

using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// The exception that is thrown when a parsing of HTML content is failed
	/// </summary>
	internal sealed class HtmlParsingException : CodeProcessingException
	{
		/// <summary>
		/// Initializes a new instance of the WebMarkupMin.Core.Parsers.HtmlParsingException class
		/// with a specified error message
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		public HtmlParsingException(string message)
			: base(message)
		{ }

		/// <summary>
		/// Initializes a new instance of the WebMarkupMin.Core.Parsers.HtmlParsingException class
		/// with a specified error message and reference to the inner exception that is
		/// the cause of this exception
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		/// <param name="innerException">Exception that is the cause of the current exception</param>
		public HtmlParsingException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		/// <summary>
		/// Initializes a new instance of the WebMarkupMin.Core.Parsers.HtmlParsingException class
		/// with a specified error message, node coordinates and source fragment
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		/// <param name="nodeCoordinates">Node coordinates</param>
		/// <param name="sourceFragment">Source fragment</param>
		public HtmlParsingException(string message, SourceCodeNodeCoordinates nodeCoordinates, string sourceFragment)
			: base(message, nodeCoordinates, sourceFragment)
		{ }

		/// <summary>
		/// Initializes a new instance of the WebMarkupMin.Core.Parsers.HtmlParsingException class
		/// with a specified error message, line number, column number and source fragment
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		/// <param name="lineNumber">Line number</param>
		/// <param name="columnNumber">Column number</param>
		/// <param name="sourceFragment">SourceFragment</param>
		public HtmlParsingException(string message, int lineNumber, int columnNumber, string sourceFragment)
			: base(message, lineNumber, columnNumber, sourceFragment)
		{ }

		/// <summary>
		/// Initializes a new instance of the WebMarkupMin.Core.Parsers.HtmlParsingException class
		/// with a specified error message, node coordinates, source fragment
		/// and reference to the inner exception that is the cause of this exception
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		/// <param name="nodeCoordinates">Node coordinates</param>
		/// <param name="sourceFragment">Source fragment</param>
		/// <param name="innerException">Exception that is the cause of the current exception</param>
		public HtmlParsingException(string message, SourceCodeNodeCoordinates nodeCoordinates,
			string sourceFragment, Exception innerException)
			: base(message, nodeCoordinates, sourceFragment, innerException)
		{ }

		/// <summary>
		/// Initializes a new instance of the WebMarkupMin.Core.Parsers.HtmlParsingException class
		/// with a specified error message, line number, column number, source fragment
		/// and reference to the inner exception that is the cause of this exception
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		/// <param name="lineNumber">Line number</param>
		/// <param name="columnNumber">Column number</param>
		/// <param name="sourceFragment">Source fragment</param>
		/// <param name="innerException">Exception that is the cause of the current exception</param>
		public HtmlParsingException(string message, int lineNumber, int columnNumber,
			string sourceFragment, Exception innerException)
			: base(message, lineNumber, columnNumber, sourceFragment, innerException)
		{ }
	}
}