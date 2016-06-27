namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML attribute types
	/// </summary>
	internal enum HtmlAttributeType : byte
	{
		Unknown = 0,
		Boolean,
		Numeric,
		Uri,
		Event,
		ClassName,
		Style,
		Text,
		Xml
	}
}