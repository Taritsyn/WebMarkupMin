using System;

namespace WebMarkupMin.Core.Utilities
{
	/// <summary>
	/// Extensions for String
	/// </summary>
	internal static class StringExtensions
	{
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
				throw new ArgumentNullException("source");
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
		/// <returns>true if the value parameter matches the beginning of this string; otherwise, false</returns>
		public static bool CustomStartsWith(this string source, string value, int startIndex,
			StringComparison comparisonType)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
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
		/// <returns>true if the value of the value parameter is the same as this string; otherwise, false</returns>
		public static bool IgnoreCaseEquals(this string source, string value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
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
		/// <returns>true if the character was received successfully; otherwise, false</returns>
		public static bool TryGetChar(this string source, int index, out char result)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
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
	}
}