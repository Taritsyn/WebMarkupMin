using System;
using System.Globalization;
using System.Text;

using AdvancedStringBuilder;

namespace WebMarkupMin.Core.Utilities
{
	public static class SourceCodeNavigator
	{
		private const byte DEFAULT_TAB_SIZE = 4;
		private const int DEFAULT_MAX_FRAGMENT_LENGTH = 95;

		/// <summary>
		/// Array of characters used to find the newline
		/// </summary>
		private static readonly char[] _newLineChars = EnvironmentShortcuts.NewLineChars;


		/// <summary>
		/// Finds a next newline
		/// </summary>
		/// <param name="sourceCode">Source code</param>
		/// <param name="startPosition">Position in the input string that defines the leftmost
		/// position to be searched</param>
		/// <param name="newLinePosition">Position of the newline string</param>
		/// <param name="newLineLength">Length of the newline string</param>
		private static void FindNextNewLine(string sourceCode, int startPosition,
			out int newLinePosition, out int newLineLength)
		{
			int length = sourceCode.Length - startPosition;

			FindNextNewLine(sourceCode, startPosition, length,
				out newLinePosition, out newLineLength);
		}

		/// <summary>
		/// Finds a next newline
		/// </summary>
		/// <param name="sourceCode">Source code</param>
		/// <param name="startPosition">Position in the input string that defines the leftmost
		/// position to be searched</param>
		/// <param name="length">Number of characters in the substring to include in the search</param>
		/// <param name="newLinePosition">Position of the newline string</param>
		/// <param name="newLineLength">Length of the newline string</param>
		private static void FindNextNewLine(string sourceCode, int startPosition, int length,
			out int newLinePosition, out int newLineLength)
		{
			newLinePosition = sourceCode.IndexOfAny(_newLineChars, startPosition, length);
			if (newLinePosition != -1)
			{
				newLineLength = 1;
				char currentCharacter = sourceCode[newLinePosition];

				int nextCharacterPosition = newLinePosition + 1;
				char nextCharacter;

				if (sourceCode.TryGetChar(nextCharacterPosition, out nextCharacter)
					&& nextCharacter.IsNewLine() && nextCharacter != currentCharacter)
				{
					newLineLength = 2;
				}
			}
			else
			{
				newLineLength = 0;
			}
		}

		/// <summary>
		/// Finds a previous newline
		/// </summary>
		/// <param name="sourceCode">Source code</param>
		/// <param name="startPosition">Position in the input string that defines the leftmost
		/// position to be searched</param>
		/// <param name="newLinePosition">Position of the newline string</param>
		/// <param name="newLineLength">Length of the newline string</param>
		private static void FindPreviousNewLine(string sourceCode, int startPosition,
			out int newLinePosition, out int newLineLength)
		{
			newLinePosition = sourceCode.LastIndexOfAny(_newLineChars, startPosition);
			if (newLinePosition != -1)
			{
				newLineLength = 1;
				char currentCharacter = sourceCode[newLinePosition];

				int previousCharacterPosition = newLinePosition - 1;
				char previousCharacter;

				if (sourceCode.TryGetChar(previousCharacterPosition, out previousCharacter)
					&& previousCharacter.IsNewLine() && previousCharacter != currentCharacter)
				{
					newLinePosition = previousCharacterPosition;
					newLineLength = 2;
				}
			}
			else
			{
				newLineLength = 0;
			}
		}

		/// <summary>
		/// Finds a next non-whitespace character
		/// </summary>
		/// <param name="sourceCode">Source code</param>
		/// <param name="startPosition">Position in the input string that defines the leftmost
		/// position to be searched</param>
		/// <param name="length">Number of characters in the substring to include in the search</param>
		/// <returns>Position of non-whitespace character</returns>
		internal static int FindNextNonWhitespaceChar(string sourceCode, int startPosition, int length)
		{
			int charPosition;
			int endPosition = startPosition + length - 1;

			for (charPosition = startPosition; charPosition <= endPosition; charPosition++)
			{
				char charValue = sourceCode[charPosition];
				if (!char.IsWhiteSpace(charValue))
				{
					break;
				}
			}

			return charPosition;
		}

