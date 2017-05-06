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

		private static readonly Regex _xmlDeclarationRegex = new Regex(@"^<\?xml\s*[^>]+?\s*\?>", RegexOptions.IgnoreCase);
		private static readonly Regex _startTagRegex = new Regex(@"^<(?<tagName>" + CommonRegExps.HtmlTagNamePattern + ")" +
			"(?<attributes>" +
				"(?:" +
					"(?:" +
						@"(?:\s+|(?<=[""'\s]))" + CommonRegExps.HtmlAttributeNamePattern +
						@"(?:\s*=\s*(?:(?:""[^""]*?"")|(?:'[^']*?')|[^>""'\s]+)?)?" +
					")" +
					@"|(?:\s*(?<invalidCharacters>(?:[^/>\s][^>\s]*?)|(?:/[^>\s]*?(?!>))))" +
				")*" +
			")" +
			@"\s*(?<emptyTagSlash>/)?>");
		private static readonly Regex _endTagRegex = new Regex(@"^<\/(?<tagName>" + CommonRegExps.HtmlTagNamePattern + @")\s*>");
		private static readonly Regex _attributeRegex =
			new Regex(@"(?<attributeName>" + CommonRegExps.HtmlAttributeNamePattern + @")(?:\s*(?<attributeEqualSign>=)\s*" +
				@"(?:" +
					@"(?:""(?<attributeValue>[^""]*?)"")" +
					@"|(?:'(?<attributeValue>[^']*?)')" +
					@"|(?<attributeValue>[^>""'\s]+)" +
				@")?)?")
				;

		private static readonly Regex _hiddenIfCommentRegex =
			new Regex(@"^<!--\[if\s+(?<expression>[^\]]+?)\]>", RegexOptions.IgnoreCase);
		private static readonly Regex _hiddenEndIfCommentRegex = new Regex(@"^<!\[endif\s*\]-->", RegexOptions.IgnoreCase);
		private static readonly Regex _revealedIfCommentRegex =
			new Regex(@"^<!\[if\s+(?<expression>[^\]]+?)\]>", RegexOptions.IgnoreCase);
		private static readonly Regex _revealedEndIfCommentRegex = new Regex(@"^<!\[endif\s*\]>", RegexOptions.IgnoreCase);
		private static readonly Regex _revealedValidatingIfCommentRegex =
			new Regex(@"^<!--\[if\s+(?<expression>[^\]]+?)(?:\]>\s*(?<ltAndPling><!)?\s*-->)", RegexOptions.IgnoreCase);
		private static readonly Regex _revealedValidatingEndIfCommentRegex =
			new Regex(@"^<!--\s*<!\[endif\s*\]-->", RegexOptions.IgnoreCase);

		#endregion

		/// <summary>
		/// HTML parsing handlers
		/// </summary>
		private readonly HtmlParsingHandlers _handlers;

		/// <summary>
		/// Stack of tags
		/// </summary>
		private readonly List<HtmlTag> _tagStack;

		/// <summary>
		/// Cache of the flags of HTML tags
		/// </summary>
		private readonly Dictionary<string, HtmlTagFlags> _htmlTagFlagsCache;

		/// <summary>
		/// Cache of the flags of custom HTML tags
		/// </summary>
		private readonly Dictionary<string, HtmlTagFlags> _customHtmlTagFlagsCache;

		/// <summary>
		/// Cache of the tag with embedded code regular expressions
		/// </summary>
		private static readonly ConcurrentDictionary<string, Regex> _tagWithEmbeddedRegexCache;

		/// <summary>
		/// Stack of conditional comments
		/// </summary>
		private readonly Stack<HtmlConditionalComment> _conditionalCommentStack;

		/// <summary>
		/// Flag for whether the conditional comment is open
		/// </summary>
		private bool _conditionalCommentOpened;

		/// <summary>
		/// Stack of XML-based tags
		/// </summary>
		private readonly Stack<string> _xmlTagStack;

		/// <summary>
		/// Gets a common markup parsing handlers
		/// </summary>
		protected override MarkupParsingHandlersBase CommonHandlers
		{
			get { return _handlers; }
		}


		/// <summary>
		/// Static constructor
		/// </summary>
		static HtmlParser()
		{
			_tagWithEmbeddedRegexCache = new ConcurrentDictionary<string, Regex>();
		}

		/// <summary>
		/// Constructs instance of HTML parser
		/// </summary>
		/// <param name="handlers">HTML parsing handlers</param>
		public HtmlParser(HtmlParsingHandlers handlers)
		{
			_handlers = handlers;

			_tagStack = new List<HtmlTag>();
			_htmlTagFlagsCache = new Dictionary<string, HtmlTagFlags>();
			_customHtmlTagFlagsCache = new Dictionary<string, HtmlTagFlags>();
			_conditionalCommentStack = new Stack<HtmlConditionalComment>();
			_conditionalCommentOpened = false;
			_xmlTagStack = new Stack<string>();
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
						HtmlTag lastStackedTag = _tagStack.LastOrDefault();

						// Make sure we're not in a tag, that contains embedded code
						if (lastStackedTag == null || !lastStackedTag.Flags.HasFlag(HtmlTagFlags.EmbeddedCode))
						{
							int firstCharPosition = _innerContext.Position;
							char firstCharValue;
							bool firstCharExist = content.TryGetChar(firstCharPosition, out firstCharValue);

							if (firstCharExist && firstCharValue == '<')
							{
								int secondCharPosition = firstCharPosition + 1;
								char secondCharValue;
								bool secondCharExist = content.TryGetChar(secondCharPosition, out secondCharValue);

								if (secondCharExist)
								{
									if (secondCharValue.IsAlphaNumeric())
									{
										// Start tag
										isProcessed = ProcessStartTag();
									}
									else
									{
										int thirdCharPosition = secondCharPosition + 1;
										char thirdCharValue;
										bool thirdCharExist = content.TryGetChar(thirdCharPosition, out thirdCharValue);

										if (thirdCharExist)
										{
											switch (secondCharValue)
											{
												case '/':
													if (thirdCharValue.IsAlphaNumeric())
													{
														isProcessed = ProcessEndTag();
													}
													break;

												case '!':
													int fourthCharPosition = thirdCharPosition + 1;
													char fourthCharValue;
													bool fourthCharExist = content.TryGetChar(fourthCharPosition, out fourthCharValue);

													if (!fourthCharExist)
													{
														break;
													}

													switch (thirdCharValue)
													{
														case '-':
															if (fourthCharValue == '-')
															{
																// Comments
																int fifthCharPosition = fourthCharPosition + 1;
																char fifthCharValue;
																bool fifthCharExist = content.TryGetChar(fifthCharPosition, out fifthCharValue);

																if (fifthCharExist)
																{
																	if (fifthCharValue == '[')
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
																}

																if (!isProcessed)
																{
																	// HTML comments
																	isProcessed = ProcessComment();
																}
															}
															break;

														case '[':
															if (fourthCharValue == 'C')
															{
																// CDATA sections
																isProcessed = ProcessCdataSection();
															}
															else
															{
																// Remaining conditional comments

																// Hidden End If conditional comment (e.g. <![endif]-->)
																isProcessed = ProcessHiddenEndIfComment();

																if (!isProcessed)
																{
																	// Revealed If conditional comment (e.g. <![if ... ]>)
																	isProcessed = ProcessRevealedIfComment();
																}

																if (!isProcessed)
																{
																	// Revealed End If conditional comment (e.g. <![endif]>)
																	isProcessed = ProcessRevealedEndIfComment();
																}
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
									}
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
					ParseEndTag();

					// Check whether there were not closed conditional comment
					if (_conditionalCommentStack.Count > 0)
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
					_htmlTagFlagsCache.Clear();
					_customHtmlTagFlagsCache.Clear();
					_conditionalCommentStack.Clear();
					_conditionalCommentOpened = false;
					_xmlTagStack.Clear();
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

				if (_handlers.XmlDeclaration != null)
				{
					_handlers.XmlDeclaration(_context, xmlDeclaration);
				}

				_innerContext.IncreasePosition(xmlDeclaration.Length);
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

			var hiddenIfCommentMatch = _hiddenIfCommentRegex.Match(content, _innerContext.Position,
				contentRemainderLength);
			if (hiddenIfCommentMatch.Success)
			{
				string hiddenIfComment = hiddenIfCommentMatch.Value;
				var groups = hiddenIfCommentMatch.Groups;
				string expression = groups["expression"].Value.Trim();

				ParseIfConditionalComment(expression, HtmlConditionalCommentType.Hidden);

				_innerContext.IncreasePosition(hiddenIfComment.Length);
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

			var hiddenEndIfCommentMatch = _hiddenEndIfCommentRegex.Match(content, _innerContext.Position,
				contentRemainderLength);
			if (hiddenEndIfCommentMatch.Success)
			{
				string hiddenEndIfComment = hiddenEndIfCommentMatch.Value;

				ParseEndIfConditionalComment(HtmlConditionalCommentType.Hidden);

				_innerContext.IncreasePosition(hiddenEndIfComment.Length);
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

			var revealedIfCommentMatch = _revealedIfCommentRegex.Match(content, _innerContext.Position,
				contentRemainderLength);
			if (revealedIfCommentMatch.Success)
			{
				string revealedIfComment = revealedIfCommentMatch.Value;
				var groups = revealedIfCommentMatch.Groups;
				string expression = groups["expression"].Value.Trim();

				ParseIfConditionalComment(expression, HtmlConditionalCommentType.Revealed);

				_innerContext.IncreasePosition(revealedIfComment.Length);
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

			var revealedEndIfCommentMatch = _revealedEndIfCommentRegex.Match(content, _innerContext.Position,
				contentRemainderLength);
			if (revealedEndIfCommentMatch.Success)
			{
				string revealedEndIfComment = revealedEndIfCommentMatch.Value;

				ParseEndIfConditionalComment(HtmlConditionalCommentType.Revealed);

				_innerContext.IncreasePosition(revealedEndIfComment.Length);
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

			var revealedValidatingIfCommentMatch = _revealedValidatingIfCommentRegex.Match(content,
				_innerContext.Position, contentRemainderLength);
			if (revealedValidatingIfCommentMatch.Success)
			{
				string revealedValidatingIfComment = revealedValidatingIfCommentMatch.Value;
				var groups = revealedValidatingIfCommentMatch.Groups;
				string expression = groups["expression"].Value.Trim();
				string ltAndPling = groups["ltAndPling"].Value;
				var type = ltAndPling.Length > 0 ?
					HtmlConditionalCommentType.RevealedValidating
					:
					HtmlConditionalCommentType.RevealedValidatingSimplified
					;

				ParseIfConditionalComment(expression, type);

				_innerContext.IncreasePosition(revealedValidatingIfComment.Length);
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

			var revealedValidatingEndIfCommentMatch = _revealedValidatingEndIfCommentRegex.Match(content,
				_innerContext.Position, contentRemainderLength);
			if (revealedValidatingEndIfCommentMatch.Success)
			{
				string revealedValidatingEndIfComment = revealedValidatingEndIfCommentMatch.Value;

				ParseEndIfConditionalComment(HtmlConditionalCommentType.RevealedValidating);

				_innerContext.IncreasePosition(revealedValidatingEndIfComment.Length);
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
			int contentRemainderLength = _innerContext.RemainderLength;

			var match = _startTagRegex.Match(content, _innerContext.Position, contentRemainderLength);
			if (match.Success)
			{
				GroupCollection groups = match.Groups;
				string startTagName = groups["tagName"].Value;
				string startTagNameInLowercase = startTagName;
				if (Utils.ContainsUppercaseCharacters(startTagName))
				{
					startTagNameInLowercase = startTagName.ToLowerInvariant();
				}

				Group invalidCharactersGroup = groups["invalidCharacters"];
				if (invalidCharactersGroup.Success)
				{
					int invalidCharactersPosition = invalidCharactersGroup.Index;
					int invalidCharactersOffset = invalidCharactersPosition - _innerContext.Position;

					_innerContext.IncreasePosition(invalidCharactersOffset);

					throw new MarkupParsingException(
						string.Format(Strings.ErrorMessage_InvalidCharactersInStartTag, startTagName),
						_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
				}

				string startTag = match.Value;
				bool isEmptyTag = groups["emptyTagSlash"].Success;

				Group attributesGroup = groups["attributes"];
				var attributesCoordinates = SourceCodeNodeCoordinates.Empty;
				int attributesPosition = -1;
				string attributesString = string.Empty;

				if (attributesGroup.Success)
				{
					attributesPosition = attributesGroup.Index;
					attributesString = attributesGroup.Value;
				}

				if (attributesPosition != -1)
				{
					int nodePosition = _innerContext.Position;
					SourceCodeNodeCoordinates nodeCoordinates = _innerContext.NodeCoordinates;
					int attributesOffset = attributesPosition - nodePosition;

					int lineBreakCount;
					int charRemainderCount;

					SourceCodeNavigator.CalculateLineBreakCount(content, nodePosition,
						attributesOffset, out lineBreakCount, out charRemainderCount);

					attributesCoordinates = SourceCodeNavigator.CalculateAbsoluteNodeCoordinates(
						nodeCoordinates, lineBreakCount, charRemainderCount);
				}

				ParseStartTag(startTagName, startTagNameInLowercase, attributesString, attributesCoordinates,
					isEmptyTag);

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
				string endTagNameInLowercase = endTagName;
				if (Utils.ContainsUppercaseCharacters(endTagName))
				{
					endTagNameInLowercase = endTagName.ToLowerInvariant();
				}

				ParseEndTag(endTagName, endTagNameInLowercase);

				_innerContext.IncreasePosition(endTag.Length);
				isProcessed = true;
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a embedded code
		/// </summary>
		private void ProcessEmbeddedCode()
		{
			string content = _innerContext.SourceCode;
			int contentRemainderLength = _innerContext.RemainderLength;

			HtmlTag stackedTag = _tagStack.LastOrDefault();
			if (stackedTag != null)
			{
				string stackedTagName = stackedTag.Name;
				string stackedTagNameInLowercase = stackedTag.NameInLowercase;
				Regex stackedTagRegex = _tagWithEmbeddedRegexCache.GetOrAdd(stackedTagNameInLowercase,
					key => new Regex(@"([\s\S]*?)</" + Regex.Escape(key) + @"\s*>", RegexOptions.IgnoreCase));

				var stackedTagMatch = stackedTagRegex.Match(content, _innerContext.Position, contentRemainderLength);
				string htmlFragment = stackedTagMatch.Value;
				string code = stackedTagMatch.Groups[1].Value;

				if (_handlers.EmbeddedCode != null)
				{
					_handlers.EmbeddedCode(_context, code);
				}

				ParseEndTag(stackedTagName, stackedTagNameInLowercase);

				_innerContext.IncreasePosition(htmlFragment.Length);
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
						if (_handlers.TemplateTag != null)
						{
							_handlers.TemplateTag(_context, expression, startDelimiter, endDelimiter);
						}

						_innerContext.IncreasePosition(startDelimiter.Length + expression.Length + endDelimiter.Length);
					},
					(localContext, textValue) =>
					{
						if (_handlers.Text != null)
						{
							_handlers.Text(_context, textValue);
						}

						_innerContext.IncreasePosition(textValue.Length);
					}
				);
			}
			else
			{
				if (_handlers.Text != null)
				{
					_handlers.Text(_context, text);
				}

				_innerContext.IncreasePosition(text.Length);
			}
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
			if (_conditionalCommentStack.Count > 0)
			{
				throw new MarkupParsingException(
					Strings.ErrorMessage_NotClosedConditionalComment,
					_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
			}

			string processedExpression = expression.Trim();

			_conditionalCommentStack.Push(new HtmlConditionalComment(expression, type));
			if (type == HtmlConditionalCommentType.Hidden || type == HtmlConditionalCommentType.Revealed)
			{
				_conditionalCommentOpened = true;
			}

			if (_handlers.IfConditionalComment != null)
			{
				_handlers.IfConditionalComment(_context, new HtmlConditionalComment(processedExpression, type));
			}
		}

		/// <summary>
		/// Parses a End If conditional comment
		/// </summary>
		/// <param name="type">Conditional comment type</param>
		private void ParseEndIfConditionalComment(HtmlConditionalCommentType type)
		{
			if (_conditionalCommentStack.Count > 0)
			{
				var stackedConditionalComment = _conditionalCommentStack.Pop();
				var stackedType = stackedConditionalComment.Type;

				if (type == HtmlConditionalCommentType.Hidden || type == HtmlConditionalCommentType.Revealed)
				{
					if (stackedType == type)
					{
						_conditionalCommentOpened = false;
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

			if (_handlers.EndIfConditionalComment != null)
			{
				_handlers.EndIfConditionalComment(_context, type);
			}
		}

		/// <summary>
		/// Parses a start tag
		/// </summary>
		/// <param name="tagName">Tag name</param>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <param name="attributesString">String representation of the attribute list</param>
		/// <param name="attributesCoordinates">Attributes coordinates</param>
		/// <param name="isEmptyTag">Flag that tag is empty</param>
		private void ParseStartTag(string tagName, string tagNameInLowercase, string attributesString,
			SourceCodeNodeCoordinates attributesCoordinates, bool isEmptyTag)
		{
			HtmlTagFlags tagFlags = GetTagFlagsByName(tagNameInLowercase);

			if (tagFlags.HasFlag(HtmlTagFlags.Optional))
			{
				HtmlTag lastStackedTag = _tagStack.LastOrDefault();
				if (lastStackedTag != null && lastStackedTag.NameInLowercase == tagNameInLowercase)
				{
					ParseEndTag(lastStackedTag.Name, lastStackedTag.NameInLowercase);
				}
				else
				{
					if (tagNameInLowercase == "body" && _tagStack.Any(t => t.NameInLowercase == "head"))
					{
						HtmlTag headTag = _tagStack.Single(t => t.NameInLowercase == "head");
						ParseEndTag(headTag.Name, headTag.NameInLowercase);
					}
				}
			}

			if (tagFlags.HasFlag(HtmlTagFlags.Empty))
			{
				isEmptyTag = true;
			}
			else if (isEmptyTag)
			{
				tagFlags |= HtmlTagFlags.Empty;
			}

			var attributes = ParseAttributes(tagName, tagNameInLowercase, tagFlags,
				attributesString, attributesCoordinates);
			var tag = new HtmlTag(tagName, tagNameInLowercase, attributes, tagFlags);

			if (!isEmptyTag)
			{
				if (_conditionalCommentOpened)
				{
					HtmlConditionalComment lastConditionalComment = _conditionalCommentStack.Peek();
					HtmlConditionalCommentType lastConditionalCommentType = lastConditionalComment.Type;

					if (tagFlags.HasFlag(HtmlTagFlags.EmbeddedCode)
						|| lastConditionalCommentType == HtmlConditionalCommentType.RevealedValidating
						|| lastConditionalCommentType == HtmlConditionalCommentType.RevealedValidatingSimplified)
					{
						_tagStack.Add(tag);
					}
				}
				else
				{
					_tagStack.Add(tag);
				}
			}

			if (_handlers.StartTag != null)
			{
				_handlers.StartTag(_context, tag);
			}

			if (tagFlags.HasFlag(HtmlTagFlags.Xml) && !tagFlags.HasFlag(HtmlTagFlags.NonIndependent))
			{
				_xmlTagStack.Push(tagNameInLowercase);
			}
		}

		/// <summary>
		/// Parses a attributes
		/// </summary>
		/// <param name="tagName">Tag name</param>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <param name="tagFlags">Tag flags</param>
		/// <param name="attributesString">String representation of the attribute list</param>
		/// <param name="attributesCoordinates">Attributes coordinates</param>
		/// <returns>List of attributes</returns>
		private IList<HtmlAttribute> ParseAttributes(string tagName, string tagNameInLowercase, HtmlTagFlags tagFlags,
			string attributesString, SourceCodeNodeCoordinates attributesCoordinates)
		{
			var attributes = new List<HtmlAttribute>();
			if (string.IsNullOrWhiteSpace(attributesString))
			{
				return attributes;
			}

			SourceCodeNodeCoordinates currentAttributesCoordinates = attributesCoordinates;
			int currentPosition = 0;
			MatchCollection attributeMatches = _attributeRegex.Matches(attributesString);

			foreach (Match attributeMatch in attributeMatches)
			{
				GroupCollection groups = attributeMatch.Groups;
				Group attributeNameGroup = groups["attributeName"];
				Group attributeEqualSignGroup = groups["attributeEqualSign"];
				Group attributeValueGroup = groups["attributeValue"];

				string attributeName = attributeNameGroup.Value;
				string attributeNameInLowercase = attributeName;
				if (Utils.ContainsUppercaseCharacters(attributeName))
				{
					attributeNameInLowercase = attributeName.ToLowerInvariant();
				}
				string attributeValue = null;

				if (attributeEqualSignGroup.Success)
				{
					if (attributeValueGroup.Success)
					{
						attributeValue = attributeValueGroup.Value;
						if (!string.IsNullOrWhiteSpace(attributeValue))
						{
							attributeValue = HtmlAttribute.HtmlAttributeDecode(attributeValue);
						}
					}
					else
					{
						attributeValue = string.Empty;
					}
				}

				var attributeNameCoordinates = SourceCodeNodeCoordinates.Empty;
				int attributeNamePosition = -1;
				if (attributeNameGroup.Success)
				{
					attributeNamePosition = attributeNameGroup.Index;
				}

				if (attributeNamePosition != -1)
				{
					int lineBreakCount;
					int charRemainderCount;

					SourceCodeNavigator.CalculateLineBreakCount(attributesString, currentPosition,
						attributeNamePosition - currentPosition, out lineBreakCount, out charRemainderCount);
					attributeNameCoordinates = SourceCodeNavigator.CalculateAbsoluteNodeCoordinates(
						currentAttributesCoordinates, lineBreakCount, charRemainderCount);

					currentAttributesCoordinates = attributeNameCoordinates;
					currentPosition = attributeNamePosition;
				}

				var attributeValueCoordinates = SourceCodeNodeCoordinates.Empty;
				int attributeValuePosition = -1;
				if (attributeValueGroup.Success)
				{
					attributeValuePosition = attributeValueGroup.Index;
				}

				if (attributeValuePosition != -1)
				{
					int lineBreakCount;
					int charRemainderCount;

					SourceCodeNavigator.CalculateLineBreakCount(attributesString, currentPosition,
						attributeValuePosition - currentPosition, out lineBreakCount, out charRemainderCount);
					attributeValueCoordinates = SourceCodeNavigator.CalculateAbsoluteNodeCoordinates(
						currentAttributesCoordinates, lineBreakCount, charRemainderCount);

					currentAttributesCoordinates = attributeValueCoordinates;
					currentPosition = attributeValuePosition;
				}

				HtmlAttributeType attributeType = GetAttributeType(tagNameInLowercase, tagFlags,
					attributeNameInLowercase, attributes);

				attributes.Add(new HtmlAttribute(attributeName, attributeNameInLowercase, attributeValue,
					attributeType, attributeNameCoordinates, attributeValueCoordinates));
			}

			return attributes;
		}

		/// <summary>
		/// Parses a end tag
		/// </summary>
		private void ParseEndTag()
		{
			ParseEndTag(null, null);
		}

		/// <summary>
		/// Parses a end tag
		/// </summary>
		/// <param name="tagName">Tag name</param>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		private void ParseEndTag(string tagName, string tagNameInLowercase)
		{
			int endTagIndex = 0;
			int lastTagIndex = _tagStack.Count - 1;
			bool tagNameNotEmpty = !string.IsNullOrEmpty(tagName);
			HtmlParsingHandlers.EndTagDelegate endTagHandler = _handlers.EndTag;

			if (tagNameNotEmpty)
			{
				for (endTagIndex = lastTagIndex; endTagIndex >= 0; endTagIndex--)
				{
					if (_tagStack[endTagIndex].NameInLowercase == tagNameInLowercase)
					{
						break;
					}
				}
			}

			if (endTagIndex >= 0)
			{
				// Close all the open elements, up the stack
				if (endTagHandler != null)
				{
					for (int tagIndex = lastTagIndex; tagIndex >= endTagIndex; tagIndex--)
					{
						HtmlTag startTag = _tagStack[tagIndex];
						string startTagNameInLowercase = startTag.NameInLowercase;
						HtmlTagFlags startTagFlags = startTag.Flags;

						string endTagName;
						if (tagNameNotEmpty && tagNameInLowercase == startTagNameInLowercase)
						{
							endTagName = tagName;
						}
						else
						{
							endTagName = startTag.Name;
						}

						if (_xmlTagStack.Count > 0 && !startTagFlags.HasFlag(HtmlTagFlags.NonIndependent))
						{
							_xmlTagStack.Pop();
						}

						var endTag = new HtmlTag(endTagName, startTagNameInLowercase, startTagFlags);
						endTagHandler(_context, endTag);
					}
				}

				// Remove the open elements from the stack
				if (endTagIndex <= lastTagIndex)
				{
					int tagToRemoveStartIndex = endTagIndex;
					int tagsToRemoveCount = lastTagIndex - endTagIndex + 1;

					_tagStack.RemoveRange(tagToRemoveStartIndex, tagsToRemoveCount);
				}
			}
			else if (tagNameNotEmpty && _conditionalCommentOpened)
			{
				if (_xmlTagStack.Count > 0 && HtmlTagFlagsHelpers.IsXmlBasedTag(tagNameInLowercase))
				{
					_xmlTagStack.Pop();
				}

				var endTag = new HtmlTag(tagName, tagNameInLowercase, GetTagFlagsByName(tagNameInLowercase));
				if (endTagHandler != null)
				{
					endTagHandler(_context, endTag);
				}
			}
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

			if (_xmlTagStack.Count == 0)
			{
				if (_htmlTagFlagsCache.ContainsKey(tagNameInLowercase))
				{
					tagFlags = _htmlTagFlagsCache[tagNameInLowercase];
				}
				else if (_customHtmlTagFlagsCache.ContainsKey(tagNameInLowercase))
				{
					tagFlags = _customHtmlTagFlagsCache[tagNameInLowercase];
				}
				else
				{
					tagFlags = HtmlTagFlags.None;
					bool isXml = false;

					var isHtml = HtmlTagFlagsHelpers.IsHtmlTag(tagNameInLowercase);
					if (!isHtml)
					{
						isXml = HtmlTagFlagsHelpers.IsXmlBasedTag(tagNameInLowercase);
					}

					if (isHtml || isXml)
					{
						if (HtmlTagFlagsHelpers.IsInvisibleTag(tagNameInLowercase))
						{
							tagFlags |= HtmlTagFlags.Invisible;
						}

						if (HtmlTagFlagsHelpers.IsEmptyTag(tagNameInLowercase))
						{
							tagFlags |= HtmlTagFlags.Empty;
						}

						if (HtmlTagFlagsHelpers.IsBlockTag(tagNameInLowercase))
						{
							tagFlags |= HtmlTagFlags.Block;
						}

						if (HtmlTagFlagsHelpers.IsInlineTag(tagNameInLowercase))
						{
							tagFlags |= HtmlTagFlags.Inline;
						}

						if (HtmlTagFlagsHelpers.IsInlineBlockTag(tagNameInLowercase))
						{
							tagFlags |= HtmlTagFlags.InlineBlock;
						}

						if (HtmlTagFlagsHelpers.IsNonIndependentTag(tagNameInLowercase))
						{
							tagFlags |= HtmlTagFlags.NonIndependent;
						}

						if (HtmlTagFlagsHelpers.IsOptionalTag(tagNameInLowercase))
						{
							tagFlags |= HtmlTagFlags.Optional;
						}

						if (HtmlTagFlagsHelpers.IsTagWithEmbeddedCode(tagNameInLowercase))
						{
							tagFlags |= HtmlTagFlags.EmbeddedCode;
						}

						if (isXml)
						{
							tagFlags |= HtmlTagFlags.Xml;
						}

						_htmlTagFlagsCache[tagNameInLowercase] = tagFlags;
					}
					else
					{
						tagFlags = HtmlTagFlags.Custom;
					}
				}
			}
			else
			{
				tagFlags = HtmlTagFlags.Xml | HtmlTagFlags.NonIndependent;
				if (HtmlTagFlagsHelpers.IsTagWithEmbeddedCode(tagNameInLowercase))
				{
					tagFlags |= HtmlTagFlags.EmbeddedCode;
				}
			}

			return tagFlags;
		}

		/// <summary>
		/// Gets a HTML attribute type
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <param name="tagFlags">Tag flags</param>
		/// <param name="attributeNameInLowercase">Attribute name in lowercase</param>
		/// <param name="attributes">List of attributes</param>
		/// <returns>Attribute type</returns>
		private HtmlAttributeType GetAttributeType(string tagNameInLowercase, HtmlTagFlags tagFlags,
			string attributeNameInLowercase, IList<HtmlAttribute> attributes)
		{
			HtmlAttributeType attributeType = HtmlAttributeType.Unknown;

			if (attributeNameInLowercase == "class")
			{
				attributeType = HtmlAttributeType.ClassName;
			}
			else if (attributeNameInLowercase == "style")
			{
				attributeType = HtmlAttributeType.Style;
			}
			else if (HtmlAttributeTypeHelpers.IsEventAttribute(attributeNameInLowercase))
			{
				attributeType = HtmlAttributeType.Event;
			}

			if (attributeType == HtmlAttributeType.Unknown && !tagFlags.HasFlag(HtmlTagFlags.Xml))
			{
				if (HtmlAttributeTypeHelpers.IsBooleanAttribute(attributeNameInLowercase))
				{
					attributeType = HtmlAttributeType.Boolean;
				}
				else if (HtmlAttributeTypeHelpers.IsNumericAttribute(tagNameInLowercase, attributeNameInLowercase))
				{
					attributeType = HtmlAttributeType.Numeric;
				}
				else if (HtmlAttributeTypeHelpers.IsUriBasedAttribute(tagNameInLowercase, attributeNameInLowercase,
					attributes))
				{
					attributeType = HtmlAttributeType.Uri;
				}
			}

			if (attributeType == HtmlAttributeType.Unknown)
			{
				attributeType = HtmlAttributeTypeHelpers.IsXmlBasedAttribute(attributeNameInLowercase) ?
					HtmlAttributeType.Xml : HtmlAttributeType.Text;
			}

			return attributeType;
		}

		#endregion
	}
}