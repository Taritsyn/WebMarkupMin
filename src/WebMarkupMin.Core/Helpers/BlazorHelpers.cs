using System;

namespace WebMarkupMin.Core.Helpers
{
	/// <summary>
	/// Blazor helpers
	/// </summary>
	internal static class BlazorHelpers
	{
		/// <summary>
		/// Blazor component marker prefix
		/// </summary>
		const string BLAZOR_COMPONENT_MARKER_PREFIX = "Blazor:";


		/// <summary>
		/// Checks whether the comment is the Blazor component marker
		/// </summary>
		/// <param name="commentText">Comment text</param>
		/// <returns>Result of check (true - is component marker;
		/// false - is not component marker)</returns>
		public static bool IsComponentMarker(string commentText)
		{
			return commentText.StartsWith(BLAZOR_COMPONENT_MARKER_PREFIX, StringComparison.Ordinal);
		}
	}
}