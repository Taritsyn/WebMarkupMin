using System.Text;

namespace WebMarkupMin.Core
{
	public interface IMarkupMinifier
	{
		/// <summary>
		/// Minify markup
		/// </summary>
		/// <param name="content">Text content</param>
		/// <returns>Minification result</returns>
		MarkupMinificationResult Minify(string content);

		/// <summary>
		/// Minify markup
		/// </summary>
		/// <param name="content">Text content</param>
		/// <param name="fileContext">File context</param>
		/// <returns>Minification result</returns>
		MarkupMinificationResult Minify(string content, string fileContext);

		/// <summary>
		/// Minify markup
		/// </summary>
		/// <param name="content">Text content</param>
		/// <param name="generateStatistics">Flag for whether to allow generate minification statistics</param>
		/// <returns>Minification result</returns>
		MarkupMinificationResult Minify(string content, bool generateStatistics);

		/// <summary>
		/// Minify markup
		/// </summary>
		/// <param name="content">Text content</param>
		/// <param name="encoding">Text encoding</param>
		/// <returns>Minification result</returns>
		MarkupMinificationResult Minify(string content, Encoding encoding);

		/// <summary>
		/// Minify markup
		/// </summary>
		/// <param name="content">Text content</param>
		/// <param name="fileContext">File context</param>
		/// <param name="encoding">Text encoding</param>
		/// <param name="generateStatistics">Flag for whether to allow generate minification statistics</param>
		/// <returns>Minification result</returns>
		MarkupMinificationResult Minify(string content, string fileContext, Encoding encoding, bool generateStatistics);
	}
}