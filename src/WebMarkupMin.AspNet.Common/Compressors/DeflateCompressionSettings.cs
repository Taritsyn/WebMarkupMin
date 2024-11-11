#if NET45 || NETSTANDARD || NET9_0_OR_GREATER
namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// Deflate compression settings
	/// </summary>
	public sealed class DeflateCompressionSettings : ZLibCompressionSettingsBase
	{
		/// <summary>
		/// Constructs an instance of the deflate compression settings
		/// </summary>
		public DeflateCompressionSettings() : base()
		{ }
	}
}
#endif