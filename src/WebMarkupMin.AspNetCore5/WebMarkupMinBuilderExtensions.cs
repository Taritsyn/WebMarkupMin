using System;

using Microsoft.AspNetCore.Builder;

using OriginalBuilderExtensions = WebMarkupMin.AspNetCore3.WebMarkupMinBuilderExtensions;

namespace WebMarkupMin.AspNetCore5
{
	/// <summary>
	/// Extension methods for <see cref="IApplicationBuilder"/> to add
	/// WebMarkupMin optimization features to the request execution pipeline
	/// </summary>
	public static class WebMarkupMinBuilderExtensions
	{
		/// <summary>
		/// Adds a WebMarkupMin optimization features to the <see cref="IApplicationBuilder"/> request execution pipeline
		/// </summary>
		/// <param name="app">The <see cref="IApplicationBuilder"/></param>
		/// <returns>The <paramref name="app"/></returns>
		[Obsolete("Use a WebMarkupMin.AspNetCore3 package")]
		public static IApplicationBuilder UseWebMarkupMin(this IApplicationBuilder app)
		{
			return OriginalBuilderExtensions.UseWebMarkupMin(app);
		}
	}
}