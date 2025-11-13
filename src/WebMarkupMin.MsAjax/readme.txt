

   --------------------------------------------------------------------------------
                  README file for Web Markup Minifier: MS Ajax v2.19.0

   --------------------------------------------------------------------------------

           Copyright (c) 2013-2025 Andrey Taritsyn - http://www.taritsyn.ru


   ===========
   DESCRIPTION
   ===========
   WebMarkupMin.MsAjax contains 2 minifier-adapters: `MsAjaxCssMinifier` (for
   minification of CSS code) and `MsAjaxJsMinifier` (for minification of JS code).
   These adapters perform minification using the Microsoft Ajax Minifier
   (https://github.com/microsoft/ajaxmin).

   =============
   RELEASE NOTES
   =============
   1. Performed a migration to the modern C# null/not-null checks;
   2. Added support for .NET 9;
   3. In the `lock` statements for .NET 9 target now uses a instances of the
      `System.Threading.Lock` class.

   =============
   DOCUMENTATION
   =============
   See documentation on GitHub - https://github.com/Taritsyn/WebMarkupMin/wiki