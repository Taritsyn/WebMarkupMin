Change log
==========

## v2.13.1 - November 9, 2022
 * In WebMarkupMin.AspNetCore3 added support of ASP.NET Core 3.1.31
 * In WebMarkupMin.AspNetCore6 added support of ASP.NET Core 6.0.11
 * In WebMarkupMin.AspNetCore7 added support of ASP.NET Core 7.0.0

## v2.13.0 - November 4, 2022
 * `Info` method of `LoggerBase` class is marked as virtual
 * In WebMarkupMin.AspNetCore3 added support of ASP.NET Core 3.1.30
 * In WebMarkupMin.AspNetCore6 added support of ASP.NET Core 6.0.10
 * In ASP.NET Core extensions was made refactoring
 * In extensions for ASP.NET Core 3.1 or higher:
   * Fixed a error “Headers are read-only, response has already started”
   * Fixed a error that caused an blank page response when using the Brotli compression
 * Created the WebMarkupMin.AspNetCore7 module, that contains middleware for ASP.NET Core 7 RC2

## v2.12.0 - August 22, 2022
 * In HTML, XHTML and XML minification settings was added two new properties: `PreserveNewLines` (default `false`) and `NewLineStyle` (default `Auto`)
 * LazyHTML wrapped fragments are now parsed correctly
 * Location of the error that occurs during minification of JSON data are now calculated correctly
 * In WebMarkupMin.Yui a JS error summary has been excluded from the list of errors
 * In WebMarkupMin.NUglify added support of the NUglify version 1.20.2
 * In WebMarkupMin.AspNet.Common and WebMarkupMin.AspNetCore2 no longer supports a .NET Core App 2.1
 * In WebMarkupMin.AspNetCore3 added support of ASP.NET Core 3.1.28
 * In WebMarkupMin.AspNetCore5 added support of ASP.NET Core 5.0.17
 * In WebMarkupMin.AspNetCore6 added support of ASP.NET Core 6.0.8

## v2.11.0 - November 8, 2021
 * In WebMarkupMin.NUglify added support of the NUglify version 1.16.1
 * In WebMarkupMin.AspNetCore3 added support of ASP.NET Core 3.1.21
 * In WebMarkupMin.AspNetCore5 added support of ASP.NET Core 5.0.12
 * Created the WebMarkupMin.AspNetCore6 module, that contains middleware for ASP.NET Core 6

## v2.10.0 - May 18, 2021
 * In WebMarkupMin.NUglify added support of the NUglify version 1.13.10
 * In `IContentProcessingManager` interface, `ContentProcessingManagerBase` class and `ContentProcessingOptionsBase` class was added a new property - `SupportedHttpStatusCodes` (default `200`)
 * In WebMarkupMin.AspNetCore3 added support of ASP.NET Core 3.1.15
 * In WebMarkupMin.AspNetCore5 added support of ASP.NET Core 5.0.6

