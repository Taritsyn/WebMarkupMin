#if NET11_0_OR_GREATER
using System;
using System.IO.Compression;

using WebMarkupMin.AspNet.Common.Resources;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Zstandard compression settings
	/// </summary>
	public sealed class ZstandardCompressionSettings : CommonCompressionSettingsBase<ZstandardCompressionOptions>
	{
		private static readonly int MinAltCompressionLevel = ZstandardCompressionOptions.MinQuality;
		private static readonly int FastestAltCompressionLevel = 1;
		private static readonly int DefaultAltCompressionLevel = ZstandardCompressionOptions.DefaultQuality;
		private static readonly int HighAltCompressionLevel = 20;
		private static readonly int MaxAltCompressionLevel = ZstandardCompressionOptions.MaxQuality;

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
		/// Values 1-22 are normal compression levels, with <see cref="ZstandardCompressionOptions.DefaultQuality"/>
		/// being the default.
		/// Values 20-22 require more memory and should be used with caution.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">The value is not 0 and is not between
		/// <see cref="ZstandardCompressionOptions.MinQuality"/> and
		/// <see cref="ZstandardCompressionOptions.MaxQuality"/>.</exception>
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
			CompressionLevel levelEnum = level switch
			{
				int n when n == MinAltCompressionLevel => CompressionLevel.NoCompression,
				int n when n > MinAltCompressionLevel && n <= FastestAltCompressionLevel => CompressionLevel.Fastest,
				int n when n == DefaultAltCompressionLevel => CompressionLevel.Optimal,
				int n when n > FastestAltCompressionLevel && n < HighAltCompressionLevel => CompressionLevel.Optimal,
				int n when n >= HighAltCompressionLevel && n <= MaxAltCompressionLevel => CompressionLevel.SmallestSize,
				_ => throw new NotSupportedException()
			};

			return levelEnum;
		}
	}
}
#endif