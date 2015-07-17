using System.IO;
using System.IO.Compression;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Deflate compressor
	/// </summary>
	public sealed class DeflateCompressor : ICompressor
	{
		/// <summary>
		/// Gets a encoding token
		/// </summary>
		public string EncodingToken
		{
			get { return EncodingTokenConstants.Deflate; }
		}


		/// <summary>
		/// Compress a stream by deflate algorithm
		/// </summary>
		/// <param name="stream">The stream</param>
		/// <returns>The compressed stream</returns>
		public Stream Compress(Stream stream)
		{
			return new DeflateStream(stream, CompressionMode.Compress);
		}
	}
}