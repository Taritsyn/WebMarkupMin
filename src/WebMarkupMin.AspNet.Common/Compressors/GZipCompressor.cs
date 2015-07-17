using System.IO;
using System.IO.Compression;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// GZip compressor
	/// </summary>
	public sealed class GZipCompressor : ICompressor
	{
		/// <summary>
		/// Gets a encoding token
		/// </summary>
		public string EncodingToken
		{
			get { return EncodingTokenConstants.GZip; }
		}


		/// <summary>
		/// Compress a stream by GZip algorithm
		/// </summary>
		/// <param name="stream">The stream</param>
		/// <returns>The compressed stream</returns>
		public Stream Compress(Stream stream)
		{
			return new GZipStream(stream, CompressionMode.Compress);
		}
	}
}