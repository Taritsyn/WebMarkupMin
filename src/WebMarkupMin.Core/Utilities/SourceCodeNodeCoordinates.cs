namespace WebMarkupMin.Core.Utilities
{
	/// <summary>
	/// Source code node coordinates
	/// </summary>
	public struct SourceCodeNodeCoordinates
	{
		/// <summary>
		/// Line number
		/// </summary>
		private int _lineNumber;

		/// <summary>
		/// Column number
		/// </summary>
		private int _columnNumber;

		/// <summary>
		/// Gets or sets a line number
		/// </summary>
		public int LineNumber
		{
			get { return _lineNumber; }
			set { _lineNumber = value; }
		}

		/// <summary>
		/// Gets or sets a column number
		/// </summary>
		public int ColumnNumber
		{
			get { return _columnNumber; }
			set { _columnNumber = value; }
		}

		/// <summary>
		/// Represents a node coordinates that has line number and column number values set to zero.
		/// </summary>
		public static readonly SourceCodeNodeCoordinates Empty = new SourceCodeNodeCoordinates(0, 0);

		/// <summary>
		/// Gets a value indicating whether this node coordinates is empty
		/// </summary>
		public bool IsEmpty
		{
			get { return (LineNumber == 0 && ColumnNumber == 0); }
		}


		/// <summary>
		/// Constructs instance of source code node coordinates
		/// </summary>
		/// <param name="lineNumber">Line number</param>
		/// <param name="columnNumber">Column number</param>
		public SourceCodeNodeCoordinates(int lineNumber, int columnNumber)
		{
			_lineNumber = lineNumber;
			_columnNumber = columnNumber;
		}
	}
}