WebMarkupMin.AspNet.Zstd contains one compressor-adapter for compression of text content by using the [Zstd](https://github.com/meta/zstd) (Zstandard) algorithm - `LegacyZstdCompressor`.
`LegacyZstdCompressor` is based on the [ZstdSharp](https://github.com/oleg-st/ZstdSharp) version 0.8.8 and is intended for old versions of ASP.NET.

If you are using extensions for ASP.NET Core 11, then you should use a `ZstdCompressor` class from `WebMarkupMin.AspNet.Common.Compressors` namespace.
In this case, the WebMarkupMin.AspNet.Zstd package will no longer be needed and you can uninstall it.