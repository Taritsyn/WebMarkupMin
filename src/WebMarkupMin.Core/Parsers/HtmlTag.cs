using System.Collections.Generic;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML tag
	/// </summary>
	internal sealed class HtmlTag
	{
		/// <summary>
		/// Gets a name
		/// </summary>
		public string Name
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a name in lowercase
		/// </summary>
		public string NameInLowercase
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a list of attributes
		/// </summary>
		public IList<HtmlAttribute> Attributes
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a flags
		/// </summary>
		public HtmlTagFlags Flags
		{
			get;
			private set;
		}

		/// <summary>
		/// Represents a empty HTML tag
		/// </summary>
		public static readonly HtmlTag Empty = new HtmlTag(string.Empty, string.Empty,
			new List<HtmlAttribute>(), HtmlTagFlags.None);


		/// <summary>
		/// Constructs instance of HTML tag
		/// </summary>
		/// <param name="name">Name</param>
		/// <param name="nameInLowercase">Name in lowercase</param>
		/// <param name="flags">Flags</param>
		public HtmlTag(string name, string nameInLowercase, HtmlTagFlags flags)
			: this(name, nameInLowercase, new List<HtmlAttribute>(), flags)
		{ }

		/// <summary>
		/// Constructs instance of HTML tag
		/// </summary>
		/// <param name="name">Name</param>
		/// <param name="nameInLowercase">Name in lowercase</param>
		/// <param name="attributes">List of attributes</param>
		/// <param name="flags">Flags</param>
		public HtmlTag(string name, string nameInLowercase, IList<HtmlAttribute> attributes, HtmlTagFlags flags)
		{
			Name = name;
			NameInLowercase = nameInLowercase;
			Attributes = attributes;
			Flags = flags;
		}
	}
}