/* This parser based on the code of Resig-Zaytsev's HTML Parser */

/* htmlparser.js
 * May 21, 2012
 *
 * HTML Parser By John Resig (http://ejohn.org)
 * Modified by Juriy "kangax" Zaytsev (http://github.com/kangax)
 * Original code by Erik Arvidsson, Mozilla Public License
 * http://erik.eae.net/simplehtmlparser/simplehtmlparser.js
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using WebMarkupMin.Core.Helpers;
using WebMarkupMin.Core.Resources;
using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML parser
	/// </summary>
	internal sealed class HtmlParser : MarkupParserBase
	{
		#region Regular expressions for parsing tags and attributes

		private static readonly Regex _xmlDeclarationRegex = new Regex(@"^<\?xml\s+[^>]+\s*\?>",
			RegexOptions.IgnoreCase | TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _doctypeRegex = new Regex(@"^<!DOCTYPE\s?[^>]+?>",
			RegexOptions.IgnoreCase | TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _startTagBeginPartRegex = new Regex(@"^<(?<tagName>" + CommonRegExps.HtmlTagNamePattern + ")",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _startTagEndPartRegex = new Regex(@"^\s*(?<emptyTagSlash>/)?>",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _endTagRegex = new Regex(@"^<\/(?<tagName>" + CommonRegExps.HtmlTagNamePattern + @")\s*>",
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _attributeRegex =
			new Regex(@"^\s*(?<name>" + CommonRegExps.HtmlAttributeNamePattern + @")" +
				"(?:" +
					@"\s*(?<equalSign>=)\s*" +
					"(?:" +
						@"(?:(?<quote>"")(?<value>[^""]*)"")" +
						@"|(?:(?<quote>')(?<value>[^']*)')" +
						@"|(?<value>[^\s""'`=<>]+)" +
					")?" +
				")?",
				TargetFrameworkShortcuts.PerformanceRegexOptions)
				;

		private static readonly Regex _hiddenIfCommentRegex = new Regex(@"^<!--\[if\s+(?<expression>[^\]]+?)\]>",
			RegexOptions.IgnoreCase | TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _hiddenEndIfCommentRegex = new Regex(@"^<!\[endif\s*\]-->",
			RegexOptions.IgnoreCase | TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _revealedIfCommentRegex = new Regex(@"^<!\[if\s+(?<expression>[^\]]+?)\]>",
			RegexOptions.IgnoreCase | TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _revealedEndIfCommentRegex = new Regex(@"^<!\[endif\s*\]>",
			RegexOptions.IgnoreCase | TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _revealedValidatingIfCommentRegex = new Regex(@"^<!--\[if\s+(?<expression>[^\]]+?)" +
			@"(?:\]>\s*(?<ltAndPling><!)?\s*-->)",
			RegexOptions.IgnoreCase | TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly Regex _revealedValidatingEndIfCommentRegex = new Regex(@"^<!--\s*<!\[endif\s*\]-->",
			RegexOptions.IgnoreCase | TargetFrameworkShortcuts.PerformanceRegexOptions);

		#endregion

		/// <summary>
		/// Cache of the regular expressions for working with end tags, that can contain embedded code
		/// </summary>
		private static readonly ConcurrentDictionary<string, Regex> _endTagWithEmbeddedCodeRegexCache =
			new ConcurrentDictionary<string, Regex>();

		/// <summary>
		/// HTML parsing handlers
		/// </summary>
		private readonly HtmlParsingHandlers _handlers;

		/// <summary>
		/// HTML tag type determiner
		/// </summary>
		private readonly HtmlTagTypeDeterminer _tagTypeDeterminer = HtmlTagTypeDeterminer.Instance;

		/// <summary>
		/// Stack of tags
		/// </summary>
		private readonly Stack<HtmlTag> _tagStack = new Stack<HtmlTag>();

		/// <summary>
		/// HTML attribute type determiner
		/// </summary>
		private readonly HtmlAttributeTypeDeterminer _attributeTypeDeterminer =
			HtmlAttributeTypeDeterminer.Instance;

		/// <summary>
		/// Temporary list of attributes
		/// </summary>
		private readonly List<HtmlAttribute> _tempAttributes = new List<HtmlAttribute>();

		/// <summary>
		/// Stack of conditional comment types
		/// </summary>
		private readonly Stack<HtmlConditionalCommentType> _conditionalCommentTypeStack =
			new Stack<HtmlConditionalCommentType>();

		/// <summary>
		/// Flag for whether the non-validating conditional comment is open
		/// </summary>
		private bool _nonValidatingConditionalCommentOpened = false;

		/// <summary>
		/// Flag for whether the XML-based tag is open
		/// </summary>
		private bool _xmlTagOpened;

		/// <summary>
		/// Gets a common markup parsing handlers
		/// </summary>
		protected override MarkupParsingHandlersBase CommonHandlers
		{
			get { return _handlers; }
		}


		/// <summary>
		/// Constructs instance of HTML parser
		/// </summary>
		/// <param name="handlers">HTML parsing handlers</param>
		public HtmlParser(HtmlParsingHandlers handlers)
		{
			_handlers = handlers;
		}


		/// <summary>
		/// Parses HTML content
		/// </summary>
		/// <param name="content">HTML content</param>
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
						HtmlTag lastStackedTag;

						// Make sure we're not in a tag, that contains embedded code
						if (!_tagStack.TryPeek(out lastStackedTag) || !lastStackedTag.Flags.IsSet(HtmlTagFlags.EmbeddedCode))
						{
							if (_innerContext.PeekCurrentChar() == '<')
							{
								switch (_innerContext.PeekNextChar())
								{
									case char c when c.IsAlphaNumeric():
										// Start tag
										isProcessed = ProcessStartTag();
										break;

									case '/':
										if (_innerContext.PeekNextChar().IsAlphaNumeric())
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
													// Comments
													if (_innerContext.PeekNextChar() == '[')
													{
														// Revealed validating If conditional comments
														// (e.g. <!--[if ... ]><!--> or <!--[if ... ]>-->)
														isProcessed = ProcessRevealedValidatingIfComment();

														if (!isProcessed)
														{
															// Hidden If conditional comments (e.g. <!--[if ... ]>)
															isProcessed = ProcessHiddenIfComment();
														}
													}
													else
													{
														// Revealed validating End If conditional comments
														// (e.g. <!--<![endif]-->)
														isProcessed = ProcessRevealedValidatingEndIfComment();
													}

													if (!isProcessed)
													{
														// HTML comments
														isProcessed = ProcessComment();
													}
												}
												break;

											case '[':
												switch (_innerContext.PeekNextChar())
												{
													case 'i':
													case 'I':
														// Revealed If conditional comment (e.g. <![if ... ]>)
														isProcessed = ProcessRevealedIfComment();
														break;

													case 'e':
													case 'E':
														// Hidden End If conditional comment (e.g. <![endif]-->)
														isProcessed = ProcessHiddenEndIfComment();

														if (!isProcessed)
														{
															// Revealed End If conditional comment (e.g. <![endif]>)
															isProcessed = ProcessRevealedEndIfComment();
														}
														break;

													case 'C':
														// CDATA sections
														isProcessed = ProcessCdataSection();
														break;
												}
												break;

											case 'D':
											case 'd':
												// Doctype declaration
												isProcessed = ProcessDoctype();
												break;
										}
										break;

									case '?':
										// XML declaration
										isProcessed = ProcessXmlDeclaration();
										break;
								}
							}

							if (!isProcessed)
							{
								// Text
								ProcessText();
							}
						}
						else
						{
							// Embedded code
							ProcessEmbeddedCode();
						}

						if (_innerContext.Position == previousPosition)
						{
							throw new MarkupParsingException(
								string.Format(Strings.ErrorMessage_MarkupParsingFailed, "HTML"),
								_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
						}

						previousPosition = _innerContext.Position;
					}

					// Clean up any remaining tags
					ParseRemainingEndTags();

					// Check whether there were not closed conditional comment
					if (_conditionalCommentTypeStack.Count > 0)
					{
						throw new MarkupParsingException(
							Strings.ErrorMessage_NotClosedConditionalComment,
							_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
					}
				}
				catch (MarkupParsingException)
				{
					throw;
				}
				finally
				{
					_tagStack.Clear();
					_tempAttributes.Clear();
					_conditionalCommentTypeStack.Clear();
					_nonValidatingConditionalCommentOpened = false;
					_xmlTagOpened = false;
					_context = null;
					_innerContext = null;
				}
			}
		}

		#region Processing methods

		/// <summary>
		/// Process a XML declaration
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		private bool ProcessXmlDeclaration()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int contentRemainderLength = _innerContext.RemainderLength;

			var match = _xmlDeclarationRegex.Match(content, _innerContext.Position, contentRemainderLength);
			if (match.Success)
			{
				string xmlDeclaration = match.Value;
				_handlers.XmlDeclaration?.Invoke(_context, xmlDeclaration);

				_innerContext.IncreasePosition(match.Length);
				isProcessed = true;
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a doctype declaration
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
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
		/// Process a hidden If conditional comments
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		private bool ProcessHiddenIfComment()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int contentRemainderLength = _innerContext.RemainderLength;

			var match = _hiddenIfCommentRegex.Match(content, _innerContext.Position, contentRemainderLength);
			if (match.Success)
			{
				var groups = match.Groups;
				string expression = groups["expression"].Value.Trim();

				ParseIfConditionalComment(expression, HtmlConditionalCommentType.Hidden);

				_innerContext.IncreasePosition(match.Length);
				isProcessed = true;
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a hidden End If conditional comment
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		private bool ProcessHiddenEndIfComment()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int contentRemainderLength = _innerContext.RemainderLength;

			var match = _hiddenEndIfCommentRegex.Match(content, _innerContext.Position, contentRemainderLength);
			if (match.Success)
			{
				ParseEndIfConditionalComment(HtmlConditionalCommentType.Hidden);

				_innerContext.IncreasePosition(match.Length);
				isProcessed = true;
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a revealed If conditional comment
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		private bool ProcessRevealedIfComment()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int contentRemainderLength = _innerContext.RemainderLength;

			var match = _revealedIfCommentRegex.Match(content, _innerContext.Position, contentRemainderLength);
			if (match.Success)
			{
				var groups = match.Groups;
				string expression = groups["expression"].Value.Trim();

				ParseIfConditionalComment(expression, HtmlConditionalCommentType.Revealed);

				_innerContext.IncreasePosition(match.Length);
				isProcessed = true;
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a revealed End If conditional comment
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		private bool ProcessRevealedEndIfComment()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int contentRemainderLength = _innerContext.RemainderLength;

			var match = _revealedEndIfCommentRegex.Match(content, _innerContext.Position, contentRemainderLength);
			if (match.Success)
			{
				ParseEndIfConditionalComment(HtmlConditionalCommentType.Revealed);

				_innerContext.IncreasePosition(match.Length);
				isProcessed = true;
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a revealed validating If conditional comments
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		private bool ProcessRevealedValidatingIfComment()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int contentRemainderLength = _innerContext.RemainderLength;

			var match = _revealedValidatingIfCommentRegex.Match(content, _innerContext.Position, contentRemainderLength);
			if (match.Success)
			{
				var groups = match.Groups;
				string expression = groups["expression"].Value.Trim();
				var type = groups["ltAndPling"].Success ?
					HtmlConditionalCommentType.RevealedValidating
					:
					HtmlConditionalCommentType.RevealedValidatingSimplified
					;

				ParseIfConditionalComment(expression, type);

				_innerContext.IncreasePosition(match.Length);
				isProcessed = true;
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a revealed validating End If conditional comments
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		private bool ProcessRevealedValidatingEndIfComment()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int contentRemainderLength = _innerContext.RemainderLength;

			var match = _revealedValidatingEndIfCommentRegex.Match(content, _innerContext.Position,
				contentRemainderLength);
			if (match.Success)
			{
				ParseEndIfConditionalComment(HtmlConditionalCommentType.RevealedValidating);

				_innerContext.IncreasePosition(match.Length);
				isProcessed = true;
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

			Match startTagBeginPartMatch = _startTagBeginPartRegex.Match(content, _innerContext.Position,
				_innerContext.RemainderLength);
			if (startTagBeginPartMatch.Success)
			{
				string startTagName = startTagBeginPartMatch.Groups["tagName"].Value;
				string startTagNameInLowercase = startTagName;
				if (Utils.ContainsUppercaseCharacters(startTagName))
				{
					startTagNameInLowercase = startTagName.ToLowerInvariant();
				}
				List<HtmlAttribute> attributes = null;
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
					attributes = attributes ?? new List<HtmlAttribute>();
					ParseStartTag(startTagName, startTagNameInLowercase, attributes, isEmptyTag);
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
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
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
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		private bool ProcessEndTag()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;

			Match match = _endTagRegex.Match(content, _innerContext.Position, _innerContext.RemainderLength);
			if (match.Success)
			{
				string endTagName = match.Groups["tagName"].Value;
				string endTagNameInLowercase = endTagName;
				if (Utils.ContainsUppercaseCharacters(endTagName))
				{
					endTagNameInLowercase = endTagName.ToLowerInvariant();
				}

				ParseEndTag(endTagName, endTagNameInLowercase);

				_innerContext.IncreasePosition(match.Length);
				isProcessed = true;
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a embedded code
		/// </summary>
		private void ProcessEmbeddedCode()
		{
			HtmlTag stackedTag;

			if (_tagStack.TryPeek(out stackedTag))
			{
				string content = _innerContext.SourceCode;
				int contentLength = content.Length;
				int currentPosition = _innerContext.Position;
				int contentRemainderLength = _innerContext.RemainderLength;

				string stackedTagName = stackedTag.Name;
				string stackedTagNameInLowercase = stackedTag.NameInLowercase;
				Regex stackedTagRegex = _endTagWithEmbeddedCodeRegexCache.GetOrAdd(stackedTagNameInLowercase,
					key => new Regex(@"</" + Regex.Escape(key) + @"\s*>", RegexOptions.IgnoreCase | TargetFrameworkShortcuts.PerformanceRegexOptions));

				bool isSupportMarkup = IsSupportEmbeddedMarkup(stackedTag);
				int endTagSearchStartPosition = currentPosition;
				int endTagSearchContentRemainderLength = contentRemainderLength;
				bool endTagFound = false;

				do
				{
					Match match = stackedTagRegex.Match(content, endTagSearchStartPosition, endTagSearchContentRemainderLength);
					if (match.Success)
					{
						int endTagPosition = match.Index;
						endTagFound = true;

						if (isSupportMarkup)
						{
							int commentStartPosition = content.IndexOf(COMMENT_BEGIN_PART, endTagSearchStartPosition);
							if (commentStartPosition != -1)
							{
								int commentTextPosition = commentStartPosition + COMMENT_BEGIN_PART.Length;
								int commentEndPosition = content.IndexOf(COMMENT_END_PART, commentTextPosition, StringComparison.Ordinal);
								if (commentEndPosition == -1)
								{
									_innerContext.IncreasePosition(commentStartPosition - currentPosition);

									throw new MarkupParsingException(
										Strings.ErrorMessage_NotClosedComment,
										_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
								}

								if (endTagPosition >= commentTextPosition && endTagPosition < commentEndPosition)
								{
									endTagSearchStartPosition = commentEndPosition + COMMENT_END_PART.Length;
									endTagSearchContentRemainderLength = contentLength - endTagSearchStartPosition;
									endTagFound = false;

									continue;
								}
							}
						}

						int endTagLength = match.Length;
						int codeLength = endTagPosition - currentPosition;

						string code = codeLength > 0 ? content.Substring(currentPosition, codeLength) : string.Empty;
						int codeAndEndTagLength = codeLength + endTagLength;

						_handlers.EmbeddedCode?.Invoke(_context, code);

						ParseEndTag(stackedTagName, stackedTagNameInLowercase);

						_innerContext.IncreasePosition(codeAndEndTagLength);
					}
				}
				while (!endTagFound);
			}
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

			if (TemplateTagHelpers.ContainsTag(text))
			{
				TemplateTagHelpers.ParseMarkup(text,
					(localContext, expression, startDelimiter, endDelimiter) =>
					{
						_handlers.TemplateTag?.Invoke(_context, expression, startDelimiter, endDelimiter);
						_innerContext.IncreasePosition(startDelimiter.Length + expression.Length + endDelimiter.Length);
					},
					(localContext, textValue) =>
					{
						_handlers.Text?.Invoke(_context, textValue);
						_innerContext.IncreasePosition(textValue.Length);
					}
				);
			}
			else
			{
				_handlers.Text?.Invoke(_context, text);
				_innerContext.IncreasePosition(text.Length);
			}
		}

		/// <summary>
		/// Process a attributes
		/// </summary>
		/// <returns>List of attributes</returns>
		private List<HtmlAttribute> ProcessAttributes()
		{
			string content = _innerContext.SourceCode;
			int currentPosition = _innerContext.Position;
			SourceCodeNodeCoordinates currentCoordinates = _innerContext.NodeCoordinates;

			Match match = _attributeRegex.Match(content, currentPosition, _innerContext.RemainderLength);

			while (match.Success)
			{
				GroupCollection groups = match.Groups;
				Group nameGroup = groups["name"];
				Group equalSignGroup = groups["equalSign"];
				Group valueGroup = groups["value"];

				string name = nameGroup.Value;
				string nameInLowercase = name;
				if (Utils.ContainsUppercaseCharacters(name))
				{
					nameInLowercase = name.ToLowerInvariant();
				}
				string value = null;
				char quoteCharacter = '\0';

				if (equalSignGroup.Success)
				{
					if (valueGroup.Success)
					{
						value = valueGroup.Value;
						if (!string.IsNullOrWhiteSpace(value))
						{
							value = HtmlAttributeValueHelpers.Decode(value);
						}
					}
					else
					{
						value = string.Empty;
					}

					Group quoteGroup = groups["quote"];
					if (quoteGroup.Success)
					{
						quoteCharacter = quoteGroup.Value[0];
					}
				}

				var nameCoordinates = SourceCodeNodeCoordinates.Empty;
				int namePosition = -1;
				if (nameGroup.Success)
				{
					namePosition = nameGroup.Index;
				}

				if (namePosition != -1)
				{
					int lineCount;
					int charRemainderCount;

					SourceCodeNavigator.CalculateLineCount(content, currentPosition,
						namePosition - currentPosition, out lineCount, out charRemainderCount);
					nameCoordinates = SourceCodeNavigator.CalculateAbsoluteNodeCoordinates(
						currentCoordinates, lineCount, charRemainderCount);

					currentPosition = namePosition;
					currentCoordinates = nameCoordinates;
				}

				var valueCoordinates = SourceCodeNodeCoordinates.Empty;
				int valuePosition = -1;
				if (valueGroup.Success)
				{
					valuePosition = valueGroup.Index;
				}

				if (valuePosition != -1)
				{
					int lineCount;
					int charRemainderCount;

					SourceCodeNavigator.CalculateLineCount(content, currentPosition,
						valuePosition - currentPosition, out lineCount, out charRemainderCount);
					valueCoordinates = SourceCodeNavigator.CalculateAbsoluteNodeCoordinates(
						currentCoordinates, lineCount, charRemainderCount);

					currentPosition = valuePosition;
					currentCoordinates = valueCoordinates;
				}

				var attribute = new HtmlAttribute(name, nameInLowercase, value, quoteCharacter,
					HtmlAttributeType.Unknown, nameCoordinates, valueCoordinates);
				_tempAttributes.Add(attribute);

				_innerContext.IncreasePosition(match.Length);
				match = _attributeRegex.Match(content, _innerContext.Position, _innerContext.RemainderLength);
			}

			int attributeCount = _tempAttributes.Count;
			var attributes = new List<HtmlAttribute>(attributeCount);

			for (int attributeIndex = 0; attributeIndex < attributeCount; attributeIndex++)
			{
				attributes.Add(_tempAttributes[attributeIndex]);
			}

			_tempAttributes.Clear();

			return attributes;
		}

		#endregion

		#region Parsing methods

		/// <summary>
		/// Parses a If conditional comment
		/// </summary>
		/// <param name="expression">Conditional expression</param>
		/// <param name="type">Conditional comment type</param>
		private void ParseIfConditionalComment(string expression, HtmlConditionalCommentType type)
		{
			if (_conditionalCommentTypeStack.Count > 0)
			{
				throw new MarkupParsingException(
					Strings.ErrorMessage_NotClosedConditionalComment,
					_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
			}

			string processedExpression = expression.Trim();

			_conditionalCommentTypeStack.Push(type);
			if (type == HtmlConditionalCommentType.Hidden || type == HtmlConditionalCommentType.Revealed)
			{
				_nonValidatingConditionalCommentOpened = true;
			}

			_handlers.IfConditionalComment?.Invoke(_context, processedExpression, type);
		}

		/// <summary>
		/// Parses a End If conditional comment
		/// </summary>
		/// <param name="type">Conditional comment type</param>
		private void ParseEndIfConditionalComment(HtmlConditionalCommentType type)
		{
			if (_conditionalCommentTypeStack.Count > 0)
			{
				HtmlConditionalCommentType stackedType = _conditionalCommentTypeStack.Pop();

				if (type == HtmlConditionalCommentType.Hidden || type == HtmlConditionalCommentType.Revealed)
				{
					if (stackedType == type)
					{
						_nonValidatingConditionalCommentOpened = false;
					}
					else
					{
						throw new MarkupParsingException(
							Strings.ErrorMessage_InvalidEndIfConditionalComment,
							_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
					}
				}
				else if (type == HtmlConditionalCommentType.RevealedValidating
					|| type == HtmlConditionalCommentType.RevealedValidatingSimplified)
				{
					if (stackedType != HtmlConditionalCommentType.RevealedValidating
						&& stackedType != HtmlConditionalCommentType.RevealedValidatingSimplified)
					{
						throw new MarkupParsingException(
							Strings.ErrorMessage_InvalidEndIfConditionalComment,
							_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
					}
				}
			}
			else
			{
				throw new MarkupParsingException(
					Strings.ErrorMessage_IfConditionalCommentNotDeclared,
					_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
			}

			_handlers.EndIfConditionalComment?.Invoke(_context, type);
		}

		/// <summary>
		/// Parses a start tag
		/// </summary>
		/// <param name="tagName">Tag name</param>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <param name="attributes">List of attributes</param>
		/// <param name="isEmptyTag">Flag that tag is empty</param>
		private void ParseStartTag(string tagName, string tagNameInLowercase, List<HtmlAttribute> attributes,
			bool isEmptyTag)
		{
			HtmlTagFlags tagFlags = GetTagFlagsByName(tagNameInLowercase);

			if (_tagStack.Count > 0 && tagFlags.IsSet(HtmlTagFlags.Optional))
			{
				HtmlTag lastStackedTag = _tagStack.Peek();
				HtmlTag optionalEndTag = null;

				if (lastStackedTag.NameInLowercase == tagNameInLowercase)
				{
					// Insert the optional end tag before the similar current tag
					optionalEndTag = lastStackedTag;
				}
				else if (tagNameInLowercase == "body")
				{
					// Insert the missing `head` end tag before the `body` tag
					if (lastStackedTag.NameInLowercase == "head")
					{
						optionalEndTag = lastStackedTag;
					}
					else if (!lastStackedTag.Flags.IsSet(HtmlTagFlags.Optional))
					{
						optionalEndTag = _tagStack.GetFirstTagByNameInLowercase("head");
					}
				}

				if (optionalEndTag != null)
				{
					ParseEndTag(optionalEndTag.Name, optionalEndTag.NameInLowercase);
				}
			}

			if (tagFlags.IsSet(HtmlTagFlags.Empty))
			{
				isEmptyTag = true;
			}
			else if (isEmptyTag)
			{
				tagFlags |= HtmlTagFlags.Empty;
			}

			int attributeCount = attributes.Count;

			for (int attributeIndex = 0; attributeIndex < attributeCount; attributeIndex++)
			{
				HtmlAttribute attribute = attributes[attributeIndex];
				attribute.Type = _attributeTypeDeterminer.GetAttributeType(tagNameInLowercase, tagFlags,
					attribute.NameInLowercase, attributes);
			}

			var tag = new HtmlTag(tagName, tagNameInLowercase, attributes, tagFlags);

			if (!isEmptyTag)
			{
				if (!_nonValidatingConditionalCommentOpened || tagFlags.IsSet(HtmlTagFlags.EmbeddedCode))
				{
					_tagStack.Push(tag);
				}

				if (!_xmlTagOpened && tagFlags.IsSet(HtmlTagFlags.Xml))
				{
					_xmlTagOpened = true;
				}
			}

			_handlers.StartTag?.Invoke(_context, tag);
		}

		/// <summary>
		/// Parses a end tag
		/// </summary>
		/// <param name="tagName">Tag name</param>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		private void ParseEndTag(string tagName, string tagNameInLowercase)
		{
			HtmlTag tag = null;

			if (_tagStack.Count > 0
				&& (!_nonValidatingConditionalCommentOpened || _tagTypeDeterminer.IsTagWithEmbeddedCode(tagNameInLowercase)))
			{
				HtmlTag currentStackedTag = _tagStack.GetFirstTagByNameInLowercase(tagNameInLowercase);

				if (currentStackedTag != null)
				{
					while (_tagStack.Count > 0)
					{
						HtmlTag stackedTag = _tagStack.Pop();
						bool isCurrentTag = stackedTag == currentStackedTag;
						string endTagName = isCurrentTag ? tagName : stackedTag.Name;

						tag = new HtmlTag(endTagName, stackedTag.NameInLowercase, stackedTag.Flags);
						InternalParseEndTag(tag);

						if (isCurrentTag)
						{
							break;
						}
					}
				}
			}

			if (tag == null)
			{
				tag = new HtmlTag(tagName, tagNameInLowercase, GetTagFlagsByName(tagNameInLowercase));
				InternalParseEndTag(tag);
			}
		}

		/// <summary>
		/// Parses a remaining end tags
		/// </summary>
		private void ParseRemainingEndTags()
		{
			// Close all the open elements, up the stack
			while (_tagStack.Count > 0)
			{
				HtmlTag stackedTag = _tagStack.Pop();
				var tag = new HtmlTag(stackedTag.Name, stackedTag.NameInLowercase, stackedTag.Flags);

				InternalParseEndTag(tag);
			}
		}

		private void InternalParseEndTag(HtmlTag tag)
		{
			HtmlTagFlags tagFlags = tag.Flags;

			if (_xmlTagOpened && !tagFlags.IsSet(HtmlTagFlags.NonIndependent))
			{
				_xmlTagOpened = false;
			}

			_handlers.EndTag?.Invoke(_context, tag);
		}

		#endregion

		#region Determining methods

		/// <summary>
		/// Gets a HTML tag flags by tag name
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Tag flags</returns>
		private HtmlTagFlags GetTagFlagsByName(string tagNameInLowercase)
		{
			HtmlTagFlags tagFlags;

			if (!_xmlTagOpened)
			{
				tagFlags = _tagTypeDeterminer.GetTagFlagsByName(tagNameInLowercase);
			}
			else
			{
				tagFlags = HtmlTagFlags.Xml | HtmlTagFlags.NonIndependent;
				if (_tagTypeDeterminer.IsTagWithEmbeddedCode(tagNameInLowercase))
				{
					tagFlags |= HtmlTagFlags.EmbeddedCode;
				}
			}

			return tagFlags;
		}

		/// <summary>
		/// Checks whether the tag supports embedded markup
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <returns>Result of check</returns>
		private static bool IsSupportEmbeddedMarkup(HtmlTag tag)
		{
			if (tag.NameInLowercase != "script")
			{
				return false;
			}

			string contentType = tag.Attributes
				.Where(a => a.NameInLowercase == "type")
				.Select(a => a.Value)
				.FirstOrDefault()
				;
			if (string.IsNullOrWhiteSpace(contentType))
			{
				return false;
			}

			string processedContentType = contentType.Trim().ToLowerInvariant();
			bool result = processedContentType.StartsWith("text/")
				&& (processedContentType.EndsWith("html") || processedContentType.EndsWith("-template"));

			return result;
		}

		#endregion
	}
}