

   --------------------------------------------------------------------------------
              README file for Web Markup Minifier: ASP.NET 4.X MVC v2.4.0

   --------------------------------------------------------------------------------

           Copyright (c) 2013-2017 Andrey Taritsyn - http://www.taritsyn.ru


   ===========
   DESCRIPTION
   ===========
   WebMarkupMin.AspNet4.Mvc contains 4 action filters: `MinifyHtmlAttribute` (for
   minification of HTML code), `MinifyXhtmlAttribute` (for minification of XHTML
   code), `MinifyXmlAttribute` (for minification of XML code) and
   `CompressContentAttribute` (for compression of text content by using GZIP or
   Deflate).

   =============
   RELEASE NOTES
   =============
   1. Now, by default, only the `GET` requests are minified and compressed (this
      behavior can be changed by using the `SupportedHttpMethods` property);
   2. Fixed a error of filtering media-types, which led to incorrect usage of HTTP
      compression.

   =============
   DOCUMENTATION
   =============
   See documentation on GitHub - http://github.com/Taritsyn/WebMarkupMin/wiki