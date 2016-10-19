using System.IO;
using System.IO.Compression;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Deflate compressor
	/// </summary>
	public sealed class DeflateCompressor : ICompressor
	{
#if NETSTANDARD1_3 || NET451
		/// <summary>
		/// Deflate compression settings
		/// </summary>
		private readonly DeflateCompressionSettings _settings;

#endif
		/// <summary>
		/// Gets a encoding token
		/// </summary>
		public string EncodingToken
		{
			get { return EncodingTokenConstants.Deflate; }
		}

#if NETSTANDARD1_3 || NET451

		/// <summary>
		/// Constructs an instance of the deflate compressor
		/// </summary>
		public DeflateCompressor()
			: this(new DeflateCompressionSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the deflate compressor
		/// </summary>
		/// <param name="settings">Deflate compression settings</param>
		public DeflateCompressor(DeflateCompressionSettings settings)
		{
			_settings = settings;
		}

#endif

		/// <summary>
		/// Compress a stream by deflate algorithm
		/// </summary>
		/// <param name="stream">The stream</param>
		/// <returns>The compressed stream</returns>
		public Stream Compress(Stream stream)
		{
#if NETSTANDARD1_3 || NET451
			return new DeflateStream(stream, _settings.Level);
#else
			return new DeflateStream(stream, CompressionMode.Compress);
#endif
		}
	}
}