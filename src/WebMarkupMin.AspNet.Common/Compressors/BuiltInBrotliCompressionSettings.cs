#if NETSTANDARD2_1 || NET9_0_OR_GREATER
using System;
using System.IO.Compression;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Built-in Brotli compression settings
	/// </summary>
	[Obsolete("Use a `WebMarkupMin.AspNet.Common.Compressors.BrotliCompressionSettings` class instead")]
	public sealed class BuiltInBrotliCompressionSettings
#if NET9_0_OR_GREATER
		: CommonCompressionSettingsBase<BrotliCompressionOptions>
#else
		: CommonCompressionSettingsBase
#endif
	{
		/// <summary>
		/// Brotli compression settings
		/// </summary>
		private BrotliCompressionSettings _innerSettings = new();


		/// <inheritdoc/>
		public override CompressionLevel Level
		{
			get
			{
				return _innerSettings.Level;
			}
			set
			{
				_innerSettings.Level = value;
			}
		}
#if NET9_0_OR_GREATER

		/// <inheritdoc/>
		/// <remarks>
		/// The higher the level, the slower the compression. Range is from 0 to 11. The default value is 4.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">The value is less than 0 or greater than 11.</exception>
		public override int AlternativeLevel
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
#endif


		/// <summary>
		/// Gets a Brotli compression settings
		/// </summary>
		/// <returns>Brotli compression settings</returns>
		internal BrotliCompressionSettings GetInnerSettings()
		{
			return _innerSettings;
		}
	}
}
#endif