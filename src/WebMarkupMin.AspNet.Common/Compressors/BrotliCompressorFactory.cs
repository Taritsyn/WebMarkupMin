#if NETSTANDARD2_1 || NET9_0_OR_GREATER
namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Brotli compressor factory
	/// </summary>
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
		}


		/// <summary>
		/// Creates a instance of the Brotli compressor
		/// </summary>
		/// <returns>Instance of the Brotli compressor</returns>
		public ICompressor CreateCompressor()
		{
			return new BrotliCompressor(CompressionSettings);
		}
	}
}
#endif