using System.Collections.Generic;

using WebMarkupMin.Core.Utilities;
using CoreStrings = WebMarkupMin.Core.Resources;

namespace WebMarkupMin.Core.Helpers
{
	/// <summary>
	/// HTML tag flags helpers
	/// </summary>
	internal static class HtmlTagFlagsHelpers
	{
		#region Invisible tags

		/// <summary>
		/// List of invisible tags
		/// </summary>
		private static readonly HashSet<string> _invisibleTags = Utils.UnionHashSets(
			// HTML5
			new []
			{
				"base", "body",
				"datalist", "dialog",
				"head", "html",
				"link",
				"meta",
				"param",
				"script", "source", "style",
				"template", "title", "track"
			},

			// Deprecated
			new []
			{
				"basefont", "bgsound"
			}
		);

		#endregion

		#region Empty tags

		/// <summary>
		/// List of empty tags
		/// </summary>
		private static readonly HashSet<string> _emptyTags = Utils.UnionHashSets(
			// HTML5
			new []
			{
				"area",
				"base", "basefont", "br",
				"col",
				"embed",
				"frame",
				"hr",
				"img", "input",
				"link",
				"meta",
				"param",
				"source",
				"track"
			},

			// Deprecated
			new []
			{
				"isindex"
			}
		);

		#endregion

		#region Block tags

		/// <summary>
		/// List of block tags
		/// </summary>
		private static readonly HashSet<string> _blockTags = Utils.UnionHashSets(
			// HTML5
			new []
			{
				"address", "article", "aside",
				"blockquote",
				"caption", "col", "colgroup", "command",
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
				"ul"
			},

			// Deprecated
			new []
			{
				"center", "dir", "frame", "frameset", "isindex", "marquee", "noframes"
			}
		);

		#endregion

		#region Inline tags

		/// <summary>
		/// List of inline tags
		/// </summary>
		private static readonly HashSet<string> _inlineTags = Utils.UnionHashSets(
			// HTML5
			new []
			{
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
				"progress",
				"q",
				"rb", "rp", "rt", "rtc", "ruby",
				"samp", "select", "small", "span", "strong", "sub", "sup",
				"textarea", "time",
				"var",
				"wbr"
			},

			// Deprecated
			new []
			{
				"acronym", "big", "blink", "font", "nobr", "s", "strike", "tt", "u"
			}
		);

		#endregion

		#region Inline-block tags

		/// <summary>
		/// List of inline-block tags
		/// </summary>
		private static readonly HashSet<string> _inlineBlockTags = Utils.UnionHashSets(
			// HTML5
			new []
			{
				"area", "audio",
				"button",
				"canvas",
				"del",
				"embed",
				"iframe", "ins",
				"map", "math",
				"object",
				"script", "svg",
				"video"
			},

			// Deprecated
			new []
			{
				"applet"
			}
		);

		#endregion

		#region Non-independent tags

		/// <summary>
		/// List of non-independent tags
		/// </summary>
		private static readonly HashSet<string> _nonIndependentTags = Utils.UnionHashSets(
			// HTML5
			new []
			{
				"area",
				"caption", "col", "colgroup",
				"dd", "dt",
				"figcaption", "frame",
				"legend", "li",
				"menuitem",
				"optgroup", "option",
				"param",
				"rb", "rp", "rt", "rtc",
				"source",
				"tbody", "td", "tfoot", "th", "thead", "tr", "track"
			},

			// Deprecated
			new []
			{
				"command"
			}
		);

		#endregion

		#region Optional end tags

		/// <summary>
		/// List of tags, that can be omitted
		/// </summary>
		private static readonly HashSet<string> _optionalTags = new HashSet<string>
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
		private static readonly HashSet<string> _tagsWithEmbeddedCode = new HashSet<string>
		{
			"script", "style"
		};

		#endregion

		#region XML-based tags

		/// <summary>
		/// List of the XML-based tags
		/// </summary>
		private static readonly HashSet<string> _xmlBasedTags = new HashSet<string>
		{
			"svg", "math"
		};

		#endregion


		/// <summary>
		/// Checks whether the tag is invisible
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Result of check (true - invisible; false - not invisible)</returns>
		public static bool IsInvisibleTag(string tagNameInLowercase)
		{
			return _invisibleTags.Contains(tagNameInLowercase);
		}

		/// <summary>
		/// Checks whether the tag is empty
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Result of check (true - empty; false - not empty)</returns>
		public static bool IsEmptyTag(string tagNameInLowercase)
		{
			return _emptyTags.Contains(tagNameInLowercase);
		}

		/// <summary>
		/// Checks whether the tag is block
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Result of check (true - block; false - not block)</returns>
		public static bool IsBlockTag(string tagNameInLowercase)
		{
			return _blockTags.Contains(tagNameInLowercase);
		}

		/// <summary>
		/// Checks whether the tag is inline
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Result of check (true - inline; false - not inline)</returns>
		public static bool IsInlineTag(string tagNameInLowercase)
		{
			return _inlineTags.Contains(tagNameInLowercase);
		}

		/// <summary>
		/// Checks whether the tag is inline-block
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Result of check (true - inline-block; false - not inline-block)</returns>
		public static bool IsInlineBlockTag(string tagNameInLowercase)
		{
			return _inlineBlockTags.Contains(tagNameInLowercase);
		}

		/// <summary>
		/// Checks whether the tag is non-independent
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Result of check (true - non-independent; false - independent)</returns>
		public static bool IsNonIndependentTag(string tagNameInLowercase)
		{
			return _nonIndependentTags.Contains(tagNameInLowercase);
		}

		/// <summary>
		/// Checks whether the tag, that can be omitted
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Result of check (true - tag is optional; false - tag is required)</returns>
		public static bool IsOptionalTag(string tagNameInLowercase)
		{
			return _optionalTags.Contains(tagNameInLowercase);
		}

		/// <summary>
		/// Checks whether the tag can contain embedded code
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Result of check (true - can contain embedded code; false - cannot contain embedded code)</returns>
		public static bool IsTagWithEmbeddedCode(string tagNameInLowercase)
		{
			return _tagsWithEmbeddedCode.Contains(tagNameInLowercase);
		}

		/// <summary>
		/// Checks whether the tag is XML-based
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <returns>Result of check (true - is XML-based; false - is not XML-based)</returns>
		public static bool IsXmlBasedTag(string tagNameInLowercase)
		{
			return _xmlBasedTags.Contains(tagNameInLowercase);
		}
	}
}