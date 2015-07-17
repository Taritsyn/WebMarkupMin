using System.Collections.Generic;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// Markup minification result
	/// </summary>
	public sealed class MarkupMinificationResult : MinificationResultBase
	{
		/// <summary>
		/// Gets a minification statistics
		/// </summary>
		public MinificationStatistics Statistics
		{
			get;
			private set;
		}


		/// <summary>
		/// Constructs instance of markup minification result
		/// </summary>
		/// <param name="minifiedContent">Minified content</param>
		public MarkupMinificationResult(string minifiedContent)
			: this(minifiedContent, new List<MinificationErrorInfo>(), new List<MinificationErrorInfo>(), null)
		{ }

		/// <summary>
		/// Constructs instance of markup minification result
		/// </summary>
		/// <param name="minifiedContent">Minified content</param>
		/// <param name="errors">List of the errors</param>
		public MarkupMinificationResult(string minifiedContent, IList<MinificationErrorInfo> errors)
			: this(minifiedContent, errors, new List<MinificationErrorInfo>(), null)
		{ }

		/// <summary>
		/// Constructs instance of markup minification result
		/// </summary>
		/// <param name="minifiedContent">Minified content</param>
		/// <param name="statistics">Minification statistics</param>
		public MarkupMinificationResult(string minifiedContent, MinificationStatistics statistics)
			: this(minifiedContent, new List<MinificationErrorInfo>(), new List<MinificationErrorInfo>(), statistics)
		{ }

		/// <summary>
		/// Constructs instance of markup minification result
		/// </summary>
		/// <param name="minifiedContent">Minified content</param>
		/// <param name="errors">List of the errors</param>
		/// <param name="statistics">Minification statistics</param>
		public MarkupMinificationResult(string minifiedContent, IList<MinificationErrorInfo> errors, MinificationStatistics statistics)
			: this(minifiedContent, errors, new List<MinificationErrorInfo>(), statistics)
		{ }

		/// <summary>
		/// Constructs instance of markup minification result
		/// </summary>
		/// <param name="minifiedContent">Minified content</param>
		/// <param name="errors">List of the errors</param>
		/// <param name="warnings">List of the warnings</param>
		public MarkupMinificationResult(string minifiedContent, IList<MinificationErrorInfo> errors, IList<MinificationErrorInfo> warnings)
			: this(minifiedContent, errors, warnings, null)
		{ }

		/// <summary>
		/// Constructs instance of markup minification result
		/// </summary>
		/// <param name="minifiedContent">Minified content</param>
		/// <param name="errors">List of the errors</param>
		/// <param name="warnings">List of the warnings</param>
		/// <param name="statistics">Minification statistics</param>
		public MarkupMinificationResult(string minifiedContent, IList<MinificationErrorInfo> errors, IList<MinificationErrorInfo> warnings,
			MinificationStatistics statistics)
			: base(minifiedContent, errors, warnings)
		{
			Statistics = statistics;
		}
	}
}