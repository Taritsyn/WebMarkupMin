using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class AttributeQuotesRenderingTests
	{
		[Fact]
		public void AttributeQuotesRendering()
		{
			// Arrange
			const string input1 = "<label for='author'>Author:</label><br>\n" +
				"<input type=text id=author name=author value=\"Vasya &quot;Monster&quot; Pupkin\"><br>\n" +
				"<label for=\"title\">Title:</label><br>\n" +
				"<input type=text id=title name=title value='HTML: Using the &apos; and &quot; characters'><br>\n" +
				"<label for=\"publishingHouse\">Publishing house:</label><br>\n" +
				"<input type=text id=publishingHouse name=publishingHouse value=\'O&apos;Hare Media\'><br>"
				;
			const string targetOutput1A = "<label for='author'>Author:</label><br>\n" +
				"<input type='text' id='author' name='author' value=\"Vasya &#34;Monster&#34; Pupkin\"><br>\n" +
				"<label for=\"title\">Title:</label><br>\n" +
				"<input type='text' id='title' name='title' value='HTML: Using the &#39; and \" characters'><br>\n" +
				"<label for=\"publishingHouse\">Publishing house:</label><br>\n" +
				"<input type='text' id='publishingHouse' name='publishingHouse' value='O&#39;Hare Media'><br>"
				;
			const string targetOutput1B = "<label for='author'>Author:</label><br>\n" +
				"<input type='text' id='author' name='author' value='Vasya \"Monster\" Pupkin'><br>\n" +
				"<label for=\"title\">Title:</label><br>\n" +
				"<input type='text' id='title' name='title' value='HTML: Using the &#39; and \" characters'><br>\n" +
				"<label for=\"publishingHouse\">Publishing house:</label><br>\n" +
				"<input type='text' id='publishingHouse' name='publishingHouse' value=\"O'Hare Media\"><br>"
				;
			const string targetOutput1C = "<label for='author'>Author:</label><br>\n" +
				"<input type='text' id='author' name='author' value='Vasya \"Monster\" Pupkin'><br>\n" +
				"<label for='title'>Title:</label><br>\n" +
				"<input type='text' id='title' name='title' value='HTML: Using the &#39; and \" characters'><br>\n" +
				"<label for='publishingHouse'>Publishing house:</label><br>\n" +
				"<input type='text' id='publishingHouse' name='publishingHouse' value='O&#39;Hare Media'><br>"
				;
			const string targetOutput1D = "<label for=\"author\">Author:</label><br>\n" +
				"<input type=\"text\" id=\"author\" name=\"author\" value=\"Vasya &#34;Monster&#34; Pupkin\"><br>\n" +
				"<label for=\"title\">Title:</label><br>\n" +
				"<input type=\"text\" id=\"title\" name=\"title\" value=\"HTML: Using the ' and &#34; characters\"><br>\n" +
				"<label for=\"publishingHouse\">Publishing house:</label><br>\n" +
				"<input type=\"text\" id=\"publishingHouse\" name=\"publishingHouse\" value=\"O'Hare Media\"><br>"
				;

			const string input2 = "<a href=clock.html><img src=clock.png width=400 height=398 alt=></a>";
			const string targetOutput2A = "<a href=\"clock.html\"><img src=\"clock.png\" width=\"400\" " +
				"height=\"398\" alt=\"\"></a>";
			const string targetOutput2B = targetOutput2A;
			const string targetOutput2C = "<a href='clock.html'><img src='clock.png' width='400' height='398' " +
				"alt=''></a>";
			const string targetOutput2D = targetOutput2A;

			var autoAttributeQuoteStyleMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { AttributeQuotesStyle = HtmlAttributeQuotesStyle.Auto });
			var optimalAttributeQuoteStyleMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { AttributeQuotesStyle = HtmlAttributeQuotesStyle.Optimal });
			var singleAttributeQuoteStyleMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { AttributeQuotesStyle = HtmlAttributeQuotesStyle.Single });
			var doubleAttributeQuoteStyleMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { AttributeQuotesStyle = HtmlAttributeQuotesStyle.Double });

			// Act
			string output1A = autoAttributeQuoteStyleMinifier.Minify(input1).MinifiedContent;
			string output1B = optimalAttributeQuoteStyleMinifier.Minify(input1).MinifiedContent;
			string output1C = singleAttributeQuoteStyleMinifier.Minify(input1).MinifiedContent;
			string output1D = doubleAttributeQuoteStyleMinifier.Minify(input1).MinifiedContent;

			string output2A = autoAttributeQuoteStyleMinifier.Minify(input2).MinifiedContent;
			string output2B = optimalAttributeQuoteStyleMinifier.Minify(input2).MinifiedContent;
			string output2C = singleAttributeQuoteStyleMinifier.Minify(input2).MinifiedContent;
			string output2D = doubleAttributeQuoteStyleMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);
			Assert.Equal(targetOutput1D, output1D);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);
			Assert.Equal(targetOutput2D, output2D);
		}
	}
}