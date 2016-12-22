using System.Xml.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;

using WebMarkupMin.Sample.Logic.Models;
using WebMarkupMin.Sample.Logic.Services;

namespace WebMarkupMin.Sample.AspNetCore1.Mvc1.Controllers
{
	public class HomeController : Controller
	{
		private readonly FileContentService _fileContentService;

		private readonly SitemapService _sitemapService;


		public HomeController(
			IConfigurationRoot configuration,
			IHostingEnvironment hostingEnvironment,
			SitemapService sitemapService)
		{
			string textContentDirectoryPath = configuration
				.GetSection("webmarkupmin")
				.GetSection("Samples")["TextContentDirectoryPath"]
				;

			_fileContentService = new FileContentService(textContentDirectoryPath, hostingEnvironment);
			_sitemapService = sitemapService;
		}


		[ResponseCache(CacheProfileName = "CacheCompressedContent5Minutes")]
		public IActionResult Index()
		{
			ViewBag.Body = new HtmlString(_fileContentService.GetFileContent("index.html"));

			return View();
		}

		[Route("minifiers")]
		[ResponseCache(CacheProfileName = "CacheCompressedContent5Minutes")]
		public IActionResult Minifiers()
		{
			return View();
		}

		[Route("change-log")]
		[ResponseCache(CacheProfileName = "CacheCompressedContent5Minutes")]
		public IActionResult ChangeLog()
		{
			ViewBag.Body = new HtmlString(_fileContentService.GetFileContent("change-log.html"));

			return View();
		}

		[Route("contact")]
		[ResponseCache(CacheProfileName = "CacheCompressedContent5Minutes")]
		public IActionResult Contact()
		{
			ViewBag.Body = new HtmlString(_fileContentService.GetFileContent("contact.html"));

			return View();
		}

		[Route("sitemap")]
		[ResponseCache(CacheProfileName = "CacheCompressedContent5Minutes")]
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
				ContentType = "text/xml"
			};
		}

		[Route("error")]
		public IActionResult Error()
		{
			return View();
		}

		[NonAction]
		private string GetAbsoluteUrl(string controllerName, string actionName)
		{
			ActionContext actionContext = Url.ActionContext;
			IUrlHelper urlHelper = new UrlHelper(actionContext);

			string url = urlHelper.Action(actionName, controllerName);
			string absoluteUrl = string.Empty;

			if (url != null)
			{
				HttpRequest request = actionContext.HttpContext.Request;
				absoluteUrl = request.Scheme + "://" + request.Host + url;
			}

			return absoluteUrl;
		}
	}
}