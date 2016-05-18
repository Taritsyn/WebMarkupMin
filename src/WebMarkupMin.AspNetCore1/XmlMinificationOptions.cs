using System.Collections.Generic;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.Core;

namespace WebMarkupMin.AspNetCore1
{
	/// <summary>
	/// XML minification options
	/// </summary>
	public sealed class XmlMinificationOptions : MarkupMinificationOptionsBase<XmlMinificationSettings>
	{
		/// <summary>
		/// Constructs a instance of XML minification options
		/// </summary>
		public XmlMinificationOptions()
		{
			MinificationSettings = new XmlMinificationSettings();
			SupportedMediaTypes = new HashSet<string>(MediaTypeGroupConstants.Xml);
		}
	}
}