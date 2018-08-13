

   --------------------------------------------------------------------------------
              README file for Web Markup Minifier: ASP.NET 4.X MVC v2.4.0

   --------------------------------------------------------------------------------

           Copyright (c) 2013-2018 Andrey Taritsyn - http://www.taritsyn.ru


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
   1. Added support of .NET Framework 4.5;
   2. Now the `NullLogger` class is used as the default logger;
   3. Now, by default, the GZip algorithm has a higher priority than the Deflate.

   =============
   DOCUMENTATION
   =============
   See documentation on GitHub - https://github.com/Taritsyn/WebMarkupMin/wiki