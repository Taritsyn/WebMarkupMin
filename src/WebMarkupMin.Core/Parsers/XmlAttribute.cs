namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// XML attribute
	/// </summary>
	internal sealed class XmlAttribute
	{
		/// <summary>
		/// Name
		/// </summary>
		public string Name
		{
			get;
			private set;
		}

		/// <summary>
		/// Value
		/// </summary>
		public string Value
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a quote character used for attribute values
		/// </summary>
		public char QuoteChar
		{
			get;
			private set;
		}


		/// <summary>
		/// Constructs instance of XML attribute
		/// </summary>
		/// <param name="name">Name</param>
		/// <param name="value">Value</param>
		/// <param name="quoteChar">Quote character</param>
		public XmlAttribute(string name, string value, char quoteChar)
		{
			Name = name;
			Value = value;
			QuoteChar = quoteChar;
		}
	}
}