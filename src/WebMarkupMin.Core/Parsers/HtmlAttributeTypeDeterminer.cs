using System;
using System.Collections.Generic;
using System.Linq;

using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// HTML attribute type determiner
	/// </summary>
	internal class HtmlAttributeTypeDeterminer
	{
		/// <summary>
		/// Instance of HTML attribute type determiner
		/// </summary>
		private static readonly Lazy<HtmlAttributeTypeDeterminer> _lazyInstance =
			new Lazy<HtmlAttributeTypeDeterminer>(() => new HtmlAttributeTypeDeterminer());

		#region Boolean attributes

		/// <summary>
		/// List of boolean attributes
		/// </summary>
		private readonly HashSet<string> _booleanAttributes = new HashSet<string>
		{
			// HTML5
			"allowfullscreen", "async", "autofocus", "autoplay",
			"checked", "controls",
			"default", "defer", "disabled",
			"formnovalidate",
			"hidden",
			"ismap", "itemscope",
			"loop",
			"multiple", "muted",
			"nomodule", "novalidate",
			"open",
			"readonly", "required", "reversed",
			"scoped", "selected",
			"typemustmatch",

			// Deprecated
			"compact",
			"declare",
			"inert",
			"nohref", "noresize", "noshade", "nowrap",
			"pubdate",
			"seamless", "sortable",
			"truespeed"
		};

		#endregion

		#region Event attributes

		/// <summary>
		/// List of event attributes
		/// </summary>
		private readonly HashSet<string> _eventAttributes = new HashSet<string>
		{
			// HTML5
			"onabort", "onafterprint", "onauxclick",
			"onbeforematch", "onbeforeprint", "onbeforeunload", "onblur",
			"oncancel", "oncanplay", "oncanplaythrough", "onchange", "onclick", "onclose",
			"oncontextlost", "oncontextmenu", "oncontextrestored", "oncopy", "oncuechange", "oncut",
			"ondblclick", "ondrag", "ondragend", "ondragenter", "ondragleave", "ondragover",
			"ondragstart", "ondrop", "ondurationchange",
			"onemptied", "onended", "onerror",
			"onfocus", "onformdata",
			"onhashchange",
			"oninput", "oninvalid",
			"onkeydown", "onkeypress", "onkeyup",
			"onlanguagechange", "onload", "onloadeddata", "onloadedmetadata", "onloadstart",
			"onmessage", "onmessageerror", "onmousedown", "onmousemove", "onmouseout", "onmouseover", "onmouseup",
			"onoffline", "ononline",
			"onpagehide", "onpageshow", "onpaste", "onpause", "onplay", "onplaying", "onpopstate", "onprogress",
			"onratechange", "onrejectionhandled", "onreset", "onresize",
			"onscroll", "onsecuritypolicyviolation", "onseeked", "onseeking", "onselect", "onslotchange",
			"onstalled", "onstorage", "onsubmit", "onsuspend",
			"ontimeupdate", "ontoggle",
			"onunhandledrejection", "onunload",
			"onvolumechange",
			"onwaiting",
			"onwebkitanimationend", "onwebkitanimationiteration", "onwebkitanimationstart", "onwebkittransitionend",
			"onwheel",

			// Obsolete
			"onmouseenter", "onmouseleave",

			// Deprecated
			"onmousewheel",

			// Nonstandard
			"onsearch"
		};

		#endregion

		#region Tags with URI based attributes

		/// <summary>
		/// List of tags with href attribute
		/// </summary>
		private readonly HashSet<string> _tagsWithHrefAttribute = new HashSet<string>
		{
			"a", "area", "base", "link"
		};

		/// <summary>
		/// List of tags with src attribute
		/// </summary>
		private readonly HashSet<string> _tagsWithSrcAttribute = new HashSet<string>
		{
			"audio", "embed", "frame", "iframe", "img", "input", "script", "source", "track", "video"
		};

		/// <summary>
		/// List of tags with cite attribute
		/// </summary>
		private readonly HashSet<string> _tagsWithCiteAttribute = new HashSet<string>
		{
			"blockquote", "del", "ins", "q"
		};

		/// <summary>
		/// List of tags with longdesc attribute
		/// </summary>
		private readonly HashSet<string> _tagsWithLongdescAttribute = new HashSet<string>
		{
			"frame", "iframe", "img"
		};

		/// <summary>
		/// URI based params
		/// </summary>
		private readonly HashSet<string> _uriBasedParams = new HashSet<string>
		{
			"movie", "pluginspage"
		};

		#endregion

		#region Tags with numeric attributes

		/// <summary>
		/// List of tags with width attribute
		/// </summary>
		private readonly HashSet<string> _tagsWithWidthAttribute = new HashSet<string>
		{
			"applet",
			"canvas", "col", "colgroup",
			"embed",
			"hr",
			"iframe", "img", "input",
			"object",
			"pre",
			"table", "td", "th",
			"video"
		};

		/// <summary>
		/// List of tags with height attribute
		/// </summary>
		private readonly HashSet<string> _tagsWithHeightAttribute = new HashSet<string>
		{
			"applet", "canvas", "embed", "iframe", "img", "input", "object", "td", "th", "video"
		};

		/// <summary>
		/// List of tags with border attribute
		/// </summary>
		private readonly HashSet<string> _tagsWithBorderAttribute = new HashSet<string>
		{
			"img", "object", "table"
		};

		/// <summary>
		/// List of tags with size attribute
		/// </summary>
		private readonly HashSet<string> _tagsWithSizeAttribute = new HashSet<string>
		{
			"basefont", "font", "hr", "input", "select"
		};

		/// <summary>
		/// List of tags with max attribute
		/// </summary>
		private readonly HashSet<string> _tagsWithMaxAttribute = new HashSet<string>
		{
			"input", "meter", "progress"
		};

		/// <summary>
		/// List of tags with min attribute
		/// </summary>
		private readonly HashSet<string> _tagsWithMinAttribute = new HashSet<string>
		{
			"input", "meter"
		};

		/// <summary>
		/// List of tags with value attribute
		/// </summary>
		private readonly HashSet<string> _tagsWithValueAttribute = new HashSet<string>
		{
			"li", "meter", "progress"
		};

		/// <summary>
		/// List of tags with charoff attribute
		/// </summary>
		private readonly HashSet<string> _tagsWithCharoffAttribute = new HashSet<string>
		{
			"col", "colgroup", "tbody", "td", "tfoot", "th", "thead", "tr"
		};

		#endregion

		/// <summary>
		/// Gets a instance of HTML attribute type determiner
		/// </summary>
		public static HtmlAttributeTypeDeterminer Instance
		{
			get { return _lazyInstance.Value; }
		}


		/// <summary>
		/// Private constructor
		/// </summary>
		private HtmlAttributeTypeDeterminer()
		{ }


		/// <summary>
		/// Gets a HTML attribute type
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <param name="tagFlags">Tag flags</param>
		/// <param name="attributeNameInLowercase">Attribute name in lowercase</param>
		/// <param name="attributes">List of attributes</param>
		/// <returns>Attribute type</returns>
		public HtmlAttributeType GetAttributeType(string tagNameInLowercase, HtmlTagFlags tagFlags,
			string attributeNameInLowercase, List<HtmlAttribute> attributes)
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
			else if (IsEventAttribute(attributeNameInLowercase))
			{
				attributeType = HtmlAttributeType.Event;
			}

			if (attributeType == HtmlAttributeType.Unknown && !tagFlags.IsSet(HtmlTagFlags.Xml))
			{
				if (IsBooleanAttribute(attributeNameInLowercase))
				{
					attributeType = HtmlAttributeType.Boolean;
				}
				else if (IsNumericAttribute(tagNameInLowercase, attributeNameInLowercase))
				{
					attributeType = HtmlAttributeType.Numeric;
				}
				else if (IsUriBasedAttribute(tagNameInLowercase, attributeNameInLowercase,
					attributes))
				{
					attributeType = HtmlAttributeType.Uri;
				}
			}

			if (attributeType == HtmlAttributeType.Unknown)
			{
				attributeType = IsXmlBasedAttribute(attributeNameInLowercase) ?
					HtmlAttributeType.Xml : HtmlAttributeType.Text;
			}

			return attributeType;
		}

		/// <summary>
		/// Checks whether the attribute is boolean
		/// </summary>
		/// <param name="attributeNameInLowercase">Attribute name in lowercase</param>
		/// <returns>Result of check (<c>true</c> - boolean; <c>false</c> - not boolean)</returns>
		private bool IsBooleanAttribute(string attributeNameInLowercase)
		{
			return _booleanAttributes.Contains(attributeNameInLowercase);
		}

		/// <summary>
		/// Checks whether the attribute is numeric
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <param name="attributeNameInLowercase">Attribute name in lowercase</param>
		/// <returns>Result of check (<c>true</c> - numeric; <c>false</c> - not numeric)</returns>
		private bool IsNumericAttribute(string tagNameInLowercase, string attributeNameInLowercase)
		{
			bool isNumeric;

			switch (attributeNameInLowercase)
			{
				case "tabindex":
					isNumeric = true;
					break;
				case "width":
					isNumeric = _tagsWithWidthAttribute.Contains(tagNameInLowercase);
					break;
				case "height":
					isNumeric = _tagsWithHeightAttribute.Contains(tagNameInLowercase);
					break;
				case "colspan":
				case "rowspan":
					isNumeric = tagNameInLowercase == "td" || tagNameInLowercase == "th";
					break;
				case "maxlength":
					isNumeric = tagNameInLowercase == "input" || tagNameInLowercase == "textarea";
					break;
				case "size":
					isNumeric = _tagsWithSizeAttribute.Contains(tagNameInLowercase);
					break;
				case "cols":
				case "rows":
					isNumeric = tagNameInLowercase == "textarea" || tagNameInLowercase == "frameset";
					break;
				case "max":
					isNumeric = _tagsWithMaxAttribute.Contains(tagNameInLowercase);
					break;
				case "min":
					isNumeric = _tagsWithMinAttribute.Contains(tagNameInLowercase);
					break;
				case "step":
					isNumeric = tagNameInLowercase == "input";
					break;
				case "value":
					isNumeric = _tagsWithValueAttribute.Contains(tagNameInLowercase);
					break;
				case "high":
				case "low":
				case "optimum":
					isNumeric = tagNameInLowercase == "meter";
					break;
				case "start":
					isNumeric = tagNameInLowercase == "ol";
					break;
				case "span":
					isNumeric = tagNameInLowercase == "colgroup" || tagNameInLowercase == "col";
					break;
				case "border":
					isNumeric = _tagsWithBorderAttribute.Contains(tagNameInLowercase);
					break;
				case "cellpadding":
				case "cellspacing":
					isNumeric = tagNameInLowercase == "table";
					break;
				case "charoff":
					isNumeric = _tagsWithCharoffAttribute.Contains(tagNameInLowercase);
					break;
				case "hspace":
				case "vspace":
					isNumeric = tagNameInLowercase == "img" || tagNameInLowercase == "object"
						|| tagNameInLowercase == "applet";
					break;
				case "frameborder":
				case "marginwidth":
				case "marginheight":
					isNumeric = tagNameInLowercase == "iframe" || tagNameInLowercase == "frame";
					break;
				default:
					isNumeric = false;
					break;
			}

			return isNumeric;
		}

		/// <summary>
		/// Checks whether the attribute is URI-based
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <param name="attributeNameInLowercase">Attribute name in lowercase</param>
		/// <param name="attributes">List of attributes</param>
		/// <returns>Result of check (<c>true</c> - URI-based; <c>false</c> - not URI-based)</returns>
		private bool IsUriBasedAttribute(string tagNameInLowercase, string attributeNameInLowercase,
			List<HtmlAttribute> attributes)
		{
			bool isUriBased;

			switch (attributeNameInLowercase)
			{
				case "href":
					isUriBased = _tagsWithHrefAttribute.Contains(tagNameInLowercase);
					break;
				case "src":
					isUriBased = _tagsWithSrcAttribute.Contains(tagNameInLowercase);
					break;
				case "action":
					isUriBased = tagNameInLowercase == "form";
					break;
				case "cite":
					isUriBased = _tagsWithCiteAttribute.Contains(tagNameInLowercase);
					break;
				case "manifest":
					isUriBased = tagNameInLowercase == "html";
					break;
				case "longdesc":
					isUriBased = _tagsWithLongdescAttribute.Contains(tagNameInLowercase);
					break;
				case "formaction":
					isUriBased = tagNameInLowercase == "button" || tagNameInLowercase == "input";
					break;
				case "poster":
					isUriBased = tagNameInLowercase == "video";
					break;
				case "icon":
					isUriBased = tagNameInLowercase == "command";
					break;
				case "data":
					isUriBased = tagNameInLowercase == "object";
					break;
				case "codebase":
				case "archive":
					isUriBased = tagNameInLowercase == "object" || tagNameInLowercase == "applet";
					break;
				case "code":
					isUriBased = tagNameInLowercase == "applet";
					break;
				case "profile":
					isUriBased = tagNameInLowercase == "head";
					break;
				case "value":
					isUriBased = tagNameInLowercase == "param" && attributes.Any(a => a.NameInLowercase == "name"
						&& _uriBasedParams.Contains(a.Value.Trim().ToLowerInvariant()));
					break;
				default:
					isUriBased = false;
					break;
			}

			return isUriBased;
		}

		/// <summary>
		/// Checks whether the attribute is event
		/// </summary>
		/// <param name="attributeNameInLowercase">Attribute name in lowercase</param>
		/// <returns>Result of check (<c>true</c> - event; <c>false</c> - not event)</returns>
		private bool IsEventAttribute(string attributeNameInLowercase)
		{
			bool isEventAttribute = false;
			int charCount = attributeNameInLowercase.Length;

			if (charCount >= 5 && attributeNameInLowercase.StartsWith("on", StringComparison.Ordinal))
			{
				isEventAttribute = true;

				for (int charIndex = 2; charIndex < charCount; charIndex++)
				{
					char charValue = attributeNameInLowercase[charIndex];

					if (!charValue.IsAlphaLower())
					{
						isEventAttribute = false;
						break;
					}
				}
			}

			if (isEventAttribute)
			{
				isEventAttribute = _eventAttributes.Contains(attributeNameInLowercase);
			}

			return isEventAttribute;
		}

		/// <summary>
		/// Checks whether the attribute is XML-based
		/// </summary>
		/// <param name="attributeNameInLowercase">Attribute name in lowercase</param>
		/// <returns>Result of check (<c>true</c> - XML-based; <c>false</c> - not XML-based)</returns>
		private bool IsXmlBasedAttribute(string attributeNameInLowercase)
		{
			bool isXmlAttribute = false;

			if (attributeNameInLowercase.Length >= 3)
			{
				isXmlAttribute = attributeNameInLowercase == "xmlns";

				if (!isXmlAttribute && attributeNameInLowercase.StartsWith("xml", StringComparison.Ordinal))
				{
					isXmlAttribute = attributeNameInLowercase.CustomStartsWith(":", 3, StringComparison.Ordinal)
						|| attributeNameInLowercase.CustomStartsWith("ns:", 3, StringComparison.Ordinal);
				}
			}

			return isXmlAttribute;
		}
	}
}