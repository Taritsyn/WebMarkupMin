using WebMarkupMin.AspNet4.Common;

namespace WebMarkupMin.AspNet4.Mvc
{
	/// <summary>
	/// Represents an attribute that is used to XML minification of action result
	/// </summary>
	public sealed class MinifyXmlAttribute : MinifyMarkupAttribute
	{
		/// <summary>
		/// Constructs a instance of markup minification attribute
		/// </summary>
		public MinifyXmlAttribute()
			: base(WebMarkupMinConfiguration.Instance, XmlMinificationManager.Current)
		{ }
	}
}