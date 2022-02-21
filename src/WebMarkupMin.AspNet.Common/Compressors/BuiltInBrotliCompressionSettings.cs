#if NETSTANDARD2_1
using System.IO.Compression;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Brotli compression settings
	/// </summary>
	public sealed class BuiltInBrotliCompressionSettings
	{
		/// <summary>
		/// Gets or sets a compression level
		/// </summary>
		public CompressionLevel Level
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs an instance of the brotli compression settings
		/// </summary>
		public BuiltInBrotliCompressionSettings()
		{
			Level = CompressionLevel.Optimal;
		}
	}
}
#endif