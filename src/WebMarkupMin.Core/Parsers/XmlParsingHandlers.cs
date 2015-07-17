using System.Collections.Generic;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// XML parsing handlers
	/// </summary>
	internal sealed class XmlParsingHandlers
	{
		/// <summary>
		/// XML declaration handler
		/// </summary>
		public XmlDeclarationDelegate XmlDeclaration
		{
			get;
			set;
		}

		/// <summary>
		/// Processing instruction handler
		/// </summary>
		public ProcessingInstructionDelegate ProcessingInstruction
		{
			get;
			set;
		}

		/// <summary>
		/// Document type declaration handler
		/// </summary>
		public DoctypeDelegate Doctype
		{
			get;
			set;
		}

		/// <summary>
		/// Comments handler
		/// </summary>
		public CommentDelegate Comment
		{
			get;
			set;
		}

		/// <summary>
		/// CDATA sections handler
		/// </summary>
		public CdataSectionDelegate CdataSection
		{
			get;
			set;
		}

		/// <summary>
		/// Start tags handler
		/// </summary>
		public StartTagDelegate StartTag
		{
			get;
			set;
		}

		/// <summary>
		/// End tags handler
		/// </summary>
		public EndTagDelegate EndTag
		{
			get;
			set;
		}

		/// <summary>
		/// Empty tags delegate
		/// </summary>
		public EmptyTagDelegate EmptyTag
		{
			get;
			set;
		}

		/// <summary>
		/// Text handler
		/// </summary>
		public TextDelegate Text
		{
			get;
			set;
		}


		/// <summary>
		/// XML declaration delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="attributes">List of attributes</param>
		public delegate void XmlDeclarationDelegate(MarkupParsingContext context, IList<XmlAttribute> attributes);

		/// <summary>
		/// Processing instruction delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="instructionName">Instruction name</param>
		/// <param name="attributes">List of attributes</param>
		public delegate void ProcessingInstructionDelegate(MarkupParsingContext context, string instructionName,
			IList<XmlAttribute> attributes);

		/// <summary>
		/// Document type declaration delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="doctype">Document type declaration</param>
		public delegate void DoctypeDelegate(MarkupParsingContext context, string doctype);

		/// <summary>
		/// Comments delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="comment">Comment text</param>
		public delegate void CommentDelegate(MarkupParsingContext context, string comment);

		/// <summary>
		/// CDATA sections delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="cdataText">CDATA text</param>
		public delegate void CdataSectionDelegate(MarkupParsingContext context, string cdataText);

		/// <summary>
		/// Start tags delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="tagName">Tag name</param>
		/// <param name="attributes">List of attributes</param>
		public delegate void StartTagDelegate(MarkupParsingContext context, string tagName,
			IList<XmlAttribute> attributes);

		/// <summary>
		/// End tags delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="tagName">Tag name</param>
		public delegate void EndTagDelegate(MarkupParsingContext context, string tagName);

		/// <summary>
		/// Empty tags delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="tagName">Tag name</param>
		/// <param name="attributes">List of attributes</param>
		public delegate void EmptyTagDelegate(MarkupParsingContext context, string tagName,
			IList<XmlAttribute> attributes);

		/// <summary>
		/// Text delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="text">Text</param>
		public delegate void TextDelegate(MarkupParsingContext context, string text);
	}
}