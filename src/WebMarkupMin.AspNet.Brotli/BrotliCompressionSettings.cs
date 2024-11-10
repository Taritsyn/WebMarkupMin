using System;

using WebMarkupMin.AspNet.Brotli.Resources;

namespace WebMarkupMin.AspNet.Brotli
{
	/// <summary>
	/// Brotli compression settings
	/// </summary>
	public sealed class BrotliCompressionSettings
	{
		/// <summary>
		/// Compression level
		/// </summary>
		private int _level;

		/// <summary>
		/// Gets or sets a compression level
		/// </summary>
		/// <remarks>
		/// The higher the level, the slower the compression. Range is from 0 to 11. The default value is 4.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">The value is less than 0 or greater than 9.</exception>
		public int Level
		{
			get { return _level; }
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

				_level = value;
			}
		}


		/// <summary>
		/// Constructs an instance of the brotli compression settings
		/// </summary>
		public BrotliCompressionSettings()
		{
			Level = BrotliCompressionLevelConstants.Default;
		}
	}
}