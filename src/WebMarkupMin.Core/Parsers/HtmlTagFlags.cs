using System;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML tag flags
	/// </summary>
	[Flags]
	internal enum HtmlTagFlags : ushort
	{
		/// <summary>
		/// No flags are set
		/// </summary>
		None = 0,

		/// <summary>
		/// Tag is invisible
		/// </summary>
		Invisible = 1,

		/// <summary>
		/// Tag is empty
		/// </summary>
		Empty = 2,

		/// <summary>
		/// Tag is block
		/// </summary>
		Block = 4,

		/// <summary>
		/// Tag is inline
		/// </summary>
		Inline = 8,

		/// <summary>
		/// Tag is inline-block
		/// </summary>
		InlineBlock = 16,

		/// <summary>
		/// Tag is non-independent
		/// </summary>
		NonIndependent = 32,

		/// <summary>
		/// Tag, that can be omitted
		/// </summary>
		Optional = 64,

		/// <summary>
		/// Tag can contain embedded code
		/// </summary>
		EmbeddedCode = 128,

		/// <summary>
		/// Tag is XML-based
		/// </summary>
		Xml = 256,
	}
}