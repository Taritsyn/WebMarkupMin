namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// GZip compressor factory
	/// </summary>
	public sealed class GZipCompressorFactory : ICompressorFactory
	{
#if COMPRESSION_SETTINGS
		/// <summary>
		/// Gets or sets a compression settings used to configure the GZip compressor
		/// </summary>
		public GZipCompressionSettings CompressionSettings
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
			get { return GZipCompressor.CompressorEncodingToken; }
		}


		/// <summary>
		/// Constructs an instance of the GZip compressor factory
		/// </summary>
		public GZipCompressorFactory()
#if COMPRESSION_SETTINGS
			: this(new GZipCompressionSettings())
#endif
		{ }
#if COMPRESSION_SETTINGS

		/// <summary>
		/// Constructs an instance of the GZip compressor factory
		/// </summary>
		/// <param name="settings">Compression settings used to configure the GZip compressor</param>
		public GZipCompressorFactory(GZipCompressionSettings settings)
		{
			CompressionSettings = settings;
		}
#endif


		/// <summary>
		/// Creates an instance of the GZip compressor
		/// </summary>
		/// <returns>Instance of the GZip compressor</returns>
		public ICompressor CreateCompressor()
		{
#if COMPRESSION_SETTINGS
			return new GZipCompressor(CompressionSettings);
#else
			return new GZipCompressor();
#endif
		}
	}
}