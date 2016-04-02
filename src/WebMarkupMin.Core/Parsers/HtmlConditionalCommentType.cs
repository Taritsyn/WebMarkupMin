namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML conditional comment types
	/// </summary>
	internal enum HtmlConditionalCommentType : byte
	{
		Hidden,
		Revealed,
		RevealedValidating,
		RevealedValidatingSimplified
	}
}