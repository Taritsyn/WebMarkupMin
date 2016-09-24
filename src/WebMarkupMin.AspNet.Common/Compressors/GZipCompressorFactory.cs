namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// GZip compressor factory
	/// </summary>
	public sealed class GZipCompressorFactory : ICompressorFactory
	{
		/// <summary>
		/// Gets a encoding token
		/// </summary>
		public string EncodingToken
		{
			get { return EncodingTokenConstants.GZip; }
		}


		/// <summary>
		/// Creates a instance of GZip compressor
		/// </summary>
		/// <returns>Instance of GZip compressor</returns>
		public ICompressor CreateCompressor()
		{
			return new GZipCompressor();
		}
	}
}