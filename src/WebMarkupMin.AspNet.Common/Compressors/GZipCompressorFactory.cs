namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// GZip compressor factory
	/// </summary>
	public sealed class GZipCompressorFactory : ICompressorFactory
	{
#if NET45 || NETSTANDARD || NET9_0_OR_GREATER
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

#if NET45 || NETSTANDARD || NET9_0_OR_GREATER

		/// <summary>
		/// Constructs an instance of the GZip compressor factory
		/// </summary>
		public GZipCompressorFactory()
			: this(new GZipCompressionSettings())
		{ }

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
		/// Creates a instance of GZip compressor
		/// </summary>
		/// <returns>Instance of GZip compressor</returns>
		public ICompressor CreateCompressor()
		{
#if NET45 || NETSTANDARD || NET9_0_OR_GREATER
			return new GZipCompressor(CompressionSettings);
#else
			return new GZipCompressor();
#endif
		}
	}
}