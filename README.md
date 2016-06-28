<img src="logo.png" width="440" height="86" alt="WebMarkupMin logo" />

The **Web Markup Minifier** (abbreviated WebMarkupMin) - a .NET library that contains a set of markup minifiers. The objective of this project is to improve the performance of web applications by reducing the size of HTML, XHTML and XML code.

WebMarkupMin absorbed the best of existing solutions from non-microsoft platforms: Juriy Zaytsev's [Experimental HTML Minifier](http://kangax.github.com/html-minifier/) (written in JavaScript) and Sergiy Kovalchuk's [HtmlCompressor](http://code.google.com/p/htmlcompressor/) (written in Java).

Minification of markup produces by removing extra whitespace, comments and redundant code (only for HTML and XHTML). In addition, HTML and XHTML minifiers supports the minification of CSS code from `style` tags and attributes, and minification of JavaScript code from `script` tags, event attributes and hyperlinks with `javascript:` protocol. WebMarkupMin.Core contains built-in JavaScript minifier based on the Douglas Crockford's [JSMin](http://github.com/douglascrockford/JSMin) and built-in CSS minifier based on the Mads Kristensen's [Efficient stylesheet minifier](http://madskristensen.net/post/efficient-stylesheet-minification-in-c). The above mentioned minifiers produce only the most simple minifications of CSS and JavaScript code, but you can always install additional modules that support the more powerful algorithms of minification: WebMarkupMin.MsAjax (contains minifier-adapters for the [Microsoft Ajax Minifier](http://ajaxmin.codeplex.com)) and WebMarkupMin.Yui (contains minifier-adapters for [YUI Compressor for .Net](http://github.com/PureKrome/YUICompressor.NET)).

Also supports minification of views of popular JavaScript template engines: [KnockoutJS](http://knockoutjs.com/), [Kendo UI MVVM](http://www.telerik.com/kendo-ui) and [AngularJS](http://angularjs.org/) 1.X.

In addition, there are several modules that integrate this library into ASP.NET: WebMarkupMin.AspNet4.HttpModules (for ASP.NET 4.X and ASP.NET Web Pages), WebMarkupMin.AspNet4.Mvc (for ASP.NET MVC 3, 4 or 5), WebMarkupMin.AspNet4.WebForms (for ASP.NET Web Forms 4.X) and WebMarkupMin.AspNetCore1 (for ASP.NET Core 1.X).

## NuGet Packages

### Core
 * [WebMarkupMin: Core](http://nuget.org/packages/WebMarkupMin.Core/) (supports .NET Framework 4.X and .NET Standard 1.3)

### External JS and CSS minifiers
 * [WebMarkupMin: MS Ajax](http://nuget.org/packages/WebMarkupMin.MsAjax/) (supports .NET Framework 4.X)
 * [WebMarkupMin: YUI](http://nuget.org/packages/WebMarkupMin.Yui/) (supports .NET Framework 4.X)

### ASP.NET Extensions
 * [WebMarkupMin: ASP.NET 4.X HTTP modules](http://nuget.org/packages/WebMarkupMin.AspNet4.HttpModules/) (supports .NET Framework 4.X)
 * [WebMarkupMin: ASP.NET 4.X MVC](http://nuget.org/packages/WebMarkupMin.AspNet4.Mvc/) (supports .NET Framework 4.X)
 * [WebMarkupMin: ASP.NET 4.X Web Forms](http://nuget.org/packages/WebMarkupMin.AspNet4.WebForms/) (supports .NET Framework 4.X)
 * [WebMarkupMin: ASP.NET Core 1.X](http://nuget.org/packages/WebMarkupMin.AspNetCore1/) (supports .NET Framework 4.5.X and .NET Standard 1.3)

Requires NuGet Package Manager version 2.8.6 or higher.

## Documentation
Documentation is located on the [wiki](https://github.com/Taritsyn/WebMarkupMin/wiki) of this Repo.

## Previous Versions
Source code and documentation for previous versions of WebMarkupMin are located on [CodePlex](http://webmarkupmin.codeplex.com/).
If you have used old versions of WebMarkupMin, then I recommend to first read [“How to upgrade applications to version 2.X”](https://github.com/Taritsyn/WebMarkupMin/wiki/How-to-upgrade-applications-to-version-2.X) section of the documentation.