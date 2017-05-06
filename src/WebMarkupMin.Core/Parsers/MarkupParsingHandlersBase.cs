namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// Markup parsing handlers
	/// </summary>
	internal abstract class MarkupParsingHandlersBase
	{
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
		/// Text handler
		/// </summary>
		public TextDelegate Text
		{
			get;
			set;
		}

		/// <summary>
		/// Ignored fragments handler
		/// </summary>
		public IgnoredFragmentDelegate IgnoredFragment
		{
			get;
			set;
		}


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
		/// Text delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="text">Text</param>
		public delegate void TextDelegate(MarkupParsingContext context, string text);

		/// <summary>
		/// Ignored fragments delegate
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="fragment">Ignored fragment</param>
		public delegate void IgnoredFragmentDelegate(MarkupParsingContext context, string fragment);
	}
}