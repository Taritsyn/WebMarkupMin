﻿using System.Collections.Generic;
using System.Text;

using AdvancedStringBuilder;

using WebMarkupMin.Core.Parsers;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// Advanced HTML minification settings
	/// </summary>
	public abstract class AdvancedHtmlMinificationSettingsBase : CommonHtmlMinificationSettingsBase
	{
		/// <summary>
		/// Gets or sets a flag for whether to remove CDATA sections from scripts and styles
		/// </summary>
		public bool RemoveCdataSectionsFromScriptsAndStyles
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a custom short DOCTYPE (e.g. <c>&lt;!DOCTYPE HTML&gt;</c>,
		/// <c>&lt;!doctype html&gt;</c>, or <c>&lt;!doctypehtml&gt;</c>)
		/// </summary>
		public string CustomShortDoctype
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to preserve case of tag and attribute names
		/// (useful for Angular 2 templates)
		/// </summary>
		public bool PreserveCase
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a render mode of HTML empty tag
		/// </summary>
		public HtmlEmptyTagRenderMode EmptyTagRenderMode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove optional end tags
		/// </summary>
		public bool RemoveOptionalEndTags
		{
			get;
			set;
		}

		#region Preservable optional tags

		/// <summary>
		/// Collection of optional tags, which should not be removed
		/// </summary>
		private readonly HashSet<string> _preservableOptionalTags;

		/// <summary>
		/// Gets a collection of optional tags, which should not be removed
		/// </summary>
		public ISet<string> PreservableOptionalTagCollection
		{
			get
			{
				return _preservableOptionalTags;
			}
		}

		/// <summary>
		/// Sets a preservable optional tags
		/// </summary>
		/// <param name="optionalTags">Collection of preservable optional tags</param>
		public int SetPreservableOptionalTags(IEnumerable<string> optionalTags)
		{
			_preservableOptionalTags.Clear();

			if (optionalTags != null)
			{
				foreach (string optionalTag in optionalTags)
				{
					AddPreservableOptionalTag(optionalTag);
				}
			}

			return _preservableOptionalTags.Count;
		}

		/// <summary>
		/// Adds a preservable optional tag to the list
		/// </summary>
		/// <param name="optionalTag">Preservable optional tag</param>
		/// <returns><c>true</c> - valid optional tag; <c>false</c> - invalid optional tag</returns>
		public bool AddPreservableOptionalTag(string optionalTag)
		{
			if (!string.IsNullOrWhiteSpace(optionalTag))
			{
				string processedOptionalTag = optionalTag.Trim().ToLowerInvariant();
				if (HtmlTagTypeDeterminer.Instance.IsOptionalTag(processedOptionalTag))
				{
					_preservableOptionalTags.Add(processedOptionalTag);
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Gets or sets a comma-separated list of names of optional tags, which should not be removed
		/// </summary>
		public string PreservableOptionalTagList
		{
			get
			{
				if (_preservableOptionalTags.Count == 0)
				{
					return string.Empty;
				}

				var stringBuilderPool = StringBuilderPool.Shared;
				StringBuilder sb = stringBuilderPool.Rent();

				foreach (string optionalTag in _preservableOptionalTags)
				{
					if (sb.Length > 0)
					{
						sb.Append(",");
					}
					sb.Append(optionalTag);
				}

				string preservableOptionalTagList = sb.ToString();
				stringBuilderPool.Return(sb);

				return preservableOptionalTagList;
			}
			set
			{
				_preservableOptionalTags.Clear();

				if (!string.IsNullOrWhiteSpace(value))
				{
					string[] optionalTags = value.Split(',');

					foreach (string optionalTag in optionalTags)
					{
						AddPreservableOptionalTag(optionalTag);
					}
				}
			}
		}

		#endregion

		/// <summary>
		/// Gets or sets a flag for whether to remove values from boolean attributes
		/// </summary>
		public bool CollapseBooleanAttributes
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a removal mode of HTML attribute quotes
		/// </summary>
		public HtmlAttributeQuotesRemovalMode AttributeQuotesRemovalMode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove <c>type="text/javascript"</c> from <c>script</c> tags
		/// </summary>
		public bool RemoveJsTypeAttributes
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove <c>type="text/css"</c> from
		/// <c>style</c> and <c>link</c> tags
		/// </summary>
		public bool RemoveCssTypeAttributes
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs instance of advanced HTML minification settings
		/// </summary>
		/// <param name="useEmptyMinificationSettings">Initiates the creation of
		/// empty advanced HTML minification settings</param>
		protected AdvancedHtmlMinificationSettingsBase(bool useEmptyMinificationSettings)
			: base(useEmptyMinificationSettings)
		{
			// No default preservable optional tags
			_preservableOptionalTags = new HashSet<string>();
		}
	}
}