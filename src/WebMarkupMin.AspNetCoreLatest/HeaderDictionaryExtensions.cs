﻿using System;

using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2
#elif ASPNETCORE3
namespace WebMarkupMin.AspNetCore3
#elif ASPNETCORE6
namespace WebMarkupMin.AspNetCore6
#elif ASPNETCORE_LATEST
namespace WebMarkupMin.AspNetCoreLatest
#else
#error No implementation for this target
#endif
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
		/// <returns>Result of check (<c>true</c> - content is encoded; <c>false</c> - content is not encoded)</returns>
		public static bool IsEncodedContent(this IHeaderDictionary headers)
		{
			return headers.ContainsKey(HeaderNames.ContentEncoding)
				&& !headers[HeaderNames.ContentEncoding].ToString().Equals("identity", StringComparison.OrdinalIgnoreCase);
		}
	}
}