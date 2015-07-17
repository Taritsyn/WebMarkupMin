namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// XML node types
	/// </summary>
	internal enum XmlNodeType
	{
		Unknown = 0,
		XmlDeclaration,
		ProcessingInstruction,
		Doctype,
		Comment,
		CdataSection,
		StartTag,
		EndTag,
		EmptyTag,
		Text
	}
}