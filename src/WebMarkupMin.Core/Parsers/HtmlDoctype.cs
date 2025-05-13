namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML document type declaration
	/// </summary>
	internal sealed class HtmlDoctype
	{
		/// <summary>
		/// Gets a DOCTYPE instruction
		/// </summary>
		public string Instruction
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a flag indicating the presence of a space before the root element
		/// </summary>
		public bool SpaceBeforeRootElement
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a parent element that contains all the other elements
		/// </summary>
		public string RootElement
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a publicity (document can be <c>PUBLIC</c> or <c>SYSTEM</c>)
		/// </summary>
		public string Publicity
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a formal public identifier
		/// </summary>
		public HtmlFormalPublicId PublicId
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a system identifier
		/// </summary>
		public HtmlSystemId SystemId
		{
			get;
			private set;
		}


		/// <summary>
		/// Constructs an instance of HTML document type declaration
		/// </summary>
		/// <param name="instruction">DOCTYPE instruction</param>
		/// <param name="spaceBeforeRootElement">Flag indicating the presence of a space before the root element</param>
		/// <param name="rootElement">Parent element that contains all the other elements</param>
		public HtmlDoctype(string instruction, bool spaceBeforeRootElement, string rootElement)
			: this(instruction, spaceBeforeRootElement, rootElement, string.Empty, null, null)
		{ }

		/// <summary>
		/// Constructs an instance of HTML document type declaration
		/// </summary>
		/// <param name="instruction">DOCTYPE instruction</param>
		/// <param name="spaceBeforeRootElement">Flag indicating the presence of a space before the root element</param>
		/// <param name="rootElement">Parent element that contains all the other elements</param>
		/// <param name="publicity">Publicity</param>
		/// <param name="publicId">Formal public identifier</param>
		public HtmlDoctype(string instruction, bool spaceBeforeRootElement, string rootElement, string publicity,
			HtmlFormalPublicId publicId)
			: this(instruction, spaceBeforeRootElement, rootElement, publicity, publicId, null)
		{ }

		/// <summary>
		/// Constructs an instance of HTML document type declaration
		/// </summary>
		/// <param name="instruction">DOCTYPE instruction</param>
		/// <param name="spaceBeforeRootElement">Flag indicating the presence of a space before the root element</param>
		/// <param name="rootElement">Parent element that contains all the other elements</param>
		/// <param name="publicity">Publicity</param>
		/// <param name="systemId">System identifier</param>
		public HtmlDoctype(string instruction, bool spaceBeforeRootElement, string rootElement, string publicity,
			HtmlSystemId systemId)
			: this(instruction, spaceBeforeRootElement, rootElement, publicity, null, systemId)
		{ }

		/// <summary>
		/// Constructs an instance of HTML document type declaration
		/// </summary>
		/// <param name="instruction">DOCTYPE instruction</param>
		/// <param name="spaceBeforeRootElement">Flag indicating the presence of a space before the root element</param>
		/// <param name="rootElement">Parent element that contains all the other elements</param>
		/// <param name="publicity">Publicity</param>
		/// <param name="publicId">Formal public identifier</param>
		/// <param name="systemId">System identifier</param>
		public HtmlDoctype(string instruction, bool spaceBeforeRootElement, string rootElement, string publicity,
			HtmlFormalPublicId publicId, HtmlSystemId systemId)
		{
			Instruction = instruction;
			SpaceBeforeRootElement = spaceBeforeRootElement;
			RootElement = rootElement;
			Publicity = publicity;
			PublicId = publicId;
			SystemId = systemId;
		}
	}
}