using Microsoft.Extensions.Options;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.Core.Loggers;

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2
#elif ASPNETCORE3
namespace WebMarkupMin.AspNetCore3
#elif ASPNETCORE5
namespace WebMarkupMin.AspNetCore5
#else
#error No implementation for this target
#endif
{
	/// <summary>
	/// XML minification manager
	/// </summary>
	public sealed class XmlMinificationManager : XmlMinificationManagerBase
	{
		/// <summary>
		/// Constructs a instance of XML minification manager
		/// </summary>
		/// <param name="logger">Logger</param>
		/// <param name="options">XML minification options</param>
		public XmlMinificationManager(ILogger logger, IOptions<XmlMinificationOptions> options)
		{
			Logger = logger;

			XmlMinificationOptions minificationOptions = options.Value;
			MinificationSettings = minificationOptions.MinificationSettings;
			SupportedHttpMethods = minificationOptions.SupportedHttpMethods;
			SupportedMediaTypes = minificationOptions.SupportedMediaTypes;
			IncludedPages = minificationOptions.IncludedPages;
			ExcludedPages = minificationOptions.ExcludedPages;
			GenerateStatistics = minificationOptions.GenerateStatistics;
		}
	}
}