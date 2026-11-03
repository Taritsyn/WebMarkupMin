#if BROTLI && COMPRESSION_SETTINGS
#if ALT_COMPRESSION_LEVEL_PROPERTY
using System;
#endif
using System.IO.Compression;
#if ALT_COMPRESSION_LEVEL_PROPERTY

using WebMarkupMin.AspNet.Common.Resources;
#endif

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Brotli compression settings
	/// </summary>
	public sealed class BrotliCompressionSettings
#if ALT_COMPRESSION_LEVEL_PROPERTY
		: CommonCompressionSettingsBase<BrotliCompressionOptions>
#else
		: CommonCompressionSettingsBase
#endif
	{
#if ALT_COMPRESSION_LEVEL_PROPERTY
		/// <summary>
		/// Gets a default alternative compression level
		/// </summary>
		public static readonly int DefaultAltCompressionLevel = 4;

		/// <summary>
		/// Gets a minimum alternative compression level
		/// </summary>
		public static readonly int MinAltCompressionLevel = 0;

		/// <summary>
		/// Gets a maximum alternative compression level
		/// </summary>
		public static readonly int MaxAltCompressionLevel = 11;

		/// <summary>
		/// Gets a fastest alternative compression level
		/// </summary>
		public static readonly int FastestAltCompressionLevel = 1;

#endif
		/// <inheritdoc/>
		public override CompressionLevel Level
		{
#if ALT_COMPRESSION_LEVEL_PROPERTY
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
#if ALT_COMPRESSION_LEVEL_PROPERTY

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
				if (value < MinAltCompressionLevel || value > MaxAltCompressionLevel)
				{
					throw new ArgumentOutOfRangeException(
						nameof(value),
						string.Format(Strings.AlternativeCompressionLevelOutOfRange,
							MinAltCompressionLevel, MaxAltCompressionLevel)
					);
				}

				_options.Quality = value;
			}
		}


		/// <summary>
		/// Constructs an instance of the Brotli compression settings
		/// </summary>
		public BrotliCompressionSettings() : base()
		{ }


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
				CompressionLevel.NoCompression => MinAltCompressionLevel,
				CompressionLevel.Fastest => FastestAltCompressionLevel,
				CompressionLevel.Optimal => DefaultAltCompressionLevel,
				CompressionLevel.SmallestSize => MaxAltCompressionLevel,
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
			if (level == DefaultAltCompressionLevel)
			{
				return CompressionLevel.Optimal;
			}

			CompressionLevel levelEnum = level switch
			{
				int n when n == MinAltCompressionLevel => CompressionLevel.NoCompression,
				int n when n > MinAltCompressionLevel && n <= 2 => CompressionLevel.Fastest,
				int n when n >= 3 && n <= 9 => CompressionLevel.Optimal,
				int n when n >= 10 && n <= MaxAltCompressionLevel => CompressionLevel.SmallestSize,
				_ => throw new NotSupportedException()
			};

			return levelEnum;
		}
#endif
	}
}
#endif