using System;
#if !NET40
using System.IO.Compression;
#endif

using WebMarkupMin.AspNet.Brotli.Resources;

namespace WebMarkupMin.AspNet.Brotli
{
	/// <summary>
	/// Legacy Brotli compression settings
	/// </summary>
	public sealed class LegacyBrotliCompressionSettings
	{
		private const int MIN_ALT_COMPRESSION_LEVEL = 0;
		private const int DEFAULT_ALT_COMPRESSION_LEVEL = 4;
		private const int MAX_ALT_COMPRESSION_LEVEL = 11;

		/// <summary>
		/// Alternative compression level
		/// </summary>
		private int _alternativeLevel;
#if !NET40

		/// <summary>
		/// Gets or sets a compression level
		/// </summary>
		public CompressionLevel Level
		{
			get
			{
				return ConvertCompressionLevelNumberToEnum(_alternativeLevel);
			}
			set
			{
				_alternativeLevel = ConvertCompressionLevelEnumToNumber(value);
			}
		}
#endif

		/// <summary>
		/// Gets or sets a alternative compression level
		/// </summary>
		/// <remarks>
		/// The higher the level, the slower the compression. Range is from 0 to 11. The default value is 4.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">The value is less than 0 or greater than 11.</exception>
		public int AlternativeLevel
		{
			get
			{
				return _alternativeLevel;
			}
			set
			{
				if (value < MIN_ALT_COMPRESSION_LEVEL || value > MAX_ALT_COMPRESSION_LEVEL)
				{
					throw new ArgumentOutOfRangeException(
						nameof(value),
						string.Format(Strings.CompressionLevelOutOfRange,
							MIN_ALT_COMPRESSION_LEVEL, MAX_ALT_COMPRESSION_LEVEL)
					);
				}

				_alternativeLevel = value;
			}
		}


		/// <summary>
		/// Constructs an instance of the legacy Brotli compression settings
		/// </summary>
		public LegacyBrotliCompressionSettings()
		{
			AlternativeLevel = DEFAULT_ALT_COMPRESSION_LEVEL;
		}
#if !NET40


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
				CompressionLevel.Optimal => DEFAULT_ALT_COMPRESSION_LEVEL,
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
				int n when n >= 3 && n <= MAX_ALT_COMPRESSION_LEVEL => CompressionLevel.Optimal,
				_ => throw new NotSupportedException()
			};

			return levelEnum;
		}
#endif
	}
}