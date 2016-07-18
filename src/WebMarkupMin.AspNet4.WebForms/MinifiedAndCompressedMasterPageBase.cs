using System;

using WebMarkupMin.AspNet4.WebForms.Components;

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
		/// <param name="minifiedComponent">Minified component</param>
		/// <param name="compressedComponent">Compressed component</param>
		protected MinifiedAndCompressedMasterPageBase(MinifiedComponentBase minifiedComponent,
			CompressedComponent compressedComponent)
			: base(minifiedComponent)
		{
			_compressedComponent = compressedComponent;
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