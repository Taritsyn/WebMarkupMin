using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Mvc;

using WebMarkupMin.AspNet4.Mvc;
using WebMarkupMin.Sample.AspNet4.Mvc4.Infrastructure.ActionResults;
using WebMarkupMin.Sample.Logic.Models;
using WebMarkupMin.Sample.Logic.Services;

namespace WebMarkupMin.Sample.AspNet4.Mvc4.Controllers
{
	public class HomeController : Controller
	{
		private readonly FileContentService _fileContentService;


		public HomeController()
			: this(new FileContentService(ConfigurationManager.AppSettings["webmarkupmin:Samples:TextContentDirectoryPath"]))
		{ }

		public HomeController(FileContentService fileContentService)
		{
			_fileContentService = fileContentService;
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
			Uri siteUrl = GetSiteUrl();
			var sitemapItems = new List<SitemapItem>
			{
				new SitemapItem(GetAbsoluteActionUrl(siteUrl, "home", "index"), null, SitemapChangeFrequency.Hourly, 0.9),
				new SitemapItem(GetAbsoluteActionUrl(siteUrl, "home", "minifiers"), null, SitemapChangeFrequency.Daily, 0.7),
				new SitemapItem(GetAbsoluteActionUrl(siteUrl, "html-minifier", "index"), null, SitemapChangeFrequency.Daily, 0.5),
				new SitemapItem(GetAbsoluteActionUrl(siteUrl, "xhtml-minifier", "index"), null, SitemapChangeFrequency.Daily, 0.5),
				new SitemapItem(GetAbsoluteActionUrl(siteUrl, "xml-minifier", "index"), null, SitemapChangeFrequency.Daily, 0.5),
				new SitemapItem(GetAbsoluteActionUrl(siteUrl, "home", "change-log"), null, SitemapChangeFrequency.Daily, 0.8),
				new SitemapItem(GetAbsoluteActionUrl(siteUrl, "home", "contact"), null, SitemapChangeFrequency.Weekly, 0.4)
			};
			var sitemap = new Sitemap(sitemapItems);

			return new XmlResult
			{
				Data = sitemap.GetXmlSitemapFormatter(),
				ContentType = "text/xml",
				ContentEncoding = Encoding.UTF8
			};
		}

		[NonAction]
		private Uri GetSiteUrl()
		{
			HttpRequestBase request = HttpContext.Request;
			Uri currrentUrl = request.Url;

			var uriBuilder = new UriBuilder();
			uriBuilder.Scheme = currrentUrl.Scheme;
			uriBuilder.Host = currrentUrl.Host;
			if (!currrentUrl.IsDefaultPort)
			{
				uriBuilder.Port = currrentUrl.Port;
			}

			return uriBuilder.Uri;
		}

		[NonAction]
		private Uri GetAbsoluteActionUrl(Uri siteUrl, string controllerName, string actionName)
		{
			string relativeUrl = Url.Action(actionName, controllerName);
			var absoluteUrl = new Uri(siteUrl, relativeUrl);

			return absoluteUrl;
		}
	}
}