using System;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet4.Common;

namespace WebMarkupMin.AspNet4.WebForms
{
	/// <summary>
	/// Base class of master page with support of markup minification and HTTP compression
	/// </summary>
	public abstract class MinifiedAndCompressedMasterPageBase : MinifiedMasterPageBase
	{
		/// <summary>
		/// Compressed component
		/// </summary>
		private readonly CompressedComponent _compressedComponent;

		/// <summary>
		/// Gets or sets a flag for whether to disable HTTP compression of content
		/// </summary>
		public bool DisableCompression
		{
			get
			{
				return _compressedComponent.DisableCompression;
			}
			set
			{
				_compressedComponent.DisableCompression = value;
			}
		}


		/// <summary>
		/// Constructs a instance of master page with support of markup minification and HTTP compression
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="minificationManager">Markup minification manager</param>
		/// <param name="compressionManager">HTTP compression manager</param>
		protected MinifiedAndCompressedMasterPageBase(WebMarkupMinConfiguration configuration,
			IMarkupMinificationManager minificationManager,
			IHttpCompressionManager compressionManager)
			: base(configuration, minificationManager)
		{
			_compressedComponent = new CompressedComponent(configuration, compressionManager);
		}


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			_compressedComponent.OnLoad(e);
		}

		protected override void OnError(EventArgs e)
		{
			base.OnError(e);
			_compressedComponent.OnError(e);
		}
	}
}