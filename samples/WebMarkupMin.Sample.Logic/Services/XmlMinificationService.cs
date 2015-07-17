using AutoMapper;

using WebMarkupMin.Core;
using WebMarkupMin.Sample.Logic.Models;

namespace WebMarkupMin.Sample.Logic.Services
{
	public sealed class XmlMinificationService
	{
		static XmlMinificationService()
		{
			Mapper.CreateMap<XmlMinificationSettings, XmlMinificationSettingsViewModel>();
			Mapper.CreateMap<XmlMinificationSettingsViewModel, XmlMinificationSettings>();

			Mapper.CreateMap<MinificationStatistics, MinificationStatisticsViewModel>();
			Mapper.CreateMap<MarkupMinificationResult, MarkupMinificationResultViewModel>();
		}


		public XmlMinificationViewModel GetInitializationData()
		{
			var settings = Mapper.Map<XmlMinificationSettingsViewModel>(new XmlMinificationSettings());
			var model = new XmlMinificationViewModel
			{
				SourceCode = string.Empty,
				Settings = settings,
				Result = null
			};

			return model;
		}

		public XmlMinificationViewModel Minify(XmlMinificationViewModel model)
		{
			var settings = Mapper.Map<XmlMinificationSettings>(model.Settings);

			var xmlMinifier = new XmlMinifier(settings);
			var result = xmlMinifier.Minify(model.SourceCode, true);

			model.Result = Mapper.Map<MarkupMinificationResultViewModel>(result);

			return model;
		}
	}
}