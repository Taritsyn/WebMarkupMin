Change log
==========

## July 17, 2015 - v2.0.0 Alpha 1
 * Removed dependency on `System.Configuration.dll` (no longer supported configuration by using the `Web.config` and `App.config` files)
 * In WebMarkupMin.Core package added support of DNX 4.5.1 and DNX Core 5.0
 * In WebMarkupMin.MsAjax and WebMarkupMin.Yui packages added support of DNX 4.5.1
 * WebMarkupMin.Web package was split into 2 packages: WebMarkupMin.AspNet4.Common and WebMarkupMin.AspNet4.HttpModules
 * WebMarkupMin.Mvc package has been replaced by WebMarkupMin.AspNet4.Mvc package
 * WebMarkupMin.WebForms package has been replaced by WebMarkupMin.AspNet4.WebForms package
 * Created WebMarkupMin.AspNet5 package, that contains middleware for ASP.NET 5
 * WebMarkupMin.ConfigurationIntelliSense package is no longer required for the current version of WebMarkupMin