using System;
#if NET45 || NETSTANDARD || NETCOREAPP2_1
using System.Buffers;
#endif
using System.IO;
using System.IO.Compression;
using System.Text;
#if NET40

using PolyfillsForOldDotNet.System.Buffers;
#endif

using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// Minification statistics
	/// </summary>
	public sealed class MinificationStatistics
	{
		/// <summary>
		/// Type of text encoding
		/// </summary>
		private readonly Encoding _encoding;

		/// <summary>
		/// Start time of minification
		/// </summary>
		private DateTime _startTime;

		/// <summary>
		/// End time of minification
		/// </summary>
		private DateTime _endTime;

		/// <summary>
		/// Gets a size of original code in bytes
		/// </summary>
		public long OriginalSize
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a size of gzipped original code in bytes
		/// </summary>
		public long OriginalGzipSize
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a size of minified code in bytes
		/// </summary>
		public long MinifiedSize
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a size of gzipped minified code in bytes
		/// </summary>
		public long MinifiedGzipSize
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a compression ratio in percent
		/// </summary>
		public decimal CompressionRatio
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a compression ratio with additional GZip-compression in percent
		/// </summary>
		public decimal CompressionGzipRatio
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a number of bytes which has been saved off the original size
		/// </summary>
		public long SavedInBytes
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a number of bytes which has been saved off the gzipped size
		/// </summary>
		public long SavedGzipInBytes
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a percent of bytes which has been saved off the original size
		/// </summary>
		public decimal SavedInPercent
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a percent of bytes which has been saved off the gzipped size
		/// </summary>
		public decimal SavedGzipInPercent
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a duration of minification in milliseconds
		/// </summary>
		public int MinificationDuration
		{
			get;
			private set;
		}


		/// <summary>
		/// Constructs instance of minification statistics
		/// </summary>
		public MinificationStatistics() : this(TextEncodingShortcuts.Default)
		{ }

		/// <summary>
		/// Constructs instance of minification statistics
		/// </summary>
		/// <param name="encoding">Type of text encoding</param>
		public MinificationStatistics(Encoding encoding)
		{
			_encoding = encoding;
		}


		/// <summary>
		/// Initializes a minification statistics
		/// </summary>
		/// <param name="originalContent">Original content</param>
		internal void Init(string originalContent)
		{
			var byteArrayPool = ArrayPool<byte>.Shared;
			int byteCount = _encoding.GetByteCount(originalContent);
			byte[] bytes = byteArrayPool.Rent(byteCount);
			long compressedByteCount = 0;

			try
			{
				_encoding.GetBytes(originalContent, 0, originalContent.Length, bytes, 0);
				compressedByteCount = CalculateGzipSize(bytes, 0, byteCount);
			}
			finally
			{
				byteArrayPool.Return(bytes);
			}

			OriginalSize = byteCount;
			OriginalGzipSize = compressedByteCount;
			MinifiedSize = 0;
			MinifiedGzipSize = 0;
			CompressionRatio = 0;
			CompressionGzipRatio = 0;
			SavedInBytes = 0;
			SavedGzipInBytes = 0;
			SavedInPercent = 0;
			SavedGzipInPercent = 0;
			MinificationDuration = 0;

			_startTime = DateTime.Now;
			_endTime = DateTime.MinValue;
		}

		/// <summary>
		/// Finalizes a minification statistics
		/// </summary>
		/// <param name="minifiedContent">Minified content</param>
		internal void End(string minifiedContent)
		{
			_endTime = DateTime.Now;

			var byteArrayPool = ArrayPool<byte>.Shared;
			int byteCount = _encoding.GetByteCount(minifiedContent);
			byte[] bytes = byteArrayPool.Rent(byteCount);
			long compressedByteCount = 0;

			try
			{
				_encoding.GetBytes(minifiedContent, 0, minifiedContent.Length, bytes, 0);
				compressedByteCount = CalculateGzipSize(bytes, 0, byteCount);
			}
			finally
			{
				byteArrayPool.Return(bytes);
			}

			MinifiedSize = byteCount;
			MinifiedGzipSize = compressedByteCount;
			CompressionRatio = CalculateCompressionRatio(OriginalSize, MinifiedSize);
			CompressionGzipRatio = CalculateCompressionRatio(OriginalGzipSize, MinifiedGzipSize);
			SavedInBytes = OriginalSize - MinifiedSize;
			SavedGzipInBytes = OriginalGzipSize - MinifiedGzipSize;
			SavedInPercent = 100 - CompressionRatio;
			SavedGzipInPercent = 100 - CompressionGzipRatio;
			MinificationDuration = (_endTime - _startTime).Milliseconds;
		}

		/// <summary>
		/// Calculates a compression ratio
		/// </summary>
		/// <param name="originalSize">Original size</param>
		/// <param name="minifiedSize">Minified size</param>
		/// <returns>Compression ratio</returns>
		private static decimal CalculateCompressionRatio(long originalSize, long minifiedSize)
		{
			decimal compressionRatio = 0;
			if (minifiedSize > 0)
			{
				compressionRatio = Math.Round((decimal)minifiedSize / originalSize * 100, 2);
			}

			return compressionRatio;
		}

		/// <summary>
		/// Calculates a size of gzipped code
		/// </summary>
		/// <param name="buffer">The buffer that contains the data to compress</param>
		/// <param name="offset">The byte offset in <paramref name="buffer"/> from which
		/// the bytes will be read</param>
		/// <param name="count">The maximum number of bytes to write</param>
		/// <returns>Size of gzipped code in bytes</returns>
		private static long CalculateGzipSize(byte[] buffer, int offset, int count)
		{
			using (var memoryStream = new MemoryStream())
			{
				// The third parameter tells the GZip stream to leave the base stream open so it doesn't
				// dispose of it when it gets disposed. This is needed because we need to dispose the
				// GZip stream before it will write ANY of its data.
				using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
				{
					gzipStream.Write(buffer, offset, count);
				}

				long compressedByteCount = memoryStream.Length;
				memoryStream.Clear();

				return compressedByteCount;
			}
		}
	}
}