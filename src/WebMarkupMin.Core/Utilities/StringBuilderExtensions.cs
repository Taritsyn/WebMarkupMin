using System;
using System.Text;

namespace WebMarkupMin.Core.Utilities
{
	/// <summary>
	/// Extensions for StringBuilder
	/// </summary>
	internal static class StringBuilderExtensions
	{
		/// <summary>
		/// Determines whether the beginning of this <see cref="StringBuilder"/> instance matches the newline character
		/// </summary>
		/// <param name="source">Instance of <see cref="StringBuilder"/></param>
		/// <returns>true if the newline character matches the beginning of this instance; otherwise, false</returns>
		public static bool StartsWithNewLine(this StringBuilder source)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (source.Length == 0)
			{
				return false;
			}

			bool result = source[0].IsNewLine();

			return result;
		}

		/// <summary>
		/// Determines whether the end of this <see cref="StringBuilder"/> instance matches the newline character
		/// </summary>
		/// <param name="source">Instance of <see cref="StringBuilder"/></param>
		/// <returns>true if the newline character matches the end of this instance; otherwise, false</returns>
		public static bool EndsWithNewLine(this StringBuilder source)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			int length = source.Length;
			if (length == 0)
			{
				return false;
			}

			bool result = source[length - 1].IsNewLine();

			return result;
		}
	}
}