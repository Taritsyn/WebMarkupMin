// TODO: In the next major version, it is necessary to completely replace this enum by the `MarkupAttributeQuotesStyle`

namespace WebMarkupMin.Core
{
	/// <summary>
	/// Style of the XML attribute quotes
	/// </summary>
	public enum XmlAttributeQuotesStyle : byte
	{
		/// <summary>
		/// Auto-detect style for attribute quotes based on the source input
		/// </summary>
		Auto = MarkupAttributeQuotesStyle.Auto,

		/// <summary>
		/// Optimal style for attribute quotes based on the attribute value
		/// </summary>
		Optimal = MarkupAttributeQuotesStyle.Optimal,

		/// <summary>
		/// Single quotes
		/// </summary>
		Single = MarkupAttributeQuotesStyle.Single,

		/// <summary>
		/// Double quotes
		/// </summary>
		Double = MarkupAttributeQuotesStyle.Double
	}
}