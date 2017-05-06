namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML node types
	/// </summary>
	internal enum HtmlNodeType : byte
	{
		Unknown = 0,
		XmlDeclaration,
		Doctype,
		Comment,
		IfConditionalComment,
		EndIfConditionalComment,
		CdataSection,
		StartTag,
		EndTag,
		Text,
		EmbeddedCode,
		TemplateTag,
		IgnoredFragment
	}
}