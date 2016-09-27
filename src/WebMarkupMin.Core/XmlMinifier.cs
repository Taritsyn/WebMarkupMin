using System;
using System.Collections.Generic;
using System.Text;

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
		/// Result of HTML minification
		/// </summary>
		private StringBuilder _result;

		/// <summary>
		/// HTML minification buffer
		/// </summary>
		private readonly List<string> _buffer;

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
				Text = TextHandler
			});

			_buffer = new List<string>();
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
					_result = StringBuilderPool.GetBuilder(estimatedCapacity);

					_xmlParser.Parse(cleanedContent);

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
				catch (XmlParsingException e)
				{
					WriteError(LogCategoryConstants.XmlParsingError, e.Message, fileContext,
						e.LineNumber, e.ColumnNumber, e.SourceFragment);
				}
				finally
				{
					StringBuilderPool.ReleaseBuilder(_result);
					_buffer.Clear();
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
				RemoveLastWhitespaceBufferItems();
			}

			_buffer.Add("<?xml");
			RenderAttributes(attributes);
			_buffer.Add("?>");
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
				RemoveLastWhitespaceBufferItems();
			}

			_buffer.Add("<?");
			_buffer.Add(instructionName);
			RenderAttributes(attributes);
			_buffer.Add("?>");
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
				RemoveLastWhitespaceBufferItems();
			}

			_buffer.Add(Utils.CollapseWhitespace(doctype));
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

				_buffer.Add("<!--");
				_buffer.Add(commentText);
				_buffer.Add("-->");
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

			_buffer.Add("<![CDATA[");
			_buffer.Add(cdataText);
			_buffer.Add("]]>");
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
					|| _xmlDeclarationBeforeText || _processingInstructionBeforeText || _doctypeBeforeText))
			{
				RemoveLastWhitespaceBufferItems();
			}

			_buffer.Add("<");
			_buffer.Add(tagName);
			RenderAttributes(attributes);
			_buffer.Add(">");
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
				if (TransformLastStartTagToEmptyTag())
				{
					FlushBuffer();
					return;
				}
			}

			if (_settings.MinifyWhitespace && previousNodeType == XmlNodeType.Text
				&& (_endTagBeforeText || _emptyTagBeforeText))
			{
				RemoveLastWhitespaceBufferItems();
			}

			// Add end tag to buffer
			_buffer.Add("</");
			_buffer.Add(tagName);
			_buffer.Add(">");

			FlushBuffer();
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
				RemoveLastWhitespaceBufferItems();
			}

			_buffer.Add("<");
			_buffer.Add(tagName);
			RenderAttributes(attributes);
			_buffer.Add(_settings.RenderEmptyTagsWithSpace ? " />" : "/>");
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
				_buffer.Add(text);

				return;
			}

			_xmlDeclarationBeforeText = false;
			_processingInstructionBeforeText = false;
			_doctypeBeforeText = false;
			_startTagBeforeText = false;
			_endTagBeforeText = false;
			_emptyTagBeforeText = false;

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
				_buffer.Add(text);
			}
		}

		#endregion

		#region Buffer helpers

		/// <summary>
		/// Transform a last start tag to empty tag
		/// </summary>
		/// <returns>Result of transforming (true - has transformed; false - has not transformed)</returns>
		private bool TransformLastStartTagToEmptyTag()
		{
			int bufferItemCount = _buffer.Count;
			if (bufferItemCount == 0)
			{
				return false;
			}

			bool isTransformed = false;
			int lastBufferItemIndex = bufferItemCount - 1;
			int lastEndTagEndAngleBracketIndex = _buffer.LastIndexOf(">");

			if (lastEndTagEndAngleBracketIndex == lastBufferItemIndex)
			{
				_buffer[lastBufferItemIndex] = _settings.RenderEmptyTagsWithSpace ? " />" : "/>";
				isTransformed = true;
			}

			return isTransformed;
		}

		/// <summary>
		/// Removes a last whitespace items from buffer
		/// </summary>
		private void RemoveLastWhitespaceBufferItems()
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
						break;
					}
				}
			}
		}

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

		#endregion

		/// <summary>
		/// Renders a list of attributes
		/// </summary>
		/// <param name="attributes">List of attributes</param>
		private void RenderAttributes(IList<XmlAttribute> attributes)
		{
			int attributeCount = attributes.Count;

			for (int attributeIndex = 0; attributeIndex < attributeCount; attributeIndex++)
			{
				_buffer.Add(BuildAttributeString(attributes[attributeIndex]));
			}
		}

		/// <summary>
		/// Builds a string representation of the attribute
		/// </summary>
		/// <param name="attribute">XML attribute</param>
		/// <returns>String representation of the XML attribute</returns>
		private static string BuildAttributeString(XmlAttribute attribute)
		{
			string encodedAttributeValue = XmlAttribute.XmlAttributeEncode(attribute.Value);

			return string.Concat(" ", attribute.Name, "=", "\"", encodedAttributeValue, "\"");
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