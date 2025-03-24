using System.IO;
using System.Net;
using System.Text;

using AdvancedStringBuilder;

using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Helpers
{
	internal static class HtmlAttributeValueHelpers
	{
		/// <summary>
		/// Array of encoding chars with double quote
		/// </summary>
		private static readonly char[] _encodingCharsWithDoubleQuote = { '"', '&', '<' };

		/// <summary>
		/// Array of encoding chars with single quote
		/// </summary>
		private static readonly char[] _encodingCharsWithSingleQuote = { '\'', '&', '<' };


		public static bool IsNotRequireQuotesInHtml4(string value)
		{
			int charCount = value.Length;
			if (charCount == 0 || value[charCount - 1] == '/')
			{
				return false;
			}

			bool result = true;

			for (int charIndex = 0; charIndex < charCount; charIndex++)
			{
				char charValue = value[charIndex];

				if (charValue.IsAlphaNumeric()
					|| charValue == '-'
					|| charValue == '_'
					|| charValue == ':'
					|| charValue == '.')
				{
					continue;
				}
				else
				{
					result = false;
					break;
				}
			}

			return result;
		}

		public static bool IsNotRequireQuotesInHtml5(string value)
		{
			int charCount = value.Length;
			if (charCount == 0 || value[charCount - 1] == '/')
			{
				return false;
			}

			bool result = true;

			for (int charIndex = 0; charIndex < charCount; charIndex++)
			{
				char charValue = value[charIndex];

				if (char.IsWhiteSpace(charValue)
					|| charValue == '"'
					|| charValue == '\''
					|| charValue == '`'
					|| charValue == '='
					|| charValue == '<'
					|| charValue == '>')
				{
					result = false;
					break;
				}
			}

			return result;
		}

		/// <summary>
		/// Converts a string that has been HTML-encoded into a decoded string
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
		/// Converts a string to an HTML-encoded string
		/// </summary>
		/// <param name="value">The string to encode</param>
		/// <param name="quoteChar">Quote character</param>
		/// <returns>The encoded string</returns>
		public static string Encode(string value, char quoteChar)
		{
			if (string.IsNullOrWhiteSpace(value) || !ContainsEncodingChars(value, quoteChar))
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
							if (quoteChar == '"' || quoteChar == '\0')
							{
								writer.Write("&#34;"); // use `&#34;` instead of `&quot;`, because it is shorter
							}
							else
							{
								writer.Write(charValue);
							}

							break;
						case '\'':
							if (quoteChar == '\'' || quoteChar == '\0')
							{
								writer.Write("&#39;"); // use `&#39;` instead of `&apos;`, because it is shorter
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

			stringBuilderPool.Return(sb);

			return result;
		}

		private static bool ContainsEncodingChars(string value, char quoteChar)
		{
			if (quoteChar == '\0')
			{
				return value.IndexOf('"') != -1 || value.IndexOfAny(_encodingCharsWithSingleQuote) != -1;
			}

			char[] encodingChars = quoteChar == '"' ?
				_encodingCharsWithDoubleQuote : _encodingCharsWithSingleQuote;
			bool result = value.IndexOfAny(encodingChars) != -1;

			return result;
		}
	}
}