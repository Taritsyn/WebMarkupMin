using System;
using System.Text.RegularExpressions;

using WebMarkupMin.Core.Parsers;

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
		/// Regular expression for working with the Knockout begin containerless comment
		/// </summary>
		private static readonly Regex _koBeginContainerlessCommentRegex =
			new Regex(@"^\s*" + KO_CONTAINERLESS_COMMENT_PREFIX + @"(?:\s+(?<expression>[\s\S]+))?\s*$");

		/// <summary>
		/// Regular expression for working with the Knockout end containerless comment
		/// </summary>
		private static readonly Regex _koEndContainerlessCommentRegex =
			new Regex(@"^\s*/" + KO_CONTAINERLESS_COMMENT_PREFIX + @"\s*$");


		/// <summary>
		/// Checks whether the comment is the Knockout begin containerless comment
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <returns>Result of check (true - is begin containerless comment; false - is not begin containerless comment)</returns>
		public static bool IsBeginContainerlessComment(string commentText)
		{
			if (commentText.IndexOf(KO_CONTAINERLESS_COMMENT_PREFIX, StringComparison.Ordinal) == -1)
			{
				return false;
			}

			return _koBeginContainerlessCommentRegex.IsMatch(commentText);
		}

		/// <summary>
		/// Parses a Knockout begin containerless comment
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <param name="expressionHandler">Binding expression handler</param>
		public static void ParseBeginContainerlessComment(string commentText,
			ExpressionDelegate expressionHandler)
		{
			Match koBeginContainerlessCommentMatch = _koBeginContainerlessCommentRegex.Match(commentText);

			if (koBeginContainerlessCommentMatch.Success)
			{
				var innerContext = new InnerMarkupParsingContext(commentText);
				var context = new MarkupParsingContext(innerContext);

				Group expressionGroup = koBeginContainerlessCommentMatch.Groups["expression"];
				int expressionPosition = expressionGroup.Index;
				string expression = expressionGroup.Value.TrimEnd();

				innerContext.IncreasePosition(expressionPosition);

				if (expressionHandler != null)
				{
					expressionHandler(context, expression);
				}
			}
		}

		/// <summary>
		/// Checks whether the comment is the Knockout end containerless comment
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <returns>Result of check (true - is end containerless comment; false - is not end containerless comment)</returns>
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