using System.Text.RegularExpressions;

using Microsoft.AspNet.Mvc.Rendering;

namespace WebMarkupMin.Sample.AspNet5Core.Mvc6.Infrastructure.Helpers
{
	public static class CommonExtensions
	{
		public static HtmlString EncodedReplace(this IHtmlHelper htmlHelper, string input,
			string pattern, string replacement)
		{
			return new HtmlString(Regex.Replace(htmlHelper.Encode(input), pattern, replacement));
		}
	}
}