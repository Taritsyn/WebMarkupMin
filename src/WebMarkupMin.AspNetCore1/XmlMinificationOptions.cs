using System.Collections.Generic;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.Core;

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2
#elif ASPNETCORE3
namespace WebMarkupMin.AspNetCore3
#else
#error No implementation for this target
#endif
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