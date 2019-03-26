using System;

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
		public void RemoveLastWhitespaceItems()
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
					break;
				}
			}

			if (whitespaceItemCount > 0)
			{
				_size = itemCount - whitespaceItemCount;
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