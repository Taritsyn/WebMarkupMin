using System.IO;
using System.Text;

using Xunit;

namespace WebMarkupMin.Core.Tests.Html.Minification
{
	public class RemovingBomTests : FileSystemTestsBase
	{
		private readonly string _htmlFilesDirectoryPath;


		public RemovingBomTests()
		{
			_htmlFilesDirectoryPath = Path.GetFullPath(Path.Combine(_baseDirectoryPath, @"../SharedFiles/html/"));
		}


		[Fact]
		public void RemovingBomAtStartIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			string inputFilePath = Path.Combine(_htmlFilesDirectoryPath, "html-document-with-bom-at-start.html");
			byte[] inputBytes = File.ReadAllBytes(inputFilePath);
			string inputContent = Encoding.UTF8.GetString(inputBytes);

			string targetOutputFilePath = Path.Combine(_htmlFilesDirectoryPath, "html-document-without-bom.html");
			byte[] targetOutputBytes = File.ReadAllBytes(targetOutputFilePath);

			// Act
			string outputContent = minifier.Minify(inputContent).MinifiedContent;
			byte[] outputBytes = Encoding.UTF8.GetBytes(outputContent);

			// Assert
			Assert.Equal(targetOutputBytes, outputBytes);
		}

		[Fact]
		public void RemovingBomFromBodyTagIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			string inputFilePath = Path.Combine(_htmlFilesDirectoryPath, "html-document-with-bom-in-body-tag.html");
			byte[] inputBytes = File.ReadAllBytes(inputFilePath);
			string inputContent = Encoding.UTF8.GetString(inputBytes);

			string targetOutputFilePath = Path.Combine(_htmlFilesDirectoryPath, "html-document-without-bom.html");
			byte[] targetOutputBytes = File.ReadAllBytes(targetOutputFilePath);

			// Act
			string outputContent = minifier.Minify(inputContent).MinifiedContent;
			byte[] outputBytes = Encoding.UTF8.GetBytes(outputContent);

			// Assert
			Assert.Equal(targetOutputBytes, outputBytes);
		}
	}
}