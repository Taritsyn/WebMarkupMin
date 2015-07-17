using System.Web.Mvc;
using System.Web.Routing;

using WebMarkupMin.Sample.AspNet4.Mvc4.Controllers;
using WebMarkupMin.Sample.AspNet4.Mvc4.Infrastructure.Routing;

namespace WebMarkupMin.Sample.AspNet4.Mvc4
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Sitemap",
				"sitemap.xml",
				new { controller = "Home", action = "Sitemap" }
			);

			var hyphenatedRouteHandler = new HyphenatedRouteHandler();

			routes.MapRoute(
				name: "Minifiers",
				url: "minifiers/{controller}/{action}",
				defaults: new { action = "Index" },
				constraints: new { controller = @"^(?:html|xhtml|xml)\-minifier$" }
			).RouteHandler = hyphenatedRouteHandler;

			routes.MapRoute(
				name: "Home",
				url: "{action}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
				constraints: new { isMethodInHomeController = new HyphenatedRootRouteConstraint<HomeController>() }
			).RouteHandler = hyphenatedRouteHandler;
		}
	}
}