		/// <summary>
		/// Calculates a line count
		/// </summary>
		/// <param name="sourceCode">Source code</param>
		/// <param name="lineCount">Number of lines</param>
		/// <param name="charRemainderCount">Number of characters left</param>
		public static void CalculateLineCount(string sourceCode, out int lineCount, out int charRemainderCount)
		{
			CalculateLineCount(sourceCode, 0, out lineCount, out charRemainderCount);
		}

		/// <summary>
		/// Calculates a line count
		/// </summary>
		/// <param name="sourceCode">Source code</param>
		/// <param name="fragmentStartPosition">Start position of fragment</param>
		/// <param name="lineCount">Number of lines</param>
		/// <param name="charRemainderCount">Number of characters left</param>
		public static void CalculateLineCount(string sourceCode, int fragmentStartPosition,
			out int lineCount, out int charRemainderCount)
		{
			int fragmentLength = sourceCode.Length - fragmentStartPosition;

			CalculateLineCount(sourceCode, fragmentStartPosition, fragmentLength,
				out lineCount, out charRemainderCount);
		}

		/// <summary>
		/// Calculates a line count
		/// </summary>
		/// <param name="sourceCode">Source code</param>
		/// <param name="fragmentStartPosition">Start position of fragment</param>
		/// <param name="fragmentLength">Length of fragment</param>
		/// <param name="lineCount">Number of lines</param>
		/// <param name="charRemainderCount">Number of characters left</param>
		public static void CalculateLineCount(string sourceCode, int fragmentStartPosition, int fragmentLength,
			out int lineCount, out int charRemainderCount)
		{
			int sourceCodeLength = sourceCode.Length;
			lineCount = 0;
			charRemainderCount = 0;

			if (string.IsNullOrWhiteSpace(sourceCode))
			{
				return;
			}

			if (fragmentStartPosition < 0)
			{
				throw new ArgumentException("", nameof(fragmentStartPosition));
			}

			if (fragmentLength > sourceCodeLength - fragmentStartPosition)
			{
				throw new ArgumentException("", nameof(fragmentLength));
			}

			int fragmentEndPosition = fragmentStartPosition + fragmentLength - 1;
			int newLinePosition = int.MinValue;
			int newLineLength = 0;
			int startLinePosition;

			do
			{
				startLinePosition = newLinePosition == int.MinValue ?
					fragmentStartPosition : newLinePosition + newLineLength;
				int lineLength = fragmentEndPosition - startLinePosition + 1;

				FindNextNewLine(sourceCode, startLinePosition, lineLength,
					out newLinePosition, out newLineLength);

				if (newLinePosition != -1)
				{
					lineCount++;
				}
			}
			while (newLinePosition != -1 && newLinePosition <= fragmentEndPosition);

			if (lineCount > 0)
			{
				charRemainderCount = fragmentEndPosition - startLinePosition + 1;
			}
			else
			{
				charRemainderCount = fragmentLength;
			}
		}

		/// <summary>
		/// Calculates a node coordinates
		/// </summary>
		/// <param name="sourceCode">Source code</param>
		/// <param name="nodePosition">Current node position</param>
		/// <returns>Node coordinates</returns>
		public static SourceCodeNodeCoordinates CalculateNodeCoordinates(string sourceCode, int nodePosition)
		{
			if (string.IsNullOrEmpty(sourceCode) || nodePosition >= sourceCode.Length)
			{
				return SourceCodeNodeCoordinates.Empty;
			}

			int fragmentLength = nodePosition + 1;
			int lineCount;
			int charRemainderCount;

			CalculateLineCount(sourceCode, 0, fragmentLength,
				out lineCount, out charRemainderCount);

			var nodeCoordinates = new SourceCodeNodeCoordinates(lineCount + 1, charRemainderCount + 1);

			return nodeCoordinates;
		}

