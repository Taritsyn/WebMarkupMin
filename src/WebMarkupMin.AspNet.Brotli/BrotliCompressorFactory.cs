using System;

#if NETSTANDARD2_1_OR_GREATER
using WebMarkupMin.AspNet.Brotli.Resources;
#endif
using WebMarkupMin.AspNet.Common.Compressors;

namespace WebMarkupMin.AspNet.Brotli
{
	/// <summary>
	/// Brotli compressor factory
	/// </summary>
	[Obsolete("Use a `LegacyBrotliCompressorFactory` class instead")]
	public sealed class BrotliCompressorFactory : ICompressorFactory
	{
		/// <summary>
		/// Gets or sets a compression settings used to configure the Brotli compressor
		/// </summary>
		public BrotliCompressionSettings CompressionSettings
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a encoding token
		/// </summary>
		public string EncodingToken
		{
			get { return BrotliCompressor.CompressorEncodingToken; }
		}


		/// <summary>
		/// Constructs an instance of the Brotli compressor factory
		/// </summary>
		public BrotliCompressorFactory()
			: this(new BrotliCompressionSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the Brotli compressor factory
		/// </summary>
		/// <param name="settings">Compression settings used to configure the Brotli compressor</param>
		public BrotliCompressorFactory(BrotliCompressionSettings settings)
		{
			CompressionSettings = settings;
#if NETSTANDARD2_1_OR_GREATER

			throw new NotSupportedException(
				string.Format(Strings.CompressorFactoryNotSupportNetCore3, nameof(BrotliCompressorFactory)));
#endif
		}


		/// <summary>
		/// Creates a instance of Brotli compressor
		/// </summary>
		/// <returns>Instance of Brotli compressor</returns>
		public ICompressor CreateCompressor()
		{
			return new BrotliCompressor(CompressionSettings);
		}
	}
}