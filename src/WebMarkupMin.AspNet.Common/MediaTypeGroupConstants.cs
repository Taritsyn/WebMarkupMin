using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Media-type groups constants
	/// </summary>
	public static class MediaTypeGroupConstants
	{
		public static readonly ReadOnlyCollection<string> Html = new List<string>
			{
				MediaTypeConstants.TextHtml
			}
			.AsReadOnly()
			;

		public static readonly ReadOnlyCollection<string> Xhtml = new List<string>
			{
				MediaTypeConstants.TextHtml,
				MediaTypeConstants.ApplicationXhtml
			}
			.AsReadOnly()
			;

		public static readonly ReadOnlyCollection<string> Xml = new List<string>
			{
				MediaTypeConstants.ApplicationXml,
				MediaTypeConstants.TextXml,
				MediaTypeConstants.ApplicationXmlDtd,
				MediaTypeConstants.ApplicationXslt,
				MediaTypeConstants.ApplicationRss,
				MediaTypeConstants.ApplicationAtom,
				MediaTypeConstants.ApplicationSoap,
				MediaTypeConstants.ApplicationWsdl,
				MediaTypeConstants.ImageSvg,
				MediaTypeConstants.ApplicationMathMl,
				MediaTypeConstants.ApplicationVoiceXml,
				MediaTypeConstants.ApplicationSrgs
			}
			.AsReadOnly()
			;
	}
}