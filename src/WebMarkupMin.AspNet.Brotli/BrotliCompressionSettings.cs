using System;

namespace WebMarkupMin.AspNet.Brotli
{
	/// <summary>
	/// Brotli compression settings
	/// </summary>
	[Obsolete("Use a `LegacyBrotliCompressionSettings` class instead")]
	public sealed class BrotliCompressionSettings
	{
		/// <summary>
		/// Legacy Brotli compression settings
		/// </summary>
		private LegacyBrotliCompressionSettings _innerSettings = new();

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
				return _innerSettings.AlternativeLevel;
			}
			set
			{
				_innerSettings.AlternativeLevel = value;
			}
		}


		/// <summary>
		/// Gets a legacy Brotli compression settings
		/// </summary>
		/// <returns>Legacy Brotli compression settings</returns>
		internal LegacyBrotliCompressionSettings GetInnerSettings()
		{
			return _innerSettings;
		}
	}
}