using System.IO;
using System.Net;
using System.Text;

using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// XML attribute
	/// </summary>
	internal sealed class XmlAttribute
	{
		/// <summary>
		/// Array of encoding chars
		/// </summary>
		private static readonly char[] _encodingChars = { '"', '&', '<', '>' };

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


		/// <summary>
		/// Converts a string that has been XML-encoded into a decoded string
		/// </summary>
		/// <param name="value">The string to decode</param>
		/// <returns>The decoded string</returns>
		public static string XmlAttributeDecode(string value)
		{
			return WebUtility.HtmlDecode(value);
		}

		/// <summary>
		/// Converts a string to an XML-encoded string
		/// </summary>
		/// <param name="value">The string to encode</param>
		/// <returns>The encoded string</returns>
		public static string XmlAttributeEncode(string value)
		{
			if (string.IsNullOrWhiteSpace(value) || !ContainsXmlAttributeEncodingChars(value))
			{
				return value;
			}

			string result;
			StringBuilder sb = StringBuilderPool.GetBuilder();

			using (var writer = new StringWriter(sb))
			{
				int charCount = value.Length;

				for (int charIndex = 0; charIndex < charCount; charIndex++)
				{
					char charValue = value[charIndex];

					switch (charValue)
					{
						case '"':
							writer.Write("&quot;");
							break;
						case '&':
							writer.Write("&amp;");
							break;
						case '<':
							writer.Write("&lt;");
							break;
						case '>':
							writer.Write("&gt;");
							break;
						default:
							writer.Write(charValue);
							break;
					}
				}

				writer.Flush();

				result = writer.ToString();
			}

			StringBuilderPool.ReleaseBuilder(sb);

			return result;
		}

		private static bool ContainsXmlAttributeEncodingChars(string value)
		{
			bool result = value.IndexOfAny(_encodingChars) != -1;

			return result;
		}
	}
}