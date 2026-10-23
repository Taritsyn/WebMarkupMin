using System;
using System.Collections.Generic;

using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class ProcessingXmlCodeTests
	{
		const string USE_XHTML_MINIFIER_FOR_XHTML_SYNTAX_RECOMMENDATION = "If you want to use XHTML syntax, it is better to " +
			"use the XHTML Minifier for minification of this document.";


		private static string GenerateXmlBasedAttributeNotAllowedMessage(string attributeName)
		{
			string message = string.Format("XML-based attribute `{0}` not allowed here.", attributeName) + " " +
				USE_XHTML_MINIFIER_FOR_XHTML_SYNTAX_RECOMMENDATION;

			return message;
		}

		[Fact]
		public void ProcessingXhtml10StrictDocumentWithoutDoctypeShortening()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\"\n" +
				"  \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\n" +
				"<html xmlns=\"http://www.w3.org/1999/xhtml\">\n" +
				"	<head>\n" +
				"		<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\n" +
				"		<title>Minimal XHTML 1.0 Strict document</title>\n" +
				"	</head>\n" +
				"	<body>\n" +
				"		<p>Some text...</p>\n" +
				"	</body>\n" +
				"</html>"
				;

			// Act
			MarkupMinificationResult result = minifier.Minify(input);
			IList<MinificationErrorInfo> errors = result.Errors;
			IList<MinificationErrorInfo> warnings = result.Warnings;

			// Assert
			Assert.Equal(1, errors.Count);
			Assert.Equal(0, warnings.Count);

			MinificationErrorInfo error1 = errors[0];
			Assert.Equal(
				"Minification of XHTML document by the HTML Minifier without `DOCTYPE` shortening is no longer supported! " +
				"Set the `UseShortDoctype` property to `true`, or use an XHTML Minifier.",
				error1.Message
			);
			Assert.Equal(1, error1.LineNumber);
			Assert.Equal(1, error1.ColumnNumber);
			Assert.Equal(
				"Line 1: <!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\"" + Environment.NewLine +
				"--------^" + Environment.NewLine +
				"Line 2:   \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">",
				error1.SourceFragment
			);
		}

		[Fact]
		public void ProcessingXhtml10StrictDocumentWithDoctypeShortening()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true)
			{
				UseShortDoctype = true
			});

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
			const string targetOutput = "\n" +
				"<!DOCTYPE html>\n" +
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

			// Act
			MarkupMinificationResult result = minifier.Minify(input);
			string output = result.MinifiedContent;
			IList<MinificationErrorInfo> errors = result.Errors;
			IList<MinificationErrorInfo> warnings = result.Warnings;

			// Assert
			Assert.Equal(targetOutput, output);
			Assert.Equal(0, errors.Count);
			Assert.Equal(3, warnings.Count);

			MinificationErrorInfo warning1 = warnings[0];
			Assert.Equal(
				"XML declaration is not supported in HTML and was therefore removed. " +
				USE_XHTML_MINIFIER_FOR_XHTML_SYNTAX_RECOMMENDATION,
				warning1.Message
			);
			Assert.Equal(1, warning1.LineNumber);
			Assert.Equal(1, warning1.ColumnNumber);
			Assert.Equal(
				"Line 1: <?xml version=\"1.0\" encoding=\"UTF-8\" ?>"+ Environment.NewLine +
				"--------^" + Environment.NewLine +
				"Line 2: <!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\"",
				warning1.SourceFragment
			);

			MinificationErrorInfo warning2 = warnings[1];
			Assert.Equal(
				GenerateXmlBasedAttributeNotAllowedMessage("xml:lang"),
				warning2.Message
			);
			Assert.Equal(4, warning2.LineNumber);
			Assert.Equal(44, warning2.ColumnNumber);
			Assert.Equal(
				"Line 3:   \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">" + Environment.NewLine +
				"Line 4: <html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\" xmlns:bi=\"urn:schemas-microsoft-com:…" + Environment.NewLine +
				"---------------------------------------------------^" + Environment.NewLine +
				"Line 5:     <head>",
				warning2.SourceFragment
			);

			MinificationErrorInfo warning3 = warnings[2];
			Assert.Equal(
				GenerateXmlBasedAttributeNotAllowedMessage("xmlns:bi"),
				warning3.Message
			);
			Assert.Equal(4, warning3.LineNumber);
			Assert.Equal(58, warning3.ColumnNumber);
			Assert.Equal(
				"Line 3: …www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">" + Environment.NewLine +
				"Line 4: …s=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\" xmlns:bi=\"urn:schemas-microsoft-com:mscom:bi\">" + Environment.NewLine +
				"--------------------------------------------------------^" + Environment.NewLine +
				"Line 5:",
				warning3.SourceFragment
			);
		}

		[Fact]
		public void ProcessingSvgCode()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input = "<svg version=\"1.1\" baseProfile=\"full\" " +
				"xmlns=\"http://www.w3.org/2000/svg\" " +
				"xmlns:xlink=\"http://www.w3.org/1999/xlink\" " +
				"xmlns:ev=\"http://www.w3.org/2001/xml-events\" " +
				"width=\"100%\" height=\"100%\">\n" +
				"	<rect fill=\"white\" x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" />\n" +
				"	<rect fill=\"silver\" x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" rx=\"1em\" />\n" +
				"</svg>"
				;
			const string targetOutput = "<svg version=\"1.1\" baseProfile=\"full\" " +
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
			Assert.Equal(targetOutput, output);
			Assert.Equal(2, warnings.Count);

			MinificationErrorInfo warning1 = warnings[0];
			Assert.Equal(
				GenerateXmlBasedAttributeNotAllowedMessage("xmlns:xlink"),
				warning1.Message
			);
			Assert.Equal(1, warning1.LineNumber);
			Assert.Equal(74, warning1.ColumnNumber);
			Assert.Equal(
				"Line 1: …file=\"full\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml…" + Environment.NewLine +
				"--------------------------------------------------------^" + Environment.NewLine +
				"Line 2: …y=\"0\" width=\"100%\" height=\"100%\" />",
				warning1.SourceFragment
			);

			MinificationErrorInfo warning2 = warnings[1];
			Assert.Equal(
				GenerateXmlBasedAttributeNotAllowedMessage("xmlns:ev"),
				warning2.Message
			);
			Assert.Equal(1, warning2.LineNumber);
			Assert.Equal(117, warning2.ColumnNumber);
			Assert.Equal(
				"Line 1: …vg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xmlns:ev=\"http://www.w3.org/2001/xml-events\" w…" + Environment.NewLine +
				"--------------------------------------------------------^" + Environment.NewLine +
				"Line 2:",
				warning2.SourceFragment
			);
		}

		[Fact]
		public void ProcessingNestedSvgCode()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

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
			Assert.Equal(1, warnings.Count);

			MinificationErrorInfo warning1 = warnings[0];
			Assert.Equal(GenerateXmlBasedAttributeNotAllowedMessage("xmlns:xlink"), warning1.Message);
			Assert.Equal(1, warning1.LineNumber);
			Assert.Equal(41, warning1.ColumnNumber);
			Assert.Equal(
				"Line 1: <svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\">" + Environment.NewLine +
				"------------------------------------------------^" + Environment.NewLine +
				"Line 2:     <svg x=\"10\">",
				warning1.SourceFragment
			);
		}

		[Fact]
		public void ProcessingMathMlCode()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

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

		[Fact]
		public void PreservingXmlNamespaceAttributesInXhtml5Document()
		{
			// Arrange
			var preservingXmlNamespaceAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true)
			{
				PreservableAttributeList = "[xmlns]"
			});
			var removingXmlNamespaceAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input = "<!DOCTYPE html>\n" +
				"<html xmlns=\"http://www.w3.org/1999/xhtml\" lang=\"ru\">\n" +
				"	<head>\n" +
				"		<meta charset=\"utf-8\"/>\n" +
				"		<title>Минимальный XHTML5-документ</title>\n" +
				"	</head>\n" +
				"	<body>\n" +
				"		<p>Какой-то текст...</p>\n" +
				"	</body>\n" +
				"</html>"
				;
			const string targetOutputA = "<!DOCTYPE html>\n" +
				"<html xmlns=\"http://www.w3.org/1999/xhtml\" lang=\"ru\">\n" +
				"	<head>\n" +
				"		<meta charset=\"utf-8\">\n" +
				"		<title>Минимальный XHTML5-документ</title>\n" +
				"	</head>\n" +
				"	<body>\n" +
				"		<p>Какой-то текст...</p>\n" +
				"	</body>\n" +
				"</html>";
			const string targetOutputB = "<!DOCTYPE html>\n" +
				"<html lang=\"ru\">\n" +
				"	<head>\n" +
				"		<meta charset=\"utf-8\">\n" +
				"		<title>Минимальный XHTML5-документ</title>\n" +
				"	</head>\n" +
				"	<body>\n" +
				"		<p>Какой-то текст...</p>\n" +
				"	</body>\n" +
				"</html>"
				;

			// Act
			string outputA = preservingXmlNamespaceAttributesMinifier.Minify(input).MinifiedContent;
			string outputB = removingXmlNamespaceAttributesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
		}
	}
}
