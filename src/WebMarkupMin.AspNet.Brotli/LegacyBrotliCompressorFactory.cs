#if NETSTANDARD2_1_OR_GREATER
using System;

#endif
#if NETSTANDARD2_1_OR_GREATER
using WebMarkupMin.AspNet.Brotli.Resources;
#endif
using WebMarkupMin.AspNet.Common.Compressors;

namespace WebMarkupMin.AspNet.Brotli
{
	/// <summary>
	/// Legacy Brotli compressor factory
	/// </summary>
	public sealed class LegacyBrotliCompressorFactory : ICompressorFactory
	{
		/// <summary>
		/// Gets or sets a compression settings used to configure the legacy Brotli compressor
		/// </summary>
		public LegacyBrotliCompressionSettings CompressionSettings
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a encoding token
		/// </summary>
		public string EncodingToken
		{
			get { return LegacyBrotliCompressor.CompressorEncodingToken; }
		}


		/// <summary>
		/// Constructs an instance of the legacy Brotli compressor factory
		/// </summary>
		public LegacyBrotliCompressorFactory()
			: this(new LegacyBrotliCompressionSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the legacy Brotli compressor factory
		/// </summary>
		/// <param name="settings">Compression settings used to configure the Brotli compressor</param>
		public LegacyBrotliCompressorFactory(LegacyBrotliCompressionSettings settings)
		{
			CompressionSettings = settings;
#if NETSTANDARD2_1_OR_GREATER

			throw new NotSupportedException(
				string.Format(Strings.CompressorFactoryNotSupportNetCore3, nameof(LegacyBrotliCompressorFactory)));
#endif
		}


		/// <summary>
		/// Creates a instance of the legacy Brotli compressor
		/// </summary>
		/// <returns>Instance of the legacy Brotli compressor</returns>
		public ICompressor CreateCompressor()
		{
			return new LegacyBrotliCompressor(CompressionSettings);
		}
	}
}