namespace WebMarkupMin.Core.Loggers
{
	/// <summary>
	/// Log category constants
	/// </summary>
	public static class LogCategoryConstants
	{
		public static readonly string HtmlParsingError = "HTML_PARSING_ERROR";
		public static readonly string HtmlMinificationError = "HTML_MINIFICATION_ERROR";
		public static readonly string HtmlMinificationWarning = "HTML_MINIFICATION_WARN";
		public static readonly string HtmlMinificationSuccess = "HTML_MINIFICATION_SUCCESS";

		public static readonly string XmlParsingError = "XML_PARSING_ERROR";
		public static readonly string XmlMinificationError = "XML_MINIFICATION_ERROR";
		public static readonly string XmlMinificationSuccess = "XML_MINIFICATION_SUCCESS";

		public static readonly string CssMinificationError = "CSS_MINIFICATION_ERROR";
		public static readonly string CssMinificationWarning = "CSS_MINIFICATION_WARN";

		public static readonly string JsMinificationError = "JS_MINIFICATION_ERROR";
		public static readonly string JsMinificationWarning = "JS_MINIFICATION_WARN";

		public static readonly string JsTemplateMinificationError = "JS_TEMPLATE_MINIFICATION_ERROR";
		public static readonly string JsTemplateMinificationWarning = "JS_TEMPLATE_MINIFICATION_WARN";
	}
}