using System.Text;

using WebMarkupMin.Core.Loggers;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// HTML minifier
	/// </summary>
	public sealed class HtmlMinifier : IMarkupMinifier
	{
		/// <summary>
		/// Generic HTML minifier
		/// </summary>
		private readonly GenericHtmlMinifier _genericHtmlMinifier;


		/// <summary>
		/// Constructs instance of HTML minifier
		/// </summary>
		/// <param name="settings">HTML minification settings</param>
		/// <param name="cssMinifier">CSS minifier</param>
		/// <param name="jsMinifier">JS minifier</param>
		/// <param name="logger">Logger</param>
		public HtmlMinifier(HtmlMinificationSettings settings = null, ICssMinifier cssMinifier = null,
			IJsMinifier jsMinifier = null, ILogger logger = null)
		{
			settings = settings ?? new HtmlMinificationSettings();
			var internalSettings = new GenericHtmlMinificationSettings
			{
				WhitespaceMinificationMode = settings.WhitespaceMinificationMode,
				PreserveNewLines = settings.PreserveNewLines,
				NewLineStyle = settings.NewLineStyle,
				RemoveHtmlComments = settings.RemoveHtmlComments,
				RemoveHtmlCommentsFromScriptsAndStyles = settings.RemoveHtmlCommentsFromScriptsAndStyles,
				RemoveCdataSectionsFromScriptsAndStyles = settings.RemoveCdataSectionsFromScriptsAndStyles,
				UseShortDoctype = settings.UseShortDoctype,
				CustomShortDoctype = settings.CustomShortDoctype,
				PreserveCase = settings.PreserveCase,
				UseMetaCharsetTag = settings.UseMetaCharsetTag,
				EmptyTagRenderMode = settings.EmptyTagRenderMode,
				RemoveOptionalEndTags = settings.RemoveOptionalEndTags,
				RemoveTagsWithoutContent = settings.RemoveTagsWithoutContent,
				AttributeQuotesStyle = settings.AttributeQuotesStyle,
				CollapseBooleanAttributes = settings.CollapseBooleanAttributes,
				AttributeQuotesRemovalMode = settings.AttributeQuotesRemovalMode,
				RemoveEmptyAttributes = settings.RemoveEmptyAttributes,
				RemoveRedundantAttributes = settings.RemoveRedundantAttributes,
				RemoveJsTypeAttributes = settings.RemoveJsTypeAttributes,
				RemoveCssTypeAttributes = settings.RemoveCssTypeAttributes,
				RemoveHttpProtocolFromAttributes = settings.RemoveHttpProtocolFromAttributes,
				RemoveHttpsProtocolFromAttributes = settings.RemoveHttpsProtocolFromAttributes,
				RemoveJsProtocolFromAttributes = settings.RemoveJsProtocolFromAttributes,
				MinifyEmbeddedCssCode = settings.MinifyEmbeddedCssCode,
				MinifyInlineCssCode = settings.MinifyInlineCssCode,
				MinifyEmbeddedJsCode = settings.MinifyEmbeddedJsCode,
				MinifyInlineJsCode = settings.MinifyInlineJsCode,
				MinifyEmbeddedJsonData = settings.MinifyEmbeddedJsonData,
				MinifyKnockoutBindingExpressions = settings.MinifyKnockoutBindingExpressions,
				MinifyAngularBindingExpressions = settings.MinifyAngularBindingExpressions,
				UseXhtmlSyntax = false
			};
			internalSettings.SetPreservableHtmlComments(settings.PreservableHtmlCommentCollection);
			internalSettings.SetPreservableOptionalTags(settings.PreservableOptionalTagCollection);
			internalSettings.SetPreservableAttributes(settings.PreservableAttributeCollection);
			internalSettings.SetProcessableScriptTypes(settings.ProcessableScriptTypeCollection);
			internalSettings.SetCustomAngularDirectives(settings.CustomAngularDirectiveCollection);

			_genericHtmlMinifier = new GenericHtmlMinifier(internalSettings, cssMinifier, jsMinifier, logger);
		}


		/// <summary>
		/// Minify HTML content
		/// </summary>
		/// <param name="content">HTML content</param>
		/// <returns>Minification result</returns>
		public MarkupMinificationResult Minify(string content)
		{
			return _genericHtmlMinifier.Minify(content);
		}

		/// <summary>
		/// Minify HTML content
		/// </summary>
		/// <param name="content">HTML content</param>
		/// <param name="fileContext">File context</param>
		/// <returns>Minification result</returns>
		public MarkupMinificationResult Minify(string content, string fileContext)
		{
			return _genericHtmlMinifier.Minify(content, fileContext);
		}

		/// <summary>
		/// Minify HTML content
		/// </summary>
		/// <param name="content">HTML content</param>
		/// <param name="encoding">Text encoding</param>
		/// <returns>Minification result</returns>
		public MarkupMinificationResult Minify(string content, Encoding encoding)
		{
			return _genericHtmlMinifier.Minify(content, encoding);
		}

		/// <summary>
		/// Minify HTML content
		/// </summary>
		/// <param name="content">HTML content</param>
		/// <param name="generateStatistics">Flag for whether to allow generate minification statistics</param>
		/// <returns>Minification result</returns>
		public MarkupMinificationResult Minify(string content, bool generateStatistics)
		{
			return _genericHtmlMinifier.Minify(content, generateStatistics);
		}

		/// <summary>
		/// Minify HTML content
		/// </summary>
		/// <param name="content">HTML content</param>
		/// <param name="fileContext">File context</param>
		/// <param name="encoding">Text encoding</param>
		/// <param name="generateStatistics">Flag for whether to allow generate minification statistics</param>
		/// <returns>Minification result</returns>
		public MarkupMinificationResult Minify(string content, string fileContext, Encoding encoding,
			bool generateStatistics)
		{
			return _genericHtmlMinifier.Minify(content, fileContext, encoding, generateStatistics);
		}
	}
}