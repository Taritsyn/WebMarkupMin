

   --------------------------------------------------------------------------------
            README file for Web Markup Minifier: Brotli for ASP.NET v2.19.0

   --------------------------------------------------------------------------------

           Copyright (c) 2013-2025 Andrey Taritsyn - http://www.taritsyn.ru


   ===========
   DESCRIPTION
   ===========
   WebMarkupMin.AspNet.Brotli contains one compressor-adapter for compression of
   text content by using the Brotli algorithm (https://github.com/google/brotli) -
   `BrotliCompressor`. `BrotliCompressor` is based on the BrotliSharpLib
   (https://github.com/master131/BrotliSharpLib) version 0.3.3.

   In version for .NET Standard 2.1 and .NET 9 uses the built-in compressor from
   the `System.IO.Compression` namespace.

   If you are using extensions for ASP.NET Core 3.1 or higher, then you should use
   a `BuiltInBrotliCompressor` class from `WebMarkupMin.AspNet.Common.Compressors`
   namespace instead of a `BrotliCompressor` class. In this case, the
   WebMarkupMin.Brotli package will no longer be needed and you can uninstall it.

   =============
   DOCUMENTATION
   =============
   See documentation on GitHub - https://github.com/Taritsyn/WebMarkupMin/wiki