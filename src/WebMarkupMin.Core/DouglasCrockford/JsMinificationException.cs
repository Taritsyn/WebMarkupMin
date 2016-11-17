using System;
#if !NETSTANDARD1_3
using System.Runtime.Serialization;
#endif

namespace WebMarkupMin.Core.DouglasCrockford
{
	/// <summary>
	/// The exception that is thrown when a minification of asset code by JSMin is failed
	/// </summary>
#if !NETSTANDARD1_3
	[Serializable]
#endif
	internal sealed class JsMinificationException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="JsMinificationException"/> class
		/// with a specified error message
		/// </summary>
		/// <param name="message">The message that describes the error</param>
		public JsMinificationException(string message)
			: base(message)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="JsMinificationException"/> class
		/// with a specified error message and a reference to the inner exception that is the cause of
		/// this exception
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception</param>
		/// <param name="innerException">The exception that is the cause of the current exception</param>
		public JsMinificationException(string message, Exception innerException)
			: base(message, innerException)
		{ }
#if !NETSTANDARD1_3

		/// <summary>
		/// Initializes a new instance of the <see cref="JsMinificationException"/> class with serialized data
		/// </summary>
		/// <param name="info">The object that holds the serialized data</param>
		/// <param name="context">The contextual information about the source or destination</param>
		private JsMinificationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
#endif
	}
}