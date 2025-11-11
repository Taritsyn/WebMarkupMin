// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NETCOREAPP3_1_OR_GREATER
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace WebMarkupMin.Sample.AspNetCore.Infrastructure.Internal
{
	internal static class ResponseContentTypeHelper
	{
		/// <summary>
		/// Gets the content type and encoding that need to be used for the response.
		/// The priority for selecting the content type is:
		/// 1. ContentType property set on the action result
		/// 2. <see cref="HttpResponse.ContentType"/> property set on <see cref="HttpResponse"/>
		/// 3. Default content type set on the action result
		/// </summary>
		/// <remarks>
		/// The user supplied content type is not modified and is used as is. For example, if user
		/// sets the content type to be "text/plain" without any encoding, then the default content type's
		/// encoding is used to write the response and the ContentType header is set to be "text/plain" without any
		/// "charset" information.
		/// </remarks>
		/// <param name="actionResultContentType">ContentType set on the action result</param>
		/// <param name="httpResponseContentType"><see cref="HttpResponse.ContentType"/> property set
		/// on <see cref="HttpResponse"/></param>
		/// <param name="defaultContentType">The default content type of the action result</param>
		/// <param name="resolvedContentType">The content type to be used for the response content type header</param>
		/// <param name="resolvedContentTypeEncoding">Encoding to be used for writing the response</param>
		public static void ResolveContentTypeAndEncoding(
			string actionResultContentType,
			string httpResponseContentType,
			string defaultContentType,
			out string resolvedContentType,
			out Encoding resolvedContentTypeEncoding)
		{
			Encoding defaultContentTypeEncoding = MediaType.GetEncoding(defaultContentType);

			// 1. User sets the ContentType property on the action result
			if (actionResultContentType is not null)
			{
				resolvedContentType = actionResultContentType;
				Encoding actionResultEncoding = MediaType.GetEncoding(actionResultContentType);
				resolvedContentTypeEncoding = actionResultEncoding ?? defaultContentTypeEncoding;
				return;
			}

			// 2. User sets the ContentType property on the http response directly
			if (!string.IsNullOrEmpty(httpResponseContentType))
			{
				Encoding mediaTypeEncoding = MediaType.GetEncoding(httpResponseContentType);
				if (mediaTypeEncoding is not null)
				{
					resolvedContentType = httpResponseContentType;
					resolvedContentTypeEncoding = mediaTypeEncoding;
				}
				else
				{
					resolvedContentType = httpResponseContentType;
					resolvedContentTypeEncoding = defaultContentTypeEncoding;
				}

				return;
			}

			// 3. Fall-back to the default content type
			resolvedContentType = defaultContentType;
			resolvedContentTypeEncoding = defaultContentTypeEncoding;
		}
	}
}
#endif