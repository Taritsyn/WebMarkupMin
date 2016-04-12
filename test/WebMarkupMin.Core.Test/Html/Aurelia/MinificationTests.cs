using Xunit;

namespace WebMarkupMin.Core.Test.Html.Aurelia
{
	public class MinificationTests
	{
		#region Removing tags without content

		[Fact]
		public void RemovingTagsWithoutContentIsCorrect()
		{
			// Arrange
			var removingTagsWithoutContentMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveTagsWithoutContent = true });

			const string input1 = "<div aurelia-app></div>";
			const string input2 = "<router-view></router-view>";

			// Act
			string output1 = removingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;
			string output2 = removingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(input2, output2);
		}

		#endregion

		#region Collapsing boolean attributes

		[Fact]
		public void ProcessingCustomBooleanAttributesIsCorrect()
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

		#endregion
	}
}