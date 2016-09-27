using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using WebMarkupMin.Core;
using WebMarkupMin.Sample.Resources;

namespace WebMarkupMin.Sample.Logic.Models
{
	/// <summary>
	/// Markup minification result
	/// </summary>
	public sealed class MarkupMinificationResultViewModel
	{
		/// <summary>
		/// Gets or sets a minified content
		/// </summary>
		[Display(Name = "DisplayName_MinifiedContent", ResourceType = typeof(MinificationStrings))]
		[DataType(DataType.Html)]
		public string MinifiedContent
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a list of the errors
		/// </summary>
		public IList<MinificationErrorInfo> Errors
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a list of the warnings
		/// </summary>
		public IList<MinificationErrorInfo> Warnings
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a minification statistics
		/// </summary>
		public MinificationStatisticsViewModel Statistics
		{
			get;
			set;
		}
	}
}