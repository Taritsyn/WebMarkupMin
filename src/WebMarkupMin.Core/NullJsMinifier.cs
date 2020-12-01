using System.Text;

using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// JS null minifier (used as a placeholder)
	/// </summary>
	public sealed class NullJsMinifier : IJsMinifier
	{
		/// <summary>
		/// Gets a value indicating the minifier supports inline code minification
		/// </summary>
		public bool IsInlineCodeMinificationSupported
		{
			get { return false; }
		}


		/// <summary>
		/// Do not performs operations with JS content
		/// </summary>
		/// <param name="content">JS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <returns>Minification result</returns>
		public CodeMinificationResult Minify(string content, bool isInlineCode)
		{
			return Minify(content, isInlineCode, TargetFrameworkShortcuts.DefaultTextEncoding);
		}

		/// <summary>
		/// Do not performs operations with JS content
		/// </summary>
		/// <param name="content">JS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <param name="encoding">Text encoding</param>
		/// <returns>Minification result</returns>
		public CodeMinificationResult Minify(string content, bool isInlineCode, Encoding encoding)
		{
			return new CodeMinificationResult(content);
		}
	}
}