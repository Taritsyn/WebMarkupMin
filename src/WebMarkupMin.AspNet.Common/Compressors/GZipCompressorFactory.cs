namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// GZip compressor factory
	/// </summary>
	public sealed class GZipCompressorFactory : ICompressorFactory
	{
#if NETSTANDARD1_3 || NET45
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
			get { return EncodingTokenConstants.GZip; }
		}

#if NETSTANDARD1_3 || NET45

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
#if NETSTANDARD1_3 || NET45
			return new GZipCompressor(CompressionSettings);
#else
			return new GZipCompressor();
#endif
		}
	}
}