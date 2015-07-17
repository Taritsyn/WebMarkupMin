using System.Collections.Generic;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// Code minification result
	/// </summary>
	public sealed class CodeMinificationResult : MinificationResultBase
	{
		/// <summary>
		/// Constructs instance of code minification result
		/// </summary>
		/// <param name="minifiedContent">Minified content</param>
		public CodeMinificationResult(string minifiedContent)
			: base(minifiedContent)
		{ }

		/// <summary>
		/// Constructs instance of code minification result
		/// </summary>
		/// <param name="minifiedContent">Minified content</param>
		/// <param name="errors">List of the errors</param>
		public CodeMinificationResult(string minifiedContent, IList<MinificationErrorInfo> errors)
			: base(minifiedContent, errors)
		{ }

		/// <summary>
		/// Constructs instance of code minification result
		/// </summary>
		/// <param name="minifiedContent">Minified content</param>
		/// <param name="errors">List of the errors</param>
		/// <param name="warnings">List of the warnings</param>
		public CodeMinificationResult(string minifiedContent, IList<MinificationErrorInfo> errors,
			IList<MinificationErrorInfo> warnings)
			: base(minifiedContent, errors, warnings)
		{ }
	}
}