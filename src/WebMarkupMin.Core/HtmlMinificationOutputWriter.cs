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
						_buffer[bufferItemIndex] = bufferItem.TrimEnd();
						break;
					}
				}
			}
		}

		/// <summary>
		/// Removes a last start tag from the output buffer
		/// </summary>
		/// <param name="tagName">Tag name</param>
		/// <returns>Result of removing (true - has removed; false - has not removed)</returns>
		public bool RemoveLastStartTag(string tagName)
		{
			int bufferItemCount = _buffer.Count;
			if (bufferItemCount == 0)
			{
				return false;
			}

			bool isEndTagRemoved = false;
			int lastStartTagBeginAngleBracketIndex = _buffer.LastIndexOf("<");

			if (lastStartTagBeginAngleBracketIndex != -1)
			{
				string lastTagName = _buffer[lastStartTagBeginAngleBracketIndex + 1];
				if (lastTagName.IgnoreCaseEquals(tagName))
				{
					int lastStartTagEndAngleBracketIndex = _buffer.IndexOf(">", lastStartTagBeginAngleBracketIndex);
					if (lastStartTagEndAngleBracketIndex != -1)
					{
						int lastBufferItemIndex = bufferItemCount - 1;
						bool noMoreContent = true;
						if (lastStartTagEndAngleBracketIndex != lastBufferItemIndex)
						{
							for (int bufferItemIndex = lastStartTagEndAngleBracketIndex + 1;
								 bufferItemIndex < bufferItemCount;
								 bufferItemIndex++)
							{
								if (!string.IsNullOrWhiteSpace(_buffer[bufferItemIndex]))
								{
									noMoreContent = false;
									break;
								}
							}
						}

						if (noMoreContent)
						{
							_buffer.RemoveRange(lastStartTagBeginAngleBracketIndex,
								bufferItemCount - lastStartTagBeginAngleBracketIndex);

							isEndTagRemoved = true;
						}
					}
				}
			}

			return isEndTagRemoved;
		}

		/// <summary>
		/// Removes a last end tag from the output buffer
		/// </summary>
		/// <param name="tagName">Tag name</param>
		public void RemoveLastEndTag(string tagName)
		{
			int bufferItemCount = _buffer.Count;
			if (bufferItemCount == 0)
			{
				return;
			}

			int lastEndTagBeginAngleBracketIndex = _buffer.LastIndexOf("</");

			if (lastEndTagBeginAngleBracketIndex != -1)
			{
				string lastEndTagName = _buffer[lastEndTagBeginAngleBracketIndex + 1];
				if (lastEndTagName.IgnoreCaseEquals(tagName))
				{
					int lastEndTagEndAngleBracketIndex = _buffer.IndexOf(">", lastEndTagBeginAngleBracketIndex);
					if (lastEndTagEndAngleBracketIndex != -1)
					{
						int lastBufferItemIndex = bufferItemCount - 1;
						bool noMoreContent = true;
						if (lastEndTagEndAngleBracketIndex != lastBufferItemIndex)
						{
							for (int bufferItemIndex = lastEndTagEndAngleBracketIndex + 1;
								bufferItemIndex < bufferItemCount;
								bufferItemIndex++)
							{
								if (!string.IsNullOrWhiteSpace(_buffer[bufferItemIndex]))
								{
									noMoreContent = false;
									break;
								}
							}
						}

						if (noMoreContent)
						{
							int endTagLength = lastEndTagEndAngleBracketIndex - lastEndTagBeginAngleBracketIndex + 1;
							_buffer.RemoveRange(lastEndTagBeginAngleBracketIndex, endTagLength);
						}
					}
				}
			}
		}
	}
}