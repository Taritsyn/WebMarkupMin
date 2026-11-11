

   --------------------------------------------------------------------------------
            README file for Web Markup Minifier: Zstd for ASP.NET v2.24.0

   --------------------------------------------------------------------------------

           Copyright (c) 2013-2026 Andrey Taritsyn - http://www.taritsyn.ru


   ===========
   DESCRIPTION
   ===========
   WebMarkupMin.AspNet.Zstd contains one compressor-adapter for compression of text
   content by using the Zstd (Zstandard) algorithm (https://github.com/meta/zstd) -
   `LegacyZstdCompressor`. `LegacyZstdCompressor` is based on the
   ZstdSharp (https://github.com/oleg-st/ZstdSharp) version 0.8.8 and is intended
   for old versions of ASP.NET.

   If you are using extensions for ASP.NET Core 11, then you should use
   a `ZstdCompressor` class from `WebMarkupMin.AspNet.Common.Compressors`
   namespace. In this case, the WebMarkupMin.AspNet.Zstd package will no longer be
   needed and you can uninstall it.

   =============
   DOCUMENTATION
   =============
   See documentation on GitHub - https://github.com/Taritsyn/WebMarkupMin/wiki