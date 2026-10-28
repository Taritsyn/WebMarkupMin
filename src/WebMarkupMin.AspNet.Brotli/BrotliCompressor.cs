#if NETSTANDARD2_1_OR_GREATER
using System;
#endif
using System.IO;
using System.IO.Compression;

using WebMarkupMin.AspNet.Common.Compressors;
#if NETSTANDARD2_1_OR_GREATER
using WebMarkupMin.AspNet.Brotli.Resources;
#endif

namespace WebMarkupMin.AspNet.Brotli
{
	/// <summary>
	/// Brotli compressor based on the BrotliSharpLib library
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
#if NETSTANDARD2_1_OR_GREATER

			throw new NotSupportedException(Strings.CompressorNotSupportNetCore3);
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
			brotliStream.SetQuality(_settings.Level);

			return brotliStream;
		}
	}
}