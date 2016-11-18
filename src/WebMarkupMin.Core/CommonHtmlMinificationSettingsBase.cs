﻿using System.Collections.Generic;
using System.Text;

using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// Common HTML minification settings
	/// </summary>
	public abstract class CommonHtmlMinificationSettingsBase
	{
		/// <summary>
		/// Gets or sets a whitespace minification mode
		/// </summary>
		public WhitespaceMinificationMode WhitespaceMinificationMode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove all HTML comments
		/// except conditional, noindex, KnockoutJS containerless comments
		/// and AngularJS 1.X comment directives
		/// </summary>
		public bool RemoveHtmlComments
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove HTML comments from scripts and styles
		/// </summary>
		public bool RemoveHtmlCommentsFromScriptsAndStyles
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to use short DOCTYPE
		/// </summary>
		public bool UseShortDoctype
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to use META charset tag
		/// </summary>
		public bool UseMetaCharsetTag
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove tags without content
		/// (except for <code>textarea</code>, <code>tr</code>, <code>th</code> and <code>td</code> tags,
		/// and tags with <code>class</code>, <code>id</code>, <code>name</code>, <code>role</code>,
		/// <code>src</code> and custom attributes)
		/// </summary>
		public bool RemoveTagsWithoutContent
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove attributes, that has empty value
		/// (valid attributes are: <code>class</code>, <code>id</code>, <code>name</code>,
		/// <code>style</code>, <code>title</code>, <code>lang</code>, <code>dir</code>, event attributes,
		/// <code>action</code> of <code>form</code> tag and <code>value</code> of <code>input</code> tag)
		/// </summary>
		public bool RemoveEmptyAttributes
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove redundant attributes
		/// </summary>
		public bool RemoveRedundantAttributes
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove the HTTP protocol portion
		/// (<code>http:</code>) from URI-based attributes
		/// </summary>
		public bool RemoveHttpProtocolFromAttributes
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove the HTTPS protocol portion
		/// (<code>https:</code>) from URI-based attributes
		/// </summary>
		public bool RemoveHttpsProtocolFromAttributes
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove the <code>javascript:</code>
		/// pseudo-protocol portion from event attributes
		/// </summary>
		public bool RemoveJsProtocolFromAttributes
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to minify CSS code in <code>style</code> tags
		/// </summary>
		public bool MinifyEmbeddedCssCode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to minify CSS code in <code>style</code> attributes
		/// </summary>
		public bool MinifyInlineCssCode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to minify JavaScript code in <code>script</code> tags
		/// </summary>
		public bool MinifyEmbeddedJsCode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to minify JS code in event attributes
		/// and hyperlinks with <code>javascript:</code> pseudo-protocol
		/// </summary>
		public bool MinifyInlineJsCode
		{
			get;
			set;
		}

		#region Processable script types

		/// <summary>
		/// Collection of types of <code>script</code> tags, that are processed by minifier
		/// </summary>
		private readonly HashSet<string> _processableScriptTypes;

		/// <summary>
		/// Gets a collection of types of <code>script</code> tags, that are processed by minifier
		/// </summary>
		public ISet<string> ProcessableScriptTypeCollection
		{
			get
			{
				return _processableScriptTypes;
			}
		}

		/// <summary>
		/// Sets a processable script types
		/// </summary>
		/// <param name="scriptTypes">Collection of processable script types</param>
		public int SetProcessableScriptTypes(IEnumerable<string> scriptTypes)
		{
			_processableScriptTypes.Clear();

			if (scriptTypes != null)
			{
				foreach (string scriptType in scriptTypes)
				{
					AddProcessableScriptType(scriptType);
				}
			}

			return _processableScriptTypes.Count;
		}

		/// <summary>
		/// Adds a processable script type to the list
		/// </summary>
		/// <param name="scriptType">Processable script type</param>
		/// <returns>true - valid script type; false - invalid script type</returns>
		public bool AddProcessableScriptType(string scriptType)
		{
			if (!string.IsNullOrWhiteSpace(scriptType))
			{
				string processedScriptType = scriptType.Trim().ToLowerInvariant();
				_processableScriptTypes.Add(processedScriptType);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Gets or sets a comma-separated list of types of <code>script</code> tags, that are processed by minifier
		/// (e.g. <code>"text/html, text/ng-template"</code>)
		/// </summary>
		public string ProcessableScriptTypeList
		{
			get
			{
				StringBuilder sb = StringBuilderPool.GetBuilder();

				foreach (string scriptType in _processableScriptTypes)
				{
					if (sb.Length > 0)
					{
						sb.Append(",");
					}
					sb.Append(scriptType);
				}

				string processableScriptTypeList = sb.ToString();
				StringBuilderPool.ReleaseBuilder(sb);

				return processableScriptTypeList;
			}
			set
			{
				_processableScriptTypes.Clear();

				if (!string.IsNullOrWhiteSpace(value))
				{
					string[] scriptTypes = value.Split(',');

					foreach (string scriptType in scriptTypes)
					{
						AddProcessableScriptType(scriptType);
					}
				}
			}
		}

		#endregion

		/// <summary>
		/// Gets or sets a flag for whether to minify the KnockoutJS binding expressions
		/// in <code>data-bind</code> attributes and containerless comments
		/// </summary>
		public bool MinifyKnockoutBindingExpressions
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to minify the AngularJS 1.X binding expressions
		/// in Mustache-style tags (<code>{{}}</code>) and directives
		/// </summary>
		public bool MinifyAngularBindingExpressions
		{
			get;
			set;
		}

		#region Custom Angular directives with expressions

		/// <summary>
		/// Collection of names of custom AngularJS 1.X directives, that contain expressions
		/// </summary>
		private readonly HashSet<string> _customAngularDirectives;

		/// <summary>
		/// Gets a collection of names of custom AngularJS 1.X directives, that contain expressions
		/// </summary>
		public ISet<string> CustomAngularDirectiveCollection
		{
			get
			{
				return _customAngularDirectives;
			}
		}

		/// <summary>
		/// Sets a names of custom AngularJS 1.X directives
		/// </summary>
		/// <param name="directiveNames">Collection of names of custom AngularJS 1.X directives</param>
		public int SetCustomAngularDirectives(IEnumerable<string> directiveNames)
		{
			_customAngularDirectives.Clear();

			if (directiveNames != null)
			{
				foreach (string directiveName in directiveNames)
				{
					AddCustomAngularDirective(directiveName);
				}
			}

			return _customAngularDirectives.Count;
		}

		/// <summary>
		/// Adds a name of custom AngularJS 1.X directive to the list
		/// </summary>
		/// <param name="directiveName">Name of custom AngularJS 1.X directive</param>
		/// <returns>true - valid directive name; false - invalid directive name</returns>
		public bool AddCustomAngularDirective(string directiveName)
		{
			if (!string.IsNullOrWhiteSpace(directiveName))
			{
				string processedDirectiveName = directiveName.Trim();
				_customAngularDirectives.Add(processedDirectiveName);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Gets or sets a comma-separated list of names of custom AngularJS 1.X
		/// directives (e.g. <code>"myDir, btfCarousel"</code>), that contain expressions
		/// </summary>
		public string CustomAngularDirectiveList
		{
			get
			{
				StringBuilder sb = StringBuilderPool.GetBuilder();

				foreach (string directiveName in _customAngularDirectives)
				{
					if (sb.Length > 0)
					{
						sb.Append(",");
					}
					sb.Append(directiveName);
				}

				string customAngularDirectiveList = sb.ToString();
				StringBuilderPool.ReleaseBuilder(sb);

				return customAngularDirectiveList;
			}
			set
			{
				_customAngularDirectives.Clear();

				if (!string.IsNullOrWhiteSpace(value))
				{
					string[] directiveNames = value.Split(',');

					foreach (string directiveName in directiveNames)
					{
						AddCustomAngularDirective(directiveName);
					}
				}
			}
		}

        #endregion

        #region Tags with redundant attributes that should be preserved
        /// <summary>
        /// Collection of names of the tags with redundant attributes to be preserved
        /// </summary>
	    private readonly HashSet<string> _preservableRedundantAttributeTags;

        /// <summary>
        /// Gets a collection of names of the tags with redundant attributes to be preserved
        /// </summary>
	    public ISet<string> PreservableRedundantAttributeTagsCollection
        {
	        get
	        {
	            return  _preservableRedundantAttributeTags;
	        }
	    }

        /// <summary>
        /// Sets a names of the tags with redundant attributes to be preserved
        /// </summary>
        /// <param name="tagsNames">Collection of names of the tags with redundant attributes to be preserved</param>
        public int SetPreservableRedundantAttributeTags(IEnumerable<string> tagsNames)
        {
            _preservableRedundantAttributeTags.Clear();

            if (tagsNames != null)
            {
                foreach (string tagName in tagsNames)
                {
                    AddPreservableRedundantAttributeTag(tagName);
                }
            }

            return _preservableRedundantAttributeTags.Count;
        }

        /// <summary>
        /// Adds a name of a tag with redundant attributes to be preserved
        /// </summary>
        /// <param name="tagName">Name of html tag</param>
        /// <returns>true - valid tag name; false - invalid tag name</returns>
        public bool AddPreservableRedundantAttributeTag(string tagName)
        {
            if (!string.IsNullOrWhiteSpace(tagName))
            {
                string processedTagName = tagName.Trim();
                _preservableRedundantAttributeTags.Add(processedTagName);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets or sets a comma-separated list names of the tags with redundant attributes
        /// (e.g. <code>"form, input, script, link"</code>), for which their redundant attributes should not be removed
        /// </summary>
        public string PreservableRedundantAttributeTagsList
        {
            get
            {
                StringBuilder sb = StringBuilderPool.GetBuilder();

                foreach (string tagName in _preservableRedundantAttributeTags)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(tagName);
                }

                string customTagsWithPreservedRedundantAttributesList = sb.ToString();
                StringBuilderPool.ReleaseBuilder(sb);

                return customTagsWithPreservedRedundantAttributesList;
            }
            set
            {
                _preservableRedundantAttributeTags.Clear();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    string[] tagNames = value.Split(',');

                    foreach (string tagName in tagNames)
                    {
                        AddPreservableRedundantAttributeTag(tagName);
                    }
                }
            }
        }


        #endregion
       
        /// <summary>
        /// Constructs instance of common HTML minification settings
        /// </summary>
        /// <param name="useEmptyMinificationSettings">Initiates the creation of
        /// empty common HTML minification settings</param>
        protected CommonHtmlMinificationSettingsBase(bool useEmptyMinificationSettings)
		{
			if (!useEmptyMinificationSettings)
			{
				WhitespaceMinificationMode = WhitespaceMinificationMode.Medium;
				RemoveHtmlComments = true;
				RemoveHtmlCommentsFromScriptsAndStyles = true;
				RemoveEmptyAttributes = true;
				RemoveJsProtocolFromAttributes = true;
				MinifyEmbeddedCssCode = true;
				MinifyInlineCssCode = true;
				MinifyEmbeddedJsCode = true;
				MinifyInlineJsCode = true;
				_processableScriptTypes = new HashSet<string> { "text/html" };
			}
			else
			{
				WhitespaceMinificationMode = WhitespaceMinificationMode.None;
				RemoveHtmlComments = false;
				RemoveHtmlCommentsFromScriptsAndStyles = false;
				RemoveEmptyAttributes = false;
				RemoveJsProtocolFromAttributes = false;
				MinifyEmbeddedCssCode = false;
				MinifyInlineCssCode = false;
				MinifyEmbeddedJsCode = false;
				MinifyInlineJsCode = false;
				_processableScriptTypes = new HashSet<string>();
			}
			RemoveTagsWithoutContent = false;
			RemoveRedundantAttributes = false;
			RemoveHttpProtocolFromAttributes = false;
			RemoveHttpsProtocolFromAttributes = false;
			MinifyKnockoutBindingExpressions = false;
			MinifyAngularBindingExpressions = false;

			// No default custom Angular directives with expressions
			_customAngularDirectives = new HashSet<string>();

            // No default tags with redundant attributes to be preserved
            _preservableRedundantAttributeTags = new HashSet<string>();
        }
    }
}