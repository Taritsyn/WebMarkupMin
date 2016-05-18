using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using WebMarkupMin.Core.Utilities;
using CoreStrings = WebMarkupMin.Core.Resources;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// XML parser
	/// </summary>
	internal sealed class XmlParser
	{
		#region Regular expressions for parsing tags and attributes

		private const string NAME_PATTERN = @"[\w-:.]+";

		private static readonly Regex _processingInstructionRegex =
			new Regex(@"^<\?(?<instructionName>" + NAME_PATTERN + ")" +
				@"(\s+(?<attributes>(\s*" + NAME_PATTERN + @"\s*=\s*((?:""[^""]*?"")|(?:'[^']*?')))+))?" +
				@"\s*\?>");
		private static readonly Regex _startTagRegex =
			new Regex(@"^<(?<tagName>" + NAME_PATTERN + ")" +
				@"(\s+(?<attributes>(\s*" + NAME_PATTERN + @"\s*=\s*((?:""[^""]*?"")|(?:'[^']*?')))+))?" +
				@"\s*(?<emptyTagSlash>/?)>");
		private static readonly Regex _endTagRegex = new Regex(@"^<\/(?<tagName>" + NAME_PATTERN + @")\s*>");
		private static readonly Regex _attributeRegex = new Regex(@"(?<attributeName>" + NAME_PATTERN + ")" +
			@"\s*=\s*" +
			@"(""(?<attributeValue>[^""]*?)""|'(?<attributeValue>[^']*?)')");

		#endregion

		/// <summary>
		/// Inner markup parsing context
		/// </summary>
		private InnerMarkupParsingContext _innerContext;

		/// <summary>
		/// Markup parsing context
		/// </summary>
		private MarkupParsingContext _context;

		/// <summary>
		/// XML parsing handlers
		/// </summary>
		private readonly XmlParsingHandlers _handlers;

		/// <summary>
		/// Stack of tags
		/// </summary>
		private Stack<StackedXmlTag> _tagStack;

		/// <summary>
		/// Synchronizer of parsing
		/// </summary>
		private readonly object _parsingSynchronizer = new object();


		/// <summary>
		/// Constructs instance of XML parser
		/// </summary>
		/// <param name="handlers">XML parsing handlers</param>
		public XmlParser(XmlParsingHandlers handlers)
		{
			_handlers = handlers;
		}


		/// <summary>
		/// Parses XML content
		/// </summary>
		/// <param name="content">XML content</param>
		public void Parse(string content)
		{
			int contentLength = content.Length;
			if (contentLength == 0)
			{
				return;
			}

			lock (_parsingSynchronizer)
			{
				_innerContext = new InnerMarkupParsingContext(content);
				_context = new MarkupParsingContext(_innerContext);

				_tagStack = new Stack<StackedXmlTag>();

				int endPosition = contentLength - 1;
				int previousPosition = -1;

				try
				{
					while (_innerContext.Position <= endPosition)
					{
						bool isProcessed = false;

						if (content.CustomStartsWith("<", _innerContext.Position, StringComparison.Ordinal))
						{
							if (content.CustomStartsWith("</", _innerContext.Position, StringComparison.Ordinal))
							{
								// End tag
								isProcessed = ProcessEndTag();
							}
							else if (content.CustomStartsWith("<!", _innerContext.Position, StringComparison.Ordinal))
							{
								// XML comments
								isProcessed = ProcessComment();

								if (!isProcessed)
								{
									// CDATA sections
									isProcessed = ProcessCdataSection();
								}

								if (!isProcessed)
								{
									// Doctype declaration
									isProcessed = ProcessDoctype();
								}
							}
							else if (content.CustomStartsWith("<?", _innerContext.Position, StringComparison.Ordinal))
							{
								// XML declaration and processing instructions
								isProcessed = ProcessProcessingInstruction();
							}
							else
							{
								// Start tag
								isProcessed = ProcessStartTag();
							}
						}

						if (!isProcessed)
						{
							// Text
							ProcessText();
						}

						if (_innerContext.Position == previousPosition)
						{
							throw new XmlParsingException(
								string.Format(CoreStrings.ErrorMessage_MarkupParsingFailed, "XML"),
								_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
						}

						previousPosition = _innerContext.Position;
					}

					// Check whether there were not closed tags
					if (_tagStack.Count > 0)
					{
						StackedXmlTag stackedTag = _tagStack.Pop();

						throw new XmlParsingException(
							string.Format(CoreStrings.ErrorMessage_NotClosedTag, stackedTag.Name),
							stackedTag.Coordinates,
							SourceCodeNavigator.GetSourceFragment(_innerContext.SourceCode, stackedTag.Coordinates));
					}
				}
				catch (XmlParsingException)
				{
					throw;
				}
				finally
				{
					_tagStack.Clear();

					_context = null;
					_innerContext = null;
				}
			}
		}

		#region Processing methods

		/// <summary>
		/// Process a XML declaration and processing instructions
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		private bool ProcessProcessingInstruction()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int contentRemainderLength = _innerContext.RemainderLength;

			var match = _processingInstructionRegex.Match(content, _innerContext.Position, contentRemainderLength);
			if (match.Success)
			{
				GroupCollection groups = match.Groups;

				string instruction = match.Value;
				string instructionName = groups["instructionName"].Value;
				string attributesString = groups["attributes"].Value;

				IList<XmlAttribute> attributes = ParseAttributes(attributesString);
				if (String.Equals(instructionName, "xml", StringComparison.OrdinalIgnoreCase))
				{
					if (_handlers.XmlDeclaration != null)
					{
						_handlers.XmlDeclaration(_context, attributes);
					}
				}
				else
				{
					if (_handlers.ProcessingInstruction != null)
					{
						_handlers.ProcessingInstruction(_context, instructionName, attributes);
					}
				}

				_innerContext.IncreasePosition(instruction.Length);
				isProcessed = true;
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a doctype declaration
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		private bool ProcessDoctype()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int contentRemainderLength = _innerContext.RemainderLength;

			var match = CommonRegExps.Doctype.Match(content, _innerContext.Position, contentRemainderLength);
			if (match.Success)
			{
				string doctype = match.Value;

				if (_handlers.Doctype != null)
				{
					_handlers.Doctype(_context, doctype);
				}

				_innerContext.IncreasePosition(doctype.Length);
				isProcessed = true;
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a XML comments
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		private bool ProcessComment()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;

			if (content.CustomStartsWith("<!--", _innerContext.Position, StringComparison.Ordinal))
			{
				int commentStartPosition = _innerContext.Position;
				int commentEndPosition = content.IndexOf("-->", commentStartPosition, StringComparison.Ordinal);

				if (commentEndPosition > commentStartPosition)
				{
					string commentText = content.Substring(commentStartPosition + 4,
						commentEndPosition - commentStartPosition - 4);

					if (_handlers.Comment != null)
					{
						_handlers.Comment(_context, commentText);
					}

					_innerContext.IncreasePosition(commentEndPosition + 3 - commentStartPosition);
					isProcessed = true;
				}
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a CDATA sections
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		private bool ProcessCdataSection()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;

			if (content.CustomStartsWith("<![CDATA[", _innerContext.Position, StringComparison.OrdinalIgnoreCase))
			{
				int cdataStartPosition = _innerContext.Position;
				int cdataEndPosition = content.IndexOf("]]>", cdataStartPosition, StringComparison.Ordinal);

				if (cdataEndPosition > cdataStartPosition)
				{
					string cdataText = content.Substring(cdataStartPosition + 9,
						cdataEndPosition - cdataStartPosition - 9);

					if (_handlers.CdataSection != null)
					{
						_handlers.CdataSection(_context, cdataText);
					}

					_innerContext.IncreasePosition(cdataEndPosition + 3 - cdataStartPosition);
					isProcessed = true;
				}
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a start tag
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		private bool ProcessStartTag()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int contentRemainderLength = _innerContext.RemainderLength;

			var match = _startTagRegex.Match(content, _innerContext.Position, contentRemainderLength);
			if (match.Success)
			{
				string startTag = match.Value;

				GroupCollection groups = match.Groups;
				string startTagName = groups["tagName"].Value;
				string attributesString = groups["attributes"].Value;
				bool isEmptyTag = (groups["emptyTagSlash"].Value.Length > 0);

				IList<XmlAttribute> attributes = ParseAttributes(attributesString);
				if (isEmptyTag)
				{
					if (_handlers.EmptyTag != null)
					{
						_handlers.EmptyTag(_context, startTagName, attributes);
					}
				}
				else
				{
					_tagStack.Push(new StackedXmlTag(startTagName, _innerContext.NodeCoordinates));

					if (_handlers.StartTag != null)
					{
						_handlers.StartTag(_context, startTagName, attributes);
					}
				}

				_innerContext.IncreasePosition(startTag.Length);
				isProcessed = true;
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a end tag
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		private bool ProcessEndTag()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int contentRemainderLength = _innerContext.RemainderLength;

			var match = _endTagRegex.Match(content, _innerContext.Position, contentRemainderLength);
			if (match.Success)
			{
				string endTag = match.Value;
				string endTagName = match.Groups["tagName"].Value;

				if (_tagStack.Count == 0)
				{
					throw new XmlParsingException(
						string.Format(CoreStrings.ErrorMessage_StartTagNotDeclared, endTagName),
						_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
				}

				StackedXmlTag stackedTag = _tagStack.Pop();
				if (stackedTag.Name != endTagName)
				{
					if (_tagStack.Any(t => t.Name == endTagName))
					{
						throw new XmlParsingException(
							string.Format(CoreStrings.ErrorMessage_NotClosedTag, stackedTag.Name),
							stackedTag.Coordinates,
							SourceCodeNavigator.GetSourceFragment(_innerContext.SourceCode, stackedTag.Coordinates));
					}

					throw new XmlParsingException(
						string.Format(CoreStrings.ErrorMessage_StartTagNotDeclared, endTagName),
						_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
				}

				if (_handlers.EndTag != null)
				{
					_handlers.EndTag(_context, endTagName);
				}

				_innerContext.IncreasePosition(endTag.Length);
				isProcessed = true;
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a text
		/// </summary>
		private void ProcessText()
		{
			string content = _innerContext.SourceCode;

			string text;
			int tagPosition = content.IndexOf("<", _innerContext.Position, StringComparison.Ordinal);

			if (tagPosition >= _innerContext.Position)
			{
				text = content.Substring(_innerContext.Position, tagPosition - _innerContext.Position);
			}
			else
			{
				text = content.Substring(_innerContext.Position);
			}

			if (_handlers.Text != null)
			{
				_handlers.Text(_context, text);
			}

			_innerContext.IncreasePosition(text.Length);
		}

		#endregion

		#region Parsing methods

		/// <summary>
		/// Parses a attributes
		/// </summary>
		/// <param name="attributesString">String representation of the attribute list</param>
		/// <returns>List of attributes</returns>
		private IList<XmlAttribute> ParseAttributes(string attributesString)
		{
			var attributes = new List<XmlAttribute>();
			if (string.IsNullOrWhiteSpace(attributesString))
			{
				return attributes;
			}

			MatchCollection attributeMatches = _attributeRegex.Matches(attributesString);

			foreach (Match attributeMatch in attributeMatches)
			{
				GroupCollection groups = attributeMatch.Groups;

				string attributeName = groups["attributeName"].Value;
				string attributeValue = groups["attributeValue"].Value;
				if (!string.IsNullOrWhiteSpace(attributeValue))
				{
					attributeValue = XmlAttribute.XmlAttributeDecode(attributeValue);
				}

				attributes.Add(new XmlAttribute(attributeName, attributeValue));
			}

			return attributes;
		}

		#endregion

		#region Internal types

		/// <summary>
		/// Stacked XML tag
		/// </summary>
		private sealed class StackedXmlTag
		{
			/// <summary>
			/// Name
			/// </summary>
			public string Name
			{
				get;
				private set;
			}

			/// <summary>
			/// Coordinates of tag
			/// </summary>
			public SourceCodeNodeCoordinates Coordinates
			{
				get;
				private set;
			}


			/// <summary>
			/// Constructs instance of stacked XML tag
			/// </summary>
			/// <param name="name">Name</param>
			/// <param name="coordinates">Coordinates of tag</param>
			public StackedXmlTag(string name, SourceCodeNodeCoordinates coordinates)
			{
				Name = name;
				Coordinates = coordinates;
			}
		}

		#endregion
	}
}