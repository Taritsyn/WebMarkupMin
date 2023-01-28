using System;
using System.Collections.Generic;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// Extensions for HTML tag stack
	/// </summary>
	internal static class HtmlTagStackExtensions
	{
		internal static HtmlTag GetFirstTagByNameInLowercase(this Stack<HtmlTag> source, string nameInLowercase)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			HtmlTag tag = null;

			foreach (HtmlTag stackedTag in source)
			{
				if (stackedTag.NameInLowercase == nameInLowercase)
				{
					tag = stackedTag;
					break;
				}
			}

			return tag;
		}
	}
}