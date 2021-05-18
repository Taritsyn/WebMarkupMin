

   --------------------------------------------------------------------------------
          README file for Web Markup Minifier: ASP.NET 4.X Web Forms v2.10.0

   --------------------------------------------------------------------------------

           Copyright (c) 2013-2021 Andrey Taritsyn - http://www.taritsyn.ru


   ===========
   DESCRIPTION
   ===========
   WebMarkupMin.AspNet4.WebForms contains 5 Web Forms page classes:
   `MinifiedHtmlPage` (supports HTML minification only), `MinifiedXhtmlPage`
   (supports XHTML minification only), `CompressedPage` (supports HTTP compression
   only), `MinifiedAndCompressedHtmlPage` (supports HTML minification and HTTP
   compression) and `MinifiedAndCompressedXhtmlPage` (supports XHTML minification
   and HTTP compression); and 5 master page classes: `MinifiedHtmlMasterPage`
   (supports HTML minification only), `MinifiedXhtmlMasterPage` (supports XHTML
   minification only), `CompressedMasterPage` (supports HTTP compression only),
   `MinifiedAndCompressedHtmlMasterPage` (supports HTML minification and HTTP
   compression) and `MinifiedAndCompressedXhtmlMasterPage` (supports XHTML
   minification and HTTP compression).

   Classes of the Web Forms pages and master pages cannot be used together.

   =============
   RELEASE NOTES
   =============
   In markup minification and compression managers was added a new configuration
   property - `SupportedHttpStatusCodes` (default `200`).

   =============
   DOCUMENTATION
   =============
   See documentation on GitHub - https://github.com/Taritsyn/WebMarkupMin/wiki