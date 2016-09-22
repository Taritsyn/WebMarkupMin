using System.Text.RegularExpressions;

using WebMarkupMin.AspNet.Common.Compressors;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Base class of HTTP compression manager
	/// </summary>
	public abstract class HttpCompressionManagerBase : IHttpCompressionManager
	{
		private const string NAME_PATTERN = @"[\w-.:+]+";

		/// <summary>
		/// Text-based content type regular expression
		/// </summary>
		private static readonly Regex _textBasedContentTypePrefixRegex = new Regex(
			string.Format(@"^text/{0}|application/(xml(\-{0})?|{0}\+xml|{0}script|json)$", NAME_PATTERN)
		);


		/// <summary>
		/// Creates a instance of compressor
		/// </summary>
		/// <param name="acceptEncoding">Value of the Accept-Encoding HTTP header</param>
		/// <returns>Instance of compressor</returns>
		public ICompressor CreateCompressor(string acceptEncoding)
		{
			ICompressor compressor = null;

			if (acceptEncoding != null)
			{
				acceptEncoding = acceptEncoding.ToLowerInvariant();
				if (acceptEncoding.Contains(EncodingTokenConstants.Deflate))
				{
					compressor = new DeflateCompressor();
				}
				else if (acceptEncoding.Contains(EncodingTokenConstants.GZip))
				{
					compressor = new GZipCompressor();
				}
			}

			compressor = compressor ?? new NullCompressor();

			return compressor;
		}

		/// <summary>
		/// Checks whether the media-type is supported
		/// </summary>
		/// <param name="mediaType">Media-type</param>
		/// <returns>Result of check (true - supported; false - not supported)</returns>
		public bool IsSupportedMediaType(string mediaType)
		{
			return _textBasedContentTypePrefixRegex.IsMatch(mediaType);
		}
	}
}