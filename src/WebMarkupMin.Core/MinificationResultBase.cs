using System.Collections.Generic;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// Base class for the minification results
	/// </summary>
	public abstract class MinificationResultBase
	{
		/// <summary>
		/// Gets or sets a minified content
		/// </summary>
		public string MinifiedContent
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets a list of the errors
		/// </summary>
		public IList<MinificationErrorInfo> Errors
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets a list of the warnings
		/// </summary>
		public IList<MinificationErrorInfo> Warnings
		{
			get;
			private set;
		}


		/// <summary>
		/// Constructs instance of minification result
		/// </summary>
		/// <param name="minifiedContent">Minified content</param>
		protected MinificationResultBase(string minifiedContent)
			: this(minifiedContent, new List<MinificationErrorInfo>(), new List<MinificationErrorInfo>())
		{ }

		/// <summary>
		/// Constructs instance of minification result
		/// </summary>
		/// <param name="minifiedContent">Minified content</param>
		/// <param name="errors">List of the errors</param>
		protected MinificationResultBase(string minifiedContent, IList<MinificationErrorInfo> errors)
			: this(minifiedContent, errors, new List<MinificationErrorInfo>())
		{ }

		/// <summary>
		/// Constructs instance of minification result
		/// </summary>
		/// <param name="minifiedContent">Minified content</param>
		/// <param name="errors">List of the errors</param>
		/// <param name="warnings">List of the warnings</param>
		protected MinificationResultBase(string minifiedContent, IList<MinificationErrorInfo> errors, IList<MinificationErrorInfo> warnings)
		{
			MinifiedContent = minifiedContent;
			Errors = errors;
			Warnings = warnings;
		}
	}
}