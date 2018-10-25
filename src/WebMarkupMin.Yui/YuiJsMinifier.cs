using System.Collections.Generic;
using System.Text;

using EcmaScript.NET;
using Yahoo.Yui.Compressor;

using WebMarkupMin.Core;
using WebMarkupMin.Yui.Reporters;

namespace WebMarkupMin.Yui
{
	/// <summary>
	/// Minifier, which produces minifiction of JS-code by using the YUI JS Compressor for .NET
	/// </summary>
	public sealed class YuiJsMinifier : YuiMinifierBase, IJsMinifier
	{
		/// <summary>
		/// Settings of the YUI JS Minifier
		/// </summary>
		private readonly YuiJsMinificationSettings _settings;

		/// <summary>
		/// Error reporter
		/// </summary>
		private YuiJsErrorReporter _errorReporter;

		/// <summary>
		/// Original JS minifier
		/// </summary>
		private JavaScriptCompressor _originalJsMinifier;

		/// <summary>
		/// Synchronizer of minification
		/// </summary>
		private readonly object _minificationSynchronizer = new object();


		/// <summary>
		/// Constructs an instance of the YUI JS Minifier
		/// </summary>
		public YuiJsMinifier()
			: this(new YuiJsMinificationSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the YUI JS Minifier
		/// </summary>
		/// <param name="settings">Settings of the YUI JS Minifier</param>
		public YuiJsMinifier(YuiJsMinificationSettings settings)
		{
			_settings = settings;
		}


		/// <summary>
		/// Creates a instance of original JS minifier
		/// </summary>
		/// <param name="settings">JS minifier settings</param>
		/// <returns>Instance of original JS minifier</returns>
		private static JavaScriptCompressor CreateOriginalJsMinifierInstance(
			YuiJsMinificationSettings settings)
		{
			var originalMinifier = new JavaScriptCompressor();
			ApplyCommonSettingsToOriginalMinifier(originalMinifier, settings);
			originalMinifier.ObfuscateJavascript = settings.ObfuscateJavascript;
			originalMinifier.PreserveAllSemicolons = settings.PreserveAllSemicolons;
			originalMinifier.DisableOptimizations = settings.DisableOptimizations;
			originalMinifier.IgnoreEval = settings.IgnoreEval;

			return originalMinifier;
		}


		#region IJsMinifier implementation

		/// <summary>
		/// Gets a value indicating the minifier supports inline code minification
		/// </summary>
		public bool IsInlineCodeMinificationSupported
		{
			get { return false; }
		}


		/// <summary>
		/// Produces a code minifiction of JS content by using the YUI JS Compressor for .NET
		/// </summary>
		/// <param name="content">JS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <returns>Minified JS content</returns>
		public CodeMinificationResult Minify(string content, bool isInlineCode)
		{
			return Minify(content, isInlineCode, Encoding.Default);
		}

		/// <summary>
		/// Produces a code minifiction of JS content by using the YUI JS Compressor for .NET
		/// </summary>
		/// <param name="content">JS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <param name="encoding">Text encoding</param>
		/// <returns>Minified JS content</returns>
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
					_errorReporter = new YuiJsErrorReporter();
				}

				if (_originalJsMinifier == null)
				{
					_originalJsMinifier = CreateOriginalJsMinifierInstance(_settings);
				}

				_originalJsMinifier.ErrorReporter = _errorReporter;
				_originalJsMinifier.Encoding = encoding;

				try
				{
					newContent = _originalJsMinifier.Compress(content);
				}
				catch (EcmaScriptRuntimeException e)
				{
					errors.Add(new MinificationErrorInfo(e.Message, e.LineNumber, e.ColumnNumber, e.LineSource));
				}
				catch (EcmaScriptException e)
				{
					errors.Add(new MinificationErrorInfo(e.Message, e.LineNumber, e.ColumnNumber, e.LineSource));
				}
				finally
				{
					errors.AddRange(_errorReporter.Errors);
					warnings.AddRange(_errorReporter.Warnings);

					_errorReporter.Clear();

					_originalJsMinifier.ErrorReporter = null;
					_originalJsMinifier.Encoding = Encoding.Default;
				}
			}

			return new CodeMinificationResult(newContent, errors, warnings);
		}

		#endregion
	}
}