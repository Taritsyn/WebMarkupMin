using System;
using System.Collections.Generic;

using WebMarkupMin.AspNet.Common.Compressors;

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2
#elif ASPNETCORE3
namespace WebMarkupMin.AspNetCore3
#elif ASPNETCORE5
namespace WebMarkupMin.AspNetCore5
#elif ASPNETCORE6
namespace WebMarkupMin.AspNetCore6
#elif ASPNETCORE7
namespace WebMarkupMin.AspNetCore7
#elif ASPNETCORE8
namespace WebMarkupMin.AspNetCore8
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
				new GZipCompressorFactory(),
				new DeflateCompressorFactory()
			};
			SupportedMediaTypePredicate = null;
		}
	}
}