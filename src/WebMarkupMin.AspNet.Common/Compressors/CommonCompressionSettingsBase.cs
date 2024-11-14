#if NET45 || NETSTANDARD || NET9_0_OR_GREATER
using System.IO.Compression;

namespace WebMarkupMin.AspNet.Common.Compressors
{
#if NET9_0_OR_GREATER
	/// <summary>
	/// Base class of common compression settings
	/// </summary>
	/// <typeparam name="TOptions">The type of generic compression options</typeparam>
#else
	/// <summary>
	/// Base class of common compression settings
	/// </summary>
#endif
	public abstract class CommonCompressionSettingsBase
#if NET9_0_OR_GREATER
<TOptions> where TOptions : class, new()
#endif
	{
#if NET9_0_OR_GREATER
		/// <summary>
		/// Compression options
		/// </summary>
		protected TOptions _options = new();

#endif
		/// <summary>
		/// Gets or sets a compression level
		/// </summary>
		public abstract CompressionLevel Level
		{
			get;
			set;
		}
#if NET9_0_OR_GREATER

		/// <summary>
		/// Gets or sets a alternative compression level
		/// </summary>
		public abstract int AlternativeLevel
		{
			get;
			set;
		}
#endif


		/// <summary>
		/// Constructs an instance of common compression settings
		/// </summary>
		protected CommonCompressionSettingsBase()
		{
			Level = CompressionLevel.Optimal;
		}
#if NET9_0_OR_GREATER


		/// <summary>
		/// Gets a compression options
		/// </summary>
		/// <returns>Compression options</returns>
		internal TOptions GetOptions()
		{
			return _options;
		}
#endif
	}
}
#endif