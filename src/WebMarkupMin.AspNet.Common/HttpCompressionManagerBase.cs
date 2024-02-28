using System;
using System.Collections.Generic;
using System.Linq;

using WebMarkupMin.AspNet.Common.Compressors;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Base class of HTTP compression manager
	/// </summary>
	public abstract class HttpCompressionManagerBase : ContentProcessingManagerBase, IHttpCompressionManager
	{
		/// <summary>
		/// Gets or sets a list of HTTP compressor factories
		/// </summary>
		public IList<ICompressorFactory> CompressorFactories
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a delegate that determines whether the media-type is supported
		/// </summary>
		public Func<string, bool> SupportedMediaTypePredicate
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs a instance of HTTP compression manager
		/// </summary>
		protected HttpCompressionManagerBase()
		{
			SupportedMediaTypePredicate = null;
		}


		/// <summary>
		/// Creates a instance of compressor
		/// </summary>
		/// <param name="acceptEncoding">Value of the Accept-Encoding HTTP header</param>
		/// <returns>Instance of compressor</returns>
		public ICompressor CreateCompressor(string acceptEncoding)
		{
			ICompressor compressor;

			if (!TryCreateCompressor(acceptEncoding, out compressor))
			{
				compressor = new NullCompressor();
			}

			return compressor;
		}

		/// <summary>
		/// Tries to create a instance of compressor.
		/// A return value indicates whether the creation succeeded.
		/// </summary>
		/// <param name="acceptEncoding">Value of the Accept-Encoding HTTP header</param>
		/// <param name="compressor">Instance of compressor</param>
		/// <returns><c>true</c> if the compressor was created; otherwise, <c>false</c></returns>
		public bool TryCreateCompressor(string acceptEncoding, out ICompressor compressor)
		{
			compressor = null;
			IList<ICompressorFactory> factories = CompressorFactories;
			int factoryCount = factories.Count;

			if (acceptEncoding != null && factoryCount > 0)
			{
				string[] encodingTokens = acceptEncoding
					.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
					.Select(e => e.Trim().ToLowerInvariant())
					.ToArray()
					;

				for (int factoryIndex = 0; factoryIndex < factoryCount; factoryIndex++)
				{
					ICompressorFactory factory = factories[factoryIndex];
					if (encodingTokens.Contains(factory.EncodingToken))
					{
						compressor = factory.CreateCompressor();
						break;
					}
				}
			}

			bool result = compressor != null;

			return result;
		}
	}
}