namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Defines a interface of HTTP compressor factory
	/// </summary>
	public interface ICompressorFactory
	{
		/// <summary>
		/// Gets a encoding token
		/// </summary>
		string EncodingToken { get; }


		/// <summary>
		/// Creates a instance of HTTP compressor
		/// </summary>
		/// <returns>Instance of HTTP compressor</returns>
		ICompressor CreateCompressor();
	}
}