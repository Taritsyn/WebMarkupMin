using System.Text.RegularExpressions;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// Common regular expressions
	/// </summary>
	internal static class CommonRegExps
	{
		public static readonly Regex Doctype = new Regex(@"^<!DOCTYPE [^>]+?>", RegexOptions.IgnoreCase);
	}
}