		/// <summary>
		/// Gets a source fragment
		/// </summary>
		/// <param name="sourceCode">Source code</param>
		/// <param name="nodePosition">Current node position</param>
		/// <param name="tabSize">Number of spaces in the tab</param>
		/// <param name="maxFragmentLength">Maximum length of the source fragment</param>
		/// <returns>Source fragment</returns>
		public static string GetSourceFragment(string sourceCode, int nodePosition,
			byte tabSize = DEFAULT_TAB_SIZE, int maxFragmentLength = DEFAULT_MAX_FRAGMENT_LENGTH)
		{
			SourceCodeNodeCoordinates nodeCoordinates = CalculateNodeCoordinates(sourceCode, nodePosition);

			return GetSourceFragment(sourceCode, nodeCoordinates, tabSize);
		}

		/// <summary>
		/// Gets a source fragment
		/// </summary>
		/// <param name="sourceCode">Source code</param>
		/// <param name="nodeCoordinates">Node coordinates</param>
		/// <param name="tabSize">Number of spaces in the tab</param>
		/// <param name="maxFragmentLength">Maximum length of the source fragment</param>
		/// <returns>Source fragment</returns>
		public static string GetSourceFragment(string sourceCode,
			SourceCodeNodeCoordinates nodeCoordinates, byte tabSize = DEFAULT_TAB_SIZE,
			int maxFragmentLength = DEFAULT_MAX_FRAGMENT_LENGTH)
		{
			string sourceFragment = string.Empty;
			int lineNumber = nodeCoordinates.LineNumber;
			int columnNumber = nodeCoordinates.ColumnNumber;

			if (!string.IsNullOrEmpty(sourceCode))
			{
				int previousLineNumber = lineNumber - 1;
				int currentLineNumber = lineNumber;
				int nextLineNumber = lineNumber + 1;

				string previousLine = string.Empty;
				string currentLine = string.Empty;
				string nextLine = string.Empty;

				int lineCount = 0;
				int newLinePosition = int.MinValue;
				int newLineLength = 0;

				do
				{
					string line;
					int startLinePosition = newLinePosition == int.MinValue ? 0 : newLinePosition + newLineLength;

					FindNextNewLine(sourceCode, startLinePosition, out newLinePosition, out newLineLength);

					if (newLinePosition != -1)
					{
						line = sourceCode.Substring(startLinePosition, newLinePosition - startLinePosition);
					}
					else
					{
						line = sourceCode.Substring(startLinePosition);
					}

					lineCount++;

					if (lineCount == previousLineNumber)
					{
						previousLine = line;
					}
					else if (lineCount == currentLineNumber)
					{
						currentLine = line;
					}
					else if (lineCount == nextLineNumber)
					{
						nextLine = line;
					}
				}
				while (newLinePosition != -1 && lineCount <= nextLineNumber);

				int lineNumberSize = nextLineNumber.ToString(CultureInfo.InvariantCulture).Length;
				if (currentLineNumber == lineCount)
				{
					lineNumberSize = currentLineNumber.ToString(CultureInfo.InvariantCulture).Length;
				}

				int fragmentStartPosition;
				int fragmentLength;

				CalculateCutPositions(currentLine, columnNumber, maxFragmentLength,
					out fragmentStartPosition, out fragmentLength);

				var stringBuilderPool = StringBuilderPool.Shared;
				StringBuilder sourceFragmentBuilder = stringBuilderPool.Rent();

				if (currentLine.Length > 0)
				{
					if (previousLine.Length > 0)
					{
						sourceFragmentBuilder.AppendLine(FormatSourceCodeLine(previousLine,
							new SourceCodeNodeCoordinates(previousLineNumber, 0),
							lineNumberSize, fragmentStartPosition, fragmentLength, tabSize));
					}

					sourceFragmentBuilder.AppendLine(FormatSourceCodeLine(currentLine,
						new SourceCodeNodeCoordinates(currentLineNumber, columnNumber),
						lineNumberSize, fragmentStartPosition, fragmentLength, tabSize));

					if (nextLine.Length > 0)
					{
						sourceFragmentBuilder.AppendLine(FormatSourceCodeLine(nextLine,
							new SourceCodeNodeCoordinates(nextLineNumber, 0),
							lineNumberSize, fragmentStartPosition, fragmentLength, tabSize));
					}
				}

				sourceFragment = sourceFragmentBuilder.ToString();
				stringBuilderPool.Return(sourceFragmentBuilder);
			}

			return sourceFragment;
		}

