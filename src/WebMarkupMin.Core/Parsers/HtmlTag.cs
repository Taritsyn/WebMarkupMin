using System.Collections.Generic;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML tag
	/// </summary>
	internal sealed class HtmlTag
	{
		/// <summary>
		/// Name
		/// </summary>
		public string Name
		{
			get;
			private set;
		}

		/// <summary>
		/// List of attributes
		/// </summary>
		public IList<HtmlAttribute> Attributes
		{
			get;
			private set;
		}

		/// <summary>
		/// Flags
		/// </summary>
		public HtmlTagFlags Flags
		{
			get;
			private set;
		}

		/// <summary>
		/// Constructs instance of HTML tag
		/// </summary>
		/// <param name="name">Name</param>
		/// <param name="flags">Flags</param>
		public HtmlTag(string name, HtmlTagFlags flags)
			: this(name, new List<HtmlAttribute>(), flags)
		{ }

		/// <summary>
		/// Constructs instance of HTML tag
		/// </summary>
		/// <param name="name">Name</param>
		/// <param name="attributes">List of attributes</param>
		/// <param name="flags">Flags</param>
		public HtmlTag(string name, IList<HtmlAttribute> attributes, HtmlTagFlags flags)
		{
			Name = name;
			Attributes = attributes;
			Flags = flags;
		}
	}
}