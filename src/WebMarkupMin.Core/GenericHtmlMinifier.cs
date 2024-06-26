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
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

using AdvancedStringBuilder;

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
		/// Placeholder for embedded code
		/// </summary>
		const string EMBEDDED_CODE_PLACEHOLDER = "(embedded code)";

		const string HTTP_PROTOCOL = "http:";
		const string HTTPS_PROTOCOL = "https:";
		const string JS_PROTOCOL = "javascript:";

		const string JS_CONTENT_TYPE = "text/javascript";
		const string VBS_CONTENT_TYPE = "text/vbscript";
		const string CSS_CONTENT_TYPE = "text/css";

		/// <summary>
		/// Canonical document type of HTML5
		/// </summary>
		const string CANONICAL_HTML5_DOCTYPE = "<!DOCTYPE html>";

		const string BEGIN_NOINDEX_COMMENT = "noindex";
		const string END_NOINDEX_COMMENT = "/noindex";

		#region Regular expressions

		private static readonly Regex _metaContentTypeTagValueRegex = new Regex(@"^(?:[a-zA-Z0-9-+./]+);" +
			@"\s*charset=(?<charset>[a-zA-Z0-9-]+)$",
			RegexOptions.IgnoreCase | TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _jsProtocolRegex = new Regex(@"^javascript:\s*",
			RegexOptions.IgnoreCase | TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _separatingCommaWithSpacesRegex = new Regex(@"\s*,\s*",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _endingCommaWithSpacesRegex = new Regex(@"\s*,\s*$",
			TargetFrameworkShortcuts.PerformanceRegexOptions);

		private static readonly Regex _beginHtmlCommentRegex = new Regex(@"^\s*<!--(?:[ \t\v]*\r?\n)?",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _endHtmlCommentRegex = new Regex(@"(?:\r?\n[ \t\v]*)?-->\s*$",
			RegexOptions.RightToLeft | TargetFrameworkShortcuts.PerformanceRegexOptions);

		private static readonly Regex _beginCdataSectionRegex = new Regex(
			@"^\s*<!\[CDATA\[(?:[ \t\v]*\r?\n)?",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _endCdataSectionRegex = new Regex(@"(?:\r?\n[ \t\v]*)?\]\]>\s*$",
			RegexOptions.RightToLeft | TargetFrameworkShortcuts.PerformanceRegexOptions);

		private static readonly Regex _styleBeginCdataSectionRegex = new Regex(
			@"^\s*/\*\s*<!\[CDATA\[\s*\*/(?:[ \t\v]*\r?\n)?",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _styleEndCdataSectionRegex = new Regex(@"(?:\r?\n[ \t\v]*)?/\*\s*\]\]>\s*\*/\s*$",
			RegexOptions.RightToLeft | TargetFrameworkShortcuts.PerformanceRegexOptions);

		private static readonly Regex _styleBeginMaxCompatibleCdataSectionRegex = new Regex(
			@"^\s*<!--\s*/\*\s*--><!\[CDATA\[\s*/\*\s*><!--\s*\*/(?:[ \t\v]*\r?\n)?",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _styleEndMaxCompatibleCdataSectionRegex = new Regex(
			@"(?:\r?\n[ \t\v]*)?/\*\s*\]\]>\s*\*/\s*-->\s*$",
			RegexOptions.RightToLeft | TargetFrameworkShortcuts.PerformanceRegexOptions);

		private static readonly Regex _scriptBeginHtmlCommentRegex = new Regex(
			@"^\s*(?://[ \t\v]*)?<!--[ \t\v\S]*(?:\r?\n)?",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _scriptEndHtmlCommentRegex = new Regex(@"(?:\r?\n)?[ \t\v\S]*-->\s*$",
			RegexOptions.RightToLeft | TargetFrameworkShortcuts.PerformanceRegexOptions);

		private static readonly Regex _scriptBeginCdataSectionRegex = new Regex(
			@"^\s*(?://[ \t\v]*<!\[CDATA\[[ \t\v\S]*\r?\n|/\*\s*<!\[CDATA\[\s*\*/(?:[ \t\v]*\r?\n)?)",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _scriptEndCdataSectionRegex = new Regex(
			@"(?:\r?\n//[ \t\v\S]*\]\]>|(?:\r?\n[ \t\v]*)?/\*\s*\]\]>\s*\*/)\s*$",
			RegexOptions.RightToLeft | TargetFrameworkShortcuts.PerformanceRegexOptions);

		private static readonly Regex _scriptBeginMaxCompatibleCdataSectionRegex = new Regex(
			@"^\s*(?:<!--[ \t\v]*//[ \t\v]*--><!\[CDATA\[[ \t\v]*//[ \t\v]*><!--[ \t\v]*\r?\n" +
			@"|<!--\s*/\*\s*--><!\[CDATA\[\s*/\*\s*><!--\s*\*/(?:[ \t\v]*\r?\n)?)",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _scriptEndMaxCompatibleCdataSectionRegex = new Regex(
			@"(?:\r?\n[ \t\v]*//[ \t\v]*--><!\]\]>" +
			@"|(?:\r?\n[ \t\v]*)?/\*\s*\]\]>\s*\*/\s*-->)\s*$",
			RegexOptions.RightToLeft | TargetFrameworkShortcuts.PerformanceRegexOptions);

		private static readonly Regex _relExternalAttributeRegex = new Regex(@"^(?:alternate\s+)?external$",
			RegexOptions.IgnoreCase | TargetFrameworkShortcuts.PerformanceRegexOptions);

		#endregion

		#region Lists of tags and attributes

		private static readonly HashSet<string> _emptyAttributesForRemoval = new HashSet<string>
		{
			"class", "id", "name", "style", "title", "lang"
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
			// HTML5
			"text/javascript", "module",

			// Deprecated
			"text/x-javascript", "text/ecmascript", "text/x-ecmascript",
			"application/javascript", "application/x-javascript", "application/ecmascript", "application/x-ecmascript"
		};

		/// <summary>
		/// List of JSON content types
		/// </summary>
		private static readonly HashSet<string> _jsonContentTypes = new HashSet<string>
		{
			"application/json", "application/ld+json", "importmap",

			// Experimental
			"speculationrules"
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
		/// Default newline string
		/// </summary>
		private string _defaultNewLine;

		/// <summary>
		/// Default quote character used for attribute values
		/// </summary>
		private char _defaultQuoteChar;

		/// <summary>
		/// HTML minification output writer
		/// </summary>
		private readonly HtmlMinificationOutputWriter _output;

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
		/// Flag indicating, that the previous node has been removed
		/// </summary>
		private bool _previousNodeRemoved;

		/// <summary>
		/// List of the errors
		/// </summary>
		private readonly List<MinificationErrorInfo> _errors;

		/// <summary>
		/// List of the warnings
		/// </summary>
		private readonly List<MinificationErrorInfo> _warnings;

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

			_output = new HtmlMinificationOutputWriter(64, _settings.NewLineStyle);
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
					PreserveNewLines = _settings.PreserveNewLines,
					NewLineStyle = _settings.NewLineStyle,
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
			return Minify(content, string.Empty, TargetFrameworkShortcuts.DefaultTextEncoding, false);
		}

		/// <summary>
		/// Minify HTML content
		/// </summary>
		/// <param name="content">HTML content</param>
		/// <param name="fileContext">File context</param>
		/// <returns>Minification result</returns>
		public MarkupMinificationResult Minify(string content, string fileContext)
		{
			return Minify(content, fileContext, TargetFrameworkShortcuts.DefaultTextEncoding, false);
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
			return Minify(content, string.Empty, TargetFrameworkShortcuts.DefaultTextEncoding, generateStatistics);
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
			var stringBuilderPool = StringBuilderPool.Shared;
			StringBuilder sb = null;
			HtmlMinificationOutputWriter output = _output;
			var errors = new List<MinificationErrorInfo>();
			var warnings = new List<MinificationErrorInfo>();

			lock (_minificationSynchronizer)
			{
				_fileContext = fileContext;
				_encoding = encoding;
				_defaultNewLine = cleanedContent.GetNewLine() ?? Environment.NewLine;

				try
				{
					if (generateStatistics)
					{
						statistics = new MinificationStatistics(_encoding);
						statistics.Init(cleanedContent);
					}

					sb = stringBuilderPool.Rent(cleanedContent.Length);
					output.StringBuilder = sb;

					_htmlParser.Parse(cleanedContent);

					output.Flush();
					if (_settings.WhitespaceMinificationMode != WhitespaceMinificationMode.None
						&& _currentNodeType != HtmlNodeType.IgnoredFragment)
					{
						output.TrimEnd();
					}

					if (_errors.Count == 0)
					{
						minifiedContent = output.ToString();

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
					output.Clear();
					output.StringBuilder = null;
					stringBuilderPool.Return(sb);
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
					_defaultNewLine = null;
					_defaultQuoteChar = '\0';
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
			HtmlNodeType previousNodeType = _currentNodeType;
			_currentNodeType = HtmlNodeType.XmlDeclaration;

			HtmlMinificationOutputWriter output = _output;
			WhitespaceMinificationMode whitespaceMinificationMode = _settings.WhitespaceMinificationMode;

			if (whitespaceMinificationMode != WhitespaceMinificationMode.None)
			{
				// Processing of whitespace, that followed before the document type declaration
				output.TrimEndLastItem(_settings.PreserveNewLines);
			}

			if (_settings.UseXhtmlSyntax)
			{
				XmlMinifier innerXmlMinifier = GetInnerXmlMinifierInstance();
				MarkupMinificationResult minificationResult = innerXmlMinifier.Minify(xmlDeclaration);

				if (minificationResult.Errors.Count == 0)
				{
					output.Write(minificationResult.MinifiedContent);
					output.Flush();
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

				_previousNodeRemoved = false;
			}
			else
			{
				string sourceCode = context.SourceCode;
				SourceCodeNodeCoordinates xmlDeclarationCoordinates = context.NodeCoordinates;

				WriteWarning(LogCategoryConstants.HtmlMinificationWarning,
					Strings.WarningMessage_XmlDeclarationNotAllowed, _fileContext,
					xmlDeclarationCoordinates.LineNumber, xmlDeclarationCoordinates.ColumnNumber,
					SourceCodeNavigator.GetSourceFragment(sourceCode, xmlDeclarationCoordinates));

				_currentNodeType = previousNodeType;
				_previousNodeRemoved = true;
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

			HtmlMinificationOutputWriter output = _output;
			WhitespaceMinificationMode whitespaceMinificationMode = _settings.WhitespaceMinificationMode;

			if (whitespaceMinificationMode != WhitespaceMinificationMode.None)
			{
				// Processing of whitespace, that followed before the document type declaration
				output.TrimEndLastItem(_settings.PreserveNewLines);
			}

			string shortDoctype = _settings.CustomShortDoctype;
			if (string.IsNullOrWhiteSpace(shortDoctype))
			{
				shortDoctype = CANONICAL_HTML5_DOCTYPE;
			}

			output.Write(_settings.UseShortDoctype ? shortDoctype : doctype.CollapseWhitespace());
			output.Flush();

			_previousNodeRemoved = false;
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

			if (commentText.Equals(BEGIN_NOINDEX_COMMENT, StringComparison.OrdinalIgnoreCase))
			{
				// Processing of begin noindex comment
				processedCommentText = BEGIN_NOINDEX_COMMENT;
			}
			else if (commentText.Equals(END_NOINDEX_COMMENT, StringComparison.OrdinalIgnoreCase))
			{
				// Processing of end noindex comment
				processedCommentText = END_NOINDEX_COMMENT;
			}
			else if (KnockoutHelpers.IsEndContainerlessComment(commentText))
			{
				// Processing of end Knockout containerless comment
				processedCommentText = "/ko";
			}
			else if (KnockoutHelpers.IsStartContainerlessComment(commentText))
			{
				// Processing of begin Knockout containerless comment
				string koExpression = string.Empty;

				KnockoutHelpers.ParseStartContainerlessComment(commentText,
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
				string ngDirectiveName = string.Empty;
				string ngExpression = string.Empty;

				AngularHelpers.ParseCommentDirective(commentText,
					(localContext, directiveName, expression) =>
					{
						ngDirectiveName = directiveName;

						SourceCodeNodeCoordinates expressionCoordinates = localContext.NodeCoordinates;
						expressionCoordinates.ColumnNumber += beginCommentLength;

						ngExpression = expression;
						if (_settings.MinifyAngularBindingExpressions
							&& ContainsAngularBindingExpression(AngularHelpers.NormalizeDirectiveName(ngDirectiveName)))
						{
							ngExpression = MinifyAngularBindingExpression(context, SourceCodeNodeCoordinates.Empty,
								expressionCoordinates, expression);
						}
					}
				);

				processedCommentText = "directive:" + ngDirectiveName + " " + ngExpression;
			}
			else if (ReactHelpers.IsDomComponentComment(commentText)
				|| BlazorHelpers.IsMarker(commentText))
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
				HtmlMinificationOutputWriter output = _output;
				output.Write("<!--");
				if (processedCommentText.Length > 0)
				{
					output.Write(processedCommentText);
				}
				output.Write("-->");

				_previousNodeRemoved = false;
			}
			else
			{
				_currentNodeType = previousNodeType;
				_previousNodeRemoved = true;
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

			HtmlMinificationOutputWriter output = _output;
			output.Write("<![CDATA[");
			output.Write(cdataText);
			output.Write("]]>");

			_previousNodeRemoved = false;
		}

		/// <summary>
		/// If conditional comments handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="expression">Conditional expression</param>
		/// <param name="type">Conditional comment type</param>
		private void IfConditionalCommentHandler(MarkupParsingContext context,
			string expression, HtmlConditionalCommentType type)
		{
			_currentNodeType = HtmlNodeType.IfConditionalComment;

			string startPart;
			string endPart;

			switch (type)
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

			HtmlMinificationOutputWriter output = _output;
			output.Write(startPart);
			output.Write(expression);
			output.Write(endPart);

			_previousNodeRemoved = false;
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

			_output.Write(endIfComment);

			_previousNodeRemoved = false;
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
			List<HtmlAttribute> attributes = tag.Attributes;

			_currentNodeType = HtmlNodeType.StartTag;
			_currentTag = tag;
			_currentText = string.Empty;

			HtmlMinificationOutputWriter output = _output;
			WhitespaceMinificationMode whitespaceMinificationMode = _settings.WhitespaceMinificationMode;
			bool preserveNewLines = _settings.PreserveNewLines;
			bool allowWhitespaceMinification = false;

			// Set whitespace flags for nested tags (for example <span> within a <pre>)
			if (whitespaceMinificationMode != WhitespaceMinificationMode.None)
			{
				if (_tagsWithNotRemovableWhitespaceQueue.Count == 0)
				{
					allowWhitespaceMinification = true;

					// Processing of whitespace, that followed before the start tag
					bool allowTrimEnd = false;
					if (tagFlags.IsSet(HtmlTagFlags.Invisible)
						|| (tagFlags.IsSet(HtmlTagFlags.NonIndependent)
							&& CanRemoveWhitespaceBetweenNonIndependentTags(previousTag, tag)))
					{
						allowTrimEnd = true;
					}
					else
					{
						if (whitespaceMinificationMode == WhitespaceMinificationMode.Medium
							|| whitespaceMinificationMode == WhitespaceMinificationMode.Aggressive)
						{
							allowTrimEnd = tagFlags.IsSet(HtmlTagFlags.Block);
						}
					}

					if (allowTrimEnd)
					{
						output.TrimEndLastItem(preserveNewLines);
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
					&& previousTag.Flags.IsSet(HtmlTagFlags.Optional)
					&& CanRemoveOptionalEndTagByNextTag(previousTag, tag))
				{
					output.RemoveLastEndTag(previousTag.NameInLowercase);
					if (allowWhitespaceMinification)
					{
						output.CollapseLastWhitespaceItem(preserveNewLines);
					}
				}
			}

			output.Flush();

			output.Write("<");
			output.Write(CanPreserveCase() ? tagName : tagNameInLowercase);

			int attributeCount = attributes.Count;
			bool unsafeLastAttribute = false;

			if (attributeCount > 0)
			{
				for (int attributeIndex = 0; attributeIndex < attributeCount; attributeIndex++)
				{
					HtmlAttribute attribute = attributes[attributeIndex];
					if (_defaultQuoteChar == '\0' && attribute.QuoteChar != '\0')
					{
						_defaultQuoteChar = attribute.QuoteChar;
					}

					HtmlAttributeViewModel attributeViewModel = BuildAttributeViewModel(context, tag, attribute);
					if (!attributeViewModel.IsEmpty)
					{
						output.Write(" ");
						output.Write(attributeViewModel.Name);
						if (attributeViewModel.HasValue)
						{
							output.Write("=");
							if (attributeViewModel.HasQuotes)
							{
								output.Write(attributeViewModel.Quote);
							}
							output.Write(attributeViewModel.Value);
							if (attributeViewModel.HasQuotes)
							{
								output.Write(attributeViewModel.Quote);
							}
						}

						unsafeLastAttribute = attributeViewModel.HasValue && !attributeViewModel.HasQuotes;
					}
				}
			}

			if (tagFlags.IsSet(HtmlTagFlags.Empty))
			{
				HtmlEmptyTagRenderMode emptyTagRenderMode = _settings.EmptyTagRenderMode;

				if (emptyTagRenderMode == HtmlEmptyTagRenderMode.NoSlash && tagFlags.IsSet(HtmlTagFlags.Xml))
				{
					emptyTagRenderMode = HtmlEmptyTagRenderMode.SpaceAndSlash;
				}

				if (emptyTagRenderMode == HtmlEmptyTagRenderMode.Slash && unsafeLastAttribute)
				{
					emptyTagRenderMode = HtmlEmptyTagRenderMode.SpaceAndSlash;
				}

				if (emptyTagRenderMode == HtmlEmptyTagRenderMode.Slash)
				{
					output.Write("/");
				}
				else if (emptyTagRenderMode == HtmlEmptyTagRenderMode.SpaceAndSlash)
				{
					output.Write(" /");
				}
			}
			output.Write(">");

			_previousNodeRemoved = false;
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

			HtmlMinificationOutputWriter output = _output;
			WhitespaceMinificationMode whitespaceMinificationMode = _settings.WhitespaceMinificationMode;
			bool preserveNewLines = _settings.PreserveNewLines;
			bool allowWhitespaceMinification = false;

			if (whitespaceMinificationMode != WhitespaceMinificationMode.None)
			{
				if (_tagsWithNotRemovableWhitespaceQueue.Count == 0 && !tagFlags.IsSet(HtmlTagFlags.EmbeddedCode))
				{
					allowWhitespaceMinification = true;

					// Processing of whitespace, that followed before the end tag
					bool allowTrimEnd = false;
					if (tagFlags.IsSet(HtmlTagFlags.Invisible)
						|| (previousTag.Flags.IsSet(HtmlTagFlags.NonIndependent)
							&& CanRemoveWhitespaceAfterEndNonIndependentTagByParentTag(previousTag, tag)))
					{
						allowTrimEnd = true;
					}
					else
					{
						if (whitespaceMinificationMode == WhitespaceMinificationMode.Medium)
						{
							allowTrimEnd = tagFlags.IsSet(HtmlTagFlags.Block);
						}
						else if (whitespaceMinificationMode == WhitespaceMinificationMode.Aggressive)
						{
							allowTrimEnd = tagFlags.IsSet(HtmlTagFlags.Block)
								|| tagFlags.IsSet(HtmlTagFlags.Inline)
								|| tagFlags.IsSet(HtmlTagFlags.InlineBlock)
								;
						}
					}

					if (allowTrimEnd)
					{
						output.TrimEndLastItem(preserveNewLines);
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
				&& previousTag.Flags.IsSet(HtmlTagFlags.Optional)
				&& (previousNodeType == HtmlNodeType.EndTag
					|| (previousTagNameInLowercase != tagNameInLowercase && string.IsNullOrWhiteSpace(previousText)))
				&& CanRemoveOptionalEndTagByParentTag(previousTag, tag))
			{
				output.RemoveLastEndTag(previousTag.NameInLowercase);
				if (allowWhitespaceMinification)
				{
					output.CollapseLastWhitespaceItem(preserveNewLines);
				}
			}

			bool isElementEmpty = string.IsNullOrWhiteSpace(previousText)
				&& previousTagNameInLowercase == tagNameInLowercase
				&& previousNodeType != HtmlNodeType.EndTag;
			if (_settings.RemoveTagsWithoutContent && isElementEmpty
				&& CanRemoveTagWithoutContent(previousTag))
			{
				// Remove last "element" from buffer, return
				if (output.RemoveLastStartTag(tag.NameInLowercase))
				{
					output.Flush();

					_currentNodeType = HtmlNodeType.Unknown;
					_currentTag = HtmlTag.Empty;
					_currentText = string.Empty;
					_previousNodeRemoved = true;

					return;
				}
			}

			if (_settings.RemoveOptionalEndTags
				&& tagFlags.IsSet(HtmlTagFlags.Optional)
				&& CanRemoveSafeOptionalEndTag(tag))
			{
				// Leave only start tag in buffer
				output.Flush();

				_currentNodeType = previousNodeType;
				_currentTag = previousTag;
				_currentText = previousText;
				_previousNodeRemoved = true;

				return;
			}

			// Add end tag to buffer
			output.Write("</");
			output.Write(CanPreserveCase() ? tagName : tagNameInLowercase);
			output.Write(">");

			if (!_settings.RemoveOptionalEndTags)
			{
				output.Flush();
			}

			_previousNodeRemoved = false;
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
			bool preserveNewLines = _settings.PreserveNewLines;
			bool allowCollapseLastItem = false;

			if (whitespaceMinificationMode != WhitespaceMinificationMode.None)
			{
				if (_tagsWithNotRemovableWhitespaceQueue.Count == 0)
				{
					if (context.Position == 0)
					{
						// Processing of starting whitespace
						text = text.TrimStart(null);
					}
					else if (context.Position + text.Length == context.Length)
					{
						// Processing of ending whitespace
						text = text.TrimEnd(null);
					}
					else if (previousNodeType == HtmlNodeType.StartTag)
					{
						// Processing of whitespace, that followed after the start tag
						bool allowTrimStart = false;
						if (tagFlags.IsSet(HtmlTagFlags.Invisible)
							|| (tagFlags.IsSet(HtmlTagFlags.NonIndependent) && tagFlags.IsSet(HtmlTagFlags.Empty)))
						{
							allowTrimStart = true;
						}
						else
						{
							if (whitespaceMinificationMode == WhitespaceMinificationMode.Medium)
							{
								allowTrimStart = tagFlags.IsSet(HtmlTagFlags.Block);
							}
							else if (whitespaceMinificationMode == WhitespaceMinificationMode.Aggressive)
							{
								allowTrimStart = tagFlags.IsSet(HtmlTagFlags.Block)
									|| ((tagFlags.IsSet(HtmlTagFlags.Inline) || tagFlags.IsSet(HtmlTagFlags.InlineBlock))
										&& !tagFlags.IsSet(HtmlTagFlags.Empty));
							}
						}

						if (allowTrimStart)
						{
							text = text.TrimStart(preserveNewLines);
						}
					}
					else if (previousNodeType == HtmlNodeType.EndTag)
					{
						// Processing of whitespace, that followed after the end tag
						bool allowTrimStart = false;
						if (tagFlags.IsSet(HtmlTagFlags.Invisible)
							|| (tagFlags.IsSet(HtmlTagFlags.NonIndependent)
								&& CanRemoveWhitespaceAfterEndNonIndependentTag(tag)))
						{
							allowTrimStart = true;
						}
						else
						{
							if (whitespaceMinificationMode == WhitespaceMinificationMode.Medium
								|| whitespaceMinificationMode == WhitespaceMinificationMode.Aggressive)
							{
								allowTrimStart = tagFlags.IsSet(HtmlTagFlags.Block);
							}
						}

						if (allowTrimStart)
						{
							text = text.TrimStart(preserveNewLines);
						}
					}
					else if (previousNodeType == HtmlNodeType.Doctype || previousNodeType == HtmlNodeType.XmlDeclaration)
					{
						// Processing of whitespace, that followed after the document type declaration
						// or XML declaration
						text = text.TrimStart(preserveNewLines);
					}

					if (!(tagFlags.IsSet(HtmlTagFlags.Xml) && tagFlags.IsSet(HtmlTagFlags.NonIndependent)))
					{
						text = text.CollapseWhitespace(preserveNewLines);
						allowCollapseLastItem = _previousNodeRemoved;
					}
				}
				else if (previousNodeType == HtmlNodeType.StartTag && tagNameInLowercase == "textarea"
					&& string.IsNullOrWhiteSpace(text))
				{
					text = preserveNewLines ? text.GetNewLine() : string.Empty;
				}
			}

			if (text.Length > 0)
			{
				HtmlMinificationOutputWriter output = _output;
				output.Write(text);
				if (allowCollapseLastItem)
				{
					output.CollapseLastWhitespaceItem(preserveNewLines);
				}

				_previousNodeRemoved = false;
			}
			else
			{
				_currentNodeType = previousNodeType;
				_previousNodeRemoved = true;
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
			List<HtmlAttribute> attributes = tag.Attributes;

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
				_previousNodeRemoved = true;
			}
			else
			{
				_previousNodeRemoved = false;
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

			HtmlMinificationOutputWriter output = _output;
			output.Write(startDelimiter);
			output.Write(processedExpression);
			output.Write(endDelimiter);

			_previousNodeRemoved = false;
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
				HtmlMinificationOutputWriter output = _output;
				output.Write(fragment);
				output.Flush();
			}

			_previousNodeRemoved = false;
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
				|| (_settings.RemoveJsTypeAttributes && IsJsTypeAttribute(tag, attribute))
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
				var stringBuilderPool = StringBuilderPool.Shared;
				StringBuilder attributeValueBuilder = stringBuilderPool.Rent();

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
							processedTextValue = textValue.CollapseWhitespace();
						}

						attributeValueBuilder.Append(processedTextValue);
					}
				);

				string processedAttributeValue = attributeValueBuilder.ToString();
				stringBuilderPool.Return(attributeValueBuilder);

				switch (attributeType)
				{
					case HtmlAttributeType.Uri:
					case HtmlAttributeType.Numeric:
					case HtmlAttributeType.ClassName:
						processedAttributeValue = processedAttributeValue.Trim();
						break;
					case HtmlAttributeType.Style:
						processedAttributeValue = processedAttributeValue.Trim();
						processedAttributeValue = Utils.RemoveEndingSemicolons(processedAttributeValue);
						break;
					default:
						if (_settings.MinifyAngularBindingExpressions && tag.Flags.IsSet(HtmlTagFlags.Custom))
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
			char recommendedQuoteChar = HtmlAttributeValueHelpers.GetAttributeQuoteCharByStyleEnum(
				_settings.AttributeQuotesStyle, attribute.Value, attribute.QuoteChar, _defaultQuoteChar);
			char quoteChar = addQuotes ? recommendedQuoteChar : '\0';
			string displayAttributeName = CanPreserveCase() ? attribute.Name : attribute.NameInLowercase;
			string encodedAttributeValue = !omitValue ?
				HtmlAttributeValueHelpers.Encode(attribute.Value, quoteChar) : null;

			var attributeViewModel = new HtmlAttributeViewModel(displayAttributeName, encodedAttributeValue,
				quoteChar);

			return attributeViewModel;
		}

		/// <summary>
		/// Determines whether a list of attributes contains the <c>rel</c> attribute with
		/// value, that equals to "external" or "alternate external"
		/// </summary>
		/// <param name="attributes">List of attributes</param>
		/// <returns>Result of check (<c>true</c> - contains; <c>false</c> - does not contain)</returns>
		private static bool ContainsRelExternalAttribute(List<HtmlAttribute> attributes)
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
		/// <returns>Result of check (<c>true</c> - can remove; <c>false</c> - cannot remove)</returns>
		private static bool CanRemoveAttributeQuotes(HtmlAttribute attribute,
			HtmlAttributeQuotesRemovalMode attributeQuotesRemovalMode)
		{
			string attributeValue = attribute.Value;
			bool result = false;

			switch (attributeQuotesRemovalMode)
			{
				case HtmlAttributeQuotesRemovalMode.KeepQuotes:
					result = false;
					break;
				case HtmlAttributeQuotesRemovalMode.Html4:
					result = HtmlAttributeValueHelpers.IsNotRequireQuotesInHtml4(attributeValue);
					break;
				case HtmlAttributeQuotesRemovalMode.Html5:
					result = HtmlAttributeValueHelpers.IsNotRequireQuotesInHtml5(attributeValue);
					break;
				default:
					result = false;
					break;
			}

			return result;
		}

		/// <summary>
		/// Checks whether the attribute is redundant
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <param name="attribute">Attribute</param>
		/// <returns>Result of check (<c>true</c> - is redundant; <c>false</c> - is not redundant)</returns>
		private static bool IsAttributeRedundant(HtmlTag tag, HtmlAttribute attribute)
		{
			string tagNameInLowercase = tag.NameInLowercase;
			List<HtmlAttribute> attributes = tag.Attributes;
			string attributeNameInLowercase = attribute.NameInLowercase;
			string attributeValue = attribute.Value;
			bool isAttributeRedundant = false;

			switch (tagNameInLowercase)
			{
				case "a":
					isAttributeRedundant = attributeNameInLowercase == "name" && attributes.Any(
						a => a.NameInLowercase == "id" && a.Value == attributeValue);
					break;

				case "area":
					isAttributeRedundant = attributeNameInLowercase == "shape"
						&& attributeValue.IgnoreCaseEquals("rect");
					break;

				case "button":
					isAttributeRedundant = attributeNameInLowercase == "type"
						&& attributeValue.IgnoreCaseEquals("submit");
					break;

				case "form":
					isAttributeRedundant = (attributeNameInLowercase == "autocomplete" && attributeValue.IgnoreCaseEquals("on"))
						|| (attributeNameInLowercase == "enctype" && attributeValue.IgnoreCaseEquals("application/x-www-form-urlencoded"))
						|| (attributeNameInLowercase == "method" && attributeValue.IgnoreCaseEquals("get"));
					break;

				case "img":
					isAttributeRedundant = attributeNameInLowercase == "decoding" && attributeValue.IgnoreCaseEquals("auto");
					break;

				case "input":
					isAttributeRedundant = attributeNameInLowercase == "type"
						&& attributeValue.IgnoreCaseEquals("text");
					break;

				case "script":
					isAttributeRedundant = (attributeNameInLowercase == "charset" && attributes.All(a => a.NameInLowercase != "src"))
						|| (attributeNameInLowercase == "language" && attributeValue.IgnoreCaseEquals("javascript"));
					break;

				case "textarea":
					isAttributeRedundant = attributeNameInLowercase == "wrap" && attributeValue.IgnoreCaseEquals("soft");
					break;

				case "track":
					isAttributeRedundant = attributeNameInLowercase == "kind" && attributeValue.IgnoreCaseEquals("subtitles");
					break;
			}

			return isAttributeRedundant;
		}

		/// <summary>
		/// Checks whether attribute is the attribute <c>type</c> of
		/// tag <c>script</c> containing the default content type
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <param name="attribute">Attribute</param>
		/// <returns>Result of check</returns>
		private static bool IsJsTypeAttribute(HtmlTag tag, HtmlAttribute attribute)
		{
			return tag.NameInLowercase == "script" && attribute.NameInLowercase == "type"
				&& attribute.Value.Trim().IgnoreCaseEquals(JS_CONTENT_TYPE);
		}

		/// <summary>
		/// Checks whether attribute is the attribute <c>type</c> of tag <c>link</c>
		/// or <c>style</c> containing the default content type
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <param name="attribute">Attribute</param>
		/// <returns>Result of check</returns>
		private static bool IsCssTypeAttribute(HtmlTag tag, HtmlAttribute attribute)
		{
			string tagNameInLowercase = tag.NameInLowercase;
			string attributeNameInLowercase = attribute.NameInLowercase;
			string attributeValue = attribute.Value;
			List<HtmlAttribute> attributes = tag.Attributes;
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
			List<HtmlAttribute> attributes = tag.Attributes;
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
						var stringBuilderPool = StringBuilderPool.Shared;
						StringBuilder classNameBuilder = stringBuilderPool.Rent();

						try
						{
							AngularHelpers.ParseClassDirective(processedAttributeValue,
								(localContext, directiveName, expression, endsWithSemicolon) =>
								{
									bool isDirective = expression.Length > 0 || endsWithSemicolon;
									if (isDirective)
									{
										int builderLength = classNameBuilder.Length;
										if (builderLength >= 2
											&& classNameBuilder[builderLength - 1] == ' '
											&& classNameBuilder[builderLength - 2] == ';')
										{
											classNameBuilder.TrimEnd();
										}
									}

									string processedExpression = expression;
									if (_settings.MinifyAngularBindingExpressions
										&& ContainsAngularBindingExpression(AngularHelpers.NormalizeDirectiveName(directiveName)))
									{
										processedExpression = MinifyAngularBindingExpression(context,
											attribute.ValueCoordinates, localContext.NodeCoordinates,
											expression);
									}

									classNameBuilder.Append(directiveName);
									if (!string.IsNullOrWhiteSpace(processedExpression))
									{
										classNameBuilder.Append(":");
										classNameBuilder.Append(processedExpression);
									}
									if (endsWithSemicolon)
									{
										classNameBuilder.Append(";");
									}
								},
								(localContext, content) =>
								{
									string processedContent = content.CollapseWhitespace();
									classNameBuilder.Append(processedContent);
								}
							);

							// Remove a trailing whitespaces
							classNameBuilder
								.TrimStart()
								.TrimEnd()
								;

							// Remove a ending semicolon
							int classNameBuilderLength = classNameBuilder.Length;
							if (classNameBuilder[classNameBuilderLength - 1] == ';')
							{
								classNameBuilder.Length = classNameBuilderLength - 1;
							}

							processedAttributeValue = classNameBuilder.ToString();
						}
						finally
						{
							stringBuilderPool.Return(classNameBuilder);
						}
					}
					else
					{
						processedAttributeValue = processedAttributeValue.Trim();
						processedAttributeValue = processedAttributeValue.CollapseWhitespace();
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
						processedAttributeValue = processedAttributeValue.CollapseWhitespace();
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
		/// <returns>Result of check (<c>true</c> - can be preserved; <c>false</c> - can not be preserved)</returns>
		private bool CanPreserveCase()
		{
			return _settings.PreserveCase || _currentTag.Flags.IsSet(HtmlTagFlags.Xml);
		}

		/// <summary>
		/// Checks whether remove an the attribute
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <param name="attribute">Attribute</param>
		/// <returns>Result of check (<c>true</c> - can be removed; <c>false</c> - can not be removed)</returns>
		private bool CanRemoveAttribute(HtmlTag tag, HtmlAttribute attribute)
		{
			IList<HtmlAttributeExpression> expressions = _settings.PreservableAttributeCollection;
			int expressionCount = expressions.Count;

			if (expressionCount == 0)
			{
				return true;
			}

			string tagNameInLowercase = tag.NameInLowercase;
			string attributeNameInLowercase = attribute.NameInLowercase;
			string attributeValue = attribute.Value;

			bool result = true;

			for (int expressionIndex = 0; expressionIndex < expressionCount; expressionIndex++)
			{
				bool cannotRemove = expressions[expressionIndex].IsMatch(tagNameInLowercase,
					attributeNameInLowercase, attributeValue);
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
		/// <returns>Result of check (<c>true</c> - can be removed; <c>false</c> - can not be removed)</returns>
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
		/// Checks whether remove an the <c>xmlns</c> attribute
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <param name="attribute">Attribute</param>
		/// <returns>Result of check (<c>true</c> - can be removed; <c>false</c> - can not be removed)</returns>
		private static bool CanRemoveXmlNamespaceAttribute(HtmlTag tag, HtmlAttribute attribute)
		{
			return tag.NameInLowercase == "html" && attribute.NameInLowercase == "xmlns";
		}

		/// <summary>
		/// Checks whether the tag is a META content-type tag
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <returns>Result of check (<c>true</c> - is META content-type tag; <c>false</c> - is not META content-type tag)</returns>
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

			HtmlAttribute contentAttribute = tag.Attributes.FirstOrDefault(a => a.NameInLowercase == "content");
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
								new HtmlAttribute("charset", "charset", charset, contentAttribute.QuoteChar,
									HtmlAttributeType.Text)
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
		/// <returns>Result of check (<c>true</c> - can be removed; <c>false</c> - can not be removed)</returns>
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
		/// <returns>Result of check (<c>true</c> - can be removed; <c>false</c> - can not be removed)</returns>
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
		/// <returns>Result of check (<c>true</c> - can be removed; <c>false</c> - can not be removed)</returns>
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
		/// <returns>Result of check (<c>true</c> - can be removed; <c>false</c> - can not be removed)</returns>
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
		/// <returns>Result of check (<c>true</c> - can be removed; <c>false</c> - can not be removed)</returns>
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
		/// <returns>Result of check (<c>true</c> - can be removed; <c>false</c> - can not be removed)</returns>
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
		/// <returns>Result of check (<c>true</c> - can be removed; <c>false</c> - can not be removed)</returns>
		private static bool CanRemoveTagWithoutContent(HtmlTag tag)
		{
			string tagNameInLowercase = tag.NameInLowercase;
			HtmlTagFlags tagFlags = tag.Flags;
			List<HtmlAttribute> attributes = tag.Attributes;

			return !(tagFlags.IsSet(HtmlTagFlags.Custom)
				|| (tagFlags.IsSet(HtmlTagFlags.Xml) && tagFlags.IsSet(HtmlTagFlags.NonIndependent))
				|| _unremovableEmptyTags.Contains(tagNameInLowercase)
				|| attributes.Any(a => IsCustomAttribute(a)
					|| (_unremovableEmptyTagAttributes.Contains(a.NameInLowercase) && !string.IsNullOrWhiteSpace(a.Value))));
		}

		/// <summary>
		/// Checks whether to minify whitespaces in text content of tag
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <returns>Result of check (<c>true</c> - can minify whitespaces; <c>false</c> - can not minify whitespaces)</returns>
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
			bool minifyWhitespace = _settings.WhitespaceMinificationMode != WhitespaceMinificationMode.None;

			bool isJavaScript = false;
			bool isJson = false;
			bool isVbScript = false;

			switch (processedContentType)
			{
				case string s when IsJsContentType(s):
					isJavaScript = true;
					break;

				case string s when _jsonContentTypes.Contains(s):
					isJson = true;
					break;

				case VBS_CONTENT_TYPE:
					isVbScript = true;
					break;
			}

			HtmlMinificationOutputWriter output = _output;

			if (isJavaScript || isJson || isVbScript)
			{
				bool removeHtmlComments = _settings.RemoveHtmlCommentsFromScriptsAndStyles;
				bool removeCdataSections = _settings.RemoveCdataSectionsFromScriptsAndStyles
					&& !_currentTag.Flags.IsSet(HtmlTagFlags.Xml);

				string startPart = string.Empty;
				string endPart = string.Empty;
				string newLine = code.GetNewLine() ?? _defaultNewLine;
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
					// Processing of JSON or VBScript code
					if (_beginCdataSectionRegex.IsMatch(content))
					{
						beforeCodeContent = _beginCdataSectionRegex.Match(content).Value;

						if (!removeCdataSections)
						{
							startPart = "<![CDATA[";
							endPart = "]]>";
						}

						code = Utils.RemovePrefixAndPostfix(content, _beginCdataSectionRegex, _endCdataSectionRegex);
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

					if (isJson && _settings.MinifyEmbeddedJsonData)
					{
						CrockfordJsMinifier innerCrockfordJsMinifier = GetInnerCrockfordJsMinifierInstance();
						CodeMinificationResult minificationResult = innerCrockfordJsMinifier.Minify(code, false);
						IList<MinificationErrorInfo> errors = minificationResult.Errors;

						if (errors.Count == 0)
						{
							code = minificationResult.MinifiedContent ?? string.Empty;
						}
						else
						{
							string sourceCode = context.SourceCode;
							var documentCoordinates = SourceCodeNavigator.CalculateAbsoluteNodeCoordinates(
								context.NodeCoordinates, beforeCodeContent);

							MinificationErrorInfo error = errors[0];
							var relativeErrorCoordinates = new SourceCodeNodeCoordinates(error.LineNumber,
								error.ColumnNumber);
							var absoluteErrorCoordinates = SourceCodeNavigator.CalculateAbsoluteNodeCoordinates(
								documentCoordinates, relativeErrorCoordinates);
							string sourceFragment = SourceCodeNavigator.GetSourceFragment(
								sourceCode, absoluteErrorCoordinates);
							string message = error.Message.Trim();

							WriteError(LogCategoryConstants.JsMinificationError, message, _fileContext,
								absoluteErrorCoordinates.LineNumber, absoluteErrorCoordinates.ColumnNumber,
								sourceFragment);
						}
					}
				}

				if (minifyWhitespace && code.Length > 0)
				{
					code = code.Trim(_settings.PreserveNewLines);
				}

				if (startPart.Length > 0)
				{
					output.Write(startPart);
					if (newLine.Length > 0 && !code.StartsWithNewLine())
					{
						output.Write(newLine);
					}
					isNotEmpty = true;
				}
				if (code.Length > 0)
				{
					output.Write(code);
					isNotEmpty = true;
				}
				if (endPart.Length > 0)
				{
					if (newLine.Length > 0 && !code.EndsWithNewLine())
					{
						output.Write(newLine);
					}
					output.Write(endPart);
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
					code = code.Trim(_settings.PreserveNewLines);
				}
			}

			if (code.Length > 0)
			{
				output.Write(code);
				isNotEmpty = true;
			}

			_currentText = isNotEmpty ? EMBEDDED_CODE_PLACEHOLDER : string.Empty;
		}

		[MethodImpl((MethodImplOptions)256 /* AggressiveInlining */)]
		private static bool IsJsContentType(string contentType)
		{
			string processedContentType = contentType;
			int semicolonPosition = contentType.IndexOf(';');

			if (semicolonPosition != -1)
			{
				processedContentType = contentType.Substring(0, semicolonPosition);
			}

			return _jsContentTypes.Contains(processedContentType);
		}

		/// <summary>
		/// Processes a embedded style content
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="content">Embedded style content</param>
		/// <param name="contentType">Content type (MIME type) of the style</param>
		private void ProcessEmbeddedStyleContent(MarkupParsingContext context, string content, string contentType)
		{
			string code = content;
			bool isNotEmpty = false;
			string processedContentType = !string.IsNullOrWhiteSpace(contentType) ?
				contentType.Trim().ToLowerInvariant() : CSS_CONTENT_TYPE;
			bool minifyWhitespace = _settings.WhitespaceMinificationMode != WhitespaceMinificationMode.None;
			HtmlMinificationOutputWriter output = _output;

			if (processedContentType == CSS_CONTENT_TYPE)
			{
				bool removeHtmlComments = _settings.RemoveHtmlCommentsFromScriptsAndStyles;
				bool removeCdataSections = _settings.RemoveCdataSectionsFromScriptsAndStyles
					&& !_currentTag.Flags.IsSet(HtmlTagFlags.Xml);

				string startPart = string.Empty;
				string endPart = string.Empty;
				string beforeCodeContent = string.Empty;

				if (_beginCdataSectionRegex.IsMatch(content))
				{
					beforeCodeContent = _beginCdataSectionRegex.Match(content).Value;

					if (!removeCdataSections)
					{
						startPart = "<![CDATA[";
						endPart = "]]>";
					}

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

				if (_settings.MinifyEmbeddedCssCode)
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
					code = code.Trim(_settings.PreserveNewLines);
				}

				if (startPart.Length > 0)
				{
					output.Write(startPart);
					isNotEmpty = true;
				}
				if (code.Length > 0)
				{
					output.Write(code);
					isNotEmpty = true;
				}
				if (endPart.Length > 0)
				{
					output.Write(endPart);
					isNotEmpty = true;
				}
			}
			else
			{
				if (minifyWhitespace && code.Length > 0)
				{
					code = code.Trim(_settings.PreserveNewLines);
				}

				if (code.Length > 0)
				{
					output.Write(code);
					isNotEmpty = true;
				}
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

			result = Utils.RemoveEndingSemicolons(result);

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

			result = Utils.RemoveEndingSemicolons(result);

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
		/// <param name="normalizedDirectiveName">Normalized directive name</param>
		/// <returns>Result of check (<c>true</c> - contains; <c>false</c> - not contains)</returns>
		private bool ContainsAngularBindingExpression(string normalizedDirectiveName)
		{
			bool result = !string.IsNullOrEmpty(normalizedDirectiveName)
				&& _angularDirectivesWithExpressions.Contains(normalizedDirectiveName);

			return result;
		}

		/// <summary>
		/// Checks whether to minify the Angular binding expression in attribute
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <param name="attribute">Attribute</param>
		/// <returns>Result of check (<c>true</c> - can minify expression; <c>false</c> - can not minify expression)</returns>
		private bool CanMinifyAngularBindingExpressionInAttribute(HtmlTag tag, HtmlAttribute attribute)
		{
			string tagNameInLowercase = tag.NameInLowercase;
			string attributeNameInLowercase = attribute.NameInLowercase;
			List<HtmlAttribute> attributes = tag.Attributes;

			bool canMinify = false;

			if (tag.Flags.IsSet(HtmlTagFlags.Custom))
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
			CodeMinificationResult minificationResult = innerCrockfordJsMinifier.MinifyAngularBindingExpression(result);
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

			result = Utils.RemoveEndingSemicolons(result);

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
		/// <param name="sourceFragment">Fragment of source content</param>
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
		/// <param name="sourceFragment">Fragment of source content</param>
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
		private readonly struct HtmlAttributeViewModel
		{
			/// <summary>
			/// Name
			/// </summary>
			public readonly string Name;

			/// <summary>
			/// Value
			/// </summary>
			public readonly string Value;

			/// <summary>
			/// Flag indicating whether the attribute has a value
			/// </summary>
			public readonly bool HasValue;

			/// <summary>
			/// Quote
			/// </summary>
			public readonly string Quote;

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
			public static readonly HtmlAttributeViewModel Empty = new HtmlAttributeViewModel(null, null, '\0');


			/// <summary>
			/// Constructs instance of HTML attribute view model
			/// </summary>
			/// <param name="name">Name</param>
			/// <param name="value">Value</param>
			/// <param name="quoteChar">Quote character</param>

			public HtmlAttributeViewModel(string name, string value, char quoteChar)
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
					Quote = HtmlAttributeValueHelpers.ConvertAttributeQuoteCharToString(quoteChar);
					HasQuotes = quoteChar != '\0';
					IsEmpty = false;
				}
				else
				{
					Name = string.Empty;
					Value = string.Empty;
					HasValue = false;
					Quote = string.Empty;
					HasQuotes = false;
					IsEmpty = true;
				}
			}
		}

		#endregion
	}
}