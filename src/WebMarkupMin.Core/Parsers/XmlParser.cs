using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using WebMarkupMin.Core.Helpers;
using WebMarkupMin.Core.Resources;
using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// XML parser
	/// </summary>
	internal sealed class XmlParser : MarkupParserBase
	{
		#region Regular expressions for parsing tags and attributes

		private const string NAME_PATTERN = @"[\w-:.]+";

		private static readonly Regex _processingInstructionBeginPartRegex = new Regex(@"^<\?(?<instructionName>" + NAME_PATTERN + ")",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _processingInstructionEndPartRegex = new Regex(@"^\s*\?>",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _doctypeRegex = new Regex(@"^<!DOCTYPE\s[^>]+?>",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _startTagBeginPartRegex = new Regex(@"^<(?<tagName>" + NAME_PATTERN + ")",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _startTagEndPartRegex = new Regex(@"^\s*(?<emptyTagSlash>/)?>",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _endTagRegex = new Regex(@"^<\/(?<tagName>" + NAME_PATTERN + @")\s*>",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _attributeRegex = new Regex(@"^\s*(?<attributeName>" + NAME_PATTERN + ")" +
			@"\s*=\s*" +
			@"(?:""(?<attributeValue>[^""]*)""|'(?<attributeValue>[^']*)')",
			TargetFrameworkShortcuts.PerformanceRegexOptions);

		#endregion

		/// <summary>
		/// XML parsing handlers
		/// </summary>
		private readonly XmlParsingHandlers _handlers;

		/// <summary>
		/// Stack of tags
		/// </summary>
		private readonly Stack<StackedXmlTag> _tagStack;

		/// <summary>
		/// Gets a common markup parsing handlers
		/// </summary>
		protected override MarkupParsingHandlersBase CommonHandlers
		{
			get { return _handlers; }
		}


		/// <summary>
		/// Constructs instance of XML parser
		/// </summary>
		/// <param name="handlers">XML parsing handlers</param>
		public XmlParser(XmlParsingHandlers handlers)
		{
			_handlers = handlers;
			_tagStack = new Stack<StackedXmlTag>();
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

				int endPosition = contentLength - 1;
				int previousPosition = -1;

				try
				{
					while (_innerContext.Position <= endPosition)
					{
						bool isProcessed = false;

						if (_innerContext.PeekCurrentChar() == '<')
						{
							switch (_innerContext.PeekNextChar())
							{
								case char c when IsTagFirstChar(c):
									// Start tag
									isProcessed = ProcessStartTag();
									break;

								case '/':
									if (IsTagFirstChar(_innerContext.PeekNextChar()))
									{
										// End tag
										isProcessed = ProcessEndTag();
									}
									break;

								case '!':
									switch (_innerContext.PeekNextChar())
									{
										case '-':
											if (_innerContext.PeekNextChar() == '-')
											{
												// XML comments
												isProcessed = ProcessComment();
											}
											break;

										case '[':
											// CDATA sections
											isProcessed = ProcessCdataSection();
											break;

										case 'D':
											// Doctype declaration
											isProcessed = ProcessDoctype();
											break;
									}
									break;

								case '?':
									// XML declaration and processing instructions
									isProcessed = ProcessProcessingInstruction();
									break;
							}
						}

						if (!isProcessed)
						{
							// Text
							ProcessText();
						}

						if (_innerContext.Position == previousPosition)
						{
							throw new MarkupParsingException(
								string.Format(Strings.ErrorMessage_MarkupParsingFailed, "XML"),
								_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
						}

						previousPosition = _innerContext.Position;
					}

					// Check whether there were not closed tags
					if (_tagStack.Count > 0)
					{
						StackedXmlTag stackedTag = _tagStack.Pop();

						throw new MarkupParsingException(
							string.Format(Strings.ErrorMessage_NotClosedTag, stackedTag.Name),
							stackedTag.Coordinates,
							SourceCodeNavigator.GetSourceFragment(_innerContext.SourceCode, stackedTag.Coordinates));
					}
				}
				catch (MarkupParsingException)
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
		/// <returns>Result of processing (<c>true</c> - is processed; <c>false</c> - is not processed)</returns>
		private bool ProcessProcessingInstruction()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;

			Match processingInstructionBeginPartMatch = _processingInstructionBeginPartRegex.Match(content,
				_innerContext.Position, _innerContext.RemainderLength);
			if (processingInstructionBeginPartMatch.Success)
			{
				string instructionName = processingInstructionBeginPartMatch.Groups["instructionName"].Value;
				bool isXmlDeclaration = instructionName.Equals("xml", StringComparison.OrdinalIgnoreCase);
				List<XmlAttribute> attributes = null;

				_innerContext.IncreasePosition(processingInstructionBeginPartMatch.Length);

				isProcessed = ProcessProcessingInstructionEndPart();
				if (!isProcessed)
				{
					attributes = ProcessAttributes();
					isProcessed = ProcessProcessingInstructionEndPart();
				}

				if (isProcessed)
				{
					attributes = attributes ?? new List<XmlAttribute>();

					if (isXmlDeclaration)
					{
						_handlers.XmlDeclaration?.Invoke(_context, attributes);
					}
					else
					{
						_handlers.ProcessingInstruction?.Invoke(_context, instructionName, attributes);
					}
				}
				else
				{
					int currentPosition = _innerContext.Position;
					int invalidCharPosition = SourceCodeNavigator.FindNextNonWhitespaceChar(content,
						currentPosition, _innerContext.RemainderLength);

					int invalidCharOffset = invalidCharPosition - currentPosition;
					if (invalidCharOffset > 0)
					{
						_innerContext.IncreasePosition(invalidCharOffset);
					}

					string errorMessage = isXmlDeclaration ?
						Strings.ErrorMessage_InvalidCharactersInXmlDeclaration
						:
						string.Format(Strings.ErrorMessage_InvalidCharactersInProcessingInstruction,
							instructionName)
						;

					throw new MarkupParsingException(errorMessage, _innerContext.NodeCoordinates,
						_innerContext.GetSourceFragment());
				}
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a end part of XML declaration and processing instructions
		/// </summary>
		/// <returns>Result of processing (<c>true</c> - is processed; <c>false</c> - is not processed)</returns>
		private bool ProcessProcessingInstructionEndPart()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;

			Match processingInstructionEndPartMatch = _processingInstructionEndPartRegex.Match(content,
				_innerContext.Position, _innerContext.RemainderLength);
			if (processingInstructionEndPartMatch.Success)
			{
				_innerContext.IncreasePosition(processingInstructionEndPartMatch.Length);
				isProcessed = true;
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a doctype declaration
		/// </summary>
		/// <returns>Result of processing (<c>true</c> - is processed; <c>false</c> - is not processed)</returns>
		protected override bool ProcessDoctype()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int contentRemainderLength = _innerContext.RemainderLength;

			var match = _doctypeRegex.Match(content, _innerContext.Position, contentRemainderLength);
			if (match.Success)
			{
				string doctype = match.Value;
				CommonHandlers.Doctype?.Invoke(_context, doctype);

				_innerContext.IncreasePosition(match.Length);
				isProcessed = true;
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a start tag
		/// </summary>
		/// <returns>Result of processing (<c>true</c> - is processed; <c>false</c> - is not processed)</returns>
		private bool ProcessStartTag()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;

			Match startTagBeginPartMatch = _startTagBeginPartRegex.Match(content, _innerContext.Position,
				_innerContext.RemainderLength);
			if (startTagBeginPartMatch.Success)
			{
				string startTagName = startTagBeginPartMatch.Groups["tagName"].Value;
				List<XmlAttribute> attributes = null;
				bool isEmptyTag;

				_innerContext.IncreasePosition(startTagBeginPartMatch.Length);

				isProcessed = ProcessStartTagEndPart(out isEmptyTag);
				if (!isProcessed)
				{
					attributes = ProcessAttributes();
					isProcessed = ProcessStartTagEndPart(out isEmptyTag);
				}

				if (isProcessed)
				{
					attributes = attributes ?? new List<XmlAttribute>();

					if (isEmptyTag)
					{
						_handlers.EmptyTag?.Invoke(_context, startTagName, attributes);
					}
					else
					{
						_tagStack.Push(new StackedXmlTag(startTagName, _innerContext.NodeCoordinates));
						_handlers.StartTag?.Invoke(_context, startTagName, attributes);
					}
				}
				else
				{
					int currentPosition = _innerContext.Position;
					int invalidCharPosition = SourceCodeNavigator.FindNextNonWhitespaceChar(content,
						currentPosition, _innerContext.RemainderLength);

					int invalidCharOffset = invalidCharPosition - currentPosition;
					if (invalidCharOffset > 0)
					{
						_innerContext.IncreasePosition(invalidCharOffset);
					}

					throw new MarkupParsingException(
						string.Format(Strings.ErrorMessage_InvalidCharactersInStartTag, startTagName),
						_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
				}
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a end part of start tag
		/// </summary>
		/// <param name="isEmptyTag">Flag that tag is empty</param>
		/// <returns>Result of processing (<c>true</c> - is processed; <c>false</c> - is not processed)</returns>
		private bool ProcessStartTagEndPart(out bool isEmptyTag)
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			isEmptyTag = false;

			Match startTagEndPartMatch = _startTagEndPartRegex.Match(content, _innerContext.Position,
				_innerContext.RemainderLength);
			if (startTagEndPartMatch.Success)
			{
				isEmptyTag = startTagEndPartMatch.Groups["emptyTagSlash"].Success;

				_innerContext.IncreasePosition(startTagEndPartMatch.Length);
				isProcessed = true;
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a end tag
		/// </summary>
		/// <returns>Result of processing (<c>true</c> - is processed; <c>false</c> - is not processed)</returns>
		private bool ProcessEndTag()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;

			Match match = _endTagRegex.Match(content, _innerContext.Position, _innerContext.RemainderLength);
			if (match.Success)
			{
				string endTagName = match.Groups["tagName"].Value;

				if (_tagStack.Count == 0)
				{
					throw new MarkupParsingException(
						string.Format(Strings.ErrorMessage_StartTagNotDeclared, endTagName),
						_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
				}

				StackedXmlTag stackedTag = _tagStack.Pop();
				if (stackedTag.Name != endTagName)
				{
					if (_tagStack.Any(t => t.Name == endTagName))
					{
						throw new MarkupParsingException(
							string.Format(Strings.ErrorMessage_NotClosedTag, stackedTag.Name),
							stackedTag.Coordinates,
							SourceCodeNavigator.GetSourceFragment(_innerContext.SourceCode, stackedTag.Coordinates));
					}

					throw new MarkupParsingException(
						string.Format(Strings.ErrorMessage_StartTagNotDeclared, endTagName),
						_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
				}

				_handlers.EndTag?.Invoke(_context, endTagName);

				_innerContext.IncreasePosition(match.Length);
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

			_handlers.Text?.Invoke(_context, text);
			_innerContext.IncreasePosition(text.Length);
		}

		/// <summary>
		/// Process a attributes
		/// </summary>
		/// <returns>List of attributes</returns>
		private List<XmlAttribute> ProcessAttributes()
		{
			var attributes = new List<XmlAttribute>();
			string content = _innerContext.SourceCode;

			Match match = _attributeRegex.Match(content, _innerContext.Position, _innerContext.RemainderLength);

			while (match.Success)
			{
				GroupCollection groups = match.Groups;

				string attributeName = groups["attributeName"].Value;
				Group valueGroup = groups["attributeValue"];
				string attributeValue = valueGroup.Value;
				if (!string.IsNullOrWhiteSpace(attributeValue))
				{
					attributeValue = XmlAttributeValueHelpers.Decode(attributeValue);
				}

				int quoteCharPosition = valueGroup.Index - 1;
				char quoteChar;

				if (!content.TryGetChar(quoteCharPosition, out quoteChar)
					|| !(quoteChar == '"' || quoteChar == '\''))
				{
					int currentPosition = _innerContext.Position;
					int quoteCharOffset = quoteCharPosition - currentPosition;

					if (quoteCharOffset > 0)
					{
						_innerContext.IncreasePosition(quoteCharOffset);
					}

					throw new MarkupParsingException(
						string.Format(Strings.ErrorMessage_QuoteExpected, quoteChar),
						_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
				}

				var attribute = new XmlAttribute(attributeName, attributeValue, quoteChar);

				attributes.Add(attribute);

				_innerContext.IncreasePosition(match.Length);
				match = _attributeRegex.Match(content, _innerContext.Position, _innerContext.RemainderLength);
			}

			return attributes;
		}

		#endregion

		#region Determining methods

		/// <summary>
		/// Checks whether the character is valid first character of XML tag name
		/// </summary>
		/// <param name="value">Character value</param>
		/// <returns>Result of check (<c>true</c> - valid; <c>false</c> - not valid)</returns>
		[MethodImpl((MethodImplOptions)256 /* AggressiveInlining */)]
		private static bool IsTagFirstChar(char value)
		{
			return char.IsLetter(value) || value == '_';
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