## v2.9.3 - April 27, 2021
 * In ASP.NET Core extensions, the `Services` property has been added to the `WebMarkupMinServicesBuilder` class. Special thanks to [Alex Rønne Petersen](https://github.com/alexrp).
 * In WebMarkupMin.NUglify added support of the NUglify version 1.13.8
 * In WebMarkupMin.AspNetCore3 added support of ASP.NET Core 3.1.14
 * In WebMarkupMin.AspNetCore5 added support of ASP.NET Core 5.0.5

## v2.9.2 - February 2, 2021
 * In WebMarkupMin.NUglify added support of the NUglify version 1.13.2
 * In WebMarkupMin.AspNetCore3 added support of ASP.NET Core 3.1.11
 * In WebMarkupMin.AspNetCore5 added support of ASP.NET Core 5.0.2

## v2.9.1 - December 4, 2020
 * In WebMarkupMin.Yui added support of the YUI Compressor for .NET version 3.1.0
 * In WebMarkupMin.NUglify added support of the NUglify version 1.11.4
 * In ASP.NET Core extensions fixed a error that occurred when excluding the Hangfire dashboard and related pages from processing by corresponding markup minifier or compressor

## v2.9.0 - December 1, 2020
 * In WebMarkupMin.Core:
   * .NET Core App 2.1 target was replaced by a .NET Standard 2.1 target
   * Version for .NET Standard 2.1 now uses a regular expression compilation for improving performance
 * In WebMarkupMin.NUglify added support of the NUglify version 1.11.3
 * In WebMarkupMin.AspNet.Common:
   * Added a .NET Core App 2.1 and .NET Standard 2.1 targets
   * In versions for .NET Core App 2.1 and .NET Standard 2.1 was added a new compressor (`BuiltInBrotliCompressor`) based on the `System.IO.Compression.BrotliStream` class
 * Created the WebMarkupMin.AspNetCore5 module, that contains middleware for ASP.NET Core 5.0

## v2.8.15 - November 13, 2020
 * In WebMarkupMin.NUglify:
   * Added support of the NUglify version 1.10.0
   * In configuration settings of minifiers was added one new property - `IndentType` (default `Space`)
 * In WebMarkupMin.AspNetCore3 added support of ASP.NET Core 3.1.10

## v2.8.14 - November 3, 2020
 * In WebMarkupMin.NUglify added support of the NUglify version 1.9.9
 * In WebMarkupMin.AspNetCore3 added support of ASP.NET Core 3.1.9

## v2.8.13 - October 10, 2020
 * In WebMarkupMin.NUglify added support of the NUglify version 1.9.7

## v2.8.12 - October 2, 2020
 * In WebMarkupMin.NUglify added support of the NUglify version 1.9.6

## v2.8.11 - September 24, 2020
 * In WebMarkupMin.NUglify added support of the NUglify version 1.9.5
 * In WebMarkupMin.AspNetCore2 updated a dependencies
 * In WebMarkupMin.AspNetCore3 added support of ASP.NET Core 3.1.8

## v2.8.10 - August 19, 2020
 * In WebMarkupMin.NUglify added support of the NUglify version 1.6.5

## v2.8.9 - August 15, 2020
 * In WebMarkupMin.AspNetCore3 added support of ASP.NET Core 3.1.7

## v2.8.8 - July 19, 2020
 * In WebMarkupMin.AspNetCore3 added support of ASP.NET Core 3.1.6

## v2.8.7 - July 8, 2020
 * In WebMarkupMin.NUglify added support of the NUglify version 1.6.4

## v2.8.6 - July 3, 2020
 * Improved a performance of the class directive processing

## v2.8.5 - June 30, 2020
 * Fixed a [error #113](https://github.com/Taritsyn/WebMarkupMin/issues/113) “Problem with the content of the html attributes”

## v2.8.4 - June 17, 2020
 * In WebMarkupMin.NUglify added support of the NUglify version 1.6.3
 * In WebMarkupMin.AspNetCore3 added support of ASP.NET Core 3.1.5

## v2.8.3 - May 22, 2020
 * Blazor component markers (`<!--Blazor:…-->`) are no longer removed
 * In WebMarkupMin.AspNetCore3 added support of ASP.NET Core 3.1.4

## v2.8.2 - March 17, 2020
 * In WebMarkupMin.AspNetCore3 fixed a [error #105](https://github.com/Taritsyn/WebMarkupMin/issues/105) “2.8.1 Crashing Out”

## v2.8.1 - March 14, 2020
 * Fixed a [error #104](https://github.com/Taritsyn/WebMarkupMin/issues/104) “Dependency Issues in release 2.8.0”

## v2.8.0 - March 13, 2020
 * In WebMarkupMin.NUglify:
   * Added support of the NUglify version 1.5.14
   * In configuration settings of CSS minifier was added one new property - `DecodeEscapes` (default `true`)
 * In WebMarkupMin.AspNetCore3:
   * .NET Core App 3.0 target was updated to version 3.1
   * Added support of ASP.NET Core 3.1.1

## v2.7.1 - November 23, 2019
 * Added support of the Douglas Crockford's JSMin version of October 30, 2019
 * Douglas Crockford's JSMin is now processing a Angular binding expressions separately from JS code

## v2.7.0 - September 24, 2019
 * The empty `dir` attribute is no longer removed
 * The `<link charset="…">` attribute is no longer considered redundant
 * The following attributes are now considered redundant: `<button type="submit">`, `<form autocomplete="on">`, `<form enctype="application/x-www-form-urlencoded">`, `<img decoding="auto">`, `<textarea wrap="soft">` and `<track kind="subtitles">`
 * In WebMarkupMin.AspNetCore3 added support of ASP.NET Core 3.0

## v2.6.3 - September 17, 2019
 * In WebMarkupMin.AspNetCore3 added support of ASP.NET Core 3.0 RC 1

## v2.6.2 - September 8, 2019
 * In WebMarkupMin.AspNetCore3 added support of ASP.NET Core 3.0 Preview 9

## v2.6.1 - August 18, 2019
 * In WebMarkupMin.AspNetCore3 added support of ASP.NET Core 3.0 Preview 8

## v2.6.0 - July 30, 2019
 * Part of the auxiliary code was replaced by the [AdvancedStringBuilder](https://github.com/Taritsyn/AdvancedStringBuilder) library
 * Slightly improved performance of markup minification
 * Optimized a memory usage during generation of statistics
 * Enabled a SourceLink in NuGet packages
 * In WebMarkupMin.AspNet.Brotli added support of ASP.NET Core 3.0
 * In WebMarkupMin.AspNetCore1 and WebMarkupMin.AspNetCore2:
   * In WebMarkupMin options was added one new property - `DefaultEncoding` (default `Encoding.Default`)
   * Optimized a memory usage
 * Created the WebMarkupMin.AspNetCore3 module, that contains middleware for ASP.NET Core 3.0

## v2.5.9 - June 5, 2019
 * In WebMarkupMin.NUglify added support of the NUglify version 1.5.13

## v2.5.8 - May 29, 2019
 * Now CDATA sections are not removed from scripts and styles if they are inside XML-based tags
 * Added the ability to specify a custom short DOCTYPE (e.g. `<!DOCTYPE HTML>`, `<!doctype html>`, or `<!doctypehtml>`)
 * In HTML minification settings was added one new property - `CustomShortDoctype` (default empty string)
 * Improved performance of HTML/XHTML minification

## v2.5.7 - April 15, 2019
 * In markup minifiers, buffer is now flushed more frequently
 * Markup parsers and output writers have been refactored
 * Slightly improved performance of markup minification
 * Added support of JSON data minification in `script` tags with `application/json` and `application/ld+json` types
 * In HTML/XHTML minification settings was added one new property - `MinifyEmbeddedJsonData` (default `true`)

## v2.5.6 - March 7, 2019
 * Fixed a [error #73](https://github.com/Taritsyn/WebMarkupMin/issues/73) “HtmlMinifier.Minify hangs permanently”
 * Fixed a [error #77](https://github.com/Taritsyn/WebMarkupMin/issues/77) “HtmlMinifier.Minify throws InvalidOperationException”
 * Slightly improved performance of markup minification
 * Code for working with the output buffers was extracted from the markup minifiers into separate classes
 * In WebMarkupMin.AspNet.Brotli added support of the BrotliSharpLib version 0.3.3

## v2.5.5 - November 6, 2018
 * `StringBuilderPool` class has become public
 * Improved performance of attributes generation in XML minifier
 * In WebMarkupMin.MsAjax, WebMarkupMin.Yui and WebMarkupMin.NUglify improved performance of minifier-adapters
 * In WebMarkupMin.MsAjax and WebMarkupMin.NUglify in configuration settings of CSS and JS minifiers was added one new property - `WarningLevel` (default `0`)
 * In WebMarkupMin.Yui in configuration settings of JS minifier was added one new property - `WarningLevel` (default `0`)

## v2.5.4 - October 24, 2018
 * Fixed a error that occurred when removing quotes from attribute with an empty value
 * `RemoveEndingSemicolon` method of `Utils` class was renamed to the `RemoveEndingSemicolons` (implementation has also been changed)
 * Mads Kristensen's CSS minifier has been refactored
 * Improved performance of adapter for the Douglas Crockford's JS minifier

## v2.5.3 - October 13, 2018
 * Improved performance of markup minification
 * In WebMarkupMin.AspNet.Brotli:
   * Added support of the BrotliSharpLib version 0.3.1
   * Added strong name signing for assembly

## v2.5.2 - August 22, 2018
 * In WebMarkupMin.NUglify added support of the NUglify version 1.5.12

## v2.5.1 - August 16, 2018
 * Improved a performance of processing attribute values
 * Fixed a error that occurred when processing of the ignoring fragments of markup

## v2.5.0 - August 13, 2018
 * In WebMarkupMin.Core, WebMarkupMin.MsAjax, WebMarkupMin.Yui, WebMarkupMin.NUglify and WebMarkupMin.AspNet.Common modules added support of .NET Standard 2.0
 * In WebMarkupMin.Yui:
   * YUI Compressor for .NET was updated to version 3.0.0
   * Now requires .NET Framework 4.5.2 or greater
 * In ASP.NET extensions:
   * Now the `NullLogger` class is used as the default logger
   * In `IHttpCompressionManager` interface was added one new method - `TryCreateCompressor`
   * In `ICompressor` interface was added one new property - `SupportsFlush`
   * Now, by default, the GZip algorithm has a higher priority than the Deflate
   * Added module based on the [Brotli](https://en.wikipedia.org/wiki/Brotli) compression algorithm
 * In ASP.NET 4.X extensions added support of .NET Framework 4.5

## v2.4.5 - July 9, 2018
 * Fixed a DOCTYPE parsing error
 * React DOM component comments are no longer removed

## v2.4.4 - July 4, 2018
 * Fixed a [error #63](https://github.com/Taritsyn/WebMarkupMin/issues/63) “The middleware blocks content streaming”

## v2.4.3 - June 1, 2018
 * In WebMarkupMin.NUglify added support of the NUglify version 1.5.11

## v2.4.2 - August 16, 2017
 * Created the WebMarkupMin.AspNetCore2 module, that contains middleware for ASP.NET Core 2.0
 * In WebMarkupMin.NUglify added support of the NUglify version 1.5.8

## v2.4.1 - June 30, 2017
 * In WebMarkupMin.NUglify added support of the NUglify version 1.5.6

## v2.4.0 - May 7, 2017
 * Added support of .NET Core 1.0.4
 * HTML and XHTML minifiers now support processing of CDATA sections outside the `script` and `style` tags
 * In `IMarkupMinificationManager` interface, `MarkupMinificationOptionsBase` class, `IHttpCompressionManager` interface and `HttpCompressionOptions` class was added a new property - `SupportedHttpMethods` (default `GET`)
 * In WebMarkupMin.AspNet4.Mvc and WebMarkupMin.AspNet4.WebForms now, by default, only the `GET` requests are minified and compressed (this behavior can be changed by using the `SupportedHttpMethods` property)
 * In `IHttpCompressionManager` interface and `HttpCompressionOptions` class was added two new properties - `IncludedPages` (default empty list) and `ExcludedPages` (default empty list)
 * In ASP.NET 4.X Extensions fixed a error of filtering media-types, which led to incorrect usage of HTTP compression

## v2.3.0 - March 7, 2017
 * Downgraded .NET Framework version from 4.5.1 to 4.5
 * Added support of .NET Core 1.0.3
 * Fixed a [error #31](https://github.com/Taritsyn/WebMarkupMin/issues/31) “Perfomance is very slow when a HTML comment is inside a JavaScript block”
 * Fixed a error in `SourceCodeNavigator` class
 * In WebMarkupMin.NUglify added support of the NUglify version 1.5.5
 * In `IMarkupMinificationManager` interface and `MarkupMinificationOptionsBase` class was added a new property - `GenerateStatistics` (default `false`)
 * From `IHttpCompressionManager` interface was removed `IsSupportedMediaType` method
 * In `IHttpCompressionManager` interface and `HttpCompressionOptions` class was added a new property - `SupportedMediaTypePredicate` (default `null`)

## v2.2.5 - December 22, 2016
 * In WebMarkupMin.AspNetCore1 fixed a error due to which instead of the status code pages displayed an empty content

## v2.2.4 - November 26, 2016
 * Added the ability to ignore fragments of markup by using the ignoring comment tags (`<!--wmm:ignore--><!--/wmm:ignore-->`)

## v2.2.3 - November 23, 2016
 * All exceptions made serializable
 * Fixed a [error #21](https://github.com/Taritsyn/WebMarkupMin/issues/21) “Remove redundant attributes, except input”

## v2.2.2 - November 11, 2016
 * Fixed a [error #18](https://github.com/Taritsyn/WebMarkupMin/issues/18) “Why is SavedGzipInBytes a decimal?”
 * Fixed a [error #20](https://github.com/Taritsyn/WebMarkupMin/issues/20) “Adding WebMarkupMin with a ServiceStack .Net Core enabled project fails”
 * Added the ability to specify a level of GZip or Deflate compression (while available only in ASP.NET Core applications)

## v2.2.1 - September 30, 2016
 * In WebMarkupMin.AspNetCore1 fixed a error “Stream does not support writing”

## v2.2.0 - September 27, 2016
 * Downgraded .NET Framework version from 4.5.2 to 4.5.1
 * Added support of .NET Core 1.0.1
 * Fixed a [error #13](https://github.com/Taritsyn/WebMarkupMin/issues/13) “HttpCompression Not Checking for Already Compressed Content”
 * Fixed a [error #14](https://github.com/Taritsyn/WebMarkupMin/issues/14) “HttpCompression Algorithm Priority”
 * Fixed a [error #15](https://github.com/Taritsyn/WebMarkupMin/issues/15) “Check for Content-Length in response headers”

## v2.1.1 - September 7, 2016
 * Fixed a [error #12](https://github.com/Taritsyn/WebMarkupMin/issues/12) “HTTP modules cause forms button do post back error”

## v2.1.0 - July 19, 2016
 * In configuration settings of HTML/XHTML minifier was changed type of `ProcessableScriptTypeCollection` and `CustomAngularDirectiveCollection` properties from `IEnumerable<string>` to `ISet<string>`
 * In configuration settings of HTML minifier was changed type of `PreservableOptionalTagCollection` property from `IEnumerable<string>` to `ISet<string>`
 * In configuration settings of HTML/XHTML minifier was changed a default value of `ProcessableScriptTypeList` property from `""` to `"text/html"`
 * In `CrockfordJsMinifier` was optimized memory usage
 * In ASP.NET 4.X Extensions was changed a mechanism of using default instances of loggers, factories and managers

## v2.0.2 - July 12, 2016
 * Added module based on the [NUglify](https://github.com/trullock/NUglify)
 * In WebMarkupMin.MsAjax and WebMarkupMin.Yui was made refactoring

## v2.0.1 - July 9, 2016
 * Optimized memory usage
 * Fixed a [error #10](https://github.com/Taritsyn/WebMarkupMin/issues/10) “Crash parsing invalid comment block”

## v2.0.0 - June 28, 2016
 * Added support of .NET Core and ASP.NET Core 1.0 RTM
 * Was made refactoring

## v2.0.0 RC 9 - June 13, 2016
 * `rb` and `rtc` tags are now considered as optional end tags
 * In configuration settings of HTML minifier was added one new property - `PreservableOptionalTagList` (default is empty)
 * Fixed a [error #8](https://github.com/Taritsyn/WebMarkupMin/issues/8) “MarkupMinificationException when having nested SVG element inside an SVG”
 * Fixed a [error #9](https://github.com/Taritsyn/WebMarkupMin/issues/9) “&lt;div&gt;${{something}}&lt;/div&gt; incorrectly minified”

## v2.0.0 RC 8 - May 19, 2016
 * Added support of .NET Core and ASP.NET Core 1.0 RC 2
 * WebMarkupMin.AspNet5 package has been replaced by WebMarkupMin.AspNetCore1 package

## v2.0.0 RC 7 - May 10, 2016
 * Improved a safe whitespace minification mode

## v2.0.0 RC 6 - April 2, 2016
 * In configuration settings of HTML minifier was added one new property - `PreserveCase` (default `false`)
 * Fixed a errors that occurred when processing of Angular 2, Aurelia and Polymer templates

## v2.0.0 RC 5 - March 24, 2016
 * Fixed a [error #6](https://github.com/Taritsyn/WebMarkupMin/issues/6) “Incompatible with DeveloperExceptionPageMiddleware (ASP.NET 5)”

## v2.0.0 RC 4 - March 17, 2016
 * Fixed a [error #3](https://github.com/Taritsyn/WebMarkupMin/issues/3) “NullReferenceException with FileContentResult in ASP.vNext RC1”

## v2.0.0 RC 3 - February 23, 2016
 * Now in WebMarkupMin.AspNet4.Mvc and WebMarkupMin.AspNet4.WebForms the responses with status codes are not equal to 200 is not minified and compressed

## v2.0.0 RC 2 - December 5, 2015
 * Now during minification removes the byte order mark (BOM)

## v2.0.0 RC 1 - November 20, 2015
 * Added support of .NET Core and ASP.NET 5 RC 1

## v2.0.0 Beta 5 - October 17, 2015
 * Added support of ASP.NET 5 Beta 8

## v2.0.0 Beta 4 - September 4, 2015
 * Added support of ASP.NET 5 Beta 7

## v2.0.0 Beta 3 - September 1, 2015
 * In WebMarkupMin.AspNet4.Mvc fixed a error “Filtering is not allowed.”, that caused by joint usage of the WebMarkupMin's action filters and the nopCommerce's widgets

## v2.0.0 Beta 2 - August 24, 2015
 * Was made refactoring
 * In WebMarkupMin.AspNet4.Mvc now the `CompressContentAttribute` action filter can be applied to the controllers

## v2.0.0 Beta 1 - August 13, 2015
 * .NET Core Libraries (CoreFX) have been upgraded to stable versions
 * Added support of xUnit.net 2.1 Beta 4
 * Fixed a problems with the NuGet package restore

## v2.0.0 Alpha 2 - July 31, 2015
 * Added support of ASP.NET 5 Beta 6
 * Now during HTTP compression the deflate algorithm has a higher priority than the gzip algorithm

## v2.0.0 Alpha 1 - July 17, 2015
 * Removed dependency on `System.Configuration.dll` (no longer supported configuration by using the `Web.config` and `App.config` files)
 * In WebMarkupMin.Core package added support of DNX 4.5.1 and DNX Core 5.0
 * In WebMarkupMin.MsAjax and WebMarkupMin.Yui packages added support of DNX 4.5.1
 * WebMarkupMin.Web package was split into 2 packages: WebMarkupMin.AspNet4.Common and WebMarkupMin.AspNet4.HttpModules
 * WebMarkupMin.Mvc package has been replaced by WebMarkupMin.AspNet4.Mvc package
 * WebMarkupMin.WebForms package has been replaced by WebMarkupMin.AspNet4.WebForms package
 * Created WebMarkupMin.AspNet5 package, that contains middleware for ASP.NET 5
 * WebMarkupMin.ConfigurationIntelliSense package is no longer required for the current version of WebMarkupMin