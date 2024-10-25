using System;
using System.Linq;

using Microsoft.Extensions.Logging;

using WebMarkupMin.Core;

using IWmmLogger = WebMarkupMin.Core.Loggers.ILogger;

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2
#elif ASPNETCORE3
namespace WebMarkupMin.AspNetCore3
#elif ASPNETCORE5
namespace WebMarkupMin.AspNetCore5
#elif ASPNETCORE6
namespace WebMarkupMin.AspNetCore6
#elif ASPNETCORE7
namespace WebMarkupMin.AspNetCore7
#elif ASPNETCORE8
namespace WebMarkupMin.AspNetCore8
#else
#error No implementation for this target
#endif
{
	/// <summary>
	/// Wrapper around the standard ASP.NET Core logger.
	/// Used to integrate the WebMarkupMin middleware with the Microsoft.Extensions.Logging library.
	/// </summary>
	public sealed class AspNetCoreLogger : IWmmLogger
	{
		/// <summary>
		/// Template of log message
		/// </summary>
		const string MESSAGE_TEMPLATE = @"{Category}: {Description}
   at {DocumentUrl}";

		/// <summary>
		/// Template of log message with source code coordinates
		/// </summary>
		const string MESSAGE_TEMPLATE_WITH_COORDINATES = MESSAGE_TEMPLATE +
			@":{LineNumber}:{ColumnNumber} -> {SourceFragment}";

		/// <summary>
		/// Template of log message with minification statistics
		/// </summary>
		const string MESSAGE_TEMPLATE_WITH_MINIFICATION_STATISTICS = MESSAGE_TEMPLATE +
			@"

Original size: {OriginalSize:N0} bytes
Minified size: {MinifiedSize:N0} bytes
Saved: {SavedInBytes:N0} bytes ({SavedInPercent:N2}%)
Minification duration: {MinificationDuration:N0} ms";

		/// <summary>
		/// Array of characters used to find the newline
		/// </summary>
		private static readonly char[] _newLineChars = new char[] { '\n', '\r' };

		/// <summary>
		/// Standard ASP.NET Core logger
		/// </summary>
		private readonly ILogger<AspNetCoreLogger> _logger;


		/// <summary>
		/// Constructs an instance of wrapper around the standard ASP.NET Core logger
		/// </summary>
		/// <param name="logger">Standard ASP.NET Core logger</param>
		public AspNetCoreLogger(ILogger<AspNetCoreLogger> logger)
		{
			_logger = logger;
		}


		/// <summary>
		/// Gets a source line from source fragment
		/// </summary>
		/// <param name="sourceFragment">Source fragment</param>
		/// <param name="lineNumber">Line number</param>
		/// <returns>Source line</returns>
		private static string GetSourceLineFromFragment(string sourceFragment, int lineNumber)
		{
			string linePrefix = string.Format("Line {0}: ", lineNumber);
			int lineBeginPosition = sourceFragment.IndexOf(linePrefix, StringComparison.Ordinal);

			if (lineBeginPosition == -1
				|| !(lineBeginPosition == 0 || _newLineChars.Contains(sourceFragment[lineBeginPosition - 1])))
			{
				return sourceFragment;
			}

			int linePrefixLength = linePrefix.Length;
			int lineContentBeginPosition = lineBeginPosition + linePrefixLength;

			int newLinePosition = sourceFragment.IndexOfAny(_newLineChars, lineContentBeginPosition);
			int lineContentLength = newLinePosition != -1 ?
				newLinePosition - linePrefixLength
				:
				sourceFragment.Length - linePrefixLength
				;

			string sourceLine = sourceFragment.Substring(lineContentBeginPosition, lineContentLength);

			return sourceLine;
		}

		#region IJsEngine implementation

		/// <inheritdoc/>
		public void Error(string category, string message, string filePath = "", int lineNumber = 0,
			int columnNumber = 0, string sourceFragment = "")
		{
			if (lineNumber > 0)
			{
				string sourceLine = GetSourceLineFromFragment(sourceFragment, lineNumber);

				_logger.LogError(MESSAGE_TEMPLATE_WITH_COORDINATES,
					category, message, filePath, lineNumber, columnNumber, sourceLine);
			}
			else
			{
				_logger.LogError(MESSAGE_TEMPLATE, category, message, filePath);
			}
		}

		/// <inheritdoc/>
		public void Warn(string category, string message, string filePath = "", int lineNumber = 0,
			int columnNumber = 0, string sourceFragment = "")
		{
			if (lineNumber > 0)
			{
				string sourceLine = GetSourceLineFromFragment(sourceFragment, lineNumber);

				_logger.LogWarning(MESSAGE_TEMPLATE_WITH_COORDINATES,
					category, message, filePath, lineNumber, columnNumber, sourceLine);
			}
			else
			{
				_logger.LogWarning(MESSAGE_TEMPLATE, category, message, filePath);
			}
		}

		/// <inheritdoc/>
		public void Debug(string category, string message, string filePath = "")
		{
			_logger.LogDebug(MESSAGE_TEMPLATE, category, message, filePath);
		}

		/// <inheritdoc/>
		public void Info(string category, string message, string filePath = "",
			MinificationStatistics statistics = null)
		{
			if (statistics != null)
			{
				_logger.LogInformation(MESSAGE_TEMPLATE_WITH_MINIFICATION_STATISTICS, category, message, filePath,
					statistics.OriginalSize, statistics.MinifiedSize, statistics.SavedInBytes,
					statistics.SavedInPercent, statistics.MinificationDuration);
			}
			else
			{
				_logger.LogInformation(MESSAGE_TEMPLATE, category, message, filePath);
			}
		}

		#endregion
	}
}