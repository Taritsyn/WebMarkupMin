using System;

using WebMarkupMin.Core;

namespace WebMarkupMin.AspNet4.Common
{
	/// <summary>
	/// Default CSS minifier factory
	/// </summary>
	public class DefaultCssMinifierFactory
	{
		/// <summary>
		/// Default instance of CSS minifier factory
		/// </summary>
		private static readonly Lazy<KristensenCssMinifierFactory> _default =
			new Lazy<KristensenCssMinifierFactory>(() => new KristensenCssMinifierFactory());

		/// <summary>
		/// Current instance of CSS minifier factory
		/// </summary>
		private static ICssMinifierFactory _current;

		/// <summary>
		/// Gets or sets a instance of CSS minifier factory
		/// </summary>
		public static ICssMinifierFactory Current
		{
			get
			{
				return _current ?? _default.Value;
			}
			set
			{
				_current = value;
			}
		}
	}
}