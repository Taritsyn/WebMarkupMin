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