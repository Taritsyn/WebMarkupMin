#if COMPRESSION_SETTINGS
namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Deflate compression settings
	/// </summary>
	public sealed class DeflateCompressionSettings : ZLibCompressionSettingsBase
	{
		/// <summary>
		/// Constructs an instance of the Deflate compression settings
		/// </summary>
		public DeflateCompressionSettings() : base()
		{ }
	}
}
#endif