using AutoMapper;

using WebMarkupMin.Core;
using WebMarkupMin.Core.Utilities;
using WebMarkupMin.Sample.Logic.Models;

namespace WebMarkupMin.Sample.Logic
{
	internal static class MappingInitializer
	{
		/// <summary>
		/// Flag that indicates if the mapper is initialized
		/// </summary>
		private static InterlockedStatedFlag _mapperInitializedFlag = new InterlockedStatedFlag();


		public static void Initialize()
		{
			if (_mapperInitializedFlag.Set())
			{
				Mapper.Initialize(configuration =>
				{
					configuration
						.CreateMap<HtmlMinificationSettings, HtmlMinificationSettingsViewModel>()
						.ReverseMap()
						;
					configuration
						.CreateMap<XhtmlMinificationSettings, XhtmlMinificationSettingsViewModel>()
						.ReverseMap()
						;
					configuration
						.CreateMap<XmlMinificationSettings, XmlMinificationSettingsViewModel>()
						.ReverseMap()
						;
					configuration.CreateMap<MarkupMinificationResult, MarkupMinificationResultViewModel>();
					configuration.CreateMap<MinificationStatistics, MinificationStatisticsViewModel>();
				});
			}
		}
	}
}