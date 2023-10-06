using System;
using System.Text;

#if NET451 || NETSTANDARD
using HostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
#elif NETCOREAPP3_1_OR_GREATER
using HostingEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;
#else
#error No implementation for this target
#endif

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.Core;
using WebMarkupMin.Core.Utilities;

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
#elif ASPNETCORE7
namespace WebMarkupMin.AspNetCore7
#elif ASPNETCORE8
namespace WebMarkupMin.AspNetCore8
#else
#error No implementation for this target
#endif
{
	/// <summary>
	/// WebMarkupMin options
	/// </summary>
	public class WebMarkupMinOptions : WebMarkupMinConfigurationBase
	{
		/// <summary>
		/// The default encoding that will be used if the content encoding
		/// could not be determined for the current HTTP response
		/// </summary>
		private Encoding _defaultEncoding;

		/// <summary>
		/// Gets or sets a flag for whether to allow markup minification
		/// if the current hosting environment name is development
		/// </summary>
		public bool AllowMinificationInDevelopmentEnvironment
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to allow HTTP compression of content
		/// if the current hosting environment name is development
		/// </summary>
		public bool AllowCompressionInDevelopmentEnvironment
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a default encoding that will be used if the content encoding
		/// could not be determined for the current HTTP response
		/// </summary>
		public Encoding DefaultEncoding
		{
			get { return _defaultEncoding; }
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}

				_defaultEncoding = value;
			}
		}

		/// <summary>
		/// Gets or sets a instance of hosting environment
		/// </summary>
		public HostingEnvironment HostingEnvironment
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs a instance of WebMarkupMin options
		/// </summary>
		public WebMarkupMinOptions()
		{
			AllowMinificationInDevelopmentEnvironment = false;
			AllowCompressionInDevelopmentEnvironment = false;
			DefaultEncoding = TargetFrameworkShortcuts.DefaultTextEncoding;
		}
	}
}