namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Deflate compressor factory
	/// </summary>
	public sealed class DeflateCompressorFactory : ICompressorFactory
	{
#if NET45 || NETSTANDARD || NETCOREAPP
		/// <summary>
		/// Gets or sets a compression settings used to configure the deflate compressor
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

#if NET45 || NETSTANDARD || NETCOREAPP

		/// <summary>
		/// Constructs an instance of the deflate compressor factory
		/// </summary>
		public DeflateCompressorFactory()
			: this(new DeflateCompressionSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the deflate compressor factory
		/// </summary>
		/// <param name="settings">Compression settings used to configure the deflate compressor</param>
		public DeflateCompressorFactory(DeflateCompressionSettings settings)
		{
			CompressionSettings = settings;
		}

#endif

		/// <summary>
		/// Creates a instance of deflate compressor
		/// </summary>
		/// <returns>Instance of deflate compressor</returns>
		public ICompressor CreateCompressor()
		{
#if NET45 || NETSTANDARD || NETCOREAPP
			return new DeflateCompressor(CompressionSettings);
#else
			return new DeflateCompressor();
#endif
		}
	}
}