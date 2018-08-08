using System.IO;
using System.IO.Compression;

namespace WebMarkupMin.AspNet.Common.Compressors
{
	/// <summary>
	/// GZip compressor
	/// </summary>
	public sealed class GZipCompressor : ICompressor
	{
#if NET45 || NETSTANDARD
		/// <summary>
		/// GZip compression settings
		/// </summary>
		private readonly GZipCompressionSettings _settings;

#endif
		/// <summary>
		/// Gets a encoding token
		/// </summary>
		public string EncodingToken
		{
			get { return EncodingTokenConstants.GZip; }
		}

#if NET45 || NETSTANDARD

		/// <summary>
		/// Constructs an instance of the GZip compressor
		/// </summary>
		public GZipCompressor()
			: this(new GZipCompressionSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the GZip compressor
		/// </summary>
		/// <param name="settings">GZip compression settings</param>
		public GZipCompressor(GZipCompressionSettings settings)
		{
			_settings = settings;
		}

#endif

		/// <summary>
		/// Compress a stream by GZip algorithm
		/// </summary>
		/// <param name="stream">The stream</param>
		/// <returns>The compressed stream</returns>
		public Stream Compress(Stream stream)
		{
#if NET45 || NETSTANDARD
			return new GZipStream(stream, _settings.Level);
#else
			return new GZipStream(stream, CompressionMode.Compress);
#endif
		}
	}
}