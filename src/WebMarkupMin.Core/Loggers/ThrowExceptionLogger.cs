using System;
using System.Text;

using WebMarkupMin.Core.Resources;
using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Loggers
{
	/// <summary>
	/// Logger, which throws exceptions when errors found
	/// </summary>
	public sealed class ThrowExceptionLogger : LoggerBase
	{
		/// <summary>
		/// Logs a information about the error
		/// </summary>
		/// <param name="category">Error category</param>
		/// <param name="message">Error message</param>
		/// <param name="filePath">File path</param>
		/// <param name="lineNumber">Line number on which the error occurred</param>
		/// <param name="columnNumber">Column number on which the error occurred</param>
		/// <param name="sourceFragment">Fragment of source code</param>
		public override void Error(string category, string message, string filePath = "",
			int lineNumber = 0, int columnNumber = 0, string sourceFragment = "")
		{
			var errorMessage = new StringBuilder();
			errorMessage.AppendFormatLine("{0}: {1}", Strings.ErrorDetails_Category, category);
			errorMessage.AppendFormatLine("{0}: {1}", Strings.ErrorDetails_Message, message);

			if (!string.IsNullOrWhiteSpace(filePath))
			{
				errorMessage.AppendFormatLine("{0}: {1}", Strings.ErrorDetails_File, filePath);
			}

			if (lineNumber > 0)
			{
				errorMessage.AppendFormatLine("{0}: {1}", Strings.ErrorDetails_LineNumber, lineNumber);
			}

			if (columnNumber > 0)
			{
				errorMessage.AppendFormatLine("{0}: {1}", Strings.ErrorDetails_ColumnNumber, columnNumber);
			}

			if (!string.IsNullOrWhiteSpace(sourceFragment))
			{
				errorMessage.AppendFormatLine("{1}:{0}{0}{2}", Environment.NewLine,
					Strings.ErrorDetails_SourceFragment, sourceFragment);
			}

			throw new MarkupMinificationException(errorMessage.ToString());
		}
	}
}