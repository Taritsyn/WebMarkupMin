#if NETSTANDARD2_1 || NET9_0_OR_GREATER
using System;
using System.IO;
using System.IO.Compression;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Built-in brotli compressor based on the <see cref="BrotliStream"/> class
	/// </summary>
	[Obsolete("Use a `WebMarkupMin.AspNet.Common.Compressors.BrotliCompressor` class instead")]
	public sealed class BuiltInBrotliCompressor : ICompressor
	{
		/// <summary>
		/// Encoding token of compressor
		/// </summary>
		public const string CompressorEncodingToken = BrotliCompressor.CompressorEncodingToken;

		/// <summary>
		/// Brotli compressor
		/// </summary>
		private readonly BrotliCompressor _innerCompressor;

		/// <summary>
		/// Gets a encoding token
		/// </summary>
		public string EncodingToken
		{
			get { return _innerCompressor.EncodingToken; }
		}

		/// <summary>
		/// Gets a value that indicates if the compressor supports flushing
		/// </summary>
		public bool SupportsFlush
		{
			get { return _innerCompressor.SupportsFlush; }
		}


		/// <summary>
		/// Constructs an instance of the built-in Brotli compressor
		/// </summary>
		public BuiltInBrotliCompressor()
			: this(new BuiltInBrotliCompressionSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the built-in Brotli compressor
		/// </summary>
		/// <param name="settings">Built-in Brotli compression settings</param>
		public BuiltInBrotliCompressor(BuiltInBrotliCompressionSettings settings)
		{
			_innerCompressor = new BrotliCompressor(settings.GetInnerSettings());
		}


		/// <summary>
		/// Compress a stream by Brotli algorithm
		/// </summary>
		/// <param name="stream">The stream</param>
		/// <returns>The compressed stream</returns>
		public Stream Compress(Stream stream)
		{
			return _innerCompressor.Compress(stream);
		}
	}
}
#endif