using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WebMarkupMin.Sample.AspNet4.Mvc4.Infrastructure.Routing
{
	public sealed class HyphenatedRootRouteConstraint<T> : IRouteConstraint
	{
		public bool Match(HttpContextBase httpContext, Route route, string parameterName,
			RouteValueDictionary values, RouteDirection routeDirection)
		{
			var rootMethodNames = typeof(T)
				.GetMethods()
				.Select(x => x.Name.ToLower())
				;

			return rootMethodNames.Contains(
				RouteHelpers.ProcessUrlPart(values["action"].ToString()));
		}
	}
}