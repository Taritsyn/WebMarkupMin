using Microsoft.AspNet.Mvc;

using WebMarkupMin.Sample.Logic.Models;
using WebMarkupMin.Sample.Logic.Services;

namespace WebMarkupMin.Sample.AspNet5Core.Mvc6.Controllers
{
	[Route("minifiers/xml-minifier")]
	public class XmlMinifierController : Controller
    {
	    private readonly XmlMinificationService _minificationService;


	    public XmlMinifierController(XmlMinificationService minificationService)
	    {
		    _minificationService = minificationService;
	    }


		[HttpGet]
		public IActionResult Index()
		{
			var model = _minificationService.GetInitializationData();

			return View(model);
		}

		[HttpPost]
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