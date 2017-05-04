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
	}
}