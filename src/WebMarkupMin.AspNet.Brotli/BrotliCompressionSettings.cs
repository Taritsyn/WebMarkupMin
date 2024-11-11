using System;
#if NET9_0_OR_GREATER
using System.IO.Compression;
#endif

using WebMarkupMin.AspNet.Brotli.Resources;

namespace WebMarkupMin.AspNet.Brotli
{
	/// <summary>
	/// Brotli compression settings
	/// </summary>
	public sealed class BrotliCompressionSettings
	{
#if NET9_0_OR_GREATER
		/// <summary>
		/// Compression options
		/// </summary>
		private BrotliCompressionOptions _options = new();
#else
		/// <summary>
		/// Compression level
		/// </summary>
		private int _level;
#endif

		/// <summary>
		/// Gets or sets a compression level
		/// </summary>
		/// <remarks>
		/// The higher the level, the slower the compression. Range is from 0 to 11. The default value is 4.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">The value is less than 0 or greater than 11.</exception>
		public int Level
		{
			get
			{
#if NET9_0_OR_GREATER
				return _options.Quality;
#else
				return _level;
#endif
			}
			set
			{
				if (value < BrotliCompressionLevelConstants.Min || value > BrotliCompressionLevelConstants.Max)
				{
					throw new ArgumentOutOfRangeException(
						nameof(value),
						string.Format(Strings.CompressionLevelOutOfRange,
							BrotliCompressionLevelConstants.Min, BrotliCompressionLevelConstants.Max)
					);
				}

#if NET9_0_OR_GREATER
				_options.Quality = value;
#else
				_level = value;
#endif
			}
		}


		/// <summary>
		/// Constructs an instance of the brotli compression settings
		/// </summary>
		public BrotliCompressionSettings()
		{
			Level = BrotliCompressionLevelConstants.Default;
		}
#if NET9_0_OR_GREATER

		/// <summary>
		/// Gets a compression options
		/// </summary>
		/// <returns>Compression options</returns>
		internal BrotliCompressionOptions GetOptions()
		{
			return _options;
		}
#endif
	}
}