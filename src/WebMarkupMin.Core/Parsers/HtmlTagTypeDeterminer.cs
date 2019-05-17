using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML tag type determiner
	/// </summary>
	internal class HtmlTagTypeDeterminer
	{
		/// <summary>
		/// Instance of HTML tag type determiner
		/// </summary>
		private static readonly Lazy<HtmlTagTypeDeterminer> _lazyInstance =
			new Lazy<HtmlTagTypeDeterminer>(() => new HtmlTagTypeDeterminer());

		/// <summary>
		/// Cache of the flags of HTML tags
		/// </summary>
		private readonly ConcurrentDictionary<string, HtmlTagFlags> _htmlTagFlagsCache =
			new ConcurrentDictionary<string, HtmlTagFlags>();

		#region HTML tags

		/// <summary>
		/// List of HTML tags
		/// </summary>
		private readonly HashSet<string> _htmlTags = new HashSet<string>
		{
			// HTML5
			"a", "abbr", "address", "area", "article", "aside", "audio",
			"b", "base", "bdi", "bdo", "blockquote", "body", "br", "button",
			"canvas", "caption", "cite", "code", "col", "colgroup",
			"data", "datalist", "dd", "del", "details", "dfn", "dialog", "div", "dl", "dt",
			"element", "em", "embed",
			"fieldset", "figcaption", "figure", "footer", "form",
			"h1", "h2", "h3", "h4", "h5", "h6", "head", "header", "hgroup", "hr", "html",
			"i", "iframe", "img", "input", "ins",
			"kbd", "keygen",
			"label", "legend", "li", "link",
			"main", "map", "mark", "menu", "menuitem", "meta", "meter",
			"nav", "noscript",
			"object", "ol", "optgroup", "option", "output",
			"p", "param", "picture", "pre", "progress",
			"q",
			"rb", "rbc", "rp", "rt", "rtc", "ruby",
			"s", "samp", "script", "section", "select", "shadow", "small", "source", "span", "strong", "style", "sub", "summary", "sup",
			"table", "tbody", "td", "template", "textarea", "tfoot", "th", "thead", "time", "title", "tr", "track",
			"u", "ul",
			"var", "video",
			"wbr",

			// Obsolete
			"acronym", "applet",
			"basefont", "big",
			"command",
			"dir",
			"font",
			"listing",
			"marquee",
			"plaintext",
			"spacer", "strike",
			"tt",
			"xmp",

			// Deprecated
			"blink",
			"center", "content",
			"frame", "frameset",
			"isindex",
			"multicol",
			"nextid", "noembed",
			"noframes",

			// Nonstandard
			"bgsound",
			"comment",
			"image",
			"nobr"
		};

		#endregion

		#region Invisible tags

		/// <summary>
		/// List of invisible tags
		/// </summary>
		private readonly HashSet<string> _invisibleTags = new HashSet<string>
		{
			// HTML5
			"base", "body",
			"datalist", "dialog",
			"head", "html",
			"link",
			"meta",
			"param",
			"script", "source", "style",
			"template", "title", "track",

			// Obsolete
			"basefont",

			// Deprecated
			"nextid",

			// Nonstandard
			"bgsound"
		};

		#endregion

		#region Empty tags

		/// <summary>
		/// List of empty tags
		/// </summary>
		private readonly HashSet<string> _emptyTags = new HashSet<string>
		{
			// HTML5
			"area",
			"base", "br",
			"col",
			"embed",
			"hr",
			"img", "input",
			"link",
			"meta",
			"param",
			"source",
			"track",
			"wbr",

			// Obsolete
			"basefont",

			// Deprecated
			"frame",
			"isindex",
			"nextid"
		};

		#endregion

		#region Block tags

		/// <summary>
		/// List of block tags
		/// </summary>
		private readonly HashSet<string> _blockTags = new HashSet<string>
		{
			// HTML5
			"address", "article", "aside",
			"blockquote",
			"caption", "col", "colgroup",
			"dd", "details", "dialog", "div", "dl", "dt",
			"fieldset", "figcaption", "figure", "footer", "form",
			"h1", "h2", "h3", "h4", "h5", "h6", "header", "hgroup", "hr",
			"legend", "li",
			"main", "menu", "menuitem",
			"noscript",
			"ol",
			"p", "pre",
			"section", "summary",
			"table", "tbody", "td", "tfoot", "th", "thead", "tr",
			"ul",

			// Obsolete
			"command",
			"dir",
			"marquee",
			"plaintext",
			"xmp",

			// Deprecated
			"center",
			"frame", "frameset",
			"isindex",
			"multicol",
			"noframes"
		};

		#endregion

		#region Inline tags

		/// <summary>
		/// List of inline tags
		/// </summary>
		private readonly HashSet<string> _inlineTags = new HashSet<string>
		{
			// HTML5
			"a", "abbr",
			"b", "bdi", "bdo", "br",
			"cite", "code",
			"data", "dfn",
			"em",
			"i", "img", "input",
			"kbd", "keygen",
			"label",
			"mark", "meter",
			"optgroup", "option", "output",
			"picture", "progress",
			"q",
			"rb", "rp", "rt", "rtc", "ruby",
			"s", "samp", "select", "small", "span", "strong", "sub", "sup",
			"textarea", "time",
			"u",
			"var",
			"wbr",

			// Obsolete
			"acronym",
			"big",
			"font",
			"spacer", "strike",
			"tt",

			// Deprecated
			"blink", "content",

			// Nonstandard
			"comment", "image", "nobr"
		};

		#endregion

		#region Inline-block tags

		/// <summary>
		/// List of inline-block tags
		/// </summary>
		private readonly HashSet<string> _inlineBlockTags = new HashSet<string>
		{
			// HTML5
			"area", "audio",
			"button",
			"canvas",
			"del",
			"embed",
			"iframe", "ins",
			"map", "math",
			"object",
			"script", "svg",
			"video",

			// Obsolete
			"applet",
			"listing",

			// Deprecated
			"noembed"
		};

		#endregion

		#region Non-independent tags

		/// <summary>
		/// List of non-independent tags
		/// </summary>
		private readonly HashSet<string> _nonIndependentTags = new HashSet<string>
		{
			// HTML5
			"area",
			"caption", "col", "colgroup",
			"dd", "dt",
			"figcaption",
			"legend", "li",
			"menuitem",
			"optgroup", "option",
			"param",
			"rb", "rp", "rt", "rtc",
			"source",
			"tbody", "td", "tfoot", "th", "thead", "tr", "track",

			// Obsolete
			"command",

			// Deprecated
			"frame"
		};

		#endregion

		#region Optional end tags

		/// <summary>
		/// List of tags, that can be omitted
		/// </summary>
		private readonly HashSet<string> _optionalTags = new HashSet<string>
		{
			"body",
			"colgroup",
			"dd", "dt",
			"head", "html",
			"li",
			"optgroup", "option",
			"p",
			"rb", "rp", "rt", "rtc",
			"tbody", "td", "tfoot", "th", "thead", "tr"
		};

		#endregion

		#region Tags with embedded code

		/// <summary>
		/// List of the tags, that can contain embedded code
		/// </summary>
		private readonly HashSet<string> _tagsWithEmbeddedCode = new HashSet<string>
		{
			"script", "style"
		};

		#endregion

		#region XML-based tags

		/// <summary>
		/// List of the XML-based tags
		/// </summary>
		private readonly HashSet<string> _xmlBasedTags = new HashSet<string>
		{
			"svg", "math"
		};

		#endregion

		/// <summary>
		/// Gets a instance of HTML tag type determiner
		/// </summary>
		public static HtmlTagTypeDeterminer Instance
		{
			get { return _lazyInstance.Value; }
		}


		/// <summary>
		/// Private constructor
		/// </summary>
		private HtmlTagTypeDeterminer()
		{ }


		/// <summary>
		/// Gets a HTML tag flags by tag name
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Tag flags</returns>
		public HtmlTagFlags GetTagFlagsByName(string tagNameInLowercase)
		{
			HtmlTagFlags tagFlags = _htmlTagFlagsCache.GetOrAdd(tagNameInLowercase, InnerGetTagFlagsByName);

			return tagFlags;
		}

		private HtmlTagFlags InnerGetTagFlagsByName(string tagNameInLowercase)
		{
			HtmlTagFlags tagFlags = HtmlTagFlags.None;
			bool isXml = false;

			var isHtml = IsHtmlTag(tagNameInLowercase);
			if (!isHtml)
			{
				isXml = IsXmlBasedTag(tagNameInLowercase);
			}

			if (isHtml || isXml)
			{
				if (IsInvisibleTag(tagNameInLowercase))
				{
					tagFlags |= HtmlTagFlags.Invisible;
				}

				if (IsEmptyTag(tagNameInLowercase))
				{
					tagFlags |= HtmlTagFlags.Empty;
				}

				if (IsBlockTag(tagNameInLowercase))
				{
					tagFlags |= HtmlTagFlags.Block;
				}

				if (IsInlineTag(tagNameInLowercase))
				{
					tagFlags |= HtmlTagFlags.Inline;
				}

				if (IsInlineBlockTag(tagNameInLowercase))
				{
					tagFlags |= HtmlTagFlags.InlineBlock;
				}

				if (IsNonIndependentTag(tagNameInLowercase))
				{
					tagFlags |= HtmlTagFlags.NonIndependent;
				}

				if (IsOptionalTag(tagNameInLowercase))
				{
					tagFlags |= HtmlTagFlags.Optional;
				}

				if (IsTagWithEmbeddedCode(tagNameInLowercase))
				{
					tagFlags |= HtmlTagFlags.EmbeddedCode;
				}

				if (isXml)
				{
					tagFlags |= HtmlTagFlags.Xml;
				}
			}
			else
			{
				tagFlags = HtmlTagFlags.Custom;
			}

			return tagFlags;
		}

		/// <summary>
		/// Checks whether the tag is HTML
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Result of check (true - HTML; false - not HTML)</returns>
		private bool IsHtmlTag(string tagNameInLowercase)
		{
			bool isHtmlTag = false;
			int charCount = tagNameInLowercase.Length;

			if (charCount > 0 && tagNameInLowercase[0].IsAlphaLower())
			{
				isHtmlTag = true;

				for (int charIndex = 1; charIndex < charCount; charIndex++)
				{
					char charValue = tagNameInLowercase[charIndex];

					if (!charValue.IsAlphaLower() && !charValue.IsNumeric())
					{
						isHtmlTag = false;
						break;
					}
				}

				if (isHtmlTag)
				{
					isHtmlTag = _htmlTags.Contains(tagNameInLowercase);
				}
			}

			return isHtmlTag;
		}

		/// <summary>
		/// Checks whether the tag is invisible
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Result of check (true - invisible; false - not invisible)</returns>
		private bool IsInvisibleTag(string tagNameInLowercase)
		{
			return _invisibleTags.Contains(tagNameInLowercase);
		}

		/// <summary>
		/// Checks whether the tag is empty
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Result of check (true - empty; false - not empty)</returns>
		private bool IsEmptyTag(string tagNameInLowercase)
		{
			return _emptyTags.Contains(tagNameInLowercase);
		}

		/// <summary>
		/// Checks whether the tag is block
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Result of check (true - block; false - not block)</returns>
		private bool IsBlockTag(string tagNameInLowercase)
		{
			return _blockTags.Contains(tagNameInLowercase);
		}

		/// <summary>
		/// Checks whether the tag is inline
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Result of check (true - inline; false - not inline)</returns>
		private bool IsInlineTag(string tagNameInLowercase)
		{
			return _inlineTags.Contains(tagNameInLowercase);
		}

		/// <summary>
		/// Checks whether the tag is inline-block
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Result of check (true - inline-block; false - not inline-block)</returns>
		private bool IsInlineBlockTag(string tagNameInLowercase)
		{
			return _inlineBlockTags.Contains(tagNameInLowercase);
		}

		/// <summary>
		/// Checks whether the tag is non-independent
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Result of check (true - non-independent; false - independent)</returns>
		private bool IsNonIndependentTag(string tagNameInLowercase)
		{
			return _nonIndependentTags.Contains(tagNameInLowercase);
		}

		/// <summary>
		/// Checks whether the tag, that can be omitted
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Result of check (true - tag is optional; false - tag is required)</returns>
		public bool IsOptionalTag(string tagNameInLowercase)
		{
			return _optionalTags.Contains(tagNameInLowercase);
		}

		/// <summary>
		/// Checks whether the tag can contain embedded code
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Result of check (true - can contain embedded code; false - cannot contain embedded code)</returns>
		public bool IsTagWithEmbeddedCode(string tagNameInLowercase)
		{
			return _tagsWithEmbeddedCode.Contains(tagNameInLowercase);
		}

		/// <summary>
		/// Checks whether the tag is XML-based
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Result of check (true - is XML-based; false - is not XML-based)</returns>
		public bool IsXmlBasedTag(string tagNameInLowercase)
		{
			return _xmlBasedTags.Contains(tagNameInLowercase);
		}
	}
}