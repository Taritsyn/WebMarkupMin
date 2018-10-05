using System;
using System.Text.RegularExpressions;

using WebMarkupMin.Core.Parsers;

namespace WebMarkupMin.Core.Helpers
{
	/// <summary>
	/// Template tag helpers
	/// </summary>
	internal static class TemplateTagHelpers
	{
		/// <summary>
		/// Curly bracket tag pattern
		/// </summary>
		private const string CURLY_BRACKET_TAG_PATTERN =
			@"(?<startDelimiter>\{{2})(?<expression>[\s\S]+?)(?<endDelimiter>\}{2})(?!\})";

		/// <summary>
		/// Square bracket tag pattern
		/// </summary>
		private const string SQUARE_BRACKET_TAG_PATTERN =
			@"(?<startDelimiter>\[{2})(?<expression>[\s\S]+?)(?<endDelimiter>\]{2})(?!\])";

		/// <summary>
		/// ES6 string interpolation pattern
		/// </summary>
		private const string ES6_STRING_INTERPOLATION_PATTERN =
			@"(?<startDelimiter>\$\{)(?<expression>[^{][\s\S]*?)(?<endDelimiter>\})(?!\})";

		/// <summary>
		/// Array of template tag first characters
		/// </summary>
		private static readonly char[] _templateTagFirstChars = { '{', '$', '[' };

		/// <summary>
		/// Regular expression for working with the template tags
		/// </summary>
		private static readonly Regex _templateTagRegex = new Regex(CURLY_BRACKET_TAG_PATTERN +
			"|" + ES6_STRING_INTERPOLATION_PATTERN +
			"|" + SQUARE_BRACKET_TAG_PATTERN);


		/// <summary>
		/// Determines whether a markup contains the template tags
		/// </summary>
		/// <param name="content">Content</param>
		/// <returns>Result of check (true - contains; false - not contains)</returns>
		public static bool ContainsTag(string content)
		{
			if (content.IndexOfAny(_templateTagFirstChars) == -1)
			{
				return false;
			}

			return _templateTagRegex.IsMatch(content);
		}

		/// <summary>
		/// Parses a markup of template
		/// </summary>
		/// <param name="content">Markup of template</param>
		/// <param name="templateTagHandler">Template tags delegate</param>
		/// <param name="textHandler">Text delegate</param>
		public static void ParseMarkup(string content,
			HtmlParsingHandlers.TemplateTagDelegate templateTagHandler,
			HtmlParsingHandlers.TextDelegate textHandler)
		{
			var innerContext = new InnerMarkupParsingContext(content);
			var context = new MarkupParsingContext(innerContext);

			MatchCollection matches = _templateTagRegex.Matches(content);
			int matchCount = matches.Count;

			if (matchCount == 0)
			{
				if (textHandler != null)
				{
					textHandler(context, content);
				}

				innerContext.IncreasePosition(content.Length);

				return;
			}

			int currentPosition = 0;
			int endPosition = content.Length - 1;

			for (int matchIndex = 0; matchIndex < matchCount; matchIndex++)
			{
				Match match = matches[matchIndex];
				int templateTagPosition = match.Index;
				int templateTagLength = match.Length;

				if (templateTagPosition > currentPosition)
				{
					string text = content.Substring(currentPosition, templateTagPosition - currentPosition);

					if (textHandler != null)
					{
						textHandler(context, text);
					}

					innerContext.IncreasePosition(text.Length);
				}

				GroupCollection groups = match.Groups;
				string expression = groups["expression"].Value;
				string startDelimiter = groups["startDelimiter"].Value;
				string endDelimiter = groups["endDelimiter"].Value;

				if (templateTagHandler != null)
				{
					templateTagHandler(context, expression, startDelimiter, endDelimiter);
				}

				innerContext.IncreasePosition(templateTagLength);
				currentPosition = templateTagPosition + templateTagLength;
			}

			if (currentPosition > 0 && currentPosition <= endPosition)
			{
				string text = content.Substring(currentPosition, endPosition - currentPosition + 1);

				if (textHandler != null)
				{
					textHandler(context, text);
				}

				innerContext.IncreasePosition(text.Length);
			}
		}
	}
}