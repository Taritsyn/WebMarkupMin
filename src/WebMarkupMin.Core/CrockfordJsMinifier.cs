using System.Collections.Generic;
using System.Text;

using WebMarkupMin.Core.DouglasCrockford;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// Minifier, which produces minifiction of JS-code
	/// by using C# port of Douglas Crockford's JSMin (version of March 29, 2013)
	/// </summary>
	public sealed class CrockfordJsMinifier : IJsMinifier
	{
		/// <summary>
		/// Gets a value indicating the minifier supports inline code minification
		/// </summary>
		public bool IsInlineCodeMinificationSupported
		{
			get { return true; }
		}


		/// <summary>
		/// Produces code minifiction of JS content by using C# port of
		/// Douglas Crockford's JSMin
		/// </summary>
		/// <param name="content">JS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <returns>Minification result</returns>
		public CodeMinificationResult Minify(string content, bool isInlineCode)
		{
			return Minify(content, isInlineCode, Encoding.GetEncoding(0));
		}

		/// <summary>
		/// Produces code minifiction of JS content by using C# port of
		/// Douglas Crockford's JSMin
		/// </summary>
		/// <param name="content">JS content</param>
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

			try
			{
				var jsMin = new JsMinifier();
				newContent = jsMin.Minify(content);
			}
			catch (JsMinificationException e)
			{
				errors.Add(new MinificationErrorInfo(e.Message));
			}

			var minificationResult = new CodeMinificationResult(newContent, errors);

			return minificationResult;
		}
	}
}