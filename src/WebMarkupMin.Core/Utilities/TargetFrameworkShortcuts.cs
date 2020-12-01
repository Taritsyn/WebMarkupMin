using System.Text;
using System.Text.RegularExpressions;

namespace WebMarkupMin.Core.Utilities
{
	/// <summary>
	/// Shortcuts for accessing to values that depend on the target framework
	/// </summary>
	public static class TargetFrameworkShortcuts
	{
		/// <summary>
		/// Gets a default encoding for current operating system
		/// </summary>
		public static readonly Encoding DefaultTextEncoding =
#if NETSTANDARD1_3
			Encoding.GetEncoding(0)
#else
			Encoding.Default
#endif
			;

		/// <summary>
		/// Gets a regular expression options for improving performance
		/// </summary>
		internal static readonly RegexOptions PerformanceRegexOptions = RegexOptions.CultureInvariant
#if NETSTANDARD2_1
			| RegexOptions.Compiled
#endif
			;
	}
}