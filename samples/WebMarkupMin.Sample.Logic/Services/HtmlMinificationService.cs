using AutoMapper;

using WebMarkupMin.Core;
using WebMarkupMin.Sample.Logic.Models;

namespace WebMarkupMin.Sample.Logic.Services
{
	public sealed class HtmlMinificationService
	{
		private readonly CssMinifierFactory _cssMinifierFactory;

		private readonly JsMinifierFactory _jsMinifierFactory;


		static HtmlMinificationService()
		{
			MappingInitializer.Initialize();
		}

#if NET40
		public HtmlMinificationService()
			: this(CssMinifierFactory.Instance, JsMinifierFactory.Instance)
		{ }

#endif
		public HtmlMinificationService(
			CssMinifierFactory cssMinifierFactory,
			JsMinifierFactory jsMinifierFactory)
		{
			_cssMinifierFactory = cssMinifierFactory;
			_jsMinifierFactory = jsMinifierFactory;
		}


		public HtmlMinificationViewModel GetInitializationData()
		{
			var settings = Mapper.Map<HtmlMinificationSettingsViewModel>(new HtmlMinificationSettings());
			var model = new HtmlMinificationViewModel
			{
				SourceCode = string.Empty,
				Settings = settings,
				Result = null
			};

			return model;
		}

		public HtmlMinificationViewModel Minify(HtmlMinificationViewModel model)
		{
			string cssMinifierName = model.Settings.CssMinifierName;
			string jsMinifierName = model.Settings.JsMinifierName;

			var settings = Mapper.Map<HtmlMinificationSettings>(model.Settings);
			ICssMinifier cssMinifier = _cssMinifierFactory.CreateMinifier(cssMinifierName);
			IJsMinifier jsMinifier = _jsMinifierFactory.CreateMinifier(jsMinifierName);

			var htmlMinifier = new HtmlMinifier(settings, cssMinifier, jsMinifier);
			var result = htmlMinifier.Minify(model.SourceCode, true);

			var modelSettings = model.Settings;
			modelSettings.PreservableHtmlCommentList = settings.PreservableHtmlCommentList;
			modelSettings.PreservableOptionalTagList = settings.PreservableOptionalTagList;
			modelSettings.PreservableAttributeList = settings.PreservableAttributeList;
			modelSettings.ProcessableScriptTypeList = settings.ProcessableScriptTypeList;
			modelSettings.CustomAngularDirectiveList = settings.CustomAngularDirectiveList;

			model.Result = Mapper.Map<MarkupMinificationResultViewModel>(result);

			return model;
		}
	}
}