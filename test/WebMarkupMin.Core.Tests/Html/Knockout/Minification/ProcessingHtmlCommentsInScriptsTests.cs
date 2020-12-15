using Xunit;

namespace WebMarkupMin.Core.Tests.Html.Knockout.Minification
{
	public class ProcessingHtmlCommentsInScriptsTests
	{
		[Fact]
		public void ProcessingHtmlCommentsInScriptsIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));
			var removingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlCommentsFromScriptsAndStyles = true });

			const string input = "<script type=\"text/html\">\r\n" +
				"<!--ko with: coords-->\r\n" +
				"	Latitude: <span data-bind=\"text: latitude\"></span>,\r\n" +
				"	Longitude: <span data-bind=\"text: longitude\"></span>\r\n" +
				"<!--/ko-->\r\n" +
				"</script>"
				;

			// Assert
			string outputA = minifier.Minify(input).MinifiedContent;
			string outputB = removingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = removingHtmlCommentsMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(input, outputA);
			Assert.Equal(input, outputB);
			Assert.Equal(input, outputC);
		}
	}
}