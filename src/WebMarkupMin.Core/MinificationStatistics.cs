using System;
using System.IO;
using System.IO.Compression;
using System.Text;

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
		public decimal SavedInBytes
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a number of bytes which has been saved off the gzipped size
		/// </summary>
		public decimal SavedGzipInBytes
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
		public MinificationStatistics() : this(Encoding.GetEncoding(0))
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
			byte[] bytes = _encoding.GetBytes(originalContent);

			OriginalSize = bytes.Length;
			OriginalGzipSize = CalculateGzipSize(bytes);
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
			byte[] bytes = _encoding.GetBytes(minifiedContent);

			MinifiedSize = bytes.Length;
			MinifiedGzipSize = CalculateGzipSize(bytes);
			CompressionRatio = CalculateCompressionRatio(OriginalSize, MinifiedSize);
			CompressionGzipRatio = CalculateCompressionRatio(OriginalGzipSize, MinifiedGzipSize);
			SavedInBytes = OriginalSize - MinifiedSize;
			SavedGzipInBytes = OriginalGzipSize - MinifiedGzipSize;
			SavedInPercent = 100 - CompressionRatio;
			SavedGzipInPercent = 100 - CompressionGzipRatio;
			MinificationDuration = (_endTime - _startTime).Milliseconds;
		}

		/// <summary>
		/// Calculatea a compression ratio
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
		/// Calculatea a size of gzipped code
		/// </summary>
		/// <param name="bytes">Array of bytes</param>
		/// <returns>Size of gzipped code in bytes</returns>
		private static long CalculateGzipSize(byte[] bytes)
		{
			using (var memoryStream = new MemoryStream())
			{
				// the third parameter tells the GZIP stream to leave the base stream open so it doesn't
				// dispose of it when it gets disposed. This is needed because we need to dispose the
				// GZIP stream before it will write ANY of its data.
				using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
				{
					gzipStream.Write(bytes, 0, bytes.Length);
				}

				return memoryStream.Position;
			}
		}
	}
}