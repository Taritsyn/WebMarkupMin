#if NETSTANDARD2_1 || NET9_0_OR_GREATER
using System;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Built-in Brotli compressor factory
	/// </summary>
	[Obsolete("Use a `WebMarkupMin.AspNet.Common.Compressors.BuiltInBrotliCompressorFactory` class instead")]
	public sealed class BuiltInBrotliCompressorFactory : ICompressorFactory
	{
		/// <summary>
		/// Gets or sets a compression settings used to configure the built-in Brotli compressor
		/// </summary>
		public BuiltInBrotliCompressionSettings CompressionSettings
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a encoding token
		/// </summary>
		public string EncodingToken
		{
			get { return BuiltInBrotliCompressor.CompressorEncodingToken; }
		}


		/// <summary>
		/// Constructs an instance of the built-in Brotli compressor factory
		/// </summary>
		public BuiltInBrotliCompressorFactory()
			: this(new BuiltInBrotliCompressionSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the built-in Brotli compressor factory
		/// </summary>
		/// <param name="settings">Compression settings used to configure the built-in Brotli compressor</param>
		public BuiltInBrotliCompressorFactory(BuiltInBrotliCompressionSettings settings)
		{
			CompressionSettings = settings;
		}


		/// <summary>
		/// Creates a instance of the built-in Brotli compressor
		/// </summary>
		/// <returns>Instance of the built-in Brotli compressor</returns>
		public ICompressor CreateCompressor()
		{
			return new BuiltInBrotliCompressor(CompressionSettings);
		}
	}
}
#endif