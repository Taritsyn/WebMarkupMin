using System.IO;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Defines a interface of HTTP compressor
	/// </summary>
	public interface ICompressor
	{
		/// <summary>
		/// Gets a encoding token
		/// </summary>
		string EncodingToken { get; }


		/// <summary>
		/// Compress a stream
		/// </summary>
		/// <param name="stream">The stream</param>
		/// <returns>The compressed stream</returns>
		Stream Compress(Stream stream);
	}
}