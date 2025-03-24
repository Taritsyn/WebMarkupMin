namespace WebMarkupMin.Core.Helpers
{
	internal static class MarkupAttributeValueHelpers
	{
		/// <summary>
		/// Gets a HTML attribute quote character by the quote style enum
		/// </summary>
		/// <param name="quoteStyle">Style of the attribute quotes</param>
		/// <param name="attributeValue">Attribute value</param>
		/// <param name="originalQuoteChar">Original attribute quote character</param>
		/// <param name="defaultQuoteChar">Default attribute quote character</param>
		/// <returns>Attribute quote character</returns>
		public static char GetAttributeQuoteCharByStyleEnum(MarkupAttributeQuotesStyle quoteStyle, string attributeValue,
			char originalQuoteChar, char defaultQuoteChar)
		{
			char quoteChar;

			switch (quoteStyle)
			{
				case MarkupAttributeQuotesStyle.Auto:
					quoteChar = GetDefaultAttributeQuoteChar(originalQuoteChar, defaultQuoteChar);
					break;
				case MarkupAttributeQuotesStyle.Optimal:
					bool containsDoubleQuote = attributeValue.IndexOf('"') != -1;
					bool containsSingleQuote = attributeValue.IndexOf('\'') != -1;

					if (containsDoubleQuote || containsSingleQuote)
					{
						if (containsDoubleQuote && containsSingleQuote)
						{
							quoteChar = GetDefaultAttributeQuoteChar(originalQuoteChar, defaultQuoteChar);
						}
						else if (containsDoubleQuote)
						{
							quoteChar = '\'';
						}
						else
						{
							quoteChar = '"';
						}
					}
					else
					{
						quoteChar = GetDefaultAttributeQuoteChar(originalQuoteChar, defaultQuoteChar);
					}

					break;
				case MarkupAttributeQuotesStyle.Single:
					quoteChar = '\'';
					break;
				case MarkupAttributeQuotesStyle.Double:
					quoteChar = '"';
					break;
				default:
					quoteChar = '\0';
					break;
			}

			return quoteChar;
		}

		private static char GetDefaultAttributeQuoteChar(char originalQuoteChar, char defaultQuoteChar)
		{
			char quoteChar = originalQuoteChar;
			if (originalQuoteChar == '\0')
			{
				quoteChar = defaultQuoteChar != '\0' ? defaultQuoteChar : '"';
			}

			return quoteChar;
		}

		public static string ConvertAttributeQuoteCharToString(char quoteChar)
		{
			string quoteString;

			switch (quoteChar)
			{
				case '"':
					quoteString = "\"";
					break;
				case '\'':
					quoteString = "'";
					break;
				default:
					quoteString = string.Empty;
					break;
			}

			return quoteString;
		}
	}
}