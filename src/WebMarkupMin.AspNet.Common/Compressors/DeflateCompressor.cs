using System.IO;
using System.IO.Compression;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Deflate compressor
	/// </summary>
	public sealed class DeflateCompressor : ICompressor
	{
#if NET45 || NETSTANDARD
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

		/// <summary>
		/// Gets a value that indicates if the compressor supports flushing
		/// </summary>
		public bool SupportsFlush
		{
			get
			{
#if NETFULL
				return false;
#elif NETSTANDARD
				return true;
#else
#error No implementation for this target
#endif
			}
		}

#if NET45 || NETSTANDARD

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
#if NET45 || NETSTANDARD
			return new DeflateStream(stream, _settings.Level);
#else
			return new DeflateStream(stream, CompressionMode.Compress);
#endif
		}
	}
}