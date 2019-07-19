using System.Collections.Generic;
using System.Text;

using Microsoft.Ajax.Utilities;
using MsCssColor = Microsoft.Ajax.Utilities.CssColor;
using MsCssComment = Microsoft.Ajax.Utilities.CssComment;

using WebMarkupMin.Core;
using WebMarkupMin.Core.Utilities;
using WebMarkupMin.MsAjax.Reporters;
using WmmCssColor = WebMarkupMin.MsAjax.CssColor;
using WmmCssComment = WebMarkupMin.MsAjax.CssComment;

namespace WebMarkupMin.MsAjax
{
	/// <summary>
	/// Minifier, which produces minifiction of CSS-code by using the Microsoft Ajax CSS Minifier
	/// </summary>
	public sealed class MsAjaxCssMinifier : MsAjaxMinifierBase, ICssMinifier
	{
		/// <summary>
		/// Microsoft Ajax CSS Minifier settings
		/// </summary>
		private readonly MsAjaxCssMinificationSettings _settings;

		/// <summary>
		/// Error reporter
		/// </summary>
		private MsAjaxErrorReporter _errorReporter;

		/// <summary>
		/// Original CSS parser for embedded code
		/// </summary>
		private CssParser _originalEmbeddedCssParser;

		/// <summary>
		/// Original CSS parser for inline code
		/// </summary>
		private CssParser _originalInlineCssParser;

		/// <summary>
		/// Synchronizer of minification
		/// </summary>
		private readonly object _minificationSynchronizer = new object();


		/// <summary>
		/// Constructs an instance of the Microsoft Ajax CSS Minifier
		/// </summary>
		public MsAjaxCssMinifier()
			: this(new MsAjaxCssMinificationSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the Microsoft Ajax CSS Minifier
		/// </summary>
		/// <param name="settings">Microsoft Ajax CSS Minifier settings</param>
		public MsAjaxCssMinifier(MsAjaxCssMinificationSettings settings)
		{
			_settings = settings;
		}


		/// <summary>
		/// Creates a instance of original CSS parser
		/// </summary>
		/// <param name="settings">CSS minifier settings</param>
		/// <param name="isInlineCode">Flag for whether to create a CSS parser for inline code</param>
		/// <returns>Instance of original CSS parser</returns>
		private static CssParser CreateOriginalCssParserInstance(MsAjaxCssMinificationSettings settings,
			bool isInlineCode)
		{
			var originalSettings = new CssSettings();
			MapCommonSettings(originalSettings, settings);
			originalSettings.ColorNames = Utils.GetEnumFromOtherEnum<WmmCssColor, MsCssColor>(
				settings.ColorNames);
			originalSettings.CommentMode = Utils.GetEnumFromOtherEnum<WmmCssComment, MsCssComment>(
				settings.CommentMode);
			originalSettings.CssType = isInlineCode ?
				CssType.DeclarationList : CssType.FullStyleSheet;
			originalSettings.MinifyExpressions = settings.MinifyExpressions;
			originalSettings.RemoveEmptyBlocks = settings.RemoveEmptyBlocks;

			var originalParser = new CssParser()
			{
				FileContext = string.Empty,
				Settings = originalSettings
			};

			return originalParser;
		}

		#region ICssMinifier implementation

		/// <summary>
		/// Gets a value indicating the minifier supports inline code minification
		/// </summary>
		public bool IsInlineCodeMinificationSupported
		{
			get { return true; }
		}

		/// <summary>
		/// Produces a code minifiction of CSS content by using the Microsoft Ajax CSS Minifier
		/// </summary>
		/// <param name="content">CSS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <returns>Minification result</returns>
		public CodeMinificationResult Minify(string content, bool isInlineCode)
		{
			return Minify(content, isInlineCode, TextEncodingShortcuts.Default);
		}

		/// <summary>
		/// Produces a code minifiction of CSS content by using the Microsoft Ajax CSS Minifier
		/// </summary>
		/// <param name="content">CSS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <param name="encoding">Text encoding</param>
		/// <returns>Minification result</returns>
		public CodeMinificationResult Minify(string content, bool isInlineCode, Encoding encoding)
		{
			if (string.IsNullOrWhiteSpace(content))
			{
				return new CodeMinificationResult(string.Empty);
			}

			string newContent = string.Empty;
			var errors = new List<MinificationErrorInfo>();
			var warnings = new List<MinificationErrorInfo>();

			lock (_minificationSynchronizer)
			{
				if (_errorReporter == null)
				{
					_errorReporter = new MsAjaxErrorReporter(_settings.WarningLevel);
				}

				CssParser originalCssParser = isInlineCode ?
					_originalInlineCssParser : _originalEmbeddedCssParser;
				if (originalCssParser == null)
				{
					originalCssParser = CreateOriginalCssParserInstance(_settings, isInlineCode);
					if (isInlineCode)
					{
						_originalInlineCssParser = originalCssParser;
					}
					else
					{
						_originalEmbeddedCssParser = originalCssParser;
					}
				}

				originalCssParser.CssError += _errorReporter.ParseErrorHandler;

				try
				{
					// Parse the input
					newContent = originalCssParser.Parse(content);
				}
				finally
				{
					originalCssParser.CssError -= _errorReporter.ParseErrorHandler;

					errors.AddRange(_errorReporter.Errors);
					warnings.AddRange(_errorReporter.Warnings);

					_errorReporter.Clear();
				}
			}

			return new CodeMinificationResult(newContent, errors, warnings);
		}

		#endregion
	}
}