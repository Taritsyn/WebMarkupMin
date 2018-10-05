using System;
using System.Text.RegularExpressions;

using WebMarkupMin.Core.Parsers;

namespace WebMarkupMin.Core.Helpers
{
	/// <summary>
	/// Angular helpers
	/// </summary>
	internal static class AngularHelpers
	{
		/// <summary>
		/// Angular comment directive prefix
		/// </summary>
		const string NG_COMMENT_DIRECTIVE_PREFIX = "directive:";

		/// <summary>
		/// Regular expression for working with the Angular directive prefixes
		/// </summary>
		private static readonly Regex _prefixRegex = new Regex(@"^(?:x|data)[-_:]", RegexOptions.IgnoreCase);

		/// <summary>
		/// Regular expression for working with the Angular class directive
		/// </summary>
		private static readonly Regex _ngClassDirectiveRegex = new Regex(
			@"(?<directiveName>[\w-]+)(?:\:(?<expression>[^;]+))?(?<semicolon>;)?");

		/// <summary>
		/// Regular expression for working with the Angular comment directive
		/// </summary>
		private static readonly Regex _ngCommentDirectiveRegex = new Regex(
			@"^\s*directive\:\s*(?<directiveName>[\w-]+)\s+(?<expression>.*)$");

		/// <summary>
		/// Regular expression for working with special characters
		/// </summary>
		private static readonly Regex _specialCharsRegex = new Regex(@"[-_:]+(?<letter>.)");


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
		/// <returns>Result of check (true - is class directive; false - is not class directive)</returns>
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
		/// <param name="directiveNameHandler">Directive name handler</param>
		/// <param name="expressionHandler">Binding expression handler</param>
		/// <param name="semicolonHandler">Semicolon handler</param>
		public static void ParseClassDirective(string className, DirectiveNameDelegate directiveNameHandler,
			ExpressionDelegate expressionHandler, SemicolonDelegate semicolonHandler)
		{
			MatchCollection matches = _ngClassDirectiveRegex.Matches(className);
			int matchCount = matches.Count;

			if (matchCount > 0)
			{
				var innerContext = new InnerMarkupParsingContext(className);
				var context = new MarkupParsingContext(innerContext);
				int currentPosition = 0;

				for (int matchIndex = 0; matchIndex < matchCount; matchIndex++)
				{
					Match match = matches[matchIndex];
					GroupCollection groups = match.Groups;

					Group directiveNameGroup = groups["directiveName"];
					int directiveNamePosition = directiveNameGroup.Index;
					string originalDirectiveName = directiveNameGroup.Value;
					string normalizedDirectiveName = NormalizeDirectiveName(originalDirectiveName);

					innerContext.IncreasePosition(directiveNamePosition - currentPosition);
					currentPosition = directiveNamePosition;

					if (directiveNameHandler != null)
					{
						directiveNameHandler(context, originalDirectiveName, normalizedDirectiveName);
					}

					Group expressionGroup = groups["expression"];
					if (expressionGroup.Success)
					{
						int expressionPosition = expressionGroup.Index;
						string expression = expressionGroup.Value.Trim();

						innerContext.IncreasePosition(expressionPosition - currentPosition);
						currentPosition = expressionPosition;

						if (expressionHandler != null)
						{
							expressionHandler(context, expression);
						}
					}

					Group semicolonGroup = groups["semicolon"];
					if (semicolonGroup.Success)
					{
						int semicolonPosition = semicolonGroup.Index;

						innerContext.IncreasePosition(semicolonPosition - currentPosition);
						currentPosition = semicolonPosition;

						if (semicolonHandler != null)
						{
							semicolonHandler(context);
						}
					}
				}
			}
		}

		/// <summary>
		/// Checks whether the comment is the Angular comment directive
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <returns>Result of check (true - is comment directive; false - is not comment directive)</returns>
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
		/// <param name="directiveNameHandler">Directive name handler</param>
		/// <param name="expressionHandler">Binding expression handler</param>
		public static void ParseCommentDirective(string commentText, DirectiveNameDelegate directiveNameHandler,
			ExpressionDelegate expressionHandler)
		{
			Match ngCommentDirectiveMatch = _ngCommentDirectiveRegex.Match(commentText);

			if (ngCommentDirectiveMatch.Success)
			{
				var innerContext = new InnerMarkupParsingContext(commentText);
				var context = new MarkupParsingContext(innerContext);

				GroupCollection groups = ngCommentDirectiveMatch.Groups;

				Group directiveNameGroup = groups["directiveName"];
				int directiveNamePosition = directiveNameGroup.Index;
				string originalDirectiveName = directiveNameGroup.Value;
				string normalizedDirectiveName = NormalizeDirectiveName(originalDirectiveName);

				innerContext.IncreasePosition(directiveNamePosition);

				if (directiveNameHandler != null)
				{
					directiveNameHandler(context, originalDirectiveName, normalizedDirectiveName);
				}

				Group expressionGroup = groups["expression"];
				if (expressionGroup.Success)
				{
					int expressionPosition = expressionGroup.Index;
					string expression = expressionGroup.Value.Trim();

					innerContext.IncreasePosition(expressionPosition - directiveNamePosition);

					if (expressionHandler != null)
					{
						expressionHandler(context, expression);
					}
				}
			}
		}

		/// <summary>
		/// Angular directive name delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="originalDirectiveName">Original directive name</param>
		/// <param name="normalizedDirectiveName">Normalized directive name</param>
		public delegate void DirectiveNameDelegate(MarkupParsingContext context, string originalDirectiveName,
			string normalizedDirectiveName);

		/// <summary>
		/// Angular binding expression delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="expression">Binding expression</param>
		public delegate void ExpressionDelegate(MarkupParsingContext context, string expression);

		/// <summary>
		/// Semicolon delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		public delegate void SemicolonDelegate(MarkupParsingContext context);
	}
}