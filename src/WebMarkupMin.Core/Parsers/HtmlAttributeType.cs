namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML attribute types
	/// </summary>
	internal enum HtmlAttributeType
	{
		Unknown = 0,
		Boolean,
		Numeric,
		Uri,
		Event,
		ClassName,
		Style,
		Text
	}
}