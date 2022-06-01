namespace WebMarkupMin.Core
{
	/// <summary>
	/// Style of the newline on different platforms
	/// </summary>
	public enum NewLineStyle : byte
	{
		/// <summary>
		/// Auto-detect style for newline based on the source input
		/// </summary>
		Auto = 0,

		/// <summary>
		/// CRLF in Windows, LF on other platforms
		/// </summary>
		Native = 1,

		/// <summary>
		/// Force the Windows style for newline (CRLF)
		/// </summary>
		Windows = 2,

		/// <summary>
		/// Force the Macintosh style for newline (CR)
		/// </summary>
		Mac = 3,

		/// <summary>
		/// Force the Unix style for newline (LF)
		/// </summary>
		Unix = 4
	}
}