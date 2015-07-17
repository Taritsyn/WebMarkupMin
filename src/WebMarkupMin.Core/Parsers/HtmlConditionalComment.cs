namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML conditional comment
	/// </summary>
	internal sealed class HtmlConditionalComment
	{
		/// <summary>
		/// Conditional expression
		/// </summary>
		public string Expression
		{
			get;
			private set;
		}

		/// <summary>
		/// Conditional comment type
		/// </summary>
		public HtmlConditionalCommentType Type
		{
			get;
			private set;
		}


		/// <summary>
		/// Constructs instance of HTML tag
		/// </summary>
		/// <param name="expression">Conditional expression</param>
		/// <param name="type">Conditional comment type</param>
		public HtmlConditionalComment(string expression, HtmlConditionalCommentType type)
		{
			Expression = expression;
			Type = type;
		}
	}
}