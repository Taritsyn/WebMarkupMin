using System;
using System.Collections.Generic;

using WebMarkupMin.AspNet.Common.Compressors;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Defines a interface of HTTP compression manager
	/// </summary>
	public interface IHttpCompressionManager : IContentProcessingManager
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
		[Obsolete("Use a `TryCreateCompressor` method")]
		ICompressor CreateCompressor(string acceptEncoding);

		/// <summary>
		/// Tries to create a instance of compressor.
		/// A return value indicates whether the creation succeeded.
		/// </summary>
		/// <param name="acceptEncoding">Value of the Accept-Encoding HTTP header</param>
		/// <param name="compressor">Instance of compressor</param>
		/// <returns>true if the compressor was created; otherwise, false</returns>
		bool TryCreateCompressor(string acceptEncoding, out ICompressor compressor);
	}
}