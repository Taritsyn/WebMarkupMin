using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// Markup parsing context
	/// </summary>
	internal sealed class MarkupParsingContext
	{
		/// <summary>
		/// Inner markup parsing context
		/// </summary>
		private readonly InnerMarkupParsingContext _innerContext;

		/// <summary>
		/// Gets a source code
		/// </summary>
		public string SourceCode
		{
			get { return _innerContext.SourceCode; }
		}

		/// <summary>
		/// Gets a current parsing position
		/// </summary>
		public int Position
		{
			get { return _innerContext.Position; }
		}

		/// <summary>
		/// Gets a length of the source code
		/// </summary>
		public int Length
		{
			get { return _innerContext.Length; }
		}

		/// <summary>
		/// Gets a node coordinates
		/// </summary>
		public SourceCodeNodeCoordinates NodeCoordinates
		{
			get { return _innerContext.NodeCoordinates; }
		}


		/// <summary>
		/// Constructs instance of markup parsing context
		/// </summary>
		/// <param name="innerContext">Inner markup parsing context</param>
		internal MarkupParsingContext(InnerMarkupParsingContext innerContext)
		{
			_innerContext = innerContext;
		}
	}
}