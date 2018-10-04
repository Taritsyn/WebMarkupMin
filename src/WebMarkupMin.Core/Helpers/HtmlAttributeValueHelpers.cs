using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Helpers
{
	internal static class HtmlAttributeValueHelpers
	{
		public static bool IsHtml4AttributeValueNotRequireQuotes(string value)
		{
			bool result = true;
			int charCount = value.Length;

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

		public static bool IsHtml5AttributeValueNotRequireQuotes(string value)
		{
			bool result = true;
			int charCount = value.Length;

			for (int charIndex = 0; charIndex < charCount; charIndex++)
			{
				char charValue = value[charIndex];

				if (char.IsWhiteSpace(charValue)
					|| charValue == '='
					|| charValue == '"'
					|| charValue == '\''
					|| charValue == '`'
					|| charValue == '<'
					|| charValue == '>')
				{
					result = false;
					break;
				}
			}

			return result;
		}
	}
}