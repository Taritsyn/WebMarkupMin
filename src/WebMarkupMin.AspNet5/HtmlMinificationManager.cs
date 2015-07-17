using Microsoft.Framework.OptionsModel;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.Core.Loggers;

namespace WebMarkupMin.AspNet5
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
			_logger = logger;

			HtmlMinificationOptions minificationOptions = options.Options;
			MinificationSettings = minificationOptions.MinificationSettings;
			SupportedMediaTypes = minificationOptions.SupportedMediaTypes;
			IncludedPages = minificationOptions.IncludedPages;
			ExcludedPages = minificationOptions.ExcludedPages;
			JsMinifierFactory = minificationOptions.JsMinifierFactory;
			CssMinifierFactory = minificationOptions.CssMinifierFactory;
		}
	}
}