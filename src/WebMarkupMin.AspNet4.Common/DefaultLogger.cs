using System;

using WebMarkupMin.Core.Loggers;

namespace WebMarkupMin.AspNet4.Common
{
	/// <summary>
	/// Default logger
	/// </summary>
	public class DefaultLogger
	{
		/// <summary>
		/// Default instance of logger
		/// </summary>
		private static readonly Lazy<NullLogger> _default
			= new Lazy<NullLogger>(() => new NullLogger());

		/// <summary>
		/// Current instance of logger
		/// </summary>
		private static ILogger _current;

		/// <summary>
		/// Gets or sets a instance of logger
		/// </summary>
		public static ILogger Current
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