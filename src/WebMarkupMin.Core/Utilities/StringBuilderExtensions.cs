using System.Text;
using System.Text.RegularExpressions;

namespace WebMarkupMin.Core.Utilities
{
	/// <summary>
	/// Extensions for StringBuilder
	/// </summary>
	public static class StringBuilderExtensions
	{
		/// <summary>
		/// Regular expression for format placeholder
		/// </summary>
		private static readonly Regex _formatPlaceholderRegExp =
			new Regex(@"\{[0-9]\}", RegexOptions.Multiline);

		/// <summary>
		/// Appends the default line terminator to the end of the current System.Text.StringBuilder object
		/// </summary>
		/// <param name="sb">Object StringBuilder</param>
		/// <returns>Object StringBuilder</returns>
		public static StringBuilder AppendFormatLine(this StringBuilder sb)
		{
			return sb.AppendLine();
		}

		/// <summary>
		/// Appends the string returned by processing a composite format string, which
		/// contains zero or more format items, with default line terminator to this instance.
		/// Each format item is replaced by the string representation of a corresponding
		/// argument in a parameter array.
		/// </summary>
		/// <param name="sb">Object StringBuilder</param>
		/// <param name="format">A composite format string</param>
		/// <param name="args">An array of objects to format</param>
		/// <returns>Object StringBuilder</returns>
		public static StringBuilder AppendFormatLine(this StringBuilder sb, string format, params object[] args)
		{
			if (_formatPlaceholderRegExp.IsMatch(format))
			{
				return sb.AppendFormat(format, args).AppendLine();
			}

			return sb.AppendLine(format.Replace("{{", "{").Replace("}}", "}"));
		}
	}
}