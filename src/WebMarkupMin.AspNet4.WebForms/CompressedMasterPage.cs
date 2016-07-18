using System;
using System.Web.UI;

using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNet4.Common;
using WebMarkupMin.AspNet4.WebForms.Components;

namespace WebMarkupMin.AspNet4.WebForms
{
	/// <summary>
	/// Master page with support of HTTP compression
	/// </summary>
	public class CompressedMasterPage : MasterPage
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
		/// Constructs a instance of master page with support of HTTP compression
		/// </summary>
		public CompressedMasterPage()
			: this(WebMarkupMinConfiguration.Instance, null)
		{ }

		/// <summary>
		/// Constructs a instance of master page with support of HTTP compression
		/// </summary>
		/// <param name="configuration">WebMarkupMin configuration</param>
		/// <param name="compressionManager">HTTP compression manager</param>
		public CompressedMasterPage(WebMarkupMinConfiguration configuration,
			IHttpCompressionManager compressionManager)
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