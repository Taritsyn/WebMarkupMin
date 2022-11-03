using System.Collections.Generic;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.Core;

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2
#elif ASPNETCORE3
namespace WebMarkupMin.AspNetCore3
#elif ASPNETCORE5
namespace WebMarkupMin.AspNetCore5
#elif ASPNETCORE6
namespace WebMarkupMin.AspNetCore6
#elif ASPNETCORE7
namespace WebMarkupMin.AspNetCore7
#else
#error No implementation for this target
#endif
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