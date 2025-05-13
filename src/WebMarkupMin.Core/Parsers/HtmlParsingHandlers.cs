namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML parsing handlers
	/// </summary>
	internal sealed class HtmlParsingHandlers : MarkupParsingHandlersBase
	{
		/// <summary>
		/// XML declaration
		/// </summary>
		public XmlDeclarationDelegate XmlDeclaration
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
		/// If conditional comments handler
		/// </summary>
		public IfConditionalCommentDelegate IfConditionalComment
		{
			get;
			set;
		}

		/// <summary>
		/// End If conditional comments handler
		/// </summary>
		public EndIfConditionalCommentDelegate EndIfConditionalComment
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
		/// Embedded code handler
		/// </summary>
		public EmbeddedCodeDelegate EmbeddedCode
		{
			get;
			set;
		}

		/// <summary>
		/// Template tags handler
		/// </summary>
		public TemplateTagDelegate TemplateTag
		{
			get;
			set;
		}


		/// <summary>
		/// XML declaration delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="xmlDeclaration">XML declaration</param>
		public delegate void XmlDeclarationDelegate(MarkupParsingContext context, string xmlDeclaration);

		/// <summary>
		/// Document type declaration delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="doctype">Document type declaration</param>
		public delegate void DoctypeDelegate(MarkupParsingContext context, HtmlDoctype doctype);

		/// <summary>
		/// If conditional comments delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="expression">Conditional expression</param>
		/// <param name="type">Conditional comment type</param>
		public delegate void IfConditionalCommentDelegate(MarkupParsingContext context,
			string expression, HtmlConditionalCommentType type);

		/// <summary>
		/// End If conditional comments delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="type">Conditional comment type</param>
		public delegate void EndIfConditionalCommentDelegate(MarkupParsingContext context,
			HtmlConditionalCommentType type);

		/// <summary>
		/// Start tags delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="tag">HTML tag</param>
		public delegate void StartTagDelegate(MarkupParsingContext context, HtmlTag tag);

		/// <summary>
		/// End tags delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="tag">HTML tag</param>
		public delegate void EndTagDelegate(MarkupParsingContext context, HtmlTag tag);

		/// <summary>
		/// Embedded code delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="code">Code</param>
		public delegate void EmbeddedCodeDelegate(MarkupParsingContext context, string code);

		/// <summary>
		/// Template tags delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="expression">Expression</param>
		/// <param name="startDelimiter">Start delimiter</param>
		/// <param name="endDelimiter">End delimiter</param>
		public delegate void TemplateTagDelegate(MarkupParsingContext context, string expression,
			string startDelimiter, string endDelimiter);
	}
}