using System;
using System.Collections.Generic;
using System.Linq;
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
		/// Gets or sets a list of HTTP compressor factories
		/// </summary>
		public IList<ICompressorFactory> CompressorFactories
		{
			get;
			set;
		}


		/// <summary>
		/// Creates a instance of compressor
		/// </summary>
		/// <param name="acceptEncoding">Value of the Accept-Encoding HTTP header</param>
		/// <returns>Instance of compressor</returns>
		public ICompressor CreateCompressor(string acceptEncoding)
		{
			ICompressor compressor = null;
			IList<ICompressorFactory> factories = CompressorFactories;

			if (acceptEncoding != null && factories != null && factories.Count > 0)
			{
				string[] encodingTokens = acceptEncoding
					.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
					.Select(e => e.Trim().ToLowerInvariant())
					.ToArray()
					;

				foreach (ICompressorFactory factory in factories)
				{
					if (encodingTokens.Contains(factory.EncodingToken))
					{
						compressor = factory.CreateCompressor();
						break;
					}
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