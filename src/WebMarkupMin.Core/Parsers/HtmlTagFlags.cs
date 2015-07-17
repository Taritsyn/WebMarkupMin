namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML tag flags
	/// </summary>
	internal class HtmlTagFlags
	{
		/// <summary>
		/// Flag whether the tag is invisible
		/// </summary>
		public bool Invisible
		{
			get;
			set;
		}

		/// <summary>
		/// Flag whether the tag is empty
		/// </summary>
		public bool Empty
		{
			get;
			set;
		}

		/// <summary>
		/// Flag whether the tag is block
		/// </summary>
		public bool Block
		{
			get;
			set;
		}

		/// <summary>
		/// Flag whether the tag is inline
		/// </summary>
		public bool Inline
		{
			get;
			set;
		}

		/// <summary>
		/// Flag whether the tag is inline-block
		/// </summary>
		public bool InlineBlock
		{
			get;
			set;
		}

		/// <summary>
		/// Flag whether the tag is non-independent
		/// </summary>
		public bool NonIndependent
		{
			get;
			set;
		}

		/// <summary>
		/// Flag whether the tag has end tag, thant can be omitted
		/// </summary>
		public bool OptionalEndTag
		{
			get;
			set;
		}

		/// <summary>
		/// Flag whether the tag can contain embedded code
		/// </summary>
		public bool EmbeddedCode
		{
			get;
			set;
		}

		/// <summary>
		/// Flag whether the tag is XML-based
		/// </summary>
		public bool Xml
		{
			get;
			set;
		}
	}
}