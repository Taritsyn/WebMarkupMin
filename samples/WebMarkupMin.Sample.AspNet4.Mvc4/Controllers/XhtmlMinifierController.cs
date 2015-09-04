using System.Web.Mvc;

using WebMarkupMin.AspNet4.Mvc;
using WebMarkupMin.Sample.Logic.Services;

namespace WebMarkupMin.Sample.AspNet4.Mvc4.Controllers
{
	public class XhtmlMinifierController : Controller
	{
		private readonly XhtmlMinificationService _minificationService;


		public XhtmlMinifierController()
			:this(new XhtmlMinificationService())
		{ }

		public XhtmlMinifierController(XhtmlMinificationService minificationService)
		{
			_minificationService = minificationService;
		}


		[HttpGet]
		[CompressContent]
		[MinifyXhtml]
		[OutputCache(CacheProfile = "CacheCompressedContent5Minutes")]
		public ActionResult Index()
		{
			var model = _minificationService.GetInitializationData();

			return View(model);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Index(FormCollection collection)
		{
			var model = _minificationService.GetInitializationData();

			TryUpdateModel(model, new[] { "SourceCode", "Settings" }, collection);

			if (ModelState.IsValid)
			{
				model = _minificationService.Minify(model);

				ModelState.Clear();
			}

			return View(model);
		}
	}
}