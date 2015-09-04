using System.Web.Mvc;

using WebMarkupMin.AspNet4.Mvc;
using WebMarkupMin.Sample.Logic.Services;

namespace WebMarkupMin.Sample.AspNet4.Mvc4.Controllers
{
	public class HtmlMinifierController : Controller
	{
		private readonly HtmlMinificationService _minificationService;


		public HtmlMinifierController()
			: this(new HtmlMinificationService())
		{ }

		public HtmlMinifierController(HtmlMinificationService minificationService)
		{
			_minificationService = minificationService;
		}


		[HttpGet]
		[CompressContent]
		[MinifyHtml]
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