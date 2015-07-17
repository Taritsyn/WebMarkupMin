using Microsoft.Ajax.Utilities;

using WebMarkupMin.Core;

namespace WebMarkupMin.MsAjax.Reporters
{
	/// <summary>
	/// MS Ajax JS error reporter
	/// </summary>
	internal sealed class MsAjaxJsErrorReporter : MsAjaxErrorReporterBase
	{
		/// <summary>
		/// JS minification error handler
		/// </summary>
		/// <param name="source">The source of the event</param>
		/// <param name="args">A Microsoft.Ajax.Utilities.ContextErrorEventArgs
		/// that contains the event data</param>
		public void JsMinificationErrorHandler(object source, ContextErrorEventArgs args)
		{
			ContextError error = args.Error;
			if (error.Severity <= 2)
			{
				var errorDetails = new MinificationErrorInfo(error.Message, error.StartLine, error.StartColumn, string.Empty);
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
	}
}