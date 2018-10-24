using System;
using System.Text;
using System.Text.RegularExpressions;

using WebMarkupMin.Core.Helpers;
using WebMarkupMin.Core.Parsers;
using WebMarkupMin.Core.Resources;
using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// HTML attribute expression
	/// </summary>
	public sealed class HtmlAttributeExpression : IEquatable<HtmlAttributeExpression>
	{
		private static readonly Regex _htmlAttributeExpressionRegex = new Regex(
			@"^(?<tagName>" + CommonRegExps.HtmlTagNamePattern + ")?" +
			@"\[" +
				@"(?<attributeName>" + CommonRegExps.HtmlAttributeNamePattern + @")" +
				@"(?:=" +
					@"(?:" +
						@"(?:" +
							@"(?:(?<quote>""|')(?<attributeValue>[\s\S]*?)(\k<quote>))" +
							@"|(?<attributeValue>[^>""'\s]+)" +
						@")" +
						@"(?<caseInsensitive>\s+i)?" +
					@")?" +
				@")?" +
			@"\]$")
			;

		/// <summary>
		/// Tag name
		/// </summary>
		private readonly string _tagName;

		/// <summary>
		/// Tag name in lowercase
		/// </summary>
		private readonly string _tagNameInLowercase;

		/// <summary>
		/// Attribute name
		/// </summary>
		private readonly string _attributeName;

		/// <summary>
		/// Attribute name in lowercase
		/// </summary>
		private readonly string _attributeNameInLowercase;

		/// <summary>
		/// Attribute value
		/// </summary>
		private readonly string _attributeValue;

		/// <summary>
		/// Flag for whether to allow case-insensitive matching for attribute value
		/// </summary>
		private readonly bool _caseInsensitive;

		/// <summary>
		/// Gets a tag name
		/// </summary>
		public string TagName
		{
			get { return _tagName; }
		}

		/// <summary>
		/// Gets a tag name in lowercase
		/// </summary>
		public string TagNameInLowercase
		{
			get { return _tagNameInLowercase; }
		}

		/// <summary>
		/// Gets a attribute name
		/// </summary>
		public string AttributeName
		{
			get { return _attributeName; }
		}

		/// <summary>
		/// Gets a attribute name in lowercase
		/// </summary>
		public string AttributeNameInLowercase
		{
			get { return _attributeNameInLowercase; }
		}

		/// <summary>
		/// Gets a attribute value
		/// </summary>
		public string AttributeValue
		{
			get { return _attributeValue; }
		}

		/// <summary>
		/// Gets a flag for whether to allow case-insensitive matching for attribute value
		/// </summary>
		public bool CaseInsensitive
		{
			get { return _caseInsensitive; }
		}


		/// <summary>
		/// Constructs an instance of the HTML attribute expression
		/// </summary>
		/// <param name="attributeName">Attribute name</param>
		public HtmlAttributeExpression(string attributeName)
			: this(null, attributeName)
		{ }

		/// <summary>
		/// Constructs an instance of the HTML attribute expression
		/// </summary>
		/// <param name="tagName">Tag name</param>
		/// <param name="attributeName">Attribute name</param>
		/// <param name="attributeValue">Attribute value</param>
		/// <param name="caseInsensitive">Flag for whether to allow case-insensitive matching for attribute value</param>
		public HtmlAttributeExpression(string tagName, string attributeName, string attributeValue = null,
			bool caseInsensitive = false)
		{
			if (attributeName == null)
			{
				throw new ArgumentNullException(nameof(attributeName));
			}

			string processedTagName = !string.IsNullOrWhiteSpace(tagName) ? tagName.Trim() : null;
			string processedAttributeName = attributeName.Trim();

			if (processedAttributeName.Length == 0)
			{
				throw new ArgumentException(Strings.Common_ValueIsEmpty, nameof(attributeName));
			}

			_tagName = processedTagName;
			if (processedTagName != null)
			{
				_tagNameInLowercase = processedTagName.ToLowerInvariant();
			}
			_attributeName = processedAttributeName;
			_attributeNameInLowercase = processedAttributeName.ToLowerInvariant();
			_attributeValue = attributeValue;
			_caseInsensitive = caseInsensitive;
		}

		/// <summary>
		/// Parses a HTML attribute expression
		/// </summary>
		/// <param name="attributeExpressionString">String representation of the HTML attribute expression</param>
		/// <returns>HTML attribute expression</returns>
		/// <exception cref="ArgumentNullException"><paramref name="attributeExpressionString"/> is null</exception>
		/// <exception cref="FormatException"><paramref name="attributeExpressionString"/> have an incorrect format</exception>
		public static HtmlAttributeExpression Parse(string attributeExpressionString)
		{
			if (attributeExpressionString == null)
			{
				throw new ArgumentNullException(nameof(attributeExpressionString));
			}

			HtmlAttributeExpression expression = InnerParse(attributeExpressionString);
			if (expression == null)
			{
				throw new FormatException(Strings.ErrorMessage_InvalidHtmlAttributeExpression);
			}

			return expression;
		}

		/// <summary>
		/// Parses a HTML attribute expression. A return value indicates whether the parsing succeeded.
		/// </summary>
		/// <param name="attributeExpressionString">String representation of the HTML attribute expression</param>
		/// <param name="result">HTML attribute expression</param>
		/// <returns>true if <paramref name="attributeExpressionString"/> was parsed successfully; otherwise, false</returns>
		public static bool TryParse(string attributeExpressionString, out HtmlAttributeExpression result)
		{
			if (attributeExpressionString == null)
			{
				result = null;
				return false;
			}

			result = InnerParse(attributeExpressionString);

			return result != null;
		}

		private static HtmlAttributeExpression InnerParse(string attributeExpressionString)
		{
			if (string.IsNullOrWhiteSpace(attributeExpressionString))
			{
				return null;
			}

			HtmlAttributeExpression expression = null;
			Match expressionMatch = _htmlAttributeExpressionRegex.Match(attributeExpressionString.Trim());

			if (expressionMatch.Success)
			{
				GroupCollection groups = expressionMatch.Groups;
				string tagName = groups["tagName"].Success ? groups["tagName"].Value : null;
				string attributeName = Regex.Unescape(groups["attributeName"].Value);
				string attributeValue = groups["attributeValue"].Success
					? Regex.Unescape(groups["attributeValue"].Value)
					: null;
				bool caseInsensitive = groups["caseInsensitive"].Success;

				expression = new HtmlAttributeExpression(tagName, attributeName, attributeValue, caseInsensitive);
			}

			return expression;
		}

		/// <summary>
		/// Indicates whether the current attribute expression matches with the specified tag name,
		/// attribute name and attribute value
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <param name="attributeNameInLowercase">Attribute name in lowercase</param>
		/// <param name="attributeValue">Attribute value</param>
		/// <returns>true if the attribute expression is match; otherwise, false</returns>
		public bool IsMatch(string tagNameInLowercase, string attributeNameInLowercase,
			string attributeValue)
		{
			if (attributeNameInLowercase == null)
			{
				throw new ArgumentNullException(nameof(attributeNameInLowercase));
			}

			if (string.IsNullOrWhiteSpace(attributeNameInLowercase))
			{
				throw new ArgumentException(Strings.Common_ValueIsEmpty, nameof(attributeNameInLowercase));
			}

			bool result = _attributeNameInLowercase == attributeNameInLowercase
				&& (_tagNameInLowercase == null || _tagNameInLowercase == tagNameInLowercase)
				&& (_attributeValue == null || _attributeValue.Equals(attributeValue,
					_caseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
				;

			return result;
		}

		/// <summary>
		/// Gets a quotation mark for attribute value
		/// </summary>
		/// <param name="attributeValue">Attribute value</param>
		/// <returns>Quotation mark</returns>
		private static string GetQuoteForAttributeValue(string attributeValue)
		{
			if (HtmlAttributeValueHelpers.IsNotRequireQuotesInHtml5(attributeValue))
			{
				return string.Empty;
			}

			return "\"";
		}

		private static bool InnerEquals(HtmlAttributeExpression expression1, HtmlAttributeExpression expression2)
		{
			if (ReferenceEquals(expression1, expression2))
			{
				return true;
			}

			if (!ReferenceEquals(expression1, null) && !ReferenceEquals(expression2, null))
			{
				return (expression1.TagNameInLowercase == expression2.TagNameInLowercase)
					&& (expression1.AttributeNameInLowercase == expression2.AttributeNameInLowercase)
					&& (expression1.AttributeValue == expression2.AttributeValue)
					&& (expression1.CaseInsensitive == expression2.CaseInsensitive)
					;
			}

			return false;
		}

		#region IEquatable<T> implementation

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type
		/// </summary>
		/// <param name="expression">An object to compare with this object</param>
		/// <returns>true if the current object is equal to the other parameter; otherwise, false</returns>
		public bool Equals(HtmlAttributeExpression expression)
		{
			if (ReferenceEquals(expression, null))
			{
				return false;
			}

			return InnerEquals(this, expression);
		}

		#endregion

		#region Object overrides

		/// <summary>
		/// Determines whether the specified <see cref="HtmlAttributeExpression"/> is equal to
		/// the current <see cref="HtmlAttributeExpression"/>
		/// </summary>
		/// <param name="obj">The object to compare with the current object</param>
		/// <returns>true if the specified <see cref="HtmlAttributeExpression"/> is equal to
		/// the current <see cref="HtmlAttributeExpression"/>; otherwise, false</returns>
		public override bool Equals(object obj)
		{
			var attributeExpression = obj as HtmlAttributeExpression;
			if (ReferenceEquals(attributeExpression, null))
			{
				return false;
			}

			return InnerEquals(this, attributeExpression);
		}

		/// <summary>
		/// Serves as a hash function for a HTML attribute expression
		/// </summary>
		/// <returns>A hash code for the current <see cref="HtmlAttributeExpression"/></returns>
		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + (_tagNameInLowercase != null ? _tagNameInLowercase.GetHashCode() : 0);
				hash = hash * 23 + (_attributeNameInLowercase != null ? _attributeNameInLowercase.GetHashCode() : 0);
				hash = hash * 23 + (_attributeValue != null ? _attributeValue.GetHashCode() : 0);
				hash = hash * 23 + _caseInsensitive.GetHashCode();

				return hash;
			}
		}

		/// <summary>
		/// Returns a string that represents the HTML attribute expression
		/// </summary>
		/// <returns>A string that represents the HTML attribute expression</returns>
		public override string ToString()
		{
			StringBuilder sb = StringBuilderPool.GetBuilder();

			if (!string.IsNullOrEmpty(_tagNameInLowercase))
			{
				sb.Append(_tagNameInLowercase);
			}
			sb.Append("[");
			sb.Append(Regex.Escape(_attributeNameInLowercase));
			if (_attributeValue != null)
			{
				string quote = GetQuoteForAttributeValue(_attributeValue);

				sb.Append("=");
				if (quote.Length > 0)
				{
					sb.Append(quote);
				}
				sb.Append(Regex.Escape(_attributeValue));
				if (quote.Length > 0)
				{
					sb.Append(quote);
				}
				if (_caseInsensitive)
				{
					sb.Append(" i");
				}
			}
			sb.Append("]");

			string attributeExpressionString = sb.ToString();
			StringBuilderPool.ReleaseBuilder(sb);

			return attributeExpressionString;
		}

		public static bool operator ==(HtmlAttributeExpression expression1, HtmlAttributeExpression expression2)
		{
			return InnerEquals(expression1, expression2);
		}

		public static bool operator !=(HtmlAttributeExpression expression1, HtmlAttributeExpression expression2)
		{
			return !InnerEquals(expression1, expression2);
		}

		#endregion
	}
}