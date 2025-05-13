namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML external identifier for document type declaration
	/// </summary>
	internal abstract class HtmlExternalIdBase
	{
		/// <summary>
		/// Gets a quote character used for identifier values
		/// </summary>
		public char QuoteChar
		{
			get;
			private set;
		}


		/// <summary>
		/// Constructs an instance of HTML external identifier
		/// </summary>
		/// <param name="quoteChar">Quote character used for identifier values</param>
		protected HtmlExternalIdBase(char quoteChar)
		{
			QuoteChar = quoteChar;
		}
	}
}