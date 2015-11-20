using Microsoft.Extensions.OptionsModel;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.Core.Loggers;

namespace WebMarkupMin.AspNet5
{
	/// <summary>
	/// XHTML minification manager
	/// </summary>
	public sealed class XhtmlMinificationManager : XhtmlMinificationManagerBase
	{
		/// <summary>
		/// Constructs a instance of XHTML minification manager
		/// </summary>
		/// <param name="logger">Logger</param>
		/// <param name="options">XHTML minification options</param>
		public XhtmlMinificationManager(ILogger logger, IOptions<XhtmlMinificationOptions> options)
		{
			_logger = logger;

			XhtmlMinificationOptions minificationOptions = options.Value;
			MinificationSettings = minificationOptions.MinificationSettings;
			SupportedMediaTypes = minificationOptions.SupportedMediaTypes;
			IncludedPages = minificationOptions.IncludedPages;
			ExcludedPages = minificationOptions.ExcludedPages;
			JsMinifierFactory = minificationOptions.JsMinifierFactory;
			CssMinifierFactory = minificationOptions.CssMinifierFactory;
		}
	}
}