#if COMPRESSION_SETTINGS
namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// GZip compression settings
	/// </summary>
	public sealed class GZipCompressionSettings : ZLibCompressionSettingsBase
	{
		/// <summary>
		/// Constructs an instance of the GZip compression settings
		/// </summary>
		public GZipCompressionSettings() : base()
		{ }
	}
}
#endif