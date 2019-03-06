using System;
using System.Collections.Generic;
using System.Text;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// Markup minification output writer
	/// </summary>
	internal abstract class MarkupMinificationOutputWriterBase
	{
		/// <summary>
		/// Underlying string builder
		/// </summary>
		private StringBuilder _sb;

		/// <summary>
		/// Buffer for items
		/// </summary>
		protected readonly List<string> _buffer;

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
		protected MarkupMinificationOutputWriterBase()
		{
			_buffer = new List<string>();
		}


		/// <summary>
		/// Writes a string to the output buffer
		/// </summary>
		/// <param name="value">The string to write</param>
		public void Write(string value)
		{
			_buffer.Add(value);
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

			int bufferItemCount = _buffer.Count;

			if (bufferItemCount > 0)
			{
				for (int bufferItemIndex = 0; bufferItemIndex < bufferItemCount; bufferItemIndex++)
				{
					string bufferItem = _buffer[bufferItemIndex];

					if (bufferItem.Length > 0)
					{
						_sb.Append(bufferItem);
					}
				}

				_buffer.Clear();
			}
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

			_buffer.Clear();
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

			if (_buffer.Count > 0)
			{
				Flush();
			}

			return _sb.ToString();
		}

		#endregion
	}
}