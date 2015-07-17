using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebMarkupMin.Sample.AspNet4.Mvc4.Infrastructure.Routing
{
	public sealed class HyphenatedRouteHandler : MvcRouteHandler
	{
		protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			var values = requestContext.RouteData.Values;
			values["controller"] = RouteHelpers.ProcessUrlPart(values["controller"].ToString());
			values["action"] = RouteHelpers.ProcessUrlPart(values["action"].ToString());

			return base.GetHttpHandler(requestContext);
		}
	}
}