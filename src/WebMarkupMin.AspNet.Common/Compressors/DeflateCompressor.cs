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
		/// Encoding token of compressor
		/// </summary>
		public const string CompressorEncodingToken = "deflate";
#if NET45 || NETSTANDARD || NET9_0_OR_GREATER

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
			get { return CompressorEncodingToken; }
		}

		/// <summary>
		/// Gets a value that indicates if the compressor supports flushing
		/// </summary>
		public bool SupportsFlush
		{
			get
			{
#if NETFRAMEWORK
				return false;
#elif NETSTANDARD || NET9_0_OR_GREATER
				return true;
#else
#error No implementation for this target
#endif
			}
		}

#if NET45 || NETSTANDARD || NET9_0_OR_GREATER

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
#if NET9_0_OR_GREATER
			ZLibCompressionOptions compressionOptions = _settings.GetOptions();
			var deflateStream = new DeflateStream(stream, compressionOptions);
#elif NET45 || NETSTANDARD
			var deflateStream = new DeflateStream(stream, _settings.Level);
#else
			var deflateStream = new DeflateStream(stream, CompressionMode.Compress);
#endif

			return deflateStream;
		}
	}
}