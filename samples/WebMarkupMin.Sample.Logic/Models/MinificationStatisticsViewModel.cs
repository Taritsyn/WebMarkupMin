using System;

namespace WebMarkupMin.Sample.Logic.Models
{
	/// <summary>
	/// Minification statistics
	/// </summary>
	public sealed class MinificationStatisticsViewModel
    {
		/// <summary>
		/// Gets a size of original code in bytes
		/// </summary>
		public long OriginalSize
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a formatted size of original code
		/// </summary>
		public string OriginalSizeFormatted
		{
			get
			{
				return FormatSize(OriginalSize);
			}
		}

		/// <summary>
		/// Gets a size of gzipped original code in bytes
		/// </summary>
		public long OriginalGzipSize
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a formatted size of gzipped original code
		/// </summary>
		public string OriginalGzipSizeFormatted
		{
			get
			{
				return FormatSize(OriginalGzipSize);
			}
		}

		/// <summary>
		/// Gets a size of minified code in bytes
		/// </summary>
		public long MinifiedSize
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a formatted size of minified code
		/// </summary>
		public string MinifiedSizeFormatted
		{
			get
			{
				return FormatSize(MinifiedSize);
			}
		}

		/// <summary>
		/// Gets a size of gzipped minified code in bytes
		/// </summary>
		public long MinifiedGzipSize
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a formatted size of gzipped minified code
		/// </summary>
		public string MinifiedGzipSizeFormatted
		{
			get
			{
				return FormatSize(MinifiedGzipSize);
			}
		}

		/// <summary>
		/// Gets a percent of bytes which has been saved off the original size
		/// </summary>
		public decimal SavedInPercent
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a percent of bytes which has been saved off the gzipped size
		/// </summary>
		public decimal SavedGzipInPercent
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a duration of minification in milliseconds
		/// </summary>
		public int MinificationDuration
		{
			get;
			set;
		}


		/// <summary>
		/// Formats a size of code
		/// </summary>
		/// <param name="size">Size of code in bytes</param>
		/// <returns>Formatted size of code</returns>
		private static string FormatSize(long size)
		{
			string formattedSize;
			if (size >= 1024)
			{
				formattedSize = string.Format("{0:0.00}KB", Math.Round((decimal)size / 1024, 2));
			}
			else
			{
				formattedSize = string.Format("{0} bytes", size);
			}

			return formattedSize;
		}
    }
}