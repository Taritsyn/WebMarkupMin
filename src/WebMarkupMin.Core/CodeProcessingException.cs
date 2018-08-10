using System;
#if !NETSTANDARD1_3
using System.Runtime.Serialization;
using System.Security.Permissions;
#endif

using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// The exception that is thrown when a processing of code is failed
	/// </summary>
#if !NETSTANDARD1_3
	[Serializable]
#endif
	public abstract class CodeProcessingException : Exception
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
		/// Source fragment
		/// </summary>
		private string _sourceFragment = string.Empty;

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
		/// Gets or sets a source fragment
		/// </summary>
		public string SourceFragment
		{
			get { return _sourceFragment; }
			set { _sourceFragment = value; }
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="CodeProcessingException"/> class
		/// with a specified error message
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		protected CodeProcessingException(string message)
			: base(message)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeProcessingException"/> class
		/// with a specified error message and reference to the inner exception that is
		/// the cause of this exception
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		/// <param name="innerException">Exception that is the cause of the current exception</param>
		protected CodeProcessingException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeProcessingException"/> class
		/// with a specified error message, node coordinates and source fragment
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		/// <param name="nodeCoordinates">Node coordinates</param>
		/// <param name="sourceFragment">Source fragment</param>
		protected CodeProcessingException(string message, SourceCodeNodeCoordinates nodeCoordinates, string sourceFragment)
			: this(message, nodeCoordinates, sourceFragment, null)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeProcessingException"/> class
		/// with a specified error message, line number, column number and source fragment
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		/// <param name="lineNumber">Line number</param>
		/// <param name="columnNumber">Column number</param>
		/// <param name="sourceFragment">SourceFragment</param>
		protected CodeProcessingException(string message, int lineNumber, int columnNumber, string sourceFragment)
			: this(message, lineNumber, columnNumber, sourceFragment, null)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeProcessingException"/> class
		/// with a specified error message, node coordinates, source fragment
		/// and reference to the inner exception that is the cause of this exception
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		/// <param name="nodeCoordinates">Node coordinates</param>
		/// <param name="sourceFragment">Source fragment</param>
		/// <param name="innerException">Exception that is the cause of the current exception</param>
		protected CodeProcessingException(string message, SourceCodeNodeCoordinates nodeCoordinates,
			string sourceFragment, Exception innerException)
			: this(message, nodeCoordinates.LineNumber, nodeCoordinates.ColumnNumber,
				sourceFragment, innerException)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeProcessingException"/> class
		/// with a specified error message, line number, column number, source fragment
		/// and reference to the inner exception that is the cause of this exception
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		/// <param name="lineNumber">Line number</param>
		/// <param name="columnNumber">Column number</param>
		/// <param name="sourceFragment">Source fragment</param>
		/// <param name="innerException">Exception that is the cause of the current exception</param>
		protected CodeProcessingException(string message, int lineNumber, int columnNumber,
			string sourceFragment, Exception innerException)
			: base(message, innerException)
		{
			_lineNumber = lineNumber;
			_columnNumber = columnNumber;
			_sourceFragment = sourceFragment;
		}
#if !NETSTANDARD1_3

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeProcessingException"/> class with serialized data
		/// </summary>
		/// <param name="info">The object that holds the serialized data</param>
		/// <param name="context">The contextual information about the source or destination</param>
		protected CodeProcessingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			if (info != null)
			{
				_lineNumber = info.GetInt32("LineNumber");
				_columnNumber = info.GetInt32("ColumnNumber");
				_sourceFragment = info.GetString("SourceFragment");
			}
		}

		#region Exception overrides

		/// <summary>
		/// Populates a <see cref="SerializationInfo"/> with the data needed to serialize the target object
		/// </summary>
		/// <param name="info">The <see cref="SerializationInfo"/> to populate with data</param>
		/// <param name="context">The destination (see <see cref="StreamingContext"/>) for this serialization</param>
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException(nameof(info));
			}

			base.GetObjectData(info, context);
			info.AddValue("LineNumber", _lineNumber);
			info.AddValue("ColumnNumber", _columnNumber);
			info.AddValue("SourceFragment", _sourceFragment);
		}

		#endregion
#endif
	}
}