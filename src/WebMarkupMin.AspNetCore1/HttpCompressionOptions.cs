using System;
using System.Collections.Generic;

using WebMarkupMin.AspNet.Common.Compressors;

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2
#else
#error No implementation for this target
#endif
{
	/// <summary>
	/// HTTP compression options
	/// </summary>
	public sealed class HttpCompressionOptions : ContentProcessingOptionsBase
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
			SupportedMediaTypePredicate = null;
		}
	}
}