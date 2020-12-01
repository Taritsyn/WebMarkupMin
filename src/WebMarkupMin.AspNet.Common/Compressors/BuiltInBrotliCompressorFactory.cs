#if NETCOREAPP2_1 || NETSTANDARD2_1
namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Built-in brotli compressor factory
	/// </summary>
	public sealed class BuiltInBrotliCompressorFactory : ICompressorFactory
	{
		/// <summary>
		/// Gets or sets a compression settings used to configure the built-in brotli compressor
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
		/// Constructs an instance of the built-in brotli compressor factory
		/// </summary>
		public BuiltInBrotliCompressorFactory()
			: this(new BuiltInBrotliCompressionSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the built-in brotli compressor factory
		/// </summary>
		/// <param name="settings">Compression settings used to configure the built-in brotli compressor</param>
		public BuiltInBrotliCompressorFactory(BuiltInBrotliCompressionSettings settings)
		{
			CompressionSettings = settings;
		}


		/// <summary>
		/// Creates a instance of the built-in brotli compressor
		/// </summary>
		/// <returns>Instance of the built-in brotli compressor</returns>
		public ICompressor CreateCompressor()
		{
			return new BuiltInBrotliCompressor(CompressionSettings);
		}
	}
}
#endif