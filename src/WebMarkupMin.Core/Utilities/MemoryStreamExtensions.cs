using System.IO;

namespace WebMarkupMin.Core.Utilities
{
	/// <summary>
	/// Memory stream extensions
	/// </summary>
	public static class MemoryStreamExtensions
	{
		/// <summary>
		/// Clears a buffer
		/// </summary>
		/// <param name="source">Instance of <see cref="MemoryStream"/></param>
		public static void Clear(this MemoryStream source)
		{
			if (source.CanWrite)
			{
				source.SetLength(0);
			}
		}
	}
}