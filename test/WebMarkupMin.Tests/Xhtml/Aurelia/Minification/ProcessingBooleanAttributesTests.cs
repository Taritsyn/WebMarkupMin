using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Xhtml.Aurelia.Minification
{
	public class ProcessingBooleanAttributesTests
	{
		[Fact]
		public void ProcessingCustomBooleanAttributes()
		{
			// Arrange
			var minifier = new XhtmlMinifier(new XhtmlMinificationSettings(true));

			const string input = "<!doctype html>\n" +
				"<html>\n" +
				"	<head>\n" +
				"		<title>Aurelia</title>\n" +
				"		<link rel=\"stylesheet\" href=\"styles/styles.css\" />\n" +
				"		<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\" />\n" +
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
			const string targetOutput = "<!doctype html>\n" +
				"<html>\n" +
				"	<head>\n" +
				"		<title>Aurelia</title>\n" +
				"		<link rel=\"stylesheet\" href=\"styles/styles.css\" />\n" +
				"		<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\" />\n" +
				"	</head>\n" +
				"	<body aurelia-app=\"\">\n" +
				"		<script src=\"jspm_packages/system.js\"></script>\n" +
				"		<script src=\"config.js\"></script>\n" +
				"		<script>\n" +
				"			SystemJS.import('aurelia-bootstrapper');\n" +
				"		</script>\n" +
				"	</body>\n" +
				"</html>"
				;

			// Act
			string output = minifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput, output);
		}
	}
}