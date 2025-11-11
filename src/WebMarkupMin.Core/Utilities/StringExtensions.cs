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
			if (source is null)
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
			if (source is null)
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
			if (source is null)
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
			if (source is null)
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
			if (source is null)
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
			if (source is null)
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
			if (source is null)
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
			if (source is null)
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
			if (source is null)
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
		/// Reports the zero-based index position of the first occurrence of non-whitespace character in this string
		/// instance
		/// </summary>
		/// <param name="source">String value</param>
		/// <returns>The zero-based index position of non-whitespace character if that character is found,
		/// or <c>-1</c> if it is not</returns>
		public static int IndexOfNonWhitespace(this string source)
		{
			if (source is null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			int sourceLength = source.Length;
			if (sourceLength == 0)
			{
				return -1;
			}

			return source.IndexOfNonWhitespace(0, sourceLength);
		}

		/// <summary>
		/// Reports the zero-based index position of the first occurrence of non-whitespace character in this string
		/// instance
		/// </summary>
		/// <remarks>
		/// The search starts at a specified character position.
		/// </remarks>
		/// <param name="source">String value</param>
		/// <param name="startIndex">The search starting position</param>
		/// <returns>The zero-based index position of non-whitespace character if that character is found,
		/// or <c>-1</c> if it is not</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="startIndex"/> is less than <c>0</c> or greater than the length of this string instance
		/// </exception>
		public static int IndexOfNonWhitespace(this string source, int startIndex)
		{
			if (source is null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			int sourceLength = source.Length;
			if (sourceLength == 0)
			{
				return -1;
			}

			if (startIndex < 0 || startIndex > sourceLength)
			{
				throw new ArgumentOutOfRangeException(nameof(startIndex));
			}

			return source.IndexOfNonWhitespace(startIndex, sourceLength - startIndex);
		}

		/// <summary>
		/// Reports the zero-based index position of the first occurrence of non-whitespace character in this string
		/// instance
		/// </summary>
		/// <remarks>
		/// The search starts at a specified character position and examines a specified number of character positions.
		/// </remarks>
		/// <param name="source">String value</param>
		/// <param name="startIndex">The search starting position</param>
		/// <param name="count">The number of character positions to examine</param>
		/// <returns>The zero-based index position of non-whitespace character if that non-whitespace character is found,
		/// or <c>-1</c> if it is not</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="startIndex"/> or <paramref name="count"/> is negative
		/// -or-
		/// <paramref name="startIndex"/> is greater than the length of this string instance
		/// -or-
		/// <paramref name="count"/> is greater than the length of this string instance minus <paramref name="startIndex"/>
		/// </exception>
		public static int IndexOfNonWhitespace(this string source, int startIndex, int count)
		{
			if (source is null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			int sourceLength = source.Length;
			if (sourceLength == 0)
			{
				return -1;
			}

			if (startIndex < 0 || startIndex > sourceLength)
			{
				throw new ArgumentOutOfRangeException(nameof(startIndex));
			}

			if (count < 0 || count > (sourceLength - startIndex))
			{
				throw new ArgumentOutOfRangeException(nameof(count));
			}

			return InternalIndexOfNonWhitespace(source, startIndex, count);
		}

		private static int InternalIndexOfNonWhitespace(string value, int startIndex, int count)
		{
			int result = -1;
			int endIndex = startIndex + count - 1;

			for (int charIndex = startIndex; charIndex <= endIndex; charIndex++)
			{
				char charValue = value[charIndex];
				if (!char.IsWhiteSpace(charValue))
				{
					result = charIndex;
					break;
				}
			}

			return result;
		}

		/// <summary>
		/// Reports the zero-based index position of the last occurrence of non-whitespace character in this string
		/// instance
		/// </summary>
		/// <param name="source">String value</param>
		/// <returns>The zero-based index position of non-whitespace character if that character is found,
		/// or <c>-1</c> if it is not</returns>
		public static int LastIndexOfNonWhitespace(this string source)
		{
			if (source is null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			int sourceLength = source.Length;
			if (sourceLength == 0)
			{
				return -1;
			}

			return InternalLastIndexOfNonWhitespace(source, sourceLength - 1, sourceLength);
		}

		/// <summary>
		/// Reports the zero-based index position of the last occurrence of non-whitespace character in this string
		/// instance
		/// </summary>
		/// <remarks>
		/// The search starts at a specified character position and proceeds backward toward the beginning of the
		/// string.
		/// </remarks>
		/// <param name="source">String value</param>
		/// <param name="startIndex">The search starting position</param>
		/// <returns>The zero-based index position of non-whitespace character if that character is found,
		/// or <c>-1</c> if it is not found.</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="startIndex"/> is less than <c>0</c> or greater than or equal to the length of this string
		/// instance
		/// </exception>
		public static int LastIndexOfNonWhitespace(this string source, int startIndex)
		{
			if (source is null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			int sourceLength = source.Length;
			if (sourceLength == 0)
			{
				return -1;
			}

			if (startIndex < 0 || startIndex >= sourceLength)
			{
				throw new ArgumentOutOfRangeException(nameof(startIndex));
			}

			return InternalLastIndexOfNonWhitespace(source, startIndex, startIndex + 1);
		}

		/// <summary>
		/// Reports the zero-based index position of the last occurrence of non-whitespace character in this string
		/// instance
		/// </summary>
		/// <remarks>
		/// The search starts at a specified character position and proceeds backward toward the beginning of the
		/// string for a specified number of character positions.
		/// </remarks>
		/// <param name="source">String value</param>
		/// <param name="startIndex">The search starting position</param>
		/// <param name="count">The number of character positions to examine</param>
		/// <returns>The zero-based index position of non-whitespace character if that character is found,
		/// or <c>-1</c> if it is not found</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="startIndex"/> or <paramref name="count"/> is negative
		/// -or-
		/// <paramref name="startIndex"/> is greater than or equal to the length of this string instance
		/// -or-
		/// <paramref name="startIndex" /> minus <paramref name="count" /> plus <c>1</c> is less than <c>0</c>
		/// </exception>
		public static int LastIndexOfNonWhitespace(this string source, int startIndex, int count)
		{
			if (source is null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			int sourceLength = source.Length;
			if (sourceLength == 0)
			{
				return -1;
			}

			if (startIndex < 0 || startIndex >= sourceLength)
			{
				throw new ArgumentOutOfRangeException(nameof(startIndex));
			}

			if (count < 0 || (startIndex - count + 1) < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(count));
			}

			return InternalLastIndexOfNonWhitespace(source, startIndex, count);
		}

		private static int InternalLastIndexOfNonWhitespace(string value, int startIndex, int count)
		{
			int charIndex = startIndex;
			int endIndex = startIndex - count + 1;
			char charValue = value[charIndex];

			while (charIndex >= endIndex && char.IsWhiteSpace(charValue))
			{
				charIndex--;
				charValue = value[charIndex];
			}

			return charIndex;
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
			if (source is null)
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
				if (newLine is null)
				{
					newLine = InternalGetNewLineByIndex(value, charIndex, true);
				}
			}

			if (charIndex == 0)
			{
				return value;
			}

			if (newLine is not null)
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
			if (source is null)
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

			if (newLine is not null)
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
			if (source is null)
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

			if (leftNewLine is not null || rightNewLine is not null)
			{
				int leftNewLineLength = leftNewLine is not null ? leftNewLine.Length : 0;
				int rightNewLineLength = rightNewLine is not null ? rightNewLine.Length : 0;

				if (leftCharIndex == leftNewLineLength && lastCharIndex - rightCharIndex == rightNewLineLength)
				{
					return value;
				}

				var stringBuilderPool = StringBuilderPool.Shared;
				StringBuilder sb = stringBuilderPool.Rent();
				if (leftNewLine is not null)
				{
					sb.Append(leftNewLine);
				}
				sb.Append(value, leftCharIndex, nonWhitespaceCharCount);
				if (rightNewLine is not null)
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
			if (source is null)
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
						if (sb is null)
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

			if (sb is null)
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

					if (newLine is null)
					{
						newLine = InternalGetNewLineByIndex(value, charIndex, true);
						int newLineLength = newLine is not null ? newLine.Length : 0;
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

					if (sb is null && whitespaceCharCount > 0)
					{
						bool whitespaceAlreadyCollapsed = false;
						if (newLine is not null)
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

					if (sb is not null)
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

			if (sb is null)
			{
				return value;
			}

			string result = sb.ToString();
			stringBuilderPool.Return(sb);

			return result;
		}
	}
}