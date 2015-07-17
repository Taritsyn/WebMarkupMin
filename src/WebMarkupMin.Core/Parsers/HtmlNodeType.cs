namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML node types
	/// </summary>
	internal enum HtmlNodeType
	{
		Unknown = 0,
		XmlDeclaration,
		Doctype,
		Comment,
		IfConditionalComment,
		EndIfConditionalComment,
		StartTag,
		EndTag,
		Text,
		TemplateTag
	}
}