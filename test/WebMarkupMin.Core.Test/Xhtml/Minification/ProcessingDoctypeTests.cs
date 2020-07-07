using Xunit;

namespace WebMarkupMin.Core.Test.Xhtml.Minification
{
	public class ProcessingDoctypeTests
	{
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
	}
}