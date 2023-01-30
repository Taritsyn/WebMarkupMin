using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Angular1.Minification
{
	public class WhitespaceMinificationTests
	{
		[Fact]
		public void WhitespaceMinification()
		{
			// Arrange
			var keepingWhitespaceMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.None,
					PreserveNewLines = false
				});
			var keepingWhitespaceAndNewLinesMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.None,
					PreserveNewLines = true
				});
			var safeRemovingWhitespaceMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Safe,
					PreserveNewLines = false
				});
			var safeRemovingWhitespaceExceptForNewLinesMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Safe,
					PreserveNewLines = true
				});
			var mediumRemovingWhitespaceMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Medium,
					PreserveNewLines = false
				});
			var mediumRemovingWhitespaceExceptForNewLinesMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Medium,
					PreserveNewLines = true
				});
			var aggressiveRemovingWhitespaceMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive,
					PreserveNewLines = false
				});
			var aggressiveRemovingWhitespaceExceptForNewLinesMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive,
					PreserveNewLines = true
				});

			const string input = "<ul>\n" +
				"	<li data-ng-repeat=\"customer in customers\">  " +
				"{{\t  customer.name  \t}}\t  -  \t{{\t  customer.city  \t}}  " +
				"</li>\n" +
				"</ul>"
				;
			const string targetOutputA = input;
			const string targetOutputB = input;
			const string targetOutputC = "<ul>" +
				"<li data-ng-repeat=\"customer in customers\"> " +
				"{{\t  customer.name  \t}} - {{\t  customer.city  \t}} " +
				"</li>" +
				"</ul>"
				;
			const string targetOutputD = "<ul>\n" +
				"<li data-ng-repeat=\"customer in customers\"> " +
				"{{\t  customer.name  \t}} - {{\t  customer.city  \t}} " +
				"</li>\n" +
				"</ul>"
				;
			const string targetOutputE = "<ul>" +
				"<li data-ng-repeat=\"customer in customers\">" +
				"{{\t  customer.name  \t}} - {{\t  customer.city  \t}}" +
				"</li>" +
				"</ul>"
				;
			const string targetOutputF = "<ul>\n" +
				"<li data-ng-repeat=\"customer in customers\">" +
				"{{\t  customer.name  \t}} - {{\t  customer.city  \t}}" +
				"</li>\n" +
				"</ul>"
				;
			const string targetOutputG = targetOutputE;
			const string targetOutputH = targetOutputF;

			// Act
			string outputA = keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = keepingWhitespaceAndNewLinesMinifier.Minify(input).MinifiedContent;
			string outputC = safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputE = mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputF = mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputG = aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputH = aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
			Assert.Equal(targetOutputE, outputE);
			Assert.Equal(targetOutputF, outputF);
			Assert.Equal(targetOutputG, outputG);
			Assert.Equal(targetOutputH, outputH);
		}
	}
}