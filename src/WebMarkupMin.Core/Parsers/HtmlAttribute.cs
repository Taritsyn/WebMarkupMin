using System.IO;
using System.Net;

using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML attribute
	/// </summary>
	internal sealed class HtmlAttribute
	{
		/// <summary>
		/// Array of encoding chars with double quote
		/// </summary>
		private static readonly char[] _encodingCharsWithDoubleQuote = { '"', '&', '<' };

		/// <summary>
		/// Array of encoding chars with single quote
		/// </summary>
		private static readonly char[] _encodingCharsWithSingleQuote = { '\'', '&', '<' };

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
		/// Gets a type
		/// </summary>
		public HtmlAttributeType Type
		{
			get;
			private set;
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
		/// <param name="type">Type</param>
		public HtmlAttribute(string name, string nameInLowercase, string value, HtmlAttributeType type)
			: this(name, nameInLowercase, value, type, SourceCodeNodeCoordinates.Empty, SourceCodeNodeCoordinates.Empty)
		{ }

		/// <summary>
		/// Constructs instance of HTML attribute
		/// </summary>
		/// <param name="name">Name</param>
		/// <param name="nameInLowercase">Name in lowercase</param>
		/// <param name="value">Value</param>
		/// <param name="type">Type</param>
		/// <param name="nameCoordinates">Coordinates of name</param>
		/// <param name="valueCoordinates">Coordinates of value</param>
		public HtmlAttribute(string name, string nameInLowercase, string value, HtmlAttributeType type,
			SourceCodeNodeCoordinates nameCoordinates, SourceCodeNodeCoordinates valueCoordinates)
		{
			Name = name;
			NameInLowercase = nameInLowercase;
			Value = value;
			Type = type;
			NameCoordinates = nameCoordinates;
			ValueCoordinates = valueCoordinates;
		}


		/// <summary>
		/// Converts a string that has been HTML-encoded into a decoded string
		/// </summary>
		/// <param name="value">The string to decode</param>
		/// <returns>The decoded string</returns>
		public static string HtmlAttributeDecode(string value)
		{
			return WebUtility.HtmlDecode(value);
		}

		/// <summary>
		/// Converts a string to an HTML-encoded string
		/// </summary>
		/// <param name="value">The string to encode</param>
		/// <param name="attributeQuotesType">HTML attribute quotes type</param>
		/// <returns>The encoded string</returns>
		public static string HtmlAttributeEncode(string value,
			HtmlAttributeQuotesType attributeQuotesType = HtmlAttributeQuotesType.Double)
		{
			char quoteCharValue = '"';
			string quoteCharReference = "&#34;"; // use `&#34;` instead of `&quot;`, because it is shorter

			if (attributeQuotesType == HtmlAttributeQuotesType.Single)
			{
				quoteCharValue = '\'';
				quoteCharReference = "&#39;";
			}

			if (string.IsNullOrWhiteSpace(value) || !ContainsHtmlAttributeEncodingChars(value, quoteCharValue))
			{
				return value;
			}

			string result;

			using (var writer = new StringWriter())
			{
				int charCount = value.Length;

				for (int charIndex = 0; charIndex < charCount; charIndex++)
				{
					char charValue = value[charIndex];

					switch (charValue)
					{
						case '"':
						case '\'':
							if (charValue == quoteCharValue)
							{
								writer.Write(quoteCharReference);
							}
							else
							{
								writer.Write(charValue);
							}

							break;
						case '&':
							writer.Write("&amp;");
							break;
						case '<':
							writer.Write("&lt;");
							break;
						default:
							writer.Write(charValue);
							break;
					}
				}

				writer.Flush();

				result = writer.ToString();
			}

			return result;
		}

		private static bool ContainsHtmlAttributeEncodingChars(string value, char quoteCharValue)
		{
			char[] _encodingChars = quoteCharValue == '"' ?
				_encodingCharsWithDoubleQuote : _encodingCharsWithSingleQuote;
			bool result = value.IndexOfAny(_encodingChars) != -1;

			return result;
		}
	}
}