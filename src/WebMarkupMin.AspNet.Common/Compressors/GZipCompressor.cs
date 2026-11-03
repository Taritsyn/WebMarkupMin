using System.IO;
using System.IO.Compression;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// GZip compressor based on the <see cref="GZipStream"/> class
	/// </summary>
	public sealed class GZipCompressor : ICompressor
	{
		/// <summary>
		/// Encoding token of compressor
		/// </summary>
		public const string CompressorEncodingToken = "gzip";
#if COMPRESSION_SETTINGS

		/// <summary>
		/// GZip compression settings
		/// </summary>
		private readonly GZipCompressionSettings _settings;
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
		/// Constructs an instance of the GZip compressor
		/// </summary>
		public GZipCompressor()
#if COMPRESSION_SETTINGS
			: this(new GZipCompressionSettings())
#endif
		{ }
#if COMPRESSION_SETTINGS

		/// <summary>
		/// Constructs an instance of the GZip compressor
		/// </summary>
		/// <param name="settings">GZip compression settings</param>
		public GZipCompressor(GZipCompressionSettings settings)
		{
			_settings = settings;
		}
#endif


		/// <summary>
		/// Compress a stream by the GZip algorithm
		/// </summary>
		/// <param name="stream">The stream</param>
		/// <returns>The compressed stream</returns>
		public Stream Compress(Stream stream)
		{
#if COMPRESSION_SETTINGS
	#if ALT_COMPRESSION_LEVEL_PROPERTY
			ZLibCompressionOptions compressionOptions = _settings.GetOptions();
			var gzipStream = new GZipStream(stream, compressionOptions);
	#else
			var gzipStream = new GZipStream(stream, _settings.Level);
	#endif
#else
			var gzipStream = new GZipStream(stream, CompressionMode.Compress);
#endif

			return gzipStream;
		}
	}
}