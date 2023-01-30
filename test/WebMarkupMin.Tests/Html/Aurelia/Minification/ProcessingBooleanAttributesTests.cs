using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Aurelia.Minification
{
	public class ProcessingBooleanAttributesTests
	{
		[Fact]
		public void ProcessingCustomBooleanAttributes()
		{
			// Arrange
			var keepingBooleanAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { CollapseBooleanAttributes = false });
			var collapsingBooleanAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { CollapseBooleanAttributes = true });

			const string input = "<!doctype html>\n" +
				"<html>\n" +
				"	<head>\n" +
				"		<title>Aurelia</title>\n" +
				"		<link rel=\"stylesheet\" href=\"styles/styles.css\">\n" +
				"		<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\n" +
				"	</head>\n" +
				"	<body aurelia-app>\n" +
				"		<script src=\"jspm_packages/system.js\"></script>\n" +
				"		<script src=\"config.js\"></script>\n" +
				"		<script>\n" +
				"			SystemJS.import('aurelia-bootstrapper');\n" +
				"		</script>\n" +
				"	</body>\n" +
				"</html>"
				;

			// Act
			string outputA = keepingBooleanAttributesMinifier.Minify(input).MinifiedContent;
			string outputB = collapsingBooleanAttributesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(input, outputA);
			Assert.Equal(input, outputB);
		}
	}
}