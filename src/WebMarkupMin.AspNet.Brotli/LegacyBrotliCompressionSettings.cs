using System;
#if COMPRESSION_LEVEL_PROPERTY
using System.IO.Compression;
#endif

using CommonStrings = WebMarkupMin.AspNet.Common.Resources.Strings;

namespace WebMarkupMin.AspNet.Brotli
{
	/// <summary>
	/// Legacy Brotli compression settings
	/// </summary>
	public sealed class LegacyBrotliCompressionSettings
	{
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

		/// <summary>
		/// Alternative compression level
		/// </summary>
		private int _alternativeLevel;
#if COMPRESSION_LEVEL_PROPERTY

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
				if (value < MinAltCompressionLevel || value > MaxAltCompressionLevel)
				{
					throw new ArgumentOutOfRangeException(
						nameof(value),
						string.Format(CommonStrings.AlternativeCompressionLevelOutOfRange,
							MinAltCompressionLevel, MaxAltCompressionLevel)
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
			AlternativeLevel = DefaultAltCompressionLevel;
		}
#if COMPRESSION_LEVEL_PROPERTY


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
				int n when n >= 3 && n <= MaxAltCompressionLevel => CompressionLevel.Optimal,
				_ => throw new NotSupportedException()
			};

			return levelEnum;
		}
#endif
	}
}