using System;
using System.Text.RegularExpressions;

using WebMarkupMin.Core.Parsers;
using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Helpers
{
	/// <summary>
	/// Angular helpers
	/// </summary>
	internal static class AngularHelpers
	{
		/// <summary>
		/// Angular directive name pattern
		/// </summary>
		const string NG_DIRECTIVE_NAME_PATTERN = @"[\w-]+";

		/// <summary>
		/// Angular comment directive prefix
		/// </summary>
		const string NG_COMMENT_DIRECTIVE_PREFIX = "directive:";

		/// <summary>
		/// Regular expression for working with the Angular directive prefixes
		/// </summary>
		private static readonly Regex _prefixRegex = new Regex(@"^(?:x|data)[-_:]",
			RegexOptions.IgnoreCase | TargetFrameworkShortcuts.PerformanceRegexOptions);

		/// <summary>
		/// Regular expression for parsing the Angular class directive
		/// </summary>
		private static readonly Regex _ngClassDirectiveRegex = new Regex(@"(?<directiveName>" + NG_DIRECTIVE_NAME_PATTERN + @")" +
			@"(?:\:(?<expression>[^;]+))?(?<semicolon>;)?",
			TargetFrameworkShortcuts.PerformanceRegexOptions);

		/// <summary>
		/// Regular expression for parsing the Angular comment directive
		/// </summary>
		private static readonly Regex _ngCommentDirectiveRegex = new Regex(
			@"^\s*" + NG_COMMENT_DIRECTIVE_PREFIX + @"\s*" +
			@"(?<directiveName>" + NG_DIRECTIVE_NAME_PATTERN + @")\s+" +
			@"(?<expression>.*)$",
			TargetFrameworkShortcuts.PerformanceRegexOptions)
			;

		/// <summary>
		/// Regular expression for working with special characters
		/// </summary>
		private static readonly Regex _specialCharsRegex = new Regex(@"[-_:]+(?<letter>.)",
			TargetFrameworkShortcuts.PerformanceRegexOptions);


		/// <summary>
		/// Normalizes a directive name
		/// </summary>
		/// <param name="directiveName">Directive name</param>
		/// <returns>Normalized directive name</returns>
		public static string NormalizeDirectiveName(string directiveName)
		{
			string processedDirectiveName = ToCamelCase(_prefixRegex.Replace(directiveName, string.Empty));

			return processedDirectiveName;
		}

		/// <summary>
		/// Converts a string value to camel case
		/// </summary>
		/// <param name="value">String value</param>
		/// <returns>Processed string value</returns>
		private static string ToCamelCase(string value)
		{
			string result = _specialCharsRegex.Replace(value, m =>
			{
				int position = m.Index;
				string letter = m.Groups["letter"].Value;

				return position > 0 ? letter.ToUpperInvariant() : letter;
			});

			return result;
		}

		/// <summary>
		/// Checks whether the class is the Angular class directive
		/// </summary>
		/// <param name="className">Class name</param>
		/// <returns>Result of check (<c>true</c> - is class directive; <c>false</c> - is not class directive)</returns>
		public static bool IsClassDirective(string className)
		{
			if (className.IndexOf(':') == -1 && className.IndexOf(';') == -1)
			{
				return false;
			}

			bool isClassDirective = _ngClassDirectiveRegex.IsMatch(className);

			return isClassDirective;
		}

		/// <summary>
		/// Parses a Angular class directive
		/// </summary>
		/// <param name="className">Class name</param>
		/// <param name="classDirectiveHandler">Angular class directive handler</param>
		/// <param name="otherContentHandler">Other content handler</param>
		public static void ParseClassDirective(string className, ClassDirectiveDelegate classDirectiveHandler,
			OtherContentDelegate otherContentHandler)
		{
			int classNameLength = className.Length;
			int currentPosition = 0;
			int remainderLength = classNameLength;

			var innerContext = new InnerMarkupParsingContext(className);
			var context = new MarkupParsingContext(innerContext);

			Match match = _ngClassDirectiveRegex.Match(className, currentPosition, remainderLength);
			while (match.Success)
			{
				GroupCollection groups = match.Groups;

				Group directiveNameGroup = groups["directiveName"];
				int directiveNamePosition = directiveNameGroup.Index;
				string directiveName = directiveNameGroup.Value;

				if (directiveNamePosition > currentPosition)
				{
					string otherContent = className.Substring(currentPosition,
						directiveNamePosition - currentPosition);
					otherContentHandler?.Invoke(context, otherContent);
				}

				Group expressionGroup = groups["expression"];
				string expression = string.Empty;

				if (expressionGroup.Success)
				{
					int expressionPosition = expressionGroup.Index;
					expression = expressionGroup.Value.Trim();

					innerContext.IncreasePosition(expressionPosition - currentPosition);
					currentPosition = expressionPosition;
					remainderLength = classNameLength - currentPosition;
				}

				Group semicolonGroup = groups["semicolon"];
				bool endsWithSemicolon = semicolonGroup.Success;

				classDirectiveHandler?.Invoke(context, directiveName, expression, endsWithSemicolon);

				int nextItemPosition = match.Index + match.Length;

				innerContext.IncreasePosition(nextItemPosition - currentPosition);
				currentPosition = nextItemPosition;
				remainderLength = classNameLength - currentPosition;

				match = _ngClassDirectiveRegex.Match(className, currentPosition, remainderLength);
			}

			if (remainderLength > 0)
			{
				string otherContent = className.Substring(currentPosition, remainderLength);
				otherContentHandler?.Invoke(context, otherContent);
			}
		}

		/// <summary>
		/// Checks whether the comment is the Angular comment directive
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <returns>Result of check (<c>true</c> - is comment directive; <c>false</c> - is not comment directive)</returns>
		public static bool IsCommentDirective(string commentText)
		{
			if (commentText.IndexOf(NG_COMMENT_DIRECTIVE_PREFIX, StringComparison.Ordinal) == -1)
			{
				return false;
			}

			return _ngCommentDirectiveRegex.IsMatch(commentText);
		}

		/// <summary>
		/// Parses a Angular comment directive
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <param name="commentDirectiveHandler">Angular comment directive handler</param>
		public static void ParseCommentDirective(string commentText, CommentDirectiveDelegate commentDirectiveHandler)
		{
			Match match = _ngCommentDirectiveRegex.Match(commentText);
			if (match.Success)
			{
				var innerContext = new InnerMarkupParsingContext(commentText);
				var context = new MarkupParsingContext(innerContext);

				GroupCollection groups = match.Groups;

				Group directiveNameGroup = groups["directiveName"];
				string directiveName = directiveNameGroup.Value;

				Group expressionGroup = groups["expression"];
				if (expressionGroup.Success)
				{
					int expressionPosition = expressionGroup.Index;
					string expression = expressionGroup.Value.Trim();

					innerContext.IncreasePosition(expressionPosition);
					commentDirectiveHandler?.Invoke(context, directiveName, expression);
				}
			}
		}

		/// <summary>
		/// Angular class directive delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="directiveName">Directive name</param>
		/// <param name="expression">Binding expression</param>
		/// <param name="endsWithSemicolon">Flag whether the directive is ends with a semicolon</param>
		public delegate void ClassDirectiveDelegate(MarkupParsingContext context, string directiveName,
			string expression, bool endsWithSemicolon);

		/// <summary>
		/// Angular comment directive delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="directiveName">Directive name</param>
		/// <param name="expression">Binding expression</param>
		public delegate void CommentDirectiveDelegate(MarkupParsingContext context, string directiveName,
			string expression);

		/// <summary>
		/// Other content delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="content">Other content</param>
		public delegate void OtherContentDelegate(MarkupParsingContext context, string content);
	}
}