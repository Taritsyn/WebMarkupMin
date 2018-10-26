﻿using System.Collections.Generic;

using NUglify;

using WebMarkupMin.Core;

namespace WebMarkupMin.NUglify.Reporters
{
	/// <summary>
	/// NUglify error reporter
	/// </summary>
	internal sealed class NUglifyErrorReporter
	{
		/// <summary>
		/// Warning level
		/// </summary>
		private readonly int _warningLevel;

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
		/// Constructs an instance of the NUglify error reporter
		/// </summary>
		public NUglifyErrorReporter()
			: this(2)
		{ }

		/// <summary>
		/// Constructs an instance of the NUglify error reporter
		/// </summary>
		/// <param name="warningLevel">Warning level</param>
		public NUglifyErrorReporter(int warningLevel)
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
			UglifyError error = args.Error;
			if (error.Severity <= _warningLevel)
			{
				var errorDetails = new MinificationErrorInfo(error.Message, error.StartLine, error.StartColumn,
					string.Empty);
				if (error.IsError)
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