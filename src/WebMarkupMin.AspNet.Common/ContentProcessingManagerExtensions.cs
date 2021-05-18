using System.Collections.Generic;

using WebMarkupMin.AspNet.Common.UrlMatchers;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Content processing manager extensions
	/// </summary>
	public static class ContentProcessingManagerExtensions
	{
		/// <summary>
		/// Checks whether the HTTP status code is supported
		/// </summary>
		/// <param name="contentProcessingManager">Content processing manager</param>
		/// <param name="statusCode">HTTP status code</param>
		/// <returns>Result of check (true - supported; false - not supported)</returns>
		public static bool IsSupportedHttpStatusCode(this IContentProcessingManager contentProcessingManager,
			int statusCode)
		{
			return contentProcessingManager.SupportedHttpStatusCodes.Contains(statusCode);
		}

		/// <summary>
		/// Checks whether the HTTP method is supported
		/// </summary>
		/// <param name="contentProcessingManager">Content processing manager</param>
		/// <param name="method">HTTP method</param>
		/// <returns>Result of check (true - supported; false - not supported)</returns>
		public static bool IsSupportedHttpMethod(this IContentProcessingManager contentProcessingManager,
			string method)
		{
			return contentProcessingManager.SupportedHttpMethods.Contains(method);
		}

		/// <summary>
		/// Сheck whether to allow processing of page
		/// </summary>
		/// <param name="contentProcessingManager">Content processing manager</param>
		/// <param name="pageUrl">URL of page</param>
		/// <returns>Result of check (true - allowed; false - disallowed)</returns>
		public static bool IsProcessablePage(this IContentProcessingManager contentProcessingManager,
			string pageUrl)
		{
			IList<IUrlMatcher> includedPages = contentProcessingManager.IncludedPages;
			int includedPageCount = includedPages.Count;

			IList<IUrlMatcher> excludedPages = contentProcessingManager.ExcludedPages;
			int excludedPageCount = excludedPages.Count;

			if (includedPageCount == 0 && excludedPageCount == 0)
			{
				return true;
			}

			if (excludedPageCount > 0)
			{
				for (int matcherIndex = 0; matcherIndex < excludedPageCount; matcherIndex++)
				{
					bool isExcludedPage = excludedPages[matcherIndex].IsMatch(pageUrl);
					if (isExcludedPage)
					{
						return false;
					}
				}
			}

			if (includedPageCount > 0)
			{
				bool isIncludedPage = false;

				for (int matcherIndex = 0; matcherIndex < includedPageCount; matcherIndex++)
				{
					isIncludedPage = includedPages[matcherIndex].IsMatch(pageUrl);
					if (isIncludedPage)
					{
						break;
					}
				}

				if (isIncludedPage)
				{
					return true;
				}
			}
			else
			{
				return true;
			}

			return false;
		}
	}
}