using Xunit;

namespace WebMarkupMin.Core.Tests.Html.Angular1.Minification
{
	public class WhitespaceMinificationTests
	{
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

			const string input = "<ul>\n" +
				"	<li data-ng-repeat=\"customer in customers\">  " +
				"{{\t  customer.name  \t}}\t  -  \t{{\t  customer.city  \t}}  " +
				"</li>\n" +
				"</ul>"
				;
			const string targetOutputA = input;
			const string targetOutputB = "<ul>" +
				"<li data-ng-repeat=\"customer in customers\"> " +
				"{{\t  customer.name  \t}} - {{\t  customer.city  \t}} " +
				"</li>" +
				"</ul>"
				;
			const string targetOutputC = "<ul>" +
				"<li data-ng-repeat=\"customer in customers\">" +
				"{{\t  customer.name  \t}} - {{\t  customer.city  \t}}" +
				"</li>" +
				"</ul>"
				;
			const string targetOutputD = "<ul>" +
				"<li data-ng-repeat=\"customer in customers\">" +
				"{{\t  customer.name  \t}} - {{\t  customer.city  \t}}" +
				"</li>" +
				"</ul>"
				;

			// Act
			string outputA = keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
		}
	}
}