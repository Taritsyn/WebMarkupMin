using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.React.Minification
{
	public class ProcessingDomComponentCommentsTests
	{
		[Fact]
		public void ProcessingDomEmptyComponentComments()
		{
			// Arrange
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = true });

			const string input = "<div id=\"react_IdJDeXYi107bZJLAy6IecB\">\n" +
				"	<div data-reactroot=\"\" data-reactid=\"1\">\n" +
				"		<!-- react-empty: 2 -->\n" +
				"		<div data-reactid=\"3\"></div>\n" +
				"	</div>\n" +
				"</div>"
				;

			// Act
			string output = removingHtmlCommentsMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(input, output);
		}

		[Fact]
		public void ProcessingDomTextComponentComments()
		{
			// Arrange
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = true });

			const string input1 = "<a href=\"/tournament\" data-reactid=\"19\">" +
				"<!-- react-text: 20 --> <!-- /react-text -->" +
				"<span class=\"title\" data-reactid=\"21\">Кто станет победителем нашего турнира?</span>" +
				"</a>"
				;
			const string input2 = "<span data-reactid=\"39\">" +
				"<!-- react-text: 40 -->— <!-- /react-text -->" +
				"<a href=\"/news/543/\" class=\"highlighted\" data-reactid=\"41\">читать далее</a>" +
				"</span>"
				;
			const string input3 = "<div class=\"more-button\" data-reactid=\"87\">" +
				"<!-- react-text: 88 -->Ещё <!-- /react-text -->" +
				"<span class=\"caret-down\" data-reactid=\"89\"></span>" +
				"</div>"
				;

			// Act
			string output1 = removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;
			string output2 = removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;
			string output3 = removingHtmlCommentsMinifier.Minify(input3).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(input2, output2);
			Assert.Equal(input3, output3);
		}
	}
}