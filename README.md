<img src="logo.png" width="440" height="86" alt="WebMarkupMin logo" />

The **Web Markup Minifier** (abbreviated WebMarkupMin) - a .NET library that contains a set of markup minifiers. The objective of this project is to improve the performance of web applications by reducing the size of HTML, XHTML and XML code.

WebMarkupMin absorbed the best of existing solutions from non-microsoft platforms: Juriy Zaytsev's [Experimental HTML Minifier](http://kangax.github.com/html-minifier/) (written in JavaScript) and Sergiy Kovalchuk's [HtmlCompressor](http://code.google.com/p/htmlcompressor/) (written in Java).

Minification of markup produces by removing extra whitespace, comments and redundant code (only for HTML and XHTML). In addition, HTML and XHTML minifiers supports the minification of CSS code from `style` tags and attributes, and minification of JavaScript code from `script` tags, event attributes and hyperlinks with `javascript:` protocol. WebMarkupMin.Core contains built-in JavaScript minifier based on the Douglas Crockford's [JSMin](http://github.com/douglascrockford/JSMin) and built-in CSS minifier based on the Mads Kristensen's [Efficient stylesheet minifier](http://madskristensen.net/post/efficient-stylesheet-minification-in-c). The above mentioned minifiers produce only the most simple minifications of CSS and JavaScript code, but you can always install additional modules that support the more powerful algorithms of minification: WebMarkupMin.MsAjax (contains minifier-adapters for the [Microsoft Ajax Minifier](http://ajaxmin.codeplex.com)) and WebMarkupMin.Yui (contains minifier-adapters for [YUI Compressor for .Net](http://github.com/PureKrome/YUICompressor.NET)).

Also supports minification of views of popular JavaScript template engines: [KnockoutJS](http://knockoutjs.com/), [Kendo UI MVVM](http://www.telerik.com/kendo-ui) and [AngularJS](http://angularjs.org/) 1.X.

In addition, there are several modules that integrate this library into ASP.NET: WebMarkupMin.AspNet4.HttpModules (for ASP.NET 4.X and ASP.NET Web Pages), WebMarkupMin.AspNet4.Mvc (for ASP.NET MVC 3, 4 or 5), WebMarkupMin.AspNet4.WebForms (for ASP.NET Web Forms 4.X) and WebMarkupMin.AspNetCore1 (for ASP.NET Core 1.X).

You can try WebMarkupMin in action and experiment with different minification settings live on the [WebMarkupMin Online](http://webmarkupmin.apphb.com/) site.

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

## Who's Using WebMarkupMin
If you use WebMarkupMin in some project, please send me a message so I can include it in this list:

### Software
 * [Blog-Umbraco](http://github.com/radyz/Blog-Umbraco) by Ernesto Chavez Sanchez
 * [Constellation.Sitecore.Presentation.Mvc](http://github.com/sitecorerick/constellation.sitecore.presentation.mvc) by Rick Cabral
 * [File Sharing Application](http://bitbucket.org/Artur2/filesharingapplication) by Artur N
 * [Html Markup Minifier](http://github.com/JadeX/Orchard.HtmlMinifier) (Orchard Module) by Liam 'Xeevis' Aqil
 * [Media Browser](http://github.com/MediaBrowser/MediaBrowser) by Luke Pulverenti
 * [MiniBlog](http://github.com/madskristensen/MiniBlog) by Mads Kristensen
 * [Minit](http://minit.codeplex.com/) by Joan Caron
 * [StaticWebHelper](http://github.com/madskristensen/StaticWebHelper) by Mads Kristensen
 * [Web Essentials 2013](http://github.com/madskristensen/WebEssentials2013) by Mads Kristensen
 * [Wyam](http://wyam.io/)

### Websites
 * [AutoThivolle.com](http://www.autothivolle.com/)
 * [BaixakiJogos.com.br](http://www.baixakijogos.com.br/)
 * [DamBeton.nl](http://www.dambeton.nl/)
 * [DocShell.ru](https://www.docshell.ru/)
 * [EmResumo.com.br](http://www.emresumo.com.br/)
 * [E-Pacientas.lt](https://e-pacientas.lt)
 * [Futuromelhor.Unilever.com.br](https://futuromelhor.unilever.com.br/)
 * [HHDSoftware.com](http://www.hhdsoftware.com/)
 * [HiHoliday.ir](http://hiholiday.ir/)
 * [HospOnline.ru](http://hosponline.ru/)
 * [iStaff.ru](http://istaff.ru/)
 * [KKBruce.tw](http://kkbruce.tw/)
 * [LogixSuite.it](http://www.logixsuite.it/)
 * [music2me.de](https://music2me.de/)
 * [NovalandGroup.net.vn](http://www.novalandgroup.net.vn/)
 * [nu.Faqtz.com](http://nu.faqtz.com/)
 * [Oostwoud.com](http://www.oostwoud.com/)
 * [Oostwoud.de](http://www.oostwoud.de/)
 * [Quickportal.it](http://www.quickportal.it/)
 * [ReXposta.com.br](http://www.rexposta.com.br/tecnologia/)
 * [ScoreYourBoss.com](http://www.scoreyourboss.com/)
 * [SkyPrimeAv.com](http://skyprimeav.com/)
 * [Speak.nl](http://www.speak.nl/)
 * [StranaGruzov.ru](http://stranagruzov.ru/)
 * [TecMundo.com.br](http://www.tecmundo.com.br/)
 * [togofogo.com](http://www.togofogo.com/)
 * [Ujat.mx](http://ujat.mx/)
 * [WomensHealthNetwork.com](http://www.womenshealthnetwork.com/)
 * [XemLichAm.com](http://xemlicham.com/)
 * [Zemana.com](http://zemana.com/)