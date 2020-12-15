using Xunit;

namespace WebMarkupMin.Core.Tests.Html.Minification
{
	public class SpaceNormalizationTests
	{
		[Fact]
		public void SpaceNormalizationBetweenAttributesIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<p title=\"Some title...\">Some text...</p>";

			const string input2 = "<p title = \"Some title...\">Some text...</p>";
			const string targetOutput2 = "<p title=\"Some title...\">Some text...</p>";

			const string input3 = "<p title\n\n\t  =\n     \"Some title...\">Some text...</p>";
			const string targetOutput3 = "<p title=\"Some title...\">Some text...</p>";

			const string input4 = "<input title=\"Some title...\"       id=\"txtName\"    value=\"Some text...\">";
			const string targetOutput4 = "<input title=\"Some title...\" id=\"txtName\" value=\"Some text...\">";

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			string output3 = minifier.Minify(input3).MinifiedContent;
			string output4 = minifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
		}
	}
}