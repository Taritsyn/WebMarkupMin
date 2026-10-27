#if NET11_0_OR_GREATER
namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Zstandard compressor factory
	/// </summary>
	public sealed class ZstandardCompressorFactory : ICompressorFactory
	{
		/// <summary>
		/// Gets or sets a compression settings used to configure the Zstandard compressor
		/// </summary>
		public ZstandardCompressionSettings CompressionSettings
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a encoding token
		/// </summary>
		public string EncodingToken
		{
			get { return ZstandardCompressor.CompressorEncodingToken; }
		}


		/// <summary>
		/// Constructs an instance of the Zstandard compressor factory
		/// </summary>
		public ZstandardCompressorFactory()
			: this(new ZstandardCompressionSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the Zstandard compressor factory
		/// </summary>
		/// <param name="settings">Compression settings used to configure the Zstandard compressor</param>
		public ZstandardCompressorFactory(ZstandardCompressionSettings settings)
		{
			CompressionSettings = settings;
		}


		/// <summary>
		/// Creates a instance of the Zstandard compressor
		/// </summary>
		/// <returns>Instance of the Zstandard compressor</returns>
		public ICompressor CreateCompressor()
		{
			return new ZstandardCompressor(CompressionSettings);
		}
	}
}
#endif