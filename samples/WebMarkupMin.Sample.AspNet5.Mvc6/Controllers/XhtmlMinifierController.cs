using Microsoft.AspNet.Mvc;

using WebMarkupMin.Sample.Logic.Models;
using WebMarkupMin.Sample.Logic.Services;

namespace WebMarkupMin.Sample.AspNet5.Mvc6.Controllers
{
	[Route("minifiers/xhtml-minifier")]
    public class XhtmlMinifierController : Controller
    {
	    private readonly XhtmlMinificationService _minificationService;


	    public XhtmlMinifierController(XhtmlMinificationService minificationService)
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
		public IActionResult Index(XhtmlMinificationViewModel model)
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