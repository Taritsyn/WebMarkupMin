using System;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// HTTP compressor extensions
	/// </summary>
	public static class CompressorExtensions
	{
		/// <summary>
		/// Appends a HTTP headers
		/// </summary>
		/// <param name="compressor">HTTP compressor</param>
		/// <param name="append">HTTP header appending delegate</param>
		public static void AppendHttpHeaders(this ICompressor compressor, Action<string, string> append)
		{
			string encodingToken = compressor.EncodingToken;

			if (!string.IsNullOrWhiteSpace(encodingToken))
			{
				append("Content-Encoding", compressor.EncodingToken);
				append("Vary", "Accept-Encoding");
			}
		}
	}
}