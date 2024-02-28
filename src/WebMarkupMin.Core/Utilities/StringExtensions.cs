using System;
using System.Text;

using AdvancedStringBuilder;

namespace WebMarkupMin.Core.Utilities
{
	/// <summary>
	/// Extensions for String
	/// </summary>
	internal static class StringExtensions
	{
		/// <summary>
		/// Array of the newline characters
		/// </summary>
		private static readonly char[] _newLineChars = EnvironmentShortcuts.NewLineChars;

		/// <summary>
		/// Array of other whitespace characters
		/// </summary>
		private static readonly char[] _otherWhitespaceChars = { '\t', '\r', '\n', '\v', '\f' };


		/// <summary>
		/// Replaces tabs by specified number of spaces
		/// </summary>
		/// <param name="source">String value</param>
		/// <param name="tabSize">Number of spaces in tab</param>
		/// <returns>Processed string value</returns>
		public static string TabsToSpaces(this string source, int tabSize)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			string result = source.Replace("\t", "".PadRight(tabSize));

			return result;
		}

		/// <summary>
		/// Determines whether the beginning of this string instance matches the specified string
		/// when compared using the specified comparison option
		/// </summary>
		/// <param name="source">The source string</param>
		/// <param name="value">The string to compare</param>
		/// <param name="startIndex">The search starting position</param>
		/// <param name="comparisonType">One of the enumeration values that determines how
		/// this string and value are compared</param>
		/// <returns><c>true</c> if the value parameter matches the beginning of this string; otherwise, <c>false</c></returns>
		public static bool CustomStartsWith(this string source, string value, int startIndex,
			StringComparison comparisonType)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			int count = value.Length;
			int sourceLength = source.Length;

			if (sourceLength - startIndex < count)
			{
				return false;
			}

