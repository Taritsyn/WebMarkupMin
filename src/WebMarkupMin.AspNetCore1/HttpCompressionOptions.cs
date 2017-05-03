using System;
using System.Collections.Generic;

using WebMarkupMin.AspNet.Common.Compressors;

namespace WebMarkupMin.AspNetCore1
{
	/// <summary>
	/// HTTP compression options
	/// </summary>
	public sealed class HttpCompressionOptions
	{
		/// <summary>
		/// Gets or sets a list of HTTP compressor factories
		/// </summary>
		public IList<ICompressorFactory> CompressorFactories
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a list of supported HTTP methods
		/// </summary>
		public ISet<string> SupportedHttpMethods
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a delegate that determines whether the media-type is supported
		/// </summary>
		public Func<string, bool> SupportedMediaTypePredicate
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs a instance of HTTP compression options
		/// </summary>
		public HttpCompressionOptions()
		{
			CompressorFactories = new List<ICompressorFactory>
			{
				new DeflateCompressorFactory(),
				new GZipCompressorFactory()
			};
			SupportedHttpMethods = new HashSet<string> { "GET" };
			SupportedMediaTypePredicate = null;
		}
	}
}