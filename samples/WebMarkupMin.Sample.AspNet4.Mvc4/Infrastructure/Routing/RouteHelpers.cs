namespace WebMarkupMin.Sample.AspNet4.Mvc4.Infrastructure.Routing
{
	internal static class RouteHelpers
	{
		public static string ProcessUrlPart(string urlPart)
		{
			return urlPart
				.Replace("-", string.Empty)
				.ToLowerInvariant()
				;
		}
	}
}