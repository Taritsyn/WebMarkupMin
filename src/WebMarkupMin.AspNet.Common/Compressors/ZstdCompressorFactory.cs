#if NET11_0_OR_GREATER
namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Zstd (Zstandard) compressor factory
	/// </summary>
	public sealed class ZstdCompressorFactory : ICompressorFactory
	{
		/// <summary>
		/// Gets or sets a compression settings used to configure the Zstd compressor
		/// </summary>
		public ZstdCompressionSettings CompressionSettings
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a encoding token
		/// </summary>
		public string EncodingToken
		{
			get { return ZstdCompressor.CompressorEncodingToken; }
		}


		/// <summary>
		/// Constructs an instance of the Zstd compressor factory
		/// </summary>
		public ZstdCompressorFactory()
			: this(new ZstdCompressionSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the Zstd compressor factory
		/// </summary>
		/// <param name="settings">Compression settings used to configure the Zstd compressor</param>
		public ZstdCompressorFactory(ZstdCompressionSettings settings)
		{
			CompressionSettings = settings;
		}


		/// <summary>
		/// Creates a instance of the Zstd compressor
		/// </summary>
		/// <returns>Instance of the Zstd compressor</returns>
		public ICompressor CreateCompressor()
		{
			return new ZstdCompressor(CompressionSettings);
		}
	}
}
#endif