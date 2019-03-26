using System;
using System.Collections.Generic;
using System.Text;

using WebMarkupMin.Core.Helpers;
using WebMarkupMin.Core.Loggers;
using WebMarkupMin.Core.Parsers;
using WebMarkupMin.Core.Resources;
using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// XML minifier
	/// </summary>
	public sealed class XmlMinifier : IMarkupMinifier
	{
		/// <summary>
		/// Average compression ratio
		/// </summary>
		const double AVERAGE_COMPRESSION_RATIO = 0.85;

		/// <summary>
		/// XML minification settings
		/// </summary>
		private readonly XmlMinificationSettings _settings;

		/// <summary>
		/// Logger
		/// </summary>
		private readonly ILogger _logger;

		/// <summary>
		/// XML parser
		/// </summary>
		private readonly XmlParser _xmlParser;

		/// <summary>
		/// XML minification output writer
		/// </summary>
		private readonly XmlMinificationOutputWriter _output;

		/// <summary>
		/// Current node type
		/// </summary>
		private XmlNodeType _currentNodeType;

		/// <summary>
		/// Current text
		/// </summary>
		private string _currentText;

		/// <summary>
		/// Flag indicating, that before the text node was located the XML declaration
		/// </summary>
		private bool _xmlDeclarationBeforeText;

		/// <summary>
		/// Flag indicating, that before the text node was located the processing instruction
		/// </summary>
		private bool _processingInstructionBeforeText;

		/// <summary>
		/// Flag indicating, that before the text node was located the document type
		/// </summary>
		private bool _doctypeBeforeText;

		/// <summary>
		/// Flag indicating, that before the text node was located the start tag
		/// </summary>
		private bool _startTagBeforeText;

		/// <summary>
		/// Flag indicating, that before the text node was located the end tag
		/// </summary>
		private bool _endTagBeforeText;

		/// <summary>
		/// Flag indicating, that before the text node was located the empty tag
		/// </summary>
		private bool _emptyTagBeforeText;

		/// <summary>
		/// Flag indicating, that before the text node was located the ignored fragment
		/// </summary>
		private bool _ignoredFragmentBeforeText;

		/// <summary>
		/// List of the errors
		/// </summary>
		private readonly IList<MinificationErrorInfo> _errors;

		/// <summary>
		/// Synchronizer of minification
		/// </summary>
		private readonly object _minificationSynchronizer = new object();


		/// <summary>
		/// Constructs instance of XML minifier
		/// </summary>
		/// <param name="settings">XML minification settings</param>
		/// <param name="logger">Logger</param>
		public XmlMinifier(XmlMinificationSettings settings = null, ILogger logger = null)
		{
			_settings = settings ?? new XmlMinificationSettings();
			_logger = logger ?? new NullLogger();
			_xmlParser = new XmlParser(new XmlParsingHandlers
			{
				XmlDeclaration = XmlDeclarationHandler,
				ProcessingInstruction = ProcessingInstructionHandler,
				Doctype = DoctypeDelegateHandler,
				Comment = CommentHandler,
				CdataSection = CdataSectionHandler,
				StartTag = StartTagHandler,
				EndTag = EndTagHandler,
				EmptyTag = EmptyTagHandler,
				Text = TextHandler,
				IgnoredFragment = IgnoredFragmentHandler
			});

			_output = new XmlMinificationOutputWriter(48);
			_errors = new List<MinificationErrorInfo>();
			_currentNodeType = XmlNodeType.Unknown;
			_currentText = string.Empty;
		}


		/// <summary>
		/// Minify XML content
		/// </summary>
		/// <param name="content">Text content</param>
		/// <returns>Minification result</returns>
		public MarkupMinificationResult Minify(string content)
		{
			return Minify(content, null, Encoding.GetEncoding(0), false);
		}

		/// <summary>
		/// Minify XML content
		/// </summary>
		/// <param name="content">Text content</param>
		/// <param name="fileContext">File context</param>
		/// <returns>Minification result</returns>
		public MarkupMinificationResult Minify(string content, string fileContext)
		{
			return Minify(content, fileContext, Encoding.GetEncoding(0), false);
		}

		/// <summary>
		/// Minify XML content
		/// </summary>
		/// <param name="content">Text content</param>
		/// <param name="encoding">Text encoding</param>
		/// <returns>Minification result</returns>
		public MarkupMinificationResult Minify(string content, Encoding encoding)
		{
			return Minify(content, string.Empty, encoding, false);
		}

		/// <summary>
		/// Minify XML content
		/// </summary>
		/// <param name="content">Text content</param>
		/// <param name="generateStatistics">Flag for whether to allow generate minification statistics</param>
		/// <returns>Minification result</returns>
		public MarkupMinificationResult Minify(string content, bool generateStatistics)
		{
			return Minify(content, string.Empty, Encoding.GetEncoding(0), generateStatistics);
		}

		/// <summary>
		/// Minify XML content
		/// </summary>
		/// <param name="content">XML content</param>
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
			StringBuilder sb = null;
			var errors = new List<MinificationErrorInfo>();

			lock (_minificationSynchronizer)
			{
				try
				{
					if (generateStatistics)
					{
						statistics = new MinificationStatistics(encoding);
						statistics.Init(cleanedContent);
					}

					int estimatedCapacity = (int)Math.Floor(cleanedContent.Length * AVERAGE_COMPRESSION_RATIO);
					sb = StringBuilderPool.GetBuilder(estimatedCapacity);
					_output.StringBuilder = sb;

					_xmlParser.Parse(cleanedContent);

					_output.Flush();

					if (_errors.Count == 0)
					{
						minifiedContent = _output.ToString();

						if (generateStatistics)
						{
							statistics.End(minifiedContent);
						}
					}
				}
				catch (MarkupParsingException e)
				{
					WriteError(LogCategoryConstants.XmlParsingError, e.Message, fileContext,
						e.LineNumber, e.ColumnNumber, e.SourceFragment);
				}
				finally
				{
					_output.Clear();
					_output.StringBuilder = null;
					StringBuilderPool.ReleaseBuilder(sb);
					_currentNodeType = XmlNodeType.Unknown;
					_currentText = string.Empty;

					errors.AddRange(_errors);
					_errors.Clear();
				}

				if (errors.Count == 0)
				{
					_logger.Info(LogCategoryConstants.XmlMinificationSuccess,
						string.Format(Strings.SuccesMessage_MarkupMinificationComplete, "XML"),
						fileContext, statistics);
				}
			}

			return new MarkupMinificationResult(minifiedContent, errors, statistics);
		}

		#region Handlers

		/// <summary>
		/// XML declaration handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="attributes">List of attributes</param>
		private void XmlDeclarationHandler(MarkupParsingContext context, IList<XmlAttribute> attributes)
		{
			_currentNodeType = XmlNodeType.XmlDeclaration;

			if (_settings.MinifyWhitespace)
			{
				_output.RemoveLastWhitespaceItems();
			}

			_output.Write("<?xml");
			WriteAttributes(attributes);
			_output.Write("?>");

			_output.Flush();
		}

		/// <summary>
		/// Processing instruction handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="instructionName">Instruction name</param>
		/// <param name="attributes">List of attributes</param>
		private void ProcessingInstructionHandler(MarkupParsingContext context, string instructionName,
			IList<XmlAttribute> attributes)
		{
			_currentNodeType = XmlNodeType.ProcessingInstruction;

			if (_settings.MinifyWhitespace)
			{
				_output.RemoveLastWhitespaceItems();
			}

			_output.Write("<?");
			_output.Write(instructionName);
			WriteAttributes(attributes);
			_output.Write("?>");

			_output.Flush();
		}

		/// <summary>
		/// Document type declaration handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="doctype">Document type declaration</param>
		private void DoctypeDelegateHandler(MarkupParsingContext context, string doctype)
		{
			_currentNodeType = XmlNodeType.Doctype;

			if (_settings.MinifyWhitespace)
			{
				_output.RemoveLastWhitespaceItems();
			}

			_output.Write(Utils.CollapseWhitespace(doctype));
			_output.Flush();
		}

		/// <summary>
		/// Comments handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="commentText">Comment text</param>
		private void CommentHandler(MarkupParsingContext context, string commentText)
		{
			if (!_settings.RemoveXmlComments)
			{
				_currentNodeType = XmlNodeType.Comment;

				_output.Write("<!--");
				_output.Write(commentText);
				_output.Write("-->");
			}
		}

		/// <summary>
		/// CDATA sections handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="cdataText">CDATA text</param>
		private void CdataSectionHandler(MarkupParsingContext context, string cdataText)
		{
			_currentNodeType = XmlNodeType.CdataSection;

			_output.Write("<![CDATA[");
			_output.Write(cdataText);
			_output.Write("]]>");
		}

		/// <summary>
		/// Start tags handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="tagName">Tag name</param>
		/// <param name="attributes">List of attributes</param>
		private void StartTagHandler(MarkupParsingContext context, string tagName,
			IList<XmlAttribute> attributes)
		{
			XmlNodeType previousNodeType = _currentNodeType;

			_currentNodeType = XmlNodeType.StartTag;
			_currentText = string.Empty;

			if (_settings.MinifyWhitespace && previousNodeType == XmlNodeType.Text
				&& (_startTagBeforeText || _endTagBeforeText || _emptyTagBeforeText
					|| _ignoredFragmentBeforeText || _xmlDeclarationBeforeText
					|| _processingInstructionBeforeText || _doctypeBeforeText))
			{
				_output.RemoveLastWhitespaceItems();
			}

			_output.Flush();

			_output.Write("<");
			_output.Write(tagName);
			WriteAttributes(attributes);
			_output.Write(">");
		}

		/// <summary>
		/// End tags handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="tagName">Tag name</param>
		private void EndTagHandler(MarkupParsingContext context, string tagName)
		{
			XmlNodeType previousNodeType = _currentNodeType;
			string previousText = _currentText;

			_currentNodeType = XmlNodeType.EndTag;
			_currentText = string.Empty;

			if (_settings.CollapseTagsWithoutContent && previousNodeType == XmlNodeType.StartTag
				&& previousText.Length == 0)
			{
				if (_output.TransformLastStartTagToEmptyTag(_settings.RenderEmptyTagsWithSpace))
				{
					_output.Flush();
					return;
				}
			}

			if (_settings.MinifyWhitespace && previousNodeType == XmlNodeType.Text
				&& (_endTagBeforeText || _emptyTagBeforeText))
			{
				_output.RemoveLastWhitespaceItems();
			}

			// Add end tag to buffer
			_output.Write("</");
			_output.Write(tagName);
			_output.Write(">");

			_output.Flush();
		}

		/// <summary>
		/// Empty tags handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="tagName">Tag name</param>
		/// <param name="attributes">List of attributes</param>
		private void EmptyTagHandler(MarkupParsingContext context, string tagName,
			IList<XmlAttribute> attributes)
		{
			XmlNodeType previousNodeType = _currentNodeType;

			_currentNodeType = XmlNodeType.EmptyTag;
			_currentText = string.Empty;

			if (_settings.MinifyWhitespace && previousNodeType == XmlNodeType.Text
				&& (_startTagBeforeText || _endTagBeforeText || _emptyTagBeforeText))
			{
				_output.RemoveLastWhitespaceItems();
			}

			_output.Write("<");
			_output.Write(tagName);
			WriteAttributes(attributes);
			_output.Write(_settings.RenderEmptyTagsWithSpace ? " />" : "/>");

			_output.Flush();
		}

		/// <summary>
		/// Text handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="text">Text</param>
		private void TextHandler(MarkupParsingContext context, string text)
		{
			XmlNodeType previousNodeType = _currentNodeType;

			_currentNodeType = XmlNodeType.Text;

			if (previousNodeType == XmlNodeType.Text)
			{
				_currentText = text;
				_output.Write(text);

				return;
			}

			_xmlDeclarationBeforeText = false;
			_processingInstructionBeforeText = false;
			_doctypeBeforeText = false;
			_startTagBeforeText = false;
			_endTagBeforeText = false;
			_emptyTagBeforeText = false;
			_ignoredFragmentBeforeText = false;

			switch (previousNodeType)
			{
				case XmlNodeType.StartTag:
					_startTagBeforeText = true;
					break;
				case XmlNodeType.EndTag:
					_endTagBeforeText = true;
					break;
				case XmlNodeType.EmptyTag:
					_emptyTagBeforeText = true;
					break;
				case XmlNodeType.IgnoredFragment:
					_ignoredFragmentBeforeText = true;
					break;
				case XmlNodeType.XmlDeclaration:
					_xmlDeclarationBeforeText = true;
					break;
				case XmlNodeType.ProcessingInstruction:
					_processingInstructionBeforeText = true;
					break;
				case XmlNodeType.Doctype:
					_doctypeBeforeText = true;
					break;
			}

			if (_settings.MinifyWhitespace)
			{
				if (context.Position == 0)
				{
					// Processing starting whitespace
					text = text.TrimStart();
				}
				else if (context.Position + text.Length == context.Length)
				{
					// Processing ending whitespace
					text = text.TrimEnd();
				}
				else if (_xmlDeclarationBeforeText || _processingInstructionBeforeText || _doctypeBeforeText)
				{
					// Processing whitespace, that followed after
					// the XML declaration, processing instruction
					// or document type declaration
					if (string.IsNullOrWhiteSpace(text))
					{
						text = string.Empty;
					}
				}
			}

			_currentText = text;

			if (text.Length > 0)
			{
				_output.Write(text);
			}
		}

		/// <summary>
		/// Ignored fragments handler
		/// </summary>
		/// <param name="context">Markup parsing context</param>
		/// <param name="fragment">Ignored fragment</param>
		private void IgnoredFragmentHandler(MarkupParsingContext context, string fragment)
		{
			_currentNodeType = XmlNodeType.IgnoredFragment;

			if (_settings.MinifyWhitespace)
			{
				_output.RemoveLastWhitespaceItems();
			}

			if (fragment.Length > 0)
			{
				_output.Write(fragment);
				_output.Flush();
			}
		}

		#endregion

		/// <summary>
		/// Writes a attributes to buffer
		/// </summary>
		/// <param name="attributes">List of attributes</param>
		private void WriteAttributes(IList<XmlAttribute> attributes)
		{
			int attributeCount = attributes.Count;

			for (int attributeIndex = 0; attributeIndex < attributeCount; attributeIndex++)
			{
				XmlAttribute attribute = attributes[attributeIndex];
				string encodedAttributeValue = XmlAttributeValueHelpers.Encode(attribute.Value);

				_output.Write(" ");
				_output.Write(attribute.Name);
				_output.Write("=\"");
				_output.Write(encodedAttributeValue);
				_output.Write("\"");
			}
		}

		/// <summary>
		/// Writes a information about the error
		/// </summary>
		/// <param name="category">Error category</param>
		/// <param name="message">Error message</param>
		/// <param name="filePath">File path</param>
		/// <param name="lineNumber">Line number on which the error occurred</param>
		/// <param name="columnNumber">Column number on which the error occurred</param>
		/// <param name="sourceFragment">Fragment of source code</param>
		private void WriteError(string category, string message, string filePath, int lineNumber, int columnNumber,
			string sourceFragment)
		{
			_errors.Add(new MinificationErrorInfo(category, message, lineNumber, columnNumber, sourceFragment));
			_logger.Error(category, message, filePath, lineNumber, columnNumber, sourceFragment);
		}
	}
}