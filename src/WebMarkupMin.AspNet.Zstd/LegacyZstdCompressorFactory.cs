using WebMarkupMin.AspNet.Common.Compressors;

namespace WebMarkupMin.AspNet.Zstd
{
	/// <summary>
	/// Legacy Zstd (Zstandard) compressor factory
	/// </summary>
	public sealed class LegacyZstdCompressorFactory : ICompressorFactory
	{
		/// <summary>
		/// Gets or sets a compression settings used to configure the legacy Zstd compressor
		/// </summary>
		public LegacyZstdCompressionSettings CompressionSettings
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a encoding token
		/// </summary>
		public string EncodingToken
		{
			get { return LegacyZstdCompressor.CompressorEncodingToken; }
		}


		/// <summary>
		/// Constructs an instance of the legacy Zstd compressor factory
		/// </summary>
		public LegacyZstdCompressorFactory()
			: this(new LegacyZstdCompressionSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the legacy Zstd compressor factory
		/// </summary>
		/// <param name="settings">Compression settings used to configure the legacy Zstd compressor</param>
		public LegacyZstdCompressorFactory(LegacyZstdCompressionSettings settings)
		{
			CompressionSettings = settings;
		}


		/// <summary>
		/// Creates an instance of the legacy Zstd compressor
		/// </summary>
		/// <returns>Instance of the legacy Zstd compressor</returns>
		public ICompressor CreateCompressor()
		{
			return new LegacyZstdCompressor(CompressionSettings);
		}
	}
}