using System.IO;

using ZstdSharp;

using WebMarkupMin.AspNet.Common.Compressors;

namespace WebMarkupMin.AspNet.Zstd
{
	/// <summary>
	/// Legacy Zstd (Zstandard) compressor based on the ZstdSharp library
	/// </summary>
	public sealed class LegacyZstdCompressor : ICompressor
	{
		/// <summary>
		/// Encoding token of compressor
		/// </summary>
		public const string CompressorEncodingToken = "zstd";

		/// <summary>
		/// Legacy Zstd compression settings
		/// </summary>
		private readonly LegacyZstdCompressionSettings _settings;

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
		/// Constructs an instance of the legacy Zstd compressor
		/// </summary>
		public LegacyZstdCompressor()
			: this(new LegacyZstdCompressionSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the legacy Zstd compressor
		/// </summary>
		/// <param name="settings">Legacy Zstd compression settings</param>
		public LegacyZstdCompressor(LegacyZstdCompressionSettings settings)
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
			var zstdStream = new CompressionStream(stream, _settings.AlternativeLevel);

			return zstdStream;
		}
	}
}