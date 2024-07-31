using System.Collections.Generic;
using System.Text;

using AdvancedStringBuilder;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// Common HTML minification settings
	/// </summary>
	public abstract class CommonHtmlMinificationSettingsBase : MarkupMinificationSettingsBase
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
		/// except conditional, noindex, KnockoutJS containerless comments,
		/// AngularJS 1.X comment directives, React DOM component comments
		/// and Blazor markers
		/// </summary>
		public bool RemoveHtmlComments
		{
			get;
			set;
		}

		#region Preservable HTML comments

		/// <summary>
		/// Collection of the simple regular expressions, that define what HTML comments can not be removed
		/// </summary>
		private readonly List<SimpleRegex> _preservableHtmlComments;

		/// <summary>
		/// Gets a collection of the simple regular expressions, that define what HTML comments can not be removed
		/// </summary>
		public IList<SimpleRegex> PreservableHtmlCommentCollection
		{
			get { return _preservableHtmlComments; }
		}

		/// <summary>
		/// Sets a simple regular expressions, that define what HTML comments can not be removed
		/// </summary>
		/// <param name="regularExpressions">Collection of the simple regular expressions, that define what
		/// HTML comments can not be removed</param>
		public int SetPreservableHtmlComments(IEnumerable<SimpleRegex> regularExpressions)
		{
			_preservableHtmlComments.Clear();

			if (regularExpressions != null)
			{
				foreach (SimpleRegex regularExpression in regularExpressions)
				{
					AddPreservableHtmlComment(regularExpression);
				}
			}

			return _preservableHtmlComments.Count;
		}

		/// <summary>
		/// Adds a string representation of the simple regular expression, that define what HTML comments can not be
		/// removed, to the list
		/// </summary>
		/// <param name="regularExpressionString">String representation of the simple regular expression, that
		/// define what HTML comments can not be removed</param>
		/// <returns><c>true</c> - valid expression; <c>false</c> - invalid expression</returns>
		public bool AddPreservableHtmlComment(string regularExpressionString)
		{
			SimpleRegex regularExpression;

			if (SimpleRegex.TryParse(regularExpressionString, out regularExpression))
			{
				AddPreservableHtmlComment(regularExpression);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Adds a simple regular expression, that define what HTML comments can not be removed, to the list
		/// </summary>
		/// <param name="regularExpression">Simple regular expression, that define what HTML comments can not be removed</param>
		public void AddPreservableHtmlComment(SimpleRegex regularExpression)
		{
			if (regularExpression != null && !_preservableHtmlComments.Contains(regularExpression))
			{
				_preservableHtmlComments.Add(regularExpression);
			}
		}

		/// <summary>
		/// Gets or sets a comma-separated list of string representations of the simple regular expressions, that
		/// define what HTML comments can not be removed (e.g. <c>"/^\s*saved from url=\(\d+\)/i, /^\/?\$$/, /^[\[\]]$/"</c>)
		/// </summary>
		/// <remarks>
		/// Simple regular expressions somewhat similar to the ECMAScript regular expression literals.
		/// There are two varieties of the simple regular expressions: <c>/pattern/</c> and <c>/pattern/i</c>.
		/// </remarks>
		public string PreservableHtmlCommentList
		{
			get
			{
				if (_preservableHtmlComments.Count == 0)
				{
					return string.Empty;
				}

				var stringBuilderPool = StringBuilderPool.Shared;
				StringBuilder sb = stringBuilderPool.Rent();

				foreach (SimpleRegex regularExpression in _preservableHtmlComments)
				{
					if (sb.Length > 0)
					{
						sb.Append(",");
					}
					sb.Append(regularExpression.ToString());
				}

				string preservableHtmlCommentList = sb.ToString();
				stringBuilderPool.Return(sb);

				return preservableHtmlCommentList;
			}
			set
			{
				_preservableHtmlComments.Clear();

				if (string.IsNullOrWhiteSpace(value))
				{
					return;
				}

				if (value.IndexOf(',') != -1)
				{
					List<SimpleRegex> regularExpressions;

					if (SimpleRegex.TryParseList(value, out regularExpressions))
					{
						SetPreservableHtmlComments(regularExpressions);
					}
				}
				else
				{
					SimpleRegex regularExpression;

					if (SimpleRegex.TryParse(value, out regularExpression))
					{
						AddPreservableHtmlComment(regularExpression);
					}
				}
			}
		}

		#endregion

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
		/// (except for <c>textarea</c>, <c>tr</c>, <c>th</c> and <c>td</c> tags,
		/// and tags with <c>class</c>, <c>id</c>, <c>name</c>, <c>role</c>,
		/// <c>src</c> and custom attributes)
		/// </summary>
		public bool RemoveTagsWithoutContent
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a style of the HTML attribute quotes
		/// </summary>
		public HtmlAttributeQuotesStyle AttributeQuotesStyle
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove attributes, that has empty value
		/// (valid attributes are: <c>class</c>, <c>id</c>, <c>name</c>,
		/// <c>style</c>, <c>title</c>, <c>lang</c>, event attributes,
		/// <c>action</c> of <c>form</c> tag and <c>value</c> of <c>input</c> tag)
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

		#region Preservable attributes

		/// <summary>
		/// Collection of attribute expressions, that define what attributes can not be removed
		/// </summary>
		private readonly List<HtmlAttributeExpression> _preservableAttributes;

		/// <summary>
		/// Gets a collection of attribute expressions, that define what attributes can not be removed
		/// </summary>
		public IList<HtmlAttributeExpression> PreservableAttributeCollection
		{
			get { return _preservableAttributes; }
		}

		/// <summary>
		/// Sets a attribute expressions, that define what attributes can not be removed
		/// </summary>
		/// <param name="attributeExpressions">Collection of attribute expressions, that define what
		/// attributes can not be removed</param>
		public int SetPreservableAttributes(IEnumerable<HtmlAttributeExpression> attributeExpressions)
		{
			_preservableAttributes.Clear();

			if (attributeExpressions != null)
			{
				foreach (HtmlAttributeExpression attributeExpression in attributeExpressions)
				{
					AddPreservableAttribute(attributeExpression);
				}
			}

			return _preservableAttributes.Count;
		}

		/// <summary>
		/// Adds a string representation of attribute expression, that define what attributes can not be
		/// removed, to the list
		/// </summary>
		/// <param name="attributeExpressionString">String representation of attribute expression, that
		/// define what attributes can not be removed</param>
		/// <returns><c>true</c> - valid expression; <c>false</c> - invalid expression</returns>
		public bool AddPreservableAttribute(string attributeExpressionString)
		{
			HtmlAttributeExpression attributeExpression;

			if (HtmlAttributeExpression.TryParse(attributeExpressionString, out attributeExpression))
			{
				AddPreservableAttribute(attributeExpression);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Adds a attribute expression, that define what attributes can not be removed, to the list
		/// </summary>
		/// <param name="attributeExpression">Attribute expression, that define what attributes can not be removed</param>
		public void AddPreservableAttribute(HtmlAttributeExpression attributeExpression)
		{
			if (!_preservableAttributes.Contains(attributeExpression))
			{
				_preservableAttributes.Add(attributeExpression);
			}
		}

		/// <summary>
		/// Gets or sets a comma-separated list of string representations of attribute expressions, that
		/// define what attributes can not be removed (e.g. <c>"form[method=get i], input[type], [xmlns]"</c>)
		/// </summary>
		/// <remarks>
		/// Attribute expressions somewhat similar to the CSS Attribute Selectors.
		/// There are six varieties of the attribute expressions: <c>[attrName]</c>, <c>tagName[attrName]</c>,
		/// <c>[attrName=attrValue]</c>, <c>tagName[attrName=attrValue]</c>, <c>[attrName=attrValue i]</c> and
		/// <c>tagName[attrName=attrValue i]</c>.
		/// </remarks>
		public string PreservableAttributeList
		{
			get
			{
				if (_preservableAttributes.Count == 0)
				{
					return string.Empty;
				}

				var stringBuilderPool = StringBuilderPool.Shared;
				StringBuilder sb = stringBuilderPool.Rent();

				foreach (HtmlAttributeExpression attributeExpression in _preservableAttributes)
				{
					if (sb.Length > 0)
					{
						sb.Append(",");
					}
					sb.Append(attributeExpression);
				}

				string preservableAttributeList = sb.ToString();
				stringBuilderPool.Return(sb);

				return preservableAttributeList;
			}
			set
			{
				_preservableAttributes.Clear();

				if (!string.IsNullOrWhiteSpace(value))
				{
					string[] attributeExpressions = value.Split(',');

					foreach (string attributeExpression in attributeExpressions)
					{
						AddPreservableAttribute(attributeExpression);
					}
				}
			}
		}

		#endregion

		/// <summary>
		/// Gets or sets a flag for whether to remove the HTTP protocol portion
		/// (<c>http:</c>) from URI-based attributes
		/// </summary>
		public bool RemoveHttpProtocolFromAttributes
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove the HTTPS protocol portion
		/// (<c>https:</c>) from URI-based attributes
		/// </summary>
		public bool RemoveHttpsProtocolFromAttributes
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to remove the <c>javascript:</c>
		/// pseudo-protocol portion from event attributes
		/// </summary>
		public bool RemoveJsProtocolFromAttributes
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to minify CSS code in <c>style</c> tags
		/// </summary>
		public bool MinifyEmbeddedCssCode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to minify CSS code in <c>style</c> attributes
		/// </summary>
		public bool MinifyInlineCssCode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to minify JavaScript code in <c>script</c> tags
		/// </summary>
		public bool MinifyEmbeddedJsCode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to minify JS code in event attributes
		/// and hyperlinks with <c>javascript:</c> pseudo-protocol
		/// </summary>
		public bool MinifyInlineJsCode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to minify JSON data in <c>script</c> tags
		/// </summary>
		public bool MinifyEmbeddedJsonData
		{
			get;
			set;
		}

		#region Processable script types

		/// <summary>
		/// Collection of types of <c>script</c> tags, that are processed by minifier
		/// </summary>
		private readonly HashSet<string> _processableScriptTypes;

		/// <summary>
		/// Gets a collection of types of <c>script</c> tags, that are processed by minifier
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
		/// <returns><c>true</c> - valid script type; <c>false</c> - invalid script type</returns>
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
		/// Gets or sets a comma-separated list of types of <c>script</c> tags, that are processed by minifier
		/// (e.g. <c>"text/html, text/ng-template"</c>)
		/// </summary>
		public string ProcessableScriptTypeList
		{
			get
			{
				if (_processableScriptTypes.Count == 0)
				{
					return string.Empty;
				}

				var stringBuilderPool = StringBuilderPool.Shared;
				StringBuilder sb = stringBuilderPool.Rent();

				foreach (string scriptType in _processableScriptTypes)
				{
					if (sb.Length > 0)
					{
						sb.Append(",");
					}
					sb.Append(scriptType);
				}

				string processableScriptTypeList = sb.ToString();
				stringBuilderPool.Return(sb);

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
		/// in <c>data-bind</c> attributes and containerless comments
		/// </summary>
		public bool MinifyKnockoutBindingExpressions
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to minify the AngularJS 1.X binding expressions
		/// in Mustache-style tags (<c>{{}}</c>) and directives
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
		/// <returns><c>true</c> - valid directive name; <c>false</c> - invalid directive name</returns>
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
		/// directives (e.g. <c>"myDir, btfCarousel"</c>), that contain expressions
		/// </summary>
		public string CustomAngularDirectiveList
		{
			get
			{
				if (_customAngularDirectives.Count == 0)
				{
					return string.Empty;
				}

				var stringBuilderPool = StringBuilderPool.Shared;
				StringBuilder sb = stringBuilderPool.Rent();

				foreach (string directiveName in _customAngularDirectives)
				{
					if (sb.Length > 0)
					{
						sb.Append(",");
					}
					sb.Append(directiveName);
				}

				string customAngularDirectiveList = sb.ToString();
				stringBuilderPool.Return(sb);

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
				MinifyEmbeddedJsonData = true;
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
				MinifyEmbeddedJsonData = false;
				_processableScriptTypes = new HashSet<string>();
			}

			// No default preservable HTML comments
			_preservableHtmlComments = new List<SimpleRegex>();

			RemoveTagsWithoutContent = false;
			AttributeQuotesStyle = HtmlAttributeQuotesStyle.Auto;
			RemoveRedundantAttributes = false;
			RemoveHttpProtocolFromAttributes = false;
			RemoveHttpsProtocolFromAttributes = false;
			MinifyKnockoutBindingExpressions = false;
			MinifyAngularBindingExpressions = false;

			// No default preservable attribute expressions
			_preservableAttributes = new List<HtmlAttributeExpression>();

			// No default custom Angular directives with expressions
			_customAngularDirectives = new HashSet<string>();
		}
	}
}