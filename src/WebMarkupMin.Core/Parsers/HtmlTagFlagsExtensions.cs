using System.Runtime.CompilerServices;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// Extensions for HTML tag flags
	/// </summary>
	internal static class HtmlTagFlagsExtensions
	{
		/// <summary>
		/// Determines whether one or more bit fields are set in the current instance
		/// </summary>
		/// <param name="source">Current instance</param>
		/// <param name="flag">An enumeration value</param>
		/// <returns><c>true</c> if the bit field or bit fields that are set in flag
		/// are also set in the current instance; otherwise, <c>false</c></returns>
		[MethodImpl((MethodImplOptions)256 /* AggressiveInlining */)]
		internal static bool IsSet(this HtmlTagFlags source, HtmlTagFlags flag)
		{
#if NETSTANDARD2_1_OR_GREATER || NET9_0_OR_GREATER
			return source.HasFlag(flag);
#else
			return (source & flag) == flag;
#endif
		}
	}
}