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
		/// Maps a common settings
		/// </summary>
		/// <param name="commonParserSettings">Common parser settings</param>
		/// <param name="commonMinifierSettings">Common minifier settings</param>
		protected static void MapCommonSettings(CommonSettings commonParserSettings,
			MsAjaxCommonMinificationSettingsBase commonMinifierSettings)
		{
			commonParserSettings.BlocksStartOnSameLine = Utils.GetEnumFromOtherEnum<WmmBlockStart, MsBlockStart>(
				commonMinifierSettings.BlocksStartOnSameLine);
			commonParserSettings.IgnoreAllErrors = commonMinifierSettings.IgnoreAllErrors;
			commonParserSettings.IgnoreErrorList = commonMinifierSettings.IgnoreErrorList;
			commonParserSettings.IndentSize = commonMinifierSettings.IndentSize;
			commonParserSettings.LineBreakThreshold = commonMinifierSettings.LineBreakThreshold;
			commonParserSettings.OutputMode = Utils.GetEnumFromOtherEnum<WmmOutputMode, MsOutputMode>(
				commonMinifierSettings.OutputMode);
			commonParserSettings.PreprocessorDefineList = commonMinifierSettings.PreprocessorDefineList;
			commonParserSettings.TermSemicolons = commonMinifierSettings.TermSemicolons;
		}
	}
}