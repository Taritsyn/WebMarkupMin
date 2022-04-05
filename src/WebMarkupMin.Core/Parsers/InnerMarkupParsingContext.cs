﻿using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// Inner markup parsing context
	/// </summary>
	internal sealed class InnerMarkupParsingContext
	{
		/// <summary>
		/// Source code
		/// </summary>
		private readonly string _sourceCode;

		/// <summary>
		/// Current parsing position
		/// </summary>
		private int _position;

		/// <summary>
		/// Node coordinates
		/// </summary>
		private SourceCodeNodeCoordinates _nodeCoordinates;

		/// <summary>
		/// Offset for peeked character
		/// </summary>
		private int _peekedCharOffset;

		/// <summary>
		/// Gets a source code
		/// </summary>
		public string SourceCode
		{
			get { return _sourceCode; }
		}

		/// <summary>
		/// Gets or sets a current parsing position
		/// </summary>
		public int Position
		{
			get { return _position; }
		}

		/// <summary>
		/// Gets a length of the source code
		/// </summary>
		public int Length
		{
			get { return _sourceCode.Length; }
		}

		/// <summary>
		/// Gets a length of the source code remainder
		/// </summary>
		public int RemainderLength
		{
			get { return _sourceCode.Length - _position; }
		}

		/// <summary>
		/// Gets a node coordinates
		/// </summary>
		public SourceCodeNodeCoordinates NodeCoordinates
		{
			get { return _nodeCoordinates; }
		}


		/// <summary>
		/// Constructs instance of inner markup parsing context
		/// </summary>
		/// <param name="sourceCode">Source code</param>
		public InnerMarkupParsingContext(string sourceCode)
		{
			_sourceCode = sourceCode;
			_position = 0;
			_nodeCoordinates = new SourceCodeNodeCoordinates(1, 1);
			_peekedCharOffset = 0;
		}

		/// <summary>
		/// Increases a current parsing position
		/// </summary>
		/// <param name="increment">Increment</param>
		public void IncreasePosition(int increment)
		{
			int oldPosition = _position;
			int newPosition = oldPosition + increment;

			int fragmentStartPosition = oldPosition;
			int fragmentLength = increment;

			int lineCount;
			int charRemainderCount;

			SourceCodeNavigator.CalculateLineCount(_sourceCode, fragmentStartPosition, fragmentLength,
				out lineCount, out charRemainderCount);
			SourceCodeNodeCoordinates currentNodeCoordinates = _nodeCoordinates;

			_nodeCoordinates = SourceCodeNavigator.CalculateAbsoluteNodeCoordinates(currentNodeCoordinates,
				lineCount, charRemainderCount);
			_position = newPosition;
			_peekedCharOffset = 0;
		}

		/// <summary>
		/// Gets a source fragment
		/// </summary>
		/// <returns>Source fragment</returns>
		public string GetSourceFragment()
		{
			return SourceCodeNavigator.GetSourceFragment(_sourceCode, _nodeCoordinates);
		}

		/// <summary>
		/// Returns the current character without changing the current position
		/// </summary>
		/// <returns>Current character</returns>
		public char PeekCurrentChar()
		{
			int currentCharPosition = _position + _peekedCharOffset;
			char peekedChar;

			if (currentCharPosition < _sourceCode.Length)
			{
				peekedChar = _sourceCode[currentCharPosition];
			}
			else
			{
				peekedChar = '\0';
			}

			return peekedChar;
		}

		/// <summary>
		/// Returns the next character without changing the current position
		/// </summary>
		/// <returns>Next character</returns>
		public char PeekNextChar()
		{
			_peekedCharOffset++;

			int nextCharPosition = _position + _peekedCharOffset;
			char peekedChar;

			if (nextCharPosition < _sourceCode.Length)
			{
				peekedChar = _sourceCode[nextCharPosition];
			}
			else
			{
				peekedChar = '\0';
			}

			return peekedChar;
		}
	}
}