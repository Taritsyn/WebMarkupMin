using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using WebMarkupMin.AspNet4.Common;

namespace WebMarkupMin.Sample.AspNet4.Mvc4
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			WebMarkupMinConfig.Configure(WebMarkupMinConfiguration.Instance);
		}
	}
}