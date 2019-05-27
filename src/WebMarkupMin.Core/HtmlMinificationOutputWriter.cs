using System;
using System.Runtime.CompilerServices;

using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// HTML minification output writer
	/// </summary>
	internal sealed class HtmlMinificationOutputWriter : MarkupMinificationOutputWriterBase
	{
		/// <summary>
		/// Constructs an instance of the HTML minification output writer
		/// </summary>
		public HtmlMinificationOutputWriter()
			: base(DefaultBufferCapacity)
		{ }

		/// <summary>
		/// Constructs an instance of the HTML minification output writer
		/// </summary>
		/// <param name="initialBufferCapacity">Initial capacity of buffer</param>
		public HtmlMinificationOutputWriter(int initialBufferCapacity)
			: base(initialBufferCapacity)
		{ }


		/// <summary>
		/// Removes a range of items from the output buffer
		/// </summary>
		/// <param name="index">The zero-based starting index of the range of items to remove</param>
		/// <param name="count">The number of items to remove</param>
		[MethodImpl((MethodImplOptions)256 /* AggressiveInlining */)]
		private void RemoveItemRange(int index, int count)
		{
			_size -= count;

			if (index < _size)
			{
				Array.Copy(_items, index + count, _items, index, _size - index);
			}
			Array.Clear(_items, _size, count);
		}

		/// <summary>
		/// Removes all trailing whitespace characters in the last item of the output buffer
		/// </summary>
		public void TrimEndLastItem()
		{
			int itemCount = _size;
			if (itemCount == 0)
			{
				return;
			}

			int whitespaceItemCount = 0;

			for (int itemIndex = itemCount - 1; itemIndex >= 0; itemIndex--)
			{
				string item = _items[itemIndex];

				if (string.IsNullOrWhiteSpace(item))
				{
					whitespaceItemCount++;
				}
				else
				{
					_items[itemIndex] = item.TrimEnd(null);
					break;
				}
			}

			if (whitespaceItemCount > 0)
			{
				_size = itemCount - whitespaceItemCount;
			}
		}

		/// <summary>
		/// Removes a last start tag from the output buffer
		/// </summary>
		/// <param name="tagName">Tag name</param>
		/// <returns>Result of removing (true - has removed; false - has not removed)</returns>
		public bool RemoveLastStartTag(string tagName)
		{
			int itemCount = _size;
			if (itemCount == 0)
			{
				return false;
			}

			bool isStartTagRemoved = false;
			int lastItemIndex = itemCount - 1;
			int tagBeginPartItemIndex = Array.LastIndexOf(_items, "<", lastItemIndex, itemCount);
			int currentTagItemIndex = tagBeginPartItemIndex;

			if (tagBeginPartItemIndex != -1 && itemCount - tagBeginPartItemIndex >= 3)
			{
				currentTagItemIndex++;
				string currentTagName = _items[currentTagItemIndex];

				if (currentTagName.IgnoreCaseEquals(tagName))
				{
					currentTagItemIndex++;
					int tagEndPartItemIndex = Array.IndexOf(_items, ">", currentTagItemIndex,
						itemCount - currentTagItemIndex);

					if (tagEndPartItemIndex != -1)
					{
						bool noMoreContent = true;

						if (tagEndPartItemIndex != lastItemIndex)
						{
							for (int itemIndex = tagEndPartItemIndex + 1; itemIndex < itemCount; itemIndex++)
							{
								if (!string.IsNullOrWhiteSpace(_items[itemIndex]))
								{
									noMoreContent = false;
									break;
								}
							}
						}

						if (noMoreContent)
						{
							_size = tagBeginPartItemIndex;
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
		public bool RemoveLastEndTag(string tagName)
		{
			int itemCount = _size;
			if (itemCount == 0)
			{
				return false;
			}

			bool isEndTagRemoved = false;
			int lastItemIndex = itemCount - 1;
			int tagBeginPartItemIndex = Array.LastIndexOf(_items, "</", lastItemIndex, itemCount);
			int currentTagItemIndex = tagBeginPartItemIndex;

			if (tagBeginPartItemIndex != -1 && itemCount - tagBeginPartItemIndex >= 3)
			{
				currentTagItemIndex++;
				string currentTagName = _items[currentTagItemIndex];

				if (currentTagName.IgnoreCaseEquals(tagName))
				{
					currentTagItemIndex++;
					int tagEndPartItemIndex = currentTagItemIndex;

					if (_items[tagEndPartItemIndex].Equals(">"))
					{
						if (tagEndPartItemIndex != lastItemIndex)
						{
							bool noMoreContent = true;

							for (int itemIndex = tagEndPartItemIndex + 1; itemIndex < itemCount; itemIndex++)
							{
								if (!string.IsNullOrWhiteSpace(_items[itemIndex]))
								{
									noMoreContent = false;
									break;
								}
							}

							if (noMoreContent)
							{
								int tagItemCount = tagEndPartItemIndex - tagBeginPartItemIndex + 1;
								RemoveItemRange(tagBeginPartItemIndex, tagItemCount);
								isEndTagRemoved = true;
							}
						}
						else
						{
							_size = tagBeginPartItemIndex;
							isEndTagRemoved = true;
						}
					}
				}
			}

			return isEndTagRemoved;
		}
	}
}