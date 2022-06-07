using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Angular1.Minification
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

			const string input = "<script type=\"text/ng-template\">\r\n" +
				"	<!--directive:date-picker -->\r\n" +
				"</script>"
				;

			// Act
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