		/// <summary>
		/// Calculates a cut positions
		/// </summary>
		/// <param name="line">Line content</param>
		/// <param name="columnNumber">Column number</param>
		/// <param name="maxFragmentLength">Maximum length of the source fragment</param>
		/// <param name="fragmentStartPosition">Start position of source fragment</param>
		/// <param name="fragmentLength">Length of source fragment</param>
		private static void CalculateCutPositions(string line, int columnNumber, int maxFragmentLength,
			out int fragmentStartPosition, out int fragmentLength)
		{
			int lineLength = line.Length;
			var leftOffset = (int)Math.Floor((double)maxFragmentLength / 2);

			fragmentStartPosition = 0;
			fragmentLength = maxFragmentLength;

			if (lineLength > maxFragmentLength)
			{
				fragmentStartPosition = columnNumber - leftOffset - 1;
				if (fragmentStartPosition < 0)
				{
					fragmentStartPosition = 0;
				}
				fragmentLength = maxFragmentLength - 2;
			}
		}

		/// <summary>
		/// Formats a line of source code
		/// </summary>
		/// <param name="line">Line content</param>
		/// <param name="nodeCoordinates">Node coordinates</param>
		/// <param name="lineNumberSize">Number of symbols in the line number caption</param>
		/// <param name="fragmentStartPosition">Start position of source fragment</param>
		/// <param name="fragmentLength">Length of source fragment</param>
		/// <param name="tabSize">Number of spaces in the tab</param>
		/// <returns>Formatted line</returns>
		private static string FormatSourceCodeLine(string line, SourceCodeNodeCoordinates nodeCoordinates,
			int lineNumberSize, int fragmentStartPosition = 0, int fragmentLength = 0, byte tabSize = 4)
		{
			const string ellipsisSymbol = "…";
			const byte leftPaddingSize = 7;

			int lineNumber = nodeCoordinates.LineNumber;
			int columnNumber = nodeCoordinates.ColumnNumber;
			int lineLength = line.Length;

			string processedLine;
			if (fragmentStartPosition == 0 && fragmentLength == lineLength)
			{
				processedLine = line;
			}
			else if (fragmentStartPosition >= lineLength)
			{
				processedLine = string.Empty;
			}
			else
			{
				int fragmentEndPosition = fragmentStartPosition + fragmentLength - 1;

				bool beginningCutOff = fragmentStartPosition > 0;
				bool endingCutOff = fragmentEndPosition <= lineLength;
				if (fragmentEndPosition + 1 == lineLength)
				{
					endingCutOff = false;
				}

				if (fragmentEndPosition >= lineLength)
				{
					endingCutOff = false;
					fragmentEndPosition = lineLength - 1;
					fragmentLength = fragmentEndPosition - fragmentStartPosition + 1;
				}

				processedLine = line.Substring(fragmentStartPosition, fragmentLength);
				if (beginningCutOff)
				{
					processedLine = ellipsisSymbol + processedLine;
				}
				if (endingCutOff)
				{
					processedLine = processedLine + ellipsisSymbol;
				}
			}

			string result = string.Format("Line {0}: {1}",
				lineNumber.ToString(CultureInfo.InvariantCulture).PadLeft(lineNumberSize),
				processedLine.TabsToSpaces(tabSize));
			if (columnNumber > 0)
			{
				int cursorOffset = columnNumber - fragmentStartPosition;
				if (fragmentStartPosition > 0)
				{
					cursorOffset++;
				}

				result += Environment.NewLine + string.Empty
					.PadRight(
						(cursorOffset < processedLine.Length ?
							processedLine.Substring(0, cursorOffset - 1) : processedLine
						).TabsToSpaces(tabSize).Length + lineNumberSize + leftPaddingSize
					)
					.Replace(" ", "-") + "^"
					;
			}

			return result;
		}

