namespace WebMarkupMin.Core.Loggers
{
	/// <summary>
	/// Base class for the loggers
	/// </summary>
	public abstract class LoggerBase : ILogger
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
		public virtual void Error(string category, string message,
			string filePath = "", int lineNumber = 0, int columnNumber = 0,
			string sourceFragment = "")
		{ }

		/// <summary>
		/// Logs a information about the warning
		/// </summary>
		/// <param name="category">Warning category</param>
		/// <param name="message">Warning message</param>
		/// <param name="filePath">File path</param>
		/// <param name="lineNumber">Line number on which the warning occurred</param>
		/// <param name="columnNumber">Column number on which the warning occurred</param>
		/// <param name="sourceFragment">Fragment of source code</param>
		public virtual void Warn(string category, string message,
			string filePath = "", int lineNumber = 0, int columnNumber = 0,
			string sourceFragment = "")
		{ }

		/// <summary>
		/// Logs a debug information
		/// </summary>
		/// <param name="category">Debug category</param>
		/// <param name="message">Debug message</param>
		/// <param name="filePath">File path</param>
		public virtual void Debug(string category, string message, string filePath = "")
		{ }

		/// <summary>
		/// Logs a information
		/// </summary>
		/// <param name="category">Information category</param>
		/// <param name="message">Information message</param>
		/// <param name="statistics">Minification statistics</param>
		/// <param name="filePath">File path</param>
		public virtual void Info(string category, string message, string filePath = "", MinificationStatistics statistics = null)
		{ }
	}
}