using System;

using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Parsers.Processors
{
	/// <summary>
	/// Markup processor
	/// </summary>
	internal abstract class MarkupProcessorBase : IDisposable
	{
		protected InnerMarkupParsingContext _innerContext;
		private int _initialPosition = -1;


		/// <summary>
		/// Constructs an instance of the markup processor
		/// </summary>
		/// <param name="innerContext">Inner markup parsing context</param>
		public MarkupProcessorBase(InnerMarkupParsingContext innerContext)
		{
			_innerContext = innerContext;
		}


		protected void SaveInitialPosition()
		{
			_initialPosition = _innerContext.Position;
		}

		protected void RestoreInitialPosition(out int charOffset)
		{
			if (_initialPosition < 0)
			{
				throw new IndexOutOfRangeException();
			}

			charOffset = _innerContext.Position - _initialPosition;
			_innerContext.Position = _initialPosition;
		}

		protected bool ProcessWhitespace()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int currentPosition = _innerContext.Position;

			int nonWhitespaceCharPosition = content.IndexOfNonWhitespace(currentPosition,
				_innerContext.RemainderLength);
			if (nonWhitespaceCharPosition != -1)
			{
				int whitespaceLength = nonWhitespaceCharPosition - currentPosition;
				if (whitespaceLength > 0)
				{
					_innerContext.IncreasePosition(whitespaceLength);
					isProcessed = true;
				}
			}

			return isProcessed;
		}


		#region IDisposable implementation

		/// <summary>
		/// Disposes the HTML document type processor
		/// </summary>
		public void Dispose()
		{
			_innerContext = null;
			_initialPosition = -1;
		}

		#endregion
	}
}