		/// <summary>
		/// Calculates a absolute node coordinates
		/// </summary>
		/// <param name="baseNodeCoordinates">Base node coordinates</param>
		/// <param name="relativeNodeCoordinates">Relative node coordinates</param>
		/// <returns>Absolute node coordinates</returns>
		public static SourceCodeNodeCoordinates CalculateAbsoluteNodeCoordinates(
			SourceCodeNodeCoordinates baseNodeCoordinates, SourceCodeNodeCoordinates relativeNodeCoordinates)
		{
			int relativeLineNumber = relativeNodeCoordinates.LineNumber;
			int relativeColumnNumber = relativeNodeCoordinates.ColumnNumber;

			int absoluteLineNumber;
			int absoluteColumnNumber;

			if (!baseNodeCoordinates.IsEmpty)
			{
				int baseLineNumber = baseNodeCoordinates.LineNumber;
				int baseColumnNumber = baseNodeCoordinates.ColumnNumber;

				absoluteLineNumber = baseLineNumber;
				absoluteColumnNumber = baseColumnNumber;

				if (relativeLineNumber > 0)
				{
					if (relativeLineNumber == 1)
					{
						if (relativeColumnNumber > 0)
						{
							absoluteColumnNumber = baseColumnNumber + relativeColumnNumber - 1;
						}
					}
					else
					{
						absoluteLineNumber = baseLineNumber + relativeLineNumber - 1;
						absoluteColumnNumber = relativeColumnNumber;
					}
				}
			}
			else
			{
				absoluteLineNumber = relativeLineNumber;
				absoluteColumnNumber = relativeColumnNumber;
			}

			var absoluteNodeCoordinates = new SourceCodeNodeCoordinates(absoluteLineNumber, absoluteColumnNumber);

			return absoluteNodeCoordinates;
		}

		/// <summary>
		/// Calculates a absolute node coordinates
		/// </summary>
		/// <param name="baseNodeCoordinates">Base node coordinates</param>
		/// <param name="additionalContent">Additional content</param>
		/// <returns>Absolute node coordinates</returns>
		public static SourceCodeNodeCoordinates CalculateAbsoluteNodeCoordinates(
			SourceCodeNodeCoordinates baseNodeCoordinates, string additionalContent)
		{
			int lineCount = 0;
			int charRemainderCount = 0;

			if (!string.IsNullOrEmpty(additionalContent))
			{
				CalculateLineCount(additionalContent, out lineCount, out charRemainderCount);
			}

			int absoluteLineNumber;
			int absoluteColumnNumber;

			if (!baseNodeCoordinates.IsEmpty)
			{
				int baseLineNumber = baseNodeCoordinates.LineNumber;
				int baseColumnNumber = baseNodeCoordinates.ColumnNumber;

				if (lineCount > 0)
				{
					absoluteLineNumber = baseLineNumber + lineCount;
					absoluteColumnNumber = charRemainderCount + 1;
				}
				else
				{
					absoluteLineNumber = baseLineNumber;
					absoluteColumnNumber = baseColumnNumber + charRemainderCount;
				}
			}
			else
			{
				absoluteLineNumber = lineCount + 1;
				absoluteColumnNumber = charRemainderCount + 1;
			}

			var absoluteNodeCoordinates = new SourceCodeNodeCoordinates(absoluteLineNumber, absoluteColumnNumber);

			return absoluteNodeCoordinates;
		}

		/// <summary>
		/// Calculates a absolute node coordinates
		/// </summary>
		/// <param name="baseNodeCoordinates">Base node coordinates</param>
		/// <param name="lineCount">Number of lines</param>
		/// <param name="charRemainderCount">Number of characters left</param>
		/// <returns>Absolute node coordinates</returns>
		public static SourceCodeNodeCoordinates CalculateAbsoluteNodeCoordinates(
			SourceCodeNodeCoordinates baseNodeCoordinates, int lineCount, int charRemainderCount)
		{
			int absoluteLineNumber;
			int absoluteColumnNumber;

			if (!baseNodeCoordinates.IsEmpty)
			{
				int baseLineNumber = baseNodeCoordinates.LineNumber;
				int baseColumnNumber = baseNodeCoordinates.ColumnNumber;

				if (lineCount > 0)
				{
					absoluteLineNumber = baseLineNumber + lineCount;
					absoluteColumnNumber = charRemainderCount + 1;
				}
				else
				{
					absoluteLineNumber = baseLineNumber;
					absoluteColumnNumber = baseColumnNumber + charRemainderCount;
				}
			}
			else
			{
				absoluteLineNumber = lineCount + 1;
				absoluteColumnNumber = charRemainderCount + 1;
			}

			var absoluteNodeCoordinates = new SourceCodeNodeCoordinates(absoluteLineNumber, absoluteColumnNumber);

			return absoluteNodeCoordinates;
		}

