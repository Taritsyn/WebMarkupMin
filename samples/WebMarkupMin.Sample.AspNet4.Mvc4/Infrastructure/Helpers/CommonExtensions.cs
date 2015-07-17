using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace WebMarkupMin.Sample.AspNet4.Mvc4.Infrastructure.Helpers
{
	public static class CommonExtensions
	{
		private static readonly HelperConstants _constants = new HelperConstants();

		public static HelperConstants Constants(this HtmlHelper htmlHelper)
		{
			return _constants;
		}

		public static MvcHtmlString EncodedReplace(this HtmlHelper htmlHelper, string input,
			string pattern, string replacement)
		{
			return new MvcHtmlString(Regex.Replace(htmlHelper.Encode(input), pattern, replacement));
		}

		internal static string CreateSubIndexName(this HtmlHelper htmlHelper, string prefix, int index)
		{
			string name = prefix;
			if (index >= 0)
			{
				name = string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", prefix, index);
			}

			return name;
		}
	}
}