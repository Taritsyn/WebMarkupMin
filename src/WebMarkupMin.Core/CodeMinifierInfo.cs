namespace WebMarkupMin.Core
{
	/// <summary>
	/// Code minifier information
	/// </summary>
	public sealed class CodeMinifierInfo
	{
		/// <summary>
		/// Gets or sets a code minifier name
		/// </summary>
		public string Name
		{
			get;
			internal set;
		}

		/// <summary>
		/// Gets or sets a code minifier display name
		/// </summary>
		public string DisplayName
		{
			get;
			internal set;
		}

		/// <summary>
		/// Gets or sets code minifier .NET-type name
		/// </summary>
		internal string Type
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs instance of code minifier information
		/// </summary>
		/// <param name="name">Code minifier name</param>
		/// <param name="displayName">Code minifier display name</param>
		/// <param name="type">Code minifier .NET-type name</param>
		internal CodeMinifierInfo(string name, string displayName, string type)
		{
			Name = name;
			DisplayName = displayName;
			Type = type;
		}
	}
}