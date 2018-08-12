using WebMarkupMin.AspNet.Common.Compressors;

namespace WebMarkupMin.AspNet.Brotli
{
	/// <summary>
	/// Brotli compressor factory
	/// </summary>
	public sealed class BrotliCompressorFactory : ICompressorFactory
	{
		/// <summary>
		/// Gets or sets a compression settings used to configure the brotli compressor
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
		/// Constructs an instance of the brotli compressor factory
		/// </summary>
		public BrotliCompressorFactory()
			: this(new BrotliCompressionSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the brotli compressor factory
		/// </summary>
		/// <param name="settings">Compression settings used to configure the brotli compressor</param>
		public BrotliCompressorFactory(BrotliCompressionSettings settings)
		{
			CompressionSettings = settings;
		}


		/// <summary>
		/// Creates a instance of brotli compressor
		/// </summary>
		/// <returns>Instance of brotli compressor</returns>
		public ICompressor CreateCompressor()
		{
			return new BrotliCompressor(CompressionSettings);
		}
	}
}