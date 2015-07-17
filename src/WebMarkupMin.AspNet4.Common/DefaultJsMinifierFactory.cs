using System;

using WebMarkupMin.Core;

namespace WebMarkupMin.AspNet4.Common
{
	/// <summary>
	/// Default JS minifier factory
	/// </summary>
	public class DefaultJsMinifierFactory
	{
		/// <summary>
		/// Default instance of JS minifier factory
		/// </summary>
		private static readonly Lazy<CrockfordJsMinifierFactory> _default
			= new Lazy<CrockfordJsMinifierFactory>(() => new CrockfordJsMinifierFactory());

		/// <summary>
		/// Current instance of JS minifier factory
		/// </summary>
		private static IJsMinifierFactory _current;

		/// <summary>
		/// Gets or sets a instance of JS minifier factory
		/// </summary>
		public static IJsMinifierFactory Current
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