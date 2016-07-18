using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet4.Common;

namespace WebMarkupMin.AspNet4.Mvc
{
	/// <summary>
	/// Represents an attribute, that applies XML minification to the action result
	/// </summary>
	public sealed class MinifyXmlAttribute : MinifyMarkupAttribute
	{
		/// <summary>
		/// Constructs a instance of markup minification attribute
		/// </summary>
		public MinifyXmlAttribute()
			: base(WebMarkupMinConfiguration.Instance, null)
		{ }


		/// <summary>
		/// Gets a instance of default XML minification manager
		/// </summary>
		/// <returns>Instance of default XML minification manager</returns>
		protected override IMarkupMinificationManager GetDefaultMinificationManager()
		{
			return XmlMinificationManager.Current;
		}
	}
}