using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using WebMarkupMin.Core.Resources;

namespace WebMarkupMin.Core.Utilities
{
	public static class Utils
	{
		/// <summary>
		/// Array of other whitespace characters
		/// </summary>
		private static readonly char[] _otherWhitespaceChars = { '\t', '\r', '\n', '\v', '\f' };


		/// <summary>
		/// Converts value of source enumeration type to value of destination enumeration type
		/// </summary>
		/// <typeparam name="TSource">Source enumeration type</typeparam>
		/// <typeparam name="TDest">Destination enumeration type</typeparam>
		/// <param name="value">Value of source enumeration type</param>
		/// <returns>Value of destination enumeration type</returns>
		public static TDest GetEnumFromOtherEnum<TSource, TDest>(TSource value)
		{
			string name = value.ToString();
			var destEnumValues = (TDest[])Enum.GetValues(typeof(TDest));

			foreach (var destEnum in destEnumValues)
			{
				if (string.Equals(destEnum.ToString(), name, StringComparison.OrdinalIgnoreCase))
				{
					return destEnum;
				}
			}

			throw new InvalidCastException(
				string.Format(Strings.Common_EnumValueConversionFailed,
					name, typeof(TSource), typeof(TDest))
			);
		}

		/// <summary>
		/// Collapses a whitespace
		/// </summary>
		/// <param name="value">String value</param>
		/// <returns>String value without extra spaces</returns>
		internal static string CollapseWhitespace(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			if (value.Length == 0
				|| (value.IndexOfAny(_otherWhitespaceChars) == -1 && value.IndexOf("  ", StringComparison.Ordinal) == -1))
			{
				return value;
			}

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
							sb = StringBuilderPool.GetBuilder();
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
			StringBuilderPool.ReleaseBuilder(sb);

			return result;
		}

		/// <summary>
		/// Removes a ending semicolons
		/// </summary>
		/// <param name="value">String value</param>
		/// <returns>String value without ending semicolons</returns>
		internal static string RemoveEndingSemicolons(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			int charCount = value.Length;
			if (charCount == 0 || value.LastIndexOf(';') == -1)
			{
				return value;
			}

			// If the string value consists only of a semicolon character, then return an empty string
			if (charCount == 1)
			{
				return string.Empty;
			}

			int resultLength = charCount;
			bool isContainsSemicolon = false;

			for (int charIndex = charCount - 1; charIndex >= 0; charIndex--)
			{
				char charValue = value[charIndex];
				bool currentSemicolon = charValue == ';';
				isContainsSemicolon = isContainsSemicolon || currentSemicolon;

				if (currentSemicolon || char.IsWhiteSpace(charValue))
				{
					resultLength--;
				}
				else
				{
					break;
				}
			}

			string result = isContainsSemicolon ? value.Substring(0, resultLength) : value;

			return result;
		}

		/// <summary>
		/// Removes a prefix and postfix
		/// </summary>
		/// <param name="value">String value</param>
		/// <param name="prefixRegex">Prefix regular expression</param>
		/// <param name="postfixRegex">Postfix regular expression</param>
		/// <returns>String value without prefix and postfix</returns>
		internal static string RemovePrefixAndPostfix(string value, Regex prefixRegex, Regex postfixRegex)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			if (prefixRegex == null)
			{
				throw new ArgumentNullException(nameof(prefixRegex));
			}

			if (postfixRegex == null)
			{
				throw new ArgumentNullException(nameof(postfixRegex));
			}

			if (value.Length == 0)
			{
				return value;
			}

			int startPosition = 0;
			int length = value.Length;

			Match prefixMatch = prefixRegex.Match(value);
			if (prefixMatch.Success)
			{
				startPosition = prefixMatch.Length;
				length -= prefixMatch.Length;
			}
			Match postfixMatch = postfixRegex.Match(value, startPosition, length);

			if (!prefixMatch.Success && !postfixMatch.Success)
			{
				return value;
			}

			if (postfixMatch.Success)
			{
				length -= postfixMatch.Length;
			}

			string result = value.Substring(startPosition, length);

			return result;
		}

		/// <summary>
		/// Removes a BOM from value content
		/// </summary>
		/// <param name="value">String value</param>
		/// <returns>String value without BOM</returns>
		internal static string RemoveByteOrderMark(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			string bomPreamble = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
			if (value.IndexOf(bomPreamble, StringComparison.OrdinalIgnoreCase) == -1)
			{
				return value;
			}

			string result = value.Replace(bomPreamble, string.Empty);

			return result;
		}

		/// <summary>
		/// Determines whether the value contains a upper case letters
		/// </summary>
		/// <param name="value">String value</param>
		/// <returns>true if the value contains an upper case letters; otherwise, false</returns>
		internal static bool ContainsUppercaseCharacters(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			int charCount = value.Length;

			for (int charIndex = 0; charIndex < charCount; charIndex++)
			{
				char charValue = value[charIndex];
				if (char.IsLetter(charValue) && char.IsUpper(charValue))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Produces a set union of many sequences
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of the input sequences</typeparam>
		/// <param name="sequences">Input sequences</param>
		/// <returns>An System.Collections.Generic.HashSet&lt;T&gt; that contains the elements from
		/// input sequences, excluding duplicates</returns>
		internal static HashSet<TSource> UnionHashSets<TSource>(params IEnumerable<TSource>[] sequences)
		{
			var result = new HashSet<TSource>();
			int sequenceCount = sequences.Length;

			for (int sequenceIndex = 0; sequenceIndex < sequenceCount; sequenceIndex++)
			{
				result.UnionWith(sequences[sequenceIndex]);
			}

			return result;
		}
	}
}