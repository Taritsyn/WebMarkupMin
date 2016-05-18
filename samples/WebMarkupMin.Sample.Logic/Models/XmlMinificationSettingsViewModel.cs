using System.ComponentModel.DataAnnotations;

using XmlMinificationStrings = WebMarkupMin.Sample.Resources.XmlMinificationResources;

namespace WebMarkupMin.Sample.Logic.Models
{
	public sealed class XmlMinificationSettingsViewModel
	{
		[Display(Name = "DisplayName_MinifyWhitespace", ResourceType = typeof(XmlMinificationStrings))]
		public bool MinifyWhitespace
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_RemoveXmlComments", ResourceType = typeof(XmlMinificationStrings))]
		public bool RemoveXmlComments
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_RenderEmptyTagsWithSpace", ResourceType = typeof(XmlMinificationStrings))]
		public bool RenderEmptyTagsWithSpace
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_CollapseTagsWithoutContent", ResourceType = typeof(XmlMinificationStrings))]
		public bool CollapseTagsWithoutContent
		{
			get;
			set;
		}
	}
}