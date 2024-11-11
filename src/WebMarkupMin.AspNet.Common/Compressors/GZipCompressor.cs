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
		/// Encoding token of compressor
		/// </summary>
		public const string CompressorEncodingToken = "gzip";
#if NET45 || NETSTANDARD || NET9_0_OR_GREATER

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

#if NET45 || NETSTANDARD || NET9_0_OR_GREATER

		/// <summary>
		/// Constructs an instance of the GZip compressor
		/// </summary>
		public GZipCompressor()
			: this(new GZipCompressionSettings())
		{ }

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
		/// Compress a stream by GZip algorithm
		/// </summary>
		/// <param name="stream">The stream</param>
		/// <returns>The compressed stream</returns>
		public Stream Compress(Stream stream)
		{
#if NET9_0_OR_GREATER
			ZLibCompressionOptions compressionOptions = _settings.GetOptions();
			var gzipStream = new GZipStream(stream, compressionOptions);
#elif NET45 || NETSTANDARD
			var gzipStream = new GZipStream(stream, _settings.Level);
#else
			var gzipStream = new GZipStream(stream, CompressionMode.Compress);
#endif

			return gzipStream;
		}
	}
}