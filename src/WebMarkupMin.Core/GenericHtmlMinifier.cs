/* This minifier based on the code of Experimental HTML Minifier
 * (http://github.com/kangax/html-minifier/) version 0.4.4.
 */

/* htmlminifier.js
 * May 14, 2012
 *
 * Copyright (c) 2010 Juriy "kangax" Zaytsev (http://github.com/kangax)
 *
 * Licensed under the MIT license.
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:

 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using WebMarkupMin.Core.Helpers;
using WebMarkupMin.Core.Loggers;
using WebMarkupMin.Core.Parsers;
using WebMarkupMin.Core.Resources;
using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// Generic HTML minifier
	/// </summary>
	internal sealed class GenericHtmlMinifier : IMarkupMinifier
	{
		/// <summary>
		/// Average compression ratio
		/// </summary>
		const double AVERAGE_COMPRESSION_RATIO = 0.7;

		/// <summary>
		/// Placeholder for embedded code
		/// </summary>
		const string EMBEDDED_CODE_PLACEHOLDER = "(embedded code)";

		const string HTTP_PROTOCOL = "http:";
		const string HTTPS_PROTOCOL = "https:";
		const string JS_PROTOCOL = "javascript:";

		const string JS_CONTENT_TYPE = "text/javascript";
		const string VBS_CONTENT_TYPE = "text/vbscript";
		const string CSS_CONTENT_TYPE = "text/css";

		#region Regular expressions

		private static readonly Regex _noindexCommentRegex = new Regex(@"^(?<closingSlash>/)?noindex$",
			RegexOptions.IgnoreCase);
		private static readonly Regex _metaContentTypeTagValueRegex =
			new Regex(@"^(?:[a-zA-Z0-9-+./]+);\s*charset=(?<charset>[a-zA-Z0-9-]+)$", RegexOptions.IgnoreCase);
		private static readonly Regex _html4AttributeValueNotRequireQuotesRegex = new Regex(@"^[a-zA-Z0-9-_:.]+$");
		private static readonly Regex _jsProtocolRegex = new Regex(@"^javascript:\s*", RegexOptions.IgnoreCase);
		private static readonly Regex _separatingCommaWithSpacesRegex = new Regex(@"\s*,\s*");
		private static readonly Regex _endingCommaWithSpacesRegex = new Regex(@"\s*,\s*$");

		private static readonly Regex _beginHtmlCommentRegex = new Regex(@"^\s*<!--(?:[ \t\v]*\r?\n)?");
		private static readonly Regex _endHtmlCommentRegex = new Regex(@"(?:\r?\n[ \t\v]*)?-->\s*$",
			RegexOptions.RightToLeft);

		private static readonly Regex _beginCdataSectionRegex = new Regex(
			@"^\s*<!\[CDATA\[(?:[ \t\v]*\r?\n)?");
		private static readonly Regex _endCdataSectionRegex = new Regex(@"(?:\r?\n[ \t\v]*)?\]\]>\s*$",
			RegexOptions.RightToLeft);

		private static readonly Regex _styleBeginCdataSectionRegex = new Regex(
			@"^\s*/\*\s*<!\[CDATA\[\s*\*/(?:[ \t\v]*\r?\n)?");
		private static readonly Regex _styleEndCdataSectionRegex = new Regex(@"(?:\r?\n[ \t\v]*)?/\*\s*\]\]>\s*\*/\s*$",
			RegexOptions.RightToLeft);

		private static readonly Regex _styleBeginMaxCompatibleCdataSectionRegex = new Regex(
			@"^\s*<!--\s*/\*\s*--><!\[CDATA\[\s*/\*\s*><!--\s*\*/(?:[ \t\v]*\r?\n)?");
		private static readonly Regex _styleEndMaxCompatibleCdataSectionRegex = new Regex(
			@"(?:\r?\n[ \t\v]*)?/\*\s*\]\]>\s*\*/\s*-->\s*$", RegexOptions.RightToLeft);

		private static readonly Regex _scriptBeginHtmlCommentRegex = new Regex(
			@"^\s*(?://[ \t\v]*)?<!--[ \t\v\S]*(?:\r?\n)?");
		private static readonly Regex _scriptEndHtmlCommentRegex = new Regex(@"(?:\r?\n)?[ \t\v\S]*-->\s*$",
			RegexOptions.RightToLeft);

		private static readonly Regex _scriptBeginCdataSectionRegex = new Regex(
			@"^\s*(?://[ \t\v]*<!\[CDATA\[[ \t\v\S]*\r?\n|/\*\s*<!\[CDATA\[\s*\*/(?:[ \t\v]*\r?\n)?)");
		private static readonly Regex _scriptEndCdataSectionRegex = new Regex(
			@"(?:\r?\n//[ \t\v\S]*\]\]>|(?:\r?\n[ \t\v]*)?/\*\s*\]\]>\s*\*/)\s*$", RegexOptions.RightToLeft);

		private static readonly Regex _scriptBeginMaxCompatibleCdataSectionRegex = new Regex(
			@"^\s*(?:<!--[ \t\v]*//[ \t\v]*--><!\[CDATA\[[ \t\v]*//[ \t\v]*><!--[ \t\v]*\r?\n" +
			@"|<!--\s*/\*\s*--><!\[CDATA\[\s*/\*\s*><!--\s*\*/(?:[ \t\v]*\r?\n)?)");
		private static readonly Regex _scriptEndMaxCompatibleCdataSectionRegex = new Regex(
			@"(?:\r?\n[ \t\v]*//[ \t\v]*--><!\]\]>" +
			@"|(?:\r?\n[ \t\v]*)?/\*\s*\]\]>\s*\*/\s*-->)\s*$", RegexOptions.RightToLeft);

		private static readonly Regex _relExternalAttributeRegex = new Regex(@"^(?:alternate\s+)?external$",
			RegexOptions.IgnoreCase);

		#endregion

		#region Lists of tags and attributes

		private static readonly HashSet<string> _emptyAttributesForRemoval = new HashSet<string>
		{
			"class", "id", "name", "style", "title", "lang", "dir"
		};

		private static readonly HashSet<string> _tagsWithNotRemovableWhitespace = new HashSet<string>
		{
			"pre", "textarea"
		};

		private static readonly HashSet<string> _unremovableEmptyTags = new HashSet<string>
		{
			"textarea", "tr", "th", "td"
		};

		private static readonly HashSet<string> _unremovableEmptyTagAttributes = new HashSet<string>
		{
			"class", "id", "name", "role", "src"
		};

		private static readonly HashSet<string> _safeOptionalEndTags = new HashSet<string>
		{
			"html", "head", "body", "colgroup"
		};

		private static readonly HashSet<string> _tagsFollowingAfterParagraphOptionalEndTag = new HashSet<string>
		{
			"address", "article", "aside",
			"blockquote",
			"div", "dl",
			"fieldset", "footer", "form",
			"h1", "h2", "h3", "h4", "h5", "h6", "header", "hgroup", "hr",
			"main",
			"nav",
			"ol",
			"p", "pre",
			"section",
			"table",
			"ul"
		};

		#endregion

		/// <summary>
		/// List of JavaScript content types
		/// </summary>
		private static readonly HashSet<string> _jsContentTypes = new HashSet<string>
		{
			"text/javascript", "text/ecmascript", "application/javascript", "application/ecmascript"
		};

		/// <summary>
		/// List of names of built-in Angular directives, that contain expressions
		/// </summary>
		private static readonly HashSet<string> _builtinAngularDirectivesWithExpressions = new HashSet<string>
		{
			"ngBind", "ngBindHtml", "ngBlur",
			"ngChange", "ngChecked", "ngClass", "ngClassEven", "ngClassOdd", "ngClick", "ngController", "ngCopy", "ngCut",
			"ngDblclick", "ngDisabled",
			"ngFocus",
			"ngHide",
			"ngIf", "ngInit",
			"ngKeydown", "ngKeypress", "ngKeyup",
			"ngMaxlength", "ngMessages", "ngMinlength", "ngModel", "ngModelOptions", "ngMousedown", "ngMouseenter",
			"ngMouseleave", "ngMousemove", "ngMouseover", "ngMouseup",
			"ngOpen", "ngOptions",
			"ngPaste", "ngPattern",
			"ngReadonly", "ngRepeat", "ngRepeatStart", "ngRequired",
			"ngSelected", "ngShow", "ngStyle", "ngSubmit", "ngSwipeLeft", "ngSwipeRight", "ngSwitch",
			"ngValue"
		};

		/// <summary>
		/// Generic HTML minification settings
		/// </summary>
		private readonly GenericHtmlMinificationSettings _settings;

		/// <summary>
		/// CSS minifier
		/// </summary>
		private readonly ICssMinifier _cssMinifier;

		/// <summary>
		/// JS minifier
		/// </summary>
		private readonly IJsMinifier _jsMinifier;

		/// <summary>
		/// Logger
		/// </summary>
		private readonly ILogger _logger;

		/// <summary>
		/// HTML parser
		/// </summary>
		private readonly HtmlParser _htmlParser;

		/// <summary>
		/// Inner HTML minifier
		/// </summary>
		private GenericHtmlMinifier _innerHtmlMinifier;

		/// <summary>
		/// Flag that indicates if the inner HTML minifier is initialized
		/// </summary>
		private InterlockedStatedFlag _innerHtmlMinifierInitializedFlag = new InterlockedStatedFlag();

		/// <summary>
		/// Inner XML minifier
		/// </summary>
		private XmlMinifier _innerXmlMinifier;

		/// <summary>
		/// Flag that indicates if the inner XML minifier is initialized
		/// </summary>
		private InterlockedStatedFlag _innerXmlMinifierInitializedFlag = new InterlockedStatedFlag();

		/// <summary>
		/// Inner Douglas Crockford's JS minifier
		/// </summary>
		private readonly Lazy<CrockfordJsMinifier> _innerCrockfordJsMinifier = new Lazy<CrockfordJsMinifier>();

		/// <summary>
		/// File context
		/// </summary>
		private string _fileContext;

		/// <summary>
		/// Text encoding
		/// </summary>
		private Encoding _encoding;

		/// <summary>
		/// Result of HTML minification
		/// </summary>
		private StringBuilder _result;

		/// <summary>
		/// HTML minification buffer
		/// </summary>
		private readonly List<string> _buffer;

		private readonly Queue<string> _tagsWithNotRemovableWhitespaceQueue;

		/// <summary>
		/// Current node type
		/// </summary>
		private HtmlNodeType _currentNodeType;

		/// <summary>
		/// Current tag
		/// </summary>
		private HtmlTag _currentTag;

		/// <summary>
		/// Current text
		/// </summary>
		private string _currentText;

		/// <summary>
		/// List of the errors
		/// </summary>
		private readonly IList<MinificationErrorInfo> _errors;

		/// <summary>
		/// List of the warnings
		/// </summary>
		private readonly IList<MinificationErrorInfo> _warnings;

		/// <summary>
		/// Synchronizer of minification
		/// </summary>
		private readonly object _minificationSynchronizer = new object();

		/// <summary>
		/// List of names of Angular directives, that contain expressions
		/// </summary>
		private readonly HashSet<string> _angularDirectivesWithExpressions;


		/// <summary>
		/// Constructs instance of generic HTML minifier
		/// </summary>
		/// <param name="settings">Generic HTML minification settings</param>
		/// <param name="cssMinifier">CSS minifier</param>
		/// <param name="jsMinifier">JS minifier</param>
		/// <param name="logger">Logger</param>
		public GenericHtmlMinifier(GenericHtmlMinificationSettings settings = null,
			ICssMinifier cssMinifier = null, IJsMinifier jsMinifier = null, ILogger logger = null)
		{
			_settings = settings ?? new GenericHtmlMinificationSettings();
			_logger = logger ?? new NullLogger();
			_cssMinifier = cssMinifier ?? new KristensenCssMinifier();
			_jsMinifier = jsMinifier ?? new CrockfordJsMinifier();
			_htmlParser = new HtmlParser(new HtmlParsingHandlers
			{
				XmlDeclaration = XmlDeclarationHandler,
				Doctype = DoctypeHandler,
				Comment = CommentHandler,
				IfConditionalComment = IfConditionalCommentHandler,
				EndIfConditionalComment = EndIfConditionalCommentHandler,
				CdataSection = CdataSectionHandler,
				StartTag = StartTagHandler,
				EndTag = EndTagHandler,
				Text = TextHandler,
				EmbeddedCode = EmbeddedCodeHandler,
				TemplateTag = TemplateTagHandler,
				IgnoredFragment = IgnoredFragmentHandler
			});

			_buffer = new List<string>();
			_errors = new List<MinificationErrorInfo>();
			_warnings = new List<MinificationErrorInfo>();
			_tagsWithNotRemovableWhitespaceQueue = new Queue<string>();
			_currentNodeType = HtmlNodeType.Unknown;
			_currentTag = null;
			_currentText = string.Empty;

			ISet<string> customAngularDirectivesWithExpressions = _settings.CustomAngularDirectiveCollection;
			_angularDirectivesWithExpressions = customAngularDirectivesWithExpressions.Count > 0 ?
				Utils.UnionHashSets(_builtinAngularDirectivesWithExpressions, customAngularDirectivesWithExpressions)
				:
				_builtinAngularDirectivesWithExpressions
				;
		}


		/// <summary>
		/// Gets a instance of inner HTML minifier
		/// </summary>
		/// <returns>Instance of inner HTML minifier</returns>
		private GenericHtmlMinifier GetInnerHtmlMinifierInstance()
		{
			if (_innerHtmlMinifierInitializedFlag.Set())
			{
				_innerHtmlMinifier = new GenericHtmlMinifier(_settings, new NullCssMinifier(), new NullJsMinifier(),
					new NullLogger());
			}

			return _innerHtmlMinifier;
		}

		/// <summary>
		/// Gets a instance of inner XML minifier
		/// </summary>
		/// <returns>Instance of inner XML minifier</returns>
		private XmlMinifier GetInnerXmlMinifierInstance()
		{
			if (_innerXmlMinifierInitializedFlag.Set())
			{
				_innerXmlMinifier = new XmlMinifier(new XmlMinificationSettings
				{
					MinifyWhitespace = _settings.WhitespaceMinificationMode != WhitespaceMinificationMode.None,
					RemoveXmlComments = _settings.RemoveHtmlComments,
					RenderEmptyTagsWithSpace = _settings.EmptyTagRenderMode != HtmlEmptyTagRenderMode.Slash
				});
			}

			return _innerXmlMinifier;
		}

		/// <summary>
		/// Gets a instance of inner Douglas Crockford's JS minifier
		/// </summary>
		/// <returns>Instance of inner Douglas Crockford's JS minifier</returns>
		private CrockfordJsMinifier GetInnerCrockfordJsMinifierInstance()
		{
			return _innerCrockfordJsMinifier.Value;
		}

		/// <summary>
		/// Minify HTML content
		/// </summary>
		/// <param name="content">HTML content</param>
		/// <returns>Minification result</returns>
		public MarkupMinificationResult Minify(string content)
		{
			return Minify(content, string.Empty, Encoding.GetEncoding(0), false);
		}

		/// <summary>
		/// Minify HTML content
		/// </summary>
		/// <param name="content">HTML content</param>
		/// <param name="fileContext">File context</param>
		/// <returns>Minification result</returns>
		public MarkupMinificationResult Minify(string content, string fileContext)
		{
			return Minify(content, fileContext, Encoding.GetEncoding(0), false);
		}

		/// <summary>
		/// Minify HTML content
		/// </summary>
		/// <param name="content">HTML content</param>
		/// <param name="encoding">Text encoding</param>
		/// <returns>Minification result</returns>
		public MarkupMinificationResult Minify(string content, Encoding encoding)
		{
			return Minify(content, string.Empty, encoding, false);
		}

		/// <summary>
		/// Minify HTML content
		/// </summary>
		/// <param name="content">HTML content</param>
		/// <param name="generateStatistics">Flag for whether to allow generate minification statistics</param>
		/// <returns>Minification result</returns>
		public MarkupMinificationResult Minify(string content, bool generateStatistics)
		{
			return Minify(content, string.Empty, Encoding.GetEncoding(0), generateStatistics);
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
			MinificationStatistics statistics = null;
			string cleanedContent = Utils.RemoveByteOrderMark(content);
			string minifiedContent = string.Empty;
			var errors = new List<MinificationErrorInfo>();
			var warnings = new List<MinificationErrorInfo>();

			lock (_minificationSynchronizer)
			{
				_fileContext = fileContext;
				_encoding = encoding;

				try
				{
					if (generateStatistics)
					{
						statistics = new MinificationStatistics(_encoding);
						statistics.Init(cleanedContent);
					}

					int estimatedCapacity = (int)Math.Floor(cleanedContent.Length * AVERAGE_COMPRESSION_RATIO);
					_result = StringBuilderPool.GetBuilder(estimatedCapacity);

					_htmlParser.Parse(cleanedContent);

					FlushBuffer();

					if (_errors.Count == 0)
					{
						minifiedContent = _result.ToString();

						if (generateStatistics)
						{
							statistics.End(minifiedContent);
						}
					}
				}
				catch (MarkupParsingException e)
				{
					WriteError(LogCategoryConstants.HtmlParsingError, e.Message, _fileContext,
						e.LineNumber, e.ColumnNumber, e.SourceFragment);
				}
				finally
				{
					StringBuilderPool.ReleaseBuilder(_result);
					_buffer.Clear();
					_tagsWithNotRemovableWhitespaceQueue.Clear();
					_currentNodeType = HtmlNodeType.Unknown;
					_currentTag = null;
					_currentText = string.Empty;

					errors.AddRange(_errors);
					warnings.AddRange(_warnings);

					_errors.Clear();
					_warnings.Clear();
					_fileContext = null;
					_encoding = null;
				}
			}

			if (errors.Count == 0)
			{
				_logger.Info(LogCategoryConstants.HtmlMinificationSuccess,
					string.Format(Strings.SuccesMessage_MarkupMinificationComplete, "HTML"),
					fileContext, statistics);
			}

			return new MarkupMinificationResult(minifiedContent, errors, warnings, statistics);
		}

		#region Handlers

		/// <summary>
		/// XML declaration handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="xmlDeclaration">XML declaration</param>
		private void XmlDeclarationHandler(MarkupParsingContext context, string xmlDeclaration)
		{
			_currentNodeType = HtmlNodeType.XmlDeclaration;

			WhitespaceMinificationMode whitespaceMinificationMode = _settings.WhitespaceMinificationMode;
			if (whitespaceMinificationMode != WhitespaceMinificationMode.None)
			{
				// Processing of whitespace, that followed before the document type declaration
				TrimEndLastBufferItem();
			}

			if (_settings.UseXhtmlSyntax)
			{
				XmlMinifier innerXmlMinifier = GetInnerXmlMinifierInstance();
				MarkupMinificationResult minificationResult = innerXmlMinifier.Minify(xmlDeclaration);

				if (minificationResult.Errors.Count == 0)
				{
					_buffer.Add(minificationResult.MinifiedContent);
				}
				else
				{
					string sourceCode = context.SourceCode;
					var documentCoordinates = context.NodeCoordinates;

					foreach (MinificationErrorInfo error in minificationResult.Errors)
					{
						var xmlNodeCoordinates = new SourceCodeNodeCoordinates(error.LineNumber, error.ColumnNumber);
						var absoluteNodeCoordinates = SourceCodeNavigator.CalculateAbsoluteNodeCoordinates(
							documentCoordinates, xmlNodeCoordinates);

						string sourceFragment = SourceCodeNavigator.GetSourceFragment(
							sourceCode, absoluteNodeCoordinates);
						string message = Strings.ErrorMessage_XmlDeclarationMinificationFailed;

						WriteError(LogCategoryConstants.HtmlMinificationError, message, _fileContext,
							absoluteNodeCoordinates.LineNumber, absoluteNodeCoordinates.ColumnNumber, sourceFragment);
					}
				}
			}
			else
			{
				string sourceCode = context.SourceCode;
				SourceCodeNodeCoordinates xmlDeclarationCoordinates = context.NodeCoordinates;

				WriteWarning(LogCategoryConstants.HtmlMinificationWarning,
					Strings.WarningMessage_XmlDeclarationNotAllowed, _fileContext,
					xmlDeclarationCoordinates.LineNumber, xmlDeclarationCoordinates.ColumnNumber,
					SourceCodeNavigator.GetSourceFragment(sourceCode, xmlDeclarationCoordinates));
			}
		}

		/// <summary>
		/// Document type declaration handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="doctype">Document type declaration</param>
		private void DoctypeHandler(MarkupParsingContext context, string doctype)
		{
			_currentNodeType = HtmlNodeType.Doctype;

			WhitespaceMinificationMode whitespaceMinificationMode = _settings.WhitespaceMinificationMode;
			if (whitespaceMinificationMode != WhitespaceMinificationMode.None)
			{
				// Processing of whitespace, that followed before the document type declaration
				TrimEndLastBufferItem();
			}

			_buffer.Add(_settings.UseShortDoctype ? "<!DOCTYPE html>" : Utils.CollapseWhitespace(doctype));
		}

		/// <summary>
		/// Comments handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="commentText">Comment text</param>
		private void CommentHandler(MarkupParsingContext context, string commentText)
		{
			HtmlNodeType previousNodeType = _currentNodeType;
			_currentNodeType = HtmlNodeType.Comment;

			const int beginCommentLength = 4;
			string processedCommentText;
			bool removeComment = false;

			if (_noindexCommentRegex.IsMatch(commentText))
			{
				// Processing of noindex comment
				Match noindexCommentMatch = _noindexCommentRegex.Match(commentText);
				processedCommentText = noindexCommentMatch.Groups["closingSlash"].Success ? "/noindex" : "noindex";
			}
			else if (KnockoutHelpers.IsEndContainerlessComment(commentText))
			{
				// Processing of end Knockout containerless comment
				processedCommentText = "/ko";
			}
			else if (KnockoutHelpers.IsBeginContainerlessComment(commentText))
			{
				// Processing of start Knockout containerless comment
				string koExpression = string.Empty;

				KnockoutHelpers.ParseBeginContainerlessComment(commentText,
					(localContext, expression) =>
					{
						SourceCodeNodeCoordinates expressionCoordinates = localContext.NodeCoordinates;
						expressionCoordinates.ColumnNumber += beginCommentLength;

						koExpression = _settings.MinifyKnockoutBindingExpressions ?
							MinifyKnockoutBindingExpression(context, expressionCoordinates, expression) : expression;
					}
				);

				processedCommentText = "ko " + koExpression;
			}
			else if (AngularHelpers.IsCommentDirective(commentText))
			{
				// Processing of Angular comment directive
				string ngOriginalDirectiveName = string.Empty;
				string ngNormalizedDirectiveName = string.Empty;
				string ngExpression = string.Empty;

				AngularHelpers.ParseCommentDirective(commentText,
					(localContext, originalDirectiveName, normalizedDirectiveName) =>
					{
						ngOriginalDirectiveName = originalDirectiveName;
						ngNormalizedDirectiveName = normalizedDirectiveName;
					},
					(localContext, expression) =>
					{
						SourceCodeNodeCoordinates expressionCoordinates = localContext.NodeCoordinates;
						expressionCoordinates.ColumnNumber += beginCommentLength;

						ngExpression = expression;
						if (_settings.MinifyAngularBindingExpressions
							&& ContainsAngularBindingExpression(ngNormalizedDirectiveName))
						{
							ngExpression = MinifyAngularBindingExpression(context, SourceCodeNodeCoordinates.Empty,
								expressionCoordinates, expression);
						}
					}
				);

				processedCommentText = "directive:" + ngOriginalDirectiveName + " " + ngExpression;
			}
			else if (ReactHelpers.IsDomComponentComment(commentText))
			{
				processedCommentText = commentText;
			}
			else
			{
				processedCommentText = commentText;
				removeComment = _settings.RemoveHtmlComments;
			}

			if (!removeComment)
			{
				_buffer.Add("<!--");
				if (processedCommentText.Length > 0)
				{
					_buffer.Add(processedCommentText);
				}
				_buffer.Add("-->");
			}
			else
			{
				_currentNodeType = previousNodeType;
			}
		}

		/// <summary>
		/// CDATA sections handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="cdataText">CDATA text</param>
		private void CdataSectionHandler(MarkupParsingContext context, string cdataText)
		{
			_currentNodeType = HtmlNodeType.CdataSection;

			_buffer.Add("<![CDATA[");
			_buffer.Add(cdataText);
			_buffer.Add("]]>");
		}

		/// <summary>
		/// If conditional comments handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="htmlConditionalComment">Conditional comment</param>
		private void IfConditionalCommentHandler(MarkupParsingContext context,
			HtmlConditionalComment htmlConditionalComment)
		{
			_currentNodeType = HtmlNodeType.IfConditionalComment;
			HtmlConditionalCommentType htmlConditionalCommentType = htmlConditionalComment.Type;

			string startPart;
			string endPart;

			switch (htmlConditionalCommentType)
			{
				case HtmlConditionalCommentType.Hidden:
					startPart = "<!--[if ";
					endPart = "]>";

					break;
				case HtmlConditionalCommentType.RevealedValidating:
					startPart = "<!--[if ";
					endPart = "]><!-->";

					break;
				case HtmlConditionalCommentType.RevealedValidatingSimplified:
					startPart = "<!--[if ";
					endPart = "]>-->";

					break;
				case HtmlConditionalCommentType.Revealed:
					startPart = "<![if ";
					endPart = "]>";

					break;
				default:
					throw new NotSupportedException();
			}

			_buffer.Add(startPart);
			_buffer.Add(htmlConditionalComment.Expression);
			_buffer.Add(endPart);
		}

		/// <summary>
		/// End If conditional comments handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="type">End If conditional comment type</param>
		private void EndIfConditionalCommentHandler(MarkupParsingContext context, HtmlConditionalCommentType type)
		{
			_currentNodeType = HtmlNodeType.EndIfConditionalComment;

			string endIfComment;

			switch (type)
			{
				case HtmlConditionalCommentType.Hidden:
					endIfComment = "<![endif]-->";
					break;
				case HtmlConditionalCommentType.RevealedValidating:
				case HtmlConditionalCommentType.RevealedValidatingSimplified:
					endIfComment = "<!--<![endif]-->";
					break;
				case HtmlConditionalCommentType.Revealed:
					endIfComment = "<![endif]>";
					break;
				default:
					throw new NotSupportedException();
			}

			_buffer.Add(endIfComment);
		}

		/// <summary>
		/// Start tags handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="tag">HTML tag</param>
		private void StartTagHandler(MarkupParsingContext context, HtmlTag tag)
		{
			HtmlNodeType previousNodeType = _currentNodeType;
			HtmlTag previousTag = _currentTag ?? HtmlTag.Empty;

			if (_settings.UseMetaCharsetTag && IsMetaContentTypeTag(tag))
			{
				tag = UpgradeToMetaCharsetTag(tag);
			}

			string tagName = tag.Name;
			string tagNameInLowercase = tag.NameInLowercase;
			HtmlTagFlags tagFlags = tag.Flags;
			IList<HtmlAttribute> attributes = tag.Attributes;

			_currentNodeType = HtmlNodeType.StartTag;
			_currentTag = tag;
			_currentText = string.Empty;

			// Set whitespace flags for nested tags (for example <span> within a <pre>)
			WhitespaceMinificationMode whitespaceMinificationMode = _settings.WhitespaceMinificationMode;
			if (whitespaceMinificationMode != WhitespaceMinificationMode.None)
			{
				if (_tagsWithNotRemovableWhitespaceQueue.Count == 0)
				{
					// Processing of whitespace, that followed before the start tag
					bool allowTrimEnd = false;
					if (tagFlags.HasFlag(HtmlTagFlags.Invisible)
						|| (tagFlags.HasFlag(HtmlTagFlags.NonIndependent)
							&& CanRemoveWhitespaceBetweenNonIndependentTags(previousTag, tag)))
					{
						allowTrimEnd = true;
					}
					else
					{
						if (whitespaceMinificationMode == WhitespaceMinificationMode.Medium
							|| whitespaceMinificationMode == WhitespaceMinificationMode.Aggressive)
						{
							allowTrimEnd = tagFlags.HasFlag(HtmlTagFlags.Block);
						}
					}

					if (allowTrimEnd)
					{
						TrimEndLastBufferItem();
					}
				}

				if (!CanMinifyWhitespace(tag))
				{
					_tagsWithNotRemovableWhitespaceQueue.Enqueue(tagNameInLowercase);
				}
			}

			if (previousNodeType != HtmlNodeType.StartTag)
			{
				if (_settings.RemoveOptionalEndTags
					&& previousTag.Flags.HasFlag(HtmlTagFlags.Optional)
					&& CanRemoveOptionalEndTagByNextTag(previousTag, tag))
				{
					RemoveLastEndTagFromBuffer(previousTag);
				}

				FlushBuffer();
			}

			_buffer.Add("<");
			_buffer.Add(CanPreserveCase() ? tagName : tagNameInLowercase);

			int attributeCount = attributes.Count;
			bool unsafeLastAttribute = false;

			if (attributeCount > 0)
			{
				for (int attributeIndex = 0; attributeIndex < attributeCount; attributeIndex++)
				{
					HtmlAttributeViewModel attributeViewModel = BuildAttributeViewModel(context, tag, attributes[attributeIndex]);
					if (!attributeViewModel.IsEmpty)
					{
						_buffer.Add(" ");
						_buffer.Add(attributeViewModel.Name);
						if (attributeViewModel.HasValue)
						{
							_buffer.Add("=");
							if (attributeViewModel.HasQuotes)
							{
								_buffer.Add("\"");
							}
							_buffer.Add(attributeViewModel.Value);
							if (attributeViewModel.HasQuotes)
							{
								_buffer.Add("\"");
							}
						}

						unsafeLastAttribute = attributeViewModel.HasValue && !attributeViewModel.HasQuotes;
					}
				}
			}

			if (tagFlags.HasFlag(HtmlTagFlags.Empty))
			{
				HtmlEmptyTagRenderMode emptyTagRenderMode = _settings.EmptyTagRenderMode;

				if (emptyTagRenderMode == HtmlEmptyTagRenderMode.NoSlash && tagFlags.HasFlag(HtmlTagFlags.Xml))
				{
					emptyTagRenderMode = HtmlEmptyTagRenderMode.SpaceAndSlash;
				}

				if (emptyTagRenderMode == HtmlEmptyTagRenderMode.Slash && unsafeLastAttribute)
				{
					emptyTagRenderMode = HtmlEmptyTagRenderMode.SpaceAndSlash;
				}

				if (emptyTagRenderMode == HtmlEmptyTagRenderMode.Slash)
				{
					_buffer.Add("/");
				}
				else if (emptyTagRenderMode == HtmlEmptyTagRenderMode.SpaceAndSlash)
				{
					_buffer.Add(" /");
				}
			}
			_buffer.Add(">");
		}

		/// <summary>
		/// End tags handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="tag">HTML tag</param>
		private void EndTagHandler(MarkupParsingContext context, HtmlTag tag)
		{
			HtmlNodeType previousNodeType = _currentNodeType;
			HtmlTag previousTag = _currentTag ?? HtmlTag.Empty;
			string previousTagNameInLowercase = previousTag.NameInLowercase;
			string previousText = _currentText;

			_currentNodeType = HtmlNodeType.EndTag;
			_currentTag = tag;
			_currentText = string.Empty;

			string tagName = tag.Name;
			string tagNameInLowercase = tag.NameInLowercase;
			HtmlTagFlags tagFlags = tag.Flags;

			WhitespaceMinificationMode whitespaceMinificationMode = _settings.WhitespaceMinificationMode;
			if (whitespaceMinificationMode != WhitespaceMinificationMode.None)
			{
				if (_tagsWithNotRemovableWhitespaceQueue.Count == 0 && !tagFlags.HasFlag(HtmlTagFlags.EmbeddedCode))
				{
					// Processing of whitespace, that followed before the end tag
					bool allowTrimEnd = false;
					if (tagFlags.HasFlag(HtmlTagFlags.Invisible)
						|| (previousTag.Flags.HasFlag(HtmlTagFlags.NonIndependent)
							&& CanRemoveWhitespaceAfterEndNonIndependentTagByParentTag(previousTag, tag)))
					{
						allowTrimEnd = true;
					}
					else
					{
						if (whitespaceMinificationMode == WhitespaceMinificationMode.Medium)
						{
							allowTrimEnd = tagFlags.HasFlag(HtmlTagFlags.Block);
						}
						else if (whitespaceMinificationMode == WhitespaceMinificationMode.Aggressive)
						{
							allowTrimEnd = tagFlags.HasFlag(HtmlTagFlags.Block)
								|| tagFlags.HasFlag(HtmlTagFlags.Inline)
								|| tagFlags.HasFlag(HtmlTagFlags.InlineBlock)
								;
						}
					}

					if (allowTrimEnd)
					{
						TrimEndLastBufferItem();
					}
				}

				// Check if current tag is in a whitespace queue
				if (_tagsWithNotRemovableWhitespaceQueue.Count > 0
					&& tagNameInLowercase == _tagsWithNotRemovableWhitespaceQueue.Last())
				{
					_tagsWithNotRemovableWhitespaceQueue.Dequeue();
				}
			}

			if (_settings.RemoveOptionalEndTags
				&& previousTag.Flags.HasFlag(HtmlTagFlags.Optional)
				&& (previousNodeType == HtmlNodeType.EndTag
					|| (previousTagNameInLowercase != tagNameInLowercase && string.IsNullOrWhiteSpace(previousText)))
				&& CanRemoveOptionalEndTagByParentTag(previousTag, tag))
			{
				RemoveLastEndTagFromBuffer(previousTag);
			}

			bool isElementEmpty = string.IsNullOrWhiteSpace(previousText)
				&& previousTagNameInLowercase == tagNameInLowercase
				&& previousNodeType != HtmlNodeType.EndTag;
			if (_settings.RemoveTagsWithoutContent && isElementEmpty
				&& CanRemoveTagWithoutContent(previousTag))
			{
				// Remove last "element" from buffer, return
				if (RemoveLastStartTagFromBuffer(tag))
				{
					FlushBuffer();
					return;
				}
			}

			if (_settings.RemoveOptionalEndTags
				&& tagFlags.HasFlag(HtmlTagFlags.Optional)
				&& CanRemoveSafeOptionalEndTag(tag))
			{
				// Leave only start tag in buffer
				FlushBuffer();
				return;
			}

			// Add end tag to buffer
			_buffer.Add("</");
			_buffer.Add(CanPreserveCase() ? tagName : tagNameInLowercase);
			_buffer.Add(">");
		}

		/// <summary>
		/// Text handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="text">Text</param>
		private void TextHandler(MarkupParsingContext context, string text)
		{
			HtmlNodeType previousNodeType = _currentNodeType;
			HtmlTag tag = _currentTag ?? HtmlTag.Empty;
			string tagNameInLowercase = tag.NameInLowercase;
			HtmlTagFlags tagFlags = tag.Flags;

			_currentNodeType = HtmlNodeType.Text;

			WhitespaceMinificationMode whitespaceMinificationMode = _settings.WhitespaceMinificationMode;
			if (whitespaceMinificationMode != WhitespaceMinificationMode.None)
			{
				if (_tagsWithNotRemovableWhitespaceQueue.Count == 0)
				{
					if (context.Position == 0)
					{
						// Processing of starting whitespace
						text = text.TrimStart();
					}
					else if (context.Position + text.Length == context.Length)
					{
						// Processing of ending whitespace
						text = text.TrimEnd();
					}
					else if (previousNodeType == HtmlNodeType.StartTag)
					{
						// Processing of whitespace, that followed after the start tag
						bool allowTrimStart = false;
						if (tagFlags.HasFlag(HtmlTagFlags.Invisible)
							|| (tagFlags.HasFlag(HtmlTagFlags.NonIndependent) && tagFlags.HasFlag(HtmlTagFlags.Empty)))
						{
							allowTrimStart = true;
						}
						else
						{
							if (whitespaceMinificationMode == WhitespaceMinificationMode.Medium)
							{
								allowTrimStart = tagFlags.HasFlag(HtmlTagFlags.Block);
							}
							else if (whitespaceMinificationMode == WhitespaceMinificationMode.Aggressive)
							{
								allowTrimStart = tagFlags.HasFlag(HtmlTagFlags.Block)
									|| ((tagFlags.HasFlag(HtmlTagFlags.Inline) || tagFlags.HasFlag(HtmlTagFlags.InlineBlock))
										&& !tagFlags.HasFlag(HtmlTagFlags.Empty));
							}
						}

						if (allowTrimStart)
						{
							text = text.TrimStart();
						}
					}
					else if (previousNodeType == HtmlNodeType.EndTag)
					{
						// Processing of whitespace, that followed after the end tag
						bool allowTrimStart = false;
						if (tagFlags.HasFlag(HtmlTagFlags.Invisible)
							|| (tagFlags.HasFlag(HtmlTagFlags.NonIndependent)
								&& CanRemoveWhitespaceAfterEndNonIndependentTag(tag)))
						{
							allowTrimStart = true;
						}
						else
						{
							if (whitespaceMinificationMode == WhitespaceMinificationMode.Medium
								|| whitespaceMinificationMode == WhitespaceMinificationMode.Aggressive)
							{
								allowTrimStart = tagFlags.HasFlag(HtmlTagFlags.Block);
							}
						}

						if (allowTrimStart)
						{
							text = text.TrimStart();
						}
					}
					else if (previousNodeType == HtmlNodeType.Doctype || previousNodeType == HtmlNodeType.XmlDeclaration)
					{
						// Processing of whitespace, that followed after the document type declaration
						// or XML declaration
						text = text.TrimStart();
					}

					if (text.Length > 0
						&& !(tagFlags.HasFlag(HtmlTagFlags.Xml) && tagFlags.HasFlag(HtmlTagFlags.NonIndependent)))
					{
						text = Utils.CollapseWhitespace(text);
					}
				}
				else if (previousNodeType == HtmlNodeType.StartTag && tagNameInLowercase == "textarea"
					&& string.IsNullOrWhiteSpace(text))
				{
					text = string.Empty;
				}
			}

			if (text.Length > 0)
			{
				_buffer.Add(text);
			}
			else
			{
				_currentNodeType = previousNodeType;
			}

			_currentText = text;
		}

		/// <summary>
		/// Embedded code handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="code">Code</param>
		private void EmbeddedCodeHandler(MarkupParsingContext context, string code)
		{
			HtmlNodeType previousNodeType = _currentNodeType;
			HtmlTag tag = _currentTag ?? HtmlTag.Empty;
			string tagNameInLowercase = tag.NameInLowercase;
			IList<HtmlAttribute> attributes = tag.Attributes;

			_currentNodeType = HtmlNodeType.EmbeddedCode;

			string contentType = attributes
				.Where(a => a.NameInLowercase == "type")
				.Select(a => a.Value)
				.FirstOrDefault()
				;

			switch (tagNameInLowercase)
			{
				case "script":
					if (string.IsNullOrWhiteSpace(contentType))
					{
						string language = attributes
							.Where(a => a.NameInLowercase == "language")
							.Select(a => a.Value)
							.FirstOrDefault()
							;

						if (!string.IsNullOrWhiteSpace(language)
							&& language.Trim().IgnoreCaseEquals("vbscript"))
						{
							contentType = VBS_CONTENT_TYPE;
						}
					}

					ProcessEmbeddedScriptContent(context, code, contentType);
					break;

				case "style":
					ProcessEmbeddedStyleContent(context, code, contentType);
					break;

				default:
					throw new NotSupportedException();
			}

			if (_currentText.Length == 0)
			{
				_currentNodeType = previousNodeType;
			}
		}

		/// <summary>
		/// Template tags handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="expression">Expression</param>
		/// <param name="startDelimiter">Start delimiter</param>
		/// <param name="endDelimiter">End delimiter</param>
		private void TemplateTagHandler(MarkupParsingContext context, string expression, string startDelimiter,
			string endDelimiter)
		{
			_currentNodeType = HtmlNodeType.TemplateTag;

			string processedExpression = expression;
			if (_settings.MinifyAngularBindingExpressions && startDelimiter == "{{" && endDelimiter == "}}")
			{
				processedExpression = MinifyAngularBindingExpression(context, expression);
			}

			_buffer.Add(startDelimiter);
			_buffer.Add(processedExpression);
			_buffer.Add(endDelimiter);
		}

		/// <summary>
		/// Ignored fragments handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="fragment">Ignored fragment</param>
		private void IgnoredFragmentHandler(MarkupParsingContext context, string fragment)
		{
			_currentNodeType = HtmlNodeType.IgnoredFragment;

			if (fragment.Length > 0)
			{
				_buffer.Add(fragment);
				FlushBuffer();
			}
		}

		#endregion

		#region Buffer helpers

		/// <summary>
		/// Flush a HTML minification buffer
		/// </summary>
		private void FlushBuffer()
		{
			int bufferItemCount = _buffer.Count;

			if (bufferItemCount > 0)
			{
				for (int bufferItemIndex = 0; bufferItemIndex < bufferItemCount; bufferItemIndex++)
				{
					string bufferItem = _buffer[bufferItemIndex];

					if (bufferItem.Length > 0)
					{
						_result.Append(bufferItem);
					}
				}

				_buffer.Clear();
			}
		}

		/// <summary>
		/// Removes all ending spaces in the last item of the HTML minification buffer
		/// </summary>
		private void TrimEndLastBufferItem()
		{
			int bufferItemCount = _buffer.Count;
			if (bufferItemCount > 0)
			{
				for (int bufferItemIndex = bufferItemCount - 1; bufferItemIndex >= 0; bufferItemIndex--)
				{
					string bufferItem = _buffer[bufferItemIndex];

					if (string.IsNullOrWhiteSpace(bufferItem))
					{
						_buffer.RemoveAt(bufferItemIndex);
					}
					else
					{
						_buffer[bufferItemIndex] = bufferItem.TrimEnd();
						break;
					}
				}
			}
		}

		/// <summary>
		/// Removes a last end tag from the HTML minification buffer
		/// </summary>
		/// <param name="endTag">End tag</param>
		private void RemoveLastEndTagFromBuffer(HtmlTag endTag)
		{
			int bufferItemCount = _buffer.Count;
			if (bufferItemCount == 0)
			{
				return;
			}

			int lastEndTagBeginAngleBracketIndex = _buffer.LastIndexOf("</");

			if (lastEndTagBeginAngleBracketIndex != -1)
			{
				string lastEndTagName = _buffer[lastEndTagBeginAngleBracketIndex + 1];
				if (lastEndTagName.IgnoreCaseEquals(endTag.NameInLowercase))
				{
					int lastEndTagEndAngleBracketIndex = _buffer.IndexOf(">", lastEndTagBeginAngleBracketIndex);
					if (lastEndTagEndAngleBracketIndex != -1)
					{
						int lastBufferItemIndex = bufferItemCount - 1;
						bool noMoreContent = true;
						if (lastEndTagEndAngleBracketIndex != lastBufferItemIndex)
						{
							for (int bufferItemIndex = lastEndTagEndAngleBracketIndex + 1;
								bufferItemIndex < bufferItemCount;
								bufferItemIndex++)
							{
								if (!string.IsNullOrWhiteSpace(_buffer[bufferItemIndex]))
								{
									noMoreContent = false;
									break;
								}
							}
						}

						if (noMoreContent)
						{
							int endTagLength = lastEndTagEndAngleBracketIndex - lastEndTagBeginAngleBracketIndex + 1;
							_buffer.RemoveRange(lastEndTagBeginAngleBracketIndex, endTagLength);
						}
					}
				}
			}
		}

		/// <summary>
		/// Removes a last start tag from the HTML minification buffer
		/// </summary>
		/// <param name="startTag">Start tag</param>
		/// <returns>Result of removing (true - has removed; false - has not removed)</returns>
		private bool RemoveLastStartTagFromBuffer(HtmlTag startTag)
		{
			int bufferItemCount = _buffer.Count;
			if (bufferItemCount == 0)
			{
				return false;
			}

			bool isEndTagRemoved = false;
			int lastStartTagBeginAngleBracketIndex = _buffer.LastIndexOf("<");

			if (lastStartTagBeginAngleBracketIndex != -1)
			{
				string lastTagName = _buffer[lastStartTagBeginAngleBracketIndex + 1];
				if (lastTagName.IgnoreCaseEquals(startTag.NameInLowercase))
				{
					int lastStartTagEndAngleBracketIndex = _buffer.IndexOf(">", lastStartTagBeginAngleBracketIndex);
					if (lastStartTagEndAngleBracketIndex != -1)
					{
						int lastBufferItemIndex = bufferItemCount - 1;
						bool noMoreContent = true;
						if (lastStartTagEndAngleBracketIndex != lastBufferItemIndex)
						{
							for (int bufferItemIndex = lastStartTagEndAngleBracketIndex + 1;
								 bufferItemIndex < bufferItemCount;
								 bufferItemIndex++)
							{
								if (!string.IsNullOrWhiteSpace(_buffer[bufferItemIndex]))
								{
									noMoreContent = false;
									break;
								}
							}
						}

						if (noMoreContent)
						{
							_buffer.RemoveRange(lastStartTagBeginAngleBracketIndex,
								bufferItemCount - lastStartTagBeginAngleBracketIndex);

							isEndTagRemoved = true;
						}
					}
				}
			}

			return isEndTagRemoved;
		}

		#endregion

		/// <summary>
		/// Builds a attribute view model
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="tag">Tag</param>
		/// <param name="attribute">Attribute</param>
		/// <returns>String representation of the attribute</returns>
		private HtmlAttributeViewModel BuildAttributeViewModel(MarkupParsingContext context, HtmlTag tag, HtmlAttribute attribute)
		{
			string tagNameInLowercase = tag.NameInLowercase;
			HtmlAttributeViewModel attributeViewModel;
			string attributeName = attribute.Name;
			string attributeNameInLowercase = attribute.NameInLowercase;
			string attributeValue = attribute.Value;
			bool attributeHasValue = attribute.HasValue;
			bool attributeHasEmptyValue = !attributeHasValue || attributeValue.Length == 0;
			HtmlAttributeType attributeType = attribute.Type;
			bool useHtmlSyntax = !_settings.UseXhtmlSyntax;

			if (useHtmlSyntax && attributeType == HtmlAttributeType.Xml && attributeNameInLowercase != "xmlns")
			{
				string sourceCode = context.SourceCode;
				SourceCodeNodeCoordinates attributeCoordinates = attribute.NameCoordinates;

				WriteWarning(LogCategoryConstants.HtmlMinificationWarning,
					string.Format(Strings.WarningMessage_XmlBasedAttributeNotAllowed, attributeName), _fileContext,
					attributeCoordinates.LineNumber, attributeCoordinates.ColumnNumber,
					SourceCodeNavigator.GetSourceFragment(sourceCode, attributeCoordinates));
			}

			if ((_settings.RemoveRedundantAttributes && IsAttributeRedundant(tag, attribute))
				|| (_settings.RemoveJsTypeAttributes && IsJavaScriptTypeAttribute(tag, attribute))
				|| (_settings.RemoveCssTypeAttributes && IsCssTypeAttribute(tag, attribute))
				|| (useHtmlSyntax && CanRemoveXmlNamespaceAttribute(tag, attribute)))
			{
				if (CanRemoveAttribute(tag, attribute))
				{
					attributeViewModel = HtmlAttributeViewModel.Empty;
					return attributeViewModel;
				}
			}

			bool isCustomBooleanAttribute = !attributeHasValue && attributeType == HtmlAttributeType.Text;
			if (isCustomBooleanAttribute)
			{
				if (useHtmlSyntax)
				{
					attributeViewModel = InnerBuildAttributeViewModel(attribute, true, false);
					return attributeViewModel;
				}

				attribute.Value = string.Empty;
			}
			else if (attributeType != HtmlAttributeType.Event
				&& !attributeHasEmptyValue
				&& TemplateTagHelpers.ContainsTag(attributeValue))
			{
				// Processing of template tags
				StringBuilder attributeValueBuilder = StringBuilderPool.GetBuilder();

				TemplateTagHelpers.ParseMarkup(attributeValue,
					(localContext, expression, startDelimiter, endDelimiter) =>
					{
						string processedExpression = expression;
						if (_settings.MinifyAngularBindingExpressions && startDelimiter == "{{" && endDelimiter == "}}")
						{
							processedExpression = MinifyAngularBindingExpression(context, attribute.ValueCoordinates,
								localContext.NodeCoordinates, expression);
						}

						attributeValueBuilder.Append(startDelimiter);
						attributeValueBuilder.Append(processedExpression);
						attributeValueBuilder.Append(endDelimiter);
					},
					(localContext, textValue) =>
					{
						string processedTextValue = textValue;
						if (attributeType == HtmlAttributeType.ClassName)
						{
							processedTextValue = Utils.CollapseWhitespace(textValue);
						}

						attributeValueBuilder.Append(processedTextValue);
					}
				);

				string processedAttributeValue = attributeValueBuilder.ToString();
				StringBuilderPool.ReleaseBuilder(attributeValueBuilder);

				switch (attributeType)
				{
					case HtmlAttributeType.Uri:
					case HtmlAttributeType.Numeric:
					case HtmlAttributeType.ClassName:
						processedAttributeValue = processedAttributeValue.Trim();
						break;
					case HtmlAttributeType.Style:
						processedAttributeValue = processedAttributeValue.Trim();
						processedAttributeValue = Utils.RemoveEndingSemicolon(processedAttributeValue);
						break;
					default:
						if (_settings.MinifyAngularBindingExpressions && tag.Flags.HasFlag(HtmlTagFlags.Custom))
						{
							string elementDirectiveName = AngularHelpers.NormalizeDirectiveName(tagNameInLowercase);
							if (elementDirectiveName == "ngPluralize" && attributeNameInLowercase == "when")
							{
								processedAttributeValue = MinifyAngularBindingExpression(context, attribute.ValueCoordinates,
									processedAttributeValue);
							}
						}

						break;
				}

				attribute.Value = processedAttributeValue;
			}
			else if (attributeType == HtmlAttributeType.Boolean)
			{
				if (_settings.CollapseBooleanAttributes)
				{
					attributeViewModel = InnerBuildAttributeViewModel(attribute, true, false);
					return attributeViewModel;
				}

				attribute.Value = attributeName;
			}
			else
			{
				if (!attributeHasEmptyValue)
				{
					attribute.Value = CleanAttributeValue(context, tag, attribute);
				}

				if (_settings.RemoveEmptyAttributes && CanRemoveEmptyAttribute(tag, attribute))
				{
					if (CanRemoveAttribute(tag, attribute))
					{
						attributeViewModel = HtmlAttributeViewModel.Empty;
						return attributeViewModel;
					}
				}
			}

			bool addQuotes = !CanRemoveAttributeQuotes(attribute, _settings.AttributeQuotesRemovalMode);
			attributeViewModel = InnerBuildAttributeViewModel(attribute, false, addQuotes);

			return attributeViewModel;
		}

		private HtmlAttributeViewModel InnerBuildAttributeViewModel(HtmlAttribute attribute, bool omitValue,
			bool addQuotes)
		{
			string displayAttributeName = CanPreserveCase() ? attribute.Name : attribute.NameInLowercase;
			string encodedAttributeValue = !omitValue ?
				HtmlAttribute.HtmlAttributeEncode(attribute.Value, HtmlAttributeQuotesType.Double) : null;
			var attributeViewModel = new HtmlAttributeViewModel(displayAttributeName, encodedAttributeValue, addQuotes);

			return attributeViewModel;
		}

		/// <summary>
		/// Determines whether a list of attributes contains the <code>rel</code> attribute with
		/// value, that equals to "external" or "alternate external"
		/// </summary>
		/// <param name="attributes">List of attributes</param>
		/// <returns>Result of check (true - contains; false - does not contain)</returns>
		private static bool ContainsRelExternalAttribute(IList<HtmlAttribute> attributes)
		{
			bool containsRelExternalAttribute = attributes.Any(a => a.NameInLowercase == "rel"
				 && _relExternalAttributeRegex.IsMatch(a.Value));

			return containsRelExternalAttribute;
		}

		/// <summary>
		/// Checks whether it is possible to remove the attribute quotes
		/// </summary>
		/// <param name="attribute">Attribute</param>
		/// <param name="attributeQuotesRemovalMode">Removal mode of HTML attribute quotes</param>
		/// <returns>Result of check (true - can remove; false - cannot remove)</returns>
		private static bool CanRemoveAttributeQuotes(HtmlAttribute attribute,
			HtmlAttributeQuotesRemovalMode attributeQuotesRemovalMode)
		{
			string attributeValue = attribute.Value;
			bool result = false;

			if (attributeQuotesRemovalMode != HtmlAttributeQuotesRemovalMode.KeepQuotes)
			{
				if (!attributeValue.EndsWith("/"))
				{
					if (attributeQuotesRemovalMode == HtmlAttributeQuotesRemovalMode.Html4)
					{
						result = _html4AttributeValueNotRequireQuotesRegex.IsMatch(attributeValue);
					}
					else if (attributeQuotesRemovalMode == HtmlAttributeQuotesRemovalMode.Html5)
					{
						result = CommonRegExps.Html5AttributeValueNotRequireQuotes.IsMatch(attributeValue);
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Checks whether the attribute is redundant
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <param name="attribute">Attribute</param>
		/// <returns>Result of check (true - is redundant; false - is not redundant)</returns>
		private static bool IsAttributeRedundant(HtmlTag tag, HtmlAttribute attribute)
		{
			string tagNameInLowercase = tag.NameInLowercase;
			IList<HtmlAttribute> attributes = tag.Attributes;
			string attributeNameInLowercase = attribute.NameInLowercase;
			string attributeValue = attribute.Value;
			string processedAttributeValue = attributeValue.Trim();

			return (
				(tagNameInLowercase == "script"
					&& ((attributeNameInLowercase == "language" && processedAttributeValue.IgnoreCaseEquals("javascript"))
					|| (attributeNameInLowercase == "charset" && attributes.All(a => a.NameInLowercase != "src"))))
				|| (tagNameInLowercase == "link" && attributeNameInLowercase == "charset" && attributes.Any(
					a => a.NameInLowercase == "rel" && a.Value.Trim().IgnoreCaseEquals("stylesheet")))
				|| (tagNameInLowercase == "form" && attributeNameInLowercase == "method"
					&& processedAttributeValue.IgnoreCaseEquals("get"))
				|| (tagNameInLowercase == "input" && attributeNameInLowercase == "type"
					&& processedAttributeValue.IgnoreCaseEquals("text"))
				|| (tagNameInLowercase == "a" && attributeNameInLowercase == "name" && attributes.Any(
					a => a.NameInLowercase == "id" && a.Value == attributeValue))
				|| (tagNameInLowercase == "area" && attributeNameInLowercase == "shape"
					&& processedAttributeValue.IgnoreCaseEquals("rect"))
			);
		}

		/// <summary>
		/// Checks whether attribute is the attribute <code>type</code> of
		/// tag <code>script</code>, that containing JavaScript code
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <param name="attribute">Attribute</param>
		/// <returns>Result of check</returns>
		private static bool IsJavaScriptTypeAttribute(HtmlTag tag, HtmlAttribute attribute)
		{
			return tag.NameInLowercase == "script" && attribute.NameInLowercase == "type"
				&& attribute.Value.Trim().IgnoreCaseEquals(JS_CONTENT_TYPE);
		}

		/// <summary>
		/// Checks whether attribute is the attribute <code>type</code> of tag <code>link</code>
		/// or <code>style</code>, that containing CSS code
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <param name="attribute">Attribute</param>
		/// <returns>Result of check</returns>
		private static bool IsCssTypeAttribute(HtmlTag tag, HtmlAttribute attribute)
		{
			string tagNameInLowercase = tag.NameInLowercase;
			string attributeNameInLowercase = attribute.NameInLowercase;
			string attributeValue = attribute.Value;
			IList<HtmlAttribute> attributes = tag.Attributes;
			bool isCssTypeAttribute = false;

			if (tagNameInLowercase == "link" || tagNameInLowercase == "style")
			{
				string processedAttributeValue = attributeValue.Trim();

				if (attributeNameInLowercase == "type" && processedAttributeValue.IgnoreCaseEquals(CSS_CONTENT_TYPE))
				{
					if (tagNameInLowercase == "link")
					{
						isCssTypeAttribute = attributes.Any(a => a.NameInLowercase == "rel"
							&& a.Value.Trim().IgnoreCaseEquals("stylesheet"));
					}
					else if (tagNameInLowercase == "style")
					{
						isCssTypeAttribute = true;
					}
				}
			}

			return isCssTypeAttribute;
		}

		/// <summary>
		/// Checks whether the attribute is custom
		/// </summary>
		/// <param name="attribute">Attribute</param>
		/// <returns>Result of check</returns>
		private static bool IsCustomAttribute(HtmlAttribute attribute)
		{
			bool isCustomAttribute = false;

			if (attribute.Type == HtmlAttributeType.Text)
			{
				string attributeNameInLowercase = attribute.NameInLowercase;
				int charCount = attributeNameInLowercase.Length;

				for (int charIndex = 0; charIndex < charCount; charIndex++)
				{
					char charValue = attributeNameInLowercase[charIndex];

					if (!charValue.IsAlphaLower())
					{
						isCustomAttribute = true;
						break;
					}
				}

				if (isCustomAttribute)
				{
					isCustomAttribute = attributeNameInLowercase != "accept-charset"
						&& attributeNameInLowercase != "http-equiv";
				}
			}

			return isCustomAttribute;
		}

		/// <summary>
		/// Cleans a attribute value
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="tag">Tag</param>
		/// <param name="attribute">Attribute</param>
		/// <returns>Processed attribute value</returns>
		private string CleanAttributeValue(MarkupParsingContext context, HtmlTag tag, HtmlAttribute attribute)
		{
			string attributeValue = attribute.Value;
			if (attributeValue.Length == 0)
			{
				return attributeValue;
			}

			string processedAttributeValue = attributeValue;
			string tagNameInLowercase = tag.NameInLowercase;
			IList<HtmlAttribute> attributes = tag.Attributes;
			string attributeNameInLowercase = attribute.NameInLowercase;
			HtmlAttributeType attributeType = attribute.Type;

			switch (attributeType)
			{
				case HtmlAttributeType.Uri:
					processedAttributeValue = processedAttributeValue.Trim();

					if (processedAttributeValue.StartsWith(HTTP_PROTOCOL, StringComparison.OrdinalIgnoreCase))
					{
						if (_settings.RemoveHttpProtocolFromAttributes && !ContainsRelExternalAttribute(attributes))
						{
							int httpProtocolLength = HTTP_PROTOCOL.Length;
							processedAttributeValue = processedAttributeValue.Substring(httpProtocolLength);
						}
					}
					else if (processedAttributeValue.StartsWith(HTTPS_PROTOCOL, StringComparison.OrdinalIgnoreCase))
					{
						if (_settings.RemoveHttpsProtocolFromAttributes && !ContainsRelExternalAttribute(attributes))
						{
							int httpsProtocolLength = HTTPS_PROTOCOL.Length;
							processedAttributeValue = processedAttributeValue.Substring(httpsProtocolLength);
						}
					}
					else if (attributeNameInLowercase == "href"
						&& processedAttributeValue.StartsWith(JS_PROTOCOL, StringComparison.OrdinalIgnoreCase))
					{
						processedAttributeValue = ProcessInlineScriptContent(context, attribute);
					}

					break;
				case HtmlAttributeType.Numeric:
					processedAttributeValue = processedAttributeValue.Trim();
					break;
				case HtmlAttributeType.ClassName:
					if (AngularHelpers.IsClassDirective(processedAttributeValue))
					{
						// Processing of Angular class directives
						string ngOriginalDirectiveName = string.Empty;
						string ngNormalizedDirectiveName = string.Empty;
						string ngExpression;
						var ngDirectives = new Dictionary<string, string>();

						AngularHelpers.ParseClassDirective(processedAttributeValue,
							(localContext, originalDirectiveName, normalizedDirectiveName) =>
							{
								ngOriginalDirectiveName = originalDirectiveName;
								ngNormalizedDirectiveName = normalizedDirectiveName;
								ngExpression = null;

								ngDirectives.Add(ngOriginalDirectiveName, ngExpression);
							},
							(localContext, expression) =>
							{
								ngExpression = expression;
								if (_settings.MinifyAngularBindingExpressions
									&& ContainsAngularBindingExpression(ngNormalizedDirectiveName))
								{
									ngExpression = MinifyAngularBindingExpression(context,
										attribute.ValueCoordinates, localContext.NodeCoordinates,
										expression);
								}

								ngDirectives[ngOriginalDirectiveName] = ngExpression;
							},
							localContext =>
							{
								if (ngDirectives[ngOriginalDirectiveName] == null)
								{
									ngDirectives[ngOriginalDirectiveName] = string.Empty;
								}
							}
						);

						int directiveCount = ngDirectives.Count;
						if (directiveCount > 0)
						{
							StringBuilder directiveBuilder = StringBuilderPool.GetBuilder();
							int directiveIndex = 0;
							int lastDirectiveIndex = directiveCount - 1;
							string previousExpression = null;

							foreach (var directive in ngDirectives)
							{
								string directiveName = directive.Key;
								string expression = directive.Value;

								if (directiveIndex > 0 && (expression == null || previousExpression == null))
								{
									directiveBuilder.Append(" ");
								}

								directiveBuilder.Append(directiveName);
								if (!string.IsNullOrWhiteSpace(expression))
								{
									directiveBuilder.AppendFormat(":{0}", expression);
								}

								if (directiveIndex < lastDirectiveIndex && expression != null)
								{
									directiveBuilder.Append(";");
								}

								previousExpression = expression;
								directiveIndex++;
							}

							processedAttributeValue = directiveBuilder.ToString();
							StringBuilderPool.ReleaseBuilder(directiveBuilder);
						}
						else
						{
							processedAttributeValue = string.Empty;
						}
					}
					else
					{
						processedAttributeValue = processedAttributeValue.Trim();
						processedAttributeValue = Utils.CollapseWhitespace(processedAttributeValue);
					}

					break;
				case HtmlAttributeType.Style:
					processedAttributeValue = ProcessInlineStyleContent(context, attribute);
					break;
				case HtmlAttributeType.Event:
					processedAttributeValue = ProcessInlineScriptContent(context, attribute);
					break;
				default:
					if (attributeNameInLowercase == "data-bind" && _settings.MinifyKnockoutBindingExpressions)
					{
						processedAttributeValue = MinifyKnockoutBindingExpression(context, attribute);
					}
					else if (tagNameInLowercase == "meta" && attributeNameInLowercase == "content"
						&& attributes.Any(a => a.NameInLowercase == "name" && a.Value.Trim().IgnoreCaseEquals("keywords")))
					{
						processedAttributeValue = processedAttributeValue.Trim();
						processedAttributeValue = Utils.CollapseWhitespace(processedAttributeValue);
						processedAttributeValue = _separatingCommaWithSpacesRegex.Replace(processedAttributeValue, ",");
						processedAttributeValue = _endingCommaWithSpacesRegex.Replace(processedAttributeValue, string.Empty);
					}
					else
					{
						if (_settings.MinifyAngularBindingExpressions
							&& CanMinifyAngularBindingExpressionInAttribute(tag, attribute))
						{
							processedAttributeValue = MinifyAngularBindingExpression(context, attribute.ValueCoordinates,
								processedAttributeValue);
						}
					}

					break;
			}

			return processedAttributeValue;
		}

		/// <summary>
		/// Checks whether preserve case of tag and attribute names
		/// </summary>
		/// <returns>Result of check (true - can be preserved; false - can not be preserved)</returns>
		private bool CanPreserveCase()
		{
			return _settings.PreserveCase || _currentTag.Flags.HasFlag(HtmlTagFlags.Xml);
		}

		/// <summary>
		/// Checks whether remove an the attribute
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <param name="attribute">Attribute</param>
		/// <returns>Result of check (true - can be removed; false - can not be removed)</returns>
		private bool CanRemoveAttribute(HtmlTag tag, HtmlAttribute attribute)
		{
			if (_settings.PreservableAttributeCollection.Count == 0)
			{
				return true;
			}

			string tagNameInLowercase = tag.NameInLowercase;
			string attributeNameInLowercase = attribute.NameInLowercase;
			string attributeValue = attribute.Value;

			bool result = true;

			foreach (HtmlAttributeExpression attributeExpression in _settings.PreservableAttributeCollection)
			{
				bool cannotRemove = attributeExpression.IsMatch(tagNameInLowercase, attributeNameInLowercase,
					attributeValue);
				if (cannotRemove)
				{
					result = false;
					break;
				}
			}

			return result;
		}

		/// <summary>
		/// Checks whether remove an the attribute, that has empty value
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <param name="attribute">Attribute</param>
		/// <returns>Result of check (true - can be removed; false - can not be removed)</returns>
		private static bool CanRemoveEmptyAttribute(HtmlTag tag, HtmlAttribute attribute)
		{
			string tagNameInLowercase = tag.NameInLowercase;
			string attributeNameInLowercase = attribute.NameInLowercase;
			string attributeValue = attribute.Value;
			HtmlAttributeType attributeType = attribute.Type;

			bool result = false;
			bool isZeroLengthString = attributeValue.Length == 0;

			if (isZeroLengthString || string.IsNullOrWhiteSpace(attributeValue))
			{
				if (tagNameInLowercase == "input" && attributeNameInLowercase == "value")
				{
					result = isZeroLengthString;
				}
				else if (attributeType == HtmlAttributeType.Event
					|| (tagNameInLowercase == "form" && attributeNameInLowercase == "action")
					|| _emptyAttributesForRemoval.Contains(attributeNameInLowercase))
				{
					result = true;
				}
			}

			return result;
		}

		/// <summary>
		/// Checks whether remove an the <code>xmlns</code> attribute
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <param name="attribute">Attribute</param>
		/// <returns>Result of check (true - can be removed; false - can not be removed)</returns>
		private static bool CanRemoveXmlNamespaceAttribute(HtmlTag tag, HtmlAttribute attribute)
		{
			return tag.NameInLowercase == "html" && attribute.NameInLowercase == "xmlns";
		}

		/// <summary>
		/// Checks whether the tag is a META content-type tag
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <returns>Result of check (true - is META content-type tag; false - is not META content-type tag)</returns>
		private static bool IsMetaContentTypeTag(HtmlTag tag)
		{
			return tag.NameInLowercase == "meta" && tag.Attributes.Any(
				a => a.NameInLowercase == "http-equiv" && a.Value.Trim().IgnoreCaseEquals("content-type"));
		}

		/// <summary>
		/// Upgrades a META content-type tag to the META charset tag
		/// </summary>
		/// <param name="tag">META content-type tag</param>
		/// <returns>META charset tag</returns>
		private HtmlTag UpgradeToMetaCharsetTag(HtmlTag tag)
		{
			HtmlTag upgradedTag = tag;

			HtmlAttribute contentAttribute = tag.Attributes.SingleOrDefault(a => a.NameInLowercase == "content");
			if (contentAttribute != null)
			{
				string content = contentAttribute.Value.Trim();
				if (content.Length > 0)
				{
					Match contentMatch = _metaContentTypeTagValueRegex.Match(content);
					if (contentMatch.Success)
					{
						string charset = contentMatch.Groups["charset"].Value;
						upgradedTag = new HtmlTag(tag.Name, tag.NameInLowercase,
							new List<HtmlAttribute>
							{
								new HtmlAttribute("charset", "charset", charset, HtmlAttributeType.Text)
							},
							tag.Flags
						);
					}
				}
			}

			return upgradedTag;
		}

		/// <summary>
		/// Checks whether remove whitespace between non-independent tags
		/// </summary>
		/// <param name="firstTag">First tag</param>
		/// <param name="secondTag">Second tag</param>
		/// <returns>Result of check (true - can be removed; false - can not be removed)</returns>
		private static bool CanRemoveWhitespaceBetweenNonIndependentTags(HtmlTag firstTag, HtmlTag secondTag)
		{
			string firstTagNameInLowercase = firstTag.NameInLowercase;
			string secondTagNameInLowercase = secondTag.NameInLowercase;
			bool cannotRemove;

			switch (firstTagNameInLowercase)
			{
				case "li":
					cannotRemove = secondTagNameInLowercase == "li";
					break;
				case "dt":
				case "dd":
					cannotRemove = secondTagNameInLowercase == "dt" || secondTagNameInLowercase == "dd";
					break;
				case "img":
					cannotRemove = secondTagNameInLowercase == "figcaption";
					break;
				default:
					cannotRemove = secondTagNameInLowercase == "rt" || secondTagNameInLowercase == "rp"
						|| secondTagNameInLowercase == "rb" || secondTagNameInLowercase == "rtc";
					break;
			}

			return !cannotRemove;
		}

		/// <summary>
		/// Checks whether remove whitespace after end non-independent tag
		/// </summary>
		/// <param name="endTag">End tag</param>
		/// <returns>Result of check (true - can be removed; false - can not be removed)</returns>
		private static bool CanRemoveWhitespaceAfterEndNonIndependentTag(HtmlTag endTag)
		{
			string endTagNameInLowercase = endTag.NameInLowercase;
			bool cannotRemove;

			switch (endTagNameInLowercase)
			{
				case "li":
				case "dt":
				case "dd":
				case "rt":
				case "rp":
				case "rb":
				case "rtc":
					cannotRemove = true;
					break;
				default:
					cannotRemove = false;
					break;
			}

			return !cannotRemove;
		}

		/// <summary>
		/// Checks whether remove whitespace after end non-independent tag by parent tag
		/// </summary>
		/// <param name="endTag">End tag</param>
		/// <param name="parentTag">Parent tag</param>
		/// <returns>Result of check (true - can be removed; false - can not be removed)</returns>
		private static bool CanRemoveWhitespaceAfterEndNonIndependentTagByParentTag(HtmlTag endTag, HtmlTag parentTag)
		{
			string endTagNameInLowercase = endTag.NameInLowercase;
			string parentTagNameInLowercase = parentTag.NameInLowercase;
			bool canRemove;

			switch (endTagNameInLowercase)
			{
				case "li":
					canRemove = parentTagNameInLowercase == "ul" || parentTagNameInLowercase == "ol"
						|| parentTagNameInLowercase == "menu";
					break;
				case "dt":
				case "dd":
					canRemove = parentTagNameInLowercase == "dl";
					break;
				default:
					canRemove = false;
					break;
			}

			return canRemove;
		}

		/// <summary>
		/// Checks whether remove an the safe optional end tag
		/// </summary>
		/// <param name="optionalEndTag">Optional end tag</param>
		/// <returns>Result of check (true - can be removed; false - can not be removed)</returns>
		private bool CanRemoveSafeOptionalEndTag(HtmlTag optionalEndTag)
		{
			string optionalEndTagNameInLowercase = optionalEndTag.NameInLowercase;
			if (_settings.PreservableOptionalTagCollection.Contains(optionalEndTagNameInLowercase))
			{
				return false;
			}

			return _safeOptionalEndTags.Contains(optionalEndTagNameInLowercase);
		}

		/// <summary>
		/// Checks whether remove an the optional end tag
		/// </summary>
		/// <param name="optionalEndTag">Optional end tag</param>
		/// <param name="nextTag">Next tag</param>
		/// <returns>Result of check (true - can be removed; false - can not be removed)</returns>
		private bool CanRemoveOptionalEndTagByNextTag(HtmlTag optionalEndTag, HtmlTag nextTag)
		{
			string optionalEndTagNameInLowercase = optionalEndTag.NameInLowercase;
			if (_settings.PreservableOptionalTagCollection.Contains(optionalEndTagNameInLowercase))
			{
				return false;
			}

			string nextTagNameInLowercase = nextTag.NameInLowercase;
			bool canRemove;

			switch (optionalEndTagNameInLowercase)
			{
				case "p":
					canRemove = _tagsFollowingAfterParagraphOptionalEndTag.Contains(nextTagNameInLowercase);
					break;
				case "li":
					canRemove = nextTagNameInLowercase == "li";
					break;
				case "thead":
				case "tbody":
					canRemove = nextTagNameInLowercase == "tbody" || nextTagNameInLowercase == "tfoot";
					break;
				case "tfoot":
					canRemove = nextTagNameInLowercase == "tbody";
					break;
				case "tr":
					canRemove = nextTagNameInLowercase == "tr";
					break;
				case "td":
				case "th":
					canRemove = nextTagNameInLowercase == "td" || nextTagNameInLowercase == "th";
					break;
				case "option":
					canRemove = nextTagNameInLowercase == "option" || nextTagNameInLowercase == "optgroup";
					break;
				case "optgroup":
					canRemove = nextTagNameInLowercase == "optgroup";
					break;
				case "dt":
				case "dd":
					canRemove = nextTagNameInLowercase == "dt" || nextTagNameInLowercase == "dd";
					break;
				case "rt":
				case "rp":
				case "rb":
					canRemove = nextTagNameInLowercase == "rt" || nextTagNameInLowercase == "rp"
						|| nextTagNameInLowercase == "rb" || nextTagNameInLowercase == "rtc"
						;
					break;
				case "rtc":
					canRemove = nextTagNameInLowercase == "rp" || nextTagNameInLowercase == "rb"
						|| nextTagNameInLowercase == "rtc"
						;
					break;
				default:
					canRemove = false;
					break;
			}

			return canRemove;
		}

		/// <summary>
		/// Checks whether remove an the optional end tag
		/// </summary>
		/// <param name="optionalEndTag">Optional end tag</param>
		/// <param name="parentTag">Parent tag</param>
		/// <returns>Result of check (true - can be removed; false - can not be removed)</returns>
		private bool CanRemoveOptionalEndTagByParentTag(HtmlTag optionalEndTag, HtmlTag parentTag)
		{
			string optionalEndTagNameInLowercase = optionalEndTag.NameInLowercase;
			if (_settings.PreservableOptionalTagCollection.Contains(optionalEndTagNameInLowercase))
			{
				return false;
			}

			string parentTagNameInLowercase = parentTag.NameInLowercase;
			bool canRemove;

			switch (optionalEndTagNameInLowercase)
			{
				case "p":
					canRemove = parentTagNameInLowercase != "a";
					break;
				case "li":
					canRemove = parentTagNameInLowercase == "ul" || parentTagNameInLowercase == "ol"
						|| parentTagNameInLowercase == "menu";
					break;
				case "tbody":
				case "tfoot":
					canRemove = parentTagNameInLowercase == "table";
					break;
				case "tr":
					canRemove = parentTagNameInLowercase == "table" || parentTagNameInLowercase == "thead"
						|| parentTagNameInLowercase == "tbody" || parentTagNameInLowercase == "tfoot";
					break;
				case "td":
				case "th":
					canRemove = parentTagNameInLowercase == "tr";
					break;
				case "option":
					canRemove = parentTagNameInLowercase == "select" || parentTagNameInLowercase == "optgroup"
						|| parentTagNameInLowercase == "datalist";
					break;
				case "optgroup":
					canRemove = parentTagNameInLowercase == "select";
					break;
				case "dd":
					canRemove = parentTagNameInLowercase == "dl";
					break;
				case "rt":
					canRemove = parentTagNameInLowercase == "ruby" || parentTagNameInLowercase == "rtc";
					break;
				case "rp":
				case "rb":
				case "rtc":
					canRemove = parentTagNameInLowercase == "ruby";
					break;
				default:
					canRemove = false;
					break;
			}

			return canRemove;
		}

		/// <summary>
		/// Checks whether remove an the tag, that has empty content
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <returns>Result of check (true - can be removed; false - can not be removed)</returns>
		private static bool CanRemoveTagWithoutContent(HtmlTag tag)
		{
			string tagNameInLowercase = tag.NameInLowercase;
			HtmlTagFlags tagFlags = tag.Flags;
			IList<HtmlAttribute> attributes = tag.Attributes;

			return !(tagFlags.HasFlag(HtmlTagFlags.Custom)
				|| (tagFlags.HasFlag(HtmlTagFlags.Xml) && tagFlags.HasFlag(HtmlTagFlags.NonIndependent))
				|| _unremovableEmptyTags.Contains(tagNameInLowercase)
				|| attributes.Any(a => IsCustomAttribute(a)
					|| (_unremovableEmptyTagAttributes.Contains(a.NameInLowercase) && !string.IsNullOrWhiteSpace(a.Value))));
		}

		/// <summary>
		/// Checks whether to minify whitespaces in text content of tag
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <returns>Result of check (true - can minify whitespaces; false - can not minify whitespaces)</returns>
		private static bool CanMinifyWhitespace(HtmlTag tag)
		{
			return !_tagsWithNotRemovableWhitespace.Contains(tag.NameInLowercase);
		}

		/// <summary>
		/// Processes a embedded script content
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="content">Embedded script content</param>
		/// <param name="contentType">Content type (MIME type) of the script</param>
		private void ProcessEmbeddedScriptContent(MarkupParsingContext context, string content, string contentType)
		{
			string code = content;
			bool isNotEmpty = false;
			string processedContentType = !string.IsNullOrWhiteSpace(contentType) ?
				contentType.Trim().ToLowerInvariant() : JS_CONTENT_TYPE;
			bool isJavaScript = _jsContentTypes.Contains(processedContentType);
			bool isVbScript = processedContentType == VBS_CONTENT_TYPE;
			bool minifyWhitespace = _settings.WhitespaceMinificationMode != WhitespaceMinificationMode.None;

			if (isJavaScript || isVbScript)
			{
				bool removeHtmlComments = _settings.RemoveHtmlCommentsFromScriptsAndStyles;
				bool removeCdataSections = _settings.RemoveCdataSectionsFromScriptsAndStyles;

				string startPart = string.Empty;
				string endPart = string.Empty;
				string newLine = Environment.NewLine;
				string beforeCodeContent = string.Empty;

				if (isJavaScript)
				{
					// Processing of JavaScript code
					if (_beginCdataSectionRegex.IsMatch(content))
					{
						beforeCodeContent = _beginCdataSectionRegex.Match(content).Value;
						startPart = "<![CDATA[";
						endPart = "]]>";
						code = Utils.RemovePrefixAndPostfix(content, _beginCdataSectionRegex, _endCdataSectionRegex);
					}
					else if (_scriptBeginCdataSectionRegex.IsMatch(content))
					{
						beforeCodeContent = _scriptBeginCdataSectionRegex.Match(content).Value;

						if (!removeCdataSections)
						{
							startPart = "//<![CDATA[";
							endPart = "//]]>";
						}

						code = Utils.RemovePrefixAndPostfix(content, _scriptBeginCdataSectionRegex,
							_scriptEndCdataSectionRegex);
					}
					else if (_scriptBeginMaxCompatibleCdataSectionRegex.IsMatch(content))
					{
						beforeCodeContent = _scriptBeginMaxCompatibleCdataSectionRegex.Match(content).Value;

						if (!removeCdataSections)
						{
							startPart = "<!--//--><![CDATA[//><!--";
							endPart = "//--><!]]>";
						}

						code = Utils.RemovePrefixAndPostfix(content, _scriptBeginMaxCompatibleCdataSectionRegex,
							_scriptEndMaxCompatibleCdataSectionRegex);
					}
					else if (_scriptBeginHtmlCommentRegex.IsMatch(content))
					{
						beforeCodeContent = _scriptBeginHtmlCommentRegex.Match(content).Value;

						if (!removeHtmlComments)
						{
							startPart = "<!--";
							endPart = "//-->";
						}

						code = Utils.RemovePrefixAndPostfix(content, _scriptBeginHtmlCommentRegex,
							_scriptEndHtmlCommentRegex);
					}

					if (_settings.MinifyEmbeddedJsCode)
					{
						CodeMinificationResult minificationResult = _jsMinifier.Minify(code, false);
						if (minificationResult.Errors.Count == 0)
						{
							code = minificationResult.MinifiedContent ?? string.Empty;
						}

						if (minificationResult.Errors.Count > 0 || minificationResult.Warnings.Count > 0)
						{
							string sourceCode = context.SourceCode;
							var documentCoordinates = SourceCodeNavigator.CalculateAbsoluteNodeCoordinates(
								context.NodeCoordinates, beforeCodeContent);

							foreach (MinificationErrorInfo error in minificationResult.Errors)
							{
								var relativeErrorCoordinates = new SourceCodeNodeCoordinates(error.LineNumber, error.ColumnNumber);
								var absoluteErrorCoordinates = SourceCodeNavigator.CalculateAbsoluteNodeCoordinates(
									documentCoordinates, relativeErrorCoordinates);
								string sourceFragment = SourceCodeNavigator.GetSourceFragment(
									sourceCode, absoluteErrorCoordinates);
								string message = error.Message.Trim();

								WriteError(LogCategoryConstants.JsMinificationError, message, _fileContext,
									absoluteErrorCoordinates.LineNumber, absoluteErrorCoordinates.ColumnNumber, sourceFragment);
							}

							foreach (MinificationErrorInfo warning in minificationResult.Warnings)
							{
								var relativeErrorCoordinates = new SourceCodeNodeCoordinates(warning.LineNumber, warning.ColumnNumber);
								var absoluteErrorCoordinates = SourceCodeNavigator.CalculateAbsoluteNodeCoordinates(
									documentCoordinates, relativeErrorCoordinates);
								string sourceFragment = SourceCodeNavigator.GetSourceFragment(
									sourceCode, absoluteErrorCoordinates);
								string message = warning.Message.Trim();

								WriteWarning(LogCategoryConstants.JsMinificationWarning, message, _fileContext,
									absoluteErrorCoordinates.LineNumber, absoluteErrorCoordinates.ColumnNumber, sourceFragment);
							}
						}
					}
				}
				else
				{
					// Processing of VBScript code
					if (_beginCdataSectionRegex.IsMatch(content))
					{
						startPart = "<![CDATA[";
						endPart = "]]>";
						code = Utils.RemovePrefixAndPostfix(content, _beginCdataSectionRegex, _endCdataSectionRegex);
					}
					else if (_beginHtmlCommentRegex.IsMatch(content))
					{
						if (!removeHtmlComments)
						{
							startPart = "<!--";
							endPart = "-->";
						}

						code = Utils.RemovePrefixAndPostfix(content, _beginHtmlCommentRegex, _endHtmlCommentRegex);
					}
				}

				if (minifyWhitespace && code.Length > 0)
				{
					code = code.Trim();
				}

				if (startPart.Length > 0)
				{
					_buffer.Add(startPart);
					if (newLine.Length > 0)
					{
						_buffer.Add(newLine);
					}
					isNotEmpty = true;
				}
				if (code.Length > 0)
				{
					_buffer.Add(code);
					isNotEmpty = true;
				}
				if (endPart.Length > 0)
				{
					if (newLine.Length > 0)
					{
						_buffer.Add(newLine);
					}
					_buffer.Add(endPart);
					isNotEmpty = true;
				}

				_currentText = isNotEmpty ? EMBEDDED_CODE_PLACEHOLDER : string.Empty;

				return;
			}

			if (_settings.ProcessableScriptTypeCollection.Contains(processedContentType))
			{
				// Processing of JavaScript template
				GenericHtmlMinifier innerHtmlMinifier = GetInnerHtmlMinifierInstance();
				MarkupMinificationResult minificationResult = innerHtmlMinifier.Minify(content, false);

				if (minificationResult.Errors.Count == 0)
				{
					code = minificationResult.MinifiedContent ?? string.Empty;
				}

				if (minificationResult.Errors.Count > 0 || minificationResult.Warnings.Count > 0)
				{
					string sourceCode = context.SourceCode;
					var documentCoordinates = context.NodeCoordinates;

					foreach (MinificationErrorInfo error in minificationResult.Errors)
					{
						var relativeErrorCoordinates = new SourceCodeNodeCoordinates(error.LineNumber, error.ColumnNumber);
						var absoluteErrorCoordinates = SourceCodeNavigator.CalculateAbsoluteNodeCoordinates(
							documentCoordinates, relativeErrorCoordinates);
						string sourceFragment = SourceCodeNavigator.GetSourceFragment(
							sourceCode, absoluteErrorCoordinates);
						string message = error.Message.Trim();

						WriteError(LogCategoryConstants.JsTemplateMinificationError, message, _fileContext,
							absoluteErrorCoordinates.LineNumber, absoluteErrorCoordinates.ColumnNumber, sourceFragment);
					}

					foreach (MinificationErrorInfo warning in minificationResult.Warnings)
					{
						var relativeErrorCoordinates = new SourceCodeNodeCoordinates(warning.LineNumber, warning.ColumnNumber);
						var absoluteErrorCoordinates = SourceCodeNavigator.CalculateAbsoluteNodeCoordinates(
							documentCoordinates, relativeErrorCoordinates);
						string sourceFragment = SourceCodeNavigator.GetSourceFragment(
							sourceCode, absoluteErrorCoordinates);
						string message = warning.Message.Trim();

						WriteWarning(LogCategoryConstants.JsTemplateMinificationWarning, message, _fileContext,
							absoluteErrorCoordinates.LineNumber, absoluteErrorCoordinates.ColumnNumber, sourceFragment);
					}
				}

				if (minifyWhitespace && code.Length > 0)
				{
					code = code.Trim();
				}
			}

			if (code.Length > 0)
			{
				_buffer.Add(code);
				isNotEmpty = true;
			}

			_currentText = isNotEmpty ? EMBEDDED_CODE_PLACEHOLDER : string.Empty;
		}

		/// <summary>
		/// Processes a embedded style content
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="content">Embedded style content</param>
		/// <param name="contentType">Content type (MIME type) of the style</param>
		private void ProcessEmbeddedStyleContent(MarkupParsingContext context, string content, string contentType)
		{
			string code;
			bool isNotEmpty = false;
			string processedContentType = !string.IsNullOrWhiteSpace(contentType) ?
				contentType.Trim().ToLowerInvariant() : CSS_CONTENT_TYPE;
			bool minifyWhitespace = _settings.WhitespaceMinificationMode != WhitespaceMinificationMode.None;
			bool removeHtmlComments = _settings.RemoveHtmlCommentsFromScriptsAndStyles;
			bool removeCdataSections = _settings.RemoveCdataSectionsFromScriptsAndStyles;

			string startPart = string.Empty;
			string endPart = string.Empty;
			string newLine = string.Empty;
			string beforeCodeContent = string.Empty;

			if (_beginCdataSectionRegex.IsMatch(content))
			{
				beforeCodeContent = _beginCdataSectionRegex.Match(content).Value;
				startPart = "<![CDATA[";
				endPart = "]]>";
				newLine = Environment.NewLine;
				code = Utils.RemovePrefixAndPostfix(content, _beginCdataSectionRegex, _endCdataSectionRegex);
			}
			else if (_styleBeginCdataSectionRegex.IsMatch(content))
			{
				beforeCodeContent = _styleBeginCdataSectionRegex.Match(content).Value;

				if (!removeCdataSections)
				{
					startPart = "/*<![CDATA[*/";
					endPart = "/*]]>*/";
				}

				code = Utils.RemovePrefixAndPostfix(content, _styleBeginCdataSectionRegex, _styleEndCdataSectionRegex);
			}
			else if (_styleBeginMaxCompatibleCdataSectionRegex.IsMatch(content))
			{
				beforeCodeContent = _styleBeginMaxCompatibleCdataSectionRegex.Match(content).Value;

				if (!removeCdataSections)
				{
					startPart = "<!--/*--><![CDATA[/*><!--*/";
					endPart = "/*]]>*/-->";
				}

				code = Utils.RemovePrefixAndPostfix(content, _styleBeginMaxCompatibleCdataSectionRegex,
					_styleEndMaxCompatibleCdataSectionRegex);
			}
			else if (_beginHtmlCommentRegex.IsMatch(content))
			{
				beforeCodeContent = _beginHtmlCommentRegex.Match(content).Value;

				if (!removeHtmlComments)
				{
					startPart = "<!--";
					endPart = "-->";
				}

				code = Utils.RemovePrefixAndPostfix(content, _beginHtmlCommentRegex, _endHtmlCommentRegex);
			}
			else
			{
				code = content;
			}

			if (processedContentType == CSS_CONTENT_TYPE && _settings.MinifyEmbeddedCssCode)
			{
				CodeMinificationResult minificationResult = _cssMinifier.Minify(code, false);
				if (minificationResult.Errors.Count == 0)
				{
					code = minificationResult.MinifiedContent ?? string.Empty;
				}

				if (minificationResult.Errors.Count > 0 || minificationResult.Warnings.Count > 0)
				{
					string sourceCode = context.SourceCode;
					var documentNodeCoordinates = SourceCodeNavigator.CalculateAbsoluteNodeCoordinates(
						context.NodeCoordinates, beforeCodeContent);

					foreach (MinificationErrorInfo error in minificationResult.Errors)
					{
						var embeddedContentNodeCoordinates = new SourceCodeNodeCoordinates(error.LineNumber, error.ColumnNumber);
						var absoluteNodeCoordinates = SourceCodeNavigator.CalculateAbsoluteNodeCoordinates(
							documentNodeCoordinates, embeddedContentNodeCoordinates);
						string sourceFragment = SourceCodeNavigator.GetSourceFragment(
							sourceCode, absoluteNodeCoordinates);
						string message = error.Message.Trim();

						WriteError(LogCategoryConstants.CssMinificationError, message, _fileContext,
							absoluteNodeCoordinates.LineNumber, absoluteNodeCoordinates.ColumnNumber, sourceFragment);
					}

					foreach (MinificationErrorInfo warning in minificationResult.Warnings)
					{
						var embeddedContentNodeCoordinates = new SourceCodeNodeCoordinates(warning.LineNumber, warning.ColumnNumber);
						var absoluteNodeCoordinates = SourceCodeNavigator.CalculateAbsoluteNodeCoordinates(
							documentNodeCoordinates, embeddedContentNodeCoordinates);
						string sourceFragment = SourceCodeNavigator.GetSourceFragment(
							sourceCode, absoluteNodeCoordinates);
						string message = warning.Message.Trim();

						WriteWarning(LogCategoryConstants.CssMinificationWarning, message, _fileContext,
							absoluteNodeCoordinates.LineNumber, absoluteNodeCoordinates.ColumnNumber, sourceFragment);
					}
				}
			}

			if (minifyWhitespace && code.Length > 0)
			{
				code = code.Trim();
			}

			if (startPart.Length > 0)
			{
				_buffer.Add(startPart);
				if (newLine.Length > 0)
				{
					_buffer.Add(newLine);
				}
				isNotEmpty = true;
			}
			if (code.Length > 0)
			{
				_buffer.Add(code);
				isNotEmpty = true;
			}
			if (endPart.Length > 0)
			{
				if (newLine.Length > 0)
				{
					_buffer.Add(newLine);
				}
				_buffer.Add(endPart);
				isNotEmpty = true;
			}

			_currentText = isNotEmpty ? EMBEDDED_CODE_PLACEHOLDER : string.Empty;
		}

		/// <summary>
		/// Processes a inline script content
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="attribute">Attribute</param>
		/// <returns>Processed inline script content</returns>
		private string ProcessInlineScriptContent(MarkupParsingContext context, HtmlAttribute attribute)
		{
			string scriptContent = attribute.Value;
			bool forHrefAttribute = attribute.NameInLowercase == "href";

			string result = scriptContent;

			if (_settings.MinifyInlineJsCode && _jsMinifier.IsInlineCodeMinificationSupported)
			{
				bool isJavascriptProtocolRemoved = false;
				if (scriptContent.StartsWith(JS_PROTOCOL, StringComparison.OrdinalIgnoreCase))
				{
					result = _jsProtocolRegex.Replace(result, string.Empty);
					isJavascriptProtocolRemoved = true;
				}

				CodeMinificationResult minificationResult = _jsMinifier.Minify(result, true);
				if (minificationResult.Errors.Count == 0)
				{
					result = minificationResult.MinifiedContent ?? string.Empty;
				}

				if (minificationResult.Errors.Count > 0 || minificationResult.Warnings.Count > 0)
				{
					string sourceCode = context.SourceCode;
					SourceCodeNodeCoordinates tagCoordinates = context.NodeCoordinates;
					SourceCodeNodeCoordinates attributeCoordinates = attribute.ValueCoordinates;

					foreach (MinificationErrorInfo error in minificationResult.Errors)
					{
						var relativeErrorCoordinates = new SourceCodeNodeCoordinates(error.LineNumber, error.ColumnNumber);
						SourceCodeNodeCoordinates absoluteErrorCoordinates = CalculateAbsoluteInlineCodeErrorCoordinates(
							tagCoordinates, attributeCoordinates, relativeErrorCoordinates);
						string sourceFragment = SourceCodeNavigator.GetSourceFragment(
							sourceCode, absoluteErrorCoordinates);
						string message = error.Message.Trim();

						WriteError(LogCategoryConstants.JsMinificationError, message, _fileContext,
							absoluteErrorCoordinates.LineNumber, absoluteErrorCoordinates.ColumnNumber, sourceFragment);
					}

					foreach (MinificationErrorInfo warning in minificationResult.Warnings)
					{
						var relativeErrorCoordinates = new SourceCodeNodeCoordinates(warning.LineNumber, warning.ColumnNumber);
						SourceCodeNodeCoordinates absoluteErrorCoordinates = CalculateAbsoluteInlineCodeErrorCoordinates(
							tagCoordinates, attributeCoordinates, relativeErrorCoordinates);
						string sourceFragment = SourceCodeNavigator.GetSourceFragment(
							sourceCode, absoluteErrorCoordinates);
						string message = warning.Message.Trim();

						WriteWarning(LogCategoryConstants.JsMinificationWarning, message, _fileContext,
							absoluteErrorCoordinates.LineNumber, absoluteErrorCoordinates.ColumnNumber, sourceFragment);
					}
				}

				if (isJavascriptProtocolRemoved
					&& (forHrefAttribute || !_settings.RemoveJsProtocolFromAttributes))
				{
					result = JS_PROTOCOL + result;
				}
			}
			else
			{
				result = result.Trim();

				if (!forHrefAttribute && _settings.RemoveJsProtocolFromAttributes)
				{
					result = _jsProtocolRegex.Replace(result, string.Empty);
				}
			}

			result = Utils.RemoveEndingSemicolon(result);

			return result;
		}

		/// <summary>
		/// Processes a inline style content
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="attribute">Attribute</param>
		/// <returns>Processed inline style content</returns>
		private string ProcessInlineStyleContent(MarkupParsingContext context, HtmlAttribute attribute)
		{
			string styleContent = attribute.Value;

			string result = styleContent;
			if (_settings.MinifyInlineCssCode && _cssMinifier.IsInlineCodeMinificationSupported)
			{
				CodeMinificationResult minificationResult = _cssMinifier.Minify(result, true);
				if (minificationResult.Errors.Count == 0)
				{
					result = minificationResult.MinifiedContent ?? string.Empty;
				}

				if (minificationResult.Errors.Count > 0 || minificationResult.Warnings.Count > 0)
				{
					string sourceCode = context.SourceCode;
					SourceCodeNodeCoordinates tagCoordinates = context.NodeCoordinates;
					SourceCodeNodeCoordinates attributeCoordinates = attribute.ValueCoordinates;

					foreach (MinificationErrorInfo error in minificationResult.Errors)
					{
						var relativeErrorCoordinates = new SourceCodeNodeCoordinates(error.LineNumber, error.ColumnNumber);
						SourceCodeNodeCoordinates absoluteErrorCoordinates = CalculateAbsoluteInlineCodeErrorCoordinates(
							tagCoordinates, attributeCoordinates, relativeErrorCoordinates);
						string sourceFragment = SourceCodeNavigator.GetSourceFragment(
							sourceCode, absoluteErrorCoordinates);
						string message = error.Message.Trim();

						WriteError(LogCategoryConstants.CssMinificationError, message, _fileContext,
							absoluteErrorCoordinates.LineNumber, absoluteErrorCoordinates.ColumnNumber, sourceFragment);
					}

					foreach (MinificationErrorInfo warning in minificationResult.Warnings)
					{
						var relativeErrorCoordinates = new SourceCodeNodeCoordinates(warning.LineNumber, warning.ColumnNumber);
						SourceCodeNodeCoordinates absoluteErrorCoordinates = CalculateAbsoluteInlineCodeErrorCoordinates(
							tagCoordinates, attributeCoordinates, relativeErrorCoordinates);
						string sourceFragment = SourceCodeNavigator.GetSourceFragment(
							sourceCode, absoluteErrorCoordinates);
						string message = warning.Message.Trim();

						WriteWarning(LogCategoryConstants.CssMinificationWarning, message, _fileContext,
							absoluteErrorCoordinates.LineNumber, absoluteErrorCoordinates.ColumnNumber, sourceFragment);
					}
				}
			}
			else
			{
				result = result.Trim();
			}

			result = Utils.RemoveEndingSemicolon(result);

			return result;
		}

		#region Knockout helpers

		/// <summary>
		/// Minify a Knockout binding expression
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="attribute">Attribute</param>
		/// <returns>Minified binding expression</returns>
		private string MinifyKnockoutBindingExpression(MarkupParsingContext context, HtmlAttribute attribute)
		{
			return MinifyKnockoutBindingExpression(context, attribute.ValueCoordinates, SourceCodeNodeCoordinates.Empty,
				attribute.Value);
		}

		/// <summary>
		/// Minify a Knockout binding expression
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="expressionCoordinates">Coordinates of expression</param>
		/// <param name="expression">Binding expression</param>
		/// <returns>Minified binding expression</returns>
		private string MinifyKnockoutBindingExpression(MarkupParsingContext context,
			SourceCodeNodeCoordinates expressionCoordinates, string expression)
		{
			return MinifyKnockoutBindingExpression(context, SourceCodeNodeCoordinates.Empty, expressionCoordinates,
				expression);
		}

		/// <summary>
		/// Minify a Knockout binding expression
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="attributeCoordinates">Coordinates of attribute value</param>
		/// <param name="expressionCoordinates">Coordinates of expression</param>
		/// <param name="expression">Binding expression</param>
		/// <returns>Minified binding expression</returns>
		private string MinifyKnockoutBindingExpression(MarkupParsingContext context,
			SourceCodeNodeCoordinates attributeCoordinates, SourceCodeNodeCoordinates expressionCoordinates,
			string expression)
		{
			if (string.IsNullOrWhiteSpace(expression))
			{
				return string.Empty;
			}

			string result = expression;
			CrockfordJsMinifier innerCrockfordJsMinifier = GetInnerCrockfordJsMinifierInstance();
			CodeMinificationResult minificationResult = innerCrockfordJsMinifier.Minify(
				JsonHelpers.WrapStringInCurlyBraces(result), true);
			IList<MinificationErrorInfo> errors = minificationResult.Errors;

			if (errors.Count == 0)
			{
				result = minificationResult.MinifiedContent ?? string.Empty;
			}
			else
			{
				SourceCodeNodeCoordinates absoluteErrorCoordinates = CalculateAbsoluteInlineCodeErrorCoordinates(
					context.NodeCoordinates, attributeCoordinates, expressionCoordinates);
				string sourceFragment = SourceCodeNavigator.GetSourceFragment(
					context.SourceCode, absoluteErrorCoordinates);
				string errorMessage = errors[0].Message;

				WriteError(LogCategoryConstants.JsTemplateMinificationError,
					string.Format(Strings.ErrorMessage_BindingExpressionMinificationFailed,
						"Knockout", errorMessage.TrimEnd('.')),
					_fileContext,
					absoluteErrorCoordinates.LineNumber, absoluteErrorCoordinates.ColumnNumber,
					sourceFragment);
			}

			result = JsonHelpers.UnwrapStringInCurlyBraces(result);

			return result;
		}

		#endregion

		#region Angular helpers

		/// <summary>
		/// Determines whether a directive contains the Angular binding expression
		/// </summary>
		/// <param name="directiveName">Directive name</param>
		/// <returns>Result of check (true - contains; false - not contains)</returns>
		private bool ContainsAngularBindingExpression(string directiveName)
		{
			bool result = !string.IsNullOrEmpty(directiveName)
				&& _angularDirectivesWithExpressions.Contains(directiveName);

			return result;
		}

		/// <summary>
		/// Checks whether to minify the Angular binding expression in attribute
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <param name="attribute">Attribute</param>
		/// <returns>Result of check (true - can minify expression; false - can not minify expression)</returns>
		private bool CanMinifyAngularBindingExpressionInAttribute(HtmlTag tag, HtmlAttribute attribute)
		{
			string tagNameInLowercase = tag.NameInLowercase;
			string attributeNameInLowercase = attribute.NameInLowercase;
			IList<HtmlAttribute> attributes = tag.Attributes;

			bool canMinify = false;

			if (tag.Flags.HasFlag(HtmlTagFlags.Custom))
			{
				string elementDirectiveName = AngularHelpers.NormalizeDirectiveName(tagNameInLowercase);

				switch (elementDirectiveName)
				{
					case "ngPluralize":
						canMinify = attributeNameInLowercase == "count" || attributeNameInLowercase == "when";
						break;
					case "ngMessages":
						canMinify = attributeNameInLowercase == "for";
						break;
				}
			}

			if (!canMinify)
			{
				string attributeDirectiveName = AngularHelpers.NormalizeDirectiveName(attributeNameInLowercase);
				canMinify = ContainsAngularBindingExpression(attributeDirectiveName);

				if (!canMinify)
				{
					switch (attributeDirectiveName)
					{
						case "ngTrueValue":
						case "ngFalseValue":
							canMinify = tagNameInLowercase == "input" && attributes.Any(
								a => a.NameInLowercase == "type" && a.Value.Trim().IgnoreCaseEquals("checkbox"));
							break;
					}
				}
			}

			return canMinify;
		}

		/// <summary>
		/// Minify a Angular binding expression
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="expression">Binding expression</param>
		/// <returns>Minified binding expression</returns>
		private string MinifyAngularBindingExpression(MarkupParsingContext context, string expression)
		{
			return MinifyAngularBindingExpression(context, SourceCodeNodeCoordinates.Empty, expression);
		}

		/// <summary>
		/// Minify a Angular binding expression
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="attributeCoordinates">Coordinates of attribute value</param>
		/// <param name="expression">Binding expression</param>
		/// <returns>Minified binding expression</returns>
		private string MinifyAngularBindingExpression(MarkupParsingContext context,
			SourceCodeNodeCoordinates attributeCoordinates, string expression)
		{
			return MinifyAngularBindingExpression(context, attributeCoordinates, SourceCodeNodeCoordinates.Empty,
				expression);
		}

		/// <summary>
		/// Minify a Angular binding expression
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="attributeCoordinates">Coordinates of attribute value</param>
		/// <param name="expressionCoordinates">Coordinates of expression</param>
		/// <param name="expression">Binding expression</param>
		/// <returns>Minified binding expression</returns>
		private string MinifyAngularBindingExpression(MarkupParsingContext context,
			SourceCodeNodeCoordinates attributeCoordinates, SourceCodeNodeCoordinates expressionCoordinates,
			string expression)
		{
			if (string.IsNullOrWhiteSpace(expression))
			{
				return expression;
			}

			string result = expression;
			CrockfordJsMinifier innerCrockfordJsMinifier = GetInnerCrockfordJsMinifierInstance();
			CodeMinificationResult minificationResult = innerCrockfordJsMinifier.Minify(result, true);
			IList<MinificationErrorInfo> errors = minificationResult.Errors;

			if (errors.Count == 0)
			{
				result = minificationResult.MinifiedContent ?? string.Empty;
			}
			else
			{
				SourceCodeNodeCoordinates absoluteErrorCoordinates = CalculateAbsoluteInlineCodeErrorCoordinates(
					context.NodeCoordinates, attributeCoordinates, expressionCoordinates);
				string sourceFragment = SourceCodeNavigator.GetSourceFragment(
					context.SourceCode, absoluteErrorCoordinates);
				string errorMessage = errors[0].Message;

				WriteError(LogCategoryConstants.JsTemplateMinificationError,
					string.Format(Strings.ErrorMessage_BindingExpressionMinificationFailed,
						"Angular", errorMessage.TrimEnd('.')),
					_fileContext,
					absoluteErrorCoordinates.LineNumber, absoluteErrorCoordinates.ColumnNumber,
					sourceFragment);
			}

			result = Utils.RemoveEndingSemicolon(result);

			return result;
		}

		#endregion

		private static SourceCodeNodeCoordinates CalculateAbsoluteInlineCodeErrorCoordinates(
			SourceCodeNodeCoordinates tagCoordinates, SourceCodeNodeCoordinates attributeCoordinates,
			SourceCodeNodeCoordinates relativeErrorCoordinates)
		{
			SourceCodeNodeCoordinates absoluteErrorCoordinates;
			if (!attributeCoordinates.IsEmpty)
			{
				absoluteErrorCoordinates = SourceCodeNavigator.CalculateAbsoluteNodeCoordinates(
					attributeCoordinates, relativeErrorCoordinates);
			}
			else
			{
				absoluteErrorCoordinates = SourceCodeNavigator.CalculateAbsoluteNodeCoordinates(
					tagCoordinates, relativeErrorCoordinates);
			}

			return absoluteErrorCoordinates;
		}

		/// <summary>
		/// Writes a information about the error
		/// </summary>
		/// <param name="category">Error category</param>
		/// <param name="message">Error message</param>
		/// <param name="filePath">File path</param>
		/// <param name="lineNumber">Line number on which the error occurred</param>
		/// <param name="columnNumber">Column number on which the error occurred</param>
		/// <param name="sourceFragment">Fragment of source svgContent</param>
		private void WriteError(string category, string message, string filePath, int lineNumber, int columnNumber,
			string sourceFragment)
		{
			_errors.Add(new MinificationErrorInfo(category, message, lineNumber, columnNumber, sourceFragment));
			_logger.Error(category, message, filePath, lineNumber, columnNumber, sourceFragment);
		}

		/// <summary>
		/// Writes a information about the warning
		/// </summary>
		/// <param name="category">Warning category</param>
		/// <param name="message">Warning message</param>
		/// <param name="filePath">File path</param>
		/// <param name="lineNumber">Line number on which the warning occurred</param>
		/// <param name="columnNumber">Column number on which the warning occurred</param>
		/// <param name="sourceFragment">Fragment of source svgContent</param>
		private void WriteWarning(string category, string message, string filePath, int lineNumber, int columnNumber,
			string sourceFragment)
		{
			_warnings.Add(new MinificationErrorInfo(category, message, lineNumber, columnNumber, sourceFragment));
			_logger.Warn(category, message, filePath, lineNumber, columnNumber, sourceFragment);
		}

		#region Internal types

		/// <summary>
		/// HTML attribute view model
		/// </summary>
		private struct HtmlAttributeViewModel
		{
			/// <summary>
			/// Name
			/// </summary>
			public readonly string Name;

			// Value
			public readonly string Value;

			/// <summary>
			/// Flag indicating whether the attribute has a value
			/// </summary>
			public readonly bool HasValue;

			/// <summary>
			/// Flag indicating whether the attribute value enclosed in quotes
			/// </summary>
			public readonly bool HasQuotes;

			/// <summary>
			/// Flag indicating whether the attribute is empty
			/// </summary>
			public readonly bool IsEmpty;

			/// <summary>
			/// Represents a empty HTML attribute view model
			/// </summary>
			public static readonly HtmlAttributeViewModel Empty = new HtmlAttributeViewModel(null, null, false);


			/// <summary>
			/// Constructs instance of HTML attribute view model
			/// </summary>
			/// <param name="name">Name</param>
			/// <param name="value">Value</param>
			/// <param name="hasQuotes">Flag indicating whether the attribute value enclosed in quotes</param>

			public HtmlAttributeViewModel(string name, string value, bool hasQuotes)
			{
				if (name != null)
				{
					Name = name;
					if (value != null)
					{
						Value = value;
						HasValue = true;
					}
					else
					{
						Value = string.Empty;
						HasValue = false;
					}
					HasQuotes = hasQuotes;
					IsEmpty = false;
				}
				else
				{
					Name = string.Empty;
					Value = string.Empty;
					HasValue = false;
					HasQuotes = false;
					IsEmpty = true;
				}
			}
		}

		#endregion
	}
}