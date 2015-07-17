using System.Collections.Generic;

using WebMarkupMin.Core;

namespace WebMarkupMin.MsAjax.Reporters
{
	/// <summary>
	/// MS Ajax error reporter
	/// </summary>
	internal abstract class MsAjaxErrorReporterBase
	{
		/// <summary>
		/// List of the errors
		/// </summary>
		protected IList<MinificationErrorInfo> _errors;

		/// <summary>
		/// List of the warnings
		/// </summary>
		protected IList<MinificationErrorInfo> _warnings;

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
		/// Constructs instance of MS Ajax error reporter
		/// </summary>
		protected MsAjaxErrorReporterBase()
		{
			_errors = new List<MinificationErrorInfo>();
			_warnings = new List<MinificationErrorInfo>();
		}
	}
}