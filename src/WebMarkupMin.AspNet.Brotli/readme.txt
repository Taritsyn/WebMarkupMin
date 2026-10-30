

   --------------------------------------------------------------------------------
            README file for Web Markup Minifier: Brotli for ASP.NET v2.23.0

   --------------------------------------------------------------------------------

           Copyright (c) 2013-2026 Andrey Taritsyn - http://www.taritsyn.ru


   ===========
   DESCRIPTION
   ===========
   WebMarkupMin.AspNet.Brotli contains one compressor-adapter for compression of
   text content by using the Brotli algorithm (https://github.com/google/brotli) -
   `BrotliCompressor`. `BrotliCompressor` is based on the BrotliSharpLib
   (https://github.com/master131/BrotliSharpLib) version 0.3.3.

   Does not support .NET Core 3.0 or higher.

   If you are using extensions for ASP.NET Core 3.1 or higher, then you should use
   a `BrotliCompressor` class from `WebMarkupMin.AspNet.Common.Compressors`
   namespace. In this case, the WebMarkupMin.Brotli package will no longer be
   needed and you can uninstall it.

   =============
   RELEASE NOTES
   =============
   1. No longer supports a .NET Core 3.0 or higher;
   2. Three classes have been deprecated: `BrotliCompressor` class replaced by the
      `LegacyBrotliCompressor` class, `BrotliCompressionSettings` class replaced by
      the `LegacyBrotliCompressionSettings` class and `BrotliCompressorFactory`
      class replaced by the `LegacyBrotliCompressorFactory` class.

   =============
   DOCUMENTATION
   =============
   See documentation on GitHub - https://github.com/Taritsyn/WebMarkupMin/wiki