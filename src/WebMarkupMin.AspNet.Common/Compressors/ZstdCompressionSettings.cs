#if ZSTD && COMPRESSION_SETTINGS
using System;
using System.IO.Compression;

using WebMarkupMin.AspNet.Common.Resources;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Zstd (Zstandard) compression settings
	/// </summary>
	public sealed class ZstdCompressionSettings : CommonCompressionSettingsBase<ZstandardCompressionOptions>
	{
		/// <summary>
		/// Gets a default alternative compression level
		/// </summary>
		public static readonly int DefaultAltCompressionLevel = ZstandardCompressionOptions.DefaultQuality;

		/// <summary>
		/// Gets a minimum alternative compression level
		/// </summary>
		public static readonly int MinAltCompressionLevel = ZstandardCompressionOptions.MinQuality;

		/// <summary>
		/// Gets a maximum alternative compression level
		/// </summary>
		public static readonly int MaxAltCompressionLevel = ZstandardCompressionOptions.MaxQuality;

		/// <summary>
		/// Gets a fastest alternative compression level
		/// </summary>
		public static readonly int FastestAltCompressionLevel = 1;


		/// <inheritdoc/>
		public override CompressionLevel Level
		{
			get
			{
				return ConvertCompressionLevelNumberToEnum(_options.Quality);
			}
			set
			{
				_options.Quality = ConvertCompressionLevelEnumToNumber(value);
			}
		}

		/// <inheritdoc/>
		/// <remarks>
		/// The alternative compression level determines the compression ratio and speed.
		/// Negative values extend the range of speed versus ratio preferences, where lower levels are faster, but
		/// provide less compression.
		/// Value 0 indicates the implementation-defined default compression level.
		/// Values 1-22 are normal compression levels, with <see cref="DefaultAltCompressionLevel"/>
		/// being the default.
		/// Values 20-22 require more memory and should be used with caution.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">The value is not 0 and is not between
		/// <see cref="MinAltCompressionLevel"/> and <see cref="MaxAltCompressionLevel"/>.</exception>
		public override int AlternativeLevel
		{
			get
			{
				return _options.Quality;
			}
			set
			{
				if (value != 0)
				{
					if (value < MinAltCompressionLevel || value > MaxAltCompressionLevel)
					{
						throw new ArgumentOutOfRangeException(
							nameof(value),
							string.Format(Strings.AlternativeCompressionLevelOutOfRange,
								MinAltCompressionLevel, MaxAltCompressionLevel)
						);
					}
				}

				_options.Quality = value;
			}
		}


		/// <summary>
		/// Constructs an instance of the Zstd compression settings
		/// </summary>
		public ZstdCompressionSettings() : base()
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
			if (level == 0 || level == DefaultAltCompressionLevel)
			{
				return CompressionLevel.Optimal;
			}

			CompressionLevel levelEnum = level switch
			{
				int n when n == MinAltCompressionLevel => CompressionLevel.NoCompression,
				int n when n > MinAltCompressionLevel && n <= 1 => CompressionLevel.Fastest,
				int n when n >= 2 && n <= 19 => CompressionLevel.Optimal,
				int n when n >= 20 && n <= MaxAltCompressionLevel => CompressionLevel.SmallestSize,
				_ => throw new NotSupportedException()
			};

			return levelEnum;
		}
	}
}
#endif