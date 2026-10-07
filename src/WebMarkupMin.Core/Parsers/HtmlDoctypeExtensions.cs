using System;

using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// Extensions for HtmlDoctype
	/// </summary>
	internal static class HtmlDoctypeExtensions
	{
		const string HTML_ROOT_ELEMENT = "html";
		const string XHTML_KEYWORD = "xhtml";


		/// <summary>
		/// Checks whether the HTML document type declaration is XHTML
		/// </summary>
		/// <param name="source">Instance of <see cref="HtmlDoctype"/></param>
		/// <returns>Result of check (<c>true</c> - is XHTML;
		/// <c>false</c> - is not XHTML)</returns>
		public static bool IsXhtml(this HtmlDoctype source)
		{
			if (source is null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (source.RootElement != HTML_ROOT_ELEMENT)
			{
				return false;
			}

			bool isXhtmlDoctype = false;

			switch (source.Publicity)
			{
				case HtmlPublicity.Public:
					HtmlFormalPublicId publicId = source.PublicId;
					isXhtmlDoctype = publicId is not null
						&& publicId.Name.StartsWith(XHTML_KEYWORD, StringComparison.OrdinalIgnoreCase)
						&& IsXhtmlSystemId(source.SystemId)
						;
					break;
				case HtmlPublicity.System:
					isXhtmlDoctype = IsXhtmlSystemId(source.SystemId);
					break;
			}

			return isXhtmlDoctype;
		}

		private static bool IsXhtmlSystemId(HtmlSystemId systemId)
		{
			if (systemId is not null)
			{
				return systemId.Url.IndexOf(XHTML_KEYWORD, StringComparison.OrdinalIgnoreCase) != -1;
			}

			return false;
		}

		/// <summary>
		/// Checks whether the HTML document type declaration is HTML5/XHTML5
		/// </summary>
		/// <param name="source">Instance of <see cref="HtmlDoctype"/></param>
		/// <returns>Result of check (<c>true</c> - is HTML5/XHTML5 document type;
		/// <c>false</c> - is not HTML5/XHTML5 document type)</returns>
		public static bool IsShort(this HtmlDoctype source)
		{
			if (source is null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (!source.RootElement.IgnoreCaseEquals(HTML_ROOT_ELEMENT))
			{
				return false;
			}

			string publicity = source.Publicity;
			if (string.IsNullOrWhiteSpace(publicity))
			{
				return true;
			}

			if (publicity.IgnoreCaseEquals(HtmlPublicity.System))
			{
				return source.SystemId?.Url == "about:legacy-compat";
			}

			return false;
		}
	}
}