#if COMPRESSION_SETTINGS
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
	/// Base class of the ZLib compression settings
	/// </summary>
	public abstract class ZLibCompressionSettingsBase
#if ALT_COMPRESSION_LEVEL_PROPERTY
		: CommonCompressionSettingsBase<ZLibCompressionOptions>
#else
		: CommonCompressionSettingsBase
#endif
	{
#if ALT_COMPRESSION_LEVEL_PROPERTY
		/// <summary>
		/// Gets a default alternative compression level
		/// </summary>
		public static readonly int DefaultAltCompressionLevel = 6;

		/// <summary>
		/// Gets a minimum alternative compression level
		/// </summary>
		public static readonly int MinAltCompressionLevel = 0;

		/// <summary>
		/// Gets a maximum alternative compression level
		/// </summary>
		public static readonly int MaxAltCompressionLevel = 9;

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
				return ConvertCompressionLevelNumberToEnum(_options.CompressionLevel);
			}
			set
			{
				_options.CompressionLevel = ConvertCompressionLevelEnumToNumber(value);
			}
#else
			get;
			set;
#endif
		}
#if ALT_COMPRESSION_LEVEL_PROPERTY

		/// <inheritdoc/>
		/// <remarks>
		/// The higher the level, the slower the compression. Range is from 0 to 9. The default value is 6.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">The value is less than 0 or greater than 9.</exception>
		public override int AlternativeLevel
		{
			get
			{
				return _options.CompressionLevel;
			}
			set
			{
				if (value != -1)
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

				_options.CompressionLevel = value;
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
			if (level == -1 || level == DefaultAltCompressionLevel)
			{
				return CompressionLevel.Optimal;
			}

			CompressionLevel levelEnum = level switch
			{
				int n when n == MinAltCompressionLevel => CompressionLevel.NoCompression,
				int n when n > MinAltCompressionLevel && n <= 2 => CompressionLevel.Fastest,
				int n when n >= 3 && n <= 7 => CompressionLevel.Optimal,
				int n when n >= 8 && n <= MaxAltCompressionLevel => CompressionLevel.SmallestSize,
				_ => throw new NotSupportedException()
			};

			return levelEnum;
		}
#endif
	}
}
#endif