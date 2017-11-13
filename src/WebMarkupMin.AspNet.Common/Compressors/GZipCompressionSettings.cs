#if NETSTANDARD1_3 || NET45
using System.IO.Compression;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// GZip compression settings
	/// </summary>
	public sealed class GZipCompressionSettings
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
		/// Constructs an instance of the GZip compression settings
		/// </summary>
		public GZipCompressionSettings()
		{
			Level = CompressionLevel.Optimal;
		}
	}
}
#endif