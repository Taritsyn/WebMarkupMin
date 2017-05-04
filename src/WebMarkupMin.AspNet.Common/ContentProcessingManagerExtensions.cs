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
			IList<IUrlMatcher> excludedPages = contentProcessingManager.ExcludedPages;

			if (includedPages.Count == 0 && excludedPages.Count == 0)
			{
				return true;
			}

			if (excludedPages.Count > 0)
			{
				foreach (IUrlMatcher matcher in excludedPages)
				{
					bool isExcludedPage = matcher.IsMatch(pageUrl);
					if (isExcludedPage)
					{
						return false;
					}
				}
			}

			if (includedPages.Count > 0)
			{
				bool isIncludedPage = false;

				foreach (IUrlMatcher matcher in includedPages)
				{
					isIncludedPage = matcher.IsMatch(pageUrl);
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