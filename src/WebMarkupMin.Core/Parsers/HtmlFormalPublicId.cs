namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML formal public identifier (FPI) for document type declaration
	/// </summary>
	internal sealed class HtmlFormalPublicId : HtmlExternalIdBase
	{
		/// <summary>
		/// Gets a registration
		/// </summary>
		/// <remarks>
		/// Can have two values:
		/// plus (+) - the developer is registered in ISO
		/// minus (-) - the developer is not registered
		/// </remarks>
		public string Registration
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a name of DTD developer
		/// </summary>
		public string Organization
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a type of the document
		/// </summary>
		public string Type
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a unique identifier describing DTD
		/// </summary>
		public string Name
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a language of the document
		/// </summary>
		public string Language
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a version
		/// </summary>
		public string Version
		{
			get;
			private set;
		}


		/// <summary>
		/// Constructs an instance of HTML formal public identifier
		/// </summary>
		/// <param name="registration">Registration</param>
		/// <param name="organization">Name of DTD developer</param>
		/// <param name="type">Type of the document</param>
		/// <param name="name">Unique identifier describing DTD</param>
		/// <param name="language">Language of the document</param>
		public HtmlFormalPublicId(string registration, string organization, string type, string name, string language)
			: this(registration, organization, type, name, language, string.Empty)
		{ }

		/// <summary>
		/// Constructs an instance of HTML formal public identifier
		/// </summary>
		/// <param name="registration">Registration</param>
		/// <param name="organization">Name of DTD developer</param>
		/// <param name="type">Type of the document</param>
		/// <param name="name">Unique identifier describing DTD</param>
		/// <param name="language">Language of the document</param>
		/// <param name="quoteChar">Quote character used for identifier values</param>
		public HtmlFormalPublicId(string registration, string organization, string type, string name, string language,
			char quoteChar)
			: this(registration, organization, type, name, language, string.Empty, quoteChar)
		{ }

		/// <summary>
		/// Constructs an instance of HTML formal public identifier
		/// </summary>
		/// <param name="registration">Registration</param>
		/// <param name="organization">Name of DTD developer</param>
		/// <param name="type">Type of the document</param>
		/// <param name="name">Unique identifier describing DTD</param>
		/// <param name="language">Language of the document</param>
		/// <param name="version">Version</param>
		public HtmlFormalPublicId(string registration, string organization, string type, string name, string language,
			string version)
			: this(registration, organization, type, name, language, version, '"')
		{ }

		/// <summary>
		/// Constructs an instance of HTML formal public identifier
		/// </summary>
		/// <param name="registration">Registration</param>
		/// <param name="organization">Name of DTD developer</param>
		/// <param name="type">Type of the document</param>
		/// <param name="name">Unique identifier describing DTD</param>
		/// <param name="language">Language of the document</param>
		/// <param name="version">Version</param>
		/// <param name="quoteChar">Quote character used for identifier values</param>
		public HtmlFormalPublicId(string registration, string organization, string type, string name, string language,
			string version, char quoteChar)
			: base(quoteChar)
		{
			Registration = registration;
			Organization = organization;
			Type = type;
			Name = name;
			Language = language;
			Version = version;
		}
	}
}