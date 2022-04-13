using System;
using System.Runtime.CompilerServices;
using System.Text;

using AdvancedStringBuilder;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// Markup minification output writer
	/// </summary>
	internal abstract class MarkupMinificationOutputWriterBase
	{
		/// <summary>
		/// The default capacity of buffer
		/// </summary>
		protected const int DefaultBufferCapacity = 16;

		/// <summary>
		/// Underlying string builder
		/// </summary>
		protected StringBuilder _sb;

		/// <summary>
		/// Buffer for items
		/// </summary>
		protected string[] _items;

		/// <summary>
		/// Size of used part of the buffer
		/// </summary>
		protected int _size;

		/// <summary>
		/// Gets or sets a underlying string builder
		/// </summary>
		public StringBuilder StringBuilder
		{
			get { return _sb; }
			set { _sb = value; }
		}


		/// <summary>
		/// Constructs an instance of the markup minification output writer
		/// </summary>
		/// <param name="initialBufferCapacity">Initial capacity of buffer</param>
		protected MarkupMinificationOutputWriterBase(int initialBufferCapacity)
		{
			_items = new string[initialBufferCapacity];
		}


		/// <summary>
		/// Ensures that the capacity of the output buffer is at least the given minimum value
		/// </summary>
		/// <param name="minCapacity">Minimum capacity</param>
		[MethodImpl((MethodImplOptions)256 /* AggressiveInlining */)]
		private void EnsureCapacity(int minCapacity)
		{
			int capacity = _items.Length;
			if (minCapacity <= capacity)
			{
				return;
			}

			int newCapacity = capacity > 0 ? capacity + DefaultBufferCapacity : DefaultBufferCapacity;
			if (newCapacity < minCapacity)
			{
				newCapacity = minCapacity;
			}

			if (newCapacity <= capacity)
			{
				return;
			}

			Array.Resize(ref _items, newCapacity);
		}

		/// <summary>
		/// Writes a string to the output buffer
		/// </summary>
		/// <param name="value">The string to write</param>
		public void Write(string value)
		{
			if (_size == _items.Length)
			{
				EnsureCapacity(_size + 1);
			}

			_items[_size++] = value;
		}

		/// <summary>
		/// Clears a buffer for the current output writer and causes any buffered data to be written
		/// to the underlying string builder
		/// </summary>
		public void Flush()
		{
			if (_sb == null)
			{
				throw new InvalidOperationException();
			}

			int itemCount = _size;
			if (itemCount > 0)
			{
				for (int itemIndex = 0; itemIndex < itemCount; itemIndex++)
				{
					string item = _items[itemIndex];

					if (item.Length > 0)
					{
						_sb.Append(item);
					}
				}

				_size = 0;
			}
		}

		/// <summary>
		/// Removes the all trailing whitespace characters from the underlying string builder
		/// </summary>
		public void TrimEnd()
		{
			if (_sb == null)
			{
				throw new InvalidOperationException();
			}

			_sb.TrimEnd();
		}

		/// <summary>
		/// Removes all items from the output buffer and underlying string builder
		/// </summary>
		public void Clear()
		{
			if (_sb == null)
			{
				throw new InvalidOperationException();
			}

			if (_size > 0)
			{
				Array.Clear(_items, 0, _items.Length);
				_size = 0;
			}

			_sb.Clear();
		}

		#region Object overrides

		/// <summary>
		/// Returns a string containing the items written to the current output writer so far
		/// </summary>
		/// <returns>The string containing the items written to the current output writer</returns>
		public override string ToString()
		{
			if (_sb == null)
			{
				throw new InvalidOperationException();
			}

			if (_size > 0)
			{
				Flush();
			}

			return _sb.ToString();
		}

		#endregion
	}
}