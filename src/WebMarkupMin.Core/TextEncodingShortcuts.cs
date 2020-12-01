using System;
using System.Text;

using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// Text encoding shortcuts
	/// </summary>
	public static class TextEncodingShortcuts
	{
		/// <summary>
		/// Gets a default encoding for current operating system
		/// </summary>
		[Obsolete("Use a `WebMarkupMin.Core.Utilities.TargetFrameworkShortcuts.DefaultTextEncoding` property instead")]
		public static readonly Encoding Default = TargetFrameworkShortcuts.DefaultTextEncoding;
	}
}