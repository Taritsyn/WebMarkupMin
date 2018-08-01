Web Markup Minifier [![NuGet version](http://img.shields.io/nuget/v/WebMarkupMin.Core.svg)](https://www.nuget.org/packages/WebMarkupMin.Core/)  [![Download count](https://img.shields.io/nuget/dt/WebMarkupMin.Core.svg)](https://www.nuget.org/packages/WebMarkupMin.Core/)
===================

<img src="https://raw.githubusercontent.com/Taritsyn/WebMarkupMin/master/images/WebMarkupMin_Logo.png" width="440" height="86" alt="WebMarkupMin logo" />

The **Web Markup Minifier** (abbreviated WebMarkupMin) - a .NET library that contains a set of markup minifiers. The objective of this project is to improve the performance of web applications by reducing the size of HTML, XHTML and XML code.

WebMarkupMin absorbed the best of existing solutions from non-microsoft platforms: Juriy Zaytsev's [Experimental HTML Minifier](https://github.com/kangax/html-minifier) (written in JavaScript) and Sergiy Kovalchuk's [HtmlCompressor](https://github.com/serg472/htmlcompressor) (written in Java).

Minification of markup produces by removing extra whitespace, comments and redundant code (only for HTML and XHTML). In addition, HTML and XHTML minifiers supports the minification of CSS code from `style` tags and attributes, and minification of JavaScript code from `script` tags, event attributes and hyperlinks with `javascript:` protocol. WebMarkupMin.Core contains built-in JavaScript minifier based on the Douglas Crockford's [JSMin](https://github.com/douglascrockford/JSMin) and built-in CSS minifier based on the Mads Kristensen's [Efficient stylesheet minifier](https://madskristensen.net/blog/efficient-stylesheet-minification-in-c). The above mentioned minifiers produce only the most simple minifications of CSS and JavaScript code, but you can always install additional modules that support the more powerful algorithms of minification: WebMarkupMin.MsAjax (contains minifier-adapters for the [Microsoft Ajax Minifier](https://ajaxmin.codeplex.com)), WebMarkupMin.Yui (contains minifier-adapters for [YUI Compressor for .Net](https://github.com/YUICompressor-NET/YUICompressor.NET)) and WebMarkupMin.NUglify (contains minifier-adapters for the [NUglify](https://github.com/xoofx/NUglify)).

Also supports minification of views of popular JavaScript template engines: [KnockoutJS](http://knockoutjs.com/), [Kendo UI MVVM](https://www.telerik.com/kendo-ui) and [AngularJS](https://angularjs.org/) 1.X.

In addition, there are several modules that integrate this library into ASP.NET: WebMarkupMin.AspNet4.HttpModules (for ASP.NET 4.X and ASP.NET Web Pages), WebMarkupMin.AspNet4.Mvc (for ASP.NET MVC 3, 4 or 5), WebMarkupMin.AspNet4.WebForms (for ASP.NET Web Forms 4.X), WebMarkupMin.AspNetCore1 (for ASP.NET Core 1.X) and WebMarkupMin.AspNetCore2 (for ASP.NET Core 2.X).

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
 * [WebMarkupMin: ASP.NET Core 2.X](http://nuget.org/packages/WebMarkupMin.AspNetCore2/) (supports .NET Standard 2.0)

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
 * [iTEAMConsulting.FormHandler](https://github.com/iteam-consulting/csharp-form-handler)
 * [Karambolo.AspNetCore.Bundling.WebMarkupMin](https://github.com/adams85/bundling)
 * [Lightweight NetCore MVC Template](https://marketplace.visualstudio.com/items?itemName=PabloMorelli.LightweightNetCoreMVCTemplate) by Pablo Morelli
 * [MiniBlog](https://github.com/madskristensen/MiniBlog) by Mads Kristensen
 * [Miniblog.Core](https://github.com/madskristensen/Miniblog.Core) by Mads Kristensen
 * [Minit](https://minit.codeplex.com/) by Joan Caron
 * [MissingCode.Umbraco.HtmlMinifier](https://our.umbraco.org/projects/developer-tools/html-minifier/) by Yakov Lebski
 * [NopCommerce.HTMLOptimiser](https://github.com/tomvanenckevort/NopCommerce.HTMLOptimiser) by Tom van Enckevort
 * [Orchard HTML Minifier](https://github.com/JadeX/Orchard.HtmlMinifier) by Liam 'Xeevis' Aqil
 * [Razor Minification](https://github.com/guardrex/RazorMinification) by Luke Latham
 * [StaticWebHelper](https://github.com/madskristensen/StaticWebHelper) by Mads Kristensen
 * [Web Essentials 2013](https://github.com/madskristensen/WebEssentials2013) by Mads Kristensen
 * [Wyam](http://wyam.io/)

### Websites
 * [ActivaERP.com](https://www.activaerp.com/)
 * [Addax.com.tr](https://www.addax.com.tr/)
 * [Adiban.ac.ir](http://adiban.ac.ir/)
 * [Aebis.com](http://aebis.com/)
 * [AgroCountry.com](https://agrocountry.com/)
 * [AlyaVillaHotel.com](http://alyavillahotel.com/)
 * [ArmanSanjesh.com](http://armansanjesh.com/)
 * [Artlist.io](https://artlist.io/)
 * [AspCore.net](http://aspcore.net/)
 * [ASP-MVC.ir](http://asp-mvc.ir/)
 * [AsteImmobili.it](https://www.asteimmobili.it/)
 * [Auto-Deffeuille.fr](https://www.auto-deffeuille.fr/)
 * [AutoThivolle.com](http://www.autothivolle.com/)
 * [Aztual.es](https://www.aztual.es/)
 * [BancoMercedes-Benz.com.br](http://bancomercedes-benz.com.br/)
 * [BeckerDVP.nl](http://www.beckerdvp.nl/)
 * [BeefEaterBBQ.com.au](https://www.beefeaterbbq.com.au/)
 * [BernerFoodAndBeverage.com](http://www.bernerfoodandbeverage.com/)
 * [BigBrandTire.com](https://www.bigbrandtire.com/)
 * [BillerudKorsnas.se](https://www.billerudkorsnas.se/)
 * [BiZiDEX.com](https://bizidex.com/)
 * [Bridge-Direct.com](https://www.bridge-direct.com/)
 * [Brusella.com.ar](http://brusella.com.ar/)
 * [BUET.ac.bd](http://www.buet.ac.bd/)
 * [CaglaTur.com](https://www.caglatur.com/)
 * [CaixaGuissona.com](https://caixaguissona.com/)
 * [Carnival.com](https://www.carnival.com/)
 * [Carot.vn](http://carot.vn/)
 * [CEV.org.tr](http://www.cev.org.tr/)
 * [Chanussot.fr](https://www.chanussot.fr/)
 * [ChefAppliances.com.au](https://www.chefappliances.com.au/)
 * [CoastalRealestatePattaya.com](http://coastalrealestatepattaya.com/)
 * [CokTatil.com.tr](https://www.coktatil.com.tr/)
 * [CompusoftGroup.com](https://www.compusoftgroup.com/)
 * [CONVVE.com](http://convve.com/)
 * [CountryChoice.co.uk](https://www.countrychoice.co.uk/)
 * [DamBeton.nl](http://www.dambeton.nl/)
 * [DBIS.edu.hk](http://dbis.edu.hk/)
 * [Dealtoday.vn](https://www.dealtoday.vn/)
 * [Deqor.com](https://www.deqor.com/)
 * [Digitope.com](https://www.digitope.com/)
 * [DN.se](https://www.dn.se/)
 * [DocShell.ru](https://www.docshell.ru/)
 * [DProtein.com](https://www.dprotein.com/)
 * [eBarkhat.ac.ir](http://ebarkhat.ac.ir/)
 * [ECItele.com](http://www.ecitele.com/)
 * [EcitServices.no](http://www.ecitservices.no/)
 * [e-damavandihe.ac.ir](http://e-damavandihe.ac.ir/)
 * [Elbek-Vejrup.dk](https://elbek-vejrup.dk/)
 * [Eperde.com](https://www.eperde.com/)
 * [EquitiGlobalMarkets.ae](http://www.equitiglobalmarkets.ae/)
 * [Esser.com.br](https://www.esser.com.br/)
 * [Eurocodes.dk](http://eurocodes.dk/)
 * [FashionFriends.com](https://www.fashionfriends.com/)
 * [Fastimport.uy](http://www.fastimport.uy/)
 * [feixiaohao.com](https://www.feixiaohao.com/)
 * [FirmaRehberi.tv.tr](https://www.firmarehberi.tv.tr/)
 * [FMOutsource.com](http://www.fmoutsource.com/)
 * [focusing.ru](https://focusing.ru/)
 * [ForemostGolf.com](https://www.foremostgolf.com/)
 * [FreeNetworkAnalyzer.com](http://freenetworkanalyzer.com/)
 * [FreeSerialAnalyzer.com](https://freeserialanalyzer.com/)
 * [FreTor.com](http://www.fretor.com/)
 * [Furthermore.Equinox.com](https://furthermore.equinox.com/)
 * [FxPro.com](https://www.fxpro.com/)
 * [GatwickParking.co.uk](https://www.gatwickparking.co.uk/)
 * [Giant-Bicycles.com](https://www.giant-bicycles.com/)
 * [Giant-Cambridge.co.uk](http://www.giant-cambridge.co.uk/)
 * [Giant-Rutland.co.uk](http://www.giant-rutland.co.uk/)
 * [Gnatta.com](https://gnatta.com/)
 * [GoDream.se](https://godream.se/)
 * [GoGetIt.com.pa](https://www.gogetit.com.pa/)
 * [Guide.DStv.com](http://guide.dstv.com/)
 * [hagtak.com](http://www.hagtak.com/)
 * [HalifaxRealEstate.com](https://halifaxrealestate.com/)
 * [HHDSoftware.com](http://www.hhdsoftware.com/)
 * [hifirmpc.com](http://hifirmpc.com/)
 * [HiHoliday.ir](http://hiholiday.ir/)
 * [HotelMaviDeniz.com](http://hotelmavideniz.com/)
 * [HotelMedusa.eu](http://www.hotelmedusa.eu/)
 * [HotelVirgilio.it](http://www.hotelvirgilio.it/)
 * [IMD.org](https://www.imd.org/)
 * [ImoRadar24.ro](https://www.imoradar24.ro/)
 * [Instat.gov.al](http://instat.gov.al/)
 * [iranzaminedu.com](http://iranzaminedu.com/)
 * [iStaff.ru](http://istaff.ru/)
 * [Juhasz.cz](http://juhasz.cz/)
 * [Kadindio.com](http://kadindio.com/)
 * [KamKan.ru](https://www.kamkan.ru/)
 * [Kempinski.com](https://www.kempinski.com/)
 * [KeyToSteel.com](http://www.keytosteel.com/)
 * [KIP.com.tr](https://www.kip.com.tr/)
 * [KomsuFirin.com.tr](http://www.komsufirin.com.tr/)
 * [KonyaSeker.com.tr](http://konyaseker.com.tr/)
 * [KpiBsc.com](http://www.kpibsc.com/)
 * [KwadrantGroep.nl](https://www.kwadrantgroep.nl/)
 * [LecomparateurAssurance.com](https://www.lecomparateurassurance.com/)
 * [LeOrchidee.it](http://www.leorchidee.it/)
 * [LikyaGardens.com](https://www.likyagardens.com/)
 * [Logix.dk](https://logix.dk/)
 * [LogixSuite.it](http://www.logixsuite.it/)
 * [Macingo.com](https://www.macingo.com/)
 * [MaiLinhExpress.vn](http://www.mailinhexpress.vn/)
 * [Medivir.se](http://www.medivir.se/)
 * [MindFintech.fr](https://www.mindfintech.fr/)
 * [Mini-ielts.com](http://mini-ielts.com/)
 * [Momentum-Biking.com](https://www.momentum-biking.com/)
 * [Moulsford.com](https://www.moulsford.com/)
 * [MPCir.com](http://mpcir.com/)
 * [Multicare.org.uk](https://multicare.org.uk/)
 * [music2me.de](https://music2me.de/)
 * [MyChronicMigraine.com](https://www.mychronicmigraine.com/)
 * [NEI.com.br](http://www.nei.com.br/)
 * [NimbusItSolutions.com](http://www.nimbusitsolutions.com/)
 * [Niteco.com](https://niteco.com/)
 * [NoorMags.ir](https://www.noormags.ir/)
 * [NottinghamHigh.co.uk](http://www.nottinghamhigh.co.uk/)
 * [OGMods.net](http://ogmods.net/)
 * [Oldflix.com.br](https://oldflix.com.br/)
 * [Ontrack.com](https://www.ontrack.com/)
 * [OntrackDataRecovery.nl](https://www.ontrackdatarecovery.nl/)
 * [OOS.SDU.edu.tr](https://oos.sdu.edu.tr/)
 * [Oostwoud.com](http://www.oostwoud.com/)
 * [Oostwoud.de](http://www.oostwoud.de/)
 * [OplevelsesGaver.dk](https://www.oplevelsesgaver.dk/)
 * [Osmanlilar.gen.tr](http://osmanlilar.gen.tr/)
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
 * [Qamshy.kz](http://www.qamshy.kz/)
 * [QuizPop.com.br](http://www.quizpop.com.br/)
 * [RAKS.com.tr](https://www.raks.com.tr/)
 * [Ramsey.com.tr](https://www.ramsey.com.tr/)
 * [raovat49.com](https://raovat49.com/)
 * [RattiAuto.it](http://rattiauto.it/)
 * [RecelInteractive.com](https://www.recelinteractive.com/)
 * [RedhamSites.ru](https://www.redhamsites.ru/)
 * [RefrigerationDiscount.co.uk](https://www.refrigerationdiscount.co.uk/)
 * [RevelDigital.com](https://www.reveldigital.com/)
 * [RosKvartal.ru](https://roskvartal.ru/)
 * [RotelliPizzaPasta.com](http://www.rotellipizzapasta.com/)
 * [SahInnParadise.com.tr](http://sahinnparadise.com.tr/)
 * [SebaSuites.com.tr](http://sebasuites.com.tr/)
 * [Shooger.com](https://shooger.com/)
 * [Siteimprove.com](https://siteimprove.com/)
 * [SitePoint.de](https://www.sitepoint.de/)
 * [SkillGamesBoard.com](http://skillgamesboard.com/)
 * [SleepersInSeattle.com](http://www.sleepersinseattle.com/)
 * [SmartLeadersIAS.com](http://smartleadersias.com/)
 * [Smitma.com](https://www.smitma.com/)
 * [Songtradr.com](https://www.songtradr.com/)
 * [SpazioAste.it](https://www.spazioaste.it/)
 * [Speak.nl](http://www.speak.nl/)
 * [SpeakWithAGeek.com](https://speakwithageek.com/)
 * [Ster.nl](https://www.ster.nl/)
 * [StranaGruzov.ru](http://stranagruzov.ru/)
 * [Sway.com](https://sway.com/)
 * [SwedishMatch.com](http://www.swedishmatch.com/)
 * [Sweet-Simple.com/](http://www.sweet-simple.com/)
 * [Synaeda.nl](http://www.synaeda.nl/)
 * [SynapseNet.ru](https://synapsenet.ru/)
 * [TecMundo.com.br](http://www.tecmundo.com.br/)
 * [Temporent.se](https://www.temporent.se/)
 * [Tergan.com.tr](https://www.tergan.com.tr/)
 * [Together.bg](http://together.bg)
 * [togofogo.com](http://www.togofogo.com/)
 * [ToolSlick.com](https://toolslick.com/)
 * [TopDev.ir](http://topdev.ir/)
 * [Toy.co.uk](https://www.toy.co.uk/)
 * [TrailerRentals.com.au](https://www.trailerrentals.com.au/)
 * [TruckAndTrailerFinancing.com](http://www.truckandtrailerfinancing.com/)
 * [Ujat.mx](http://ujat.mx/)
 * [USdirectory.com](https://usdirectory.com/)
 * [VegaITSourcing.rs](https://www.vegaitsourcing.rs/)
 * [Vico.vn](https://vico.vn/)
 * [Vidal-Vidal.com](https://www.vidal-vidal.com/)
 * [ViettechSolution.com](https://www.viettechsolution.com/)
 * [Vindlov.se](http://www.vindlov.se/)
 * [VisitTCI.com](https://www.visittci.com/)
 * [VogueBodrum.com](http://voguebodrum.com/)
 * [VolareSystems.com](https://volaresystems.com/)
 * [WiesnKini.de](http://wiesnkini.de/)
 * [WooshAirportExtras.com](https://www.wooshairportextras.com/)
 * [XemLichAm.com](http://xemlicham.com/)
 * [xln.co.uk](https://www.xln.co.uk/)
 * [Zemana.com](https://www.zemana.com)
 * [ZkontrolujsiAuto.cz](https://www.zkontrolujsiauto.cz/)
 * [Zolv.com](https://www.zolv.com/)