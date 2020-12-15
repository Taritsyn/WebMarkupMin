using System.IO;
using System.Text;

using Xunit;

namespace WebMarkupMin.Core.Tests.Xml.Minification
{
	public class RemovingBomTests : FileSystemTestsBase
	{
		private readonly string _xmlFilesDirectoryPath;


		public RemovingBomTests()
		{
			_xmlFilesDirectoryPath = Path.GetFullPath(Path.Combine(_baseDirectoryPath, @"../SharedFiles/xml/"));
		}


		[Fact]
		public void RemovingBomAtStartIsCorrect()
		{
			// Arrange
			var minifier = new XmlMinifier(new XmlMinificationSettings(true));

			string inputFilePath = Path.Combine(_xmlFilesDirectoryPath, "xml-document-with-bom-at-start.xml");
			byte[] inputBytes = File.ReadAllBytes(inputFilePath);
			string inputContent = Encoding.UTF8.GetString(inputBytes);

			string targetOutputFilePath = Path.Combine(_xmlFilesDirectoryPath, "xml-document-without-bom.xml");
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
			var minifier = new XmlMinifier(new XmlMinificationSettings(true));

			string inputFilePath = Path.Combine(_xmlFilesDirectoryPath, "xml-document-with-bom-in-tag.xml");
			byte[] inputBytes = File.ReadAllBytes(inputFilePath);
			string inputContent = Encoding.UTF8.GetString(inputBytes);

			string targetOutputFilePath = Path.Combine(_xmlFilesDirectoryPath, "xml-document-without-bom.xml");
			byte[] targetOutputBytes = File.ReadAllBytes(targetOutputFilePath);

			// Act
			string outputContent = minifier.Minify(inputContent).MinifiedContent;
			byte[] outputBytes = Encoding.UTF8.GetBytes(outputContent);

			// Assert
			Assert.Equal(targetOutputBytes, outputBytes);
		}
	}
}