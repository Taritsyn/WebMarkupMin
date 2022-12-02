using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;

using WebMarkupMin.Sample.AspNetCore.Infrastructure.ActionResults;
using WebMarkupMin.Sample.AspNetCore.Infrastructure.Extensions;
using WebMarkupMin.Sample.Logic.Models;
using WebMarkupMin.Sample.Logic.Services;

namespace WebMarkupMin.Sample.AspNetCore1Full.Mvc1.Controllers
{
	public class HomeController : Controller
	{
		private readonly FileContentService _fileContentService;


		public HomeController(
			IConfigurationRoot configuration,
			IHostingEnvironment hostingEnvironment)
		{
			string textContentDirectoryPath = configuration
				.GetSection("webmarkupmin")
				.GetSection("Samples")["TextContentDirectoryPath"]
				;

			_fileContentService = new FileContentService(textContentDirectoryPath, hostingEnvironment);
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
			Uri siteUrl = this.GetSiteUrl();
			var sitemapItems = new List<SitemapItem>
			{
				new SitemapItem(this.GetAbsoluteActionUrl(siteUrl, "Home", "Index"), null, SitemapChangeFrequency.Hourly, 0.9),
				new SitemapItem(this.GetAbsoluteActionUrl(siteUrl, "Home", "Minifiers"), null, SitemapChangeFrequency.Daily, 0.7),
				new SitemapItem(this.GetAbsoluteActionUrl(siteUrl, "HtmlMinifier", "Index"), null, SitemapChangeFrequency.Daily, 0.5),
				new SitemapItem(this.GetAbsoluteActionUrl(siteUrl, "XhtmlMinifier", "Index"), null, SitemapChangeFrequency.Daily, 0.5),
				new SitemapItem(this.GetAbsoluteActionUrl(siteUrl, "XmlMinifier", "Index"), null, SitemapChangeFrequency.Daily, 0.5),
				new SitemapItem(this.GetAbsoluteActionUrl(siteUrl, "Home", "Changelog"), null, SitemapChangeFrequency.Daily, 0.8),
				new SitemapItem(this.GetAbsoluteActionUrl(siteUrl, "Home", "Contact"), null, SitemapChangeFrequency.Weekly, 0.4)
			};
			var sitemap = new Sitemap(sitemapItems);

			return new XmlResult(sitemap.GetXmlSitemapFormatter())
			{
				ContentType = new MediaTypeHeaderValue("text/xml") { Encoding = Encoding.UTF8 }.ToString()
			};
		}

		[Route("error")]
		public IActionResult Error()
		{
			return View();
		}
	}
}