using System.Text.RegularExpressions;

using WebMarkupMin.Core.Parsers;

namespace WebMarkupMin.Core.Helpers
{
	/// <summary>
	/// Mustache-style tag helpers
	/// </summary>
	internal static class MustacheStyleTagHelpers
	{
		/// <summary>
		/// Regular expression for working with the Mustache-style tags
		/// </summary>
		private static readonly Regex _mustacheStyleTagRegex = new Regex(
			@"(?<startDelimiter>\{{2})(?<expression>[\s\S]+?)(?<endDelimiter>\}{2})(?!\})");


		/// <summary>
		/// Determines whether a markup contains the Mustache-style tags
		/// </summary>
		/// <param name="content">Content</param>
		/// <returns>Result of check (true - contains; false - not contains)</returns>
		public static bool ContainsMustacheStyleTag(string content)
		{
			return _mustacheStyleTagRegex.IsMatch(content);
		}

		/// <summary>
		/// Parses a Mustache-style markup
		/// </summary>
		/// <param name="content">Mustache-style markup</param>
		/// <param name="mustacheStyleTagHandler">Mustache-style tags handler</param>
		/// <param name="textHandler">Text handler</param>
		public static void ParseMarkup(string content, MustacheStyleTagDelegate mustacheStyleTagHandler,
			TextDelegate textHandler)
		{
			var innerContext = new InnerMarkupParsingContext(content);
			var context = new MarkupParsingContext(innerContext);

			MatchCollection mustacheStyleTagMatches = _mustacheStyleTagRegex.Matches(content);
			if (mustacheStyleTagMatches.Count == 0)
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

			foreach (Match mustacheStyleTagMatch in mustacheStyleTagMatches)
			{
				int mustacheStyleTagPosition = mustacheStyleTagMatch.Index;
				int mustacheStyleTagLength = mustacheStyleTagMatch.Length;

				if (mustacheStyleTagPosition > currentPosition)
				{
					string text = content.Substring(currentPosition, mustacheStyleTagPosition - currentPosition);

					if (textHandler != null)
					{
						textHandler(context, text);
					}

					innerContext.IncreasePosition(text.Length);
				}

				GroupCollection mustacheStyleTagGroups = mustacheStyleTagMatch.Groups;

				string expression = mustacheStyleTagGroups["expression"].Value;
				string startDelimiter = mustacheStyleTagGroups["startDelimiter"].Value;
				string endDelimiter = mustacheStyleTagGroups["endDelimiter"].Value;

				if (expression.StartsWith("{") && expression.EndsWith("}"))
				{
					expression = expression.Substring(1, expression.Length - 2);
					startDelimiter = "{{{";
					endDelimiter = "}}}";
				}

				if (mustacheStyleTagHandler != null)
				{
					mustacheStyleTagHandler(context, expression, startDelimiter, endDelimiter);
				}

				innerContext.IncreasePosition(mustacheStyleTagLength);
				currentPosition = mustacheStyleTagPosition + mustacheStyleTagLength;
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

		/// <summary>
		/// Mustache-style tag delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="expression">Expression</param>
		/// <param name="startDelimiter">Start delimiter</param>
		/// <param name="endDelimiter">End delimiter</param>
		public delegate void MustacheStyleTagDelegate(MarkupParsingContext context, string expression,
			string startDelimiter, string endDelimiter);

		/// <summary>
		/// Text delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="text">Text</param>
		public delegate void TextDelegate(MarkupParsingContext context, string text);
	}
}