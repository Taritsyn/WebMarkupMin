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
	/// Common HTML minification settings view model
	/// </summary>
	public abstract class CommonHtmlMinificationSettingsViewModel : MarkupMinificationSettingsViewModel
	{
		private static readonly IEnumerable<SelectListItem> _whitespaceMinificationModes;
		private static readonly IEnumerable<SelectListItem> _availableCssMinifierList;
		private static readonly IEnumerable<SelectListItem> _availableJsMinifierList;


		[Display(Name = "DisplayName_WhitespaceMinificationMode", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public WhitespaceMinificationMode WhitespaceMinificationMode
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_RemoveHtmlComments", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public bool RemoveHtmlComments
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_RemoveHtmlCommentsFromScriptsAndStyles", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public bool RemoveHtmlCommentsFromScriptsAndStyles
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_UseShortDoctype", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public bool UseShortDoctype
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_UseMetaCharsetTag", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public bool UseMetaCharsetTag
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_RemoveTagsWithoutContent", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public bool RemoveTagsWithoutContent
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_RemoveEmptyAttributes", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public bool RemoveEmptyAttributes
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_RemoveRedundantAttributes", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public bool RemoveRedundantAttributes
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_PreservableAttributeList", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public string PreservableAttributeList
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_RemoveHttpProtocolFromAttributes", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public bool RemoveHttpProtocolFromAttributes
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_RemoveHttpsProtocolFromAttributes", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public bool RemoveHttpsProtocolFromAttributes
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_RemoveJsProtocolFromAttributes", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public bool RemoveJsProtocolFromAttributes
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_MinifyEmbeddedCssCode", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public bool MinifyEmbeddedCssCode
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_MinifyInlineCssCode", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public bool MinifyInlineCssCode
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_CssMinifierName", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public string CssMinifierName
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> AvailableCssMinifierList
		{
			get { return _availableCssMinifierList; }
		}

		[Display(Name = "DisplayName_MinifyEmbeddedJsCode", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public bool MinifyEmbeddedJsCode
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_MinifyInlineJsCode", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public bool MinifyInlineJsCode
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_JsMinifierName", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public string JsMinifierName
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> AvailableJsMinifierList
		{
			get { return _availableJsMinifierList; }
		}

		[Display(Name = "DisplayName_MinifyEmbeddedJsonData", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public bool MinifyEmbeddedJsonData
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_ProcessableScriptTypeList", ResourceType = typeof(CommonHtmlMinificationStrings))]
		[StringLength(256)]
		public string ProcessableScriptTypeList
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_MinifyKnockoutBindingExpressions", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public bool MinifyKnockoutBindingExpressions
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_MinifyAngularBindingExpressions", ResourceType = typeof(CommonHtmlMinificationStrings))]
		public bool MinifyAngularBindingExpressions
		{
			get;
			set;
		}

		[Display(Name = "DisplayName_CustomAngularDirectiveList", ResourceType = typeof(CommonHtmlMinificationStrings))]
		[StringLength(256)]
		public string CustomAngularDirectiveList
		{
			get;
			set;
		}

		public IEnumerable<SelectListItem> WhitespaceMinificationModeList
		{
			get { return _whitespaceMinificationModes; }
		}


		static CommonHtmlMinificationSettingsViewModel()
		{
			_whitespaceMinificationModes = new List<SelectListItem>
			{
				new SelectListItem
				{
					Value = WhitespaceMinificationMode.None.ToString(),
					Text = CommonHtmlMinificationStrings.ListItem_WhitespaceMinificationMode_None
				},
				new SelectListItem
				{
					Value = WhitespaceMinificationMode.Safe.ToString(),
					Text = CommonHtmlMinificationStrings.ListItem_WhitespaceMinificationMode_Safe
				},
				new SelectListItem
				{
					Value = WhitespaceMinificationMode.Medium.ToString(),
					Text = CommonHtmlMinificationStrings.ListItem_WhitespaceMinificationMode_Medium
				},
				new SelectListItem
				{
					Value = WhitespaceMinificationMode.Aggressive.ToString(),
					Text = CommonHtmlMinificationStrings.ListItem_WhitespaceMinificationMode_Aggressive
				}
			};

			_availableCssMinifierList = new List<SelectListItem>
			{
				new SelectListItem { Value = "KristensenCssMinifier", Text = "Mads Kristensen's CSS minifier" },
#if !NETSTANDARD1_6
				new SelectListItem { Value = "MsAjaxCssMinifier", Text = "Microsoft Ajax CSS Minifier" },
#endif
#if !NET40 && !NETSTANDARD1_6
				new SelectListItem { Value = "YuiCssMinifier", Text = "YUI CSS Minifier" },
#endif
				new SelectListItem { Value = "NUglifyCssMinifier", Text = "NUglify CSS Minifier" }
			}
			;

			_availableJsMinifierList = new List<SelectListItem>
			{
				new SelectListItem { Value = "CrockfordJsMinifier", Text = "Douglas Crockford's JS Minifier" },
#if !NETSTANDARD1_6
				new SelectListItem { Value = "MsAjaxJsMinifier", Text = "Microsoft Ajax JS Minifier" },
#endif
#if !NET40 && !NETSTANDARD1_6
				new SelectListItem { Value = "YuiJsMinifier", Text = "YUI JS Minifier" },
#endif
				new SelectListItem { Value = "NUglifyJsMinifier", Text = "NUglify JS Minifier" }
			}
			;
		}

		protected CommonHtmlMinificationSettingsViewModel()
		{
			CssMinifierName = "KristensenCssMinifier";
			JsMinifierName ="CrockfordJsMinifier";
		}
	}
}