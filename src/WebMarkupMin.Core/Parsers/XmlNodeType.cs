namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// XML node types
	/// </summary>
	internal enum XmlNodeType : byte
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
		Text,
		IgnoredFragment
	}
}