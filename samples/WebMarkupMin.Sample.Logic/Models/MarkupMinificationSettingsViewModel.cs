using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#if NET452 || NETSTANDARD || NETCOREAPP
using Microsoft.AspNetCore.Mvc.Rendering;
#elif NET40
using System.Web.Mvc;
#else
#error No implementation for this target
#endif

using WebMarkupMin.Core;
using WebMarkupMin.Sample.Resources;

namespace WebMarkupMin.Sample.Logic.Models
{
	/// <summary>
	/// Markup minification settings view model
	/// </summary>
	public abstract class MarkupMinificationSettingsViewModel
	{
		private static readonly IEnumerable<SelectListItem> _newLineStyles;


		[Display(Name = "DisplayName_PreserveNewLines", ResourceType = typeof(MinificationStrings))]
		public bool PreserveNewLines
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_NewLineStyle", ResourceType = typeof(MinificationStrings))]
		public NewLineStyle NewLineStyle
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> NewLineStyleList
		{
			get { return _newLineStyles; }
		}


		static MarkupMinificationSettingsViewModel()
		{
			_newLineStyles = new List<SelectListItem>
			{
				new SelectListItem
				{
					Value = NewLineStyle.Auto.ToString(),
					Text = MinificationStrings.ListItem_NewLineStyle_Auto
				},
				new SelectListItem
				{
					Value = NewLineStyle.Native.ToString(),
					Text = MinificationStrings.ListItem_NewLineStyle_Native
				},
				new SelectListItem
				{
					Value = NewLineStyle.Windows.ToString(),
					Text = MinificationStrings.ListItem_NewLineStyle_Windows
				},
				new SelectListItem
				{
					Value = NewLineStyle.Mac.ToString(),
					Text = MinificationStrings.ListItem_NewLineStyle_Mac
				},
				new SelectListItem
				{
					Value = NewLineStyle.Unix.ToString(),
					Text = MinificationStrings.ListItem_NewLineStyle_Unix
				}
			};
		}
	}
}