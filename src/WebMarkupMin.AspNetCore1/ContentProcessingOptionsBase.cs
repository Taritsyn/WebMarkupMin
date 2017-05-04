using System.Collections.Generic;

namespace WebMarkupMin.AspNetCore1
{
	/// <summary>
	/// Base class of content processing options
	/// </summary>
	public abstract class ContentProcessingOptionsBase
	{
		/// <summary>
		/// Gets or sets a list of supported HTTP methods
		/// </summary>
		public ISet<string> SupportedHttpMethods
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs a instance of content processing options
		/// </summary>
		protected ContentProcessingOptionsBase()
		{
			SupportedHttpMethods = new HashSet<string> { "GET" };
		}
	}
}