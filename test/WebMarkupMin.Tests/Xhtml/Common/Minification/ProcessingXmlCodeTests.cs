using System.Collections.Generic;

using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Xhtml.Common.Minification
{
	public class ProcessingXmlCodeTests
	{
		[Fact]
		public void ProcessingXhtml10StrictDocument()
		{
			// Arrange
			var minifier = new XhtmlMinifier(new XhtmlMinificationSettings(true));

			const string input = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\n" +
				"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\"\n" +
				"  \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\n" +
				"<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\" " +
				"xmlns:bi=\"urn:schemas-microsoft-com:mscom:bi\">\n" +
				"	<head>\n" +
				"		<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\n" +
				"		<title>Some title...</title>\n" +
				"	</head>\n" +
				"	<body>\n" +
				"		<div id=\"content\">\n" +
				"			<p>Some text...</p>\n" +
				"		</div>\n" +
				"	</body>\n" +
				"</html>"
				;
			const string targetOutput = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
				"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" " +
				"\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\n" +
				"<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\" " +
				"xmlns:bi=\"urn:schemas-microsoft-com:mscom:bi\">\n" +
				"	<head>\n" +
				"		<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\n" +
				"		<title>Some title...</title>\n" +
				"	</head>\n" +
				"	<body>\n" +
				"		<div id=\"content\">\n" +
				"			<p>Some text...</p>\n" +
				"		</div>\n" +
				"	</body>\n" +
				"</html>"
				;

			// Act
			MarkupMinificationResult result = minifier.Minify(input);
			string output = result.MinifiedContent;
			IList<MinificationErrorInfo> warnings = result.Warnings;

			// Assert
			Assert.Equal(targetOutput, output);
			Assert.Equal(0, warnings.Count);
		}

		[Fact]
		public void ProcessingSvgCode()
		{
			// Arrange
			var minifier = new XhtmlMinifier(new XhtmlMinificationSettings(true));

			const string input = "<svg version=\"1.1\" baseProfile=\"full\" " +
				"xmlns=\"http://www.w3.org/2000/svg\" " +
				"xmlns:xlink=\"http://www.w3.org/1999/xlink\" " +
				"xmlns:ev=\"http://www.w3.org/2001/xml-events\" " +
				"width=\"100%\" height=\"100%\">\n" +
				"	<rect fill=\"white\" x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" />\n" +
				"	<rect fill=\"silver\" x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" rx=\"1em\" />\n" +
				"</svg>"
				;

			// Act
			MarkupMinificationResult result = minifier.Minify(input);
			string output = result.MinifiedContent;
			IList<MinificationErrorInfo> warnings = result.Warnings;

			// Assert
			Assert.Equal(input, output);
			Assert.Equal(0, warnings.Count);
		}

		[Fact]
		public void ProcessingNestedSvgCode()
		{
			// Arrange
			var minifier = new XhtmlMinifier(new XhtmlMinificationSettings(true));

			// Act
			const string input = "<svg xmlns=\"http://www.w3.org/2000/svg\" " +
				"xmlns:xlink=\"http://www.w3.org/1999/xlink\">\n" +
				"	<svg x=\"10\">\n" +
				"		<rect x=\"10\" y=\"10\" height=\"100\" width=\"100\" " +
				"style=\"stroke:#ff0000; fill: #0000ff\"/>\n" +
				"	</svg>\n" +
				"	<svg x=\"200\">\n" +
				"		<rect x=\"10\" y=\"10\" height=\"100\" width=\"100\" " +
				"style=\"stroke:#009900; fill: #00cc00\"/>\n" +
				"	</svg>\n" +
				"</svg>"
				;
			const string targetOutput = "<svg xmlns=\"http://www.w3.org/2000/svg\" " +
				"xmlns:xlink=\"http://www.w3.org/1999/xlink\">\n" +
				"	<svg x=\"10\">\n" +
				"		<rect x=\"10\" y=\"10\" height=\"100\" width=\"100\" " +
				"style=\"stroke:#ff0000; fill: #0000ff\" />\n" +
				"	</svg>\n" +
				"	<svg x=\"200\">\n" +
				"		<rect x=\"10\" y=\"10\" height=\"100\" width=\"100\" " +
				"style=\"stroke:#009900; fill: #00cc00\" />\n" +
				"	</svg>\n" +
				"</svg>";

			// Assert
			MarkupMinificationResult result = minifier.Minify(input);
			string output = result.MinifiedContent;
			IList<MinificationErrorInfo> warnings = result.Warnings;

			Assert.Equal(targetOutput, output);
			Assert.Equal(0, warnings.Count);
		}

		[Fact]
		public void ProcessingMathMlCode()
		{
			// Arrange
			var minifier = new XhtmlMinifier(new XhtmlMinificationSettings(true));

			const string input = "<math xmlns=\"http://www.w3.org/1998/Math/MathML\">\n" +
				"	<infinity />\n" +
				"</math>"
				;

			// Act
			MarkupMinificationResult result = minifier.Minify(input);
			string output = result.MinifiedContent;
			IList<MinificationErrorInfo> warnings = result.Warnings;

			// Assert
			Assert.Equal(input, output);
			Assert.Equal(0, warnings.Count);
		}
	}
}