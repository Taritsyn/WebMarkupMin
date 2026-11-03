using System.IO;
using System.IO.Compression;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Deflate compressor based on the <see cref="DeflateStream"/> class
	/// </summary>
	public sealed class DeflateCompressor : ICompressor
	{
		/// <summary>
		/// Encoding token of compressor
		/// </summary>
		public const string CompressorEncodingToken = "deflate";
#if COMPRESSION_SETTINGS

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


		/// <summary>
		/// Constructs an instance of the Deflate compressor
		/// </summary>
		public DeflateCompressor()
#if COMPRESSION_SETTINGS
			: this(new DeflateCompressionSettings())
#endif
		{ }
#if COMPRESSION_SETTINGS

		/// <summary>
		/// Constructs an instance of the Deflate compressor
		/// </summary>
		/// <param name="settings">Deflate compression settings</param>
		public DeflateCompressor(DeflateCompressionSettings settings)
		{
			_settings = settings;
		}
#endif


		/// <summary>
		/// Compress a stream by the Deflate algorithm
		/// </summary>
		/// <param name="stream">The stream</param>
		/// <returns>The compressed stream</returns>
		public Stream Compress(Stream stream)
		{
#if COMPRESSION_SETTINGS
	#if ALT_COMPRESSION_LEVEL_PROPERTY
			ZLibCompressionOptions compressionOptions = _settings.GetOptions();
			var deflateStream = new DeflateStream(stream, compressionOptions);
	#else
			var deflateStream = new DeflateStream(stream, _settings.Level);
	#endif
#else
			var deflateStream = new DeflateStream(stream, CompressionMode.Compress);
#endif

			return deflateStream;
		}
	}
}