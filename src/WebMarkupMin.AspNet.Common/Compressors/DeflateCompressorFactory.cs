namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Deflate compressor factory
	/// </summary>
	public sealed class DeflateCompressorFactory : ICompressorFactory
	{
		/// <summary>
		/// Gets a encoding token
		/// </summary>
		public string EncodingToken
		{
			get { return EncodingTokenConstants.Deflate; }
		}


		/// <summary>
		/// Creates a instance of deflate compressor
		/// </summary>
		/// <returns>Instance of deflate compressor</returns>
		public ICompressor CreateCompressor()
		{
			return new DeflateCompressor();
		}
	}
}