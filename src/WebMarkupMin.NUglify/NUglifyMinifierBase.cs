using NUglify;
using NuOutputMode = NUglify.OutputMode;
using NuBlockStart = NUglify.BlockStart;

using WebMarkupMin.Core.Utilities;
using WmmOutputMode = WebMarkupMin.NUglify.OutputMode;
using WmmBlockStart = WebMarkupMin.NUglify.BlockStart;

namespace WebMarkupMin.NUglify
{
	/// <summary>
	/// Base class for the NUglify Minifier
	/// </summary>
	public abstract class NUglifyMinifierBase
	{
		/// <summary>
		/// Maps a common minifier settings
		/// </summary>
		/// <param name="originalSettings">Original common minifier settings</param>
		/// <param name="settings">Common minifier settings</param>
		protected static void MapCommonSettings(CommonSettings originalSettings,
			NUglifyCommonMinificationSettingsBase settings)
		{
			originalSettings.BlocksStartOnSameLine = Utils.GetEnumFromOtherEnum<WmmBlockStart, NuBlockStart>(
				settings.BlocksStartOnSameLine);
			originalSettings.IgnoreAllErrors = settings.IgnoreAllErrors;
			originalSettings.IgnoreErrorList = settings.IgnoreErrorList;
			originalSettings.IndentSize = settings.IndentSize;
			originalSettings.LineBreakThreshold = settings.LineBreakThreshold;
			originalSettings.OutputMode = Utils.GetEnumFromOtherEnum<WmmOutputMode, NuOutputMode>(
				settings.OutputMode);
			originalSettings.PreprocessorDefineList = settings.PreprocessorDefineList;
			originalSettings.TermSemicolons = settings.TermSemicolons;
			originalSettings.WarningLevel = 0;
		}
	}
}