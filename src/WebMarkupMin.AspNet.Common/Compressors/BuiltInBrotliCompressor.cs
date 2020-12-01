#if NETCOREAPP2_1 || NETSTANDARD2_1
using System.IO;
using System.IO.Compression;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Built-in brotli compressor
	/// </summary>
	public sealed class BuiltInBrotliCompressor : ICompressor
	{
		/// <summary>
		/// Encoding token of compressor
		/// </summary>
		public const string CompressorEncodingToken = "br";

		/// <summary>
		/// Brotli compression settings
		/// </summary>
		private readonly BuiltInBrotliCompressionSettings _settings;

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
		/// Constructs an instance of the built-in brotli compressor
		/// </summary>
		public BuiltInBrotliCompressor()
			: this(new BuiltInBrotliCompressionSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the built-in brotli compressor
		/// </summary>
		/// <param name="settings">Brotli compression settings</param>
		public BuiltInBrotliCompressor(BuiltInBrotliCompressionSettings settings)
		{
			_settings = settings;
		}


		/// <summary>
		/// Compress a stream by brotli algorithm
		/// </summary>
		/// <param name="stream">The stream</param>
		/// <returns>The compressed stream</returns>
		public Stream Compress(Stream stream)
		{
			return new BrotliStream(stream, _settings.Level);
		}
	}
}
#endif