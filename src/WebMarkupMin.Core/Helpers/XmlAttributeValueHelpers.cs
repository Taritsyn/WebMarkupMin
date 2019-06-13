using System.IO;
using System.Net;
using System.Text;

using AdvancedStringBuilder;

namespace WebMarkupMin.Core.Helpers
{
	internal static class XmlAttributeValueHelpers
	{
		/// <summary>
		/// Array of encoding chars
		/// </summary>
		private static readonly char[] _encodingChars = { '"', '&', '<', '>' };

		/// <summary>
		/// Converts a string that has been XML-encoded into a decoded string
		/// </summary>
		/// <param name="value">The string to decode</param>
		/// <returns>The decoded string</returns>
		public static string Decode(string value)
		{
			if (value.IndexOf('&') == -1 || value.IndexOf(';') == -1)
			{
				return value;
			}

			return WebUtility.HtmlDecode(value);
		}

		/// <summary>
		/// Converts a string to an XML-encoded string
		/// </summary>
		/// <param name="value">The string to encode</param>
		/// <returns>The encoded string</returns>
		public static string Encode(string value)
		{
			if (string.IsNullOrWhiteSpace(value) || !ContainsXmlAttributeEncodingChars(value))
			{
				return value;
			}

			string result;
			var stringBuilderPool = StringBuilderPool.Shared;
			StringBuilder sb = stringBuilderPool.Rent();

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

			stringBuilderPool.Return(sb);

			return result;
		}

		private static bool ContainsXmlAttributeEncodingChars(string value)
		{
			bool result = value.IndexOfAny(_encodingChars) != -1;

			return result;
		}
	}
}