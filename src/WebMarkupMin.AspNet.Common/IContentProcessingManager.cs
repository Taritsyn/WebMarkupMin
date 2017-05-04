using System.Collections.Generic;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Defines a interface of content processing manager
	/// </summary>
	public interface IContentProcessingManager
	{
		/// <summary>
		/// Gets or sets a list of supported HTTP methods
		/// </summary>
		ISet<string> SupportedHttpMethods
		{
			get;
			set;
		}
	}
}