Web Markup Minifier [![NuGet version](http://img.shields.io/nuget/v/WebMarkupMin.Core.svg)](https://www.nuget.org/packages/WebMarkupMin.Core/)  [![Download count](https://img.shields.io/nuget/dt/WebMarkupMin.Core.svg)](https://www.nuget.org/packages/WebMarkupMin.Core/)
===================

<img src="https://raw.githubusercontent.com/Taritsyn/WebMarkupMin/master/images/WebMarkupMin_Logo.png" width="440" height="86" alt="WebMarkupMin logo" />

The **Web Markup Minifier** (abbreviated WebMarkupMin) - a .NET library that contains a set of markup minifiers.
The objective of this project is to improve the performance of web applications by reducing the size of HTML, XHTML and XML code.

WebMarkupMin absorbed the best of existing solutions from non-microsoft platforms: Juriy Zaytsev's [HTML Minifier](https://github.com/kangax/html-minifier) (written in JavaScript) and Sergiy Kovalchuk's [HtmlCompressor](https://github.com/serg472/htmlcompressor) (written in Java).

Minification of markup produces by removing extra whitespace, comments and redundant code (only for HTML and XHTML).
In addition, HTML and XHTML minifiers supports the minification of CSS code from `style` tags and attributes, and minification of JavaScript code from `script` tags, event attributes and hyperlinks with `javascript:` protocol.
WebMarkupMin.Core contains built-in JavaScript minifier based on the Douglas Crockford's [JSMin](https://github.com/douglascrockford/JSMin) and built-in CSS minifier based on the Mads Kristensen's [Efficient stylesheet minifier](https://madskristensen.net/blog/efficient-stylesheet-minification-in-c).
The above mentioned minifiers produce only the most simple minifications of CSS and JavaScript code, but you can always install additional modules that support the more powerful algorithms of minification: WebMarkupMin.MsAjax (contains minifier-adapters for the [Microsoft Ajax Minifier](https://github.com/microsoft/ajaxmin)), WebMarkupMin.Yui (contains minifier-adapters for the [YUI Compressor for .NET](https://github.com/YUICompressor-NET/YUICompressor.NET)) and WebMarkupMin.NUglify (contains minifier-adapters for the [NUglify](https://github.com/trullock/NUglify)).

Also supports minification of views of popular JavaScript template engines: [KnockoutJS](https://knockoutjs.com/), [Kendo UI MVVM](https://www.telerik.com/kendo-ui) and [AngularJS](https://angularjs.org/) 1.X.

In addition, there are several modules that integrate this library into ASP.NET: WebMarkupMin.AspNet4.HttpModules (for ASP.NET 4.X and ASP.NET Web Pages), WebMarkupMin.AspNet4.Mvc (for ASP.NET MVC 3, 4 or 5), WebMarkupMin.AspNet4.WebForms (for ASP.NET Web Forms 4.X), WebMarkupMin.AspNetCore1 (for ASP.NET Core 1.X), WebMarkupMin.AspNetCore2 (for ASP.NET Core 2.X), WebMarkupMin.AspNetCore3 (for ASP.NET Core 3.1 and 5), WebMarkupMin.AspNetCore6 (for ASP.NET Core 6 and 7) and WebMarkupMin.AspNetCoreLatest (for ASP.NET Core 8 and 9).

You can try WebMarkupMin in action and experiment with different minification settings live on the [WebMarkupMin Online](https://webmarkupmin.rsite.net/) site.

## NuGet Packages

### Core
 * [WebMarkupMin: Core](http://nuget.org/packages/WebMarkupMin.Core/) (supports .NET Framework 4.0 Client, .NET Framework 4.5, .NET Standard 1.3, .NET Standard 2.0, .NET Standard 2.1 and .NET 9)

### External JS and CSS minifiers
 * [WebMarkupMin: MS Ajax](http://nuget.org/packages/WebMarkupMin.MsAjax/) (supports .NET Framework 4.0 Client, .NET Framework 4.5, .NET Standard 2.0 and .NET 9)
 * [WebMarkupMin: YUI](http://nuget.org/packages/WebMarkupMin.Yui/) (supports .NET Framework 4.5.2, .NET Standard 2.0 and .NET 9)
 * [WebMarkupMin: NUglify](http://nuget.org/packages/WebMarkupMin.NUglify/) (supports .NET Framework 4.0 Client, .NET Framework 4.5, .NET Standard 1.3, .NET Standard 2.0 and .NET 9)

### ASP.NET Extensions
 * [WebMarkupMin: ASP.NET 4.X HTTP modules](http://nuget.org/packages/WebMarkupMin.AspNet4.HttpModules/) (supports .NET Framework 4.0 and .NET Framework 4.5)
 * [WebMarkupMin: ASP.NET 4.X MVC](http://nuget.org/packages/WebMarkupMin.AspNet4.Mvc/) (supports .NET Framework 4.0 and .NET Framework 4.5)
 * [WebMarkupMin: ASP.NET 4.X Web Forms](http://nuget.org/packages/WebMarkupMin.AspNet4.WebForms/) (supports .NET Framework 4.0 and .NET Framework 4.5)
 * [WebMarkupMin: ASP.NET Core 1.X](http://nuget.org/packages/WebMarkupMin.AspNetCore1/) (supports .NET Framework 4.5.1 and .NET Standard 1.3)
 * [WebMarkupMin: ASP.NET Core 2.X](http://nuget.org/packages/WebMarkupMin.AspNetCore2/) (supports .NET Standard 2.0)
 * [WebMarkupMin: ASP.NET Core 3.1+](http://nuget.org/packages/WebMarkupMin.AspNetCore3/) (supports .NET Core App 3.1)
 * [WebMarkupMin: ASP.NET Core 6+](http://nuget.org/packages/WebMarkupMin.AspNetCore6/) (supports .NET 6)
 * [WebMarkupMin: ASP.NET Core Latest](https://www.nuget.org/packages/WebMarkupMin.AspNetCoreLatest/) (supports .NET 8, .NET 9 and .NET 10)
 * [WebMarkupMin: Brotli for ASP.NET](http://nuget.org/packages/WebMarkupMin.AspNet.Brotli/) (supports .NET Framework 4.0, .NET Framework 4.5, .NET Standard 1.3, .NET Standard 2.0, .NET Standard 2.1 and .NET 9)

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
 * [AspNetStatic](https://github.com/ZarehD/AspNetStatic)
 * [Blog-Umbraco](https://github.com/radyz/Blog-Umbraco) by Ernesto Chavez Sanchez
 * [Colorful.Cache.Web](https://www.nuget.org/packages/Colorful.Cache.Web)
 * [Colorful.CMS.Core](https://www.nuget.org/packages/Colorful.CMS.Core/)
 * [Constellation.Foundation.Mvc](https://github.com/constellation4sitecore/constellation-sitecore9) by Rick Cabral
 * [Constellation.Sitecore.Presentation.Mvc](https://github.com/sitecorerick/constellation.sitecore.presentation.mvc) by Rick Cabral
 * [Dyfort Umbraco Html Minifier](https://www.dyfort.com/products/html-minifier/)
 * [Elect AspNetCore Useful Middlewares](https://github.com/topnguyen/Elect/tree/master/src/Web/Elect.Web.Middlewares) by Top Nguyen
 * [FAV Rocks](https://github.com/billbogaiv/fav-rocks) by Bill Boga
 * [File Sharing Application](http://bitbucket.org/Artur2/filesharingapplication) by Artur N
 * [GeeksCoreLibrary](https://github.com/happy-geeks/geeks-core-library)
 * [Google Pagespeed Tools for NopCommerce](https://www.foxnetsoft.com/noppagespeedtools) by FoxNetSoft
 * [GrandNode](https://github.com/grandnode/grandnode2)
 * [iTEAMConsulting.FormHandler](https://github.com/iteam-consulting/csharp-form-handler)
 * [Karambolo.AspNetCore.Bundling.WebMarkupMin](https://github.com/adams85/bundling)
 * [Lightweight NetCore MVC Template](https://marketplace.visualstudio.com/items?itemName=PabloMorelli.LightweightNetCoreMVCTemplate) by Pablo Morelli
 * [MiniBlog](https://github.com/madskristensen/MiniBlog) by Mads Kristensen
 * [Miniblog.Core](https://github.com/madskristensen/Miniblog.Core) by Mads Kristensen
 * [nopCommerce](https://www.nopcommerce.com/)
 * [Razor Minification](https://github.com/guardrex/RazorMinification) by Luke Latham
 * [StaticWebHelper](https://github.com/madskristensen/StaticWebHelper) by Mads Kristensen
 * [Statiq Framework](https://statiq.dev/framework) (formerly known as [Wyam](http://wyam.io/))
 * [Web Essentials 2013](https://github.com/madskristensen/WebEssentials2013) by Mads Kristensen

### Websites
 * [101Sport.net](https://www.101sport.net/)
 * [ActivaERP.com](https://www.activaerp.com/)
 * [Adiban.ac.ir](http://adiban.ac.ir/)
 * [Aebis.com](http://aebis.com/)
 * [AgroCountry.com](https://agrocountry.com/)
 * [Apple-NIC.com](https://www.apple-nic.com/) by Sepehr Davarnia
 * [ASFINAG.at](https://www.asfinag.at/)
 * [AspCore.net](http://aspcore.net/)
 * [ASP-MVC.ir](http://asp-mvc.ir/)
 * [Assist2Sell.com](https://assist2sell.com/) by Christopher Dengler
 * [BBB.org](https://www.bbb.org/)
 * [BeautifyCode.net](https://beautifycode.net/) by Anghel Valentin
 * [BillerudKorsnas.se](https://www.billerudkorsnas.se/)
 * [Bridge-Direct.com](https://www.bridge-direct.com/)
 * [Bye-Airport.com](https://bye-airport.com/) by Yasin Kultur
 * [CaixaGuissona.com](https://caixaguissona.com/)
 * [CampingDeCarlton.nl](https://campingdecarlton.nl/)
 * [Carnival.com](https://www.carnival.com/)
 * [ChannelEngine.com](https://www.channelengine.com/)
 * [CityOfBoise.org](https://www.cityofboise.org/)
 * [ClientEarth.org](https://www.clientearth.org/)
 * [CloudParser.ru](https://cloudparser.ru/)
 * [CLS.vn](https://cls.vn/)
 * [CollectOffers.com](https://www.collectoffers.com/)
 * [Crossmedial.be](https://www.crossmedial.be/)
 * [DamBeton.nl](http://www.dambeton.nl/)
 * [DanTri.com.vn](https://dantri.com.vn/)
 * [DBIS.edu.hk](http://dbis.edu.hk/)
 * [DDPlanet.ru](https://www.ddplanet.ru/)
 * [Dealtoday.vn](https://www.dealtoday.vn/)
 * [Dell.com](https://www.dell.com/)
 * [DProtein.com](https://www.dprotein.com/)
 * [eBarkhat.ac.ir](http://ebarkhat.ac.ir/)
 * [ECIT.com](https://www.ecit.com/)
 * [e-damavandihe.ac.ir](http://e-damavandihe.ac.ir/)
 * [Elbek-Vejrup.dk](https://elbek-vejrup.dk/)
 * [EquitiGlobalMarkets.ae](http://www.equitiglobalmarkets.ae/)
 * [FCMImobiliaria.com.br](https://fcmimobiliaria.com.br/)
 * [FirmaRehberi.tv.tr](https://www.firmarehberi.tv.tr/)
 * [FMOutsource.com](http://www.fmoutsource.com/)
 * [FreeNetworkAnalyzer.com](https://freenetworkanalyzer.com/)
 * [FreeSerialAnalyzer.com](https://freeserialanalyzer.com/)
 * [Giant-Bicycles.com](https://www.giant-bicycles.com/)
 * [Giant-Cambridge.co.uk](http://www.giant-cambridge.co.uk/)
 * [Giant-Rutland.co.uk](http://www.giant-rutland.co.uk/)
 * [GoGetIt.com.pa](https://www.gogetit.com.pa/)
 * [GreatLittleBreaks.com](https://www.greatlittlebreaks.com/)
 * [Guide.DStv.com](http://guide.dstv.com/)
 * [GurkanTuna.com](https://gurkantuna.com/) by Gürkan Tuna
 * [HAS.nl](https://www.has.nl/)
 * [HHDSoftware.com](http://www.hhdsoftware.com/)
 * [HiHoliday.ir](http://hiholiday.ir/)
 * [HometownBanks.com](https://www.hometownbanks.com/)
 * [HotelMedusa.eu](http://www.hotelmedusa.eu/)
 * [HotelVirgilio.it](http://www.hotelvirgilio.it/)
 * [HousingNow.com](https://housingnow.com/) by Christopher Dengler
 * [HypeProxy.io](http://hypeproxy.io/)
 * [IDcreation.be](https://www.idcreation.be/)
 * [Immobilier.CBRE.fr](https://immobilier.cbre.fr/)
 * [ImoRadar24.ro](https://www.imoradar24.ro/)
 * [inCONCRETO.net](https://www.inconcreto.net/)
 * [INGENIO-WEB.it](https://www.ingenio-web.it/)
 * [Insights.com](https://www.insights.com/)
 * [Instat.gov.al](http://instat.gov.al/)
 * [Intuitive.gg](https://www.intuitive.gg/)
 * [ItaliaOggi.it](https://www.italiaoggi.it/)
 * [KamKan.ru](https://www.kamkan.ru/)
 * [KCEBV.nl](https://www.kcebv.nl/)
 * [KeyToSteel.com](http://www.keytosteel.com/)
 * [King.com](https://www.king.com/)
 * [Kleisma.com](https://www.kleisma.com/)
 * [KnaufLaw.com](https://www.knauflaw.com/)
 * [KonyaSeker.com.tr](http://konyaseker.com.tr/)
 * [LeOrchidee.it](http://www.leorchidee.it/)
 * [LiderGD.com](https://www.lidergd.com/)
 * [Liv-Cycling.com](https://www.liv-cycling.com/)
 * [Logix.dk](https://logix.dk/)
 * [LogixSuite.it](http://www.logixsuite.it/)
 * [Macingo.com](https://www.macingo.com/)
 * [MaiLinhExpress.vn](http://www.mailinhexpress.vn/)
 * [MALL.TV](https://www.mall.tv/)
 * [Medivir.se](http://www.medivir.se/)
 * [Mini-ielts.com](http://mini-ielts.com/)
 * [mishlohim.co.il](https://www.mishlohim.co.il/)
 * [Momentum-Biking.com](https://www.momentum-biking.com/)
 * [MottMac.com](https://www.mottmac.com/)
 * [Moulsford.com](https://www.moulsford.com/)
 * [Multicare.org.uk](https://multicare.org.uk/)
 * [Nilfisk.com](https://www.nilfisk.com/)
 * [NimbusItSolutions.com](http://www.nimbusitsolutions.com/)
 * [NOA.nl](https://www.noa.nl/)
 * [noihoidonganh.com](https://noihoidonganh.com/)
 * [NVB.nl](https://www.nvb.nl/)
 * [NYAS.org](https://www.nyas.org/)
 * [OGMods.net](http://ogmods.net/)
 * [Oldflix.com.br](https://oldflix.com.br/)
 * [OOS.SDU.edu.tr](https://oos.sdu.edu.tr/)
 * [OXXOshop.com](http://www.oxxoshop.com/)
 * [ParizanSanat.com](http://parizansanat.com/)
 * [PatientAccess.com](https://www.patientaccess.com/)
 * [PAVIMENTI-WEB.it](https://www.pavimenti-web.it/)
 * [PaxinasGalegas.es](https://www.paxinasgalegas.es/)
 * [Permira.com](https://www.permira.com/)
 * [PisoTermico.com.br](http://pisotermico.com.br/)
 * [PortaleAste.com](https://www.portaleaste.com/)
 * [PortRegis.com](https://www.portregis.com/)
 * [PositiveCoach.org](https://positivecoach.org/)
 * [PostRandomonium.com](http://postrandomonium.com/)
 * [PranaCrystals.com](https://www.pranacrystals.com/)
 * [PrepSpotLight.tv](http://prepspotlight.tv/)
 * [RAKS.com.tr](https://www.raks.com.tr/)
 * [Randommer.io](https://randommer.io/) by Anghel Valentin
 * [raovat49.com](https://raovat49.com/)
 * [RattiAuto.it](http://rattiauto.it/)
 * [RecelInteractive.com](https://www.recelinteractive.com/)
 * [RefrigerationDiscount.co.uk](https://www.refrigerationdiscount.co.uk/)
 * [research.ox.ac.uk](https://www.research.ox.ac.uk/)
 * [RevelDigital.com](https://www.reveldigital.com/)
 * [RewardPay.com](https://www.rewardpay.com/)
 * [RosKvartal.ru](https://roskvartal.ru/)
 * [RystadEnergy.com](https://www.rystadenergy.com/)
 * [Sazas.nl](https://www.sazas.nl/)
 * [ScholenOpDeKaart.nl](https://scholenopdekaart.nl/)
 * [Seek4Cars.net](https://seek4cars.net/)
 * [Shooger.com](https://shooger.com/)
 * [SkillGamesBoard.com](http://skillgamesboard.com/)
 * [SleepersInSeattle.com](http://www.sleepersinseattle.com/)
 * [Songtradr.com](https://www.songtradr.com/)
 * [SpazioAste.it](https://www.spazioaste.it/)
 * [Sports.NDTV.com](https://sports.ndtv.com/)
 * [Sublet.com](https://www.sublet.com/)
 * [Sway.Office.com](https://sway.office.com/)
 * [SwedishMatch.com](http://www.swedishmatch.com/)
 * [Synaeda.nl](http://www.synaeda.nl/)
 * [SynapseNet.ru](https://synapsenet.ru/)
 * [Together.bg](http://together.bg)
 * [Toy.co.uk](https://www.toy.co.uk/)
 * [TrailerRentals.com.au](https://www.trailerrentals.com.au/)
 * [Tritac.com](https://www.tritac.com/nl/)
 * [UK.tonzo.com](https://uk.tonzo.com/) by Yasin Kultur
 * [USdirectory.com](https://usdirectory.com/)
 * [vbr.ru](https://www.vbr.ru/)
 * [VegaITSourcing.rs](https://www.vegaitsourcing.rs/)
 * [Vesteda.com](https://www.vesteda.com/)
 * [VGSupply.com](https://www.vgsupply.com/)
 * [VolareSystems.com](https://volaresystems.com/)
 * [WhoCalled.co.uk](https://whocalled.co.uk/)
 * [XemLichAm.com](http://xemlicham.com/)
 * [YeniSafak.com](https://www.yenisafak.com/)
 * [Youvia.nl](https://www.youvia.nl/)
 * [Zemana.com](https://www.zemana.com)
 * [ZkontrolujsiAuto.cz](https://www.zkontrolujsiauto.cz/)
 * [Zolv.com](https://www.zolv.com/)