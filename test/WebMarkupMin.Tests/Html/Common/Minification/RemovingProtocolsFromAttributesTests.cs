using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class RemovingProtocolsFromAttributesTests
	{
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
	}
}
