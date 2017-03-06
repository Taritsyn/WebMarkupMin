using Microsoft.Extensions.Options;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.Core.Loggers;

namespace WebMarkupMin.AspNetCore1
{
	/// <summary>
	/// HTML minification manager
	/// </summary>
	public sealed class HtmlMinificationManager : HtmlMinificationManagerBase
	{
		/// <summary>
		/// Constructs a instance of HTML minification manager
		/// </summary>
		/// <param name="logger">Logger</param>
		/// <param name="options">HTML minification options</param>
		public HtmlMinificationManager(ILogger logger, IOptions<HtmlMinificationOptions> options)
		{
			Logger = logger;

			HtmlMinificationOptions minificationOptions = options.Value;
			MinificationSettings = minificationOptions.MinificationSettings;
			SupportedMediaTypes = minificationOptions.SupportedMediaTypes;
			IncludedPages = minificationOptions.IncludedPages;
			ExcludedPages = minificationOptions.ExcludedPages;
			GenerateStatistics = minificationOptions.GenerateStatistics;
			JsMinifierFactory = minificationOptions.JsMinifierFactory;
			CssMinifierFactory = minificationOptions.CssMinifierFactory;
		}
	}
}