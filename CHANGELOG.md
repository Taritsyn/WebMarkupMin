Change log
==========

## May 10, 2016 - v2.0.0 RC 7
 * Improved a safe whitespace minification mode

## April 2, 2016 - v2.0.0 RC 6
 * In configuration settings of HTML minifier was added one new property - `PreserveCase` (default `false`)
 * Fixed a errors that occurred when processing of Angular 2, Aurelia and Polymer templates

## March 24, 2016 - v2.0.0 RC 5
 * Fixed a [error #6](https://github.com/Taritsyn/WebMarkupMin/issues/6) “Incompatible with DeveloperExceptionPageMiddleware (ASP.NET 5)”

## March 17, 2016 - v2.0.0 RC 4
 * Fixed a [error #3](https://github.com/Taritsyn/WebMarkupMin/issues/3) “NullReferenceException with FileContentResult in ASP.vNext RC1”

## February 23, 2016 - v2.0.0 RC 3
 * Now in WebMarkupMin.AspNet4.Mvc and WebMarkupMin.AspNet4.WebForms the responses with status codes are not equal to 200 is not minified and compressed

## December 5, 2015 - v2.0.0 RC 2
 * Now during minification removes the byte order mark (BOM)

## November 20, 2015 - v2.0.0 RC 1
 * Added support of .NET Core and ASP.NET 5 RC 1

## October 17, 2015 - v2.0.0 Beta 5
 * Added support of ASP.NET 5 Beta 8

## September 4, 2015 - v2.0.0 Beta 4
 * Added support of ASP.NET 5 Beta 7

## September 1, 2015 - v2.0.0 Beta 3
 * In WebMarkupMin.AspNet4.Mvc fixed a bug “Filtering is not allowed.”, that caused by joint usage of the WebMarkupMin's action filters and the nopCommerce's widgets

## August 24, 2015 - v2.0.0 Beta 2
 * Was made refactoring
 * In WebMarkupMin.AspNet4.Mvc now the `CompressContentAttribute` action filter can be applied to the controllers

## August 13, 2015 - v2.0.0 Beta 1
 * .NET Core Libraries (CoreFX) have been upgraded to stable versions
 * Added support of xUnit.net 2.1 Beta 4
 * Fixed a problems with the NuGet package restore

## July 31, 2015 - v2.0.0 Alpha 2
 * Added support of ASP.NET 5 Beta 6
 * Now during HTTP compression the deflate algorithm has a higher priority than the gzip algorithm

## July 17, 2015 - v2.0.0 Alpha 1
 * Removed dependency on `System.Configuration.dll` (no longer supported configuration by using the `Web.config` and `App.config` files)
 * In WebMarkupMin.Core package added support of DNX 4.5.1 and DNX Core 5.0
 * In WebMarkupMin.MsAjax and WebMarkupMin.Yui packages added support of DNX 4.5.1
 * WebMarkupMin.Web package was split into 2 packages: WebMarkupMin.AspNet4.Common and WebMarkupMin.AspNet4.HttpModules
 * WebMarkupMin.Mvc package has been replaced by WebMarkupMin.AspNet4.Mvc package
 * WebMarkupMin.WebForms package has been replaced by WebMarkupMin.AspNet4.WebForms package
 * Created WebMarkupMin.AspNet5 package, that contains middleware for ASP.NET 5
 * WebMarkupMin.ConfigurationIntelliSense package is no longer required for the current version of WebMarkupMin