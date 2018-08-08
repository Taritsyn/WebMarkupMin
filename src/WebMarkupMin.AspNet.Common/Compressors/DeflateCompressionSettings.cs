#if NET45 || NETSTANDARD
using System.IO.Compression;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Deflate compression settings
	/// </summary>
	public sealed class DeflateCompressionSettings
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
		/// Constructs an instance of the deflate compression settings
		/// </summary>
		public DeflateCompressionSettings()
		{
			Level = CompressionLevel.Optimal;
		}
	}
}
#endif