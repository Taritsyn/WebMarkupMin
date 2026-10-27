using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

using WebMarkupMin.Sample.AspNetCore11.Mvc11.Models;

namespace WebMarkupMin.Sample.AspNetCore11.Mvc11.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}