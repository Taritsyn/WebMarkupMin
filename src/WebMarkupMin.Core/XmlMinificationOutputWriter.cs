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
			int bufferItemCount = _buffer.Count;
			if (bufferItemCount > 0)
			{
				for (int bufferItemIndex = bufferItemCount - 1; bufferItemIndex >= 0; bufferItemIndex--)
				{
					string bufferItem = _buffer[bufferItemIndex];

					if (string.IsNullOrWhiteSpace(bufferItem))
					{
						_buffer.RemoveAt(bufferItemIndex);
					}
					else
					{
						break;
					}
				}
			}
		}

		/// <summary>
		/// Transform a last start tag to empty tag in the output buffer
		/// </summary>
		/// <returns>Result of transforming (true - has transformed; false - has not transformed)</returns>
		public bool TransformLastStartTagToEmptyTag(bool renderEmptyTagsWithSpace)
		{
			int bufferItemCount = _buffer.Count;
			if (bufferItemCount == 0)
			{
				return false;
			}

			bool isTransformed = false;
			int lastBufferItemIndex = bufferItemCount - 1;
			int lastEndTagEndAngleBracketIndex = _buffer.LastIndexOf(">");

			if (lastEndTagEndAngleBracketIndex == lastBufferItemIndex)
			{
				_buffer[lastBufferItemIndex] = renderEmptyTagsWithSpace ? " />" : "/>";
				isTransformed = true;
			}

			return isTransformed;
		}
	}
}