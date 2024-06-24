using System;
using System.Text.RegularExpressions;

using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Helpers
{
	/// <summary>
	/// React helpers
	/// </summary>
	internal static class ReactHelpers
	{
		/// <summary>
		/// React DOM component prefix
		/// </summary>
		const string REACT_DOM_COMPONENT_PREFIX = "react-";

		/// <summary>
		/// Regular expression for working with the React begin DOM component comment
		/// </summary>
		private static readonly Regex _reactBeginDomComponentCommentRegex = new Regex(
			@"^ " + REACT_DOM_COMPONENT_PREFIX + @"[a-z]+\: \d+ $",
			TargetFrameworkShortcuts.PerformanceRegexOptions);

		/// <summary>
		/// Regular expression for working with the React end DOM component comment
		/// </summary>
		private static readonly Regex _reactEndDomComponentCommentRegex = new Regex(
			@"^ /" + REACT_DOM_COMPONENT_PREFIX + @"[a-z]+ $",
			TargetFrameworkShortcuts.PerformanceRegexOptions);


		/// <summary>
		/// Checks whether the comment is the React begin DOM component comment
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <returns>Result of check (<c>true</c> - is begin DOM component comment;
		/// <c>false</c> - is not begin DOM component comment)</returns>
		public static bool IsBeginDomComponentComment(string commentText)
		{
			return _reactBeginDomComponentCommentRegex.IsMatch(commentText);
		}

		/// <summary>
		/// Checks whether the comment is the React end DOM component comment
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <returns>Result of check (<c>true</c> - is end DOM component comment;
		/// <c>false</c> - is not end DOM component comment)</returns>
		public static bool IsEndDomComponentComment(string commentText)
		{
			return _reactEndDomComponentCommentRegex.IsMatch(commentText);
		}

		/// <summary>
		/// Checks whether the comment is the React DOM component comment
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <returns>Result of check (<c>true</c> - is DOM component comment;
		/// <c>false</c> - is not DOM component comment)</returns>
		public static bool IsDomComponentComment(string commentText)
		{
			if (commentText.IndexOf(REACT_DOM_COMPONENT_PREFIX, StringComparison.Ordinal) == -1)
			{
				return false;
			}

			return IsBeginDomComponentComment(commentText)
				|| IsEndDomComponentComment(commentText)
				;
		}
	}
}