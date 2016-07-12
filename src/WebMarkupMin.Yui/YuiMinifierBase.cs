using Yahoo.Yui.Compressor;

namespace WebMarkupMin.Yui
{
	/// <summary>
	/// Base class for the YUI Minifier
	/// </summary>
	public abstract class YuiMinifierBase
	{
		/// <summary>
		/// Applies a common settings to original minifier
		/// </summary>
		/// <param name="originalMinifier">Original minifier</param>
		/// <param name="settings">Common minifier settings</param>
		protected static void ApplyCommonSettingsToOriginalMinifier(Compressor originalMinifier,
			YuiCommonMinificationSettingsBase settings)
		{
			originalMinifier.LineBreakPosition = settings.LineBreakPosition;
		}
	}
}