using System.Collections.Generic;
using System.Text;

using WebMarkupMin.Core.DouglasCrockford;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// Minifier, which produces minifiction of JS-code and Angular binding expressions
	/// by using C# port of Douglas Crockford's JSMin (version of October 30, 2019)
	/// </summary>
	public sealed class CrockfordJsMinifier : IJsMinifier
	{
		/// <summary>
		/// Original JS minifier
		/// </summary>
		private readonly JsMinifier _originalJsMinifier = new JsMinifier();


		private CodeMinificationResult InnerMinify(string content, bool isAngularBindingExpression)
		{
			if (string.IsNullOrWhiteSpace(content))
			{
				return new CodeMinificationResult(string.Empty);
			}

			string newContent = string.Empty;
			var errors = new List<MinificationErrorInfo>();

			try
			{
				newContent = _originalJsMinifier.Minify(content, isAngularBindingExpression);
			}
			catch (JsMinificationException e)
			{
				errors.Add(new MinificationErrorInfo(e.Message));
			}

			var minificationResult = new CodeMinificationResult(newContent, errors);

			return minificationResult;
		}

		#region IJsMinifier implementation

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
			return InnerMinify(content, false);
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
			return InnerMinify(content, false);
		}

		#endregion

		/// <summary>
		/// Produces code minifiction of Angular binding expression by using C# port of
		/// Douglas Crockford's JSMin
		/// </summary>
		/// <param name="expression">Binding expression</param>
		/// <returns>Minification result</returns>
		public CodeMinificationResult MinifyAngularBindingExpression(string expression)
		{
			return InnerMinify(expression, true);
		}

		/// <summary>
		/// Produces code minifiction of Angular binding expression by using C# port of
		/// Douglas Crockford's JSMin
		/// </summary>
		/// <param name="expression">Binding expression</param>
		/// <param name="encoding">Text encoding</param>
		/// <returns>Minification result</returns>
		public CodeMinificationResult MinifyAngularBindingExpression(string expression, Encoding encoding)
		{
			return InnerMinify(expression, true);
		}
	}
}