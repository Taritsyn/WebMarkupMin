using System;
using System.Collections.Generic;

using WebMarkupMin.AspNet.Common.Compressors;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Defines a interface of HTTP compression manager
	/// </summary>
	public interface IHttpCompressionManager
	{
		/// <summary>
		/// Gets or sets a list of HTTP compressor factories
		/// </summary>
		IList<ICompressorFactory> CompressorFactories
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a delegate that determines whether the media-type is supported
		/// </summary>
		Func<string, bool> SupportedMediaTypePredicate
		{
			get;
			set;
		}


		/// <summary>
		/// Creates a instance of compressor
		/// </summary>
		/// <param name="acceptEncoding">Value of the Accept-Encoding HTTP header</param>
		/// <returns>Instance of compressor</returns>
		ICompressor CreateCompressor(string acceptEncoding);
	}
}