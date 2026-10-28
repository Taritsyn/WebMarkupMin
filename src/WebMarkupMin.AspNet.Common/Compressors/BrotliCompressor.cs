#if NETSTANDARD2_1 || NET9_0_OR_GREATER
using System.IO;
using System.IO.Compression;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Brotli compressor based on the <see cref="BrotliStream"/> class
	/// </summary>
	public sealed class BrotliCompressor : ICompressor
	{
		/// <summary>
		/// Encoding token of compressor
		/// </summary>
		public const string CompressorEncodingToken = "br";

		/// <summary>
		/// Brotli compression settings
		/// </summary>
		private readonly BrotliCompressionSettings _settings;

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
		/// Constructs an instance of the Brotli compressor
		/// </summary>
		public BrotliCompressor()
			: this(new BrotliCompressionSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the Brotli compressor
		/// </summary>
		/// <param name="settings">Brotli compression settings</param>
		public BrotliCompressor(BrotliCompressionSettings settings)
		{
			_settings = settings;
		}


		/// <summary>
		/// Compress a stream by Brotli algorithm
		/// </summary>
		/// <param name="stream">The stream</param>
		/// <returns>The compressed stream</returns>
		public Stream Compress(Stream stream)
		{
#if NET9_0_OR_GREATER
			BrotliCompressionOptions compressionOptions = _settings.GetOptions();
			var brotliStream = new BrotliStream(stream, compressionOptions);
#else
			var brotliStream = new BrotliStream(stream, _settings.Level);
#endif

			return brotliStream;
		}
	}
}
#endif