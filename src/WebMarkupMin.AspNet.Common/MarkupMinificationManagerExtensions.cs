using System;
using System.Collections.Generic;

using WebMarkupMin.AspNet.Common.UrlMatchers;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Markup minification manager extensions
	/// </summary>
	public static class MarkupMinificationManagerExtensions
	{
		/// <summary>
		/// Checks whether the HTTP method is supported
		/// </summary>
		/// <param name="minificationManager">Markup minification manager</param>
		/// <param name="method">HTTP method</param>
		/// <returns>Result of check (true - supported; false - not supported)</returns>
		public static bool IsSupportedHttpMethod(this IMarkupMinificationManager minificationManager,
			string method)
		{
			return minificationManager.SupportedHttpMethods.Contains(method);
		}

		/// <summary>
		/// Checks whether the media-type is supported
		/// </summary>
		/// <param name="minificationManager">Markup minification manager</param>
		/// <param name="mediaType">Media-type</param>
		/// <returns>Result of check (true - supported; false - not supported)</returns>
		public static bool IsSupportedMediaType(this IMarkupMinificationManager minificationManager,
			string mediaType)
		{
			return minificationManager.SupportedMediaTypes.Contains(mediaType);
		}

		/// <summary>
		/// Сheck whether to allow processing of page
		/// </summary>
		/// <param name="minificationManager">Markup minification manager</param>
		/// <param name="pageUrl">URL of page</param>
		/// <returns>Result of check (true - allowed; false - disallowed)</returns>
		public static bool IsProcessablePage(this IMarkupMinificationManager minificationManager,
			string pageUrl)
		{
			IList<IUrlMatcher> includedPages = minificationManager.IncludedPages;
			IList<IUrlMatcher> excludedPages = minificationManager.ExcludedPages;

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

		/// <summary>
		/// Appends a <code>*-Minification-Powered-By</code> HTTP-header
		/// </summary>
		/// <param name="minificationManager">Markup minification manager</param>
		/// <param name="append">HTTP header appending delegate</param>
		public static void AppendPoweredByHttpHeader(this IMarkupMinificationManager minificationManager,
			Action<string, string> append)
		{
			KeyValuePair<string, string> poweredByHttpHeader = minificationManager.PoweredByHttpHeader;

			append(poweredByHttpHeader.Key, poweredByHttpHeader.Value);
		}
	}
}