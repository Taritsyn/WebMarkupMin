using AutoMapper;

using WebMarkupMin.Core;
using WebMarkupMin.Sample.Logic.Models;

namespace WebMarkupMin.Sample.Logic.Services
{
	public sealed class XhtmlMinificationService
	{
		private readonly CssMinifierFactory _cssMinifierFactory;

		private readonly JsMinifierFactory _jsMinifierFactory;


		static XhtmlMinificationService()
		{
			MappingInitializer.Initialize();
		}

#if NET40
		public XhtmlMinificationService()
			: this(CssMinifierFactory.Instance, JsMinifierFactory.Instance)
		{ }

#endif
		public XhtmlMinificationService(
			CssMinifierFactory cssMinifierFactory,
			JsMinifierFactory jsMinifierFactory)
		{
			_cssMinifierFactory = cssMinifierFactory;
			_jsMinifierFactory = jsMinifierFactory;
		}


		public XhtmlMinificationViewModel GetInitializationData()
		{
			var settings = Mapper.Map<XhtmlMinificationSettingsViewModel>(new XhtmlMinificationSettings());
			var model = new XhtmlMinificationViewModel
			{
				SourceCode = string.Empty,
				Settings = settings,
				Result = null
			};

			return model;
		}

		public XhtmlMinificationViewModel Minify(XhtmlMinificationViewModel model)
		{
			string cssMinifierName = model.Settings.CssMinifierName;
			string jsMinifierName = model.Settings.JsMinifierName;

			var settings = Mapper.Map<XhtmlMinificationSettings>(model.Settings);
			ICssMinifier cssMinifier = _cssMinifierFactory.CreateMinifier(cssMinifierName);
			IJsMinifier jsMinifier = _jsMinifierFactory.CreateMinifier(jsMinifierName);

			var xhtmlMinifier = new XhtmlMinifier(settings, cssMinifier, jsMinifier);
			var result = xhtmlMinifier.Minify(model.SourceCode, true);

			var modelSettings = model.Settings;
			modelSettings.PreservableHtmlCommentList = settings.PreservableHtmlCommentList;
			modelSettings.PreservableAttributeList = settings.PreservableAttributeList;
			modelSettings.ProcessableScriptTypeList = settings.ProcessableScriptTypeList;
			modelSettings.CustomAngularDirectiveList = settings.CustomAngularDirectiveList;

			model.Result = Mapper.Map<MarkupMinificationResultViewModel>(result);

			return model;
		}
	}
}