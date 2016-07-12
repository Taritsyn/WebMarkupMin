using System.Text;
using System.Collections.Generic;

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
		/// Original JS minifier
		/// </summary>
		private readonly JavaScriptCompressor _originalJsMinifier;

		/// <summary>
		/// Synchronizer of minification
		/// </summary>
		private readonly object _minificationSynchronizer = new object();

		/// <summary>
		/// Gets a value indicating the minifier supports inline code minification
		/// </summary>
		public bool IsInlineCodeMinificationSupported
		{
			get { return false; }
		}


		/// <summary>
		/// Constructs an instance of the YUI JS Minifier
		/// </summary>
		public YuiJsMinifier() : this(new YuiJsMinificationSettings())
		{ }

		/// <summary>
		/// Constructs an instance of the YUI JS Minifier
		/// </summary>
		/// <param name="settings">Settings of YUI JS Minifier</param>
		public YuiJsMinifier(YuiJsMinificationSettings settings)
		{
			_originalJsMinifier = CreateOriginalJsMinifierInstance(settings);
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
			var errorReporter = new YuiJsErrorReporter();

			lock (_minificationSynchronizer)
			{
				_originalJsMinifier.ErrorReporter = errorReporter;
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
					_originalJsMinifier.ErrorReporter = null;
					_originalJsMinifier.Encoding = Encoding.Default;
				}
			}

			errors.AddRange(errorReporter.Errors);
			warnings.AddRange(errorReporter.Warnings);

			return new CodeMinificationResult(newContent, errors, warnings);
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
			ApplyJsSettingsToOriginalJsMinifier(originalMinifier, settings);

			return originalMinifier;
		}

		/// <summary>
		/// Applies a JS settings to original JS minifier
		/// </summary>
		/// <param name="originalMinifier">Original JS minifier</param>
		/// <param name="settings">JS minifier settings</param>
		private static void ApplyJsSettingsToOriginalJsMinifier(JavaScriptCompressor originalMinifier,
			YuiJsMinificationSettings settings)
		{
			ApplyCommonSettingsToOriginalMinifier(originalMinifier, settings);

			originalMinifier.ObfuscateJavascript = settings.ObfuscateJavascript;
			originalMinifier.PreserveAllSemicolons = settings.PreserveAllSemicolons;
			originalMinifier.DisableOptimizations = settings.DisableOptimizations;
			originalMinifier.IgnoreEval = settings.IgnoreEval;
		}
	}
}