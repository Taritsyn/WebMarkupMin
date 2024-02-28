using System;
using System.Collections.Specialized;

namespace WebMarkupMin.AspNet4.Common.Helpers
{
	/// <summary>
	/// HTTP headers helpers
	/// </summary>
	public static class HttpHeadersHelpers
	{
		/// <summary>
		/// Checks whether the content is encoded
		/// </summary>
		/// <param name="headers">The HTTP headers</param>
		/// <returns>Result of check (<c>true</c> - content is encoded; <c>false</c> - content is not encoded)</returns>
		public static bool IsEncodedContent(NameValueCollection headers)
		{
			return headers["Content-Encoding"] != null
				&& !headers["Content-Encoding"].Equals("identity", StringComparison.OrdinalIgnoreCase);
		}
	}
}