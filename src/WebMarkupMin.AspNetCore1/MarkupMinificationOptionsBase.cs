using System.Collections.Generic;

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2
#elif ASPNETCORE3
namespace WebMarkupMin.AspNetCore3
#elif ASPNETCORE5
namespace WebMarkupMin.AspNetCore5
#elif ASPNETCORE6
namespace WebMarkupMin.AspNetCore6
#else
#error No implementation for this target
#endif
{
	/// <summary>
	/// Base class of markup minification options
	/// </summary>
	/// <typeparam name="TSettings">The type of markup minification settings</typeparam>
	public abstract class MarkupMinificationOptionsBase<TSettings> : ContentProcessingOptionsBase
		where TSettings : class, new()
	{
		/// <summary>
		/// Gets or sets a markup minification settings used to configure the HTML minifier
		/// </summary>
		public virtual TSettings MinificationSettings
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a list of supported media-types
		/// </summary>
		public virtual ISet<string> SupportedMediaTypes
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to allow generate minification statistics
		/// (available through the logger)
		/// </summary>
		public bool GenerateStatistics
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs a instance of markup minification options
		/// </summary>
		protected MarkupMinificationOptionsBase()
		{
			GenerateStatistics = false;
		}
	}
}