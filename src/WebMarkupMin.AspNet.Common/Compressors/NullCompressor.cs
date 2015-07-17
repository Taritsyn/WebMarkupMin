using System.IO;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Null compressor
	/// </summary>
	public class NullCompressor : ICompressor
	{
		/// <summary>
		/// Gets a encoding token
		/// </summary>
		public string EncodingToken
		{
			get { return string.Empty; }
		}


		/// <summary>
		/// Do not performs operations with the stream
		/// </summary>
		/// <param name="stream">The stream</param>
		/// <returns>The stream</returns>
		public Stream Compress(Stream stream)
		{
			return stream;
		}
	}
}