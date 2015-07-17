using System.Collections.Generic;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.Core;

namespace WebMarkupMin.AspNet5
{
	/// <summary>
	/// XHTML minification options
	/// </summary>
	public sealed class XhtmlMinificationOptions
		: CommonHtmlMinificationOptionsBase<XhtmlMinificationSettings>
    {
		/// <summary>
		/// Constructs a instance of XHTML minification options
		/// </summary>
		public XhtmlMinificationOptions()
		{
			MinificationSettings = new XhtmlMinificationSettings();
			SupportedMediaTypes = new HashSet<string>(MediaTypeGroupConstants.Xhtml);
		}
	}
}