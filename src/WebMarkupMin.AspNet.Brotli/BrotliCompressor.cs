using System;
using System.IO;

#if NETSTANDARD2_1_OR_GREATER
using WebMarkupMin.AspNet.Brotli.Resources;
#endif
using WebMarkupMin.AspNet.Common.Compressors;

namespace WebMarkupMin.AspNet.Brotli
{
	/// <summary>
	/// Brotli compressor based on the BrotliSharpLib library
	/// </summary>
	[Obsolete("Use a `LegacyBrotliCompressor` class instead")]
	public sealed class BrotliCompressor : ICompressor
	{
		/// <summary>
		/// Encoding token of compressor
		/// </summary>
		public const string CompressorEncodingToken = LegacyBrotliCompressor.CompressorEncodingToken;

		/// <summary>
		/// Legacy Brotli compressor
		/// </summary>
		private readonly LegacyBrotliCompressor _innerCompressor;

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
			_innerCompressor = new LegacyBrotliCompressor(settings.GetInnerSettings());
#if NETSTANDARD2_1_OR_GREATER

			throw new NotSupportedException(
				string.Format(Strings.CompressorNotSupportNetCore3, nameof(BrotliCompressor)));
#endif
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