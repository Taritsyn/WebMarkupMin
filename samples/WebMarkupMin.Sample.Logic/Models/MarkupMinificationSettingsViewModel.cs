using System.ComponentModel.DataAnnotations;

using WebMarkupMin.Sample.Resources;

namespace WebMarkupMin.Sample.Logic.Models
{
	/// <summary>
	/// Markup minification settings view model
	/// </summary>
	public abstract class MarkupMinificationSettingsViewModel
	{
		[Display(Name = "DisplayName_PreserveNewLines", ResourceType = typeof(MinificationStrings))]
		public bool PreserveNewLines
		{
			get;
			set;
		}
	}
}