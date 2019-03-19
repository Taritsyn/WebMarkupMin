using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// HTML minification output writer
	/// </summary>
	internal sealed class HtmlMinificationOutputWriter : MarkupMinificationOutputWriterBase
	{
		/// <summary>
		/// Removes all trailing whitespace characters in the last item of the output buffer
		/// </summary>
		public void TrimEndLastItem()
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
					_buffer[itemIndex] = item.TrimEnd();
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
		/// Removes a last start tag from the output buffer
		/// </summary>
		/// <param name="tagName">Tag name</param>
		/// <returns>Result of removing (true - has removed; false - has not removed)</returns>
		public bool RemoveLastStartTag(string tagName)
		{
			int itemCount = _buffer.Count;
			if (itemCount == 0)
			{
				return false;
			}

			bool isStartTagRemoved = false;
			int tagBeginPartItemIndex = _buffer.LastIndexOf("<");

			if (tagBeginPartItemIndex != -1 && itemCount - tagBeginPartItemIndex >= 3)
			{
				string lastTagName = _buffer[tagBeginPartItemIndex + 1];
				if (lastTagName.IgnoreCaseEquals(tagName))
				{
					int tagEndPartItemIndex = _buffer.IndexOf(">", tagBeginPartItemIndex);
					if (tagEndPartItemIndex != -1)
					{
						int lastItemIndex = itemCount - 1;
						bool noMoreContent = true;

						if (tagEndPartItemIndex != lastItemIndex)
						{
							for (int itemIndex = tagEndPartItemIndex + 1; itemIndex < itemCount; itemIndex++)
							{
								if (!string.IsNullOrWhiteSpace(_buffer[itemIndex]))
								{
									noMoreContent = false;
									break;
								}
							}
						}

						if (noMoreContent)
						{
							int remainingItemCount = itemCount - tagBeginPartItemIndex;
							_buffer.RemoveRange(tagBeginPartItemIndex, remainingItemCount);

							isStartTagRemoved = true;
						}
					}
				}
			}

			return isStartTagRemoved;
		}

		/// <summary>
		/// Removes a last end tag from the output buffer
		/// </summary>
		/// <param name="tagName">Tag name</param>
		public void RemoveLastEndTag(string tagName)
		{
			int itemCount = _buffer.Count;
			if (itemCount == 0)
			{
				return;
			}

			int tagBeginPartItemIndex = _buffer.LastIndexOf("</");
			if (tagBeginPartItemIndex != -1 && itemCount - tagBeginPartItemIndex >= 3)
			{
				string lastTagName = _buffer[tagBeginPartItemIndex + 1];
				if (lastTagName.IgnoreCaseEquals(tagName))
				{
					int tagEndPartItemIndex = _buffer.IndexOf(">", tagBeginPartItemIndex);
					if (tagEndPartItemIndex != -1)
					{
						int lastItemIndex = itemCount - 1;
						bool noMoreContent = true;

						if (tagEndPartItemIndex != lastItemIndex)
						{
							for (int itemIndex = tagEndPartItemIndex + 1; itemIndex < itemCount; itemIndex++)
							{
								if (!string.IsNullOrWhiteSpace(_buffer[itemIndex]))
								{
									noMoreContent = false;
									break;
								}
							}
						}

						if (noMoreContent)
						{
							int tagItemCount = tagEndPartItemIndex - tagBeginPartItemIndex + 1;
							_buffer.RemoveRange(tagBeginPartItemIndex, tagItemCount);
						}
					}
				}
			}
		}
	}
}