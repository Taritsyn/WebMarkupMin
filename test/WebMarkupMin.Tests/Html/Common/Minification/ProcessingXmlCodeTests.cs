using System.Collections.Generic;

using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class ProcessingXmlCodeTests
	{
		[Fact]
		public void ProcessingXmlNodesIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\n" +
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
			const string targetOutput1 = "\n" +
				"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" " +
				"\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\n" +
				"<html xml:lang=\"en\" " +
				"xmlns:bi=\"urn:schemas-microsoft-com:mscom:bi\">\n" +
				"	<head>\n" +
				"		<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\n" +
				"		<title>Some title...</title>\n" +
				"	</head>\n" +
				"	<body>\n" +
				"		<div id=\"content\">\n" +
				"			<p>Some text...</p>\n" +
				"		</div>\n" +
				"	</body>\n" +
				"</html>"
				;

			const string input2 = "<svg version=\"1.1\" baseProfile=\"full\" " +
				"xmlns=\"http://www.w3.org/2000/svg\" " +
				"xmlns:xlink=\"http://www.w3.org/1999/xlink\" " +
				"xmlns:ev=\"http://www.w3.org/2001/xml-events\" " +
				"width=\"100%\" height=\"100%\">\n" +
				"	<rect fill=\"white\" x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" />\n" +
				"	<rect fill=\"silver\" x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" rx=\"1em\" />\n" +
				"</svg>"
				;
			const string targetOutput2 = "<svg version=\"1.1\" baseProfile=\"full\" " +
				"xmlns=\"http://www.w3.org/2000/svg\" " +
				"xmlns:xlink=\"http://www.w3.org/1999/xlink\" " +
				"xmlns:ev=\"http://www.w3.org/2001/xml-events\" " +
				"width=\"100%\" height=\"100%\">\n" +
				"	<rect fill=\"white\" x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" />\n" +
				"	<rect fill=\"silver\" x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" rx=\"1em\" />\n" +
				"</svg>"
				;

			const string input3 = "<svg xmlns=\"http://www.w3.org/2000/svg\" " +
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
			const string targetOutput3 = "<svg xmlns=\"http://www.w3.org/2000/svg\" " +
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

			const string input4 = "<math xmlns=\"http://www.w3.org/1998/Math/MathML\">\n" +
				"	<infinity />\n" +
				"</math>"
				;
			const string targetOutput4 = input4;

			// Act
			MarkupMinificationResult result1 = minifier.Minify(input1);
			string output1 = result1.MinifiedContent;
			IList<MinificationErrorInfo> warnings1 = result1.Warnings;

			MarkupMinificationResult result2 = minifier.Minify(input2);
			string output2 = result2.MinifiedContent;
			IList<MinificationErrorInfo> warnings2 = result2.Warnings;

			MarkupMinificationResult result3 = minifier.Minify(input3);
			string output3 = result3.MinifiedContent;
			IList<MinificationErrorInfo> warnings3 = result3.Warnings;

			MarkupMinificationResult result4 = minifier.Minify(input4);
			string output4 = result4.MinifiedContent;
			IList<MinificationErrorInfo> warnings4 = result4.Warnings;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(3, warnings1.Count);
			Assert.Equal(1, warnings1[0].LineNumber);
			Assert.Equal(1, warnings1[0].ColumnNumber);
			Assert.Equal(4, warnings1[1].LineNumber);
			Assert.Equal(44, warnings1[1].ColumnNumber);
			Assert.Equal(4, warnings1[2].LineNumber);
			Assert.Equal(58, warnings1[2].ColumnNumber);

			Assert.Equal(targetOutput2, output2);
			Assert.Equal(2, warnings2.Count);
			Assert.Equal(1, warnings2[0].LineNumber);
			Assert.Equal(74, warnings2[0].ColumnNumber);
			Assert.Equal(1, warnings2[1].LineNumber);
			Assert.Equal(117, warnings2[1].ColumnNumber);

			Assert.Equal(targetOutput3, output3);
			Assert.Equal(1, warnings3.Count);
			Assert.Equal(1, warnings3[0].LineNumber);
			Assert.Equal(41, warnings3[0].ColumnNumber);

			Assert.Equal(targetOutput4, output4);
			Assert.Equal(0, warnings4.Count);
		}

		[Fact]
		public void RemovingXmlNamespaceAttributesIsCorrect()
		{
			// Arrange
			var keepingXmlNamespaceAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { PreservableAttributeList = "[xmlns]" });
			var removingXmlNamespaceAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\"" +
				" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\n" +
				"<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"ru-RU\" lang=\"ru\">\n" +
				"	<head>\n" +
				"		<title>Какой-то заголовок</title>\n" +
				"	</head>\n" +
				"	<body>\n" +
				"		<div id=\"content\">\n" +
				"			<p>Какой-то текст</p>\n" +
				"		</div>\n" +
				"	</body>\n" +
				"</html>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\"" +
				" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\n" +
				"<html xml:lang=\"ru-RU\" lang=\"ru\">\n" +
				"	<head>\n" +
				"		<title>Какой-то заголовок</title>\n" +
				"	</head>\n" +
				"	<body>\n" +
				"		<div id=\"content\">\n" +
				"			<p>Какой-то текст</p>\n" +
				"		</div>\n" +
				"	</body>\n" +
				"</html>"
				;

			// Act
			string output1A = keepingXmlNamespaceAttributesMinifier.Minify(input1).MinifiedContent;
			string output1B = removingXmlNamespaceAttributesMinifier.Minify(input1).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
		}
	}
}
