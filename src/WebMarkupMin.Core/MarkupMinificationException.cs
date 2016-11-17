using System;
#if !NETSTANDARD1_3
using System.Runtime.Serialization;
#endif

namespace WebMarkupMin.Core
{
	/// <summary>
	/// The exception that is thrown when a markup minification is failed
	/// </summary>
#if !NETSTANDARD1_3
	[Serializable]
#endif
	public sealed class MarkupMinificationException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MarkupMinificationException"/> class
		/// with a specified error message
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		public MarkupMinificationException(string message)
			: base(message)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="MarkupMinificationException"/> class
		/// with a specified error message and reference to the inner exception that is
		/// the cause of this exception
		/// </summary>
		/// <param name="message">Error message that explains the reason for the exception</param>
		/// <param name="innerException">Exception that is the cause of the current exception</param>
		public MarkupMinificationException(string message, Exception innerException)
			: base(message, innerException)
		{ }
#if !NETSTANDARD1_3

		/// <summary>
		/// Initializes a new instance of the <see cref="MarkupMinificationException"/> class with serialized data
		/// </summary>
		/// <param name="info">The object that holds the serialized data</param>
		/// <param name="context">The contextual information about the source or destination</param>
		private MarkupMinificationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
#endif
	}
}