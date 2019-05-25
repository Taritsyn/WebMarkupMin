using System.Collections.Generic;

using Microsoft.Ajax.Utilities;

using WebMarkupMin.Core;

namespace WebMarkupMin.MsAjax.Reporters
{
	/// <summary>
	/// Microsoft Ajax error reporter
	/// </summary>
	internal sealed class MsAjaxErrorReporter
	{
		/// <summary>
		/// Warning level threshold for reporting errors
		/// </summary>
		private readonly int _warningLevel;

		/// <summary>
		/// List of the errors
		/// </summary>
		private readonly List<MinificationErrorInfo> _errors;

		/// <summary>
		/// List of the warnings
		/// </summary>
		private readonly List<MinificationErrorInfo> _warnings;

		/// <summary>
		/// Gets a list of the errors
		/// </summary>
		public List<MinificationErrorInfo> Errors
		{
			get { return _errors; }
		}

		/// <summary>
		/// Gets a list of the warnings
		/// </summary>
		public List<MinificationErrorInfo> Warnings
		{
			get { return _warnings; }
		}


		/// <summary>
		/// Constructs an instance of the Microsoft Ajax error reporter
		/// </summary>
		/// <param name="warningLevel">Warning level threshold for reporting errors</param>
		public MsAjaxErrorReporter(int warningLevel)
		{
			_warningLevel = warningLevel;
			_errors = new List<MinificationErrorInfo>();
			_warnings = new List<MinificationErrorInfo>();
		}


		/// <summary>
		/// Parse error handler
		/// </summary>
		/// <param name="source">The source of the event</param>
		/// <param name="args">A <see cref="ContextErrorEventArgs"/> that contains the event data</param>
		public void ParseErrorHandler(object source, ContextErrorEventArgs args)
		{
			ContextError error = args.Error;
			if (error.Severity <= _warningLevel)
			{
				var errorDetails = new MinificationErrorInfo(error.Message, error.StartLine, error.StartColumn,
					string.Empty);
				if (error.Severity < 1)
				{
					_errors.Add(errorDetails);
				}
				else
				{
					_warnings.Add(errorDetails);
				}
			}
		}

		/// <summary>
		/// Clears a error and warning lists
		/// </summary>
		public void Clear()
		{
			_errors.Clear();
			_warnings.Clear();
		}
	}
}