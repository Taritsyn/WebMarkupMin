using Xunit;

namespace WebMarkupMin.Core.Test.Html.Blazor.Minification
{
	public class ProcessingComponentMarkersTests
	{
		[Fact]
		public void ProcessingPrerenderedServerComponentMarkersIsCorrect()
		{
			// Arrange
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = true });

			const string input = "    <app>\n" +
				"        <!--Blazor:{\"sequence\":0,\"type\":\"server\"," +
				"\"prerenderId\":\"06e9ae1e55c3437eb41a4c1d63cd9dc8\"," +
				"\"descriptor\":\"CfDJ8NK75dCQyrlElvJvfLSrrguC1bsLxXsAuW0q1g/H0zl" +
				"\u002Bp\u002B\u002B/TqOa8setXFvAD\u002Bz9A9MdprpBvSN557bPtkPK5YlCmXwocCGGDrR3O3QN" +
				"\u002BcwXhn0sAYeIUqaj2\u002BYyQXKeetXKhsX4j5OCYaH\u002B3UJAguu8" +
				"\u002B\u002BxsuGS8CUGjJwwhbfUC5wcmo4LziUz7XMrTYXaYEOMmzFA7Eq5ved31kdZZaOFECuhYfpY" +
				"\u002BgsmXo1c\u002BTiyf6ZX2vCKycTBtAPxWMT3O01Bh8171scgfPRUWlSm0iV5WgTc9hkv9x/bpQtOz0LcC" +
				"\u002BDH/MVr3fKNRagRNX/azUfUei45AhSBobLliNugCSYAPBn1v9vtoD9nqXy0ciFLn\"}-->\n" +
				"    <div>\n" +
				"        <h1>Counter</h1>\n\n" +
				"<p>Current count: 0</p>\n\n" +
				"<button class=\"btn btn-primary\">Click me</button>\n" +
				"    </div>\n" +
				"    <!--Blazor:{\"prerenderId\":\"06e9ae1e55c3437eb41a4c1d63cd9dc8\"}-->\n" +
				"    </app>"
				;

			// Act
			string output = removingHtmlCommentsMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(input, output);
		}

		[Fact]
		public void ProcessingServerComponentMarkersIsCorrect()
		{
			// Arrange
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = true });

			const string input = "    <app>\n" +
				"        <!--Blazor:{\"sequence\":0,\"type\":\"server\"," +
				"\"descriptor\":\"CfDJ8NK75dCQyrlElvJvfLSrrgsusP7rGOvnC/" +
				"WXFUytyBYGcQSY7IMzMiXKdOuDQ/fhxDAKzXeNdf3NeExaW2C" +
				"\u002BqOKNVNTyILq87lxEbrgxr3Gh\u002BKW\u002BL0SYKtGgqMs4CGa" +
				"\u002ByvFM5bTecRi8uEEs/szWltI3muNRxRuleATXIqrUXr" +
				"\u002B2SGcyuMnqgdXIaqyiyUPdeXn5bgTxKO9gqFoYIJHXvA8YRNUoY7lv40l1oUMsRNVVD/" +
				"GHXWOwRhpsHehXvx2Vbz/eAbvSDOjFJ2UuVMuIPXOxN1GUj7gEpzvLNK9uRfarj" +
				"\u002BoV9ajUx3lz4Bk0o9uRRR2aDgevrG7hvTOXmg7bNt1CrsRW38pGmXs1vAvo\u002BI3G\"}-->\n" +
				"    </app>"
				;

			// Act
			string output = removingHtmlCommentsMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(input, output);
		}
	}
}