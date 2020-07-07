using Xunit;

namespace WebMarkupMin.Core.Test.Html.Minification
{
	public class RemovingEmptyAttributesTests
	{
		[Fact]
		public void RemovingEmptyAttributesIsCorrect()
		{
			// Arrange
			var keepingAllEmptyAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveEmptyAttributes = false });
			var removingEmptyAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveEmptyAttributes = true });
			var keepingSomeEmptyAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					RemoveEmptyAttributes = true,
					PreservableAttributeList = "[style], [lang], [dir], [onmousedown], [onmouseup], " +
						"input[onchange], input[value], form[action]"
				}
			);

			const string input1 = "<p id=\"\" class=\"\" STYLE=\" \" title=\"\n\" lang=\"\" dir=\"\">Some text…</p>";
			const string targetOutput1A = "<p id=\"\" class=\"\" style=\"\" title=\"\n\" lang=\"\" dir=\"\">Some text…</p>";
			const string targetOutput1B = "<p dir=\"\">Some text…</p>";
			const string targetOutput1C = "<p style=\"\" lang=\"\" dir=\"\">Some text…</p>";

			const string input2 = "<p onclick=\"\"   ondblclick=\" \" onmousedown=\"\" ONMOUSEUP=\"\" " +
				"onmouseover=\" \" onmousemove=\"\" onmouseout=\"\" " +
				"onkeypress=\n\n  \"\n     \" onkeydown=\n\"\" onkeyup\n=\"\">Some text…</p>";
			const string targetOutput2A = "<p onclick=\"\" ondblclick=\"\" onmousedown=\"\" onmouseup=\"\" " +
				"onmouseover=\"\" onmousemove=\"\" onmouseout=\"\" " +
				"onkeypress=\"\" onkeydown=\"\" onkeyup=\"\">Some text…</p>";
			const string targetOutput2B = "<p>Some text…</p>";
			const string targetOutput2C = "<p onmousedown=\"\" onmouseup=\"\">Some text…</p>";

			const string input3 = "<input onfocus=\"\" onblur=\"\" onchange=\" \" value=\" Some value… \">";
			const string targetOutput3A = "<input onfocus=\"\" onblur=\"\" onchange=\"\" value=\" Some value… \">";
			const string targetOutput3B = "<input value=\" Some value… \">";
			const string targetOutput3C = "<input onchange=\"\" value=\" Some value… \">";

			const string input4 = "<input name=\"Some Name…\" value=\"\">";
			const string targetOutput4A = input4;
			const string targetOutput4B = "<input name=\"Some Name…\">";
			const string targetOutput4C = input4;

			const string input5 = "<img src=\"\" alt=\"\">";

			const string input6 = "<form action=\"\">Some controls…</form>";
			const string targetOutput6A = input6;
			const string targetOutput6B = "<form>Some controls…</form>";
			const string targetOutput6C = input6;

			// Act
			string output1A = keepingAllEmptyAttributesMinifier.Minify(input1).MinifiedContent;
			string output1B = removingEmptyAttributesMinifier.Minify(input1).MinifiedContent;
			string output1C = keepingSomeEmptyAttributesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingAllEmptyAttributesMinifier.Minify(input2).MinifiedContent;
			string output2B = removingEmptyAttributesMinifier.Minify(input2).MinifiedContent;
			string output2C = keepingSomeEmptyAttributesMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingAllEmptyAttributesMinifier.Minify(input3).MinifiedContent;
			string output3B = removingEmptyAttributesMinifier.Minify(input3).MinifiedContent;
			string output3C = keepingSomeEmptyAttributesMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingAllEmptyAttributesMinifier.Minify(input4).MinifiedContent;
			string output4B = removingEmptyAttributesMinifier.Minify(input4).MinifiedContent;
			string output4C = keepingSomeEmptyAttributesMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingAllEmptyAttributesMinifier.Minify(input5).MinifiedContent;
			string output5B = removingEmptyAttributesMinifier.Minify(input5).MinifiedContent;
			string output5C = keepingSomeEmptyAttributesMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingAllEmptyAttributesMinifier.Minify(input6).MinifiedContent;
			string output6B = removingEmptyAttributesMinifier.Minify(input6).MinifiedContent;
			string output6C = keepingSomeEmptyAttributesMinifier.Minify(input6).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);

			Assert.Equal(input5, output5A);
			Assert.Equal(input5, output5B);
			Assert.Equal(input5, output5C);

			Assert.Equal(targetOutput6A, output6A);
			Assert.Equal(targetOutput6B, output6B);
			Assert.Equal(targetOutput6C, output6C);
		}
	}
}