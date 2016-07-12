using System.Collections.Generic;

using EcmaScript.NET;

using WebMarkupMin.Core;

namespace WebMarkupMin.Yui.Reporters
{
	/// <summary>
	/// YUI JS error reporter
	/// </summary>
	internal sealed class YuiJsErrorReporter : ErrorReporter
	{
		/// <summary>
		/// List of the errors
		/// </summary>
		private readonly IList<MinificationErrorInfo> _errors;

		/// <summary>
		/// List of the warnings
		/// </summary>
		private readonly IList<MinificationErrorInfo> _warnings;

		/// <summary>
		/// Gets a list of the errors
		/// </summary>
		public IList<MinificationErrorInfo> Errors
		{
			get { return _errors; }
		}

		/// <summary>
		/// Gets a list of the warnings
		/// </summary>
		public IList<MinificationErrorInfo> Warnings
		{
			get { return _warnings; }
		}


		/// <summary>
		/// Constructs an instance of the YUI JS error reporter
		/// </summary>
		public YuiJsErrorReporter()
		{
			_errors = new List<MinificationErrorInfo>();
			_warnings = new List<MinificationErrorInfo>();
		}


		/// <summary>
		/// Creates a ECMAScript runtime exception
		/// </summary>
		/// <param name="message">Message</param>
		/// <param name="sourceName">Fragment of source code</param>
		/// <param name="line">Line number</param>
		/// <param name="lineSource">Line content</param>
		/// <param name="lineOffset">Column number</param>
		/// <returns>ECMAScript runtime exception</returns>
		public EcmaScriptRuntimeException RuntimeError(string message, string sourceName, int line,
			string lineSource, int lineOffset)
		{
			return new EcmaScriptRuntimeException(message, sourceName, line, lineSource, lineOffset);
		}

		/// <summary>
		/// Reports a information about the error
		/// </summary>
		/// <param name="message">Message</param>
		/// <param name="sourceName">Fragment of source code</param>
		/// <param name="line">Line number</param>
		/// <param name="lineSource">Line content</param>
		/// <param name="lineOffset">Column number</param>
		public void Error(string message, string sourceName, int line, string lineSource, int lineOffset)
		{
			_errors.Add(new MinificationErrorInfo(message, line, lineOffset, lineSource));
		}

		/// <summary>
		/// Reports a information about the warning
		/// </summary>
		/// <param name="message">Message</param>
		/// <param name="sourceName">Fragment of source code</param>
		/// <param name="line">Line number</param>
		/// <param name="lineSource">Line content</param>
		/// <param name="lineOffset">Column number</param>
		public void Warning(string message, string sourceName, int line, string lineSource, int lineOffset)
		{
			_warnings.Add(new MinificationErrorInfo(message, line, lineOffset, lineSource));
		}
	}
}