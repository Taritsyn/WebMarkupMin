namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML system identifier for document type declaration
	/// </summary>
	internal sealed class HtmlSystemId : HtmlExternalIdBase
	{
		/// <summary>
		/// Gets a URL of the document type description
		/// </summary>
		public string Url
		{
			get;
			private set;
		}


		/// <summary>
		/// Constructs an instance of HTML system identifier
		/// </summary>
		/// <param name="url">URL of the document type description</param>
		public HtmlSystemId(string url)
			: this(url, '"')
		{ }

		/// <summary>
		/// Constructs an instance of HTML system identifier
		/// </summary>
		/// <param name="url">URL of the document type description</param>
		/// <param name="quoteChar">Quote character used for identifier values</param>
		public HtmlSystemId(string url, char quoteChar)
			: base(quoteChar)
		{
			Url = url;
		}
	}
}