using Microsoft.AspNetCore.Mvc;

using WebMarkupMin.Sample.Logic.Models;
using WebMarkupMin.Sample.Logic.Services;

namespace WebMarkupMin.Sample.AspNetCore1Full.Mvc1.Controllers
{
	[Route("minifiers")]
	public class XmlMinifierController : Controller
	{
		private readonly XmlMinificationService _minificationService;


		public XmlMinifierController(XmlMinificationService minificationService)
		{
			_minificationService = minificationService;
		}


		[HttpGet]
		[Route("xml-minifier")]
		public IActionResult Index()
		{
			var model = _minificationService.GetInitializationData();

			return View(model);
		}

		[HttpPost]
		[Route("xml-minifier")]
		public IActionResult Index(XmlMinificationViewModel model)
		{
			if (ModelState.IsValid)
			{
				model = _minificationService.Minify(model);

				ModelState.Clear();
			}

			return View(model);
		}
	}
}