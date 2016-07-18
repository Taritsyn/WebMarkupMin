using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet4.Common;

namespace WebMarkupMin.AspNet4.Mvc
{
	/// <summary>
	/// Represents an attribute, that applies XHTML minification to the action result
	/// </summary>
	public sealed class MinifyXhtmlAttribute : MinifyMarkupAttribute
	{
		/// <summary>
		/// Constructs a instance of XHTML minification attribute
		/// </summary>
		public MinifyXhtmlAttribute()
			: base(WebMarkupMinConfiguration.Instance, null)
		{ }


		/// <summary>
		/// Gets a instance of default XHTML minification manager
		/// </summary>
		/// <returns>Instance of default XHTML minification manager</returns>
		protected override IMarkupMinificationManager GetDefaultMinificationManager()
		{
			return XhtmlMinificationManager.Current;
		}
	}
}