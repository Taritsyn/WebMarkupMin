namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Deflate compressor factory
	/// </summary>
	public sealed class DeflateCompressorFactory : ICompressorFactory
	{
#if COMPRESSION_SETTINGS
		/// <summary>
		/// Gets or sets a compression settings used to configure the Deflate compressor
		/// </summary>
		public DeflateCompressionSettings CompressionSettings
		{
			get;
			set;
		}

#endif
		/// <summary>
		/// Gets a encoding token
		/// </summary>
		public string EncodingToken
		{
			get { return DeflateCompressor.CompressorEncodingToken; }
		}


		/// <summary>
		/// Constructs an instance of the Deflate compressor factory
		/// </summary>
		public DeflateCompressorFactory()
#if COMPRESSION_SETTINGS
			: this(new DeflateCompressionSettings())
#endif
		{ }
#if COMPRESSION_SETTINGS

		/// <summary>
		/// Constructs an instance of the Deflate compressor factory
		/// </summary>
		/// <param name="settings">Compression settings used to configure the Deflate compressor</param>
		public DeflateCompressorFactory(DeflateCompressionSettings settings)
		{
			CompressionSettings = settings;
		}
#endif


		/// <summary>
		/// Creates an instance of the Deflate compressor
		/// </summary>
		/// <returns>Instance of the Deflate compressor</returns>
		public ICompressor CreateCompressor()
		{
#if COMPRESSION_SETTINGS
			return new DeflateCompressor(CompressionSettings);
#else
			return new DeflateCompressor();
#endif
		}
	}
}