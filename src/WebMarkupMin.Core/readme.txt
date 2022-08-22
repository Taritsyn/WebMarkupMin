

   --------------------------------------------------------------------------------
                   README file for Web Markup Minifier: Core v2.12.0

   --------------------------------------------------------------------------------

           Copyright (c) 2013-2022 Andrey Taritsyn - http://www.taritsyn.ru


   ===========
   DESCRIPTION
   ===========
   The Web Markup Minifier (abbreviated WebMarkupMin) is a .NET library that
   contains a set of markup minifiers. The objective of this project is to improve
   the performance of web applications by reducing the size of HTML, XHTML and XML
   code.

   WebMarkupMin absorbed the best of existing solutions from non-microsoft
   platforms: Juriy Zaytsev's HTML Minifier
   (https://github.com/kangax/html-minifier/) (written in JavaScript) and Sergiy
   Kovalchuk's HtmlCompressor (https://github.com/serg472/htmlcompressor) (written
   in Java).

   Minification of markup produces by removing extra whitespaces, comments and
   redundant code (only for HTML and XHTML). In addition, HTML and XHTML minifiers
   supports the minification of CSS code from style tags and attributes, and
   minification of JavaScript code from script tags, event attributes and
   hyperlinks with javascript: protocol. WebMarkupMin.Core contains built-in
   JavaScript minifier based on the Douglas Crockford's JSMin
   (https://github.com/douglascrockford/JSMin) and built-in CSS minifier based on
   the Mads Kristensen's Efficient stylesheet minifier
   (https://madskristensen.net/blog/Efficient-stylesheet-minification-in-C).
   The above mentioned minifiers produce only the most simple minifications of
   CSS and JavaScript code, but you can always install additional modules that
   support the more powerful algorithms of minification: WebMarkupMin.MsAjax
   (contains minifier-adapters for the Microsoft Ajax Minifier -
   https://github.com/microsoft/ajaxmin), WebMarkupMin.Yui (contains
   minifier-adapters for YUI Compressor for .NET -
   https://github.com/YUICompressor-NET/YUICompressor.NET) and WebMarkupMin.NUglify
   (contains minifier-adapters for the NUglify - https://github.com/trullock/NUglify).

   Also supports minification of views of popular JavaScript template engines:
   KnockoutJS, Kendo UI MVVM and AngularJS 1.X.

   =============
   RELEASE NOTES
   =============
   1. In HTML, XHTML and XML minification settings was added two new properties:
      `PreserveNewLines` (default `false`) and `NewLineStyle` (default `Auto`);
   2. LazyHTML wrapped fragments are now parsed correctly;
   3. Location of the error that occurs during minification of JSON data are now
      calculated correctly.

   =============
   DOCUMENTATION
   =============
   See documentation on GitHub - https://github.com/Taritsyn/WebMarkupMin/wiki