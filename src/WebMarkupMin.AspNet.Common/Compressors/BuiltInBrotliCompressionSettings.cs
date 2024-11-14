#if NETSTANDARD2_1 || NET9_0_OR_GREATER
#if NET9_0_OR_GREATER
using System;
#endif
using System.IO.Compression;
#if NET9_0_OR_GREATER

using WebMarkupMin.AspNet.Common.Resources;
#endif

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Brotli compression settings
	/// </summary>
	public sealed class BuiltInBrotliCompressionSettings
#if NET9_0_OR_GREATER
		: CommonCompressionSettingsBase<BrotliCompressionOptions>
#else
		: CommonCompressionSettingsBase
#endif
	{
#if NET9_0_OR_GREATER
		/// <summary>
		/// The minimum compression level
		/// </summary>
		private const int MIN_ALT_COMPRESSION_LEVEL = 0;

		/// <summary>
		/// The maximum compression level
		/// </summary>
		private const int MAX_ALT_COMPRESSION_LEVEL = 11;

#endif
		/// <inheritdoc/>
		public override CompressionLevel Level
		{
#if NET9_0_OR_GREATER
			get
			{
				return ConvertCompressionLevelNumberToEnum(_options.Quality);
			}
			set
			{
				_options.Quality = ConvertCompressionLevelEnumToNumber(value);
			}
#else
			get;
			set;
#endif
		}
#if NET9_0_OR_GREATER

		/// <inheritdoc/>
		/// <remarks>
		/// The higher the level, the slower the compression. Range is from 0 to 11. The default value is 4.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">The value is less than 0 or greater than 11.</exception>
		public override int AlternativeLevel
		{
			get
			{
				return _options.Quality;
			}
			set
			{
				if (value < MIN_ALT_COMPRESSION_LEVEL || value > MAX_ALT_COMPRESSION_LEVEL)
				{
					throw new ArgumentOutOfRangeException(
						nameof(value),
						string.Format(Strings.AlternativeCompressionLevelOutOfRange,
							MIN_ALT_COMPRESSION_LEVEL, MAX_ALT_COMPRESSION_LEVEL)
					);
				}

				_options.Quality = value;
			}
		}


		/// <summary>
		/// Converts a enum representation of the compression level to a number
		/// </summary>
		/// <param name="level">Compression level represented as an enum</param>
		/// <returns>Compression level represented as a number</returns>
		/// <exception cref="NotSupportedException"/>
		private static int ConvertCompressionLevelEnumToNumber(CompressionLevel level)
		{
			int levelNumber = level switch
			{
				CompressionLevel.NoCompression => MIN_ALT_COMPRESSION_LEVEL,
				CompressionLevel.Fastest => 1,
				CompressionLevel.Optimal => 4,
				CompressionLevel.SmallestSize => MAX_ALT_COMPRESSION_LEVEL,
				_ => throw new NotSupportedException()
			};

			return levelNumber;
		}

		/// <summary>
		/// Converts a numeric representation of the compression level to an enum
		/// </summary>
		/// <param name="level">Compression level represented as a number</param>
		/// <returns>Compression level represented as an enum</returns>
		/// <exception cref="NotSupportedException"/>
		private static CompressionLevel ConvertCompressionLevelNumberToEnum(int level)
		{
			CompressionLevel levelEnum = level switch
			{
				MIN_ALT_COMPRESSION_LEVEL => CompressionLevel.NoCompression,
				int n when n == 1 || n == 2 => CompressionLevel.Fastest,
				int n when n >= 3 && n <= 9 => CompressionLevel.Optimal,
				int n when n == 10 || n == MAX_ALT_COMPRESSION_LEVEL => CompressionLevel.SmallestSize,
				_ => throw new NotSupportedException()
			};

			return levelEnum;
		}
#endif
	}
}
#endif