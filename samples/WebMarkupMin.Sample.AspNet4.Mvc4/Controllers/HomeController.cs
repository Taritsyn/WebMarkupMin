using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Web.Mvc;

using WebMarkupMin.AspNet4.Mvc;
using WebMarkupMin.Sample.Logic.Models;
using WebMarkupMin.Sample.Logic.Services;

namespace WebMarkupMin.Sample.AspNet4.Mvc4.Controllers
{
	public class HomeController : Controller
	{
		private readonly FileContentService _fileContentService;

		private readonly SitemapService _sitemapService;


		public HomeController()
			: this(
				new FileContentService(ConfigurationManager.AppSettings["webmarkupmin:Samples:TextContentDirectoryPath"]),
				new SitemapService()
			)
		{ }

		public HomeController(FileContentService fileContentService, SitemapService sitemapService)
		{
			_fileContentService = fileContentService;
			_sitemapService = sitemapService;
		}


		[CompressContent]
		[MinifyHtml]
		[OutputCache(CacheProfile = "CacheCompressedContent5Minutes")]
		public ActionResult Index()
		{
			ViewBag.Body = _fileContentService.GetFileContent("index.html");

			return View();
		}

		[CompressContent]
		[MinifyHtml]
		[OutputCache(CacheProfile = "CacheCompressedContent5Minutes")]
		public ActionResult Minifiers()
		{
			return View();
		}

		[CompressContent]
		[MinifyHtml]
		[OutputCache(CacheProfile = "CacheCompressedContent5Minutes")]
		public ActionResult ChangeLog()
		{
			ViewBag.Body = _fileContentService.GetFileContent("change-log.html");

			return View();
		}

		[CompressContent]
		[MinifyXhtml]
		[OutputCache(CacheProfile = "CacheCompressedContent5Minutes")]
		public ActionResult Contact()
		{
			ViewBag.Body = _fileContentService.GetFileContent("contact.html");

			return View();
		}

		[CompressContent]
		[MinifyXml]
		[OutputCache(CacheProfile = "CacheCompressedContent5Minutes")]
		public ActionResult Sitemap()
		{
			var sitemapItems = new List<SitemapItem>
			{
				new SitemapItem(GetAbsoluteUrl("home", "index"), null, SitemapChangeFrequency.Hourly, 0.9),
				new SitemapItem(GetAbsoluteUrl("home", "minifiers"), null, SitemapChangeFrequency.Daily, 0.7),
				new SitemapItem(GetAbsoluteUrl("html-minifier", "index"), null, SitemapChangeFrequency.Daily, 0.5),
				new SitemapItem(GetAbsoluteUrl("xhtml-minifier", "index"), null, SitemapChangeFrequency.Daily, 0.5),
				new SitemapItem(GetAbsoluteUrl("xml-minifier", "index"), null, SitemapChangeFrequency.Daily, 0.5),
				new SitemapItem(GetAbsoluteUrl("home", "change-log"), null, SitemapChangeFrequency.Daily, 0.8),
				new SitemapItem(GetAbsoluteUrl("home", "contact"), null, SitemapChangeFrequency.Weekly, 0.4)
			};

			XDocument xmlSitemap = _sitemapService.GenerateXmlSiteMap(sitemapItems);

			return new ContentResult
			{
				Content = xmlSitemap.ToString(),
				ContentType = "text/xml",
				ContentEncoding = Encoding.UTF8
			};
		}

		[NonAction]
		private string GetAbsoluteUrl(string controllerName, string actionName)
		{
			var urlHelper = new UrlHelper(ControllerContext.RequestContext);
			string url = urlHelper.Action(actionName, controllerName);

			string absoluteUrl = string.Empty;
			if (url != null)
			{
				Uri baseUri = Request.Url;
				Uri absoluteUri = (baseUri != null) ? new Uri(baseUri, url) : new Uri(url);
				absoluteUrl = absoluteUri.Scheme + Uri.SchemeDelimiter +
					absoluteUri.Host +
					(absoluteUri.IsDefaultPort ? string.Empty : ":" + absoluteUri.Port) +
					absoluteUri.PathAndQuery;
			}

			return absoluteUrl;
		}
	}
}