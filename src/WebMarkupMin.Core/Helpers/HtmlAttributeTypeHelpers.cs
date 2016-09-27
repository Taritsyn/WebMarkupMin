using System;
using System.Collections.Generic;
using System.Linq;

using WebMarkupMin.Core.Parsers;
using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Helpers
{
	/// <summary>
	/// HTML attribute type helpers
	/// </summary>
	internal static class HtmlAttributeTypeHelpers
	{
		#region Boolean attributes

		/// <summary>
		/// List of boolean attributes
		/// </summary>
		private static readonly HashSet<string> _booleanAttributes = new HashSet<string>
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
			"novalidate",
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

		#region Tags with URI based attributes

		/// <summary>
		/// List of tags with href attribute
		/// </summary>
		private static readonly HashSet<string> _tagsWithHrefAttribute = new HashSet<string>
		{
			"a", "area", "base", "link"
		};

		/// <summary>
		/// List of tags with src attribute
		/// </summary>
		private static readonly HashSet<string> _tagsWithSrcAttribute = new HashSet<string>
		{
			"audio", "embed", "frame", "iframe", "img", "input", "script", "source", "track", "video"
		};

		/// <summary>
		/// List of tags with cite attribute
		/// </summary>
		private static readonly HashSet<string> _tagsWithCiteAttribute = new HashSet<string>
		{
			"blockquote", "del", "ins", "q"
		};

		/// <summary>
		/// List of tags with longdesc attribute
		/// </summary>
		private static readonly HashSet<string> _tagsWithLongdescAttribute = new HashSet<string>
		{
			"frame", "iframe", "img"
		};

		/// <summary>
		/// URI based params
		/// </summary>
		private static readonly HashSet<string> _uriBasedParams = new HashSet<string>
		{
			"movie", "pluginspage"
		};

		#endregion

		#region Tags with numeric attributes

		/// <summary>
		/// List of tags with width attribute
		/// </summary>
		private static readonly HashSet<string> _tagsWithWidthAttribute = new HashSet<string>
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
		private static readonly HashSet<string> _tagsWithHeightAttribute = new HashSet<string>
		{
			"applet", "canvas", "embed", "iframe", "img", "input", "object", "td", "th", "video"
		};

		/// <summary>
		/// List of tags with border attribute
		/// </summary>
		private static readonly HashSet<string> _tagsWithBorderAttribute = new HashSet<string>
		{
			"img", "object", "table"
		};

		/// <summary>
		/// List of tags with size attribute
		/// </summary>
		private static readonly HashSet<string> _tagsWithSizeAttribute = new HashSet<string>
		{
			"basefont", "font", "hr", "input", "select"
		};

		/// <summary>
		/// List of tags with max attribute
		/// </summary>
		private static readonly HashSet<string> _tagsWithMaxAttribute = new HashSet<string>
		{
			"input", "meter", "progress"
		};

		/// <summary>
		/// List of tags with min attribute
		/// </summary>
		private static readonly HashSet<string> _tagsWithMinAttribute = new HashSet<string>
		{
			"input", "meter"
		};

		/// <summary>
		/// List of tags with value attribute
		/// </summary>
		private static readonly HashSet<string> _tagsWithValueAttribute = new HashSet<string>
		{
			"li", "meter", "progress"
		};

		/// <summary>
		/// List of tags with charoff attribute
		/// </summary>
		private static readonly HashSet<string> _tagsWithCharoffAttribute = new HashSet<string>
		{
			"col", "colgroup", "tbody", "td", "tfoot", "th", "thead", "tr"
		};

		#endregion


		/// <summary>
		/// Checks whether the attribute is boolean
		/// </summary>
		/// <param name="attributeNameInLowercase">Attribute name in lowercase</param>
		/// <returns>Result of check (true - boolean; false - not boolean)</returns>
		public static bool IsBooleanAttribute(string attributeNameInLowercase)
		{
			return _booleanAttributes.Contains(attributeNameInLowercase);
		}

		/// <summary>
		/// Checks whether the attribute is numeric
		/// </summary>
		/// <param name="tagNameInLowercase">Tag name in lowercase</param>
		/// <param name="attributeNameInLowercase">Attribute name in lowercase</param>
		/// <returns>Result of check (true - numeric; false - not numeric)</returns>
		public static bool IsNumericAttribute(string tagNameInLowercase, string attributeNameInLowercase)
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
		/// <returns>Result of check (true - URI-based; false - not URI-based)</returns>
		public static bool IsUriBasedAttribute(string tagNameInLowercase, string attributeNameInLowercase,
			IList<HtmlAttribute> attributes)
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
		/// <returns>Result of check (true - event; false - not event)</returns>
		public static bool IsEventAttribute(string attributeNameInLowercase)
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

			return isEventAttribute;
		}

		/// <summary>
		/// Checks whether the attribute is XML-based
		/// </summary>
		/// <param name="attributeNameInLowercase">Attribute name in lowercase</param>
		/// <returns>Result of check (true - XML-based; false - not XML-based)</returns>
		public static bool IsXmlBasedAttribute(string attributeNameInLowercase)
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