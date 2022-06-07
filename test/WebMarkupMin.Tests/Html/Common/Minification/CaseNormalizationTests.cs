using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class CaseNormalizationTests
	{
		[Fact]
		public void CaseNormalizationIsCorrect()
		{
			// Arrange
			var preservingCaseMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { PreserveCase = true });
			var changingCaseMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { PreserveCase = false });

			const string input1 = "<P>Some text...</p>";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<p>Some text...</p>";

			const string input2 = "<DIV>Some text..</DIV>";
			const string targetOutput2A = input2;
			const string targetOutput2B = "<div>Some text..</div>";

			const string input3 = "<DIV title=\"Some title...\">Some text..</DiV>";
			const string targetOutput3A = input3;
			const string targetOutput3B = "<div title=\"Some title...\">Some text..</div>";

			const string input4 = "<DIV TITLE=\"Some title...\">Some text..</DIV>";
			const string targetOutput4A = input4;
			const string targetOutput4B = "<div title=\"Some title...\">Some text..</div>";

			const string input5 = "<DIV tItLe=\"Some title...\">Some text..</DIV>";
			const string targetOutput5A = input5;
			const string targetOutput5B = "<div title=\"Some title...\">Some text..</div>";

			const string input6 = "<DiV tItLe=\"Some title...\">Some text..</DIV>";
			const string targetOutput6A = input6;
			const string targetOutput6B = "<div title=\"Some title...\">Some text..</div>";

			// Act
			string output1A = preservingCaseMinifier.Minify(input1).MinifiedContent;
			string output1B = changingCaseMinifier.Minify(input1).MinifiedContent;

			string output2A = preservingCaseMinifier.Minify(input2).MinifiedContent;
			string output2B = changingCaseMinifier.Minify(input2).MinifiedContent;

			string output3A = preservingCaseMinifier.Minify(input3).MinifiedContent;
			string output3B = changingCaseMinifier.Minify(input3).MinifiedContent;

			string output4A = preservingCaseMinifier.Minify(input4).MinifiedContent;
			string output4B = changingCaseMinifier.Minify(input4).MinifiedContent;

			string output5A = preservingCaseMinifier.Minify(input5).MinifiedContent;
			string output5B = changingCaseMinifier.Minify(input5).MinifiedContent;

			string output6A = preservingCaseMinifier.Minify(input6).MinifiedContent;
			string output6B = changingCaseMinifier.Minify(input6).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);

			Assert.Equal(targetOutput6A, output6A);
			Assert.Equal(targetOutput6B, output6B);
		}
	}
}