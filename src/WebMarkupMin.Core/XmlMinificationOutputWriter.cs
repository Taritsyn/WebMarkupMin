using System;

using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// XML minification output writer
	/// </summary>
	internal sealed class XmlMinificationOutputWriter : MarkupMinificationOutputWriterBase
	{
		/// <summary>
		/// Constructs an instance of the XML minification output writer
		/// </summary>
		public XmlMinificationOutputWriter()
			: base(DefaultBufferCapacity)
		{ }

		/// <summary>
		/// Constructs an instance of the XML minification output writer
		/// </summary>
		/// <param name="initialBufferCapacity">Initial capacity of buffer</param>
		public XmlMinificationOutputWriter(int initialBufferCapacity)
			: base(initialBufferCapacity)
		{ }


		/// <summary>
		/// Removes a last whitespace items from the output buffer
		/// </summary>
		/// <param name="preserveNewLines">Flag for whether to collapse whitespace to one newline string
		/// when whitespace contains a newline</param>
		public void RemoveLastWhitespaceItems(bool preserveNewLines)
		{
			if (_size == 0)
			{
				return;
			}

			if (preserveNewLines)
			{
				InternalRemoveLastWhitespaceItemsWithNewLinesPreserved(_items, ref _size);
			}
			else
			{
				InternalRemoveLastWhitespaceItems(_items, ref _size);
			}
		}

		private static void InternalRemoveLastWhitespaceItems(string[] items, ref int size)
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
					break;
				}
			}

			if (whitespaceItemCount > 0)
			{
				size = itemCount - whitespaceItemCount;
			}
		}

		private static void InternalRemoveLastWhitespaceItemsWithNewLinesPreserved(string[] items, ref int size)
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
				allowTrimmingWhitespaceItems = !nonWhitespaceItem.EndsWithNewLine();
			}

			if (allowTrimmingWhitespaceItems)
			{
				int firstWhitespaceItemIndex = nonWhitespaceItemIndex + 1;

				for (int whitespaceItemIndex = firstWhitespaceItemIndex; whitespaceItemIndex < itemCount; whitespaceItemIndex++)
				{
					string whitespaceItem = items[whitespaceItemIndex];
					string processedWhitespaceItem = whitespaceItem.GetNewLine();

					if (processedWhitespaceItem != null)
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
		/// Transform a last start tag to empty tag in the output buffer
		/// </summary>
		/// <returns>Result of transforming (true - has transformed; false - has not transformed)</returns>
		public bool TransformLastStartTagToEmptyTag(bool renderEmptyTagsWithSpace)
		{
			int itemCount = _size;
			if (itemCount == 0)
			{
				return false;
			}

			bool isTransformed = false;
			int lastItemIndex = itemCount - 1;
			int tagEndPartItemIndex = Array.LastIndexOf(_items, ">", lastItemIndex, itemCount);

			if (tagEndPartItemIndex == lastItemIndex)
			{
				_items[tagEndPartItemIndex] = renderEmptyTagsWithSpace ? " />" : "/>";
				isTransformed = true;
			}

			return isTransformed;
		}
	}
}