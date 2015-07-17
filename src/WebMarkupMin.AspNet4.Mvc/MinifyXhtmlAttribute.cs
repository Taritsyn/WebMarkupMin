using WebMarkupMin.AspNet4.Common;

namespace WebMarkupMin.AspNet4.Mvc
{
	/// <summary>
	/// Represents an attribute that is used to XHTML minification of action result
	/// </summary>
	public sealed class MinifyXhtmlAttribute : MinifyMarkupAttribute
	{
		/// <summary>
		/// Constructs a instance of XHTML minification attribute
		/// </summary>
		public MinifyXhtmlAttribute()
			: base(WebMarkupMinConfiguration.Instance, XhtmlMinificationManager.Current)
		{ }
	}
}