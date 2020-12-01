Web Markup Minifier [![NuGet version](http://img.shields.io/nuget/v/WebMarkupMin.Core.svg)](https://www.nuget.org/packages/WebMarkupMin.Core/)  [![Download count](https://img.shields.io/nuget/dt/WebMarkupMin.Core.svg)](https://www.nuget.org/packages/WebMarkupMin.Core/)
===================

<img src="https://raw.githubusercontent.com/Taritsyn/WebMarkupMin/master/images/WebMarkupMin_Logo.png" width="440" height="86" alt="WebMarkupMin logo" />

The **Web Markup Minifier** (abbreviated WebMarkupMin) - a .NET library that contains a set of markup minifiers. The objective of this project is to improve the performance of web applications by reducing the size of HTML, XHTML and XML code.

WebMarkupMin absorbed the best of existing solutions from non-microsoft platforms: Juriy Zaytsev's [HTML Minifier](https://github.com/kangax/html-minifier) (written in JavaScript) and Sergiy Kovalchuk's [HtmlCompressor](https://github.com/serg472/htmlcompressor) (written in Java).

Minification of markup produces by removing extra whitespace, comments and redundant code (only for HTML and XHTML). In addition, HTML and XHTML minifiers supports the minification of CSS code from `style` tags and attributes, and minification of JavaScript code from `script` tags, event attributes and hyperlinks with `javascript:` protocol. WebMarkupMin.Core contains built-in JavaScript minifier based on the Douglas Crockford's [JSMin](https://github.com/douglascrockford/JSMin) and built-in CSS minifier based on the Mads Kristensen's [Efficient stylesheet minifier](https://madskristensen.net/blog/efficient-stylesheet-minification-in-c). The above mentioned minifiers produce only the most simple minifications of CSS and JavaScript code, but you can always install additional modules that support the more powerful algorithms of minification: WebMarkupMin.MsAjax (contains minifier-adapters for the [Microsoft Ajax Minifier](https://github.com/microsoft/ajaxmin)), WebMarkupMin.Yui (contains minifier-adapters for [YUI Compressor for .NET](https://github.com/YUICompressor-NET/YUICompressor.NET)) and WebMarkupMin.NUglify (contains minifier-adapters for the [NUglify](https://github.com/trullock/NUglify)).

Also supports minification of views of popular JavaScript template engines: [KnockoutJS](http://knockoutjs.com/), [Kendo UI MVVM](https://www.telerik.com/kendo-ui) and [AngularJS](https://angularjs.org/) 1.X.

In addition, there are several modules that integrate this library into ASP.NET: WebMarkupMin.AspNet4.HttpModules (for ASP.NET 4.X and ASP.NET Web Pages), WebMarkupMin.AspNet4.Mvc (for ASP.NET MVC 3, 4 or 5), WebMarkupMin.AspNet4.WebForms (for ASP.NET Web Forms 4.X), WebMarkupMin.AspNetCore1 (for ASP.NET Core 1.X) and WebMarkupMin.AspNetCore2 (for ASP.NET Core 2.X).

You can try WebMarkupMin in action and experiment with different minification settings live on the [WebMarkupMin Online](http://webmarkupmin.apphb.com/) site.

## NuGet Packages

### Core
 * [WebMarkupMin: Core](http://nuget.org/packages/WebMarkupMin.Core/) (supports .NET Framework 4.0 Client, .NET Framework 4.5, .NET Standard 1.3, .NET Standard 2.0 and .NET Standard 2.1)

### External JS and CSS minifiers
 * [WebMarkupMin: MS Ajax](http://nuget.org/packages/WebMarkupMin.MsAjax/) (supports .NET Framework 4.0 Client, .NET Framework 4.5 and .NET Standard 2.0)
 * [WebMarkupMin: YUI](http://nuget.org/packages/WebMarkupMin.Yui/) (supports .NET Framework 4.5.2 and .NET Standard 2.0)
 * [WebMarkupMin: NUglify](http://nuget.org/packages/WebMarkupMin.NUglify/) (supports .NET Framework 4.0 Client, .NET Framework 4.5, .NET Standard 1.3 and .NET Standard 2.0)

### ASP.NET Extensions
 * [WebMarkupMin: ASP.NET 4.X HTTP modules](http://nuget.org/packages/WebMarkupMin.AspNet4.HttpModules/) (supports .NET Framework 4.0 and .NET Framework 4.5)
 * [WebMarkupMin: ASP.NET 4.X MVC](http://nuget.org/packages/WebMarkupMin.AspNet4.Mvc/) (supports .NET Framework 4.0 and .NET Framework 4.5)
 * [WebMarkupMin: ASP.NET 4.X Web Forms](http://nuget.org/packages/WebMarkupMin.AspNet4.WebForms/) (supports .NET Framework 4.0 and .NET Framework 4.5)
 * [WebMarkupMin: ASP.NET Core 1.X](http://nuget.org/packages/WebMarkupMin.AspNetCore1/) (supports .NET Framework 4.5.1 and .NET Standard 1.3)
 * [WebMarkupMin: ASP.NET Core 2.X](http://nuget.org/packages/WebMarkupMin.AspNetCore2/) (supports .NET Standard 2.0 and .NET Core App 2.1)
 * [WebMarkupMin: ASP.NET Core 3.X](http://nuget.org/packages/WebMarkupMin.AspNetCore3/) (supports .NET Core App 3.1)
 * [WebMarkupMin: ASP.NET Core 5.X](http://nuget.org/packages/WebMarkupMin.AspNetCore5/) (supports .NET 5.0)
 * [WebMarkupMin: Brotli for ASP.NET](http://nuget.org/packages/WebMarkupMin.AspNet.Brotli/) (supports .NET Framework 4.0, .NET Framework 4.5, .NET Standard 1.3, .NET Standard 2.0 and .NET Standard 2.1)

### Unofficial modules
 * [Syku.WebMarkupMin.Config](https://www.nuget.org/packages/Syku.WebMarkupMin.Config/) (supports .NET Standard 2.0) by Michał Sykutera
 * [WebMarkupMin.Brotli](https://www.nuget.org/packages/WebMarkupMin.Brotli/) (supports .NET Standard 1.3) by Michał Sykutera

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
 * [Colorful.Cache.Web](https://www.nuget.org/packages/Colorful.Cache.Web)
 * [Colorful.CMS.Core](https://www.nuget.org/packages/Colorful.CMS.Core/)
 * [Constellation.Foundation.Mvc](https://github.com/sitecorerick/constellation-sitecore9) by Rick Cabral
 * [Constellation.Sitecore.Presentation.Mvc](https://github.com/sitecorerick/constellation.sitecore.presentation.mvc) by Rick Cabral
 * [Elect AspNetCore Useful Middlewares](https://github.com/topnguyen/Elect/tree/master/src/Web/Elect.Web.Middlewares) by Top Nguyen
 * [FAV Rocks](https://github.com/billbogaiv/fav-rocks) by Bill Boga
 * [File Sharing Application](http://bitbucket.org/Artur2/filesharingapplication) by Artur N
 * [Google Pagespeed Tools for NopCommerce](https://www.foxnetsoft.com/noppagespeedtools) by FoxNetSoft
 * [GrandNode](https://github.com/grandnode/grandnode)
 * [iTEAMConsulting.FormHandler](https://github.com/iteam-consulting/csharp-form-handler)
 * [Karambolo.AspNetCore.Bundling.WebMarkupMin](https://github.com/adams85/bundling)
 * [Lightweight NetCore MVC Template](https://marketplace.visualstudio.com/items?itemName=PabloMorelli.LightweightNetCoreMVCTemplate) by Pablo Morelli
 * [MiniBlog](https://github.com/madskristensen/MiniBlog) by Mads Kristensen
 * [Miniblog.Core](https://github.com/madskristensen/Miniblog.Core) by Mads Kristensen
 * [Minit](https://minit.codeplex.com/) by Joan Caron
 * [MissingCode.Umbraco.HtmlMinifier](https://our.umbraco.org/projects/developer-tools/html-minifier/) by Yakov Lebski
 * [nopCommerce](https://www.nopcommerce.com/)
 * [NopCommerce.HTMLOptimiser](https://github.com/tomvanenckevort/NopCommerce.HTMLOptimiser) by Tom van Enckevort
 * [Orchard HTML Minifier](https://github.com/JadeX/Orchard.HtmlMinifier) by Liam 'Xeevis' Aqil
 * [Razor Minification](https://github.com/guardrex/RazorMinification) by Luke Latham
 * [StaticWebHelper](https://github.com/madskristensen/StaticWebHelper) by Mads Kristensen
 * [Statiq Framework](https://statiq.dev/framework) (formerly known as [Wyam](http://wyam.io/))
 * [Web Essentials 2013](https://github.com/madskristensen/WebEssentials2013) by Mads Kristensen

### Websites
 * [ActivaERP.com](https://www.activaerp.com/)
 * [Adiban.ac.ir](http://adiban.ac.ir/)
 * [Aebis.com](http://aebis.com/)
 * [AgroCountry.com](https://agrocountry.com/)
 * [AspCore.net](http://aspcore.net/)
 * [ASP-MVC.ir](http://asp-mvc.ir/)
 * [AsteImmobili.it](https://www.asteimmobili.it/)
 * [Aztual.es](https://www.aztual.es/)
 * [BancoMercedes-Benz.com.br](http://bancomercedes-benz.com.br/)
 * [BillerudKorsnas.se](https://www.billerudkorsnas.se/)
 * [Bridge-Direct.com](https://www.bridge-direct.com/)
 * [Brusella.com.ar](http://brusella.com.ar/)
 * [BUET.ac.bd](http://www.buet.ac.bd/)
 * [CaixaGuissona.com](https://caixaguissona.com/)
 * [Carnival.com](https://www.carnival.com/)
 * [CEV.org.tr](http://www.cev.org.tr/)
 * [ChefAppliances.com.au](https://www.chefappliances.com.au/)
 * [CoastalRealestatePattaya.com](http://coastalrealestatepattaya.com/)
 * [Colachip.com](https://colachip.com/) by Nguyen Huu Thuan
 * [DamBeton.nl](http://www.dambeton.nl/)
 * [DBIS.edu.hk](http://dbis.edu.hk/)
 * [Dealtoday.vn](https://www.dealtoday.vn/)
 * [Deqor.com](https://www.deqor.com/)
 * [DProtein.com](https://www.dprotein.com/)
 * [eBarkhat.ac.ir](http://ebarkhat.ac.ir/)
 * [EcitServices.no](http://www.ecitservices.no/)
 * [e-damavandihe.ac.ir](http://e-damavandihe.ac.ir/)
 * [Elbek-Vejrup.dk](https://elbek-vejrup.dk/)
 * [EquitiGlobalMarkets.ae](http://www.equitiglobalmarkets.ae/)
 * [Eurocodes.dk](http://eurocodes.dk/)
 * [FashionFriends.com](https://www.fashionfriends.com/)
 * [Fastimport.uy](http://www.fastimport.uy/)
 * [FirmaRehberi.tv.tr](https://www.firmarehberi.tv.tr/)
 * [FMOutsource.com](http://www.fmoutsource.com/)
 * [FreeNetworkAnalyzer.com](http://freenetworkanalyzer.com/)
 * [FreeSerialAnalyzer.com](https://freeserialanalyzer.com/)
 * [Furthermore.Equinox.com](https://furthermore.equinox.com/)
 * [GatwickParking.co.uk](https://www.gatwickparking.co.uk/)
 * [Giant-Bicycles.com](https://www.giant-bicycles.com/)
 * [Giant-Cambridge.co.uk](http://www.giant-cambridge.co.uk/)
 * [Giant-Rutland.co.uk](http://www.giant-rutland.co.uk/)
 * [Gnatta.com](https://gnatta.com/)
 * [GoDream.se](https://godream.se/)
 * [GoGetIt.com.pa](https://www.gogetit.com.pa/)
 * [Guide.DStv.com](http://guide.dstv.com/)
 * [HHDSoftware.com](http://www.hhdsoftware.com/)
 * [HiHoliday.ir](http://hiholiday.ir/)
 * [HotelMedusa.eu](http://www.hotelmedusa.eu/)
 * [HotelVirgilio.it](http://www.hotelvirgilio.it/)
 * [HypeProxy.io](http://hypeproxy.io/)
 * [IMD.org](https://www.imd.org/)
 * [ImoRadar24.ro](https://www.imoradar24.ro/)
 * [Instat.gov.al](http://instat.gov.al/)
 * [Ioshirt.com](https://ioshirt.com/) by Nguyen Huu Thuan
 * [iranzaminedu.com](http://iranzaminedu.com/)
 * [iStaff.ru](http://istaff.ru/)
 * [KamKan.ru](https://www.kamkan.ru/)
 * [KeyToSteel.com](http://www.keytosteel.com/)
 * [KonyaSeker.com.tr](http://konyaseker.com.tr/)
 * [KwadrantGroep.nl](https://www.kwadrantgroep.nl/)
 * [LecomparateurAssurance.com](https://www.lecomparateurassurance.com/)
 * [LeOrchidee.it](http://www.leorchidee.it/)
 * [LikyaGardens.com](https://www.likyagardens.com/)
 * [Logix.dk](https://logix.dk/)
 * [LogixSuite.it](http://www.logixsuite.it/)
 * [Macingo.com](https://www.macingo.com/)
 * [MaiLinhExpress.vn](http://www.mailinhexpress.vn/)
 * [Medivir.se](http://www.medivir.se/)
 * [Mini-ielts.com](http://mini-ielts.com/)
 * [Momentum-Biking.com](https://www.momentum-biking.com/)
 * [Moulsford.com](https://www.moulsford.com/)
 * [Multicare.org.uk](https://multicare.org.uk/)
 * [NimbusItSolutions.com](http://www.nimbusitsolutions.com/)
 * [NoorMags.ir](https://www.noormags.ir/)
 * [NottinghamHigh.co.uk](http://www.nottinghamhigh.co.uk/)
 * [OGMods.net](http://ogmods.net/)
 * [Oldflix.com.br](https://oldflix.com.br/)
 * [Ontrack.com](https://www.ontrack.com/)
 * [OntrackDataRecovery.nl](https://www.ontrackdatarecovery.nl/)
 * [OOS.SDU.edu.tr](https://oos.sdu.edu.tr/)
 * [OplevelsesGaver.dk](https://www.oplevelsesgaver.dk/)
 * [OXXOshop.com](http://www.oxxoshop.com/)
 * [ParizanSanat.com](http://parizansanat.com/)
 * [Partners.1Gl.ru](http://partners.1gl.ru/)
 * [PatioLiving.com](https://www.patioliving.com/)
 * [Pavimenti-Web.it](https://www.pavimenti-web.it/)
 * [Permira.com](https://www.permira.com/)
 * [PisoTermico.com.br](http://pisotermico.com.br/)
 * [PlanetaKoles.ru](http://www.planetakoles.ru/)
 * [PortRegis.com](https://www.portregis.com/)
 * [PostRandomonium.com](http://postrandomonium.com/)
 * [PranaCrystals.com](https://www.pranacrystals.com/)
 * [PrepSpotLight.tv](http://prepspotlight.tv/)
 * [QuizPop.com.br](http://www.quizpop.com.br/)
 * [RAKS.com.tr](https://www.raks.com.tr/)
 * [Randommer.io](https://randommer.io/)
 * [raovat49.com](https://raovat49.com/)
 * [RattiAuto.it](http://rattiauto.it/)
 * [RecelInteractive.com](https://www.recelinteractive.com/)
 * [RefrigerationDiscount.co.uk](https://www.refrigerationdiscount.co.uk/)
 * [RevelDigital.com](https://www.reveldigital.com/)
 * [RosKvartal.ru](https://roskvartal.ru/)
 * [SahInnParadise.com.tr](http://sahinnparadise.com.tr/)
 * [SebaSuites.com.tr](http://sebasuites.com.tr/)
 * [Shooger.com](https://shooger.com/)
 * [Simpleman.life](https://simpleman.life/) by Nguyen Huu Thuan
 * [SkillGamesBoard.com](http://skillgamesboard.com/)
 * [SleepersInSeattle.com](http://www.sleepersinseattle.com/)
 * [SmartLeadersIAS.com](http://smartleadersias.com/)
 * [Smitma.com](https://www.smitma.com/)
 * [Songtradr.com](https://www.songtradr.com/)
 * [SpazioAste.it](https://www.spazioaste.it/)
 * [StranaGruzov.ru](http://stranagruzov.ru/)
 * [Sway.com](https://sway.com/)
 * [SwedishMatch.com](http://www.swedishmatch.com/)
 * [Synaeda.nl](http://www.synaeda.nl/)
 * [SynapseNet.ru](https://synapsenet.ru/)
 * [TecMundo.com.br](http://www.tecmundo.com.br/)
 * [Together.bg](http://together.bg)
 * [togofogo.com](http://www.togofogo.com/)
 * [ToolSlick.com](https://toolslick.com/)
 * [Toy.co.uk](https://www.toy.co.uk/)
 * [TrailerRentals.com.au](https://www.trailerrentals.com.au/)
 * [USdirectory.com](https://usdirectory.com/)
 * [VegaITSourcing.rs](https://www.vegaitsourcing.rs/)
 * [Vidal-Vidal.com](https://www.vidal-vidal.com/)
 * [ViettechSolution.com](https://www.viettechsolution.com/)
 * [VolareSystems.com](https://volaresystems.com/)
 * [WiesnKini.de](http://wiesnkini.de/)
 * [WooshAirportExtras.com](https://www.wooshairportextras.com/)
 * [XemLichAm.com](http://xemlicham.com/)
 * [xln.co.uk](https://www.xln.co.uk/)
 * [Zashirt.com](https://zashirt.com/) by Nguyen Huu Thuan
 * [Zchip.club](https://zchip.club/) by Nguyen Huu Thuan
 * [Zemana.com](https://www.zemana.com)
 * [ZkontrolujsiAuto.cz](https://www.zkontrolujsiauto.cz/)
 * [Zolv.com](https://www.zolv.com/)