			return source.IndexOf(value, startIndex, count, comparisonType) == startIndex;
		}

		/// <summary>
		/// Determines a equality of two strings with ignoring case
		/// </summary>
		/// <param name="source">The source string</param>
		/// <param name="value">The string to compare</param>
		/// <returns><c>true</c> if the value of the value parameter is the same as this string; otherwise, <c>false</c></returns>
		public static bool IgnoreCaseEquals(this string source, string value)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			return source.Equals(value, StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Gets a character at the specified index from the string.
		/// A return value indicates whether the receiving succeeded.
		/// </summary>
		/// <param name="source">The source string</param>
		/// <param name="index">The zero-based index of the character</param>
		/// <param name="result">When this method returns, contains the character from the string,
		/// if the receiving succeeded, or null character if the receiving failed.
		/// The receiving fails if the index out of bounds.</param>
		/// <returns><c>true</c> if the character was received successfully; otherwise, <c>false</c></returns>
		public static bool TryGetChar(this string source, int index, out char result)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			bool isSuccess;
			int length = source.Length;

			if (length > 0 && index >= 0 && index < length)
			{
				result = source[index];
				isSuccess = true;
			}
			else
			{
				result = '\0';
				isSuccess = false;
			}

			return isSuccess;
		}

		/// <summary>
		/// Gets a newline string from a string value
		/// </summary>
		/// <param name="source">String value</param>
		/// <returns>Newline string</returns>
		public static string GetNewLine(this string source)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (source.Length == 0)
			{
				return null;
			}

			string newLine = null;
			int newLineIndex = source.IndexOfAny(_newLineChars);

			if (newLineIndex != -1)
			{
				newLine = InternalGetNewLineByIndex(source, newLineIndex, false);
			}

			return newLine;
		}

		/// <summary>
		/// Gets a newline string from a string value
		/// </summary>
		/// <param name="source">String value</param>
		/// <param name="startIndex">The search starting position</param>
		/// <returns>Newline string</returns>
		public static string GetNewLine(this string source, int startIndex)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			int length = source.Length;
			if (length == 0)
			{
				return null;
			}

			if (startIndex < 0 || startIndex >= length)
			{
				throw new ArgumentOutOfRangeException(nameof(startIndex));
			}

			string newLine = null;
			int newLineIndex = source.IndexOfAny(_newLineChars, startIndex);

			if (newLineIndex != -1)
			{
				newLine = InternalGetNewLineByIndex(source, newLineIndex, false);
			}

			return newLine;
		}

		/// <summary>
		/// Gets a newline string from a string value
		/// </summary>
		/// <param name="source">String value</param>
		/// <param name="startIndex">The search starting position</param>
		/// <param name="count">The number of character positions to examine</param>
		/// <returns>Newline string</returns>
		public static string GetNewLine(this string source, int startIndex, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			int length = source.Length;
			if (length == 0)
			{
				return null;
			}

			if (startIndex < 0 || startIndex >= length)
			{
				throw new ArgumentOutOfRangeException(nameof(startIndex));
			}

			if (count < 1 || count > length - startIndex)
			{
				throw new ArgumentOutOfRangeException(nameof(count));
			}

			string newLine = null;
			int newLineIndex = source.IndexOfAny(_newLineChars, startIndex, count);

			if (newLineIndex != -1)
			{
				newLine = InternalGetNewLineByIndex(source, newLineIndex, false);
			}

			return newLine;
		}

		private static string InternalGetNewLineByIndex(string value, int index, bool checkFirstChar)
		{
			char firstChar = value[index];
			if (checkFirstChar && !firstChar.IsNewLine())
			{
				return null;
			}

			string newLine = null;
			char secondChar = '\0';

			if (value.TryGetChar(index + 1, out secondChar) && secondChar.IsNewLine() && firstChar != secondChar)
			{
				newLine = firstChar == '\n' ? "\n\r" : "\r\n";
			}
			else
			{
				newLine = firstChar == '\n' ? "\n" : "\r";
			}

			return newLine;
		}

		/// <summary>
		/// Determines whether the beginning of this string instance matches the newline character
		/// </summary>
		/// <param name="source">String value</param>
		/// <returns><c>true</c> if the newline character matches the beginning of this string; otherwise, <c>false</c></returns>
		public static bool StartsWithNewLine(this string source)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (source.Length == 0)
			{
				return false;
			}

			bool result = source[0].IsNewLine();

			return result;
		}

		/// <summary>
		/// Determines whether the end of this string instance matches the newline character
		/// </summary>
		/// <param name="source">String value</param>
		/// <returns><c>true</c> if the newline character matches the end of this instance; otherwise, <c>false</c></returns>
		public static bool EndsWithNewLine(this string source)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			int length = source.Length;
			if (length == 0)
			{
				return false;
			}

			bool result = source[length - 1].IsNewLine();

			return result;
		}

		/// <summary>
		/// Removes all leading whitespace characters from the current <see cref="T:System.String" /> object
		/// </summary>
		/// <param name="source">String value</param>
		/// <param name="preserveNewLines">Flag for whether to collapse leading whitespace to one newline string
		/// when whitespace contains a newline</param>
		/// <returns>The string that remains after all whitespace characters are removed from the start of
		/// the current string. If no characters can be trimmed from the current instance, the method returns
		/// the current instance unchanged.</returns>
		public static string TrimStart(this string source, bool preserveNewLines)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (source.Length == 0)
			{
				return source;
			}

			if (preserveNewLines && source.IndexOfAny(_newLineChars) != -1)
			{
				return InternalTrimStartWithNewLinesPreserved(source);
			}
			else
			{
				return source.TrimStart(null);
			}
		}

		private static string InternalTrimStartWithNewLinesPreserved(string value)
		{
			int charIndex = 0;
			int charCount = value.Length;
			string newLine = null;

			for (charIndex = 0; charIndex < charCount && char.IsWhiteSpace(value[charIndex]); charIndex++)
			{
				if (newLine == null)
				{
					newLine = InternalGetNewLineByIndex(value, charIndex, true);
				}
			}

			if (charIndex == 0)
			{
				return value;
			}

			if (newLine != null)
			{
				if (charIndex == newLine.Length)
				{
					return value;
				}

				var stringBuilderPool = StringBuilderPool.Shared;
				StringBuilder sb = stringBuilderPool.Rent();
				sb.Append(newLine);
				sb.Append(value, charIndex, charCount - charIndex);

				string result = sb.ToString();
				stringBuilderPool.Return(sb);

				return result;
			}
			else
			{
				return value.TrimStart(null);
			}
		}

		/// <summary>
		/// Removes all trailing whitespace characters from the current <see cref="T:System.String" /> object
		/// </summary>
		/// <param name="source">String value</param>
		/// <param name="preserveNewLines">Flag for whether to collapse trailing whitespace to one newline string
		/// when whitespace contains a newline</param>
		/// <returns>The string that remains after all whitespace characters are removed from the end of
		/// the current string. If no characters can be trimmed from the current instance, the method returns
		/// the current instance unchanged.</returns>
		public static string TrimEnd(this string source, bool preserveNewLines)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (source.Length == 0)
			{
				return source;
			}

			if (preserveNewLines && source.IndexOfAny(_newLineChars) != -1)
			{
				return InternalTrimEndWithNewLinesPreserved(source);
			}
			else
			{
				return source.TrimEnd(null);
			}
		}

		private static string InternalTrimEndWithNewLinesPreserved(string value)
		{
			int charCount = value.Length;
			int lastCharIndex = charCount - 1;
			int charIndex = lastCharIndex;

			while (charIndex >= 0 && char.IsWhiteSpace(value[charIndex]))
			{
				charIndex--;
			}

			if (charIndex == lastCharIndex)
			{
				return value;
			}

			int whitespaceCharIndex = charIndex + 1;
			string newLine = value.GetNewLine(whitespaceCharIndex);

			if (newLine != null)
			{
				if (charCount - whitespaceCharIndex == newLine.Length)
				{
					return value;
				}

				var stringBuilderPool = StringBuilderPool.Shared;
				StringBuilder sb = stringBuilderPool.Rent();
				sb.Append(value, 0, whitespaceCharIndex);
				sb.Append(newLine);

				string result = sb.ToString();
				stringBuilderPool.Return(sb);

				return result;
			}
			else
			{
				return value.TrimEnd(null);
			}
		}

		/// <summary>
		/// Removes all leading and trailing whitespace characters from the current <see cref="T:System.String" /> object
		/// </summary>
		/// <param name="source">String value</param>
		/// <param name="preserveNewLines">Flag for whether to collapse leading and trailing whitespace to one newline
		/// string when whitespace contains a newline</param>
		/// <returns>The string that remains after all whitespace characters are removed from the start and end of
		/// the current string. If no characters can be trimmed from the current instance, the method returns
		/// the current instance unchanged.</returns>
		public static string Trim(this string source, bool preserveNewLines)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (source.Length == 0)
			{
				return source;
			}

			if (preserveNewLines && source.IndexOfAny(_newLineChars) != -1)
			{
				return InternalTrimWithNewLinesPreserved(source);
			}
			else
			{
				return source.Trim(null);
			}
		}

		private static string InternalTrimWithNewLinesPreserved(string value)
		{
			int charCount = value.Length;
			int lastCharIndex = charCount - 1;
			int leftCharIndex;
			int rightCharIndex = lastCharIndex;

			for (leftCharIndex = 0; leftCharIndex < charCount && char.IsWhiteSpace(value[leftCharIndex]); leftCharIndex++)
			{ }

			while (rightCharIndex >= leftCharIndex && char.IsWhiteSpace(value[rightCharIndex]))
			{
				rightCharIndex--;
			}

			int nonWhitespaceCharCount = rightCharIndex - leftCharIndex + 1;

			if (nonWhitespaceCharCount == charCount)
			{
				return value;
			}

			if (nonWhitespaceCharCount == 0)
			{
				return value.GetNewLine() ?? string.Empty;
			}

			bool allowTrimStart = leftCharIndex != 0;
			string leftNewLine = allowTrimStart ? value.GetNewLine(0, leftCharIndex) : null;

			bool allowTrimEnd = rightCharIndex != lastCharIndex;
			string rightNewLine = allowTrimEnd ?
				value.GetNewLine(rightCharIndex + 1, lastCharIndex - rightCharIndex) : null;

			if (leftNewLine != null || rightNewLine != null)
			{
				int leftNewLineLength = leftNewLine != null ? leftNewLine.Length : 0;
				int rightNewLineLength = rightNewLine != null ? rightNewLine.Length : 0;

				if (leftCharIndex == leftNewLineLength && lastCharIndex - rightCharIndex == rightNewLineLength)
				{
					return value;
				}

				var stringBuilderPool = StringBuilderPool.Shared;
				StringBuilder sb = stringBuilderPool.Rent();
				if (leftNewLine != null)
				{
					sb.Append(leftNewLine);
				}
				sb.Append(value, leftCharIndex, nonWhitespaceCharCount);
				if (rightNewLine != null)
				{
					sb.Append(rightNewLine);
				}

				string result = sb.ToString();
				stringBuilderPool.Return(sb);

				return result;
			}
			else
			{
				if (allowTrimStart && allowTrimEnd)
				{
					return value.Trim();
				}
				else if (allowTrimStart)
				{
					return value.TrimStart(null);
				}
				else
				{
					return value.TrimEnd(null);
				}
			}
		}

		/// <summary>
		/// Collapses a whitespace
		/// </summary>
		/// <param name="source">String value</param>
		/// <returns>String value without extra whitespace characters</returns>
		public static string CollapseWhitespace(this string source)
		{
			return CollapseWhitespace(source, false);
		}

		/// <summary>
		/// Collapses a whitespace
		/// </summary>
		/// <param name="source">String value</param>
		/// <param name="preserveNewLines">Flag for whether to collapse whitespace to one newline string
		/// when whitespace contains a newline</param>
		/// <returns>String value without extra whitespace characters</returns>
		public static string CollapseWhitespace(this string source, bool preserveNewLines)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (source.Length == 0)
			{
				return source;
			}

			if (preserveNewLines && source.IndexOfAny(_newLineChars) != -1)
			{
				return InternalCollapseWhitespaceToNewLine(source);
			}
			else
			{
				return InternalCollapseWhitespaceToSpace(source);
			}
		}

		private static string InternalCollapseWhitespaceToSpace(string value)
		{
			if (value.IndexOfAny(_otherWhitespaceChars) == -1 && value.IndexOf("  ", StringComparison.Ordinal) == -1)
			{
				return value;
			}

			var stringBuilderPool = StringBuilderPool.Shared;
			StringBuilder sb = null;
			bool previousWhitespace = false;
			int previousCharIndex = 0;
			int charCount = value.Length;

			for (int charIndex = 0; charIndex < charCount; charIndex++)
			{
				char charValue = value[charIndex];
				bool currentWhitespace = char.IsWhiteSpace(charValue);

				if (currentWhitespace)
				{
					if (previousWhitespace || charValue != ' ')
					{
						if (sb == null)
						{
							sb = stringBuilderPool.Rent();
						}

						if (previousCharIndex < charIndex)
						{
							sb.Append(value, previousCharIndex, charIndex - previousCharIndex);
						}

						if (!previousWhitespace)
						{
							sb.Append(' ');
						}

						previousCharIndex = charIndex + 1;
					}
				}

				previousWhitespace = currentWhitespace;
			}

			if (sb == null)
			{
				return value;
			}

			if (previousCharIndex < charCount)
			{
				sb.Append(value, previousCharIndex, charCount - previousCharIndex);
			}

			string result = sb.ToString();
			stringBuilderPool.Return(sb);

			return result;
		}

		private static string InternalCollapseWhitespaceToNewLine(string value)
		{
			var stringBuilderPool = StringBuilderPool.Shared;
			StringBuilder sb = null;
			int charCount = value.Length;
			int lastCharIndex = charCount - 1;
			int whitespaceCharIndex = -1;
			int whitespaceCharCount = 0;
			string newLine = null;

			for (int charIndex = 0; charIndex < charCount; charIndex++)
			{
				char charValue = value[charIndex];
				bool isWhitespaceChar = char.IsWhiteSpace(charValue);

				if (isWhitespaceChar)
				{
					if (whitespaceCharIndex == -1)
					{
						whitespaceCharIndex = charIndex;
					}

					if (newLine == null)
					{
						newLine = InternalGetNewLineByIndex(value, charIndex, true);
						int newLineLength = newLine != null ? newLine.Length : 0;
						if (newLineLength > 1)
						{
							whitespaceCharCount += newLineLength;
							charIndex += newLineLength - 1;
							continue;
						}
					}

					whitespaceCharCount++;
				}

				if (!isWhitespaceChar || charIndex == lastCharIndex)
				{
					if (whitespaceCharCount == charCount)
					{
						return newLine ?? " ";
					}

					if (sb == null && whitespaceCharCount > 0)
					{
						bool whitespaceAlreadyCollapsed = false;
						if (newLine != null)
						{
							whitespaceAlreadyCollapsed = whitespaceCharCount == newLine.Length;
						}
						else
						{
							whitespaceAlreadyCollapsed = whitespaceCharCount == 1 && value[charIndex - 1] == ' ';
						}

						if (!whitespaceAlreadyCollapsed)
						{
							sb = stringBuilderPool.Rent();
							if (whitespaceCharIndex > 0)
							{
								sb.Append(value, 0, whitespaceCharIndex);
							}
						}
					}

					if (sb != null)
					{
						if (whitespaceCharCount > 0)
						{
							sb.Append(newLine ?? " ");
						}
						if (!isWhitespaceChar)
						{
							sb.Append(charValue);
						}
					}

					newLine = null;
					whitespaceCharIndex = -1;
					whitespaceCharCount = 0;
				}
			}

			if (sb == null)
			{
				return value;
			}

			string result = sb.ToString();
			stringBuilderPool.Return(sb);

			return result;
		}
	}
}