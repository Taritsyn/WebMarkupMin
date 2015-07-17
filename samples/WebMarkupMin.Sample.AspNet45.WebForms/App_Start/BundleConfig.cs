using System.Web.Optimization;

using BundleTransformer.Core.Bundles;
using BundleTransformer.Core.Orderers;

namespace WebMarkupMin.Sample.AspNet45.WebForms
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.UseCdn = true;

			var nullOrderer = new NullOrderer();

			var commonStylesBundle = new CustomStyleBundle("~/Bundles/CommonStyles");
			commonStylesBundle.Include("~/Content/app.less");
			commonStylesBundle.Orderer = nullOrderer;

			bundles.Add(commonStylesBundle);

			var modernizrBundle = new CustomScriptBundle("~/Bundles/Modernizr");
			modernizrBundle.Include("~/Scripts/modernizr-2.*");
			modernizrBundle.Orderer = nullOrderer;

			bundles.Add(modernizrBundle);

			var jQueryBundle = new CustomScriptBundle("~/Bundles/Jquery",
				"http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.10.2.min.js");
			jQueryBundle.Include("~/Scripts/jquery-{version}.js");
			jQueryBundle.Orderer = nullOrderer;
			jQueryBundle.CdnFallbackExpression = "window.jquery";

			bundles.Add(jQueryBundle);

			var commonScriptsBundle = new CustomScriptBundle("~/Bundles/CommonScripts");
			commonScriptsBundle.Include("~/Scripts/bootstrap.js");
			commonScriptsBundle.Orderer = nullOrderer;

			bundles.Add(commonScriptsBundle);
		}
	}
}