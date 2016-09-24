using System;
using System.Collections.Generic;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet.Common.Compressors;

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


		/// <summary>
		/// Constructs a instance of HTTP compression manager
		/// </summary>
		public HttpCompressionManager()
			: this(new List<ICompressorFactory>
			{
				new DeflateCompressorFactory(),
				new GZipCompressorFactory()
			})
		{ }

		/// <summary>
		/// Constructs a instance of HTTP compression manager
		/// </summary>
		/// <param name="compressorFactories">List of HTTP compressor factories</param>
		public HttpCompressionManager(IList<ICompressorFactory> compressorFactories)
		{
			CompressorFactories = compressorFactories;
		}
	}
}