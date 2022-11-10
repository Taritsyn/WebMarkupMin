using System;
using System.IO;
using System.IO.Compression;
#if !NETSTANDARD2_1 && !NET6_0_OR_GREATER

using BrotliSharpLib;
#endif

using WebMarkupMin.AspNet.Common.Compressors;

namespace WebMarkupMin.AspNet.Brotli
{
	/// <summary>
	/// Brotli compressor
	/// </summary>
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
	[Obsolete("Use a `BuiltInBrotliCompressor` class from `WebMarkupMin.AspNet.Common.Compressors` namespace")]
#endif
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
		/// Constructs an instance of the brotli compressor
		/// </summary>
		public BrotliCompressor()
			: this(new BrotliCompressionSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the brotli compressor
		/// </summary>
		/// <param name="settings">Brotli compression settings</param>
		public BrotliCompressor(BrotliCompressionSettings settings)
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
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
			CompressionLevel compressionLevel = ConvertCompressionLevelNumberToEnum(_settings.Level);
			var brotliStream = new BrotliStream(stream, compressionLevel);
#else
			var brotliStream = new BrotliStream(stream, CompressionMode.Compress);
			brotliStream.SetQuality(_settings.Level);
#endif

			return brotliStream;
		}
#if NETSTANDARD2_1 || NET6_0_OR_GREATER

		/// <summary>
		/// Converts a numeric representation of the compression level to an enum
		/// </summary>
		/// <param name="level">Compression level represented as a number</param>
		/// <returns>Compression level represented as an enum</returns>
		/// <exception cref="NotSupportedException"/>
		private static CompressionLevel ConvertCompressionLevelNumberToEnum(int level)
		{
			CompressionLevel levelEnum;

			switch (level)
			{
				case 0:
					levelEnum = CompressionLevel.NoCompression;
					break;
				case 1:
				case 2:
					levelEnum = CompressionLevel.Fastest;
					break;
#if NET7_0_OR_GREATER
				case int n when n >= 3 && n <= 9:
					levelEnum = CompressionLevel.Optimal;
					break;
				case 10:
				case 11:
					levelEnum = CompressionLevel.SmallestSize;
					break;
#elif NET6_0
				case 3:
					levelEnum = (CompressionLevel)4;
					break;
				case int n when n >= 4 && n <= 11:
					levelEnum = (CompressionLevel)level;
					break;
#else

				case int n when n >= 3 && n <= 11:
					levelEnum = (CompressionLevel)level;
					break;
#endif
				default:
					throw new NotSupportedException();
			}

			return levelEnum;
		}
#endif
	}
}