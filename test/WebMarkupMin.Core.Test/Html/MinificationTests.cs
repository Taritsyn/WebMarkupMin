using System.Collections.Generic;
using System.IO;
using System.Text;

using Xunit;

namespace WebMarkupMin.Core.Test.Html
{
	public class MinificationTests : FileSystemTestsBase
	{
		private readonly string _htmlFilesDirectoryPath;


		public MinificationTests()
		{
			_htmlFilesDirectoryPath = Path.GetFullPath(Path.Combine(_baseDirectoryPath, @"files/html/"));
		}


		#region Removing BOM

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

		#endregion

		#region Encoding attribute values

		[Fact]
		public void EncodingAttributeValuesIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<input value='<a href=\"/product.asp?id=12&category=5&returnUrl=%2Fdefault.asp\">" +
				"Show product</a>'>";
			const string targetOutput1 = "<input value=\"&lt;a href=&#34;/product.asp?id=12&amp;category=5" +
				"&amp;returnUrl=%2Fdefault.asp&#34;>Show product&lt;/a>\">";

			const string input2 = "<input value=\"<a href='/product.asp?id=12&category=5&returnUrl=%2Fdefault.asp'>" +
				"Show product</a>\">";
			const string targetOutput2 = "<input value=\"&lt;a href='/product.asp?id=12&amp;category=5&amp;returnUrl=%2Fdefault.asp'>" +
				"Show product&lt;/a>\">";

			const string input3 = "<select>\n" +
				"	<option value='Douglas Crockford&#39;s JS Minifier'>Douglas Crockford's JS Minifier</option>\n" +
				"	<option value='Microsoft Ajax JS Minifier'>Microsoft Ajax JS Minifier</option>\n" +
				"	<option value='YUI JS Minifier'>YUI JS Minifier</option>\n" +
				"</select>"
				;
			const string targetOutput3 = "<select>\n" +
				"	<option value=\"Douglas Crockford's JS Minifier\">Douglas Crockford's JS Minifier</option>\n" +
				"	<option value=\"Microsoft Ajax JS Minifier\">Microsoft Ajax JS Minifier</option>\n" +
				"	<option value=\"YUI JS Minifier\">YUI JS Minifier</option>\n" +
				"</select>"
				;

			const string input4 = "<input type=\"button\" value=\"Remove article &quot;Паранойя оптимизатора&quot;\">";
			const string targetOutput4 = "<input type=\"button\" value=\"Remove article &#34;Паранойя оптимизатора&#34;\">";

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			string output3 = minifier.Minify(input3).MinifiedContent;
			string output4 = minifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
		}

		#endregion

		#region Case normalization

