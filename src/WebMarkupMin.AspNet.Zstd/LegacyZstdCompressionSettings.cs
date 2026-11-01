using System;
using System.IO.Compression;

using ZstdSharp;

using CommonStrings = WebMarkupMin.AspNet.Common.Resources.Strings;

namespace WebMarkupMin.AspNet.Zstd
{
	/// <summary>
	/// Legacy Zstd (Zstandard) compression settings
	/// </summary>
	public sealed class LegacyZstdCompressionSettings
	{
		private static readonly int MinAltCompressionLevel = Compressor.MinCompressionLevel;
		private static readonly int FastestAltCompressionLevel = 1;
		private static readonly int DefaultAltCompressionLevel = Compressor.DefaultCompressionLevel;
#if NET6_0_OR_GREATER
		private static readonly int HighAltCompressionLevel = 20;
#endif
		private static readonly int MaxAltCompressionLevel = Compressor.MaxCompressionLevel;

		/// <summary>
		/// Alternative compression level
		/// </summary>
		private int _alternativeLevel;

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

		/// <summary>
		/// Gets or sets a alternative compression level
		/// </summary>
		/// <remarks>
		/// The alternative compression level determines the compression ratio and speed.
		/// Negative values extend the range of speed versus ratio preferences, where lower levels are faster, but
		/// provide less compression.
		/// Value 0 indicates the implementation-defined default compression level.
		/// Values 1-22 are normal compression levels, with <see cref="Compressor.DefaultCompressionLevel"/>
		/// being the default.
		/// Values 20-22 require more memory and should be used with caution.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">The value is not 0 and is not between
		/// <see cref="Compressor.MinCompressionLevel"/> and
		/// <see cref="Compressor.MaxCompressionLevel"/>.</exception>
		public int AlternativeLevel
		{
			get
			{
				return _alternativeLevel;
			}
			set
			{
				if (value != 0)
				{
					if (value < MinAltCompressionLevel || value > MaxAltCompressionLevel)
					{
						throw new ArgumentOutOfRangeException(
							nameof(value),
							string.Format(CommonStrings.AlternativeCompressionLevelOutOfRange,
								MinAltCompressionLevel, MaxAltCompressionLevel)
						);
					}
				}

				_alternativeLevel = value;
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
#if NET6_0_OR_GREATER
				CompressionLevel.SmallestSize => MaxAltCompressionLevel,
#endif
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
#if NET6_0_OR_GREATER
				int n when n > FastestAltCompressionLevel && n < HighAltCompressionLevel => CompressionLevel.Optimal,
				int n when n >= HighAltCompressionLevel && n <= MaxAltCompressionLevel => CompressionLevel.SmallestSize,
#else
				int n when n > FastestAltCompressionLevel && n <= MaxAltCompressionLevel => CompressionLevel.Optimal,
#endif
				_ => throw new NotSupportedException()
			};

			return levelEnum;
		}
	}
}