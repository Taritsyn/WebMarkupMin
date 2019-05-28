using System.Collections.Generic;

using Xunit;

namespace WebMarkupMin.Core.Test.Xhtml
{
	public class MinificationTests : FileSystemTestsBase
	{
		#region Processing XML nodes

		[Fact]
		public void ProcessingXmlNodesIsCorrect()
		{
			// Arrange
			var minifier = new XhtmlMinifier(new XhtmlMinificationSettings(true));

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
			const string targetOutput1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
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

			const string input2 = "<svg version=\"1.1\" baseProfile=\"full\" " +
				"xmlns=\"http://www.w3.org/2000/svg\" " +
				"xmlns:xlink=\"http://www.w3.org/1999/xlink\" " +
				"xmlns:ev=\"http://www.w3.org/2001/xml-events\" " +
				"width=\"100%\" height=\"100%\">\n" +
				"	<rect fill=\"white\" x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" />\n" +
				"	<rect fill=\"silver\" x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" rx=\"1em\" />\n" +
				"</svg>"
				;

			const string input3 = "<math xmlns=\"http://www.w3.org/1998/Math/MathML\">\n" +
				"	<infinity />\n" +
				"</math>"
				;

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

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(0, warnings1.Count);

			Assert.Equal(input2, output2);
			Assert.Equal(0, warnings2.Count);

			Assert.Equal(input3, output3);
			Assert.Equal(0, warnings3.Count);
		}

		#endregion

		#region Processing DOCTYPE declaration

		[Fact]
		public void ProcessingDoctypeIsCorrect()
		{
			// Arrange
			var minifier = new XhtmlMinifier(new XhtmlMinificationSettings(true));

			const string input1 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\"\n" +
				"   \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">";
			const string targetOutput1 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\"" +
				" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">";

			const string input2 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\"" +
				" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
			const string targetOutput2 = input2;

			const string input3 = "<!DOCTYPE\n" +
				"    html PUBLIC \"-//W3C//DTD XHTML 1.0 Frameset//EN\"\n" +
				"    \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd\">"
				;
			const string targetOutput3 = "<!DOCTYPE" +
				" html PUBLIC \"-//W3C//DTD XHTML 1.0 Frameset//EN\"" +
				" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd\">"
				;

			const string input4 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML Basic 1.1//EN\"\r\n" +
				"    \"http://www.w3.org/TR/xhtml-basic/xhtml-basic11.dtd\">";
			const string targetOutput4 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML Basic 1.1//EN\"" +
				" \"http://www.w3.org/TR/xhtml-basic/xhtml-basic11.dtd\">";

			const string input5 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \n" +
				"	\"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">";
			const string targetOutput5 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\"" +
				" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">";

			const string input6 = "<!DOCTYPE html>";
			const string targetOutput6 = input6;

			const string input7 = "<!DOCTYPE\r\nhtml>";
			const string targetOutput7 = "<!DOCTYPE html>";

			const string input8 = "<!DOCTYPE\thtml>";
			const string targetOutput8 = "<!DOCTYPE html>";

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			string output3 = minifier.Minify(input3).MinifiedContent;
			string output4 = minifier.Minify(input4).MinifiedContent;
			string output5 = minifier.Minify(input5).MinifiedContent;
			string output6 = minifier.Minify(input6).MinifiedContent;
			string output7 = minifier.Minify(input7).MinifiedContent;
			string output8 = minifier.Minify(input8).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
			Assert.Equal(targetOutput6, output6);
			Assert.Equal(targetOutput7, output7);
			Assert.Equal(targetOutput8, output8);
		}

		[Fact]
		public void ShorteningDoctypeIsCorrect()
		{
			// Arrange
			var minifier = new XhtmlMinifier(new XhtmlMinificationSettings(true)
			{
				UseShortDoctype = true
			});

			const string input = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\"\r\n" +
				"   \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
			const string targetOutput = "<!DOCTYPE html>";

			// Act
			string output = minifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput, output);
		}

		#endregion

		#region Processing custom boolean attributes

		[Fact]
		public void ProcessingCustomBooleanAttributesIsCorrect()
		{
			// Arrange
			var minifier = new XhtmlMinifier(new XhtmlMinificationSettings(true));

			const string input1 = "<div custom-attribute></div>";
			const string targetOutput1 = "<div custom-attribute=\"\"></div>";

			const string input2 = "<div class></div>";
			const string targetOutput2 = "<div class=\"\"></div>";

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
		}

		#endregion
	}
}