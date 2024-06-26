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
		/// Regular expression for working with the React start DOM component comment
		/// </summary>
		private static readonly Regex _reactStartDomComponentCommentRegex = new Regex(
			@"^ " + REACT_DOM_COMPONENT_PREFIX + @"[a-z]+\: \d+ $",
			TargetFrameworkShortcuts.PerformanceRegexOptions);

		/// <summary>
		/// Regular expression for working with the React end DOM component comment
		/// </summary>
		private static readonly Regex _reactEndDomComponentCommentRegex = new Regex(
			@"^ /" + REACT_DOM_COMPONENT_PREFIX + @"[a-z]+ $",
			TargetFrameworkShortcuts.PerformanceRegexOptions);


		/// <summary>
		/// Checks whether the comment is the React start DOM component comment
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <returns>Result of check (<c>true</c> - is start DOM component comment;
		/// <c>false</c> - is not start DOM component comment)</returns>
		private static bool IsStartDomComponentComment(string commentText)
		{
			return _reactStartDomComponentCommentRegex.IsMatch(commentText);
		}

		/// <summary>
		/// Checks whether the comment is the React end DOM component comment
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <returns>Result of check (<c>true</c> - is end DOM component comment;
		/// <c>false</c> - is not end DOM component comment)</returns>
		private static bool IsEndDomComponentComment(string commentText)
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

			return IsStartDomComponentComment(commentText)
				|| IsEndDomComponentComment(commentText)
				;
		}
	}
}