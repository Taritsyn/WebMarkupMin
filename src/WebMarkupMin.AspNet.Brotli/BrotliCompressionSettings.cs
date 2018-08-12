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
		/// The minimum compression level
		/// </summary>
		private const int MIN_COMPRESSION_LEVEL = 0;

		/// <summary>
		/// The maximum compression level
		/// </summary>
		private const int MAX_COMPRESSION_LEVEL = 11;

		/// <summary>
		/// Compression level
		/// </summary>
		private int _level;

		/// <summary>
		/// Gets or sets a compression level
		/// </summary>
		public int Level
		{
			get { return _level; }
			set
			{
				if (value < MIN_COMPRESSION_LEVEL || value > MAX_COMPRESSION_LEVEL)
				{
					throw new ArgumentOutOfRangeException(
						nameof(value),
						string.Format(Strings.CompressionLevelOutOfRange,
							MIN_COMPRESSION_LEVEL, MAX_COMPRESSION_LEVEL)
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
			Level = 5;
		}
	}
}