using System.Collections.Generic;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.Core;

namespace WebMarkupMin.AspNet5
{
	/// <summary>
	/// HTML minification options
	/// </summary>
	public sealed class HtmlMinificationOptions
		: CommonHtmlMinificationOptionsBase<HtmlMinificationSettings>
	{
		/// <summary>
		/// Constructs a instance of HTML minification options
		/// </summary>
		public HtmlMinificationOptions()
		{
			MinificationSettings = new HtmlMinificationSettings();
			SupportedMediaTypes = new HashSet<string>(MediaTypeGroupConstants.Html);
		}
	}
}