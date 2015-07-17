using System;

using WebMarkupMin.AspNet.Common;

namespace WebMarkupMin.AspNet4.Common
{
	/// <summary>
	/// HTTP compression manager
	/// </summary>
	public sealed class HttpCompressionManager : HttpCompressionManagerBase
	{
		/// <summary>
		/// Default instance of HTTP compression manager
		/// </summary>
		private static readonly Lazy<HttpCompressionManager> _default
			= new Lazy<HttpCompressionManager>(() => new HttpCompressionManager());

		/// <summary>
		/// Current instance of HTTP compression manager
		/// </summary>
		private static IHttpCompressionManager _current;

		/// <summary>
		/// Gets or sets a instance of HTTP compression manager
		/// </summary>
		public static IHttpCompressionManager Current
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