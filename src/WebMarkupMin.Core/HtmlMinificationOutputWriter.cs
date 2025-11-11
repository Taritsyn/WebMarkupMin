using System;
using System.Runtime.CompilerServices;
using System.Text;

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
		/// <param name="initialBufferCapacity">Initial capacity of the buffer</param>
		public HtmlMinificationOutputWriter(int initialBufferCapacity)
			: base(initialBufferCapacity)
		{ }

		/// <summary>
		/// Constructs an instance of the HTML minification output writer
		/// </summary>
		/// <param name="initialBufferCapacity">Initial capacity of the buffer</param>
		/// <param name="newLineStyle">Style of the newline</param>
		public HtmlMinificationOutputWriter(int initialBufferCapacity, NewLineStyle newLineStyle)
			: base(initialBufferCapacity, newLineStyle)
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
		/// <param name="preserveNewLines">Flag for whether to collapse trailing whitespace to one newline string
		/// when whitespace contains a newline</param>
		public void TrimEndLastItem(bool preserveNewLines)
		{
			if (_size == 0)
			{
				return;
			}

			if (preserveNewLines)
			{
				InternalTrimEndLastItemWithNewLinesPreserved(_sb, _items, ref _size);
			}
			else
			{
				InternalTrimEndLastItem(_items, ref _size);
			}
		}

		private static void InternalTrimEndLastItem(string[] items, ref int size)
		{
			int itemCount = size;
			int whitespaceItemCount = 0;

			for (int itemIndex = itemCount - 1; itemIndex >= 0; itemIndex--)
			{
				string item = items[itemIndex];

				if (string.IsNullOrWhiteSpace(item))
				{
					whitespaceItemCount++;
				}
				else
				{
					items[itemIndex] = item.TrimEnd(null);
					break;
				}
			}

			if (whitespaceItemCount > 0)
			{
				size = itemCount - whitespaceItemCount;
			}
		}

		private static void InternalTrimEndLastItemWithNewLinesPreserved(StringBuilder sb, string[] items, ref int size)
		{
			int itemCount = size;
			int whitespaceItemCount = 0;
			int nonWhitespaceItemIndex = -1;

			for (int itemIndex = itemCount - 1; itemIndex >= 0; itemIndex--)
			{
				if (string.IsNullOrWhiteSpace(items[itemIndex]))
				{
					whitespaceItemCount++;
				}
				else
				{
					nonWhitespaceItemIndex = itemIndex;
					break;
				}
			}

			bool allowTrimmingWhitespaceItems = whitespaceItemCount > 0;

			if (nonWhitespaceItemIndex != -1)
			{
				string nonWhitespaceItem = items[nonWhitespaceItemIndex];
				string processedNonWhitespaceItem = nonWhitespaceItem.TrimEnd(true);

				items[nonWhitespaceItemIndex] = processedNonWhitespaceItem;
				allowTrimmingWhitespaceItems = !processedNonWhitespaceItem.EndsWithNewLine();
			}
			else
			{
				allowTrimmingWhitespaceItems = !sb.EndsWithNewLine();
			}

			if (allowTrimmingWhitespaceItems)
			{
				int firstWhitespaceItemIndex = nonWhitespaceItemIndex + 1;

				for (int whitespaceItemIndex = firstWhitespaceItemIndex; whitespaceItemIndex < itemCount; whitespaceItemIndex++)
				{
					string whitespaceItem = items[whitespaceItemIndex];
					string processedWhitespaceItem = whitespaceItem.GetNewLine();

					if (processedWhitespaceItem is not null)
					{
						items[firstWhitespaceItemIndex] = processedWhitespaceItem;
						whitespaceItemCount--;
						break;
					}
				}
			}

			if (whitespaceItemCount > 0)
			{
				size = itemCount - whitespaceItemCount;
			}
		}

		/// <summary>
		/// Collapses all trailing whitespace characters in the last item of the output buffer
		/// </summary>
		/// <param name="preserveNewLines">Flag for whether to collapse trailing whitespace to one newline string
		/// when whitespace contains a newline</param>
		public void CollapseLastWhitespaceItem(bool preserveNewLines)
		{
			if (_size == 0)
			{
				return;
			}

			if (preserveNewLines)
			{
				InternalCollapseLastWhitespaceItemWithNewLinesPreserved(_items, ref _size);
			}
			else
			{
				InternalCollapseLastWhitespaceItem(_items, ref _size);
			}
		}

		private static void InternalCollapseLastWhitespaceItem(string[] items, ref int size)
		{
			int itemCount = size;
			int whitespaceItemCount = 0;
			int nonWhitespaceItemIndex = -1;

			for (int itemIndex = itemCount - 1; itemIndex >= 0; itemIndex--)
			{
				if (string.IsNullOrWhiteSpace(items[itemIndex]))
				{
					whitespaceItemCount++;
				}
				else
				{
					nonWhitespaceItemIndex = itemIndex;
					break;
				}
			}

			bool allowCollapsingWhitespaceItems = whitespaceItemCount > 0;

			if (nonWhitespaceItemIndex != -1)
			{
				string nonWhitespaceItem = items[nonWhitespaceItemIndex];
				char lastCharValue = nonWhitespaceItem[nonWhitespaceItem.Length - 1];

				allowCollapsingWhitespaceItems = !char.IsWhiteSpace(lastCharValue);
			}

			if (allowCollapsingWhitespaceItems)
			{
				int firstWhitespaceItemIndex = nonWhitespaceItemIndex + 1;
				items[firstWhitespaceItemIndex] = " ";
				whitespaceItemCount--;
			}

			if (whitespaceItemCount > 0)
			{
				size = itemCount - whitespaceItemCount;
			}
		}

		private static void InternalCollapseLastWhitespaceItemWithNewLinesPreserved(string[] items, ref int size)
		{
			int itemCount = size;
			int whitespaceItemCount = 0;
			int nonWhitespaceItemIndex = -1;

			for (int itemIndex = itemCount - 1; itemIndex >= 0; itemIndex--)
			{
				if (string.IsNullOrWhiteSpace(items[itemIndex]))
				{
					whitespaceItemCount++;
				}
				else
				{
					nonWhitespaceItemIndex = itemIndex;
					break;
				}
			}

			bool allowCollapsingWhitespaceItems = whitespaceItemCount > 0;

			if (nonWhitespaceItemIndex != -1)
			{
				string nonWhitespaceItem = items[nonWhitespaceItemIndex];
				allowCollapsingWhitespaceItems = !nonWhitespaceItem.EndsWithNewLine();
			}

			if (allowCollapsingWhitespaceItems)
			{
				int firstWhitespaceItemIndex = nonWhitespaceItemIndex + 1;
				bool firstWhitespaceItemCollapsed = false;

				for (int whitespaceItemIndex = firstWhitespaceItemIndex; whitespaceItemIndex < itemCount; whitespaceItemIndex++)
				{
					string whitespaceItem = items[whitespaceItemIndex];
					string processedWhitespaceItem = whitespaceItem.GetNewLine();

					if (processedWhitespaceItem is not null)
					{
						items[firstWhitespaceItemIndex] = processedWhitespaceItem;
						firstWhitespaceItemCollapsed = true;
						whitespaceItemCount--;
						break;
					}
				}

				if (!firstWhitespaceItemCollapsed)
				{
					items[firstWhitespaceItemIndex] = " ";
					whitespaceItemCount--;
				}
			}

			if (whitespaceItemCount > 0)
			{
				size = itemCount - whitespaceItemCount;
			}
		}

		/// <summary>
		/// Removes a last start tag from the output buffer
		/// </summary>
		/// <param name="tagName">Tag name</param>
		/// <returns>Result of removing (<c>true</c> - has removed; <c>false</c> - has not removed)</returns>
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