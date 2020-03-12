using System.Collections.Generic;
using System.Text;

using NUglify.Css;
using NuCssColor = NUglify.Css.CssColor;
using NuCssComment = NUglify.Css.CssComment;

using WebMarkupMin.Core;
using WebMarkupMin.Core.Utilities;
using WebMarkupMin.NUglify.Reporters;
using WmmCssColor = WebMarkupMin.NUglify.CssColor;
using WmmCssComment = WebMarkupMin.NUglify.CssComment;

namespace WebMarkupMin.NUglify
{
	/// <summary>
	/// Minifier, which produces minifiction of CSS-code by using the NUglify CSS Minifier
	/// </summary>
	public sealed class NUglifyCssMinifier : NUglifyMinifierBase, ICssMinifier
	{
		/// <summary>
		/// NUglify CSS Minifier settings
		/// </summary>
		private readonly NUglifyCssMinificationSettings _settings;

		/// <summary>
		/// Error reporter
		/// </summary>
		private NUglifyErrorReporter _errorReporter;

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
		/// Constructs an instance of the NUglify CSS Minifier
		/// </summary>
		public NUglifyCssMinifier()
			: this(new NUglifyCssMinificationSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the NUglify CSS Minifier
		/// </summary>
		/// <param name="settings">NUglify CSS Minifier settings</param>
		public NUglifyCssMinifier(NUglifyCssMinificationSettings settings)
		{
			_settings = settings;
		}


		/// <summary>
		/// Creates a instance of original CSS parser
		/// </summary>
		/// <param name="settings">CSS minifier settings</param>
		/// <param name="isInlineCode">Flag for whether to create a CSS parser for inline code</param>
		/// <returns>Instance of original CSS parser</returns>
		private static CssParser CreateOriginalCssParserInstance(NUglifyCssMinificationSettings settings,
			bool isInlineCode)
		{
			var originalSettings = new CssSettings();
			MapCommonSettings(originalSettings, settings);
			originalSettings.ColorNames = Utils.GetEnumFromOtherEnum<WmmCssColor, NuCssColor>(
				settings.ColorNames);
			originalSettings.CommentMode = Utils.GetEnumFromOtherEnum<WmmCssComment, NuCssComment>(
				settings.CommentMode);
			originalSettings.CssType = isInlineCode ?
				CssType.DeclarationList : CssType.FullStyleSheet;
			originalSettings.DecodeEscapes = settings.DecodeEscapes;
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
		/// Produces a code minifiction of CSS content by using the NUglify CSS Minifier
		/// </summary>
		/// <param name="content">CSS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <returns>Minification result</returns>
		public CodeMinificationResult Minify(string content, bool isInlineCode)
		{
			return Minify(content, isInlineCode, TextEncodingShortcuts.Default);
		}

		/// <summary>
		/// Produces a code minifiction of CSS content by using the NUglify CSS Minifier
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
					_errorReporter = new NUglifyErrorReporter(_settings.WarningLevel);
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