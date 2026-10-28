

   --------------------------------------------------------------------------------
              README file for Web Markup Minifier: ASP.NET Common v2.23.0

   --------------------------------------------------------------------------------

           Copyright (c) 2013-2026 Andrey Taritsyn - http://www.taritsyn.ru


   ===========
   DESCRIPTION
   ===========
   WebMarkupMin.AspNet.Common is auxiliary package, that contains classes and
   interfaces for all ASP.NET extensions.

   =============
   RELEASE NOTES
   =============
   1. Added a .NET 11 target;
   2. In version for .NET 11 was added a new compressor (`ZstdCompressor`)
      based on the `System.IO.Compression.ZstandardStream` class;
   3. Three classes have been deprecated: `BuiltInBrotliCompressor` class replaced
      by the `BrotliCompressor` class, `BuiltInBrotliCompressionSettings` class
      replaced by the `BrotliCompressionSettings` class and
     `BuiltInBrotliCompressorFactory` class replaced by the
     `BrotliCompressorFactory` class.

   =============
   DOCUMENTATION
   =============
   See documentation on GitHub - https://github.com/Taritsyn/WebMarkupMin/wiki