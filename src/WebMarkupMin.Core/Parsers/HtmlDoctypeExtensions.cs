using System;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// Extensions for HtmlDoctype
	/// </summary>
	internal static class HtmlDoctypeExtensions
	{
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

			if (source.RootElement != "html")
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
	}
}