namespace WebMarkupMin.Core
{
	/// <summary>
	/// XML minification output writer
	/// </summary>
	internal sealed class XmlMinificationOutputWriter : MarkupMinificationOutputWriterBase
	{
		/// <summary>
		/// Removes a last whitespace items from the output buffer
		/// </summary>
		public void RemoveLastWhitespaceItems()
		{
			int itemCount = _buffer.Count;
			if (itemCount == 0)
			{
				return;
			}

			int whitespaceItemCount = 0;

			for (int itemIndex = itemCount - 1; itemIndex >= 0; itemIndex--)
			{
				string item = _buffer[itemIndex];

				if (string.IsNullOrWhiteSpace(item))
				{
					whitespaceItemCount++;
				}
				else
				{
					break;
				}
			}

			if (whitespaceItemCount == 1)
			{
				_buffer.RemoveAt(itemCount - 1);
			}
			else if (whitespaceItemCount > 1)
			{
				_buffer.RemoveRange(itemCount - whitespaceItemCount, whitespaceItemCount);
			}
		}

		/// <summary>
		/// Transform a last start tag to empty tag in the output buffer
		/// </summary>
		/// <returns>Result of transforming (true - has transformed; false - has not transformed)</returns>
		public bool TransformLastStartTagToEmptyTag(bool renderEmptyTagsWithSpace)
		{
			int itemCount = _buffer.Count;
			if (itemCount == 0)
			{
				return false;
			}

			bool isTransformed = false;
			int lastItemIndex = itemCount - 1;
			int tagEndPartItemIndex = _buffer.LastIndexOf(">");

			if (tagEndPartItemIndex == lastItemIndex)
			{
				_buffer[tagEndPartItemIndex] = renderEmptyTagsWithSpace ? " />" : "/>";
				isTransformed = true;
			}

			return isTransformed;
		}
	}
}