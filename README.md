Web Markup Minifier
===================

<img src="https://raw.githubusercontent.com/Taritsyn/WebMarkupMin/master/images/WebMarkupMin_Logo.png" width="440" height="86" alt="WebMarkupMin logo" />

The **Web Markup Minifier** (abbreviated WebMarkupMin) - a .NET library that contains a set of markup minifiers. The objective of this project is to improve the performance of web applications by reducing the size of HTML, XHTML and XML code.

WebMarkupMin absorbed the best of existing solutions from non-microsoft platforms: Juriy Zaytsev's [Experimental HTML Minifier](http://kangax.github.com/html-minifier/) (written in JavaScript) and Sergiy Kovalchuk's [HtmlCompressor](http://code.google.com/p/htmlcompressor/) (written in Java).

Minification of markup produces by removing extra whitespace, comments and redundant code (only for HTML and XHTML). In addition, HTML and XHTML minifiers supports the minification of CSS code from `style` tags and attributes, and minification of JavaScript code from `script` tags, event attributes and hyperlinks with `javascript:` protocol. WebMarkupMin.Core contains built-in JavaScript minifier based on the Douglas Crockford's [JSMin](https://github.com/douglascrockford/JSMin) and built-in CSS minifier based on the Mads Kristensen's [Efficient stylesheet minifier](http://madskristensen.net/post/efficient-stylesheet-minification-in-c). The above mentioned minifiers produce only the most simple minifications of CSS and JavaScript code, but you can always install additional modules that support the more powerful algorithms of minification: WebMarkupMin.MsAjax (contains minifier-adapters for the [Microsoft Ajax Minifier](https://ajaxmin.codeplex.com)), WebMarkupMin.Yui (contains minifier-adapters for [YUI Compressor for .Net](https://github.com/PureKrome/YUICompressor.NET)) and WebMarkupMin.NUglify (contains minifier-adapters for the [NUglify](https://github.com/xoofx/NUglify)).

Also supports minification of views of popular JavaScript template engines: [KnockoutJS](http://knockoutjs.com/), [Kendo UI MVVM](http://www.telerik.com/kendo-ui) and [AngularJS](http://angularjs.org/) 1.X.

In addition, there are several modules that integrate this library into ASP.NET: WebMarkupMin.AspNet4.HttpModules (for ASP.NET 4.X and ASP.NET Web Pages), WebMarkupMin.AspNet4.Mvc (for ASP.NET MVC 3, 4 or 5), WebMarkupMin.AspNet4.WebForms (for ASP.NET Web Forms 4.X) and WebMarkupMin.AspNetCore1 (for ASP.NET Core 1.X).

You can try WebMarkupMin in action and experiment with different minification settings live on the [WebMarkupMin Online](http://webmarkupmin.apphb.com/) site.

## NuGet Packages

### Core
 * [WebMarkupMin: Core](http://nuget.org/packages/WebMarkupMin.Core/) (supports .NET Framework 4.0 Client, .NET Framework 4.5 and .NET Standard 1.3)

### External JS and CSS minifiers
 * [WebMarkupMin: MS Ajax](http://nuget.org/packages/WebMarkupMin.MsAjax/) (supports .NET Framework 4.0 Client and .NET Framework 4.5)
 * [WebMarkupMin: YUI](http://nuget.org/packages/WebMarkupMin.Yui/) (supports .NET Framework 4.0 Client and .NET Framework 4.5)
 * [WebMarkupMin: NUglify](http://nuget.org/packages/WebMarkupMin.NUglify/) (supports .NET Framework 4.0 Client, .NET Framework 4.5 and .NET Standard 1.3)

### ASP.NET Extensions
 * [WebMarkupMin: ASP.NET 4.X HTTP modules](http://nuget.org/packages/WebMarkupMin.AspNet4.HttpModules/) (supports .NET Framework 4.0)
 * [WebMarkupMin: ASP.NET 4.X MVC](http://nuget.org/packages/WebMarkupMin.AspNet4.Mvc/) (supports .NET Framework 4.0)
 * [WebMarkupMin: ASP.NET 4.X Web Forms](http://nuget.org/packages/WebMarkupMin.AspNet4.WebForms/) (supports .NET Framework 4.0)
 * [WebMarkupMin: ASP.NET Core 1.X](http://nuget.org/packages/WebMarkupMin.AspNetCore1/) (supports .NET Framework 4.5.1 and .NET Standard 1.3)

Requires NuGet Package Manager version 2.8.6 or higher.

## Documentation
Documentation is located on the [wiki](https://github.com/Taritsyn/WebMarkupMin/wiki) of this Repo.

## Previous Versions
Source code and documentation for previous versions of WebMarkupMin are located on [CodePlex](https://webmarkupmin.codeplex.com/).
If you have used old versions of WebMarkupMin, then I recommend to first read [“How to upgrade applications to version 2.X”](https://github.com/Taritsyn/WebMarkupMin/wiki/How-to-upgrade-applications-to-version-2.X) section of the documentation.

## Who's Using WebMarkupMin
If you use WebMarkupMin in some project, please send me a message so I can include it in this list:

### Software
 * [AngularTemplates.Compile](https://github.com/vadimi/AngularTemplates.Compile) by Vadim Ivanou
 * [Blog-Umbraco](https://github.com/radyz/Blog-Umbraco) by Ernesto Chavez Sanchez
 * [Constellation.Sitecore.Presentation.Mvc](https://github.com/sitecorerick/constellation.sitecore.presentation.mvc) by Rick Cabral
 * [FAV Rocks](https://github.com/billbogaiv/fav-rocks) by Bill Boga
 * [File Sharing Application](http://bitbucket.org/Artur2/filesharingapplication) by Artur N
 * [iTEAMConsulting.FormHandler](https://github.com/iteam-consulting/csharp-form-handler)
 * [Orchard HTML Minifier](https://github.com/JadeX/Orchard.HtmlMinifier) by Liam 'Xeevis' Aqil
 * [MiniBlog](https://github.com/madskristensen/MiniBlog) by Mads Kristensen
 * [Minit](https://minit.codeplex.com/) by Joan Caron
 * [StaticWebHelper](https://github.com/madskristensen/StaticWebHelper) by Mads Kristensen
 * [Web Essentials 2013](https://github.com/madskristensen/WebEssentials2013) by Mads Kristensen
 * [Wyam](http://wyam.io/)

### Websites
 * [AllegheResort.it](http://www.allegheresort.it/)
 * [AutoThivolle.com](http://www.autothivolle.com/)
 * [Batdongsan24h.com.vn](http://batdongsan24h.com.vn/)
 * [BeckerDVP.nl](http://www.beckerdvp.nl/)
 * [BrightFuture.Unilever.com](https://brightfuture.unilever.com/)
 * [BTAssurance.com](http://www.btassurance.com/)
 * [Carot.vn](http://carot.vn/)
 * [ChaabiBank.co.uk](http://www.chaabibank.co.uk/)
 * [CoastalRealestatePattaya.com](http://coastalrealestatepattaya.com/)
 * [DamBeton.nl](http://www.dambeton.nl/)
 * [DBIS.edu.hk](http://dbis.edu.hk/)
 * [DocShell.ru](https://www.docshell.ru/)
 * [DProtein.com](https://www.dprotein.com/)
 * [EmResumo.com.br](http://www.emresumo.com.br/)
 * [E-Pacientas.lt](https://e-pacientas.lt)
 * [Fastimport.uy](http://www.fastimport.uy/)
 * [FAV.rocks](https://www.fav.rocks/) by Bill Boga
 * [FMOutsource.com](http://www.fmoutsource.com/)
 * [FreeNetworkAnalyzer.com](http://freenetworkanalyzer.com/)
 * [FreTor.com](http://www.fretor.com/)
 * [Futuromelhor.Unilever.com.br](https://futuromelhor.unilever.com.br/)
 * [Games.TecMundo.com.br](http://games.tecmundo.com.br/)
 * [GatwickParking.co.uk](https://www.gatwickparking.co.uk/)
 * [Gina.com](http://www.gina.com/)
 * [Gnatta.com](https://gnatta.com/)
 * [HHDSoftware.com](http://www.hhdsoftware.com/)
 * [HiHoliday.ir](http://hiholiday.ir/)
 * [HospOnline.ru](http://hosponline.ru/)
 * [iStaff.ru](http://istaff.ru/)
 * [Juhasz.cz](http://juhasz.cz/)
 * [KonyaSeker.com.tr](http://konyaseker.com.tr/)
 * [LogixSuite.it](http://www.logixsuite.it/)
 * [Macingo.com](https://www.macingo.com/)
 * [music2me.de](https://music2me.de/)
 * [NEI.com.br](http://www.nei.com.br/)
 * [nu.Faqtz.com](http://nu.faqtz.com/)
 * [OOS.SDU.edu.tr](https://oos.sdu.edu.tr/)
 * [Oostwoud.com](http://www.oostwoud.com/)
 * [Oostwoud.de](http://www.oostwoud.de/)
 * [Osmanlilar.gen.tr](http://osmanlilar.gen.tr/)
 * [Partners.1Gl.ru](http://partners.1gl.ru/)
 * [PisoTermico.com.br](http://pisotermico.com.br/)
 * [PlanetaKoles.ru](http://www.planetakoles.ru/)
 * [PostRandomonium.com](http://postrandomonium.com/)
 * [Quickportal.it](http://www.quickportal.it/)
 * [QuizPop.com.br](http://www.quizpop.com.br/)
 * [ReXposta.com.br](http://www.rexposta.com.br/tecnologia/)
 * [SkillGamesBoard.com](http://skillgamesboard.com/)
 * [SkyPrimeAv.com](http://skyprimeav.com/)
 * [SleepersInSeattle.com](http://www.sleepersinseattle.com/)
 * [Songtradr.com](https://www.songtradr.com/)
 * [Speak.nl](http://www.speak.nl/)
 * [Ster.nl](https://www.ster.nl/)
 * [StranaGruzov.ru](http://stranagruzov.ru/)
 * [SwedishMatch.com](http://www.swedishmatch.com/)
 * [TecMundo.com.br](http://www.tecmundo.com.br/)
 * [togofogo.com](http://www.togofogo.com/)
 * [TrailerRentals.com.au](https://www.trailerrentals.com.au/)
 * [Ujat.mx](http://ujat.mx/)
 * [Vindlov.se](http://www.vindlov.se/)
 * [VogueBodrum.com](http://voguebodrum.com/)
 * [WaNew.info](http://wanew.info/)
 * [WaNew.org](http://wanew.org/)
 * [WomensHealthNetwork.com](http://www.womenshealthnetwork.com/)
 * [WooshAirportExtras.com](https://www.wooshairportextras.com/)
 * [XemLichAm.com](http://xemlicham.com/)
 * [ZkontrolujsiAuto.cz](https://www.zkontrolujsiauto.cz/)
 * [Zolv.com](https://www.zolv.com/)