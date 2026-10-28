#if NET11_0_OR_GREATER
using System.IO;
using System.IO.Compression;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Zstd (Zstandard) compressor based on the <see cref="ZstandardStream"/> class
	/// </summary>
	public sealed class ZstdCompressor : ICompressor
	{
		/// <summary>
		/// Encoding token of compressor
		/// </summary>
		public const string CompressorEncodingToken = "zstd";

		/// <summary>
		/// Zstd compression settings
		/// </summary>
		private readonly ZstdCompressionSettings _settings;

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
			get { return true; }
		}


		/// <summary>
		/// Constructs an instance of the Zstd compressor
		/// </summary>
		public ZstdCompressor() : this(new ZstdCompressionSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the Zstd compressor
		/// </summary>
		/// <param name="settings">Zstd compression settings</param>
		public ZstdCompressor(ZstdCompressionSettings settings)
		{
			_settings = settings;
		}


		/// <summary>
		/// Compress a stream by the Zstd (Zstandard) algorithm
		/// </summary>
		/// <param name="stream">The stream</param>
		/// <returns>The compressed stream</returns>
		public Stream Compress(Stream stream)
		{
			ZstandardCompressionOptions compressionOptions = _settings.GetOptions();
			var zstdStream = new ZstandardStream(stream, compressionOptions);

			return zstdStream;
		}
	}
}
#endif