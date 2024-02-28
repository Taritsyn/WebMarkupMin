using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class ProcessingBooleanAttributesTests
	{
		[Fact]
		public void CollapsingBooleanAttributes()
		{
			// Arrange
			var collapsingBooleanAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { CollapseBooleanAttributes = true });

			const string input1 = "<input disabled=\"disabled\">";
			const string targetOutput1 = "<input disabled>";

			const string input2 = "<input CHECKED = \"checked\" readonly=\"readonly\">";
			const string targetOutput2 = "<input checked readonly>";

			const string input3 = "<option name=\"fixed\" selected=\"selected\">Fixed</option>";
			const string targetOutput3 = "<option name=\"fixed\" selected>Fixed</option>";

			const string input4 = "<script nomodule=\"nomodule\" src=\"/legacy.js\"></script>";
			const string targetOutput4 = "<script nomodule src=\"/legacy.js\"></script>";

			// Act
			string output1 = collapsingBooleanAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = collapsingBooleanAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = collapsingBooleanAttributesMinifier.Minify(input3).MinifiedContent;
			string output4 = collapsingBooleanAttributesMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
		}

		[Fact]
		public void ProcessingCustomBooleanAttributes()
		{
			// Arrange
			var keepingBooleanAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { CollapseBooleanAttributes = false });
			var collapsingBooleanAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { CollapseBooleanAttributes = true });

			const string input1 = "<div custom-attribute></div>";
			const string targetOutput1 = input1;

			const string input2 = "<div class></div>";
			const string targetOutput2 = "<div class=\"\"></div>";

			// Act
			string output1A = keepingBooleanAttributesMinifier.Minify(input1).MinifiedContent;
			string output1B = collapsingBooleanAttributesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingBooleanAttributesMinifier.Minify(input2).MinifiedContent;
			string output2B = collapsingBooleanAttributesMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1A);
			Assert.Equal(targetOutput1, output1B);

			Assert.Equal(targetOutput2, output2A);
			Assert.Equal(targetOutput2, output2B);
		}
	}
}