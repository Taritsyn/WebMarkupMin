using System.Runtime.CompilerServices;

namespace WebMarkupMin.Core.Utilities
{
	/// <summary>
	/// Extensions for Char
	/// </summary>
	internal static class CharExtensions
	{
		[MethodImpl((MethodImplOptions)256 /* AggressiveInlining */)]
		public static bool IsNumeric(this char source)
		{
			return source >= '0' && source <= '9';
		}

		[MethodImpl((MethodImplOptions)256 /* AggressiveInlining */)]
		public static bool IsAlphaLower(this char source)
		{
			return source >= 'a' && source <= 'z';
		}

		[MethodImpl((MethodImplOptions)256 /* AggressiveInlining */)]
		public static bool IsAlphaUpper(this char source)
		{
			return source >= 'A' && source <= 'Z';
		}

		[MethodImpl((MethodImplOptions)256 /* AggressiveInlining */)]
		public static bool IsAlpha(this char source)
		{
			return IsAlphaLower(source) || IsAlphaUpper(source);
		}

		[MethodImpl((MethodImplOptions)256 /* AggressiveInlining */)]
		public static bool IsAlphaNumeric(this char source)
		{
			return IsAlpha(source) || IsNumeric(source);
		}
	}
}