		/// <summary>
		/// Gets a current line content
		/// </summary>
		/// <param name="sourceCode">Source code</param>
		/// <param name="currentPosition">Current position</param>
		/// <param name="startLinePosition">Start position of line</param>
		/// <param name="endLinePosition">End position of line</param>
		/// <returns>Line content</returns>
		public static string GetCurrentLine(string sourceCode, int currentPosition,
			out int startLinePosition, out int endLinePosition)
		{
			startLinePosition = -1;
			endLinePosition = -1;

			if (string.IsNullOrEmpty(sourceCode))
			{
				return string.Empty;
			}

			int sourceCodeLength = sourceCode.Length;
			if (currentPosition >= sourceCodeLength)
			{
				throw new ArgumentException("", nameof(currentPosition));
			}

			char currentChar = sourceCode[currentPosition];
			if (currentChar.IsNewLine())
			{
				return string.Empty;
			}

			int startNewLinePosition;
			int startNewLineLength;

			FindPreviousNewLine(sourceCode, currentPosition, out startNewLinePosition, out startNewLineLength);
			if (startNewLinePosition != -1)
			{
				startLinePosition = startNewLinePosition + startNewLineLength;
			}
			else
			{
				startLinePosition = 0;
			}

			int endNewLinePosition;
			int endNewLineLength;

			FindNextNewLine(sourceCode, currentPosition, out endNewLinePosition, out endNewLineLength);
			if (endNewLinePosition != -1)
			{
				endLinePosition = endNewLinePosition - 1;
			}
			else
			{
				endLinePosition = sourceCodeLength - 1;
			}

			int lineLength = endLinePosition - startLinePosition + 1;
			string lineContent = sourceCode.Substring(startLinePosition, lineLength);

			return lineContent;
		}

		#region Obsolete methods

		/// <summary>
		/// Calculates a line break count
		/// </summary>
		/// <param name="sourceCode">Source code</param>
		/// <param name="lineBreakCount">Number of line breaks</param>
		/// <param name="charRemainderCount">Number of characters left</param>
		[Obsolete("Use a `CalculateLineCount` method")]
		public static void CalculateLineBreakCount(string sourceCode, out int lineBreakCount, out int charRemainderCount)
		{
			CalculateLineCount(sourceCode, out lineBreakCount, out charRemainderCount);
		}

		/// <summary>
		/// Calculates a line break count
		/// </summary>
		/// <param name="sourceCode">Source code</param>
		/// <param name="fragmentStartPosition">Start position of fragment</param>
		/// <param name="lineBreakCount">Number of line breaks</param>
		/// <param name="charRemainderCount">Number of characters left</param>
		[Obsolete("Use a `CalculateLineCount` method")]
		public static void CalculateLineBreakCount(string sourceCode, int fragmentStartPosition,
			out int lineBreakCount, out int charRemainderCount)
		{
			CalculateLineCount(sourceCode, fragmentStartPosition, out lineBreakCount, out charRemainderCount);
		}

		/// <summary>
		/// Calculates a line break count
		/// </summary>
		/// <param name="sourceCode">Source code</param>
		/// <param name="fragmentStartPosition">Start position of fragment</param>
		/// <param name="fragmentLength">Length of fragment</param>
		/// <param name="lineBreakCount">Number of line breaks</param>
		/// <param name="charRemainderCount">Number of characters left</param>
		[Obsolete("Use a `CalculateLineCount` method")]
		public static void CalculateLineBreakCount(string sourceCode, int fragmentStartPosition, int fragmentLength,
			out int lineBreakCount, out int charRemainderCount)
		{
			CalculateLineCount(sourceCode, fragmentStartPosition, fragmentLength, out lineBreakCount, out charRemainderCount);
		}

		#endregion
	}
}