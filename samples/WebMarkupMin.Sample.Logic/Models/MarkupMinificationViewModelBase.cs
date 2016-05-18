using System.ComponentModel.DataAnnotations;

using MinificationStrings = WebMarkupMin.Sample.Resources.MinificationResources;

namespace WebMarkupMin.Sample.Logic.Models
{
	/// <summary>
	/// Minification view model
	/// </summary>
	public abstract class MarkupMinificationViewModelBase
	{
		/// <summary>
		/// Gets or sets a source code
		/// </summary>
		[Display(Name = "DisplayName_SourceCode", ResourceType = typeof(MinificationStrings))]
		[DataType(DataType.Html)]
		[Required(ErrorMessageResourceName = "ErrorMessage_FormFieldIsNotFilled", ErrorMessageResourceType = typeof(MinificationStrings))]
		[StringLength(1000000, ErrorMessageResourceName = "ErrorMessage_FormFieldValueTooLong", ErrorMessageResourceType = typeof(MinificationStrings))]
		public string SourceCode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a minification result
		/// </summary>
		public MarkupMinificationResultViewModel Result
		{
			get;
			set;
		}
	}
}