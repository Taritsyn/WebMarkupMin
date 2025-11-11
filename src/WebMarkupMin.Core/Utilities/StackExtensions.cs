#if !NETSTANDARD2_1_OR_GREATER
using System;
using System.Collections.Generic;

namespace WebMarkupMin.Core.Utilities
{
	/// <summary>
	/// Extensions for Stack
	/// </summary>
	internal static class StackExtensions
	{
		public static bool TryPeek<T>(this Stack<T> source, out T result)
		{
			if (source is null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			bool stackIsNotEmpty = source.Count > 0;
			result = stackIsNotEmpty ? source.Peek() : default(T);

			return stackIsNotEmpty;
		}
	}
}
#endif