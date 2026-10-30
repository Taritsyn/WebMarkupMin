#if NETSTANDARD2_1_OR_GREATER
using System;
#endif
using System.IO;
using System.IO.Compression;

#if NETSTANDARD2_1_OR_GREATER
using WebMarkupMin.AspNet.Brotli.Resources;
#endif
using WebMarkupMin.AspNet.Common.Compressors;

namespace WebMarkupMin.AspNet.Brotli
{
	/// <summary>
	/// Legacy Brotli compressor based on the BrotliSharpLib library
	/// </summary>
	public sealed class LegacyBrotliCompressor : ICompressor
	{
		/// <summary>
		/// Encoding token of compressor
		/// </summary>
		public const string CompressorEncodingToken = "br";

		/// <summary>
		/// Legacy Brotli compression settings
		/// </summary>
		private readonly LegacyBrotliCompressionSettings _settings;

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
		/// Constructs an instance of the legacy Brotli compressor
		/// </summary>
		public LegacyBrotliCompressor()
			: this(new LegacyBrotliCompressionSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the legacy Brotli compressor
		/// </summary>
		/// <param name="settings">Legacy Brotli compression settings</param>
		public LegacyBrotliCompressor(LegacyBrotliCompressionSettings settings)
		{
			_settings = settings;
#if NETSTANDARD2_1_OR_GREATER

			throw new NotSupportedException(
				string.Format(Strings.CompressorNotSupportNetCore3, nameof(LegacyBrotliCompressor)));
#endif
		}


		/// <summary>
		/// Compress a stream by Brotli algorithm
		/// </summary>
		/// <param name="stream">The stream</param>
		/// <returns>The compressed stream</returns>
		public Stream Compress(Stream stream)
		{
			var brotliStream = new BrotliSharpLib.BrotliStream(stream, CompressionMode.Compress);
			brotliStream.SetQuality(_settings.AlternativeLevel);

			return brotliStream;
		}
	}
}