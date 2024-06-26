using System;
using System.Text.RegularExpressions;

using WebMarkupMin.Core.Parsers;
using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Helpers
{
	/// <summary>
	/// Knockout helpers
	/// </summary>
	internal static class KnockoutHelpers
	{
		/// <summary>
		/// Knockout containerless comment prefix
		/// </summary>
		const string KO_CONTAINERLESS_COMMENT_PREFIX = "ko";

		/// <summary>
		/// Regular expression for working with the Knockout start containerless comment
		/// </summary>
		private static readonly Regex _koStartContainerlessCommentRegex =
			new Regex(@"^\s*" + KO_CONTAINERLESS_COMMENT_PREFIX + @"(?:\s+(?<expression>[\s\S]+))?\s*$",
				TargetFrameworkShortcuts.PerformanceRegexOptions);

		/// <summary>
		/// Regular expression for working with the Knockout end containerless comment
		/// </summary>
		private static readonly Regex _koEndContainerlessCommentRegex =
			new Regex(@"^\s*/" + KO_CONTAINERLESS_COMMENT_PREFIX + @"\s*$",
				TargetFrameworkShortcuts.PerformanceRegexOptions);


		/// <summary>
		/// Checks whether the comment is the Knockout start containerless comment
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <returns>Result of check (<c>true</c> - is start containerless comment;
		/// <c>false</c> - is not start containerless comment)</returns>
		public static bool IsStartContainerlessComment(string commentText)
		{
			if (commentText.IndexOf(KO_CONTAINERLESS_COMMENT_PREFIX, StringComparison.Ordinal) == -1)
			{
				return false;
			}

			return _koStartContainerlessCommentRegex.IsMatch(commentText);
		}

		/// <summary>
		/// Parses a Knockout start containerless comment
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <param name="expressionHandler">Binding expression handler</param>
		public static void ParseStartContainerlessComment(string commentText,
			ExpressionDelegate expressionHandler)
		{
			Match match = _koStartContainerlessCommentRegex.Match(commentText);
			if (match.Success)
			{
				var innerContext = new InnerMarkupParsingContext(commentText);
				var context = new MarkupParsingContext(innerContext);

				Group expressionGroup = match.Groups["expression"];
				int expressionPosition = expressionGroup.Index;
				string expression = expressionGroup.Value.TrimEnd(null);

				innerContext.IncreasePosition(expressionPosition);
				expressionHandler?.Invoke(context, expression);
			}
		}

		/// <summary>
		/// Checks whether the comment is the Knockout end containerless comment
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <returns>Result of check (<c>true</c> - is end containerless comment;
		/// <c>false</c> - is not end containerless comment)</returns>
		public static bool IsEndContainerlessComment(string commentText)
		{
			if (commentText.IndexOf(KO_CONTAINERLESS_COMMENT_PREFIX, StringComparison.Ordinal) == -1)
			{
				return false;
			}

			return _koEndContainerlessCommentRegex.IsMatch(commentText);
		}

		/// <summary>
		/// Knockout binding expression delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="expression">Binding expression</param>
		public delegate void ExpressionDelegate(MarkupParsingContext context, string expression);
	}
}