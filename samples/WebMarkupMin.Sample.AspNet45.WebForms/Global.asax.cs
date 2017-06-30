using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

using JavaScriptEngineSwitcher.Core;

using WebMarkupMin.AspNet4.Common;

namespace WebMarkupMin.Sample.AspNet45.WebForms
{
	public class Global : HttpApplication
	{
		void Application_Start(object sender, EventArgs e)
		{
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			WebMarkupMinConfig.Configure(WebMarkupMinConfiguration.Instance);
			JsEngineSwitcherConfig.Configure(JsEngineSwitcher.Instance);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		void Application_End(object sender, EventArgs e)
		{ }

		void Application_Error(object sender, EventArgs e)
		{ }
	}
}