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
		/// Constructs instance of XML attribute
		/// </summary>
		/// <param name="name">Name</param>
		/// <param name="value">Value</param>
		public XmlAttribute(string name, string value)
		{
			Name = name;
			Value = value;
		}
	}
}