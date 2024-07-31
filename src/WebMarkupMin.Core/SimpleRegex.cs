using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using AdvancedStringBuilder;

using WebMarkupMin.Core.Resources;
using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// Represents an simple regular expression
	/// </summary>
	public sealed class SimpleRegex : IEquatable<SimpleRegex>
	{
		/// <summary>
		/// Pattern for working with the string representation of the simple regular expression
		/// </summary>
		const string SIMPLE_REGEX_STRING_PATTERN = @"/(?<pattern>[\s\S]+?)/(?<caseInsensitive>i)?";

		/// <summary>
		/// Regular expression for working with the string representation of the simple regular expression
		/// </summary>
		private static readonly Regex _simpleRegexStringRegex = new Regex(
			@"^" + SIMPLE_REGEX_STRING_PATTERN + @"$",
			TargetFrameworkShortcuts.PerformanceRegexOptions);

		/// <summary>
		/// Regular expression for working with the string representation of the simple regular expression in
		/// comma-separated list
		/// </summary>
		private static readonly Regex _simpleRegexStringInListRegex = new Regex(
			@"(?:^|\s*,\s*)" + SIMPLE_REGEX_STRING_PATTERN + @"(?:\s*,\s*|$)",
			TargetFrameworkShortcuts.PerformanceRegexOptions);

		/// <summary>
		/// Regular expression
		/// </summary>
		private readonly Regex _regex;

		/// <summary>
		/// Regular expression pattern (ECMAScript standard compliant)
		/// </summary>
		private readonly string _pattern;

		/// <summary>
		/// Flag for whether to allow case-insensitive matching
		/// </summary>
		private readonly bool _caseInsensitive;

		/// <summary>
		/// Gets a regular expression pattern (ECMAScript standard compliant)
		/// </summary>
		public string Pattern
		{
			get { return _pattern; }
		}

		/// <summary>
		/// Gets a flag for whether to allow case-insensitive matching
		/// </summary>
		public bool CaseInsensitive
		{
			get { return _caseInsensitive; }
		}


		/// <summary>
		/// Constructs an instance of the simple regular expression
		/// </summary>
		/// <param name="pattern">Regular expression pattern (ECMAScript standard compliant)</param>
		public SimpleRegex(string pattern)
			: this(pattern, false)
		{ }

		/// <summary>
		/// Constructs an instance of the simple regular expression
		/// </summary>
		/// <param name="pattern">Regular expression pattern (ECMAScript standard compliant)</param>
		/// <param name="caseInsensitive">Flag for whether to allow case-insensitive matching</param>
		public SimpleRegex(string pattern, bool caseInsensitive)
		{
			if (pattern == null)
			{
				throw new ArgumentNullException(nameof(pattern));
			}

			if (string.IsNullOrWhiteSpace(pattern))
			{
				throw new ArgumentException(Strings.Common_ValueIsEmpty, nameof(pattern));
			}

			_pattern = pattern;
			_caseInsensitive = caseInsensitive;

			RegexOptions options = RegexOptions.ECMAScript | RegexOptions.CultureInvariant;
			if (caseInsensitive)
			{
				options = options | RegexOptions.IgnoreCase;
			}

			_regex = new Regex(pattern, options);
		}


		/// <summary>
		/// Parses a simple regular expression
		/// </summary>
		/// <param name="regularExpressionString">String representation of the simple regular expression</param>
		/// <returns>Simple regular expression</returns>
		/// <exception cref="ArgumentNullException"><paramref name="regularExpressionString"/> is null</exception>
		/// <exception cref="FormatException"><paramref name="regularExpressionString"/> have an incorrect format</exception>
		public static SimpleRegex Parse(string regularExpressionString)
		{
			if (regularExpressionString == null)
			{
				throw new ArgumentNullException(nameof(regularExpressionString));
			}

			SimpleRegex regularExpression = InternalParse(regularExpressionString);
			if (regularExpression == null)
			{
				throw new FormatException(Strings.ErrorMessage_InvalidSimpleRegularExpression);
			}

			return regularExpression;
		}

		/// <summary>
		/// Parses a simple regular expression.
		/// A return value indicates whether the parsing succeeded.
		/// </summary>
		/// <param name="regularExpressionString">String representation of the simple regular expression</param>
		/// <param name="result">Simple regular expression</param>
		/// <returns><c>true</c> if <paramref name="regularExpressionString"/> was parsed successfully; otherwise, <c>false</c></returns>
		public static bool TryParse(string regularExpressionString, out SimpleRegex result)
		{
			if (regularExpressionString == null)
			{
				result = null;
				return false;
			}

			result = InternalParse(regularExpressionString);

			return result != null;
		}

		private static SimpleRegex InternalParse(string regularExpressionString)
		{
			int position;
			int remainderLength;

			if (!Utils.TryGetNonWhitespaceStringSegment(regularExpressionString, out position, out remainderLength))
			{
				return null;
			}

			SimpleRegex result = null;
			Match regularExpressionMatch = _simpleRegexStringRegex.Match(regularExpressionString, position,
				remainderLength);

			if (regularExpressionMatch.Success)
			{
				result = ParseRegularExpressionMatch(regularExpressionMatch);
			}

			return result;
		}

		/// <summary>
		/// Parses a comma-separated list of string representations of the simple regular expressions
		/// </summary>
		/// <param name="regularExpressionsString">Comma-separated list of string representations of the simple regular expressions</param>
		/// <returns>List of the simple regular expressions</returns>
		/// <exception cref="ArgumentNullException"><paramref name="regularExpressionsString"/> is null</exception>
		/// <exception cref="FormatException"><paramref name="regularExpressionsString"/> have an incorrect format</exception>
		public static List<SimpleRegex> ParseList(string regularExpressionsString)
		{
			if (regularExpressionsString == null)
			{
				throw new ArgumentNullException(nameof(regularExpressionsString));
			}

			List<SimpleRegex> result = InternalParseList(regularExpressionsString);
			if (result == null)
			{
				throw new FormatException(Strings.ErrorMessage_InvalidSimpleRegularExpressionList);
			}

			return result;
		}

		/// <summary>
		/// Parses a comma-separated list of string representations of the simple regular expressions.
		/// A return value indicates whether the parsing succeeded.
		/// </summary>
		/// <param name="regularExpressionsString">Comma-separated list of string representations of the simple regular expressions</param>
		/// <param name="result">List of the simple regular expressions</param>
		/// <returns><c>true</c> if <paramref name="regularExpressionsString"/> was parsed successfully; otherwise, <c>false</c></returns>
		public static bool TryParseList(string regularExpressionsString, out List<SimpleRegex> result)
		{
			if (regularExpressionsString == null)
			{
				result = null;
				return false;
			}

			result = InternalParseList(regularExpressionsString);

			return result != null;
		}

		private static List<SimpleRegex> InternalParseList(string regularExpressionsString)
		{
			int position;
			int remainderLength;

			if (!Utils.TryGetNonWhitespaceStringSegment(regularExpressionsString, out position, out remainderLength))
			{
				return null;
			}

			List<SimpleRegex> result = null;
			Match regularExpressionMatch = _simpleRegexStringInListRegex.Match(regularExpressionsString, position,
				remainderLength);

			while (regularExpressionMatch.Success)
			{
				if (result == null)
				{
					result = new List<SimpleRegex>();
				}

				SimpleRegex regularExpression = ParseRegularExpressionMatch(regularExpressionMatch);
				result.Add(regularExpression);

				int regularExpressionMatchLength = regularExpressionMatch.Length;

				position += regularExpressionMatchLength;
				remainderLength -= regularExpressionMatchLength;
				regularExpressionMatch = _simpleRegexStringInListRegex.Match(regularExpressionsString, position,
					remainderLength);
			}

			return result;
		}

		private static SimpleRegex ParseRegularExpressionMatch(Match expressionMatch)
		{
			GroupCollection groups = expressionMatch.Groups;
			string pattern = UnescapePatternFromLiteral(groups["pattern"].Value);
			bool caseInsensitive = groups["caseInsensitive"].Success;

			var regularExpression = new SimpleRegex(pattern, caseInsensitive);

			return regularExpression;
		}

		private static string EscapePatternForLiteral(string pattern)
		{
			return pattern.Replace("/", "\\/");
		}

		private static string UnescapePatternFromLiteral(string pattern)
		{
			return pattern.Replace("\\/", "/");
		}

		/// <summary>
		/// Indicates whether the simple regular expression finds a match in a specified input string
		/// </summary>
		/// <param name="input">The string to search for a match</param>
		/// <returns><c>true</c> if the simple regular expression finds a match; otherwise, <c>false</c></returns>
		public bool IsMatch(string input)
		{
			if (input == null)
			{
				throw new ArgumentNullException(nameof(input));
			}

			return _regex.IsMatch(input);
		}

		private static bool InternalEquals(SimpleRegex regex1, SimpleRegex regex2)
		{
			if (ReferenceEquals(regex1, regex2))
			{
				return true;
			}

			if (!ReferenceEquals(regex1, null) && !ReferenceEquals(regex2, null))
			{
				return (regex1.Pattern == regex2.Pattern)
					&& (regex1.CaseInsensitive == regex2.CaseInsensitive)
					;
			}

			return false;
		}

		#region IEquatable<T> implementation

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type
		/// </summary>
		/// <param name="regex">An object to compare with this object</param>
		/// <returns><c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c></returns>
		public bool Equals(SimpleRegex regex)
		{
			if (ReferenceEquals(regex, null))
			{
				return false;
			}

			return InternalEquals(this, regex);
		}

		#endregion

		#region Object overrides

		/// <summary>
		/// Determines whether the specified <see cref="SimpleRegex"/> is equal to
		/// the current <see cref="SimpleRegex"/>
		/// </summary>
		/// <param name="obj">The object to compare with the current object</param>
		/// <returns><c>true</c> if the specified <see cref="SimpleRegex"/> is equal to
		/// the current <see cref="SimpleRegex"/>; otherwise, <c>false</c></returns>
		public override bool Equals(object obj)
		{
			var regex = obj as SimpleRegex;
			if (ReferenceEquals(regex, null))
			{
				return false;
			}

			return InternalEquals(this, regex);
		}

		/// <summary>
		/// Serves as a hash function for a simple regular expression
		/// </summary>
		/// <returns>A hash code for the current <see cref="SimpleRegex"/></returns>
		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + (_pattern != null ? _pattern.GetHashCode() : 0);
				hash = hash * 23 + _caseInsensitive.GetHashCode();

				return hash;
			}
		}

		/// <summary>
		/// Returns a string that represents the simple regular expression
		/// </summary>
		/// <returns>A string that represents the simple regular expression</returns>
		public override string ToString()
		{
			var stringBuilderPool = StringBuilderPool.Shared;
			StringBuilder sb = stringBuilderPool.Rent();

			sb.Append('/');
			sb.Append(EscapePatternForLiteral(_pattern));
			sb.Append('/');
			if (_caseInsensitive)
			{
				sb.Append('i');
			}

			string regularExpressionString = sb.ToString();
			stringBuilderPool.Return(sb);

			return regularExpressionString;
		}

		public static bool operator ==(SimpleRegex regex1, SimpleRegex regex2)
		{
			return InternalEquals(regex1, regex2);
		}

		public static bool operator !=(SimpleRegex regex1, SimpleRegex regex2)
		{
			return !InternalEquals(regex1, regex2);
		}

		#endregion
	}
}