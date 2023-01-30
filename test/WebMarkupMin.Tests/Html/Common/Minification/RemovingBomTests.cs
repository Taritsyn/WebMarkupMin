using System.IO;
using System.Text;

using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class RemovingBomTests
	{
		private const string HTML_FILES_DIRECTORY_PATH = @"Files/html/";


		[Fact]
		public void RemovingBomAtStart()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			string inputFilePath = Path.Combine(HTML_FILES_DIRECTORY_PATH, "html-document-with-bom-at-start.html");
			byte[] inputBytes = File.ReadAllBytes(inputFilePath);
			string inputContent = Encoding.UTF8.GetString(inputBytes);

			string targetOutputFilePath = Path.Combine(HTML_FILES_DIRECTORY_PATH, "html-document-without-bom.html");
			byte[] targetOutputBytes = File.ReadAllBytes(targetOutputFilePath);

			// Act
			string outputContent = minifier.Minify(inputContent).MinifiedContent;
			byte[] outputBytes = Encoding.UTF8.GetBytes(outputContent);

			// Assert
			Assert.Equal(targetOutputBytes, outputBytes);
		}

		[Fact]
		public void RemovingBomFromBodyTag()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			string inputFilePath = Path.Combine(HTML_FILES_DIRECTORY_PATH, "html-document-with-bom-in-body-tag.html");
			byte[] inputBytes = File.ReadAllBytes(inputFilePath);
			string inputContent = Encoding.UTF8.GetString(inputBytes);

			string targetOutputFilePath = Path.Combine(HTML_FILES_DIRECTORY_PATH, "html-document-without-bom.html");
			byte[] targetOutputBytes = File.ReadAllBytes(targetOutputFilePath);

			// Act
			string outputContent = minifier.Minify(inputContent).MinifiedContent;
			byte[] outputBytes = Encoding.UTF8.GetBytes(outputContent);

			// Assert
			Assert.Equal(targetOutputBytes, outputBytes);
		}
	}
}