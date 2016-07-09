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
		private static readonly Regex _templateTagRegex = new Regex(
			CURLY_BRACKET_TAG_PATTERN + "|" + ES6_STRING_INTERPOLATION_PATTERN + "|" + SQUARE_BRACKET_TAG_PATTERN);


		/// <summary>
		/// Determines whether a markup contains the template tags
		/// </summary>
		/// <param name="content">Content</param>
		/// <returns>Result of check (true - contains; false - not contains)</returns>
		public static bool ContainsTag(string content)
		{
			return content.IndexOfAny(_templateTagFirstChars) != -1 && _templateTagRegex.IsMatch(content);
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

			MatchCollection templateTagMatches = _templateTagRegex.Matches(content);
			if (templateTagMatches.Count == 0)
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

			foreach (Match templateTagMatch in templateTagMatches)
			{
				int templateTagPosition = templateTagMatch.Index;
				int templateTagLength = templateTagMatch.Length;

				if (templateTagPosition > currentPosition)
				{
					string text = content.Substring(currentPosition, templateTagPosition - currentPosition);

					if (textHandler != null)
					{
						textHandler(context, text);
					}

					innerContext.IncreasePosition(text.Length);
				}

				GroupCollection templateTagGroups = templateTagMatch.Groups;

				string expression = templateTagGroups["expression"].Value;
				string startDelimiter = templateTagGroups["startDelimiter"].Value;
				string endDelimiter = templateTagGroups["endDelimiter"].Value;

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