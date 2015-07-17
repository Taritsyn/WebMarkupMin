using Yahoo.Yui.Compressor;

namespace WebMarkupMin.Yui
{
	/// <summary>
	/// Base class for the YUI Minifier
	/// </summary>
	public abstract class YuiMinifierBase
	{
		/// <summary>
		/// Applies a settings to code compressor
		/// </summary>
		/// <param name="compressor">Code compressor</param>
		/// <param name="commonSettings">Common settings of YUI Minifier</param>
		protected static void ApplyCommonSettingsToCompressor(Compressor compressor,
			YuiCommonMinificationSettingsBase commonSettings)
		{
			compressor.LineBreakPosition = commonSettings.LineBreakPosition;
		}
	}
}