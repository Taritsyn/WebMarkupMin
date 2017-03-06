using Microsoft.Extensions.Options;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.Core.Loggers;

namespace WebMarkupMin.AspNetCore1
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
			SupportedMediaTypes = minificationOptions.SupportedMediaTypes;
			IncludedPages = minificationOptions.IncludedPages;
			ExcludedPages = minificationOptions.ExcludedPages;
			GenerateStatistics = minificationOptions.GenerateStatistics;
		}
	}
}