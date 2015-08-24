using System.Text;
using System.Collections.Generic;

using EcmaScript.NET;
using Yahoo.Yui.Compressor;

using WebMarkupMin.Core;
using WebMarkupMin.Yui.Reporters;

namespace WebMarkupMin.Yui
{
	/// <summary>
	/// Minifier, which produces minifiction of JS-code
	/// by using YUI JS Compressor for .NET
	/// </summary>
	public sealed class YuiJsMinifier : YuiMinifierBase, IJsMinifier
	{
		/// <summary>
		/// Settings of YUI JS Minifier
		/// </summary>
		private readonly YuiJsMinificationSettings _settings;

		/// <summary>
		/// Gets a value indicating the minifier supports inline code minification
		/// </summary>
		public bool IsInlineCodeMinificationSupported
		{
			get { return false; }
		}


		/// <summary>
		/// Constructs instance of YUI JS Minifier
		/// </summary>
		public YuiJsMinifier() : this(new YuiJsMinificationSettings())
		{ }

		/// <summary>
		/// Constructs instance of YUI JS Minifier
		/// </summary>
		/// <param name="settings">Settings of YUI JS Minifier</param>
		public YuiJsMinifier(YuiJsMinificationSettings settings)
		{
			_settings = settings;
		}


		/// <summary>
		/// Produces code minifiction of JS content by using
		/// YUI JS Compressor for .NET
		/// </summary>
		/// <param name="content">JS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <returns>Minified JS content</returns>
		public CodeMinificationResult Minify(string content, bool isInlineCode)
		{
			return Minify(content, isInlineCode, Encoding.Default);
		}

		/// <summary>
		/// Produces code minifiction of JS content by using
		/// YUI JS Compressor for .NET
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

			var errors = new List<MinificationErrorInfo>();
			var warnings = new List<MinificationErrorInfo>();
			var errorReporter = new YuiJsErrorReporter();

			var jsCompressor = CreateJavaScriptCompressorInstance();
			jsCompressor.ErrorReporter = errorReporter;
			jsCompressor.Encoding = encoding;

			string newContent = string.Empty;

			try
			{
				newContent = jsCompressor.Compress(content);
			}
			catch (EcmaScriptRuntimeException e)
			{
				errors.Add(new MinificationErrorInfo(e.Message, e.LineNumber, e.ColumnNumber, e.LineSource));
			}
			catch (EcmaScriptException e)
			{
				errors.Add(new MinificationErrorInfo(e.Message, e.LineNumber, e.ColumnNumber, e.LineSource));
			}

			errors.AddRange(errorReporter.Errors);
			warnings.AddRange(errorReporter.Warnings);

			return new CodeMinificationResult(newContent, errors, warnings);
		}

		/// <summary>
		/// Creates a instance of JS-code compressor
		/// </summary>
		/// <returns>JavaScript-code compressor</returns>
		private JavaScriptCompressor CreateJavaScriptCompressorInstance()
		{
			var jsCompressor = new JavaScriptCompressor();
			ApplyJsSettingsToJsCompressor(jsCompressor, _settings);

			return jsCompressor;
		}

		/// <summary>
		/// Applies a settings to JS-code compressor
		/// </summary>
		/// <param name="jsCompressor">JS-code compressor</param>
		/// <param name="jsSettings">Settings of YUI JS Minifier</param>
		private static void ApplyJsSettingsToJsCompressor(JavaScriptCompressor jsCompressor,
			YuiJsMinificationSettings jsSettings)
		{
			ApplyCommonSettingsToCompressor(jsCompressor, jsSettings);

			jsCompressor.ObfuscateJavascript = jsSettings.ObfuscateJavascript;
			jsCompressor.PreserveAllSemicolons = jsSettings.PreserveAllSemicolons;
			jsCompressor.DisableOptimizations = jsSettings.DisableOptimizations;
			jsCompressor.IgnoreEval = jsSettings.IgnoreEval;
		}
	}
}