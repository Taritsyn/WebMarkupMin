namespace WebMarkupMin.AspNet.Common.UrlMatchers
{
	/// <summary>
	/// Defines a interface of URL matcher
	/// </summary>
	public interface IUrlMatcher
	{
		/// <summary>
		/// Indicates whether the matching rule finds a match in the specified URL
		/// </summary>
		/// <param name="url">URL</param>
		/// <returns><c>true</c> if the matching rule finds a match; otherwise, <c>false</c>.</returns>
		bool IsMatch(string url);
	}
}