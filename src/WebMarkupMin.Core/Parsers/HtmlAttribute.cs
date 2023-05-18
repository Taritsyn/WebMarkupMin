using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML attribute
	/// </summary>
	internal sealed class HtmlAttribute
	{
		/// <summary>
		/// Value
		/// </summary>
		private string _value;

		/// <summary>
		/// Flag indicating whether the HTML attribute has a value
		/// </summary>
		private bool _hasValue;

		/// <summary>
		/// Gets a name
		/// </summary>
		public string Name
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a name in lowercase
		/// </summary>
		public string NameInLowercase
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets a value
		/// </summary>
		public string Value
		{
			get
			{
				return _value;
			}
			set
			{
				if (value != null)
				{
					_value = value;
					_hasValue = true;
				}
				else
				{
					_value = string.Empty;
					_hasValue = false;
				}
			}
		}

		/// <summary>
		/// Gets a flag indicating whether the HTML attribute has a value
		/// </summary>
		public bool HasValue
		{
			get
			{
				return _hasValue;
			}
		}

		/// <summary>
		/// Gets a quote character used for attribute values
		/// </summary>
		public char QuoteCharacter
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a type
		/// </summary>
		public HtmlAttributeType Type
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a coordinates of name
		/// </summary>
		public SourceCodeNodeCoordinates NameCoordinates
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a coordinates of value
		/// </summary>
		public SourceCodeNodeCoordinates ValueCoordinates
		{
			get;
			private set;
		}


		/// <summary>
		/// Constructs instance of HTML attribute
		/// </summary>
		/// <param name="name">Name</param>
		/// <param name="nameInLowercase">Name in lowercase</param>
		/// <param name="value">Value</param>
		/// <param name="quoteCharacter">Quote character</param>
		/// <param name="type">Type</param>
		public HtmlAttribute(string name, string nameInLowercase, string value, char quoteCharacter,
			HtmlAttributeType type)
			: this(name, nameInLowercase, value, quoteCharacter, type, SourceCodeNodeCoordinates.Empty,
				  SourceCodeNodeCoordinates.Empty)
		{ }

		/// <summary>
		/// Constructs instance of HTML attribute
		/// </summary>
		/// <param name="name">Name</param>
		/// <param name="nameInLowercase">Name in lowercase</param>
		/// <param name="value">Value</param>
		/// <param name="quoteCharacter">Quote character</param>
		/// <param name="type">Type</param>
		/// <param name="nameCoordinates">Coordinates of name</param>
		/// <param name="valueCoordinates">Coordinates of value</param>
		public HtmlAttribute(string name, string nameInLowercase, string value, char quoteCharacter,
			HtmlAttributeType type, SourceCodeNodeCoordinates nameCoordinates, SourceCodeNodeCoordinates valueCoordinates)
		{
			Name = name;
			NameInLowercase = nameInLowercase;
			Value = value;
			QuoteCharacter = quoteCharacter;
			Type = type;
			NameCoordinates = nameCoordinates;
			ValueCoordinates = valueCoordinates;
		}
	}
}