<img src="logo.png" width="440" height="86" alt="WebMarkupMin logo" />

The **Web Markup Minifier** (abbreviated WebMarkupMin) - a .NET library that contains a set of markup minifiers. The objective of this project is to improve the performance of web applications by reducing the size of HTML, XHTML and XML code.

WebMarkupMin absorbed the best of existing solutions from non-microsoft platforms: Juriy Zaytsev's [Experimental HTML Minifier](http://kangax.github.com/html-minifier/) (written in JavaScript) and Sergiy Kovalchuk's [HtmlCompressor](http://code.google.com/p/htmlcompressor/) (written in Java).

Minification of markup produces by removing extra whitespace, comments and redundant code (only for HTML and XHTML). In addition, HTML and XHTML minifiers supports the minification of CSS code from `style` tags and attributes, and minification of JavaScript code from `script` tags, event attributes and hyperlinks with `javascript:` protocol. WebMarkupMin.Core contains built-in JavaScript minifier based on the Douglas Crockford's [JSMin](http://github.com/douglascrockford/JSMin) and built-in CSS minifier based on the Mads Kristensen's [Efficient stylesheet minifier](http://madskristensen.net/post/efficient-stylesheet-minification-in-c). The above mentioned minifiers produce only the most simple minifications of CSS and JavaScript code, but you can always install additional modules that support the more powerful algorithms of minification: WebMarkupMin.MsAjax (contains minifier-adapters for the [Microsoft Ajax Minifier](http://ajaxmin.codeplex.com)) and WebMarkupMin.Yui (contains minifier-adapters for [YUI Compressor for .Net](http://github.com/PureKrome/YUICompressor.NET)).

Also supports minification of views of popular JavaScript template engines: [KnockoutJS](http://knockoutjs.com/), [Kendo UI MVVM](http://www.telerik.com/kendo-ui) and [AngularJS](http://angularjs.org/) 1.X.

In addition, there are several modules that integrate this library into ASP.NET: WebMarkupMin.AspNet4.HttpModules (for ASP.NET Core 4.X and ASP.NET Web Pages), WebMarkupMin.AspNet4.Mvc (for ASP.NET MVC 3, 4 or 5), WebMarkupMin.AspNet4.WebForms (for ASP.NET Web Forms 4.X) and WebMarkupMin.AspNet5 (for ASP.NET 5).

## NuGet Packages

### Core
 * [WebMarkupMin: Core](http://nuget.org/packages/WebMarkupMin.Core/2.0.0-beta4) (supports .NET Framework 4.X, DNX 4.5.1 and .NET Platform 5.0)

### External JS and CSS minifiers
 * [WebMarkupMin: MS Ajax](http://nuget.org/packages/WebMarkupMin.MsAjax/2.0.0-beta4) (supports .NET Framework 4.X and DNX 4.5.1)
 * [WebMarkupMin: YUI](http://nuget.org/packages/WebMarkupMin.Yui/2.0.0-beta4) (supports .NET Framework 4.X and DNX 4.5.1)

### ASP.NET Extensions
 * [WebMarkupMin: ASP.NET 4.X HTTP modules](http://nuget.org/packages/WebMarkupMin.AspNet4.HttpModules/2.0.0-beta4) (supports .NET Framework 4.X)
 * [WebMarkupMin: ASP.NET 4.X MVC](http://nuget.org/packages/WebMarkupMin.AspNet4.Mvc/2.0.0-beta4) (supports .NET Framework 4.X)
 * [WebMarkupMin: ASP.NET 4.X Web Forms](http://nuget.org/packages/WebMarkupMin.AspNet4.WebForms/2.0.0-beta4) (supports .NET Framework 4.X)
 * [WebMarkupMin: ASP.NET 5](http://nuget.org/packages/WebMarkupMin.AspNet5/2.0.0-beta4) (supports DNX 4.5.1 and DNX Core 5.0)

Requires NuGet Package Manager version 2.8.6 or higher.

## Documentation
Documentation is located on the [wiki](http://github.com/Taritsyn/WebMarkupMin/wiki) of this Repo.

## Mailing List 
If you want to get information about new releases, then join to the [WebMarkupMin group](https://groups.google.com/forum/#!forum/webmarkupmin) on Google Groups.