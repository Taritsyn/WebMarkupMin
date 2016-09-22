using System;

using Microsoft.AspNetCore.Http;

namespace WebMarkupMin.AspNetCore1
{
	/// <summary>
	/// Header dictionary extensions
	/// </summary>
	internal static class HeaderDictionaryExtensions
	{
		/// <summary>
		/// Checks whether the content is encoded
		/// </summary>
		/// <param name="headers">The <see cref="IHeaderDictionary"/> to use</param>
		/// <returns>Result of check (true - content is encoded; false - content is not encoded)</returns>
		public static bool IsEncodedContent(this IHeaderDictionary headers)
		{
			return headers.ContainsKey("Content-Encoding")
				&& !headers["Content-Encoding"].ToString().Equals("identity", StringComparison.OrdinalIgnoreCase);
		}
	}
}