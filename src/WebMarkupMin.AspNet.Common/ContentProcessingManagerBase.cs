using System.Collections.Generic;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Base class of content processing manager
	/// </summary>
	public abstract class ContentProcessingManagerBase : IContentProcessingManager
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
		/// Constructs a instance of content processing manager
		/// </summary>
		protected ContentProcessingManagerBase()
		{
			SupportedHttpMethods = new HashSet<string> { "GET" };
		}
	}
}