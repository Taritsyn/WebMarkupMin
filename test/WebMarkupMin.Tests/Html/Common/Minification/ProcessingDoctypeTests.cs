using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class ProcessingDoctypeTests
	{
		[Fact]
		public void ProcessingDoctype()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\"\n" +
				"   \"http://www.w3.org/TR/html4/strict.dtd\">";
			const string targetOutput1 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\"" +
				" \"http://www.w3.org/TR/html4/strict.dtd\">";

			const string input2 = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\"" +
				" \"http://www.w3.org/TR/html4/loose.dtd\">";
			const string targetOutput2 = input2;

			const string input3 = "<!DOCTYPE\n" +
				"    HTML PUBLIC \"-//W3C//DTD HTML 4.01 Frameset//EN\"\n" +
				"    \"http://www.w3.org/TR/html4/frameset.dtd\">"
				;
			const string targetOutput3 = "<!DOCTYPE" +
				" HTML PUBLIC \"-//W3C//DTD HTML 4.01 Frameset//EN\"" +
				" \"http://www.w3.org/TR/html4/frameset.dtd\">"
				;

			const string input4 = "<!DOCTYPE html>";
			const string targetOutput4 = input4;

			const string input5 = "<!DOCTYPE\r\nhtml>";
			const string targetOutput5 = "<!DOCTYPE html>";

			const string input6 = "<!DOCTYPE\thtml>";
			const string targetOutput6 = "<!DOCTYPE html>";

			const string input7 = "<!DOCTYPE HTML>";
			const string targetOutput7 = input7;

			const string input8 = "<!doctype html>";
			const string targetOutput8 = input8;

			const string input9 = "<!doctypehtml>";
			const string targetOutput9 = input9;

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			string output3 = minifier.Minify(input3).MinifiedContent;
			string output4 = minifier.Minify(input4).MinifiedContent;
			string output5 = minifier.Minify(input5).MinifiedContent;
			string output6 = minifier.Minify(input6).MinifiedContent;
			string output7 = minifier.Minify(input7).MinifiedContent;
			string output8 = minifier.Minify(input8).MinifiedContent;
			string output9 = minifier.Minify(input9).MinifiedContent;

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
		}

		[Fact]
		public void ShorteningDoctype()
		{
			// Arrange
			var emptyDoctypeMinifier = new HtmlMinifier(new HtmlMinificationSettings(true)
			{
				CustomShortDoctype = string.Empty,
				UseShortDoctype = true
			});
			var canonicalDoctypeMinifier = new HtmlMinifier(new HtmlMinificationSettings(true)
			{
				CustomShortDoctype = "<!DOCTYPE html>",
				UseShortDoctype = true
			});
			var firstNonStandardDoctypeMinifier = new HtmlMinifier(new HtmlMinificationSettings(true)
			{
				CustomShortDoctype = "<!DOCTYPE HTML>",
				UseShortDoctype = true
			});
			var secondNonStandardDoctypeMinifier = new HtmlMinifier(new HtmlMinificationSettings(true)
			{
				CustomShortDoctype = "<!doctype html>",
				UseShortDoctype = true
			});
			var thirdNonStandardDoctypeMinifier = new HtmlMinifier(new HtmlMinificationSettings(true)
			{
				CustomShortDoctype = "<!doctypehtml>",
				UseShortDoctype = true
			});

			const string input = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\"\r\n" +
				"   \"http://www.w3.org/TR/html4/loose.dtd\">";
			const string targetOutput1 = "<!DOCTYPE html>";
			const string targetOutput2 = "<!DOCTYPE html>";
			const string targetOutput3 = "<!DOCTYPE HTML>";
			const string targetOutput4 = "<!doctype html>";
			const string targetOutput5 = "<!doctypehtml>";

			// Act
			string output1 = emptyDoctypeMinifier.Minify(input).MinifiedContent;
			string output2 = canonicalDoctypeMinifier.Minify(input).MinifiedContent;
			string output3 = firstNonStandardDoctypeMinifier.Minify(input).MinifiedContent;
			string output4 = secondNonStandardDoctypeMinifier.Minify(input).MinifiedContent;
			string output5 = thirdNonStandardDoctypeMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
		}
	}
}