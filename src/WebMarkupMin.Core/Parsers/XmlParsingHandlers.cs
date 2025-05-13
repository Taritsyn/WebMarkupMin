using System.Collections.Generic;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// XML parsing handlers
	/// </summary>
	internal sealed class XmlParsingHandlers : MarkupParsingHandlersBase
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
		/// XML declaration delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="attributes">List of attributes</param>
		public delegate void XmlDeclarationDelegate(MarkupParsingContext context, List<XmlAttribute> attributes);

		/// <summary>
		/// Processing instruction delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="instructionName">Instruction name</param>
		/// <param name="attributes">List of attributes</param>
		public delegate void ProcessingInstructionDelegate(MarkupParsingContext context, string instructionName,
			List<XmlAttribute> attributes);

		/// <summary>
		/// Document type declaration delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="doctype">Document type declaration</param>
		public delegate void DoctypeDelegate(MarkupParsingContext context, string doctype);

		/// <summary>
		/// Start tags delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="tagName">Tag name</param>
		/// <param name="attributes">List of attributes</param>
		public delegate void StartTagDelegate(MarkupParsingContext context, string tagName,
			List<XmlAttribute> attributes);

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
			List<XmlAttribute> attributes);
	}
}