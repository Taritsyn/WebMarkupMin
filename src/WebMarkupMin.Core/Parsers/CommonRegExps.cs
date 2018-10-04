namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// Common regular expressions
	/// </summary>
	internal static class CommonRegExps
	{
		public const string HtmlTagNamePattern = @"[a-zA-Z0-9][a-zA-Z0-9-_:]*";
		public const string HtmlAttributeNamePattern = @"[^\s""'<>/=]+";
	}
}