		[Fact]
		public void CaseNormalizationIsCorrect()
		{
			// Arrange
			var preservingCaseMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { PreserveCase = true });
			var changingCaseMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { PreserveCase = false });

			const string input1 = "<P>Some text...</p>";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<p>Some text...</p>";

			const string input2 = "<DIV>Some text..</DIV>";
			const string targetOutput2A = input2;
			const string targetOutput2B = "<div>Some text..</div>";

			const string input3 = "<DIV title=\"Some title...\">Some text..</DiV>";
			const string targetOutput3A = input3;
			const string targetOutput3B = "<div title=\"Some title...\">Some text..</div>";

			const string input4 = "<DIV TITLE=\"Some title...\">Some text..</DIV>";
			const string targetOutput4A = input4;
			const string targetOutput4B = "<div title=\"Some title...\">Some text..</div>";

			const string input5 = "<DIV tItLe=\"Some title...\">Some text..</DIV>";
			const string targetOutput5A = input5;
			const string targetOutput5B = "<div title=\"Some title...\">Some text..</div>";

			const string input6 = "<DiV tItLe=\"Some title...\">Some text..</DIV>";
			const string targetOutput6A = input6;
			const string targetOutput6B = "<div title=\"Some title...\">Some text..</div>";

			// Act
			string output1A = preservingCaseMinifier.Minify(input1).MinifiedContent;
			string output1B = changingCaseMinifier.Minify(input1).MinifiedContent;

			string output2A = preservingCaseMinifier.Minify(input2).MinifiedContent;
			string output2B = changingCaseMinifier.Minify(input2).MinifiedContent;

			string output3A = preservingCaseMinifier.Minify(input3).MinifiedContent;
			string output3B = changingCaseMinifier.Minify(input3).MinifiedContent;

			string output4A = preservingCaseMinifier.Minify(input4).MinifiedContent;
			string output4B = changingCaseMinifier.Minify(input4).MinifiedContent;

			string output5A = preservingCaseMinifier.Minify(input5).MinifiedContent;
			string output5B = changingCaseMinifier.Minify(input5).MinifiedContent;

			string output6A = preservingCaseMinifier.Minify(input6).MinifiedContent;
			string output6B = changingCaseMinifier.Minify(input6).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);

			Assert.Equal(targetOutput6A, output6A);
			Assert.Equal(targetOutput6B, output6B);
		}

		#endregion

		#region Space normalization between attributes

		[Fact]
		public void SpaceNormalizationBetweenAttributesIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<p title=\"Some title...\">Some text...</p>";

			const string input2 = "<p title = \"Some title...\">Some text...</p>";
			const string targetOutput2 = "<p title=\"Some title...\">Some text...</p>";

			const string input3 = "<p title\n\n\t  =\n     \"Some title...\">Some text...</p>";
			const string targetOutput3 = "<p title=\"Some title...\">Some text...</p>";

			const string input4 = "<input title=\"Some title...\"       id=\"txtName\"    value=\"Some text...\">";
			const string targetOutput4 = "<input title=\"Some title...\" id=\"txtName\" value=\"Some text...\">";

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			string output3 = minifier.Minify(input3).MinifiedContent;
			string output4 = minifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
		}

		#endregion

		#region Processing XML nodes

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

		#endregion

		#region Processing DOCTYPE declaration

		[Fact]
		public void ProcessingDoctypeIsCorrect()
		{
			// Arrange
			var originalDoctypeMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { UseShortDoctype = false });
			var shortDoctypeMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { UseShortDoctype = true });

			const string input1 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\"\n    \"http://www.w3.org/TR/html4/strict.dtd\">";
			const string targetOutput1A = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\">";
			const string targetOutput1B = "<!DOCTYPE html>";

			const string input2 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">";
			const string targetOutput2A = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">";
			const string targetOutput2B = "<!DOCTYPE html>";

			const string input3 = "<!DOCTYPE html>";

			// Act
			string output1A = originalDoctypeMinifier.Minify(input1).MinifiedContent;
			string output1B = shortDoctypeMinifier.Minify(input1).MinifiedContent;

			string output2A = originalDoctypeMinifier.Minify(input2).MinifiedContent;
			string output2B = shortDoctypeMinifier.Minify(input2).MinifiedContent;

			string output3A = originalDoctypeMinifier.Minify(input3).MinifiedContent;
			string output3B = shortDoctypeMinifier.Minify(input3).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);

			Assert.Equal(input3, output3A);
			Assert.Equal(input3, output3B);
		}

		#endregion

		#region Upgrading META encoding tag

		[Fact]
		public void UpgradingToMetaCharsetTag()
		{
			// Arrange
			var originalMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { UseMetaCharsetTag = false });
			var upgradingMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { UseMetaCharsetTag = true });

			const string input1 = "<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\">";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<meta charset=\"utf-8\">";

			const string input2 = "<meta http-equiv=\"Content-Type\" content=\"text/html;charset=windows-1251\">";
			const string targetOutput2A = input2;
			const string targetOutput2B = "<meta charset=\"windows-1251\">";

			const string input3 = "<META http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\">";
			const string targetOutput3A = "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\">";
			const string targetOutput3B = "<meta charset=\"iso-8859-1\">";

			const string input4 = "<meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml;charset=UTF-8\"/>";
			const string targetOutput4A = "<meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml;charset=UTF-8\">";
			const string targetOutput4B = "<meta charset=\"UTF-8\">";

			// Act
			string output1A = originalMinifier.Minify(input1).MinifiedContent;
			string output1B = upgradingMinifier.Minify(input1).MinifiedContent;

			string output2A = originalMinifier.Minify(input2).MinifiedContent;
			string output2B = upgradingMinifier.Minify(input2).MinifiedContent;

			string output3A = originalMinifier.Minify(input3).MinifiedContent;
			string output3B = upgradingMinifier.Minify(input3).MinifiedContent;

			string output4A = originalMinifier.Minify(input4).MinifiedContent;
			string output4B = upgradingMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
		}

		#endregion

		#region Cleaning attributes

		[Fact]
		public void CleaningClassAttributesIsCorrect()
		{
			// Arrange
			var cleaningAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<div class=\" b-inline b-top-bar__item  \">Some text…</div>";
			const string targetOutput1 = "<div class=\"b-inline b-top-bar__item\">Some text…</div>";

			const string input2 = "<p class=\" b-sethome__popupa-text      \">Some text…</p>";
			const string targetOutput2 = "<p class=\"b-sethome__popupa-text\">Some text…</p>";

			const string input3 = "<p class=\"\n  \n b-sethome__popupa-text   \n\n\t  \t\n   \">Some text…</p>";
			const string targetOutput3 = "<p class=\"b-sethome__popupa-text\">Some text…</p>";

			const string input4 = "<div class=\"\n  \n b-dropdowna   \n\n\t  \t\n  i-bem b-dropdowna_is-bem_yes \">" +
				"Some text…</div>";
			const string targetOutput4 = "<div class=\"b-dropdowna i-bem b-dropdowna_is-bem_yes\">Some text…</div>";

			const string input5 = "<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\">\r\n" +
				"	<style type=\"text/css\"><![CDATA[\r\n" +
				"	circle.myGreen {\r\n" +
				"		stroke: #006600;\r\n" +
				"		fill: #00cc00;\r\n" +
				"	}\r\n" +
				"	circle.myRed {\r\n" +
				"		stroke: #660000;\r\n" +
				"		fill: #cc0000;\r\n" +
				"	}\r\n" +
				"]]></style>\r\n" +
				"	<circle class=\"\t  myGreen  \t\" cx=\"40\" cy=\"40\" r=\"24\" />\r\n" +
				"	<circle class=\"\t  myRed  \t\" cx=\"40\" cy=\"100\" r=\"24\" />\r\n" +
				"</svg>"
				;
			const string targetOutput5 = "<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\">\r\n" +
				"	<style type=\"text/css\"><![CDATA[\r\n" +
				"	circle.myGreen {\r\n" +
				"		stroke: #006600;\r\n" +
				"		fill: #00cc00;\r\n" +
				"	}\r\n" +
				"	circle.myRed {\r\n" +
				"		stroke: #660000;\r\n" +
				"		fill: #cc0000;\r\n" +
				"	}\r\n" +
				"]]></style>\r\n" +
				"	<circle class=\"myGreen\" cx=\"40\" cy=\"40\" r=\"24\" />\r\n" +
				"	<circle class=\"myRed\" cx=\"40\" cy=\"100\" r=\"24\" />\r\n" +
				"</svg>"
				;

			// Act
			string output1 = cleaningAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = cleaningAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = cleaningAttributesMinifier.Minify(input3).MinifiedContent;
			string output4 = cleaningAttributesMinifier.Minify(input4).MinifiedContent;
			string output5 = cleaningAttributesMinifier.Minify(input5).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
		}

		[Fact]
		public void CleaningStyleAttributesIsCorrect()
		{
			// Arrange
			var cleaningAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<p style=\"    color: red; background-color: rgba(255, 0, 0, 0.7);  \">Some text…</p>";
			const string targetOutput1 = "<p style=\"color: red; background-color: rgba(255, 0, 0, 0.7)\">Some text…</p>";

			const string input2 = "<p style=\"font-weight: bold  ; \">Some text…</p>";
			const string targetOutput2 = "<p style=\"font-weight: bold\">Some text…</p>";

			const string input3 = "<svg>\n" +
				"	<rect x=\"20\" y=\"20\" width=\"100\" height=\"100\" style=\"stroke: #009900;\n" +
				"			 stroke-width: 3;\n" +
				"			 stroke-dasharray: 10 5;\n" +
				"			 fill: none;\n" +
				"			\"\n" +
				"		/>\n" +
				"</svg>"
				;
			const string targetOutput3 = "<svg>\n" +
				"	<rect x=\"20\" y=\"20\" width=\"100\" height=\"100\" style=\"stroke: #009900;\n" +
				"			 stroke-width: 3;\n" +
				"			 stroke-dasharray: 10 5;\n" +
				"			 fill: none\" />\n" +
				"</svg>"
				;

			// Act
			string output1 = cleaningAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = cleaningAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = cleaningAttributesMinifier.Minify(input3).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
		}

		[Fact]
		public void CleaningUriBasedAttributesIsCorrect()
		{
			// Arrange
			var cleaningAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<a href=\"   http://example.com  \">Some text...</a>";
			const string targetOutput1 = "<a href=\"http://example.com\">Some text...</a>";

			const string input2 = "<a href=\"  \t\t  \n \t  \">Some text...</a>";
			const string targetOutput2 = "<a href=\"\">Some text...</a>";

			const string input3 = "<img src=\"   http://example.com/html.png  \" title=\"Some title...   \" " +
				"longdesc=\"  http://example.com/html.txt \n\n   \t \">";
			const string targetOutput3 = "<img src=\"http://example.com/html.png\" title=\"Some title...   \" " +
				"longdesc=\"http://example.com/html.txt\">";

			const string input4 = "<video src=\"/video/movie.mp4\" poster=\"   /video/poster.png  \"></video>";
			const string targetOutput4 = "<video src=\"/video/movie.mp4\" poster=\"/video/poster.png\"></video>";

			const string input5 = "<form action=\"  guestbook/add_record?user=monitor2002&language=russian     \"></form>";
			const string targetOutput5 = "<form action=\"guestbook/add_record?user=monitor2002&amp;language=russian\"></form>";

			const string input6 = "<BLOCKQUOTE cite=\" \n\n\n http://validator.w3.org/docs/help.html     \">" +
				"<P>This validator checks the markup validity of Web documents in HTML, XHTML, SMIL, MathML, etc.</P>" +
				"</BLOCKQUOTE>";
			const string targetOutput6 = "<blockquote cite=\"http://validator.w3.org/docs/help.html\">" +
				"<p>This validator checks the markup validity of Web documents in HTML, XHTML, SMIL, MathML, etc.</p>" +
				"</blockquote>";

			const string input7 = "<head profile=\"       http://gmpg.org/xfn/11    \"></head>";
			const string targetOutput7 = "<head profile=\"http://gmpg.org/xfn/11\"></head>";

			const string input8 = "<object codebase=\"   http://example.com  \"></object>";
			const string targetOutput8 = "<object codebase=\"http://example.com\"></object>";

			const string input9 = "<object type=\"application/x-shockwave-flash\" data=\"	 player_flv_mini.swf \">" +
				"<param name=\"movie\" value=\"	 player_flv_mini.swf \">" +
				"</object>";
			const string targetOutput9 = "<object type=\"application/x-shockwave-flash\" data=\"player_flv_mini.swf\">" +
				"<param name=\"movie\" value=\"player_flv_mini.swf\">" +
				"</object>";

			const string input10 = "<object data=\"  movie.mov	\" type=\"video/quicktime\">" +
				"<param name=\"pluginspage\" value=\"   http://quicktime.apple.com/	\">" +
				"</object>";
			const string targetOutput10 = "<object data=\"movie.mov\" type=\"video/quicktime\">" +
				"<param name=\"pluginspage\" value=\"http://quicktime.apple.com/\">" +
				"</object>";

			const string input11 = "<span profile=\"   6, 7, 8  \">Some text...</span>";
			const string input12 = "<div action=\"  one-two-three \">Some other text ...</div>";

			// Act
			string output1 = cleaningAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = cleaningAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = cleaningAttributesMinifier.Minify(input3).MinifiedContent;
			string output4 = cleaningAttributesMinifier.Minify(input4).MinifiedContent;
			string output5 = cleaningAttributesMinifier.Minify(input5).MinifiedContent;
			string output6 = cleaningAttributesMinifier.Minify(input6).MinifiedContent;
			string output7 = cleaningAttributesMinifier.Minify(input7).MinifiedContent;
			string output8 = cleaningAttributesMinifier.Minify(input8).MinifiedContent;
			string output9 = cleaningAttributesMinifier.Minify(input9).MinifiedContent;
			string output10 = cleaningAttributesMinifier.Minify(input10).MinifiedContent;
			string output11 = cleaningAttributesMinifier.Minify(input11).MinifiedContent;
			string output12 = cleaningAttributesMinifier.Minify(input12).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
			Assert.Equal(targetOutput6, output6);
			Assert.Equal(targetOutput7, output7);
			Assert.Equal(targetOutput8, output8);
			Assert.Equal(targetOutput9, output9);
			Assert.Equal(targetOutput10, output10);
			Assert.Equal(input11, output11);
			Assert.Equal(input12, output12);
		}

		[Fact]
		public void CleaningNumericAttributesIsCorrect()
		{
			// Arrange
			var cleaningAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<button tabindex=\"   1  \">Save</button> <a href=\"#\" tabindex=\"   2  \">Cancel</a>";
			const string targetOutput1 = "<button tabindex=\"1\">Save</button> <a href=\"#\" tabindex=\"2\">Cancel</a>";

			const string input2 = "<input value=\"\" maxlength=\"     5 \">";
			const string targetOutput2 = "<input value=\"\" maxlength=\"5\">";

			const string input3 = "<select size=\"  15   \t\t \"><option>Some text…</option></select>";
			const string targetOutput3 = "<select size=\"15\"><option>Some text…</option></select>";

			const string input4 = "<textarea rows=\"   10  \" cols=\"  40      \"></textarea>";
			const string targetOutput4 = "<textarea rows=\"10\" cols=\"40\"></textarea>";

			const string input5 = "<COLGROUP span=\"   20  \"><COL span=\"  19 \"></COLGROUP>";
			const string targetOutput5 = "<colgroup span=\"20\"><col span=\"19\"></colgroup>";

			const string input6 = "<tr><td colspan=\"    2   \">Some text…</td><td rowspan=\"   3 \"></td></tr>";
			const string targetOutput6 = "<tr><td colspan=\"2\">Some text…</td><td rowspan=\"3\"></td></tr>";

			// Act
			string output1 = cleaningAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = cleaningAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = cleaningAttributesMinifier.Minify(input3).MinifiedContent;
			string output4 = cleaningAttributesMinifier.Minify(input4).MinifiedContent;
			string output5 = cleaningAttributesMinifier.Minify(input5).MinifiedContent;
			string output6 = cleaningAttributesMinifier.Minify(input6).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
			Assert.Equal(targetOutput6, output6);
		}

		[Fact]
		public void CleaningEventAttributesIsCorrect()
		{
			// Arrange
			var cleaningAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<button type=\"button\" onmouseover=\" \n\n alert('Hooray!')  \t \n\t  \">Some text…</button>";
			const string targetOutput1 = "<button type=\"button\" onmouseover=\"alert('Hooray!')\">Some text…</button>";

			const string input2 = "<a href=\"http://example.com/login.php\"" +
				" onclick=\"  window.open('http://example.com/login.php','_blank','toolbar=0,location=0,status=0," +
				"width=520,height=270,scrollbars=0,resizable=1');return false; \">" +
				"Some text…</a>"
				;
			const string targetOutput2 = "<a href=\"http://example.com/login.php\"" +
				" onclick=\"window.open('http://example.com/login.php','_blank','toolbar=0,location=0,status=0," +
				"width=520,height=270,scrollbars=0,resizable=1');return false\">" +
				"Some text…</a>"
				;

			const string input3 = "<body onload=\"  initStatistics();   initForms() ;  \"><p>Some text…</p></body>";
			const string targetOutput3 = "<body onload=\"initStatistics();   initForms()\"><p>Some text…</p></body>";

			const string input4 = "<svg width=\"500\" height=\"100\">\n" +
				"	<rect x=\"10\" y=\"10\" width=\"100\" height=\"75\"" +
				" onmouseover=\"  this.style.stroke = '#ff0000'; this.style['stroke-width'] = 5;  \"" +
				" onmouseout=\"  this.style.stroke = '#000000'; this.style['stroke-width'] = 1;  \" />\n" +
				"</svg>"
				;
			const string targetOutput4 = "<svg width=\"500\" height=\"100\">\n" +
				"	<rect x=\"10\" y=\"10\" width=\"100\" height=\"75\"" +
				" onmouseover=\"this.style.stroke = '#ff0000'; this.style['stroke-width'] = 5\"" +
				" onmouseout=\"this.style.stroke = '#000000'; this.style['stroke-width'] = 1\" />\n" +
				"</svg>"
				;

			// Act
			string output1 = cleaningAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = cleaningAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = cleaningAttributesMinifier.Minify(input3).MinifiedContent;
			string output4 = cleaningAttributesMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
		}

		[Fact]
		public void CleaningOtherAttributesIsCorrect()
		{
			// Arrange
			var cleaningAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input = "<meta name=\"keywords\" content=\"	HTML5, CSS3, ECMAScript 5, \">";
			const string targetOutput = "<meta name=\"keywords\" content=\"HTML5,CSS3,ECMAScript 5\">";

			// Act
			string output = cleaningAttributesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput, output);
		}

		#endregion

		#region Whitespace minification

		[Fact]
		public void WhitespaceMinificationIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.None });
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Safe });
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive });

			const string input1 = " \n \n\t <!-- meta name=\"GENERATOR\" content=\"Microsoft FrontPage 1.0\" --> \n  \n\t\n" +
				"<!DOCTYPE html>\n" +
				"<html>\n" +
				"	<head>\n" +
				"		<meta charset=\"utf-8\">\n" +
				"		<title> \t  Some  title...  \t  </title>\n" +
				"		<base href=\"http://www.example.com/\" target=\"_blank\">\n" +
				"		<link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\">\n" +
				"		<meta name=\"viewport\" content=\"width=device-width\">\n" +
				"		<link href=\"/Bundles/CommonStyles\" rel=\"stylesheet\">\n" +
				"		<style type=\"text/css\">\n" +
				"			.ie table.min-width-content {\n" +
				"				table-layout: auto !important;\n" +
				"			}\n" +
				"		</style>\n" +
				"		<script src=\"/Bundles/Modernizr\"></script>\n" +
				"	</head>\n" +
				"	<body>\n" +
				"		<p>Some text...</p>\n" +
				"		<script src=\"http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js\"></script>\n" +
				"		<script>\n" +
				"			(window.jquery) || document.write('<script src=\"/Bundles/Jquery\"><\\/script>');  \n" +
				"</script>\n" +
				"	    <script src=\"/Bundles/CommonScripts\"></script>\n" +
				"	</body>\n" +
				"</html>\n\n\t \n " +
				"<!-- MEOW -->\t \n\n  \n "
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<!-- meta name=\"GENERATOR\" content=\"Microsoft FrontPage 1.0\" -->" +
				"<!DOCTYPE html>" +
				"<html>" +
				"<head>" +
				"<meta charset=\"utf-8\">" +
				"<title>Some title...</title>" +
				"<base href=\"http://www.example.com/\" target=\"_blank\">" +
				"<link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\">" +
				"<meta name=\"viewport\" content=\"width=device-width\">" +
				"<link href=\"/Bundles/CommonStyles\" rel=\"stylesheet\">" +
				"<style type=\"text/css\">" +
				".ie table.min-width-content {\n" +
				"				table-layout: auto !important;\n" +
				"			}" +
				"</style>" +
				"<script src=\"/Bundles/Modernizr\"></script>" +
				"</head>" +
				"<body>" +
				"<p>Some text...</p>" +
				"<script src=\"http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js\"></script>" +
				"<script>(window.jquery) || document.write('<script src=\"/Bundles/Jquery\"><\\/script>');</script>" +
				"<script src=\"/Bundles/CommonScripts\"></script>" +
				"</body>" +
				"</html>" +
				"<!-- MEOW -->"
				;
			const string targetOutput1C = targetOutput1B;
			const string targetOutput1D = targetOutput1B;


			const string input2 = "<script>alert(\"Hello,     world!\");    </script>";
			const string targetOutput2A = input2;
			const string targetOutput2B = "<script>alert(\"Hello,     world!\");</script>";
			const string targetOutput2C = targetOutput2B;
			const string targetOutput2D = targetOutput2B;


			const string input3 = "<style>cite { quotes: \" «\" \"» \"; }    </style>";
			const string targetOutput3A = input3;
			const string targetOutput3B = "<style>cite { quotes: \" «\" \"» \"; }</style>";
			const string targetOutput3C = targetOutput3B;
			const string targetOutput3D = targetOutput3B;

			const string input4 = "<table class=\"table\">\n" +
				"	<caption>	 Monthly savings \n</caption>\n" +
				"	<colgroup>\n" +
				"		<col style=\"text-align: left\">\n" +
				"		<col style=\"text-align: right\">\n" +
				"	</colgroup>\n" +
				"	<thead>\n" +
				"		<tr>\n" +
				"			<th>	 Month \n</th>\n" +
				"			<th>	 Savings \n</th>\n" +
				"		</tr>\n" +
				"	</thead>\n" +
				"	<tbody>\n" +
				"		<tr>\n" +
				"			<td>	 Jul \n</td>\n" +
				"			<td>	 $2900 \n</td>\n" +
				"		</tr>\n" +
				"		<tr>\n" +
				"			<td>	 Oct \n</td>\n" +
				"			<td>	 $3120 \n</td>\n" +
				"		</tr>\n" +
				"	</tbody>\n" +
				"	<tfoot>\n" +
				"		<tr>\n" +
				"			<td>	 Total \n</td>\n" +
				"			<td>	 $6250 \n</td>\n" +
				"		</tr>\n" +
				"	</tfoot>\n" +
				"</table>"
				;
			const string targetOutput4A = input4;
			const string targetOutput4B = "<table class=\"table\">" +
				"<caption> Monthly savings </caption>" +
				"<colgroup>" +
				"<col style=\"text-align: left\">" +
				"<col style=\"text-align: right\">" +
				"</colgroup>" +
				"<thead>" +
				"<tr>" +
				"<th> Month </th>" +
				"<th> Savings </th>" +
				"</tr>" +
				"</thead>" +
				"<tbody>" +
				"<tr>" +
				"<td> Jul </td>" +
				"<td> $2900 </td>" +
				"</tr>" +
				"<tr>" +
				"<td> Oct </td>" +
				"<td> $3120 </td>" +
				"</tr>" +
				"</tbody>" +
				"<tfoot>" +
				"<tr>" +
				"<td> Total </td>" +
				"<td> $6250 </td>" +
				"</tr>" +
				"</tfoot>" +
				"</table>"
				;
			const string targetOutput4C = "<table class=\"table\">" +
				"<caption>Monthly savings</caption>" +
				"<colgroup>" +
				"<col style=\"text-align: left\">" +
				"<col style=\"text-align: right\">" +
				"</colgroup>" +
				"<thead>" +
				"<tr>" +
				"<th>Month</th>" +
				"<th>Savings</th>" +
				"</tr>" +
				"</thead>" +
				"<tbody>" +
				"<tr>" +
				"<td>Jul</td>" +
				"<td>$2900</td>" +
				"</tr>" +
				"<tr>" +
				"<td>Oct</td>" +
				"<td>$3120</td>" +
				"</tr>" +
				"</tbody>" +
				"<tfoot>" +
				"<tr>" +
				"<td>Total</td>" +
				"<td>$6250</td>" +
				"</tr>" +
				"</tfoot>" +
				"</table>"
				;
			const string targetOutput4D = targetOutput4C;


			const string input5 = "<select name=\"preprocessors\">\n" +
				"	<optgroup label=\"Styles\">\n" +
				"		<option value=\"sass\">	 Sass \n</option>\n" +
				"		<option value=\"less\">	 LESS \n</option>\n" +
				"		<option value=\"stylus\">	 Stylus \n</option>\n" +
				"	</optgroup>\n" +
				"	<optgroup label=\"Scripts\">\n" +
				"		<option value=\"coffeescript\">	 CoffeeScript \n</option>\n" +
				"		<option value=\"typescript\">	 TypeScript \n</option>\n" +
				"		<option value=\"kaffeine\">	 Kaffeine \n</option>\n" +
				"	</optgroup>\n" +
				"	<option value=\"dart\">	 Dart \n</option>\n" +
				"</select>"
				;
			const string targetOutput5A = input5;
			const string targetOutput5B = "<select name=\"preprocessors\">" +
				"<optgroup label=\"Styles\">" +
				"<option value=\"sass\"> Sass </option>" +
				"<option value=\"less\"> LESS </option>" +
				"<option value=\"stylus\"> Stylus </option>" +
				"</optgroup>" +
				"<optgroup label=\"Scripts\">" +
				"<option value=\"coffeescript\"> CoffeeScript </option>" +
				"<option value=\"typescript\"> TypeScript </option>" +
				"<option value=\"kaffeine\"> Kaffeine </option>" +
				"</optgroup>" +
				"<option value=\"dart\"> Dart </option>" +
				"</select>"
				;
			const string targetOutput5C = targetOutput5B;
			const string targetOutput5D = "<select name=\"preprocessors\">" +
				"<optgroup label=\"Styles\">" +
				"<option value=\"sass\">Sass</option>" +
				"<option value=\"less\">LESS</option>" +
				"<option value=\"stylus\">Stylus</option>" +
				"</optgroup>" +
				"<optgroup label=\"Scripts\">" +
				"<option value=\"coffeescript\">CoffeeScript</option>" +
				"<option value=\"typescript\">TypeScript</option>" +
				"<option value=\"kaffeine\">Kaffeine</option>" +
				"</optgroup>" +
				"<option value=\"dart\">Dart</option>" +
				"</select>"
				;


			const string input6 = "<video width=\"320\" height=\"240\" poster=\"video/poster.png\" controls=\"controls\">\n" +
				"	<source src=\"video/ie6.ogv\" type=\"video/ogg\">\n" +
				"	<source src=\"video/ie6.mp4\" type=\"video/mp4\">\n" +
				"	<object type=\"application/x-shockwave-flash\" data=\"player_flv_mini.swf\" " +
				"width=\"320\" height=\"240\">\n" +
				"		<param name=\"movie\" value=\"player_flv_mini.swf\">\n" +
				"		<param name=\"wmode\" value=\"opaque\">\n" +
				"		<param name=\"allowScriptAccess\" value=\"sameDomain\">\n" +
				"		<param name=\"quality\" value=\"high\">\n" +
				"		<param name=\"menu\" value=\"true\">\n" +
				"		<param name=\"autoplay\" value=\"false\">\n" +
				"		<param name=\"autoload\" value=\"false\">\n" +
				"		<param name=\"FlashVars\" value=\"flv=video/ie6.flv&amp;width=320&amp;height=240&amp;buffer=5\">\n" +
				"		<a href=\"video/ie6.flv\">Скачать видео-файл</a>\n" +
				"	</object>\n" +
				"</video>"
				;
			const string targetOutput6A = input6;
			const string targetOutput6B = "<video width=\"320\" height=\"240\" poster=\"video/poster.png\" controls=\"controls\">" +
				"<source src=\"video/ie6.ogv\" type=\"video/ogg\">" +
				"<source src=\"video/ie6.mp4\" type=\"video/mp4\">" +
				"<object type=\"application/x-shockwave-flash\" data=\"player_flv_mini.swf\" " +
				"width=\"320\" height=\"240\">" +
				"<param name=\"movie\" value=\"player_flv_mini.swf\">" +
				"<param name=\"wmode\" value=\"opaque\">" +
				"<param name=\"allowScriptAccess\" value=\"sameDomain\">" +
				"<param name=\"quality\" value=\"high\">" +
				"<param name=\"menu\" value=\"true\">" +
				"<param name=\"autoplay\" value=\"false\">" +
				"<param name=\"autoload\" value=\"false\">" +
				"<param name=\"FlashVars\" value=\"flv=video/ie6.flv&amp;width=320&amp;height=240&amp;buffer=5\">" +
				"<a href=\"video/ie6.flv\">Скачать видео-файл</a> " +
				"</object> " +
				"</video>"
				;
			const string targetOutput6C = targetOutput6B;
			const string targetOutput6D = "<video width=\"320\" height=\"240\" poster=\"video/poster.png\" controls=\"controls\">" +
				"<source src=\"video/ie6.ogv\" type=\"video/ogg\">" +
				"<source src=\"video/ie6.mp4\" type=\"video/mp4\">" +
				"<object type=\"application/x-shockwave-flash\" data=\"player_flv_mini.swf\" " +
				"width=\"320\" height=\"240\">" +
				"<param name=\"movie\" value=\"player_flv_mini.swf\">" +
				"<param name=\"wmode\" value=\"opaque\">" +
				"<param name=\"allowScriptAccess\" value=\"sameDomain\">" +
				"<param name=\"quality\" value=\"high\">" +
				"<param name=\"menu\" value=\"true\">" +
				"<param name=\"autoplay\" value=\"false\">" +
				"<param name=\"autoload\" value=\"false\">" +
				"<param name=\"FlashVars\" value=\"flv=video/ie6.flv&amp;width=320&amp;height=240&amp;buffer=5\">" +
				"<a href=\"video/ie6.flv\">Скачать видео-файл</a>" +
				"</object>" +
				"</video>"
				;


			const string input7 = "<ul>\n" +
				"	<li>	 Item 1 \n</li>\n" +
				"	<li>	 Item 2\n" +
				"		<ul>\n" +
				"			<li>	 Item 21 \n</li>\n" +
				"			<li>	 Item 22 \n</li>\n" +
				"		</ul>\n" +
				"	</li>\n" +
				"	<li>	 Item 3 \n</li>\n" +
				"</ul>"
				;
			const string targetOutput7A = input7;
			const string targetOutput7B = "<ul>" +
				"<li> Item 1 </li> " +
				"<li> Item 2 " +
				"<ul>" +
				"<li> Item 21 </li> " +
				"<li> Item 22 </li>" +
				"</ul> " +
				"</li> " +
				"<li> Item 3 </li>" +
				"</ul>"
				;
			const string targetOutput7C = "<ul>" +
				"<li>Item 1</li>" +
				"<li>Item 2" +
				"<ul>" +
				"<li>Item 21</li>" +
				"<li>Item 22</li>" +
				"</ul>" +
				"</li>" +
				"<li>Item 3</li>" +
				"</ul>"
				;
			const string targetOutput7D = targetOutput7C;


			const string input8 = "<p>	 one  </p>    \n" +
				"<p>  two	 </p>\n\n    \n\t\t  " +
				"<div title=\"Some title...\">  three	 </div>"
				;
			const string targetOutput8A = input8;
			const string targetOutput8B = "<p> one </p> " +
				"<p> two </p> " +
				"<div title=\"Some title...\"> three </div>"
				;
			const string targetOutput8C = "<p>one</p>" +
				"<p>two</p>" +
				"<div title=\"Some title...\">three</div>"
				;
			const string targetOutput8D = targetOutput8C;


			const string input9 = "<p>  New  \n  Release	\n</p>";
			const string targetOutput9A = input9;
			const string targetOutput9B = "<p> New Release </p>";
			const string targetOutput9C = "<p>New Release</p>";
			const string targetOutput9D = targetOutput9C;


			const string input10 = "<p> I'll   tell  you   my  story  with    <span>  5   slides </span> -  " +
				"<img src=\"\"> <span>	!  </span>	</p>";
			const string targetOutput10A = input10;
			const string targetOutput10B = "<p> I'll tell you my story with <span> 5 slides </span> - " +
				"<img src=\"\"> <span> ! </span> </p>";
			const string targetOutput10C = "<p>I'll tell you my story with <span> 5 slides </span> - " +
				"<img src=\"\"> <span> ! </span></p>";
			const string targetOutput10D = "<p>I'll tell you my story with <span>5 slides</span> - " +
				"<img src=\"\"> <span>!</span></p>";


			const string input11 = "<label>		Text:  \n </label> \n\t  " +
				"<textarea> THERE IS NO     KNOWLEDGE \n\n   – \t    THAT IS NOT POWER </textarea> \t\n  ";
			const string targetOutput11A = input11;
			const string targetOutput11B = "<label> Text: </label> " +
				"<textarea> THERE IS NO     KNOWLEDGE \n\n   – \t    THAT IS NOT POWER </textarea>";
			const string targetOutput11C = targetOutput11B;
			const string targetOutput11D = "<label>Text:</label> " +
				"<textarea> THERE IS NO     KNOWLEDGE \n\n   – \t    THAT IS NOT POWER </textarea>";


			const string input12 = "<span>Some text...</span> \n\t  " +
				"<pre title=\"Some title...\">   Some     text </pre> \t\n  " +
				"<span>Some text...</span>"
				;
			const string targetOutput12A = input12;
			const string targetOutput12B = "<span>Some text...</span> " +
				"<pre title=\"Some title...\">   Some     text </pre> " +
				"<span>Some text...</span>"
				;
			const string targetOutput12C = "<span>Some text...</span>" +
				"<pre title=\"Some title...\">   Some     text </pre>" +
				"<span>Some text...</span>"
				;
			const string targetOutput12D = targetOutput12C;


			const string input13 = "<p>Some text...</p> \n\t  " +
				"<pre title=\"Some title...\">	<code>   Some     text </code>\n</pre> \t\n  " +
				"<p>Some text...</p>"
				;
			const string targetOutput13A = input13;
			const string targetOutput13B = "<p>Some text...</p> " +
				"<pre title=\"Some title...\">	<code>   Some     text </code>\n</pre> " +
				"<p>Some text...</p>"
				;
			const string targetOutput13C = "<p>Some text...</p>" +
				"<pre title=\"Some title...\">	<code>   Some     text </code>\n</pre>" +
				"<p>Some text...</p>"
				;
			const string targetOutput13D = targetOutput13C;


			const string input14 = "<p>  An  <del>	 old \n</del>  <ins>	new  \n </ins>  embedded flash animation:  \n " +
				"<embed src=\"helloworld.swf\">	 !  </p>";
			const string targetOutput14A = input14;
			const string targetOutput14B = "<p> An <del> old </del> <ins> new </ins> embedded flash animation: " +
				"<embed src=\"helloworld.swf\"> ! </p>";
			const string targetOutput14C = "<p>An <del> old </del> <ins> new </ins> embedded flash animation: " +
				"<embed src=\"helloworld.swf\"> !</p>";
			const string targetOutput14D = "<p>An <del>old</del> <ins>new</ins> embedded flash animation: " +
				"<embed src=\"helloworld.swf\"> !</p>";


			const string input15 = "<div>\n" +
				"	<svg width=\"150\" height=\"100\" viewBox=\"0 0 3 2\">\n" +
				"		<rect width=\"1\" height=\"2\" x=\"0\" fill=\"#008d46\" />\n" +
				"		<rect width=\"1\" height=\"2\" x=\"1\" fill=\"#ffffff\" />\n" +
				"		<rect width=\"1\" height=\"2\" x=\"2\" fill=\"#d2232c\" />\n" +
				"	</svg>\n" +
				"</div>"
				;
			const string targetOutput15A = input15;
			const string targetOutput15B = "<div> " +
				"<svg width=\"150\" height=\"100\" viewBox=\"0 0 3 2\">" +
				"<rect width=\"1\" height=\"2\" x=\"0\" fill=\"#008d46\" />" +
				"<rect width=\"1\" height=\"2\" x=\"1\" fill=\"#ffffff\" />" +
				"<rect width=\"1\" height=\"2\" x=\"2\" fill=\"#d2232c\" />" +
				"</svg> " +
				"</div>"
				;
			const string targetOutput15C = "<div>" +
				"<svg width=\"150\" height=\"100\" viewBox=\"0 0 3 2\">" +
				"<rect width=\"1\" height=\"2\" x=\"0\" fill=\"#008d46\" />" +
				"<rect width=\"1\" height=\"2\" x=\"1\" fill=\"#ffffff\" />" +
				"<rect width=\"1\" height=\"2\" x=\"2\" fill=\"#d2232c\" />" +
				"</svg>" +
				"</div>"
				;
			const string targetOutput15D = targetOutput15C;


			const string input16 = "<div>\n" +
				"	<svg>\n" +
				"		<text x=\"20\" y=\"40\" " +
				"transform=\"rotate(30, 20,40)\" " +
				"style=\"stroke: none; fill: #000000\">\n" +
				"\t\t  Rotated  SVG  text  \t\n" +
				"		</text>\n" +
				"	</svg>\n" +
				"</div>"
				;
			const string targetOutput16A = input16;
			const string targetOutput16B = "<div> " +
				"<svg>" +
				"<text x=\"20\" y=\"40\" " +
				"transform=\"rotate(30, 20,40)\" " +
				"style=\"stroke: none; fill: #000000\">\n" +
				"\t\t  Rotated  SVG  text  \t\n" +
				"		</text>" +
				"</svg> " +
				"</div>"
				;
			const string targetOutput16C = "<div>" +
				"<svg>" +
				"<text x=\"20\" y=\"40\" " +
				"transform=\"rotate(30, 20,40)\" " +
				"style=\"stroke: none; fill: #000000\">\n" +
				"\t\t  Rotated  SVG  text  \t\n" +
				"		</text>" +
				"</svg>" +
				"</div>"
				;
			const string targetOutput16D = targetOutput16C;


			const string input17 = "<div>\n" +
				"	<math>\n" +
				"		<mrow>\n" +
				"			<mrow>\n" +
				"				<msup>\n" +
				"					<mi>a</mi>\n" +
				"					<mn>2</mn>\n" +
				"				</msup>\n" +
				"				<mo>+</mo>\n" +
				"				<msup>\n" +
				"					<mi>b</mi>\n" +
				"					<mn>2</mn>\n" +
				"				</msup>\n" +
				"			</mrow>\n" +
				"			<mo>=</mo>\n" +
				"			<msup>\n" +
				"				<mi>c</mi>\n" +
				"				<mn>2</mn>\n" +
				"			</msup>\n" +
				"		</mrow>\n" +
				"	</math>\n" +
				"</div>"
				;
			const string targetOutput17A = input17;
			const string targetOutput17B = "<div> " +
				"<math>" +
				"<mrow>" +
				"<mrow>" +
				"<msup>" +
				"<mi>a</mi>" +
				"<mn>2</mn>" +
				"</msup>" +
				"<mo>+</mo>" +
				"<msup>" +
				"<mi>b</mi>" +
				"<mn>2</mn>" +
				"</msup>" +
				"</mrow>" +
				"<mo>=</mo>" +
				"<msup>" +
				"<mi>c</mi>" +
				"<mn>2</mn>" +
				"</msup>" +
				"</mrow>" +
				"</math> " +
				"</div>"
				;
			const string targetOutput17C = "<div>" +
				"<math>" +
				"<mrow>" +
				"<mrow>" +
				"<msup>" +
				"<mi>a</mi>" +
				"<mn>2</mn>" +
				"</msup>" +
				"<mo>+</mo>" +
				"<msup>" +
				"<mi>b</mi>" +
				"<mn>2</mn>" +
				"</msup>" +
				"</mrow>" +
				"<mo>=</mo>" +
				"<msup>" +
				"<mi>c</mi>" +
				"<mn>2</mn>" +
				"</msup>" +
				"</mrow>" +
				"</math>" +
				"</div>"
				;
			const string targetOutput17D = targetOutput17C;


			const string input18 = "<dl>\n" +
				"	<dt>  Name:  </dt>\n" +
				"	<dd>  John Doe  \n" +
				"</dd>\n\n" +
				"	<dt>  Gender:  </dt>\n" +
				"	<dd>  Male  \n" +
				"</dd>\n\n" +
				"	<dt>  Day  of  Birth:  </dt>\n" +
				"	<dd>  Unknown  \n" +
				"</dd>\n" +
				"</dl>"
				;
			const string targetOutput18A = input18;
			const string targetOutput18B = "<dl>" +
				"<dt> Name: </dt> " +
				"<dd> John Doe </dd> " +
				"<dt> Gender: </dt> " +
				"<dd> Male </dd> " +
				"<dt> Day of Birth: </dt> " +
				"<dd> Unknown </dd>" +
				"</dl>"
				;
			const string targetOutput18C = "<dl>" +
				"<dt>Name:</dt>" +
				"<dd>John Doe</dd>" +
				"<dt>Gender:</dt>" +
				"<dd>Male</dd>" +
				"<dt>Day of Birth:</dt>" +
				"<dd>Unknown</dd>" +
				"</dl>"
				;
			const string targetOutput18D = targetOutput18C;


			const string input19 = "<menu>\n" +
				"	<menuitem label=\"New\" icon=\"icons/new.png\" onclick=\"new()\">\n" +
				"	</menuitem>\n" +
				"	<menuitem label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\">\n" +
				"	</menuitem>\n" +
				"	<menuitem label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\">\n" +
				"	</menuitem>\n" +
				"</menu>"
				;
			const string targetOutput19A = input19;
			const string targetOutput19B = "<menu>" +
				"<menuitem label=\"New\" icon=\"icons/new.png\" onclick=\"new()\"> </menuitem>" +
				"<menuitem label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\"> </menuitem>" +
				"<menuitem label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\"> </menuitem>" +
				"</menu>"
				;
			const string targetOutput19C = "<menu>" +
				"<menuitem label=\"New\" icon=\"icons/new.png\" onclick=\"new()\"></menuitem>" +
				"<menuitem label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\"></menuitem>" +
				"<menuitem label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\"></menuitem>" +
				"</menu>"
				;
			const string targetOutput19D = targetOutput19C;


			const string input20 = "<menu>\n" +
				"	<command type=\"command\" label=\"New\" icon=\"icons/new.png\" onclick=\"new()\">\n" +
				"	</command>\n" +
				"	<command type=\"command\" label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\">\n" +
				"	</command>\n" +
				"	<command type=\"command\" label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\">\n" +
				"	</command>\n" +
				"</menu>"
				;
			const string targetOutput20A = input20;
			const string targetOutput20B = "<menu>" +
				"<command type=\"command\" label=\"New\" icon=\"icons/new.png\" onclick=\"new()\"> </command>" +
				"<command type=\"command\" label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\"> </command>" +
				"<command type=\"command\" label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\"> </command>" +
				"</menu>"
				;
			const string targetOutput20C = "<menu>" +
				"<command type=\"command\" label=\"New\" icon=\"icons/new.png\" onclick=\"new()\"></command>" +
				"<command type=\"command\" label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\"></command>" +
				"<command type=\"command\" label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\"></command>" +
				"</menu>"
				;
			const string targetOutput20D = targetOutput20C;


			const string input21 = "<figure>\n" +
				"	<img src=\"libsass-logo.png\" alt=\"LibSass logo\" width=\"640\" height=\"320\">\n" +
				"	<figcaption>  Fig 1.  -  LibSass logo. \n" +
				"</figcaption>\n" +
				"</figure>"
				;
			const string targetOutput21A = input21;
			const string targetOutput21B = "<figure>" +
				" <img src=\"libsass-logo.png\" alt=\"LibSass logo\" width=\"640\" height=\"320\"> " +
				"<figcaption> Fig 1. - LibSass logo. </figcaption>" +
				"</figure>"
				;
			const string targetOutput21C = "<figure>" +
				"<img src=\"libsass-logo.png\" alt=\"LibSass logo\" width=\"640\" height=\"320\">" +
				"<figcaption>Fig 1. - LibSass logo.</figcaption>" +
				"</figure>"
				;
			const string targetOutput21D = targetOutput21C;


			const string input22 = "<form>\n" +
				"	<fieldset>\n" +
				"		<legend>  Personal data \n" +
				"</legend>\n" +
				"		Name: <input type=\"text\" size=\"50\"><br>\n" +
				"		Email: <input type=\"text\" size=\"50\"><br>\n" +
				"		Date of birth: <input type=\"text\" size=\"10\">\n" +
				"	</fieldset>\n" +
				"</form>"
				;
			const string targetOutput22A = input22;
			const string targetOutput22B = "<form> " +
				"<fieldset>" +
				"<legend> Personal data </legend>" +
				"Name: <input type=\"text\" size=\"50\"><br> " +
				"Email: <input type=\"text\" size=\"50\"><br> " +
				"Date of birth: <input type=\"text\" size=\"10\"> " +
				"</fieldset> " +
				"</form>"
				;
			const string targetOutput22C = "<form>" +
				"<fieldset>" +
				"<legend>Personal data</legend>" +
				"Name: <input type=\"text\" size=\"50\"><br> " +
				"Email: <input type=\"text\" size=\"50\"><br> " +
				"Date of birth: <input type=\"text\" size=\"10\">" +
				"</fieldset>" +
				"</form>"
				;
			const string targetOutput22D = targetOutput22C;


			const string input23 = "<ruby>\n" +
				"	漢  <rt>  Kan  </rt>\n" +
				"	字  <rt>  ji  </rt>\n" +
				"</ruby>"
				;
			const string targetOutput23A = input23;
			const string targetOutput23B = "<ruby> " +
				"漢 <rt> Kan </rt> " +
				"字 <rt> ji </rt> " +
				"</ruby>"
				;
			const string targetOutput23C = targetOutput23B;
			const string targetOutput23D = "<ruby>" +
				"漢 <rt>Kan</rt> " +
				"字 <rt>ji</rt>" +
				"</ruby>"
				;


			const string input24 = "<ruby>\n" +
				"	漢  <rp>  (</rp>  <rt>  Kan  </rt>  <rp>)  </rp>\n" +
				"	字  <rp>  (</rp>  <rt>  ji  </rt>  <rp>)  </rp>\n" +
				"</ruby>"
				;
			const string targetOutput24A = input24;
			const string targetOutput24B = "<ruby> " +
				"漢 <rp> (</rp> <rt> Kan </rt> <rp>) </rp> " +
				"字 <rp> (</rp> <rt> ji </rt> <rp>) </rp> " +
				"</ruby>"
				;
			const string targetOutput24C = targetOutput24B;
			const string targetOutput24D = "<ruby>" +
				"漢 <rp>(</rp> <rt>Kan</rt> <rp>)</rp> " +
				"字 <rp>(</rp> <rt>ji</rt> <rp>)</rp>" +
				"</ruby>"
				;


			const string input25 = "<ruby>\n" +
				"	東\n" +
				"	<rb>  京  </rb>\n" +
				"	<rt>  とう  </rt>\n" +
				"	<rt>  きょう  </rt>\n" +
				"</ruby>"
				;
			const string targetOutput25A = input25;
			const string targetOutput25B = "<ruby> " +
				"東 " +
				"<rb> 京 </rb> " +
				"<rt> とう </rt> " +
				"<rt> きょう </rt> " +
				"</ruby>";
			const string targetOutput25C = targetOutput25B;
			const string targetOutput25D = "<ruby>" +
				"東 " +
				"<rb>京</rb> " +
				"<rt>とう</rt> " +
				"<rt>きょう</rt>" +
				"</ruby>"
				;


			const string input26 = "<ruby>\n" +
				"	♥\n" +
				"	<rp>:  </rp>\n" +
				"	<rt>  Heart  </rt>\n" +
				"	<rp>,  </rp>\n" +
				"	<rtc>\n" +
				"		<rt lang=\"ru\">  Сердце  </rt>\n" +
				"	</rtc>\n" +
				"	<rp>.  </rp>\n" +
				"	☘\n" +
				"	<rp>:  </rp>\n" +
				"	<rt>  Shamrock  </rt>\n" +
				"	<rp>,  </rp>\n" +
				"	<rtc>\n" +
				"		<rt lang=\"ru\">  Трилистник  </rt>\n" +
				"	</rtc>\n" +
				"	<rp>.  </rp>\n" +
				"	✶\n" +
				"	<rp>:  </rp>\n" +
				"	<rt>  Star  </rt>\n" +
				"	<rp>,  </rp>\n" +
				"	<rtc>\n" +
				"		<rt lang=\"ru\">  Звезда  </rt>\n" +
				"	</rtc>\n" +
				"	<rp>.  </rp>\n" +
				"</ruby>"
				;
			const string targetOutput26A = input26;
			const string targetOutput26B = "<ruby> " +
				"♥ " +
				"<rp>: </rp> " +
				"<rt> Heart </rt> " +
				"<rp>, </rp> " +
				"<rtc> " +
				"<rt lang=\"ru\"> Сердце </rt> " +
				"</rtc> " +
				"<rp>. </rp> " +
				"☘ " +
				"<rp>: </rp> " +
				"<rt> Shamrock </rt> " +
				"<rp>, </rp> " +
				"<rtc> " +
				"<rt lang=\"ru\"> Трилистник </rt> " +
				"</rtc> " +
				"<rp>. </rp> " +
				"✶ " +
				"<rp>: </rp> " +
				"<rt> Star </rt> " +
				"<rp>, </rp> " +
				"<rtc> " +
				"<rt lang=\"ru\"> Звезда </rt> " +
				"</rtc> " +
				"<rp>. </rp> " +
				"</ruby>"
				;
			const string targetOutput26C = targetOutput26B;
			const string targetOutput26D = "<ruby>" +
				"♥ " +
				"<rp>:</rp> " +
				"<rt>Heart</rt> " +
				"<rp>,</rp> " +
				"<rtc>" +
				"<rt lang=\"ru\">Сердце</rt>" +
				"</rtc> " +
				"<rp>.</rp> " +
				"☘ " +
				"<rp>:</rp> " +
				"<rt>Shamrock</rt> " +
				"<rp>,</rp> " +
				"<rtc>" +
				"<rt lang=\"ru\">Трилистник</rt>" +
				"</rtc> " +
				"<rp>.</rp> " +
				"✶ " +
				"<rp>:</rp> " +
				"<rt>Star</rt> " +
				"<rp>,</rp> " +
				"<rtc>" +
				"<rt lang=\"ru\">Звезда</rt>" +
				"</rtc> " +
				"<rp>.</rp>" +
				"</ruby>"
				;


			const string input27 = "<ruby>\n" +
				"	<rb>  旧  </rb>  <rb>  金  </rb>  <rb>  山  </rb>\n" +
				"	<rt>  jiù  </rt>  <rt>  jīn  </rt> <rt>  shān  </rt>\n" +
				"	<rtc>  Сан-Франциско  </rtc>\n" +
				"</ruby>"
				;
			const string targetOutput27A = input27;
			const string targetOutput27B = "<ruby> " +
				"<rb> 旧 </rb> <rb> 金 </rb> <rb> 山 </rb> " +
				"<rt> jiù </rt> <rt> jīn </rt> <rt> shān </rt> " +
				"<rtc> Сан-Франциско </rtc> " +
				"</ruby>"
				;
			const string targetOutput27C = targetOutput27B;
			const string targetOutput27D = "<ruby>" +
				"<rb>旧</rb> <rb>金</rb> <rb>山</rb> " +
				"<rt>jiù</rt> <rt>jīn</rt> <rt>shān</rt> " +
				"<rtc>Сан-Франциско</rtc>" +
				"</ruby>"
				;

			// Act
			string output1A = keepingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1B = safeRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = mediumRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1D = aggressiveRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2B = safeRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = mediumRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2D = aggressiveRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3B = safeRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3C = mediumRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3D = aggressiveRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4B = safeRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4C = mediumRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4D = aggressiveRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5B = safeRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5C = mediumRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5D = aggressiveRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingWhitespaceMinifier.Minify(input6).MinifiedContent;
			string output6B = safeRemovingWhitespaceMinifier.Minify(input6).MinifiedContent;
			string output6C = mediumRemovingWhitespaceMinifier.Minify(input6).MinifiedContent;
			string output6D = aggressiveRemovingWhitespaceMinifier.Minify(input6).MinifiedContent;

			string output7A = keepingWhitespaceMinifier.Minify(input7).MinifiedContent;
			string output7B = safeRemovingWhitespaceMinifier.Minify(input7).MinifiedContent;
			string output7C = mediumRemovingWhitespaceMinifier.Minify(input7).MinifiedContent;
			string output7D = aggressiveRemovingWhitespaceMinifier.Minify(input7).MinifiedContent;

			string output8A = keepingWhitespaceMinifier.Minify(input8).MinifiedContent;
			string output8B = safeRemovingWhitespaceMinifier.Minify(input8).MinifiedContent;
			string output8C = mediumRemovingWhitespaceMinifier.Minify(input8).MinifiedContent;
			string output8D = aggressiveRemovingWhitespaceMinifier.Minify(input8).MinifiedContent;

			string output9A = keepingWhitespaceMinifier.Minify(input9).MinifiedContent;
			string output9B = safeRemovingWhitespaceMinifier.Minify(input9).MinifiedContent;
			string output9C = mediumRemovingWhitespaceMinifier.Minify(input9).MinifiedContent;
			string output9D = aggressiveRemovingWhitespaceMinifier.Minify(input9).MinifiedContent;

			string output10A = keepingWhitespaceMinifier.Minify(input10).MinifiedContent;
			string output10B = safeRemovingWhitespaceMinifier.Minify(input10).MinifiedContent;
			string output10C = mediumRemovingWhitespaceMinifier.Minify(input10).MinifiedContent;
			string output10D = aggressiveRemovingWhitespaceMinifier.Minify(input10).MinifiedContent;

			string output11A = keepingWhitespaceMinifier.Minify(input11).MinifiedContent;
			string output11B = safeRemovingWhitespaceMinifier.Minify(input11).MinifiedContent;
			string output11C = mediumRemovingWhitespaceMinifier.Minify(input11).MinifiedContent;
			string output11D = aggressiveRemovingWhitespaceMinifier.Minify(input11).MinifiedContent;

			string output12A = keepingWhitespaceMinifier.Minify(input12).MinifiedContent;
			string output12B = safeRemovingWhitespaceMinifier.Minify(input12).MinifiedContent;
			string output12C = mediumRemovingWhitespaceMinifier.Minify(input12).MinifiedContent;
			string output12D = aggressiveRemovingWhitespaceMinifier.Minify(input12).MinifiedContent;

			string output13A = keepingWhitespaceMinifier.Minify(input13).MinifiedContent;
			string output13B = safeRemovingWhitespaceMinifier.Minify(input13).MinifiedContent;
			string output13C = mediumRemovingWhitespaceMinifier.Minify(input13).MinifiedContent;
			string output13D = aggressiveRemovingWhitespaceMinifier.Minify(input13).MinifiedContent;

			string output14A = keepingWhitespaceMinifier.Minify(input14).MinifiedContent;
			string output14B = safeRemovingWhitespaceMinifier.Minify(input14).MinifiedContent;
			string output14C = mediumRemovingWhitespaceMinifier.Minify(input14).MinifiedContent;
			string output14D = aggressiveRemovingWhitespaceMinifier.Minify(input14).MinifiedContent;

			string output15A = keepingWhitespaceMinifier.Minify(input15).MinifiedContent;
			string output15B = safeRemovingWhitespaceMinifier.Minify(input15).MinifiedContent;
			string output15C = mediumRemovingWhitespaceMinifier.Minify(input15).MinifiedContent;
			string output15D = aggressiveRemovingWhitespaceMinifier.Minify(input15).MinifiedContent;

			string output16A = keepingWhitespaceMinifier.Minify(input16).MinifiedContent;
			string output16B = safeRemovingWhitespaceMinifier.Minify(input16).MinifiedContent;
			string output16C = mediumRemovingWhitespaceMinifier.Minify(input16).MinifiedContent;
			string output16D = aggressiveRemovingWhitespaceMinifier.Minify(input16).MinifiedContent;

			string output17A = keepingWhitespaceMinifier.Minify(input17).MinifiedContent;
			string output17B = safeRemovingWhitespaceMinifier.Minify(input17).MinifiedContent;
			string output17C = mediumRemovingWhitespaceMinifier.Minify(input17).MinifiedContent;
			string output17D = aggressiveRemovingWhitespaceMinifier.Minify(input17).MinifiedContent;

			string output18A = keepingWhitespaceMinifier.Minify(input18).MinifiedContent;
			string output18B = safeRemovingWhitespaceMinifier.Minify(input18).MinifiedContent;
			string output18C = mediumRemovingWhitespaceMinifier.Minify(input18).MinifiedContent;
			string output18D = aggressiveRemovingWhitespaceMinifier.Minify(input18).MinifiedContent;

			string output19A = keepingWhitespaceMinifier.Minify(input19).MinifiedContent;
			string output19B = safeRemovingWhitespaceMinifier.Minify(input19).MinifiedContent;
			string output19C = mediumRemovingWhitespaceMinifier.Minify(input19).MinifiedContent;
			string output19D = aggressiveRemovingWhitespaceMinifier.Minify(input19).MinifiedContent;

			string output20A = keepingWhitespaceMinifier.Minify(input20).MinifiedContent;
			string output20B = safeRemovingWhitespaceMinifier.Minify(input20).MinifiedContent;
			string output20C = mediumRemovingWhitespaceMinifier.Minify(input20).MinifiedContent;
			string output20D = aggressiveRemovingWhitespaceMinifier.Minify(input20).MinifiedContent;

			string output21A = keepingWhitespaceMinifier.Minify(input21).MinifiedContent;
			string output21B = safeRemovingWhitespaceMinifier.Minify(input21).MinifiedContent;
			string output21C = mediumRemovingWhitespaceMinifier.Minify(input21).MinifiedContent;
			string output21D = aggressiveRemovingWhitespaceMinifier.Minify(input21).MinifiedContent;

			string output22A = keepingWhitespaceMinifier.Minify(input22).MinifiedContent;
			string output22B = safeRemovingWhitespaceMinifier.Minify(input22).MinifiedContent;
			string output22C = mediumRemovingWhitespaceMinifier.Minify(input22).MinifiedContent;
			string output22D = aggressiveRemovingWhitespaceMinifier.Minify(input22).MinifiedContent;

			string output23A = keepingWhitespaceMinifier.Minify(input23).MinifiedContent;
			string output23B = safeRemovingWhitespaceMinifier.Minify(input23).MinifiedContent;
			string output23C = mediumRemovingWhitespaceMinifier.Minify(input23).MinifiedContent;
			string output23D = aggressiveRemovingWhitespaceMinifier.Minify(input23).MinifiedContent;

			string output24A = keepingWhitespaceMinifier.Minify(input24).MinifiedContent;
			string output24B = safeRemovingWhitespaceMinifier.Minify(input24).MinifiedContent;
			string output24C = mediumRemovingWhitespaceMinifier.Minify(input24).MinifiedContent;
			string output24D = aggressiveRemovingWhitespaceMinifier.Minify(input24).MinifiedContent;

			string output25A = keepingWhitespaceMinifier.Minify(input25).MinifiedContent;
			string output25B = safeRemovingWhitespaceMinifier.Minify(input25).MinifiedContent;
			string output25C = mediumRemovingWhitespaceMinifier.Minify(input25).MinifiedContent;
			string output25D = aggressiveRemovingWhitespaceMinifier.Minify(input25).MinifiedContent;

			string output26A = keepingWhitespaceMinifier.Minify(input26).MinifiedContent;
			string output26B = safeRemovingWhitespaceMinifier.Minify(input26).MinifiedContent;
			string output26C = mediumRemovingWhitespaceMinifier.Minify(input26).MinifiedContent;
			string output26D = aggressiveRemovingWhitespaceMinifier.Minify(input26).MinifiedContent;

			string output27A = keepingWhitespaceMinifier.Minify(input27).MinifiedContent;
			string output27B = safeRemovingWhitespaceMinifier.Minify(input27).MinifiedContent;
			string output27C = mediumRemovingWhitespaceMinifier.Minify(input27).MinifiedContent;
			string output27D = aggressiveRemovingWhitespaceMinifier.Minify(input27).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);
			Assert.Equal(targetOutput1D, output1D);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);
			Assert.Equal(targetOutput2D, output2D);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);
			Assert.Equal(targetOutput3D, output3D);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);
			Assert.Equal(targetOutput4D, output4D);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);
			Assert.Equal(targetOutput5D, output5D);

			Assert.Equal(targetOutput6A, output6A);
			Assert.Equal(targetOutput6B, output6B);
			Assert.Equal(targetOutput6C, output6C);
			Assert.Equal(targetOutput6D, output6D);

			Assert.Equal(targetOutput7A, output7A);
			Assert.Equal(targetOutput7B, output7B);
			Assert.Equal(targetOutput7C, output7C);
			Assert.Equal(targetOutput7D, output7D);

			Assert.Equal(targetOutput8A, output8A);
			Assert.Equal(targetOutput8B, output8B);
			Assert.Equal(targetOutput8C, output8C);
			Assert.Equal(targetOutput8D, output8D);

			Assert.Equal(targetOutput9A, output9A);
			Assert.Equal(targetOutput9B, output9B);
			Assert.Equal(targetOutput9C, output9C);
			Assert.Equal(targetOutput9D, output9D);

			Assert.Equal(targetOutput10A, output10A);
			Assert.Equal(targetOutput10B, output10B);
			Assert.Equal(targetOutput10C, output10C);
			Assert.Equal(targetOutput10D, output10D);

			Assert.Equal(targetOutput11A, output11A);
			Assert.Equal(targetOutput11B, output11B);
			Assert.Equal(targetOutput11C, output11C);
			Assert.Equal(targetOutput11D, output11D);

			Assert.Equal(targetOutput12A, output12A);
			Assert.Equal(targetOutput12B, output12B);
			Assert.Equal(targetOutput12C, output12C);
			Assert.Equal(targetOutput12D, output12D);

			Assert.Equal(targetOutput13A, output13A);
			Assert.Equal(targetOutput13B, output13B);
			Assert.Equal(targetOutput13C, output13C);
			Assert.Equal(targetOutput13D, output13D);

			Assert.Equal(targetOutput14A, output14A);
			Assert.Equal(targetOutput14B, output14B);
			Assert.Equal(targetOutput14C, output14C);
			Assert.Equal(targetOutput14D, output14D);

			Assert.Equal(targetOutput15A, output15A);
			Assert.Equal(targetOutput15B, output15B);
			Assert.Equal(targetOutput15C, output15C);
			Assert.Equal(targetOutput15D, output15D);

			Assert.Equal(targetOutput16A, output16A);
			Assert.Equal(targetOutput16B, output16B);
			Assert.Equal(targetOutput16C, output16C);
			Assert.Equal(targetOutput16D, output16D);

			Assert.Equal(targetOutput17A, output17A);
			Assert.Equal(targetOutput17B, output17B);
			Assert.Equal(targetOutput17C, output17C);
			Assert.Equal(targetOutput17D, output17D);

			Assert.Equal(targetOutput18A, output18A);
			Assert.Equal(targetOutput18B, output18B);
			Assert.Equal(targetOutput18C, output18C);
			Assert.Equal(targetOutput18D, output18D);

			Assert.Equal(targetOutput19A, output19A);
			Assert.Equal(targetOutput19B, output19B);
			Assert.Equal(targetOutput19C, output19C);
			Assert.Equal(targetOutput19D, output19D);

			Assert.Equal(targetOutput20A, output20A);
			Assert.Equal(targetOutput20B, output20B);
			Assert.Equal(targetOutput20C, output20C);
			Assert.Equal(targetOutput20D, output20D);

			Assert.Equal(targetOutput21A, output21A);
			Assert.Equal(targetOutput21B, output21B);
			Assert.Equal(targetOutput21C, output21C);
			Assert.Equal(targetOutput21D, output21D);

			Assert.Equal(targetOutput22A, output22A);
			Assert.Equal(targetOutput22B, output22B);
			Assert.Equal(targetOutput22C, output22C);
			Assert.Equal(targetOutput22D, output22D);

			Assert.Equal(targetOutput23A, output23A);
			Assert.Equal(targetOutput23B, output23B);
			Assert.Equal(targetOutput23C, output23C);
			Assert.Equal(targetOutput23D, output23D);

			Assert.Equal(targetOutput24A, output24A);
			Assert.Equal(targetOutput24B, output24B);
			Assert.Equal(targetOutput24C, output24C);
			Assert.Equal(targetOutput24D, output24D);

			Assert.Equal(targetOutput25A, output25A);
			Assert.Equal(targetOutput25B, output25B);
			Assert.Equal(targetOutput25C, output25C);
			Assert.Equal(targetOutput25D, output25D);

			Assert.Equal(targetOutput26A, output26A);
			Assert.Equal(targetOutput26B, output26B);
			Assert.Equal(targetOutput26C, output26C);
			Assert.Equal(targetOutput26D, output26D);

			Assert.Equal(targetOutput27A, output27A);
			Assert.Equal(targetOutput27B, output27B);
			Assert.Equal(targetOutput27C, output27C);
			Assert.Equal(targetOutput27D, output27D);
		}

		#endregion

		#region Processing HTML comments

		[Fact]
		public void RemovingHtmlCommentsIsCorrect()
		{
			// Arrange
			var keepingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = false });
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = true });

			const string input1 = "<!---->";
			const string targetOutput1A = input1;
			const string targetOutput1B = "";

			const string input2 = "<!-- -->";
			const string targetOutput2A = input2;
			const string targetOutput2B = "";

			const string input3 = "<!-- Some comment... -->";
			const string targetOutput3A = input3;
			const string targetOutput3B = "";

			const string input4 = "<!-- Initial comment... -->" +
				"<div>Some text...</div>" +
				"<!-- Final comment\n\n Some other comments ... -->"
				;
			const string targetOutput4A = input4;
			const string targetOutput4B = "<div>Some text...</div>";

			const string input5 = "<p title=\"&lt;!-- Some comment... -->\">Some text...</p>";

			const string input6 = "<div>\n" +
				"\t<svg width=\"150\" height=\"100\" viewBox=\"0 0 3 2\">\n" +
				"\t\t<!-- SVG content -->\n" +
				"\t\t<rect width=\"1\" height=\"2\" x=\"0\" fill=\"#008d46\" />\n" +
				"\t\t<rect width=\"1\" height=\"2\" x=\"1\" fill=\"#ffffff\" />\n" +
				"\t\t<rect width=\"1\" height=\"2\" x=\"2\" fill=\"#d2232c\" />\n" +
				"\t\t<!-- /SVG content -->\n" +
				"\t</svg>\n" +
				"</div>"
				;
			const string targetOutput6A = input6;
			const string targetOutput6B = "<div>\n" +
				"\t<svg width=\"150\" height=\"100\" viewBox=\"0 0 3 2\">\n" +
				"\t\t\n" +
				"\t\t<rect width=\"1\" height=\"2\" x=\"0\" fill=\"#008d46\" />\n" +
				"\t\t<rect width=\"1\" height=\"2\" x=\"1\" fill=\"#ffffff\" />\n" +
				"\t\t<rect width=\"1\" height=\"2\" x=\"2\" fill=\"#d2232c\" />\n" +
				"\t\t\n" +
				"\t</svg>\n" +
				"</div>";

			const string input7 = "<div>\n" +
				"\t<math>\n" +
				"\t\t<!-- MathML content -->\n" +
				"\t\t<mrow>\n" +
				"\t\t\t<mrow>\n" +
				"\t\t\t\t<msup>\n" +
				"\t\t\t\t\t<mi>a</mi>\n" +
				"\t\t\t\t\t<mn>2</mn>\n" +
				"\t\t\t\t</msup>\n" +
				"\t\t\t\t<mo>+</mo>\n" +
				"\t\t\t\t<msup>\n" +
				"\t\t\t\t\t<mi>b</mi>\n" +
				"\t\t\t\t\t<mn>2</mn>\n" +
				"\t\t\t\t</msup>\n" +
				"\t\t\t</mrow>\n" +
				"\t\t\t<mo>=</mo>\n" +
				"\t\t\t<msup>\n" +
				"\t\t\t\t<mi>c</mi>\n" +
				"\t\t\t\t<mn>2</mn>\n" +
				"\t\t\t</msup>\n" +
				"\t\t</mrow>\n" +
				"\t\t<!-- /MathML content -->\n" +
				"\t</math>\n" +
				"</div>"
			;
			const string targetOutput7A = input7;
			const string targetOutput7B = "<div>\n" +
				"\t<math>\n" +
				"\t\t\n" +
				"\t\t<mrow>\n" +
				"\t\t\t<mrow>\n" +
				"\t\t\t\t<msup>\n" +
				"\t\t\t\t\t<mi>a</mi>\n" +
				"\t\t\t\t\t<mn>2</mn>\n" +
				"\t\t\t\t</msup>\n" +
				"\t\t\t\t<mo>+</mo>\n" +
				"\t\t\t\t<msup>\n" +
				"\t\t\t\t\t<mi>b</mi>\n" +
				"\t\t\t\t\t<mn>2</mn>\n" +
				"\t\t\t\t</msup>\n" +
				"\t\t\t</mrow>\n" +
				"\t\t\t<mo>=</mo>\n" +
				"\t\t\t<msup>\n" +
				"\t\t\t\t<mi>c</mi>\n" +
				"\t\t\t\t<mn>2</mn>\n" +
				"\t\t\t</msup>\n" +
				"\t\t</mrow>\n" +
				"\t\t\n" +
				"\t</math>\n" +
				"</div>"
			;

			// Act
			string output1A = keepingHtmlCommentsMinifier.Minify(input1).MinifiedContent;
			string output1B = removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingHtmlCommentsMinifier.Minify(input2).MinifiedContent;
			string output2B = removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingHtmlCommentsMinifier.Minify(input3).MinifiedContent;
			string output3B = removingHtmlCommentsMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingHtmlCommentsMinifier.Minify(input4).MinifiedContent;
			string output4B = removingHtmlCommentsMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingHtmlCommentsMinifier.Minify(input5).MinifiedContent;
			string output5B = removingHtmlCommentsMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingHtmlCommentsMinifier.Minify(input6).MinifiedContent;
			string output6B = removingHtmlCommentsMinifier.Minify(input6).MinifiedContent;

			string output7A = keepingHtmlCommentsMinifier.Minify(input7).MinifiedContent;
			string output7B = removingHtmlCommentsMinifier.Minify(input7).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);

			Assert.Equal(input5, output5A);
			Assert.Equal(input5, output5B);

			Assert.Equal(targetOutput6A, output6A);
			Assert.Equal(targetOutput6B, output6B);

			Assert.Equal(targetOutput7A, output7A);
			Assert.Equal(targetOutput7B, output7B);
		}

		[Fact]
		public void ProcessingConditionalCommentsIsCorrect()
		{
			// Arrange
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = true });

			const string input1 = "<!--[if lt IE 9]>" +
				"<script src=\"Scripts/html5shiv.js\"></script>" +
				"<![endif]-->"
				;

			const string input2 = "<!--[if IEMobile 7]> <html class=\"no-js iem7\"> <![endif \t  ]-->\n" +
				"<!--[if lt IE 9 ]> <html class=\"no-js lte-ie8\"> <![endif  \n ]-->\n" +
				"<!--[if (gt IE 8)|(gt IEMobile 7)|!(IEMobile)|!(IE)]> \t\n  <!  \t\n --> <html class=\"no-js\" lang=\"en\"> <!-- \t\n  <![endif  \t\n ]-->\n"
				;
			const string targetOutput2 = "<!--[if IEMobile 7]> <html class=\"no-js iem7\"> <![endif]-->\n" +
				"<!--[if lt IE 9]> <html class=\"no-js lte-ie8\"> <![endif]-->\n" +
				"<!--[if (gt IE 8)|(gt IEMobile 7)|!(IEMobile)|!(IE)]><!--> <html class=\"no-js\" lang=\"en\"> <!--<![endif]-->\n" +
				"</html>"
				;

			const string input3 = "<!--[if IEMobile 7 ]> <html class=\"no-js iem7\"> <![endif]-->\n" +
				"<!--[if lt IE 9]> <html class=\"no-js lte-ie8\"> <![endif]-->\n" +
				"<!--[if (gt IE 8)|(gt IEMobile 7)|!(IEMobile)|!(IE)]><!--> <html class=\"no-js\" lang=\"en\"> <!--<![endif]-->\n"
				;
			const string targetOutput3 = "<!--[if IEMobile 7]> <html class=\"no-js iem7\"> <![endif]-->\n" +
				"<!--[if lt IE 9]> <html class=\"no-js lte-ie8\"> <![endif]-->\n" +
				"<!--[if (gt IE 8)|(gt IEMobile 7)|!(IEMobile)|!(IE)]><!--> <html class=\"no-js\" lang=\"en\"> <!--<![endif]-->\n" +
				"</html>"
				;

			const string input4 = "<!--[if lt IE 7 ]> <html class=\"no-js ie6 ielt8\"> <![endif]-->\n" +
				"<!--[if IE 7 ]> <html class=\"no-js ie7 ielt8\"> <![endif]-->\n" +
				"<!--[if IE 8 ]> <html class=\"no-js ie8\"> <![endif]-->\n" +
				"<!--[if IE 9 ]> <html class=\"no-js ie9\"> <![endif]-->\n" +
				"<!--[if (gt IE 9)|!(IE)]> \t\n  --> <html class=\"no-js\"> <!--  \t\n <![endif \t\n  ]-->\n"
				;
			const string targetOutput4 = "<!--[if lt IE 7]> <html class=\"no-js ie6 ielt8\"> <![endif]-->\n" +
				"<!--[if IE 7]> <html class=\"no-js ie7 ielt8\"> <![endif]-->\n" +
				"<!--[if IE 8]> <html class=\"no-js ie8\"> <![endif]-->\n" +
				"<!--[if IE 9]> <html class=\"no-js ie9\"> <![endif]-->\n" +
				"<!--[if (gt IE 9)|!(IE)]>--> <html class=\"no-js\"> <!--<![endif]-->\n" +
				"</html>"
				;

			const string input5 = "<!--[if lt IE 7]> <html class=\"no-js ie6 oldie\" " +
				"data-text-direction=\"left-to-right\">\n" +
				"<![endif  \t ]-->\n" +
				"<!--[if IE 7]> <html class=\"no-js ie7 oldie\" " +
				"data-text-direction=\"left-to-right\">\n" +
				"<![endif \n  ]-->\n" +
				"<!--[if IE 8]> <html class=\"no-js ie8 oldie\" " +
				"data-text-direction=\"left-to-right\">\n" +
				"<![endif]-->\n" +
				"<!--[if gt IE 8]>--> <html class=\"no-js\"" +
				"data-text-direction=\"left-to-right\">\n" +
				"<!--<![endif]-->\n"
				;
			const string targetOutput5 = "<!--[if lt IE 7]> <html class=\"no-js ie6 oldie\" " +
				"data-text-direction=\"left-to-right\">\n" +
				"<![endif]-->\n" +
				"<!--[if IE 7]> <html class=\"no-js ie7 oldie\" " +
				"data-text-direction=\"left-to-right\">\n" +
				"<![endif]-->\n" +
				"<!--[if IE 8]> <html class=\"no-js ie8 oldie\" " +
				"data-text-direction=\"left-to-right\">\n" +
				"<![endif]-->\n" +
				"<!--[if gt IE 8]>--> <html class=\"no-js\" " +
				"data-text-direction=\"left-to-right\">\n" +
				"<!--<![endif]-->\n" +
				"</html>"
				;

			const string input6 = "<!--[if IE 7]>\n" +
				"<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/site_ie7.css\" media=\"all\">\n" +
				"	<![endif]-->\n\n" +
				"<!--[if IE 6]>\n" +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/site_ie6.css\" media=\"all\">\n " +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/ie6_site_extras.css\" media=\"all\">\n" +
				"<![endif]-->\n\n" +
				"<!--[if IE 5]>\n" +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/site_ie5.css\" media=\"all\">\n" +
				"<![endif]-->"
				;

			const string input7 = "<!--[if IE 7 ]>\n" +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/site_ie7.css\" media=\"all\"/>\n" +
				"<![endif]-->\n\n" +
				"<!--[if IE 6 ]>\n" +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/site_ie6.css\" media=\"all\"/>\n" +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/ie6_site_extras.css\" media=\"all\"/>\n" +
				"<![endif]-->\n\n" +
				"<!--[if IE 5 ]>\n" +
				"	<link  \trel=\"stylesheet\" type=\"text/css\"  href=\"/Content/site_ie5.css\" media=\"all\"/>\n" +
				"<![endif]-->"
				;
			const string targetOutput7 = "<!--[if IE 7]>\n" +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/site_ie7.css\" media=\"all\">\n" +
				"<![endif]-->\n\n" +
				"<!--[if IE 6]>\n" +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/site_ie6.css\" media=\"all\">\n" +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/ie6_site_extras.css\" media=\"all\">\n" +
				"<![endif]-->\n\n" +
				"<!--[if IE 5]>\n" +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/site_ie5.css\" media=\"all\">\n" +
				"<![endif]-->"
				;

			const string input8 = "<![if IE 5.0000]>\n" +
				"	<p>Welcome to Internet Explorer 5.0!</p>\n" +
				"<![endif \t  ]>\n" +
				"<![if IE 5]>\n" +
				"	<p>Welcome to any incremental version of Internet Explorer 5!</p>\n" +
				"<![endif  \n ]>\n" +
				"<![if lt IE 8]>\n" +
				"	<p class=\"text-warning\">Please upgrade to Internet Explorer version 8.</p>\n" +
				"<![endif]>"
				;
			const string targetOutput8 = "<![if IE 5.0000]>\n" +
				"	<p>Welcome to Internet Explorer 5.0!</p>\n" +
				"<![endif]>\n" +
				"<![if IE 5]>\n" +
				"	<p>Welcome to any incremental version of Internet Explorer 5!</p>\n" +
				"<![endif]>\n" +
				"<![if lt IE 8]>\n" +
				"	<p class=\"text-warning\">Please upgrade to Internet Explorer version 8.</p>\n" +
				"<![endif]>"
				;

			const string input9 = "<![if IE 5.0000 ]>\n" +
				"	<p>Welcome to Internet Explorer 5.0!</p>\n" +
				"<![endif \t  ]>\n" +
				"<![if IE 5 ]>\n" +
				"	<p>Welcome to any incremental version of Internet Explorer 5!</p>\n" +
				"<![endif  \n ]>\n" +
				"<![if lt IE 8]>\n" +
				"	<p class=\"text-warning\">Please upgrade to Internet Explorer version 8.</p>\n" +
				"<![endif]>"
				;
			const string targetOutput9 = "<![if IE 5.0000]>\n" +
				"	<p>Welcome to Internet Explorer 5.0!</p>\n" +
				"<![endif]>\n" +
				"<![if IE 5]>\n" +
				"	<p>Welcome to any incremental version of Internet Explorer 5!</p>\n" +
				"<![endif]>\n" +
				"<![if lt IE 8]>\n" +
				"	<p class=\"text-warning\">Please upgrade to Internet Explorer version 8.</p>\n" +
				"<![endif]>"
				;

			const string input10 = "<picture>\n" +
				"	<!--[if IE 9]><video style=\"display: none\"><![endif]-->\n" +
				"	<source srcset=\"examples/images/extralarge.jpg\" media=\"(min-width: 1000px)\">\n" +
				"	<source srcset=\"examples/images/large.jpg\" media=\"(min-width: 800px)\">\n" +
				"	<source srcset=\"examples/images/medium.jpg\">\n" +
				"	<!--[if IE 9]></video><![endif]-->\n" +
				"	<img srcset=\"examples/images/medium.jpg\">\n" +
				"</picture>"
				;

			const string input11 = "<!--[if lt IE 9]>\n" +
				"<script src=\"//ajax.aspnetcdn.com/ajax/jquery/jquery-1.11.1.min.js\"></script>\n" +
				"<script>\n" +
				"	window.jQuery || document.write('<script src=\"/scripts/jquery-1.11.1.min.js\"><\\/script>')\n" +
				"</script>\n" +
				"<![endif]-->\n" +
				"<!--[if gte IE 9]><!-->\n" +
				"<script src=\"//ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.1.min.js\"></script>\n" +
				"<script>\n" +
				"	window.jQuery || document.write('<script src=\"/scripts/jquery-2.1.1.min.js\"><\\/script>')\n" +
				"</script>\n" +
				"<!--<![endif]-->"
				;

			// Act
			string output1 = removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;
			string output2 = removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;
			string output3 = removingHtmlCommentsMinifier.Minify(input3).MinifiedContent;
			string output4 = removingHtmlCommentsMinifier.Minify(input4).MinifiedContent;
			string output5 = removingHtmlCommentsMinifier.Minify(input5).MinifiedContent;
			string output6 = removingHtmlCommentsMinifier.Minify(input6).MinifiedContent;
			string output7 = removingHtmlCommentsMinifier.Minify(input7).MinifiedContent;
			string output8 = removingHtmlCommentsMinifier.Minify(input8).MinifiedContent;
			string output9 = removingHtmlCommentsMinifier.Minify(input9).MinifiedContent;
			string output10 = removingHtmlCommentsMinifier.Minify(input10).MinifiedContent;
			string output11 = removingHtmlCommentsMinifier.Minify(input11).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
			Assert.Equal(input6, output6);
			Assert.Equal(targetOutput7, output7);
			Assert.Equal(targetOutput8, output8);
			Assert.Equal(targetOutput9, output9);
			Assert.Equal(input10, output10);
			Assert.Equal(input11, output11);
		}

		[Fact]
		public void ProcessingNoindexCommentsIsCorrect()
		{
			// Arrange
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = true });

			const string input1 = "<div class=\"selector\">\n" +
				"<!--noindex--><a href=\"?orderBy=relevance\" rel=\"nofollow\">Sort by relevance</a><!--/noindex-->\n" +
				"</div>"
				;

			const string input2 = "<div class=\"selector\">\n" +
				"<!--NOINDEX--><a href=\"?orderBy=relevance\" rel=\"nofollow\">Sort by relevance</a><!--/NOINDEX-->\n" +
				"</div>"
				;
			const string targetOutput2 = "<div class=\"selector\">\n" +
				"<!--noindex--><a href=\"?orderBy=relevance\" rel=\"nofollow\">Sort by relevance</a><!--/noindex-->\n" +
				"</div>"
				;

			// Act
			string output1 = removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;
			string output2 = removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(targetOutput2, output2);
		}

		#endregion

		#region Processing HTML comments in scripts and styles

		[Fact]
		public void ProcessingHtmlCommentsInStylesIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));
			var removingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode= WhitespaceMinificationMode.Medium });
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlCommentsFromScriptsAndStyles = true });

			const string input1 = "<style type=\"text/css\"><!--\tp { color: #f00; } --></style>";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<style type=\"text/css\"><!--p { color: #f00; }--></style>";
			const string targetOutput1C = "<style type=\"text/css\">\tp { color: #f00; } </style>";


			const string input2 = "<style type=\"text/css\">\r\n" +
				"<!--\r\n" +
				"\tp { color: #f00; } " +
				"\r\n-->" +
				"\r\n</style>"
				;
			const string targetOutput2A = "<style type=\"text/css\">" +
				"<!--" +
				"\tp { color: #f00; } " +
				"-->" +
				"</style>"
				;
			const string targetOutput2B = "<style type=\"text/css\">" +
				"<!--" +
				"p { color: #f00; }" +
				"-->" +
				"</style>"
				;
			const string targetOutput2C = "<style type=\"text/css\">" +
				"\tp { color: #f00; } " +
				"</style>"
				;


			const string input3 = "<style type=\"text/css\">\tp::before { content: \"<!--\" } </style>";
			const string targetOutput3A = input3;
			const string targetOutput3B = "<style type=\"text/css\">p::before { content: \"<!--\" }</style>";
			const string targetOutput3C = input3;

			// Act
			string output1A = minifier.Minify(input1).MinifiedContent;
			string output1B = removingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;

			string output2A = minifier.Minify(input2).MinifiedContent;
			string output2B = removingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			string output3A = minifier.Minify(input3).MinifiedContent;
			string output3B = removingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3C = removingHtmlCommentsMinifier.Minify(input3).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);
		}

		[Fact]
		public void ProcessingHtmlCommentsInScriptsIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));
			var removingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlCommentsFromScriptsAndStyles = true });

			const string input1 = "<script type=\"text/javascript\">\r\n" +
				"<!-- alert('Hooray!'); -->" +
				"\r\n</script>"
				;
			const string targetOutput1A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput1B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput1C = "<script type=\"text/javascript\"></script>";


			const string input2 = "<script type=\"text/javascript\">\r\n" +
				"<!-- alert('-->'); -->" +
				"\r\n</script>"
				;
			const string targetOutput2A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput2B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput2C = "<script type=\"text/javascript\"></script>";


			const string input3 = "<script type=\"text/javascript\">\r\n" +
				"<!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n-->" +
				"\r\n</script>"
				;
			const string targetOutput3A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput3B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput3C = "<script type=\"text/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input4 = "<script type=\"text/javascript\">\r\n" +
				"<!--\r\n" +
				"\talert('-->'); " +
				"\r\n-->" +
				"\r\n</script>"
				;
			const string targetOutput4A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('-->'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput4B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('-->');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput4C = "<script type=\"text/javascript\">" +
				"\talert('-->'); " +
				"</script>"
				;


			const string input5 = "<script type=\"text/javascript\">\r\n" +
				"<!-- alert('Test 1');\r\n" +
				"\talert('Test 2'); \r\n" +
				"\talert('Test 3'); -->" +
				"\r\n</script>"
				;
			const string targetOutput5A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('Test 2'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput5B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('Test 2');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput5C = "<script type=\"text/javascript\">" +
				"\talert('Test 2'); " +
				"</script>"
				;


			const string input6 = "<script type=\"text/javascript\">\r\n" +
				"<!-- alert('1 -->');\r\n" +
				"\talert('2 -->'); \r\n" +
				"\talert('3 -->'); -->" +
				"\r\n</script>"
				;
			const string targetOutput6A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('2 -->'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput6B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('2 -->');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput6C = "<script type=\"text/javascript\">" +
				"\talert('2 -->'); " +
				"</script>"
				;

			const string input7 = "<script type=\"text/javascript\">\r\n" +
				"<!--//\r\n" +
				"\talert('-->'); " +
				"\r\n//-->" +
				"\r\n</script>"
				;
			const string targetOutput7A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('-->'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput7B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('-->');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput7C = "<script type=\"text/javascript\">" +
				"\talert('-->'); " +
				"</script>"
				;


			const string input8 = "<script type=\"text/javascript\">\r\n" +
				"<!-- //\r\n" +
				"\talert('-->'); " +
				"\r\n// -->" +
				"\r\n</script>"
				;
			const string targetOutput8A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('-->'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput8B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('-->');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput8C = "<script type=\"text/javascript\">" +
				"\talert('-->'); " +
				"</script>"
				;


			const string input9 = "<script type=\"text/javascript\">\r\n" +
				"//<!--\r\n" +
				"\talert('-->'); " +
				"\r\n//-->" +
				"\r\n</script>"
				;
			const string targetOutput9A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('-->'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput9B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('-->');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput9C = "<script type=\"text/javascript\">" +
				"\talert('-->'); " +
				"</script>"
				;


			const string input10 = "<script type=\"text/javascript\">\r\n" +
				"// <!--\r\n" +
				"\talert('-->'); " +
				"\r\n// -->" +
				"\r\n</script>"
				;
			const string targetOutput10A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('-->'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput10B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('-->');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput10C = "<script type=\"text/javascript\">" +
				"\talert('-->'); " +
				"</script>"
				;


			const string input11 = "<script type=\"text/javascript\">\r\n" +
				"<!-- Earlier versions of browsers ignore this code\r\n" +
				"\talert('Hooray!'); " +
				"\r\nHiding code stops here -->\r\n" +
				"</script>"
				;
			const string targetOutput11A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput11B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput11C = "<script type=\"text/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input12 = "<script type=\"text/javascript\">\r\n" +
				"<!-- Earlier versions of browsers ignore this code\r\n" +
				"\talert('Hooray!'); " +
				"\r\n// Hiding code stops here -->\r\n" +
				"</script>"
				;
			const string targetOutput12A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput12B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput12C = "<script type=\"text/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input13 = "<script type=\"text/ecmascript\">\r\n" +
				"<!-- // Earlier versions of browsers ignore this code\r\n" +
				"\talert('Hooray!'); " +
				"\r\n// Hiding code stops here -->\r\n" +
				"</script>"
				;
			const string targetOutput13A = "<script type=\"text/ecmascript\">" +
				"<!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput13B = "<script type=\"text/ecmascript\">" +
				"<!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput13C = "<script type=\"text/ecmascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input14 = "<script type=\"application/ecmascript\">\r\n" +
				"//<!-- Earlier versions of browsers ignore this code\r\n" +
				"\talert('Hooray!'); " +
				"\r\n// Hiding code stops here -->\r\n" +
				"</script>"
				;
			const string targetOutput14A = "<script type=\"application/ecmascript\">" +
				"<!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput14B = "<script type=\"application/ecmascript\">" +
				"<!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput14C = "<script type=\"application/ecmascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input15 = "<script type=\"application/javascript\">\r\n" +
				"// <!-- Earlier versions of browsers ignore this code\r\n" +
				"\talert('Hooray!'); " +
				"\r\n// Hiding code stops here -->\r\n" +
				"</script>"
				;
			const string targetOutput15A = "<script type=\"application/javascript\">" +
				"<!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput15B = "<script type=\"application/javascript\">" +
				"<!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput15C = "<script type=\"application/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input16 = "<script type=\"text/vbscript\">\r\n" +
				"<!--\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"-->\r\n" +
				"</script>"
				;
			const string targetOutput16A = "<script type=\"text/vbscript\">" +
				"<!--\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"-->" +
				"</script>"
				;
			const string targetOutput16B = "<script type=\"text/vbscript\">" +
				"<!--\r\n" +
				"Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"-->" +
				"</script>"
				;
			const string targetOutput16C = "<script type=\"text/vbscript\">" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function" +
				"</script>"
				;


			const string input17 = "<script language=\"VBScript\">\r\n" +
				"<!--\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"-->\r\n" +
				"</script>"
				;
			const string targetOutput17A = "<script language=\"VBScript\">" +
				"<!--\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"-->" +
				"</script>"
				;
			const string targetOutput17B = "<script language=\"VBScript\">" +
				"<!--\r\n" +
				"Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"-->" +
				"</script>"
				;
			const string targetOutput17C = "<script language=\"VBScript\">" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function" +
				"</script>"
				;

			// Act
			string output1A = minifier.Minify(input1).MinifiedContent;
			string output1B = removingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;

			string output2A = minifier.Minify(input2).MinifiedContent;
			string output2B = removingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			string output3A = minifier.Minify(input3).MinifiedContent;
			string output3B = removingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3C = removingHtmlCommentsMinifier.Minify(input3).MinifiedContent;

			string output4A = minifier.Minify(input4).MinifiedContent;
			string output4B = removingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4C = removingHtmlCommentsMinifier.Minify(input4).MinifiedContent;

			string output5A = minifier.Minify(input5).MinifiedContent;
			string output5B = removingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5C = removingHtmlCommentsMinifier.Minify(input5).MinifiedContent;

			string output6A = minifier.Minify(input6).MinifiedContent;
			string output6B = removingWhitespaceMinifier.Minify(input6).MinifiedContent;
			string output6C = removingHtmlCommentsMinifier.Minify(input6).MinifiedContent;

			string output7A = minifier.Minify(input7).MinifiedContent;
			string output7B = removingWhitespaceMinifier.Minify(input7).MinifiedContent;
			string output7C = removingHtmlCommentsMinifier.Minify(input7).MinifiedContent;

			string output8A = minifier.Minify(input8).MinifiedContent;
			string output8B = removingWhitespaceMinifier.Minify(input8).MinifiedContent;
			string output8C = removingHtmlCommentsMinifier.Minify(input8).MinifiedContent;

			string output9A = minifier.Minify(input9).MinifiedContent;
			string output9B = removingWhitespaceMinifier.Minify(input9).MinifiedContent;
			string output9C = removingHtmlCommentsMinifier.Minify(input9).MinifiedContent;

			string output10A = minifier.Minify(input10).MinifiedContent;
			string output10B = removingWhitespaceMinifier.Minify(input10).MinifiedContent;
			string output10C = removingHtmlCommentsMinifier.Minify(input10).MinifiedContent;

			string output11A = minifier.Minify(input11).MinifiedContent;
			string output11B = removingWhitespaceMinifier.Minify(input11).MinifiedContent;
			string output11C = removingHtmlCommentsMinifier.Minify(input11).MinifiedContent;

			string output12A = minifier.Minify(input12).MinifiedContent;
			string output12B = removingWhitespaceMinifier.Minify(input12).MinifiedContent;
			string output12C = removingHtmlCommentsMinifier.Minify(input12).MinifiedContent;

			string output13A = minifier.Minify(input13).MinifiedContent;
			string output13B = removingWhitespaceMinifier.Minify(input13).MinifiedContent;
			string output13C = removingHtmlCommentsMinifier.Minify(input13).MinifiedContent;

			string output14A = minifier.Minify(input14).MinifiedContent;
			string output14B = removingWhitespaceMinifier.Minify(input14).MinifiedContent;
			string output14C = removingHtmlCommentsMinifier.Minify(input14).MinifiedContent;

			string output15A = minifier.Minify(input15).MinifiedContent;
			string output15B = removingWhitespaceMinifier.Minify(input15).MinifiedContent;
			string output15C = removingHtmlCommentsMinifier.Minify(input15).MinifiedContent;

			string output16A = minifier.Minify(input16).MinifiedContent;
			string output16B = removingWhitespaceMinifier.Minify(input16).MinifiedContent;
			string output16C = removingHtmlCommentsMinifier.Minify(input16).MinifiedContent;

			string output17A = minifier.Minify(input17).MinifiedContent;
			string output17B = removingWhitespaceMinifier.Minify(input17).MinifiedContent;
			string output17C = removingHtmlCommentsMinifier.Minify(input17).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);

			Assert.Equal(targetOutput6A, output6A);
			Assert.Equal(targetOutput6B, output6B);
			Assert.Equal(targetOutput6C, output6C);

			Assert.Equal(targetOutput7A, output7A);
			Assert.Equal(targetOutput7B, output7B);
			Assert.Equal(targetOutput7C, output7C);

			Assert.Equal(targetOutput8A, output8A);
			Assert.Equal(targetOutput8B, output8B);
			Assert.Equal(targetOutput8C, output8C);

			Assert.Equal(targetOutput9A, output9A);
			Assert.Equal(targetOutput9B, output9B);
			Assert.Equal(targetOutput9C, output9C);

			Assert.Equal(targetOutput10A, output10A);
			Assert.Equal(targetOutput10B, output10B);
			Assert.Equal(targetOutput10C, output10C);

			Assert.Equal(targetOutput11A, output11A);
			Assert.Equal(targetOutput11B, output11B);
			Assert.Equal(targetOutput11C, output11C);

			Assert.Equal(targetOutput12A, output12A);
			Assert.Equal(targetOutput12B, output12B);
			Assert.Equal(targetOutput12C, output12C);

			Assert.Equal(targetOutput13A, output13A);
			Assert.Equal(targetOutput13B, output13B);
			Assert.Equal(targetOutput13C, output13C);

			Assert.Equal(targetOutput14A, output14A);
			Assert.Equal(targetOutput14B, output14B);
			Assert.Equal(targetOutput14C, output14C);

			Assert.Equal(targetOutput15A, output15A);
			Assert.Equal(targetOutput15B, output15B);
			Assert.Equal(targetOutput15C, output15C);

			Assert.Equal(targetOutput16A, output16A);
			Assert.Equal(targetOutput16B, output16B);
			Assert.Equal(targetOutput16C, output16C);

			Assert.Equal(targetOutput17A, output17A);
			Assert.Equal(targetOutput17B, output17B);
			Assert.Equal(targetOutput17C, output17C);
		}

		#endregion

		#region Processing CDATA sections in scripts and styles

		[Fact]
		public void ProcessingCdataSectionsInStylesIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));
			var removingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var removingCdataSectionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveCdataSectionsFromScriptsAndStyles = true });

			const string input1 = "<style type=\"text/css\">\r\n" +
				"/*<![CDATA[*/\r\n" +
				"\tp { color: #f00; } " +
				"\r\n/*]]>*/" +
				"\r\n</style>"
				;
			const string targetOutput1A = "<style type=\"text/css\">" +
				"/*<![CDATA[*/" +
				"\tp { color: #f00; } " +
				"/*]]>*/" +
				"</style>"
				;
			const string targetOutput1B = "<style type=\"text/css\">" +
				"/*<![CDATA[*/" +
				"p { color: #f00; }" +
				"/*]]>*/" +
				"</style>"
				;
			const string targetOutput1C = "<style type=\"text/css\">" +
				"\tp { color: #f00; } " +
				"</style>"
				;


			const string input2 = "<style type=\"text/css\">\r\n" +
				"/* <![CDATA[ */\r\n" +
				"\tp { color: #f00; } " +
				"\r\n/* ]]> */" +
				"\r\n</style>"
				;
			const string targetOutput2A = "<style type=\"text/css\">" +
				"/*<![CDATA[*/" +
				"\tp { color: #f00; } " +
				"/*]]>*/" +
				"</style>"
				;
			const string targetOutput2B = "<style type=\"text/css\">" +
				"/*<![CDATA[*/" +
				"p { color: #f00; }" +
				"/*]]>*/" +
				"</style>"
				;
			const string targetOutput2C = "<style type=\"text/css\">" +
				"\tp { color: #f00; } " +
				"</style>"
				;


			const string input3 = "<style type=\"text/css\">\r\n" +
				"<!--/*--><![CDATA[/*><!--*/\r\n" +
				"\tp { color: #f00; } " +
				"\r\n/*]]>*/-->" +
				"\r\n</style>"
				;
			const string targetOutput3A = "<style type=\"text/css\">" +
				"<!--/*--><![CDATA[/*><!--*/" +
				"\tp { color: #f00; } " +
				"/*]]>*/-->" +
				"</style>"
				;
			const string targetOutput3B = "<style type=\"text/css\">" +
				"<!--/*--><![CDATA[/*><!--*/" +
				"p { color: #f00; }" +
				"/*]]>*/-->" +
				"</style>"
				;
			const string targetOutput3C = "<style type=\"text/css\">" +
				"\tp { color: #f00; } " +
				"</style>"
				;


			const string input4 = "<style type=\"text/css\">\r\n" +
				"<!-- /* --><![CDATA[ /* ><!-- */\r\n" +
				"\tp { color: #f00; } " +
				"\r\n/* ]]> */ -->" +
				"\r\n</style>"
				;
			const string targetOutput4A = "<style type=\"text/css\">" +
				"<!--/*--><![CDATA[/*><!--*/" +
				"\tp { color: #f00; } " +
				"/*]]>*/-->" +
				"</style>"
				;
			const string targetOutput4B = "<style type=\"text/css\">" +
				"<!--/*--><![CDATA[/*><!--*/" +
				"p { color: #f00; }" +
				"/*]]>*/-->" +
				"</style>"
				;
			const string targetOutput4C = "<style type=\"text/css\">" +
				"\tp { color: #f00; } " +
				"</style>"
				;


			const string input5 = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 100\">\r\n" +
				"	<style>\r\n" +
				"	/* <![CDATA[ */\r\n" +
				"	circle {\r\n" +
				"		fill: orange;\r\n" +
				"		stroke: black;\r\n" +
				"		stroke-width: 10px; // Note that the value of a pixel depend on the viewBox\r\n" +
				"	}\r\n" +
				"	/* ]]> */\r\n" +
				"	</style>\r\n" +
				"	<circle cx=\"50\" cy=\"50\" r=\"40\"/>\r\n" +
				"</svg>"
				;
			const string targetOutput5A = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 100\">\r\n" +
				"	<style>" +
				"/*<![CDATA[*/" +
				"	circle {\r\n" +
				"		fill: orange;\r\n" +
				"		stroke: black;\r\n" +
				"		stroke-width: 10px; // Note that the value of a pixel depend on the viewBox\r\n" +
				"	}" +
				"/*]]>*/" +
				"</style>\r\n" +
				"	<circle cx=\"50\" cy=\"50\" r=\"40\" />\r\n" +
				"</svg>"
				;
			const string targetOutput5B = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 100\">" +
				"<style>" +
				"/*<![CDATA[*/" +
				"circle {\r\n" +
				"		fill: orange;\r\n" +
				"		stroke: black;\r\n" +
				"		stroke-width: 10px; // Note that the value of a pixel depend on the viewBox\r\n" +
				"	}" +
				"/*]]>*/" +
				"</style>" +
				"<circle cx=\"50\" cy=\"50\" r=\"40\" />" +
				"</svg>"
				;
			const string targetOutput5C = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 100\">\r\n" +
				"	<style>" +
				"	circle {\r\n" +
				"		fill: orange;\r\n" +
				"		stroke: black;\r\n" +
				"		stroke-width: 10px; // Note that the value of a pixel depend on the viewBox\r\n" +
				"	}" +
				"</style>\r\n" +
				"	<circle cx=\"50\" cy=\"50\" r=\"40\" />\r\n" +
				"</svg>"
				;


			const string input6 = "<svg version=\"1.1\" width=\"10cm\" height=\"5cm\" viewBox=\"0 0 1000 500\">\r\n" +
				"	<defs>\r\n" +
				"		<style type=\"text/css\">\r\n" +
				"<![CDATA[" +
				"		rect {\r\n" +
				"			fill: red;\r\n" +
				"			stroke: blue;\r\n" +
				"			stroke-width: 3;\r\n" +
				"		}\r\n" +
				"]]>" +
				"</style>\r\n" +
				"	</defs>\r\n" +
				"	<rect x=\"200\" y=\"100\" width=\"600\" height=\"300\" />\r\n" +
				"</svg>"
				;
			const string targetOutput6A = "<svg version=\"1.1\" width=\"10cm\" height=\"5cm\" viewBox=\"0 0 1000 500\">\r\n" +
				"	<defs>\r\n" +
				"		<style type=\"text/css\">" +
				"<![CDATA[\r\n" +
				"		rect {\r\n" +
				"			fill: red;\r\n" +
				"			stroke: blue;\r\n" +
				"			stroke-width: 3;\r\n" +
				"		}\r\n" +
				"]]>" +
				"</style>\r\n" +
				"	</defs>\r\n" +
				"	<rect x=\"200\" y=\"100\" width=\"600\" height=\"300\" />\r\n" +
				"</svg>"
				;
			const string targetOutput6B = "<svg version=\"1.1\" width=\"10cm\" height=\"5cm\" viewBox=\"0 0 1000 500\">" +
				"<defs>" +
				"<style type=\"text/css\">" +
				"<![CDATA[\r\n" +
				"rect {\r\n" +
				"			fill: red;\r\n" +
				"			stroke: blue;\r\n" +
				"			stroke-width: 3;\r\n" +
				"		}\r\n" +
				"]]>" +
				"</style>" +
				"</defs>" +
				"<rect x=\"200\" y=\"100\" width=\"600\" height=\"300\" />" +
				"</svg>"
				;
			const string targetOutput6C = targetOutput6A;

			// Act
			string output1A = minifier.Minify(input1).MinifiedContent;
			string output1B = removingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = removingCdataSectionsMinifier.Minify(input1).MinifiedContent;

			string output2A = minifier.Minify(input2).MinifiedContent;
			string output2B = removingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = removingCdataSectionsMinifier.Minify(input2).MinifiedContent;

			string output3A = minifier.Minify(input3).MinifiedContent;
			string output3B = removingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3C = removingCdataSectionsMinifier.Minify(input3).MinifiedContent;

			string output4A = minifier.Minify(input4).MinifiedContent;
			string output4B = removingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4C = removingCdataSectionsMinifier.Minify(input4).MinifiedContent;

			string output5A = minifier.Minify(input5).MinifiedContent;
			string output5B = removingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5C = removingCdataSectionsMinifier.Minify(input5).MinifiedContent;

			string output6A = minifier.Minify(input6).MinifiedContent;
			string output6B = removingWhitespaceMinifier.Minify(input6).MinifiedContent;
			string output6C = removingCdataSectionsMinifier.Minify(input6).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);

			Assert.Equal(targetOutput6A, output6A);
			Assert.Equal(targetOutput6B, output6B);
			Assert.Equal(targetOutput6C, output6C);
		}

		[Fact]
		public void ProcessingCdataSectionsInScriptsIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));
			var removingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var removingCdataSectionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveCdataSectionsFromScriptsAndStyles = true });

			const string input1 = "<script type=\"text/javascript\">\r\n" +
				"//<![CDATA[\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//]]>" +
				"\r\n</script>"
				;
			const string targetOutput1A = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput1B = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"alert('Hooray!');" +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput1C = "<script type=\"text/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input2 = "<script type=\"text/javascript\">\r\n" +
				"// <![CDATA[\r\n" +
				"\talert('Hooray!'); " +
				"\r\n// ]]>" +
				"\r\n</script>"
				;
			const string targetOutput2A = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput2B = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"alert('Hooray!');" +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput2C = "<script type=\"text/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input3 = "<script type=\"text/javascript\">\r\n" +
				"// <![CDATA[ alert('Test 1'); \r\n" +
				"\talert('Test 2'); " +
				"\r\n//\talert('Test 3'); ]]>" +
				"\r\n</script>"
				;
			const string targetOutput3A = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"\talert('Test 2'); " +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput3B = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"alert('Test 2');" +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput3C = "<script type=\"text/javascript\">" +
				"\talert('Test 2'); " +
				"</script>"
				;


			const string input4 = "<script type=\"text/javascript\">\r\n" +
				"/*<![CDATA[*/\r\n" +
				"\talert('Hooray!'); " +
				"\r\n/*]]>*" +
				"/\r\n</script>"
				;
			const string targetOutput4A = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput4B = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"alert('Hooray!');" +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput4C = "<script type=\"text/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input5 = "<script type=\"text/javascript\">\r\n" +
				"/* <![CDATA[ */\r\n" +
				"\talert('Hooray!'); " +
				"\r\n/* ]]> */" +
				"\r\n</script>"
				;
			const string targetOutput5A = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput5B = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"alert('Hooray!');" +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput5C = "<script type=\"text/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input6 = "<script type=\"text/javascript\">\r\n" +
				"<!--//--><![CDATA[//><!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//--><!]]>" +
				"\r\n</script>"
				;
			const string targetOutput6A = "<script type=\"text/javascript\">" +
				"<!--//--><![CDATA[//><!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//--><!]]>" +
				"</script>"
				;
			const string targetOutput6B = "<script type=\"text/javascript\">" +
				"<!--//--><![CDATA[//><!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//--><!]]>" +
				"</script>"
				;
			const string targetOutput6C = "<script type=\"text/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input7 = "<script type=\"text/ecmascript\">\r\n" +
				"<!-- // --><![CDATA[ // ><!-- \r\n" +
				"\talert('Hooray!'); " +
				"\r\n // --><!]]>" +
				"\r\n</script>"
				;
			const string targetOutput7A = "<script type=\"text/ecmascript\">" +
				"<!--//--><![CDATA[//><!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//--><!]]>" +
				"</script>"
				;
			const string targetOutput7B = "<script type=\"text/ecmascript\">" +
				"<!--//--><![CDATA[//><!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//--><!]]>" +
				"</script>"
				;
			const string targetOutput7C = "<script type=\"text/ecmascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input8 = "<script type=\"application/ecmascript\">\r\n" +
				"<!--/*--><![CDATA[/*><!--*/\r\n" +
				"\talert('Hooray!'); " +
				"\r\n/*]]>*/-->" +
				"\r\n</script>"
				;
			const string targetOutput8A = "<script type=\"application/ecmascript\">" +
				"<!--//--><![CDATA[//><!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//--><!]]>" +
				"</script>"
				;
			const string targetOutput8B = "<script type=\"application/ecmascript\">" +
				"<!--//--><![CDATA[//><!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//--><!]]>" +
				"</script>"
				;
			const string targetOutput8C = "<script type=\"application/ecmascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input9 = "<script type=\"application/javascript\">\r\n" +
				"<!-- /* --><![CDATA[ /* ><!-- */ \r\n" +
				"\talert('Hooray!'); " +
				"\r\n/* ]]> */ -->" +
				"\r\n</script>"
				;
			const string targetOutput9A = "<script type=\"application/javascript\">" +
				"<!--//--><![CDATA[//><!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//--><!]]>" +
				"</script>"
				;
			const string targetOutput9B = "<script type=\"application/javascript\">" +
				"<!--//--><![CDATA[//><!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//--><!]]>" +
				"</script>"
				;
			const string targetOutput9C = "<script type=\"application/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input10 = "<script type=\"text/javascript\">\r\n" +
				"	<![CDATA[\r\n" +
				"	// is the variable a non-negative integer less than 15?\r\n" +
				"	if (variable < 15 && variable >= 0) {\r\n" +
				"		doSomething();\r\n" +
				"	}\r\n" +
				"	]]>\r\n" +
				"</script>"
				;
			const string targetOutput10A = "<script type=\"text/javascript\">" +
				"<![CDATA[\r\n" +
				"	// is the variable a non-negative integer less than 15?\r\n" +
				"	if (variable < 15 && variable >= 0) {\r\n" +
				"		doSomething();\r\n" +
				"	}\r\n" +
				"]]>" +
				"</script>"
				;
			const string targetOutput10B = "<script type=\"text/javascript\">" +
				"<![CDATA[\r\n" +
				"// is the variable a non-negative integer less than 15?\r\n" +
				"	if (variable < 15 && variable >= 0) {\r\n" +
				"		doSomething();\r\n" +
				"	}\r\n" +
				"]]>" +
				"</script>"
				;
			const string targetOutput10C = "<script type=\"text/javascript\">" +
				"<![CDATA[\r\n" +
				"	// is the variable a non-negative integer less than 15?\r\n" +
				"	if (variable < 15 && variable >= 0) {\r\n" +
				"		doSomething();\r\n" +
				"	}\r\n" +
				"]]>" +
				"</script>"
				;


			const string input11 = "<script type=\"text/vbscript\">\r\n" +
				"<![CDATA[\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"]]>\r\n" +
				"</script>"
				;
			const string targetOutput11A = "<script type=\"text/vbscript\">" +
				"<![CDATA[\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"]]>" +
				"</script>"
				;
			const string targetOutput11B = "<script type=\"text/vbscript\">" +
				"<![CDATA[\r\n" +
				"Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"]]>" +
				"</script>"
				;
			const string targetOutput11C = targetOutput11A;


			const string input12 = "<script language=\"VBScript\">\r\n" +
				"<![CDATA[\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"]]>\r\n" +
				"</script>"
				;
			const string targetOutput12A = "<script language=\"VBScript\">" +
				"<![CDATA[\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"]]>" +
				"</script>"
				;
			const string targetOutput12B = "<script language=\"VBScript\">" +
				"<![CDATA[\r\n" +
				"Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"]]>" +
				"</script>"
				;
			const string targetOutput12C = targetOutput12A;

			// Act
			string output1A = minifier.Minify(input1).MinifiedContent;
			string output1B = removingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = removingCdataSectionsMinifier.Minify(input1).MinifiedContent;

			string output2A = minifier.Minify(input2).MinifiedContent;
			string output2B = removingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = removingCdataSectionsMinifier.Minify(input2).MinifiedContent;

			string output3A = minifier.Minify(input3).MinifiedContent;
			string output3B = removingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3C = removingCdataSectionsMinifier.Minify(input3).MinifiedContent;

			string output4A = minifier.Minify(input4).MinifiedContent;
			string output4B = removingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4C = removingCdataSectionsMinifier.Minify(input4).MinifiedContent;

			string output5A = minifier.Minify(input5).MinifiedContent;
			string output5B = removingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5C = removingCdataSectionsMinifier.Minify(input5).MinifiedContent;

			string output6A = minifier.Minify(input6).MinifiedContent;
			string output6B = removingWhitespaceMinifier.Minify(input6).MinifiedContent;
			string output6C = removingCdataSectionsMinifier.Minify(input6).MinifiedContent;

			string output7A = minifier.Minify(input7).MinifiedContent;
			string output7B = removingWhitespaceMinifier.Minify(input7).MinifiedContent;
			string output7C = removingCdataSectionsMinifier.Minify(input7).MinifiedContent;

			string output8A = minifier.Minify(input8).MinifiedContent;
			string output8B = removingWhitespaceMinifier.Minify(input8).MinifiedContent;
			string output8C = removingCdataSectionsMinifier.Minify(input8).MinifiedContent;

			string output9A = minifier.Minify(input9).MinifiedContent;
			string output9B = removingWhitespaceMinifier.Minify(input9).MinifiedContent;
			string output9C = removingCdataSectionsMinifier.Minify(input9).MinifiedContent;

			string output10A = minifier.Minify(input10).MinifiedContent;
			string output10B = removingWhitespaceMinifier.Minify(input10).MinifiedContent;
			string output10C = removingCdataSectionsMinifier.Minify(input10).MinifiedContent;

			string output11A = minifier.Minify(input11).MinifiedContent;
			string output11B = removingWhitespaceMinifier.Minify(input11).MinifiedContent;
			string output11C = removingCdataSectionsMinifier.Minify(input11).MinifiedContent;

			string output12A = minifier.Minify(input12).MinifiedContent;
			string output12B = removingWhitespaceMinifier.Minify(input12).MinifiedContent;
			string output12C = removingCdataSectionsMinifier.Minify(input12).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);

			Assert.Equal(targetOutput6A, output6A);
			Assert.Equal(targetOutput6B, output6B);
			Assert.Equal(targetOutput6C, output6C);

			Assert.Equal(targetOutput7A, output7A);
			Assert.Equal(targetOutput7B, output7B);
			Assert.Equal(targetOutput7C, output7C);

			Assert.Equal(targetOutput8A, output8A);
			Assert.Equal(targetOutput8B, output8B);
			Assert.Equal(targetOutput8C, output8C);

			Assert.Equal(targetOutput9A, output9A);
			Assert.Equal(targetOutput9B, output9B);
			Assert.Equal(targetOutput9C, output9C);

			Assert.Equal(targetOutput10A, output10A);
			Assert.Equal(targetOutput10B, output10B);
			Assert.Equal(targetOutput10C, output10C);

			Assert.Equal(targetOutput11A, output11A);
			Assert.Equal(targetOutput11B, output11B);
			Assert.Equal(targetOutput11C, output11C);

			Assert.Equal(targetOutput12A, output12A);
			Assert.Equal(targetOutput12B, output12B);
			Assert.Equal(targetOutput12C, output12C);
		}

		#endregion

		#region Empty tag rendering

		[Fact]
		public void EmptyTagRenderingIsCorrect()
		{
			// Arrange
			var emptyTagWithoutSlashMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { EmptyTagRenderMode = HtmlEmptyTagRenderMode.NoSlash });
			var emptyTagWithSlashMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { EmptyTagRenderMode = HtmlEmptyTagRenderMode.Slash });
			var emptyTagWithSpaceAndSlashMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { EmptyTagRenderMode = HtmlEmptyTagRenderMode.SpaceAndSlash });

			const string input1 = "<img src=\"/images/0.gif\">";
			const string targetOutput1A = "<img src=\"/images/0.gif\">";
			const string targetOutput1B = "<img src=\"/images/0.gif\"/>";
			const string targetOutput1C = "<img src=\"/images/0.gif\" />";

			const string input2 = "<br/>";
			const string targetOutput2A = "<br>";
			const string targetOutput2B = "<br/>";
			const string targetOutput2C = "<br />";

			const string input3 = "<hr />";
			const string targetOutput3A = "<hr>";
			const string targetOutput3B = "<hr/>";
			const string targetOutput3C = "<hr />";

			const string input4 = "<svg width=\"220\" height=\"220\">" +
				"<circle cx=\"110\" cy=\"110\" r=\"100\" fill=\"#fafaa2\" stroke=\"#000\" />" +
				"<rect x=\"10\" y=\"10\" width=\"200\" height=\"200\" fill=\"#fafaa2\" stroke=\"#000\"/>" +
				"</svg>"
				;
			const string targetOutput4A = "<svg width=\"220\" height=\"220\">" +
				"<circle cx=\"110\" cy=\"110\" r=\"100\" fill=\"#fafaa2\" stroke=\"#000\" />" +
				"<rect x=\"10\" y=\"10\" width=\"200\" height=\"200\" fill=\"#fafaa2\" stroke=\"#000\" />" +
				"</svg>"
				;
			const string targetOutput4B = "<svg width=\"220\" height=\"220\">" +
				"<circle cx=\"110\" cy=\"110\" r=\"100\" fill=\"#fafaa2\" stroke=\"#000\"/>" +
				"<rect x=\"10\" y=\"10\" width=\"200\" height=\"200\" fill=\"#fafaa2\" stroke=\"#000\"/>" +
				"</svg>"
				;
			const string targetOutput4C = targetOutput4A;

			const string input5 = "<div>" +
				"<math>" +
				"<apply>" +
				"<plus/>" +
				"<apply>" +
				"<times />" +
				"<ci>a</ci>" +
				"<apply>" +
				"<power/>" +
				"<ci>x</ci>" +
				"<cn>2</cn>" +
				"</apply>" +
				"</apply>" +
				"<apply>" +
				"<times />" +
				"<ci>b</ci>" +
				"<ci>x</ci>" +
				"</apply>" +
				"<ci>c</ci>" +
				"</apply>" +
				"</math>" +
				"</div>"
				;
			const string targetOutput5A = "<div>" +
				"<math>" +
				"<apply>" +
				"<plus />" +
				"<apply>" +
				"<times />" +
				"<ci>a</ci>" +
				"<apply>" +
				"<power />" +
				"<ci>x</ci>" +
				"<cn>2</cn>" +
				"</apply>" +
				"</apply>" +
				"<apply>" +
				"<times />" +
				"<ci>b</ci>" +
				"<ci>x</ci>" +
				"</apply>" +
				"<ci>c</ci>" +
				"</apply>" +
				"</math>" +
				"</div>"
				;
			const string targetOutput5B = "<div>" +
				"<math>" +
				"<apply>" +
				"<plus/>" +
				"<apply>" +
				"<times/>" +
				"<ci>a</ci>" +
				"<apply>" +
				"<power/>" +
				"<ci>x</ci>" +
				"<cn>2</cn>" +
				"</apply>" +
				"</apply>" +
				"<apply>" +
				"<times/>" +
				"<ci>b</ci>" +
				"<ci>x</ci>" +
				"</apply>" +
				"<ci>c</ci>" +
				"</apply>" +
				"</math>" +
				"</div>"
				;
			const string targetOutput5C = targetOutput5A;

			// Act
			string output1A = emptyTagWithoutSlashMinifier.Minify(input1).MinifiedContent;
			string output1B = emptyTagWithSlashMinifier.Minify(input1).MinifiedContent;
			string output1C = emptyTagWithSpaceAndSlashMinifier.Minify(input1).MinifiedContent;

			string output2A = emptyTagWithoutSlashMinifier.Minify(input2).MinifiedContent;
			string output2B = emptyTagWithSlashMinifier.Minify(input2).MinifiedContent;
			string output2C = emptyTagWithSpaceAndSlashMinifier.Minify(input2).MinifiedContent;

			string output3A = emptyTagWithoutSlashMinifier.Minify(input3).MinifiedContent;
			string output3B = emptyTagWithSlashMinifier.Minify(input3).MinifiedContent;
			string output3C = emptyTagWithSpaceAndSlashMinifier.Minify(input3).MinifiedContent;

			string output4A = emptyTagWithoutSlashMinifier.Minify(input4).MinifiedContent;
			string output4B = emptyTagWithSlashMinifier.Minify(input4).MinifiedContent;
			string output4C = emptyTagWithSpaceAndSlashMinifier.Minify(input4).MinifiedContent;

			string output5A = emptyTagWithoutSlashMinifier.Minify(input5).MinifiedContent;
			string output5B = emptyTagWithSlashMinifier.Minify(input5).MinifiedContent;
			string output5C = emptyTagWithSpaceAndSlashMinifier.Minify(input5).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);
		}

		#endregion

		#region Removing optional end tags

		[Fact]
		public void RemovingStructuralOptionalEndTagsIsCorrect()
		{
			// Arrange
			var keepingOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveOptionalEndTags = false });
			var removingOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveOptionalEndTags = true });
			var keepingTopLevelOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					RemoveOptionalEndTags = true,
					PreservableOptionalTagList = "html,head,body"
				}
			);

			const string input1 = "<html>\n" +
				"	<head>\n" +
				"		<title>Some title…</title>\n" +
				"	</head>\n" +
				"	<body>\n" +
				"		<div><strong>Welcome!</strong></div>\n" +
				"	</body>\n" +
				"</html>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<html>\n" +
				"	<head>\n" +
				"		<title>Some title…</title>\n" +
				"	\n" +
				"	<body>\n" +
				"		<div><strong>Welcome!</strong></div>\n" +
				"	\n"
				;
			const string targetOutput1C = input1;

			// Act
			string output1A = keepingOptionalEndTagsMinifier.Minify(input1).MinifiedContent;
			string output1B = removingOptionalEndTagsMinifier.Minify(input1).MinifiedContent;
			string output1C = keepingTopLevelOptionalEndTagsMinifier.Minify(input1).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);
		}

		[Fact]
		public void RemovingTypographicalOptionalEndTags()
		{
			// Arrange
			var keepingOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveOptionalEndTags = false });
			var removingOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveOptionalEndTags = true });
			var keepingUnsafeOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					RemoveOptionalEndTags = true,
					PreservableOptionalTagList = "li"
				}
			);

			const string input1 = "<form>" +
				"<p>Some text 1…</p>" +
				"<p>Some text 2…</p>" +
				"<textarea name=\"txtBody\" rows=\"8\" cols=\"40\"></textarea>" +
				"<br>" +
				"<button type=\"button\">Save</button>" +
				"<p>Some text 3…</p>" +
				"<p>Some text 4…</p>" +
				"<div>Some content…</div>" +
				"<p>Some text 5…</p>" +
				"</form>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<form>" +
				"<p>Some text 1…" +
				"<p>Some text 2…</p>" +
				"<textarea name=\"txtBody\" rows=\"8\" cols=\"40\"></textarea>" +
				"<br>" +
				"<button type=\"button\">Save</button>" +
				"<p>Some text 3…" +
				"<p>Some text 4…" +
				"<div>Some content…</div>" +
				"<p>Some text 5…" +
				"</form>"
				;
			const string targetOutput1C = targetOutput1B;


			const string input2 = "<div>\n" +
				"	<p>Some text…</p>\n" +
				"</div>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = "<div>\n" +
				"	<p>Some text…\n" +
				"</div>"
				;
			const string targetOutput2C = targetOutput2B;


			const string input3 = "<div>\n" +
				"	<p>Some text…</p>\n" +
				"	Some other text…\n" +
				"</div>"
				;


			const string input4 = "<a href=\"http://www.example.com/\">\n" +
				"	<p>Some text…</p>\n" +
				"</a>"
				;


			const string input5 = "<ul>\n" +
				"	<li>Item 1</li>\n" +
				"	<li>Item 2\n" +
				"		<ul>\n" +
				"			<li>Item 21</li>\n" +
				"			<li>Item 22</li>\n" +
				"		</ul>\n" +
				"	</li>\n" +
				"	<li>Item 3</li>\n" +
				"</ul>"
				;
			const string targetOutput5A = input5;
			const string targetOutput5B = "<ul>\n" +
				"	<li>Item 1\n" +
				"	<li>Item 2\n" +
				"		<ul>\n" +
				"			<li>Item 21\n" +
				"			<li>Item 22\n" +
				"		</ul>\n" +
				"	\n" +
				"	<li>Item 3\n" +
				"</ul>"
				;
			const string targetOutput5C = input5;


			const string input6 = "<dl>\n" +
				"	<dt>CoffeeScript</dt>\n" +
				"	<dd>Little language that compiles into JavaScript</dd>\n" +
				"	<dt>TypeScript</dt>\n" +
				"	<dd>Typed superset of JavaScript that compiles to plain JavaScript</dd>\n" +
				"</dl>"
				;
			const string targetOutput6A = input6;
			const string targetOutput6B = "<dl>\n" +
				"	<dt>CoffeeScript\n" +
				"	<dd>Little language that compiles into JavaScript\n" +
				"	<dt>TypeScript\n" +
				"	<dd>Typed superset of JavaScript that compiles to plain JavaScript\n" +
				"</dl>"
				;
			const string targetOutput6C = targetOutput6B;


			const string input7 = "<dl>\n" +
				"	<dt>LESS</dt>\n" +
				"	<dd><img src=\"/images/less-logo.png\" width=\"199\" height=\"81\" alt=\"LESS logo\"></dd>\n" +
				"	<dd>The dynamic stylesheet language</dd>\n" +
				"	<dt>Sass</dt>\n" +
				"	<dd><img src=\"/images/sass-logo.gif\" width=\"217\" height=\"238\" alt=\"Sass logo\"></dd>\n" +
				"	<dd>Extension of CSS3, adding nested rules, variables, mixins, selector inheritance, and more</dd>\n" +
				"</dl>"
				;
			const string targetOutput7A = input7;
			const string targetOutput7B = "<dl>\n" +
				"	<dt>LESS\n" +
				"	<dd><img src=\"/images/less-logo.png\" width=\"199\" height=\"81\" alt=\"LESS logo\">\n" +
				"	<dd>The dynamic stylesheet language\n" +
				"	<dt>Sass\n" +
				"	<dd><img src=\"/images/sass-logo.gif\" width=\"217\" height=\"238\" alt=\"Sass logo\">\n" +
				"	<dd>Extension of CSS3, adding nested rules, variables, mixins, selector inheritance, and more\n" +
				"</dl>"
				;
			const string targetOutput7C = targetOutput7B;


			const string input8 = "<ruby>\n" +
				"	攻殻\n" +
				"	<rt>こうかく</rt>\n" +
				"	機動隊\n" +
				"	<rt>きどうたい</rt>\n" +
				"</ruby>"
				;
			const string targetOutput8A = input8;
			const string targetOutput8B = "<ruby>\n" +
				"	攻殻\n" +
				"	<rt>こうかく</rt>\n" +
				"	機動隊\n" +
				"	<rt>きどうたい\n" +
				"</ruby>"
				;
			const string targetOutput8C = targetOutput8B;


			const string input9 = "<ruby>\n" +
				"	攻殻\n" +
				"	<rp>（</rp>\n" +
				"	<rt>こうかく</rt>\n" +
				"	<rp>）</rp>\n" +
				"	機動隊\n" +
				"	<rp>（</rp>\n" +
				"	<rt>きどうたい</rt>\n" +
				"	<rp>）</rp>\n" +
				"</ruby>"
				;
			const string targetOutput9A = input9;
			const string targetOutput9B = "<ruby>\n" +
				"	攻殻\n" +
				"	<rp>（\n" +
				"	<rt>こうかく\n" +
				"	<rp>）</rp>\n" +
				"	機動隊\n" +
				"	<rp>（\n" +
				"	<rt>きどうたい\n" +
				"	<rp>）\n" +
				"</ruby>"
				;
			const string targetOutput9C = targetOutput9B;


			const string input10 = "<ruby>\n" +
				"	<ruby>\n" +
				"		攻\n" +
				"		<rp>（</rp>\n" +
				"		<rt>こう</rt>\n" +
				"		<rp>）</rp>\n" +
				"		殻\n" +
				"		<rp>（</rp>\n" +
				"		<rt>かく</rt>\n" +
				"		<rp>）</rp>\n" +
				"		機\n" +
				"		<rp>（</rp>\n" +
				"		<rt>き</rt>\n" +
				"		<rp>）</rp>\n" +
				"		動\n" +
				"		<rp>（</rp>\n" +
				"		<rt>どう</rt>\n" +
				"		<rp>）</rp>\n" +
				"		隊\n" +
				"		<rp>（</rp>\n" +
				"		<rt>たい</rt>\n" +
				"		<rp>）</rp>\n" +
				"	</ruby>\n" +
				"	<rp>（</rp>\n" +
				"	<rt>Kōkakukidōtai</rt>\n" +
				"	<rp>）</rp>\n" +
				"</ruby>"
				;
			const string targetOutput10A = input10;
			const string targetOutput10B = "<ruby>\n" +
				"	<ruby>\n" +
				"		攻\n" +
				"		<rp>（\n" +
				"		<rt>こう\n" +
				"		<rp>）</rp>\n" +
				"		殻\n" +
				"		<rp>（\n" +
				"		<rt>かく\n" +
				"		<rp>）</rp>\n" +
				"		機\n" +
				"		<rp>（\n" +
				"		<rt>き\n" +
				"		<rp>）</rp>\n" +
				"		動\n" +
				"		<rp>（\n" +
				"		<rt>どう\n" +
				"		<rp>）</rp>\n" +
				"		隊\n" +
				"		<rp>（\n" +
				"		<rt>たい\n" +
				"		<rp>）\n" +
				"	</ruby>\n" +
				"	<rp>（\n" +
				"	<rt>Kōkakukidōtai\n" +
				"	<rp>）\n" +
				"</ruby>"
				;
			const string targetOutput10C = targetOutput10B;


			const string input11 = "<ruby>\n" +
				"	<rb>家辺 勝文</rb>\n" +
				"	<rt>liaison</rt>\n" +
				"</ruby>"
				;
			const string targetOutput11A = input11;
			const string targetOutput11B = "<ruby>\n" +
				"	<rb>家辺 勝文\n" +
				"	<rt>liaison\n" +
				"</ruby>"
				;
			const string targetOutput11C = targetOutput11B;


			const string input12 = "<ruby>\n" +
				"	♥<rp>: </rp><rt>Heart</rt><rp>, </rp><rtc><rt lang=\"fr\">Cœur</rt></rtc><rp>.</rp>\n" +
				"	☘<rp>: </rp><rt>Shamrock</rt><rp>, </rp><rtc><rt lang=\"fr\">Trèfle</rt></rtc><rp>.</rp>\n" +
				"	✶<rp>: </rp><rt>Star</rt><rp>, </rp><rtc><rt lang=\"fr\">Étoile</rt></rtc><rp>.</rp>\n" +
				"</ruby>"
				;
			const string targetOutput12A = input12;
			const string targetOutput12B = "<ruby>\n" +
				"	♥<rp>: <rt>Heart<rp>, <rtc><rt lang=\"fr\">Cœur<rp>.</rp>\n" +
				"	☘<rp>: <rt>Shamrock<rp>, <rtc><rt lang=\"fr\">Trèfle<rp>.</rp>\n" +
				"	✶<rp>: <rt>Star<rp>, <rtc><rt lang=\"fr\">Étoile<rp>.\n" +
				"</ruby>"
				;
			const string targetOutput12C = targetOutput12B;


			const string input13 = "<ruby>\n" +
				"	<rb>旧</rb>\n" +
				"	<rb>金</rb>\n" +
				"	<rb>山</rb>\n" +
				"	<rt>jiù</rt>\n" +
				"	<rt>jīn</rt>\n" +
				"	<rt>shān</rt>\n" +
				"	<rtc>San Francisco</rtc>\n" +
				"</ruby>"
				;
			const string targetOutput13A = input13;
			const string targetOutput13B = "<ruby>\n" +
				"	<rb>旧\n" +
				"	<rb>金\n" +
				"	<rb>山\n" +
				"	<rt>jiù\n" +
				"	<rt>jīn\n" +
				"	<rt>shān\n" +
				"	<rtc>San Francisco\n" +
				"</ruby>"
				;
			const string targetOutput13C = targetOutput13B;

			// Act
			string output1A = keepingOptionalEndTagsMinifier.Minify(input1).MinifiedContent;
			string output1B = removingOptionalEndTagsMinifier.Minify(input1).MinifiedContent;
			string output1C = keepingUnsafeOptionalEndTagsMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingOptionalEndTagsMinifier.Minify(input2).MinifiedContent;
			string output2B = removingOptionalEndTagsMinifier.Minify(input2).MinifiedContent;
			string output2C = keepingUnsafeOptionalEndTagsMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingOptionalEndTagsMinifier.Minify(input3).MinifiedContent;
			string output3B = removingOptionalEndTagsMinifier.Minify(input3).MinifiedContent;
			string output3C = keepingUnsafeOptionalEndTagsMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingOptionalEndTagsMinifier.Minify(input4).MinifiedContent;
			string output4B = removingOptionalEndTagsMinifier.Minify(input4).MinifiedContent;
			string output4C = keepingUnsafeOptionalEndTagsMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingOptionalEndTagsMinifier.Minify(input5).MinifiedContent;
			string output5B = removingOptionalEndTagsMinifier.Minify(input5).MinifiedContent;
			string output5C = keepingUnsafeOptionalEndTagsMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingOptionalEndTagsMinifier.Minify(input6).MinifiedContent;
			string output6B = removingOptionalEndTagsMinifier.Minify(input6).MinifiedContent;
			string output6C = keepingUnsafeOptionalEndTagsMinifier.Minify(input6).MinifiedContent;

			string output7A = keepingOptionalEndTagsMinifier.Minify(input7).MinifiedContent;
			string output7B = removingOptionalEndTagsMinifier.Minify(input7).MinifiedContent;
			string output7C = keepingUnsafeOptionalEndTagsMinifier.Minify(input7).MinifiedContent;

			string output8A = keepingOptionalEndTagsMinifier.Minify(input8).MinifiedContent;
			string output8B = removingOptionalEndTagsMinifier.Minify(input8).MinifiedContent;
			string output8C = keepingUnsafeOptionalEndTagsMinifier.Minify(input8).MinifiedContent;

			string output9A = keepingOptionalEndTagsMinifier.Minify(input9).MinifiedContent;
			string output9B = removingOptionalEndTagsMinifier.Minify(input9).MinifiedContent;
			string output9C = keepingUnsafeOptionalEndTagsMinifier.Minify(input9).MinifiedContent;

			string output10A = keepingOptionalEndTagsMinifier.Minify(input10).MinifiedContent;
			string output10B = removingOptionalEndTagsMinifier.Minify(input10).MinifiedContent;
			string output10C = keepingUnsafeOptionalEndTagsMinifier.Minify(input10).MinifiedContent;

			string output11A = keepingOptionalEndTagsMinifier.Minify(input11).MinifiedContent;
			string output11B = removingOptionalEndTagsMinifier.Minify(input11).MinifiedContent;
			string output11C = keepingUnsafeOptionalEndTagsMinifier.Minify(input11).MinifiedContent;

			string output12A = keepingOptionalEndTagsMinifier.Minify(input12).MinifiedContent;
			string output12B = removingOptionalEndTagsMinifier.Minify(input12).MinifiedContent;
			string output12C = keepingUnsafeOptionalEndTagsMinifier.Minify(input12).MinifiedContent;

			string output13A = keepingOptionalEndTagsMinifier.Minify(input13).MinifiedContent;
			string output13B = removingOptionalEndTagsMinifier.Minify(input13).MinifiedContent;
			string output13C = keepingUnsafeOptionalEndTagsMinifier.Minify(input13).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(input3, output3A);
			Assert.Equal(input3, output3B);
			Assert.Equal(input3, output3C);

			Assert.Equal(input4, output4A);
			Assert.Equal(input4, output4B);
			Assert.Equal(input4, output4C);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);

			Assert.Equal(targetOutput6A, output6A);
			Assert.Equal(targetOutput6B, output6B);
			Assert.Equal(targetOutput6C, output6C);

			Assert.Equal(targetOutput7A, output7A);
			Assert.Equal(targetOutput7B, output7B);
			Assert.Equal(targetOutput7C, output7C);

			Assert.Equal(targetOutput8A, output8A);
			Assert.Equal(targetOutput8B, output8B);
			Assert.Equal(targetOutput8C, output8C);

			Assert.Equal(targetOutput9A, output9A);
			Assert.Equal(targetOutput9B, output9B);
			Assert.Equal(targetOutput9C, output9C);

			Assert.Equal(targetOutput10A, output10A);
			Assert.Equal(targetOutput10B, output10B);
			Assert.Equal(targetOutput10C, output10C);

			Assert.Equal(targetOutput11A, output11A);
			Assert.Equal(targetOutput11B, output11B);
			Assert.Equal(targetOutput11C, output11C);

			Assert.Equal(targetOutput12A, output12A);
			Assert.Equal(targetOutput12B, output12B);
			Assert.Equal(targetOutput12C, output12C);

			Assert.Equal(targetOutput13A, output13A);
			Assert.Equal(targetOutput13B, output13B);
			Assert.Equal(targetOutput13C, output13C);
		}

		[Fact]
		public void RemovingOptionalEndTagsInTablesIsCorrect()
		{
			// Arrange
			var keepingOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveOptionalEndTags = false });
			var removingOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveOptionalEndTags = true });

			const string input1 = "<table border=\"1\">\n" +
				"	<caption>Books</caption>\n" +
				"	<colgroup>\n" +
				"		<col span=\"2\" style=\"background-color: #ccc\">\n" +
				"		<col style=\"background-color: #fff\">\n" +
				"	</colgroup>\n" +
				"	<colgroup style=\"background-color: #ccc\"></colgroup>\n" +
				"	<tr>\n" +
				"		<th>Title</th>\n" +
				"		<th>Author</th>\n" +
				"		<th>Description</th>\n" +
				"		<th>Price</th>\n" +
				"	</tr>\n" +
				"	<tr>\n" +
				"		<td>HTML5 for pupils</td>\n" +
				"		<td>John Smith</td>\n" +
				"		<td>A flexible guide to developing small sites.</td>\n" +
				"		<td>$1.75</td>\n" +
				"	</tr>\n" +
				"</table>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<table border=\"1\">\n" +
				"	<caption>Books</caption>\n" +
				"	<colgroup>\n" +
				"		<col span=\"2\" style=\"background-color: #ccc\">\n" +
				"		<col style=\"background-color: #fff\">\n" +
				"	\n" +
				"	<colgroup style=\"background-color: #ccc\">\n" +
				"	<tr>\n" +
				"		<th>Title\n" +
				"		<th>Author\n" +
				"		<th>Description\n" +
				"		<th>Price\n" +
				"	\n" +
				"	<tr>\n" +
				"		<td>HTML5 for pupils\n" +
				"		<td>John Smith\n" +
				"		<td>A flexible guide to developing small sites.\n" +
				"		<td>$1.75\n" +
				"	\n" +
				"</table>"
				;


			const string input2 = "<table class=\"table\">\n" +
				"	<thead>\n" +
				"		<tr>\n" +
				"			<th>Month</th>\n" +
				"			<th>Savings</th>\n" +
				"		</tr>\n" +
				"	</thead>\n" +
				"	<tbody>\n" +
				"		<tr>\n" +
				"			<td>Jan</td>\n" +
				"			<td>$2800</td>\n" +
				"		</tr>\n" +
				"		<tr>\n" +
				"			<td>Feb</td>\n" +
				"			<td>$3000</td>\n" +
				"		</tr>\n" +
				"	</tbody>\n" +
				"</table>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = "<table class=\"table\">\n" +
				"	<thead>\n" +
				"		<tr>\n" +
				"			<th>Month\n" +
				"			<th>Savings\n" +
				"		\n" +
				"	\n" +
				"	<tbody>\n" +
				"		<tr>\n" +
				"			<td>Jan\n" +
				"			<td>$2800\n" +
				"		\n" +
				"		<tr>\n" +
				"			<td>Feb\n" +
				"			<td>$3000\n" +
				"		\n" +
				"	\n" +
				"</table>"
				;


			const string input3 = "<table class=\"table\">\n" +
				"	<thead>\n" +
				"		<tr>\n" +
				"			<th>Month</th>\n" +
				"			<th>Savings</th>\n" +
				"		</tr>\n" +
				"	</thead>\n" +
				"	<tfoot>\n" +
				"		<tr>\n" +
				"			<td>Total</td>\n" +
				"			<td>$6250</td>\n" +
				"		</tr>\n" +
				"	</tfoot>\n" +
				"	<tbody>\n" +
				"		<tr>\n" +
				"			<td>Sep</td>\n" +
				"			<td>$3100</td>\n" +
				"		</tr>\n" +
				"		<tr>\n" +
				"			<td>Oct</td>\n" +
				"			<td>$3150</td>\n" +
				"		</tr>\n" +
				"	</tbody>\n" +
				"</table>"
				;
			const string targetOutput3A = input3;
			const string targetOutput3B = "<table class=\"table\">\n" +
				"	<thead>\n" +
				"		<tr>\n" +
				"			<th>Month\n" +
				"			<th>Savings\n" +
				"		\n" +
				"	\n" +
				"	<tfoot>\n" +
				"		<tr>\n" +
				"			<td>Total\n" +
				"			<td>$6250\n" +
				"		\n" +
				"	\n" +
				"	<tbody>\n" +
				"		<tr>\n" +
				"			<td>Sep\n" +
				"			<td>$3100\n" +
				"		\n" +
				"		<tr>\n" +
				"			<td>Oct\n" +
				"			<td>$3150\n" +
				"		\n" +
				"	\n" +
				"</table>"
				;


			const string input4 = "<table class=\"table\">\n" +
				"	<thead>\n" +
				"		<tr>\n" +
				"			<th>Month</th>\n" +
				"			<th>Savings</th>\n" +
				"		</tr>\n" +
				"	</thead>\n" +
				"	<tbody id=\"firstQuarter\">\n" +
				"		<tr>\n" +
				"			<td>Jan</td>\n" +
				"			<td>$2800</td>\n" +
				"		</tr>\n" +
				"		<tr>\n" +
				"			<td>Feb</td>\n" +
				"			<td>$3000</td>\n" +
				"		</tr>\n" +
				"		<tr>\n" +
				"			<td>Mar</td>\n" +
				"			<td>$2950</td>\n" +
				"		</tr>\n" +
				"	</tbody>\n" +
				"	<tbody id=\"secondQuarter\">\n" +
				"		<tr>\n" +
				"			<td>Apr</td>\n" +
				"			<td>$2900</td>\n" +
				"		</tr>\n" +
				"		<tr>\n" +
				"			<td>May</td>\n" +
				"			<td>$3050</td>\n" +
				"		</tr>\n" +
				"		<tr>\n" +
				"			<td>Jun</td>\n" +
				"			<td>$3010</td>\n" +
				"		</tr>\n" +
				"	</tbody>\n" +
				"</table>"
				;
			const string targetOutput4A = input4;
			const string targetOutput4B = "<table class=\"table\">\n" +
				"	<thead>\n" +
				"		<tr>\n" +
				"			<th>Month\n" +
				"			<th>Savings\n" +
				"		\n" +
				"	\n" +
				"	<tbody id=\"firstQuarter\">\n" +
				"		<tr>\n" +
				"			<td>Jan\n" +
				"			<td>$2800\n" +
				"		\n" +
				"		<tr>\n" +
				"			<td>Feb\n" +
				"			<td>$3000\n" +
				"		\n" +
				"		<tr>\n" +
				"			<td>Mar\n" +
				"			<td>$2950\n" +
				"		\n" +
				"	\n" +
				"	<tbody id=\"secondQuarter\">\n" +
				"		<tr>\n" +
				"			<td>Apr\n" +
				"			<td>$2900\n" +
				"		\n" +
				"		<tr>\n" +
				"			<td>May\n" +
				"			<td>$3050\n" +
				"		\n" +
				"		<tr>\n" +
				"			<td>Jun\n" +
				"			<td>$3010\n" +
				"		\n" +
				"	\n" +
				"</table>"
				;


			const string input5 = "<table class=\"table\">\n" +
				"	<thead>\n" +
				"		<tr>\n" +
				"			<th>Month</th>\n" +
				"			<th>Savings</th>\n" +
				"		</tr>\n" +
				"	</thead>\n" +
				"	<tbody>\n" +
				"		<tr>\n" +
				"			<td>Jul</td>\n" +
				"			<td>$2900</td>\n" +
				"		</tr>\n" +
				"		<tr>\n" +
				"			<td>Oct</td>\n" +
				"			<td>$3120</td>\n" +
				"		</tr>\n" +
				"	</tbody>\n" +
				"	<tfoot>\n" +
				"		<tr>\n" +
				"			<td>Total</td>\n" +
				"			<td>$6250</td>\n" +
				"		</tr>\n" +
				"	</tfoot>\n" +
				"</table>"
				;
			const string targetOutput5A = input5;
			const string targetOutput5B = "<table class=\"table\">\n" +
				"	<thead>\n" +
				"		<tr>\n" +
				"			<th>Month\n" +
				"			<th>Savings\n" +
				"		\n" +
				"	\n" +
				"	<tbody>\n" +
				"		<tr>\n" +
				"			<td>Jul\n" +
				"			<td>$2900\n" +
				"		\n" +
				"		<tr>\n" +
				"			<td>Oct\n" +
				"			<td>$3120\n" +
				"		\n" +
				"	\n" +
				"	<tfoot>\n" +
				"		<tr>\n" +
				"			<td>Total\n" +
				"			<td>$6250\n" +
				"		\n" +
				"	\n" +
				"</table>"
				;


			const string input6 = "<table class=\"table\">\n" +
				"	<tr>\n" +
				"		<th>Some header 1…</th>\n" +
				"		<td>Some text 1…</td>\n" +
				"		<th>Some other header 1…</th>\n" +
				"		<td>Some other text 1…</td>\n" +
				"	</tr>\n" +
				"	<tr>\n" +
				"		<th>Some header 2…</th>\n" +
				"		<td>Some text 2…</td>\n" +
				"		<th>Some other header 2…</th>\n" +
				"		<td>Some other text 2…</td>\n" +
				"	</tr>\n" +
				"</table>"
				;
			const string targetOutput6A = input6;
			const string targetOutput6B = "<table class=\"table\">\n" +
				"	<tr>\n" +
				"		<th>Some header 1…\n" +
				"		<td>Some text 1…\n" +
				"		<th>Some other header 1…\n" +
				"		<td>Some other text 1…\n" +
				"	\n" +
				"	<tr>\n" +
				"		<th>Some header 2…\n" +
				"		<td>Some text 2…\n" +
				"		<th>Some other header 2…\n" +
				"		<td>Some other text 2…\n" +
				"	\n" +
				"</table>"
				;


			// Act
			string output1A = keepingOptionalEndTagsMinifier.Minify(input1).MinifiedContent;
			string output1B = removingOptionalEndTagsMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingOptionalEndTagsMinifier.Minify(input2).MinifiedContent;
			string output2B = removingOptionalEndTagsMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingOptionalEndTagsMinifier.Minify(input3).MinifiedContent;
			string output3B = removingOptionalEndTagsMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingOptionalEndTagsMinifier.Minify(input4).MinifiedContent;
			string output4B = removingOptionalEndTagsMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingOptionalEndTagsMinifier.Minify(input5).MinifiedContent;
			string output5B = removingOptionalEndTagsMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingOptionalEndTagsMinifier.Minify(input6).MinifiedContent;
			string output6B = removingOptionalEndTagsMinifier.Minify(input6).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);

			Assert.Equal(targetOutput6A, output6A);
			Assert.Equal(targetOutput6B, output6B);
		}

		[Fact]
		public void RemovingOptionalEndTagsInSelectsIsCorrect()
		{
			// Arrange
			var keepingOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveOptionalEndTags = false });
			var removingOptionalEndTagsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveOptionalEndTags = true });

			const string input1 = "<select name=\"city\">\n" +
				"	<option>Moscow</option>\n" +
				"	<option>St. Petersburg</option>\n" +
				"	<option>Kharkiv</option>\n" +
				"</select>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<select name=\"city\">\n" +
				"	<option>Moscow\n" +
				"	<option>St. Petersburg\n" +
				"	<option>Kharkiv\n" +
				"</select>"
				;


			const string input2 = "<select name=\"preprocessors\">\n" +
				"	<optgroup label=\"Styles\">\n" +
				"		<option>Sass</option>\n" +
				"		<option>LESS</option>\n" +
				"		<option>Stylus</option>\n" +
				"	</optgroup>\n" +
				"	<optgroup label=\"Scripts\">\n" +
				"		<option>CoffeeScript</option>\n" +
				"		<option>TypeScript</option>\n" +
				"		<option>Kaffeine</option>\n" +
				"	</optgroup>\n" +
				"	<option>Dart</option>\n" +
				"</select>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = "<select name=\"preprocessors\">\n" +
				"	<optgroup label=\"Styles\">\n" +
				"		<option>Sass\n" +
				"		<option>LESS\n" +
				"		<option>Stylus\n" +
				"	\n" +
				"	<optgroup label=\"Scripts\">\n" +
				"		<option>CoffeeScript\n" +
				"		<option>TypeScript\n" +
				"		<option>Kaffeine\n" +
				"	</optgroup>\n" +
				"	<option>Dart\n" +
				"</select>"
				;


			const string input3 = "<select name=\"programming_languages\">\n" +
				"	<option>C++</option>\n" +
				"	<option>Delphi</option>\n" +
				"	<option>Java</option>\n" +
				"	<optgroup label=\".NET\">\n" +
				"		<option>C#</option>\n" +
				"		<option>VB.NET</option>\n" +
				"		<option>F#</option>\n" +
				"	</optgroup>\n" +
				"</select>"
				;
			const string targetOutput3A = input3;
			const string targetOutput3B = "<select name=\"programming_languages\">\n" +
				"	<option>C++\n" +
				"	<option>Delphi\n" +
				"	<option>Java\n" +
				"	<optgroup label=\".NET\">\n" +
				"		<option>C#\n" +
				"		<option>VB.NET\n" +
				"		<option>F#\n" +
				"	\n" +
				"</select>"
				;

			// Act
			string output1A = keepingOptionalEndTagsMinifier.Minify(input1).MinifiedContent;
			string output1B = removingOptionalEndTagsMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingOptionalEndTagsMinifier.Minify(input2).MinifiedContent;
			string output2B = removingOptionalEndTagsMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingOptionalEndTagsMinifier.Minify(input3).MinifiedContent;
			string output3B = removingOptionalEndTagsMinifier.Minify(input3).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
		}

		#endregion

		#region Removing tags without content

		[Fact]
		public void RemovingTagsWithoutContentIsCorrect()
		{
			// Arrange
			var removingTagsWithoutContentMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveTagsWithoutContent = true });

			const string input1 = "<p>Some text...</p>";

			const string input2 = "<p></p>";
			const string targetOutput2 = "";

			const string input3 = "<p>Some text...<span>Some other text...</span><span></span></p>";
			const string targetOutput3 = "<p>Some text...<span>Some other text...</span></p>";

			const string input4 = "<a href=\"http://example/com\" title=\"Some title...\"></a>";
			const string targetOutput4 = "";

			const string input5 = "<textarea cols=\"80\" rows=\"10\"></textarea>";
			const string input6 = "<div>Hello, <span>%Username%</span>!</div>";

			const string input7 = "<div>one<div>two <div>three</div><div></div>four</div>five</div>";
			const string targetOutput7 = "<div>one<div>two <div>three</div>four</div>five</div>";

			const string input8 = "<img src=\"\">";
			const string input9 = "<p><!-- Some comment... --></p>";
			const string input10 = "<p>Some text...<span title=\"Some title...\" class=\"caret\"></span></p>";

			const string input11 = "<p>Some text...<span title=\"Some title...\" class=\"\"></span></p>";
			const string targetOutput11 = "<p>Some text...</p>";

			const string input12 = "<p>Some text...<span title=\"Some title...\" id=\"label\"></span></p>";

			const string input13 = "<p>Some text...<span title=\"Some title...\" id=\"\"></span></p>";
			const string targetOutput13 = "<p>Some text...</p>";

			const string input14 = "<p>Some text...<a title=\"Some title...\" name=\"anchor\"></a></p>";

			const string input15 = "<p>Some text...<a title=\"Some title...\" name=\"\"></a></p>";
			const string targetOutput15 = "<p>Some text...</p>";

			const string input16 = "<p>Some text...<span title=\"Some title...\" data-description=\"Some description...\"></span></p>";
			const string input17 = "<p>Some text...<span title=\"Some title...\" data-description=\"\"></span></p>";
			const string input18 = "<div>" +
				"<svg width=\"220\" height=\"220\">" +
				"<circle cx=\"110\" cy=\"110\" r=\"100\" fill=\"#fafaa2\" stroke=\"#000\" />" +
				"</svg>" +
				"</div>"
				;
			const string input19 = "<div>" +
				"<math><infinity /></math>" +
				"</div>"
				;
			const string input20 = "<div role=\"contentinfo\"></div>";

			const string input21 = "<div role=\"\"></div>";
			const string targetOutput21 = "";

			const string input22 = "<div custom-attribute=\"\"></div>";

			const string input23 = "<div>\n" +
				"	<p>	\n  </p>\n" +
				"</div>"
				;
			const string targetOutput23 = "<div>\n" +
				"	\n" +
				"</div>"
				;

			const string input24 = "<style></style>";
			const string targetOutput24 = "";

			const string input25 = "<script></script>";
			const string targetOutput25 = "";

			const string input26 = "<div>\n" +
				"	<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"100\" height=\"50\"></svg>\n" +
				"</div>"
				;
			const string targetOutput26 = "<div>\n" +
				"	\n" +
				"</div>"
				;

			const string input27 = "<div>\n" +
				"	<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"100\" height=\"50\">\n" +
				"		<text x=\"10\" y=\"20\" transform=\"rotate(30 20,40)\"></text>\n" +
				"	</svg>\n" +
				"</div>"
				;

			// Act
			string output1 = removingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;
			string output2 = removingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;
			string output3 = removingTagsWithoutContentMinifier.Minify(input3).MinifiedContent;
			string output4 = removingTagsWithoutContentMinifier.Minify(input4).MinifiedContent;
			string output5 = removingTagsWithoutContentMinifier.Minify(input5).MinifiedContent;
			string output6 = removingTagsWithoutContentMinifier.Minify(input6).MinifiedContent;
			string output7 = removingTagsWithoutContentMinifier.Minify(input7).MinifiedContent;
			string output8 = removingTagsWithoutContentMinifier.Minify(input8).MinifiedContent;
			string output9 = removingTagsWithoutContentMinifier.Minify(input9).MinifiedContent;
			string output10 = removingTagsWithoutContentMinifier.Minify(input10).MinifiedContent;
			string output11 = removingTagsWithoutContentMinifier.Minify(input11).MinifiedContent;
			string output12 = removingTagsWithoutContentMinifier.Minify(input12).MinifiedContent;
			string output13 = removingTagsWithoutContentMinifier.Minify(input13).MinifiedContent;
			string output14 = removingTagsWithoutContentMinifier.Minify(input14).MinifiedContent;
			string output15 = removingTagsWithoutContentMinifier.Minify(input15).MinifiedContent;
			string output16 = removingTagsWithoutContentMinifier.Minify(input16).MinifiedContent;
			string output17 = removingTagsWithoutContentMinifier.Minify(input17).MinifiedContent;
			string output18 = removingTagsWithoutContentMinifier.Minify(input18).MinifiedContent;
			string output19 = removingTagsWithoutContentMinifier.Minify(input19).MinifiedContent;
			string output20 = removingTagsWithoutContentMinifier.Minify(input20).MinifiedContent;
			string output21 = removingTagsWithoutContentMinifier.Minify(input21).MinifiedContent;
			string output22 = removingTagsWithoutContentMinifier.Minify(input22).MinifiedContent;
			string output23 = removingTagsWithoutContentMinifier.Minify(input23).MinifiedContent;
			string output24 = removingTagsWithoutContentMinifier.Minify(input24).MinifiedContent;
			string output25 = removingTagsWithoutContentMinifier.Minify(input25).MinifiedContent;
			string output26 = removingTagsWithoutContentMinifier.Minify(input26).MinifiedContent;
			string output27 = removingTagsWithoutContentMinifier.Minify(input27).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(input5, output5);
			Assert.Equal(input6, output6);
			Assert.Equal(targetOutput7, output7);
			Assert.Equal(input8, output8);
			Assert.Equal(input9, output9);
			Assert.Equal(input10, output10);
			Assert.Equal(targetOutput11, output11);
			Assert.Equal(input12, output12);
			Assert.Equal(targetOutput13, output13);
			Assert.Equal(input14, output14);
			Assert.Equal(targetOutput15, output15);
			Assert.Equal(input16, output16);
			Assert.Equal(input17, output17);
			Assert.Equal(input18, output18);
			Assert.Equal(input19, output19);
			Assert.Equal(input20, output20);
			Assert.Equal(targetOutput21, output21);
			Assert.Equal(input22, output22);
			Assert.Equal(targetOutput23, output23);
			Assert.Equal(targetOutput24, output24);
			Assert.Equal(targetOutput25, output25);
			Assert.Equal(targetOutput26, output26);
			Assert.Equal(input27, output27);
		}

		#endregion

		#region Collapsing boolean attributes

		[Fact]
		public void CollapsingBooleanAttributesIsCorrect()
		{
			// Arrange
			var collapsingBooleanAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { CollapseBooleanAttributes = true });

			const string input1 = "<input disabled=\"disabled\">";
			const string targetOutput1 = "<input disabled>";

			const string input2 = "<input CHECKED = \"checked\" readonly=\"readonly\">";
			const string targetOutput2 = "<input checked readonly>";

			const string input3 = "<option name=\"fixed\" selected=\"selected\">Fixed</option>";
			const string targetOutput3 = "<option name=\"fixed\" selected>Fixed</option>";

			// Act
			string output1 = collapsingBooleanAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = collapsingBooleanAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = collapsingBooleanAttributesMinifier.Minify(input3).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
		}

		[Fact]
		public void ProcessingCustomBooleanAttributesIsCorrect()
		{
			// Arrange
			var keepingBooleanAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { CollapseBooleanAttributes = false });
			var collapsingBooleanAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { CollapseBooleanAttributes = true });

			const string input1 = "<div custom-attribute></div>";
			const string targetOutput1 = input1;

			const string input2 = "<div class></div>";
			const string targetOutput2 = "<div class=\"\"></div>";

			// Act
			string output1A = keepingBooleanAttributesMinifier.Minify(input1).MinifiedContent;
			string output1B = collapsingBooleanAttributesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingBooleanAttributesMinifier.Minify(input2).MinifiedContent;
			string output2B = collapsingBooleanAttributesMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1A);
			Assert.Equal(targetOutput1, output1B);

			Assert.Equal(targetOutput2, output2A);
			Assert.Equal(targetOutput2, output2B);
		}

		#endregion

		#region Removing empty attributes

		[Fact]
		public void RemovingEmptyAttributesIsCorrect()
		{
			// Arrange
			var removingEmptyAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveEmptyAttributes = true });

			const string input1 = "<p id=\"\" class=\"\" STYLE=\" \" title=\"\n\" lang=\"\" dir=\"\">Some text…</p>";
			const string targetOutput1 = "<p>Some text…</p>";

			const string input2 = "<p onclick=\"\"   ondblclick=\" \" onmousedown=\"\" ONMOUSEUP=\"\" " +
				"onmouseover=\" \" onmousemove=\"\" onmouseout=\"\" " +
				"onkeypress=\n\n  \"\n     \" onkeydown=\n\"\" onkeyup\n=\"\">Some text…</p>";
			const string targetOutput2 = "<p>Some text…</p>";

			const string input3 = "<input onfocus=\"\" onblur=\"\" onchange=\" \" value=\" Some value… \">";
			const string targetOutput3 = "<input value=\" Some value… \">";

			const string input4 = "<input name=\"Some Name…\" value=\"\">";
			const string targetOutput4 = "<input name=\"Some Name…\">";

			const string input5 = "<img src=\"\" alt=\"\">";

			const string input6 = "<form action=\"\">Some controls…</form>";
			const string targetOutput5 = "<form>Some controls…</form>";

			// Act
			string output1 = removingEmptyAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = removingEmptyAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = removingEmptyAttributesMinifier.Minify(input3).MinifiedContent;
			string output4 = removingEmptyAttributesMinifier.Minify(input4).MinifiedContent;
			string output5 = removingEmptyAttributesMinifier.Minify(input5).MinifiedContent;
			string output6 = removingEmptyAttributesMinifier.Minify(input6).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(input5, output5);
			Assert.Equal(targetOutput5, output6);
		}

		#endregion

		#region Processing attribute quotes

		[Fact]
		public void RemovingAttributeQuotesIsCorrect()
		{
			// Arrange
			var keepingAttributeQuotesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { AttributeQuotesRemovalMode = HtmlAttributeQuotesRemovalMode.KeepQuotes });
			var html4RemovingAttributeQuotesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { AttributeQuotesRemovalMode = HtmlAttributeQuotesRemovalMode.Html4 });
			var html5RemovingAttributeQuotesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { AttributeQuotesRemovalMode = HtmlAttributeQuotesRemovalMode.Html5 });

			const string input1 = "<input value=\"Minifier\">";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<input value=Minifier>";
			const string targetOutput1C = "<input value=Minifier>";

			const string input2 = "<input value=\"Минимизатор\">";
			const string targetOutput2A = input2;
			const string targetOutput2B = input2;
			const string targetOutput2C = "<input value=Минимизатор>";

			const string input3 = "<input value=\"極小\">";
			const string targetOutput3A = input3;
			const string targetOutput3B = input3;
			const string targetOutput3C = "<input value=極小>";

			const string input4 = "<input value=\"HTML Minifier\">";

			const string input5 = "<div class=\"l-constrained\"></div>";
			const string targetOutput5A = input5;
			const string targetOutput5B = "<div class=l-constrained></div>";
			const string targetOutput5C = "<div class=l-constrained></div>";

			const string input6 = "<input class=\"search__button\">";
			const string targetOutput6A = input6;
			const string targetOutput6B = "<input class=search__button>";
			const string targetOutput6C = "<input class=search__button>";

			const string input7 = "<link href=\"favicon.ico\" type=\"image/x-icon\">";
			const string targetOutput7A = input7;
			const string targetOutput7B = "<link href=favicon.ico type=\"image/x-icon\">";
			const string targetOutput7C = "<link href=favicon.ico type=image/x-icon>";

			const string input8 = "<a href=\"\\\\sun\">Intranet portal</a>";
			const string targetOutput8A = input8;
			const string targetOutput8B = input8;
			const string targetOutput8C = "<a href=\\\\sun>Intranet portal</a>";

			const string input9 = "<input value=\"a&lt;0\">";
			const string input10 = "<input value=\"a=0\">";
			const string input11 = "<input value=\"a>0\">";

			const string input12 = "<a href=\"#forms\" title=\"Form`s\"></a>";
			const string targetOutput12A = input12;
			const string targetOutput12B = input12;
			const string targetOutput12C = "<a href=#forms title=\"Form`s\"></a>";

			const string input13 = "<a href=\"?forms\" title=\"Form's\"></a>";
			const string targetOutput13A = input13;
			const string targetOutput13B = input13;
			const string targetOutput13C = "<a href=?forms title=\"Form's\"></a>";

			const string input14 = "<a href=\"/\">Home page</a>";

			const string input15 = "<a href=\"#/\">Home page</a>";

			const string input16 = "<input value=\"localhost:86\">";
			const string targetOutput16A = input16;
			const string targetOutput16B = "<input value=localhost:86>";
			const string targetOutput16C = "<input value=localhost:86>";

			const string input17 = "<svg width=\"220\" height=\"220\">" +
				"<line x1=\"6\" y1=\"14\" x2=\"177\" y2=\"198\" stroke=\"#000\" />" +
				"</svg>"
				;
			const string targetOutput17A = input17;
			const string targetOutput17B = "<svg width=220 height=220>" +
				"<line x1=6 y1=14 x2=177 y2=198 stroke=\"#000\" />" +
				"</svg>"
				;
			const string targetOutput17C = "<svg width=220 height=220>" +
				"<line x1=6 y1=14 x2=177 y2=198 stroke=#000 />" +
				"</svg>"
				;

			const string input18 = "<math>" +
				"<apply>" +
				"<in />" +
				"<cn type=\"complex-cartesian\">17<sep />29</cn>" +
				"<complexes />" +
				"</apply>" +
				"</math>"
				;
			const string targetOutput18A = input18;
			const string targetOutput18B = "<math>" +
				"<apply>" +
				"<in />" +
				"<cn type=complex-cartesian>17<sep />29</cn>" +
				"<complexes />" +
				"</apply>" +
				"</math>"
				;
			const string targetOutput18C = targetOutput18B;

			// Act
			string output1A = keepingAttributeQuotesMinifier.Minify(input1).MinifiedContent;
			string output1B = html4RemovingAttributeQuotesMinifier.Minify(input1).MinifiedContent;
			string output1C = html5RemovingAttributeQuotesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingAttributeQuotesMinifier.Minify(input2).MinifiedContent;
			string output2B = html4RemovingAttributeQuotesMinifier.Minify(input2).MinifiedContent;
			string output2C = html5RemovingAttributeQuotesMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingAttributeQuotesMinifier.Minify(input3).MinifiedContent;
			string output3B = html4RemovingAttributeQuotesMinifier.Minify(input3).MinifiedContent;
			string output3C = html5RemovingAttributeQuotesMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingAttributeQuotesMinifier.Minify(input4).MinifiedContent;
			string output4B = html4RemovingAttributeQuotesMinifier.Minify(input4).MinifiedContent;
			string output4C = html5RemovingAttributeQuotesMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingAttributeQuotesMinifier.Minify(input5).MinifiedContent;
			string output5B = html4RemovingAttributeQuotesMinifier.Minify(input5).MinifiedContent;
			string output5C = html5RemovingAttributeQuotesMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingAttributeQuotesMinifier.Minify(input6).MinifiedContent;
			string output6B = html4RemovingAttributeQuotesMinifier.Minify(input6).MinifiedContent;
			string output6C = html5RemovingAttributeQuotesMinifier.Minify(input6).MinifiedContent;

			string output7A = keepingAttributeQuotesMinifier.Minify(input7).MinifiedContent;
			string output7B = html4RemovingAttributeQuotesMinifier.Minify(input7).MinifiedContent;
			string output7C = html5RemovingAttributeQuotesMinifier.Minify(input7).MinifiedContent;

			string output8A = keepingAttributeQuotesMinifier.Minify(input8).MinifiedContent;
			string output8B = html4RemovingAttributeQuotesMinifier.Minify(input8).MinifiedContent;
			string output8C = html5RemovingAttributeQuotesMinifier.Minify(input8).MinifiedContent;

			string output9A = keepingAttributeQuotesMinifier.Minify(input9).MinifiedContent;
			string output9B = html4RemovingAttributeQuotesMinifier.Minify(input9).MinifiedContent;
			string output9C = html5RemovingAttributeQuotesMinifier.Minify(input9).MinifiedContent;

			string output10A = keepingAttributeQuotesMinifier.Minify(input10).MinifiedContent;
			string output10B = html4RemovingAttributeQuotesMinifier.Minify(input10).MinifiedContent;
			string output10C = html5RemovingAttributeQuotesMinifier.Minify(input10).MinifiedContent;

			string output11A = keepingAttributeQuotesMinifier.Minify(input11).MinifiedContent;
			string output11B = html4RemovingAttributeQuotesMinifier.Minify(input11).MinifiedContent;
			string output11C = html5RemovingAttributeQuotesMinifier.Minify(input11).MinifiedContent;

			string output12A = keepingAttributeQuotesMinifier.Minify(input12).MinifiedContent;
			string output12B = html4RemovingAttributeQuotesMinifier.Minify(input12).MinifiedContent;
			string output12C = html5RemovingAttributeQuotesMinifier.Minify(input12).MinifiedContent;

			string output13A = keepingAttributeQuotesMinifier.Minify(input13).MinifiedContent;
			string output13B = html4RemovingAttributeQuotesMinifier.Minify(input13).MinifiedContent;
			string output13C = html5RemovingAttributeQuotesMinifier.Minify(input13).MinifiedContent;

			string output14A = keepingAttributeQuotesMinifier.Minify(input14).MinifiedContent;
			string output14B = html4RemovingAttributeQuotesMinifier.Minify(input14).MinifiedContent;
			string output14C = html5RemovingAttributeQuotesMinifier.Minify(input14).MinifiedContent;

			string output15A = keepingAttributeQuotesMinifier.Minify(input15).MinifiedContent;
			string output15B = html4RemovingAttributeQuotesMinifier.Minify(input15).MinifiedContent;
			string output15C = html5RemovingAttributeQuotesMinifier.Minify(input15).MinifiedContent;

			string output16A = keepingAttributeQuotesMinifier.Minify(input16).MinifiedContent;
			string output16B = html4RemovingAttributeQuotesMinifier.Minify(input16).MinifiedContent;
			string output16C = html5RemovingAttributeQuotesMinifier.Minify(input16).MinifiedContent;

			string output17A = keepingAttributeQuotesMinifier.Minify(input17).MinifiedContent;
			string output17B = html4RemovingAttributeQuotesMinifier.Minify(input17).MinifiedContent;
			string output17C = html5RemovingAttributeQuotesMinifier.Minify(input17).MinifiedContent;

			string output18A = keepingAttributeQuotesMinifier.Minify(input18).MinifiedContent;
			string output18B = html4RemovingAttributeQuotesMinifier.Minify(input18).MinifiedContent;
			string output18C = html5RemovingAttributeQuotesMinifier.Minify(input18).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(input4, output4A);
			Assert.Equal(input4, output4B);
			Assert.Equal(input4, output4C);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);

			Assert.Equal(targetOutput6A, output6A);
			Assert.Equal(targetOutput6B, output6B);
			Assert.Equal(targetOutput6C, output6C);

			Assert.Equal(targetOutput7A, output7A);
			Assert.Equal(targetOutput7B, output7B);
			Assert.Equal(targetOutput7C, output7C);

			Assert.Equal(targetOutput8A, output8A);
			Assert.Equal(targetOutput8B, output8B);
			Assert.Equal(targetOutput8C, output8C);

			Assert.Equal(input9, output9A);
			Assert.Equal(input9, output9B);
			Assert.Equal(input9, output9C);

			Assert.Equal(input10, output10A);
			Assert.Equal(input10, output10B);
			Assert.Equal(input10, output10C);

			Assert.Equal(input11, output11A);
			Assert.Equal(input11, output11B);
			Assert.Equal(input11, output11C);

			Assert.Equal(targetOutput12A, output12A);
			Assert.Equal(targetOutput12B, output12B);
			Assert.Equal(targetOutput12C, output12C);

			Assert.Equal(targetOutput13A, output13A);
			Assert.Equal(targetOutput13B, output13B);
			Assert.Equal(targetOutput13C, output13C);

			Assert.Equal(input14, output14A);
			Assert.Equal(input14, output14B);
			Assert.Equal(input14, output14C);

			Assert.Equal(input15, output15A);
			Assert.Equal(input15, output15B);
			Assert.Equal(input15, output15C);

			Assert.Equal(targetOutput16A, output16A);
			Assert.Equal(targetOutput16B, output16B);
			Assert.Equal(targetOutput16C, output16C);

			Assert.Equal(targetOutput17A, output17A);
			Assert.Equal(targetOutput17B, output17B);
			Assert.Equal(targetOutput17C, output17C);

			Assert.Equal(targetOutput18A, output18A);
			Assert.Equal(targetOutput18B, output18B);
			Assert.Equal(targetOutput18C, output18C);
		}

		#endregion

		#region Removing redundant attributes

		[Fact]
		public void RemovingRedundantFormAttributesIsCorrect()
		{
			// Arrange
			var removingRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = true });

			const string input1 = "<form method=\"get\">Some controls…</form>";
			const string targetOutput1 = "<form>Some controls…</form>";

			const string input2 = "<form method=\"post\">Some controls…</form>";

			// Act
			string output1 = removingRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = removingRedundantAttributesMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(input2, output2);
		}

		[Fact]
		public void RemovingRedundantInputAttributesIsCorrect()
		{
			// Arrange
			var removingRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = true });

			const string input1 = "<input type=\"text\">";
			const string targetOutput1 = "<input>";

			const string input2 = "<input type=\"  TEXT  \" value=\"Some value…\">";
			const string targetOutput2 = "<input value=\"Some value…\">";

			const string input3 = "<input type=\"checkbox\">";

			// Act
			string output1 = removingRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = removingRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = removingRedundantAttributesMinifier.Minify(input3).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(input3, output3);
		}

		[Fact]
		public void RemovingRedundantAnchorAttributesIsCorrect()
		{
			// Arrange
			var removingRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = true });

			const string input1 = "<a id=\"_toppage\" name=\"_toppage\"></a>";
			const string targetOutput1 = "<a id=\"_toppage\"></a>";

			const string input2 = "<a name=\"_toppage\"></a>";

			const string input3 = "<a href=\"http://example.com/\" id=\"lnkExample\" name=\"lnkExample\"></a>";
			const string targetOutput3 = "<a href=\"http://example.com/\" id=\"lnkExample\"></a>";

			const string input4 = "<input id=\"txtEmail\" name=\"admin@example.com\">";

			const string input5 = "<a href=\"http://example.com/\" id=\"lnkExample\" name=\"example\"></a>";

			// Act
			string output1 = removingRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = removingRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = removingRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output4 = removingRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output5 = removingRedundantAttributesMinifier.Minify(input5).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(input2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(input4, output4);
			Assert.Equal(input5, output5);
		}

		[Fact]
		public void RemovingRedundantStyleAttributesIsCorrect()
		{
			// Arrange
			var removingRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = true });

			const string input1 = "<link rel=\"stylesheet\" charset=\"Windows-1251\" href=\"mystyles.css\">";
			const string targetOutput1 = "<link rel=\"stylesheet\" href=\"mystyles.css\">";

			const string input2 = "<link rel=\"index\" href=\"../index.html\">";

			// Act
			string output1 = removingRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = removingRedundantAttributesMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(input2, output2);
		}

		[Fact]
		public void RemovingRedundantScriptAttributesIsCorrect()
		{
			// Arrange
			var removingRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = true });

			const string input1 = "<script type=\"text/javascript\" charset=\"UTF-8\">alert(\"Hooray!\");</script>";
			const string targetOutput1 = "<script type=\"text/javascript\">alert(\"Hooray!\");</script>";

			const string input2 = "<script type=\"text/javascript\"" +
				" src=\"http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js\"" +
				" charset=\"UTF-8\"></script>"
				;

			const string input3 = "<script CHARSET=\"UTF-8\">alert(\"Hooray!\");</script>";
			const string targetOutput3 = "<script>alert(\"Hooray!\");</script>";

			const string input4 = "<script language=\"Javascript\">var a = 1, b = 3;</script>";
			const string targetOutput4 = "<script>var a = 1, b = 3;</script>";

			const string input5 = "<script LANGUAGE = \"  javaScript  \">var a = 1, b = 3;</script>";
			const string targetOutput5 = "<script>var a = 1, b = 3;</script>";

			// Act
			string output1 = removingRedundantAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = removingRedundantAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = removingRedundantAttributesMinifier.Minify(input3).MinifiedContent;
			string output4 = removingRedundantAttributesMinifier.Minify(input4).MinifiedContent;
			string output5 = removingRedundantAttributesMinifier.Minify(input5).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(input2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
		}

		[Fact]
		public void RemovingRedundantAreaAttributesIsCorrect()
		{
			// Arrange
			var removingRedundantAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveRedundantAttributes = true });

			const string input1 = "<area shape=\"rect\" coords=\"0,0,82,126\"" +
				" href=\"http://example.com/\" title=\"Some title…\">";
			const string targetOutput1 = "<area coords=\"0,0,82,126\"" +
				" href=\"http://example.com/\" title=\"Some title…\">";

			// Act
			string output1 = removingRedundantAttributesMinifier.Minify(input1).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
		}

		#endregion

		#region Removing JavaScript type attributes

		[Fact]
		public void RemovingJavaScriptTypeAttributesIsCorrect()
		{
			// Arrange
			var removingJavaScriptTypeAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveJsTypeAttributes = true });
			var keepingJavaScriptTypeAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveJsTypeAttributes = false });

			const string input1 = "<script type=\"text/javascript\">alert(\"Hooray!\");</script>";
			const string targetOutput1 = "<script>alert(\"Hooray!\");</script>";

			const string input2 = "<SCRIPT TYPE=\"  text/javascript \">alert(\"Hooray!\");</script>";
			const string targetOutput2 = "<script>alert(\"Hooray!\");</script>";

			const string input3 = "<script type=\"text/ecmascript\">alert(\"Hooray!\");</script>";
			const string input4 = "<script type=\"text/vbscript\">MsgBox(\"Hooray!\")</script>";

			// Act
			string output1A = removingJavaScriptTypeAttributesMinifier.Minify(input1).MinifiedContent;
			string output1B = keepingJavaScriptTypeAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = removingJavaScriptTypeAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = removingJavaScriptTypeAttributesMinifier.Minify(input3).MinifiedContent;
			string output4 = removingJavaScriptTypeAttributesMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1A);
			Assert.Equal(input1, output1B);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(input3, output3);
			Assert.Equal(input4, output4);
		}

		#endregion

		#region Removing CSS type attributes

		[Fact]
		public void RemovingCssTypeAttributesIsCorrect()
		{
			// Arrange
			var removingCssTypeAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveCssTypeAttributes = true });
			var keepingCssTypeAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveCssTypeAttributes = false });

			const string input1 = "<style type=\"text/css\">.error { color: #b94a48; }</style>";
			const string targetOutput1 = "<style>.error { color: #b94a48; }</style>";

			const string input2 = "<STYLE TYPE = \"  text/CSS \">body { font-size: 14px; }</style>";
			const string targetOutput2 = "<style>body { font-size: 14px; }</style>";

			const string input3 = "<style type=\"text/less\">\n" +
				"	@color: #4D926F;\n\n" +
				"	#header { color: @color; }\n" +
				"</style>"
				;

			const string input4 = "<link rel=\"stylesheet\" type=\"text/css\"" +
				" href=\"http://twitter.github.com/bootstrap/assets/css/bootstrap.css\">";
			const string targetOutput4 = "<link rel=\"stylesheet\"" +
				" href=\"http://twitter.github.com/bootstrap/assets/css/bootstrap.css\">";

			const string input5 = "<link rel=\"stylesheet/less\" type=\"text/css\" href=\"styles.less\">";

			// Act
			string output1A = removingCssTypeAttributesMinifier.Minify(input1).MinifiedContent;
			string output1B = keepingCssTypeAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = removingCssTypeAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = removingCssTypeAttributesMinifier.Minify(input3).MinifiedContent;
			string output4 = removingCssTypeAttributesMinifier.Minify(input4).MinifiedContent;
			string output5 = removingCssTypeAttributesMinifier.Minify(input5).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1A);
			Assert.Equal(input1, output1B);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(input3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(input5, output5);
		}

		#endregion

		#region Removing HTTP protocol from attributes

		[Fact]
		public void RemovingHttpProtocolFromAttributesIsCorrect()
		{
			// Arrange
			var removingHttpProtocolMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHttpProtocolFromAttributes = true });

			const string input1 = "<script src=\"http://ajax.aspnetcdn.com/ajax/jquery/jquery-1.8.0.js\"></script>";
			const string targetOutput1 = "<script src=\"//ajax.aspnetcdn.com/ajax/jquery/jquery-1.8.0.js\"></script>";

			const string input2 = "<link rel=\"Stylesheet\" href=\"http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.10/themes/redmond/jquery-ui.css\">";
			const string targetOutput2 = "<link rel=\"Stylesheet\" href=\"//ajax.aspnetcdn.com/ajax/jquery.ui/1.8.10/themes/redmond/jquery-ui.css\">";

			const string input3 = "<input value=\"http://kangax.github.com/html-minifier/\">";
			const string input4 = "<a href=\"https://github.com/kangax/html-minifier\">Experimental HTML Minifier</a>";
			const string input5 = "<link rel=\"external\" href=\"http://example.com/about\">";
			const string input6 = "<link rel=\"alternate external\" href=\"http://example.com/about\">";

			// Act
			string output1 = removingHttpProtocolMinifier.Minify(input1).MinifiedContent;
			string output2 = removingHttpProtocolMinifier.Minify(input2).MinifiedContent;
			string output3 = removingHttpProtocolMinifier.Minify(input3).MinifiedContent;
			string output4 = removingHttpProtocolMinifier.Minify(input4).MinifiedContent;
			string output5 = removingHttpProtocolMinifier.Minify(input5).MinifiedContent;
			string output6 = removingHttpProtocolMinifier.Minify(input6).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(input3, output3);
			Assert.Equal(input4, output4);
			Assert.Equal(input5, output5);
			Assert.Equal(input6, output6);
		}

		#endregion

		#region Removing HTTPS protocol from attributes

		[Fact]
		public void RemovingHttpsProtocolFromAttributesIsCorrect()
		{
			// Arrange
			var removingHttpsProtocolMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHttpsProtocolFromAttributes = true });

			const string input1 = "<a href=\"https://code.google.com/p/htmlcompressor/\">HTML Compressor and Minifier</a>";
			const string targetOutput1 = "<a href=\"//code.google.com/p/htmlcompressor/\">HTML Compressor and Minifier</a>";

			const string input2 = "<iframe src=\"https://www.facebook.com/plugins/registration\"></iframe>";
			const string targetOutput2 = "<iframe src=\"//www.facebook.com/plugins/registration\"></iframe>";

			const string input3 = "<input value=\"https://google-styleguide.googlecode.com/svn/trunk/htmlcssguide.xml\">";
			const string input4 = "<a href=\"http://htmlcompressor.com/\">HTMLCompressor.com</a>";
			const string input5 = "<link rel=\"external\" href=\"https://example.com/about\">";
			const string input6 = "<link rel=\"alternate external\" href=\"https://example.com/about\">";

			// Act
			string output1 = removingHttpsProtocolMinifier.Minify(input1).MinifiedContent;
			string output2 = removingHttpsProtocolMinifier.Minify(input2).MinifiedContent;
			string output3 = removingHttpsProtocolMinifier.Minify(input3).MinifiedContent;
			string output4 = removingHttpsProtocolMinifier.Minify(input4).MinifiedContent;
			string output5 = removingHttpsProtocolMinifier.Minify(input5).MinifiedContent;
			string output6 = removingHttpsProtocolMinifier.Minify(input6).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(input3, output3);
			Assert.Equal(input4, output4);
			Assert.Equal(input5, output5);
			Assert.Equal(input6, output6);
		}

		#endregion

		#region Removing JavaScript protocol from attributes

		[Fact]
		public void RemovingJsProtocolFromAttributesIsCorrect()
		{
			// Arrange
			var keepingJsProtocolMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveJsProtocolFromAttributes = false });
			var removingJsProtocolMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveJsProtocolFromAttributes = true });

			const string input1 = "<p onclick=\"javascript:alert('Hooray!')\">Some text…</p>";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<p onclick=\"alert('Hooray!')\">Some text…</p>";

			const string input2 = "<p onclick=\"javascript:hideSuggest();\">Some text…</p>";
			const string targetOutput2A = "<p onclick=\"javascript:hideSuggest()\">Some text…</p>";
			const string targetOutput2B = "<p onclick=\"hideSuggest()\">Some text…</p>";

			const string input3 = "<p onclick=\" JavaScript: switchHPLeaders(this, 'rus'); return false; \">Some text…</p>";
			const string targetOutput3A = "<p onclick=\"JavaScript: switchHPLeaders(this, 'rus'); return false\">Some text…</p>";
			const string targetOutput3B = "<p onclick=\"switchHPLeaders(this, 'rus'); return false\">Some text…</p>";

			const string input4 = "<p title=\"javascript:(function(){ /* Some code… */ })()\">Some text…</p>";

			const string input5 = "<a href=\"javascript:webCall('qsd07cggfg3bjg6gkl', null, 'poll:true');\">Call from Web site</a>";
			const string targetOutput5 = "<a href=\"javascript:webCall('qsd07cggfg3bjg6gkl', null, 'poll:true')\">Call from Web site</a>";

			// Act
			string output1A = keepingJsProtocolMinifier.Minify(input1).MinifiedContent;
			string output1B = removingJsProtocolMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingJsProtocolMinifier.Minify(input2).MinifiedContent;
			string output2B = removingJsProtocolMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingJsProtocolMinifier.Minify(input3).MinifiedContent;
			string output3B = removingJsProtocolMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingJsProtocolMinifier.Minify(input4).MinifiedContent;
			string output4B = removingJsProtocolMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingJsProtocolMinifier.Minify(input5).MinifiedContent;
			string output5B = removingJsProtocolMinifier.Minify(input5).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);

			Assert.Equal(input4, output4A);
			Assert.Equal(input4, output4B);

			Assert.Equal(targetOutput5, output5A);
			Assert.Equal(targetOutput5, output5B);
		}

		#endregion

		#region Minification of embedded JavaScript templates

		[Fact]
		public void MinificationOfEmbeddedJsTemplatesIsCorrect()
		{
			// Arrange
			var keepingEmbeddedJsTemplatesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					ProcessableScriptTypeList = "",
					WhitespaceMinificationMode = WhitespaceMinificationMode.Medium
				});
			var minifyingEmbeddedJsTemplatesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					ProcessableScriptTypeList = "text/html,text/x-kendo-template,text/ng-template",
					WhitespaceMinificationMode = WhitespaceMinificationMode.Medium
				});

			const string input1 = "<script type=\"text/html\" id=\"person-template\">\n" +
				"	<h3 data-bind=\"text: name\"></h3>\n" +
				"	<p>Credits: <span data-bind=\"text: credits\"></span></p>\n" +
				"</script>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<script type=\"text/html\" id=\"person-template\">" +
				"<h3 data-bind=\"text: name\"></h3>" +
				"<p>Credits: <span data-bind=\"text: credits\"></span></p>" +
				"</script>"
				;

			const string input2 = "<script id=\"ul-template\" type=\"text/x-kendo-template\">\n" +
				"	<li> id: <span data-bind=\"text: id\"></span> </li>\n" +
				"	<li> details: <span data-bind=\"text: id\"></span> </li>\n" +
				"</script>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = "<script id=\"ul-template\" type=\"text/x-kendo-template\">" +
				"<li>id: <span data-bind=\"text: id\"></span></li>" +
				"<li>details: <span data-bind=\"text: id\"></span></li>" +
				"</script>"
				;

			const string input3 = "<script id=\"entry-template\" type=\"text/x-handlebars-template\">\n" +
				"	<div class=\"entry\">\n" +
				"		<h1>{{title}}</h1>\n" +
				"		<div class=\"body\">\n" +
				"			{{{body}}}\n" +
				"		</div>\n" +
				"	</div>\n" +
				"</script>"
				;

			const string input4 = "<script type=\"text/ng-template\" id=\"questionnaire\">\n" +
				"	<div class=\"question\">\n" +
				"		<span>{{question.number}}.</span>&nbsp;\n" +
				"		<span>{{question.question}}</span> -&nbsp;\n" +
				"		<span>{{question.type}}</span>\n" +
				"		<div class=\"answer\" ng-include=\"'templates/'+question.type+'.html'\">\n" +
				"		</div>\n" +
				"	</div>\n" +
				"</script>"
				;
			const string targetOutput4A = input4;
			const string targetOutput4B = "<script type=\"text/ng-template\" id=\"questionnaire\">" +
				"<div class=\"question\">" +
				"<span>{{question.number}}.</span>&nbsp; " +
				"<span>{{question.question}}</span> -&nbsp; " +
				"<span>{{question.type}}</span>" +
				"<div class=\"answer\" ng-include=\"'templates/'+question.type+'.html'\">" +
				"</div>" +
				"</div>" +
				"</script>"
				;

			// Act
			string output1A = keepingEmbeddedJsTemplatesMinifier.Minify(input1).MinifiedContent;
			string output1B = minifyingEmbeddedJsTemplatesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingEmbeddedJsTemplatesMinifier.Minify(input2).MinifiedContent;
			string output2B = minifyingEmbeddedJsTemplatesMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingEmbeddedJsTemplatesMinifier.Minify(input3).MinifiedContent;
			string output3B = minifyingEmbeddedJsTemplatesMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingEmbeddedJsTemplatesMinifier.Minify(input4).MinifiedContent;
			string output4B = minifyingEmbeddedJsTemplatesMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);

			Assert.Equal(input3, output3A);
			Assert.Equal(input3, output3B);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
		}

		#endregion
	}
}