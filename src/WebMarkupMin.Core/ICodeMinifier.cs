using System.Text;

namespace WebMarkupMin.Core
{
	/// <summary>
	/// Defines interface of code minifier
	/// </summary>
	public interface ICodeMinifier
	{
		/// <summary>
		/// Gets a value indicating the minifier supports inline code minification
		/// </summary>
		bool IsInlineCodeMinificationSupported
		{
			get;
		}


		/// <summary>
		/// Minify CSS content
		/// </summary>
		/// <param name="content">CSS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <returns>Minification result</returns>
		CodeMinificationResult Minify(string content, bool isInlineCode);

		/// <summary>
		/// Minify CSS content
		/// </summary>
		/// <param name="content">CSS content</param>
		/// <param name="isInlineCode">Flag whether the content is inline code</param>
		/// <param name="encoding">Text encoding</param>
		/// <returns>Minification result</returns>
		CodeMinificationResult Minify(string content, bool isInlineCode, Encoding encoding);
	}
}