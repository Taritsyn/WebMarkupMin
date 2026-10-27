#if NET11_0_OR_GREATER
using System.IO;
using System.IO.Compression;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Zstandard compressor
	/// </summary>
	public sealed class ZstandardCompressor : ICompressor
	{
		/// <summary>
		/// Encoding token of compressor
		/// </summary>
		public const string CompressorEncodingToken = "zstd";

		/// <summary>
		/// Zstandard compression settings
		/// </summary>
		private readonly ZstandardCompressionSettings _settings;

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
		/// Constructs an instance of the Zstandard compressor
		/// </summary>
		public ZstandardCompressor() : this(new ZstandardCompressionSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the Zstandard compressor
		/// </summary>
		/// <param name="settings">Zstandard compression settings</param>
		public ZstandardCompressor(ZstandardCompressionSettings settings)
		{
			_settings = settings;
		}


		/// <summary>
		/// Compress a stream by Zstandard algorithm
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