using AutoMapper;

using WebMarkupMin.Core;
using WebMarkupMin.Sample.Logic.Models;

namespace WebMarkupMin.Sample.Logic
{
	internal static class MappingInitializer
	{
		private static readonly object _synchronizer = new object();
		private static bool _initialized;


		public static void Initialize()
		{
			if (!_initialized)
			{
				lock (_synchronizer)
				{
					if (!_initialized)
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

						_initialized = true;
					}
				}
			}
		}
	}
}