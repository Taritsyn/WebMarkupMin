using Microsoft.Ajax.Utilities;
using MsOutputMode = Microsoft.Ajax.Utilities.OutputMode;
using MsBlockStart = Microsoft.Ajax.Utilities.BlockStart;

using WebMarkupMin.Core.Utilities;
using WmmOutputMode = WebMarkupMin.MsAjax.OutputMode;
using WmmBlockStart = WebMarkupMin.MsAjax.BlockStart;

namespace WebMarkupMin.MsAjax
{
	/// <summary>
	/// Base class for the Microsoft Ajax Minifier
	/// </summary>
	public abstract class MsAjaxMinifierBase
	{
		/// <summary>
		/// Maps a common minifier settings
		/// </summary>
		/// <param name="originalSettings">Original common minifier settings</param>
		/// <param name="settings">Common minifier settings</param>
		protected static void MapCommonSettings(CommonSettings originalSettings,
			MsAjaxCommonMinificationSettingsBase settings)
		{
			originalSettings.BlocksStartOnSameLine = Utils.GetEnumFromOtherEnum<WmmBlockStart, MsBlockStart>(
				settings.BlocksStartOnSameLine);
			originalSettings.IgnoreAllErrors = settings.IgnoreAllErrors;
			originalSettings.IgnoreErrorList = settings.IgnoreErrorList;
			originalSettings.IndentSize = settings.IndentSize;
			originalSettings.LineBreakThreshold = settings.LineBreakThreshold;
			originalSettings.OutputMode = Utils.GetEnumFromOtherEnum<WmmOutputMode, MsOutputMode>(
				settings.OutputMode);
			originalSettings.PreprocessorDefineList = settings.PreprocessorDefineList;
			originalSettings.TermSemicolons = settings.TermSemicolons;
		}
	}
}