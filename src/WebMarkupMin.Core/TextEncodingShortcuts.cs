using System.Text;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// Text encoding shortcuts
	/// </summary>
	public static class TextEncodingShortcuts
	{
		/// <summary>
		/// Gets a default encoding for current operating system
		/// </summary>
		public static readonly Encoding Default =
#if NETSTANDARD1_3
			Encoding.GetEncoding(0)
#else
			Encoding.Default
#endif
			;
	}
}