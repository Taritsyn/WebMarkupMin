using System;
using System.Xml.Linq;
using System.Collections.Generic;

using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.Configuration;
using Microsoft.Net.Http.Headers;
using Microsoft.Framework.Runtime;

using WebMarkupMin.Sample.Logic.Models;
using WebMarkupMin.Sample.Logic.Services;

namespace WebMarkupMin.Sample.AspNet5Core.Mvc6.Controllers
{
    public class HomeController : Controller
    {
		private readonly FileContentService _fileContentService;

		private readonly SitemapService _sitemapService;


		public HomeController(
			IConfiguration configuration,
			IApplicationEnvironment applicationEnvironment,
			SitemapService sitemapService)
		{
			_fileContentService = new FileContentService(
				configuration.Get("webmarkupmin:Samples:TextContentDirectoryPath"),
				applicationEnvironment);
			_sitemapService = sitemapService;
		}


		public IActionResult Index()
        {
			ViewBag.Body = new HtmlString(_fileContentService.GetFileContent("index.html"));

            return View();
        }

		[Route("minifiers")]
		public IActionResult Minifiers()
		{
			return View();
		}

		[Route("change-log")]
		public IActionResult ChangeLog()
		{
			ViewBag.Body = new HtmlString(_fileContentService.GetFileContent("change-log.html"));

			return View();
		}

		[Route("contact")]
		public IActionResult Contact()
        {
			ViewBag.Body = new HtmlString(_fileContentService.GetFileContent("contact.html"));

			return View();
        }

		[Route("sitemap")]
		public IActionResult Sitemap()
		{
			var sitemapItems = new List<SitemapItem>
			{
				new SitemapItem(GetAbsoluteUrl("Home", "Index"), null, SitemapChangeFrequency.Hourly, 0.9),
				new SitemapItem(GetAbsoluteUrl("Home", "Minifiers"), null, SitemapChangeFrequency.Daily, 0.7),
				new SitemapItem(GetAbsoluteUrl("HtmlMinifier", "Index"), null, SitemapChangeFrequency.Daily, 0.5),
				new SitemapItem(GetAbsoluteUrl("XhtmlMinifier", "Index"), null, SitemapChangeFrequency.Daily, 0.5),
				new SitemapItem(GetAbsoluteUrl("XmlMinifier", "Index"), null, SitemapChangeFrequency.Daily, 0.5),
				new SitemapItem(GetAbsoluteUrl("Home", "Changelog"), null, SitemapChangeFrequency.Daily, 0.8),
				new SitemapItem(GetAbsoluteUrl("Home", "Contact"), null, SitemapChangeFrequency.Weekly, 0.4)
			};

			XDocument xmlSitemap = _sitemapService.GenerateXmlSiteMap(sitemapItems);

			return new ContentResult
			{
				Content = xmlSitemap.ToString(),
				ContentType = new MediaTypeHeaderValue("text/xml")
			};
		}

		[NonAction]
		private string GetAbsoluteUrl(string controllerName, string actionName)
		{
			IServiceProvider services = Context.RequestServices;

			var urlHelper = new UrlHelper(
				(IScopedInstance<ActionContext>)services.GetService(typeof(IScopedInstance<ActionContext>)),
				(IActionSelector)services.GetService(typeof(IActionSelector)));
			string url = urlHelper.Action(actionName, controllerName);

			string absoluteUrl = string.Empty;
			if (url != null)
			{
				HttpRequest request = Context.Request;
				absoluteUrl = request.Scheme + "://" + request.Host + url;
			}

			return absoluteUrl;
		}
	}
}