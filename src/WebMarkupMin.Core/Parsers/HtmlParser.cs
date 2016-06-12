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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using WebMarkupMin.Core.Helpers;
using WebMarkupMin.Core.Utilities;
using CoreStrings = WebMarkupMin.Core.Resources;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML parser
	/// </summary>
	internal sealed class HtmlParser
	{
		#region Regular expressions for parsing tags and attributes

		private const string TAG_NAME_PATTERN = @"[a-zA-Z0-9][a-zA-Z0-9-_:]*";
		private const string ATTR_NAME_PATTERN = @"[^\s""'<>/=]+";

		private static readonly Regex _xmlDeclarationRegex = new Regex(@"^<\?xml\s*[^>]+?\s*\?>", RegexOptions.IgnoreCase);
		private static readonly Regex _startTagRegex = new Regex(@"^<(?<tagName>" + TAG_NAME_PATTERN + ")" +
			"(?<attributes>" +
				"(?:" +
					"(?:" +
						@"(?:\s+|(?<=[""'\s]))" + ATTR_NAME_PATTERN +
						@"(?:\s*=\s*(?:(?:""[^""]*?"")|(?:'[^']*?')|[^>""'\s]+)?)?" +
					")" +
					@"|(?:\s*(?<invalidCharacters>(?:[^/>\s][^>\s]*?)|(?:/[^>\s]*?(?!>))))" +
				")*" +
			")" +
			@"\s*(?<emptyTagSlash>/?)>");
		private static readonly Regex _endTagRegex = new Regex(@"^<\/(?<tagName>" + TAG_NAME_PATTERN + @")\s*>");
		private static readonly Regex _attributeRegex =
			new Regex(@"(?<attributeName>" + ATTR_NAME_PATTERN + @")(?:\s*(?<attributeEqualSign>=)\s*" +
				@"(?:(?:""(?<attributeValue>[^""]*?)"")|(?:'(?<attributeValue>[^']*?)')|(?<attributeValue>[^>""'\s]+))?)?");

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
		/// Inner markup parsing context
		/// </summary>
		private InnerMarkupParsingContext _innerContext;

		/// <summary>
		/// Markup parsing context
		/// </summary>
		private MarkupParsingContext _context;

		/// <summary>
		/// HTML parsing handlers
		/// </summary>
		private readonly HtmlParsingHandlers _handlers;

		/// <summary>
		/// Stack of tags
		/// </summary>
		private List<HtmlTag> _tagStack;

		/// <summary>
		/// Cache of HTML tag flags
		/// </summary>
		private Dictionary<string, HtmlTagFlags> _tagFlagsCache;

		/// <summary>
		/// Cache of the tag with embedded code regular expressions
		/// </summary>
		private Dictionary<string, Regex> _tagWithEmbeddedRegexCache;

		/// <summary>
		/// Stack of conditional comments
		/// </summary>
		private Stack<HtmlConditionalComment> _conditionalCommentStack;

		/// <summary>
		/// Flag for whether the conditional comment is open
		/// </summary>
		private bool _conditionalCommentOpened;

		/// <summary>
		/// Synchronizer of parsing
		/// </summary>
		private readonly object _parsingSynchronizer = new object();


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

				_tagStack = new List<HtmlTag>();
				_tagFlagsCache = new Dictionary<string, HtmlTagFlags>();
				_tagWithEmbeddedRegexCache = new Dictionary<string, Regex>();
				_conditionalCommentStack = new Stack<HtmlConditionalComment>();
				_conditionalCommentOpened = false;

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
							if (content.CustomStartsWith("<", _innerContext.Position, StringComparison.Ordinal))
							{
								if (content.CustomStartsWith("</", _innerContext.Position, StringComparison.Ordinal))
								{
									// End tag
									isProcessed = ProcessEndTag();
								}
								else if (content.CustomStartsWith("<!", _innerContext.Position, StringComparison.Ordinal))
								{
									if (content.CustomStartsWith("<!--", _innerContext.Position, StringComparison.Ordinal))
									{
										// Comments
										if (content.CustomStartsWith("<!--[", _innerContext.Position, StringComparison.Ordinal))
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

										if (!isProcessed)
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
									else if (content.CustomStartsWith("<![", _innerContext.Position, StringComparison.Ordinal))
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
									else
									{
										// Doctype declaration
										isProcessed = ProcessDoctype();
									}
								}
								else if (content.CustomStartsWith("<?", _innerContext.Position, StringComparison.Ordinal))
								{
									// XML declaration
									isProcessed = ProcessXmlDeclaration();
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
						}
						else
						{
							// Embedded code
							ProcessEmbeddedCode();
						}

						if (_innerContext.Position == previousPosition)
						{
							throw new HtmlParsingException(
								string.Format(CoreStrings.ErrorMessage_MarkupParsingFailed, "HTML"),
								_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
						}

						previousPosition = _innerContext.Position;
					}

					// Clean up any remaining tags
					ParseEndTag();

					// Check whether there were not closed conditional comment
					if (_conditionalCommentStack.Count > 0)
					{
						throw new HtmlParsingException(
							CoreStrings.ErrorMessage_NotClosedConditionalComment,
							_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
					}
				}
				catch (HtmlParsingException)
				{
					throw;
				}
				finally
				{
					_tagStack.Clear();
					_tagFlagsCache.Clear();
					_tagWithEmbeddedRegexCache.Clear();
					_conditionalCommentStack.Clear();
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
		/// Process a HTML comments
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		private bool ProcessComment()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;

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

					throw new HtmlParsingException(
						string.Format(CoreStrings.ErrorMessage_InvalidCharactersInStartTag, startTagName),
						_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
				}

				string startTag = match.Value;
				bool isEmptyTag = groups["emptyTagSlash"].Value.Length > 0;

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
				Regex stackedTagRegex;

				if (_tagWithEmbeddedRegexCache.ContainsKey(stackedTagNameInLowercase))
				{
					stackedTagRegex = _tagWithEmbeddedRegexCache[stackedTagNameInLowercase];
				}
				else
				{
					stackedTagRegex = new Regex(@"([\s\S]*?)</" + Regex.Escape(stackedTagNameInLowercase) + @"\s*>",
						RegexOptions.IgnoreCase);
					_tagWithEmbeddedRegexCache[stackedTagNameInLowercase] = stackedTagRegex;
				}

				var stackedTagMatch = stackedTagRegex.Match(content, _innerContext.Position, contentRemainderLength);
				string htmlFragment = stackedTagMatch.Value;
				string text = stackedTagMatch.Groups[1].Value;

				if (_handlers.Text != null)
				{
					_handlers.Text(_context, text);
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
				throw new HtmlParsingException(
					CoreStrings.ErrorMessage_NotClosedConditionalComment,
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
						throw new HtmlParsingException(
							CoreStrings.ErrorMessage_InvalidEndIfConditionalComment,
							_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
					}
				}
				else if (type == HtmlConditionalCommentType.RevealedValidating
					|| type == HtmlConditionalCommentType.RevealedValidatingSimplified)
				{
					if (stackedType != HtmlConditionalCommentType.RevealedValidating
						&& stackedType != HtmlConditionalCommentType.RevealedValidatingSimplified)
					{
						throw new HtmlParsingException(
							CoreStrings.ErrorMessage_InvalidEndIfConditionalComment,
							_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
					}
				}
			}
			else
			{
				throw new HtmlParsingException(
					CoreStrings.ErrorMessage_IfConditionalCommentNotDeclared,
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

			var attributes = ParseAttributes(tagName, tagNameInLowercase, attributesString, attributesCoordinates);
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
		}

		/// <summary>
		/// Parses a attributes
		/// </summary>
		/// <param name="tagName">Tag name</param>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <param name="attributesString">String representation of the attribute list</param>
		/// <param name="attributesCoordinates">Attributes coordinates</param>
		/// <returns>List of attributes</returns>
		private IList<HtmlAttribute> ParseAttributes(string tagName, string tagNameInLowercase,
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

				HtmlAttributeType attributeType = GetAttributeType(tagNameInLowercase, attributeNameInLowercase,
					attributes);

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

						string endTagName;
						if (tagNameNotEmpty && tagNameInLowercase == startTagNameInLowercase)
						{
							endTagName = tagName;
						}
						else
						{
							endTagName = startTag.Name;
						}
						var endTag = new HtmlTag(endTagName, startTagNameInLowercase, startTag.Flags);

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

			if (_tagFlagsCache.ContainsKey(tagNameInLowercase))
			{
				tagFlags = _tagFlagsCache[tagNameInLowercase];
			}
			else
			{
				tagFlags = HtmlTagFlags.None;
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
				if (HtmlTagFlagsHelpers.IsXmlBasedTag(tagNameInLowercase))
				{
					tagFlags |= HtmlTagFlags.Xml;
				}

				_tagFlagsCache[tagNameInLowercase] = tagFlags;
			}

			return tagFlags;
		}

		/// <summary>
		/// Gets a HTML attribute type
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <param name="attributeNameInLowercase">Attribute name in lowercase</param>
		/// <param name="attributes">List of attributes</param>
		/// <returns>Attribute type</returns>
		private HtmlAttributeType GetAttributeType(string tagNameInLowercase, string attributeNameInLowercase,
			IList<HtmlAttribute> attributes)
		{
			HtmlAttributeType attributeType;
			if (attributeNameInLowercase == "class")
			{
				attributeType = HtmlAttributeType.ClassName;
			}
			else if (attributeNameInLowercase == "style")
			{
				attributeType = HtmlAttributeType.Style;
			}
			else if (HtmlAttributeTypeHelpers.IsBooleanAttribute(attributeNameInLowercase))
			{
				attributeType = HtmlAttributeType.Boolean;
			}
			else if (HtmlAttributeTypeHelpers.IsEventAttribute(attributeNameInLowercase))
			{
				attributeType = HtmlAttributeType.Event;
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
			else
			{
				attributeType = HtmlAttributeType.Text;
			}

			return attributeType;
		}

		#endregion
	}
}