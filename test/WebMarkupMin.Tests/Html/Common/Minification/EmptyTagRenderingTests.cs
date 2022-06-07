using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class EmptyTagRenderingTests
	{
		[Fact]
		public void EmptyTagRenderingIsCorrect()
		{
			// Arrange
			var emptyTagWithoutSlashMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { EmptyTagRenderMode = HtmlEmptyTagRenderMode.NoSlash });
			var emptyTagWithSlashMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { EmptyTagRenderMode = HtmlEmptyTagRenderMode.Slash });
			var emptyTagWithSpaceAndSlashMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { EmptyTagRenderMode = HtmlEmptyTagRenderMode.SpaceAndSlash });

			const string input1 = "<img src=\"/images/0.gif\">";
			const string targetOutput1A = "<img src=\"/images/0.gif\">";
			const string targetOutput1B = "<img src=\"/images/0.gif\"/>";
			const string targetOutput1C = "<img src=\"/images/0.gif\" />";

			const string input2 = "<br/>";
			const string targetOutput2A = "<br>";
			const string targetOutput2B = "<br/>";
			const string targetOutput2C = "<br />";

			const string input3 = "<hr />";
			const string targetOutput3A = "<hr>";
			const string targetOutput3B = "<hr/>";
			const string targetOutput3C = "<hr />";

			const string input4 = "<svg width=\"220\" height=\"220\">" +
				"<circle cx=\"110\" cy=\"110\" r=\"100\" fill=\"#fafaa2\" stroke=\"#000\" />" +
				"<rect x=\"10\" y=\"10\" width=\"200\" height=\"200\" fill=\"#fafaa2\" stroke=\"#000\"/>" +
				"</svg>"
				;
			const string targetOutput4A = "<svg width=\"220\" height=\"220\">" +
				"<circle cx=\"110\" cy=\"110\" r=\"100\" fill=\"#fafaa2\" stroke=\"#000\" />" +
				"<rect x=\"10\" y=\"10\" width=\"200\" height=\"200\" fill=\"#fafaa2\" stroke=\"#000\" />" +
				"</svg>"
				;
			const string targetOutput4B = "<svg width=\"220\" height=\"220\">" +
				"<circle cx=\"110\" cy=\"110\" r=\"100\" fill=\"#fafaa2\" stroke=\"#000\"/>" +
				"<rect x=\"10\" y=\"10\" width=\"200\" height=\"200\" fill=\"#fafaa2\" stroke=\"#000\"/>" +
				"</svg>"
				;
			const string targetOutput4C = targetOutput4A;

			const string input5 = "<div>" +
				"<math>" +
				"<apply>" +
				"<plus/>" +
				"<apply>" +
				"<times />" +
				"<ci>a</ci>" +
				"<apply>" +
				"<power/>" +
				"<ci>x</ci>" +
				"<cn>2</cn>" +
				"</apply>" +
				"</apply>" +
				"<apply>" +
				"<times />" +
				"<ci>b</ci>" +
				"<ci>x</ci>" +
				"</apply>" +
				"<ci>c</ci>" +
				"</apply>" +
				"</math>" +
				"</div>"
				;
			const string targetOutput5A = "<div>" +
				"<math>" +
				"<apply>" +
				"<plus />" +
				"<apply>" +
				"<times />" +
				"<ci>a</ci>" +
				"<apply>" +
				"<power />" +
				"<ci>x</ci>" +
				"<cn>2</cn>" +
				"</apply>" +
				"</apply>" +
				"<apply>" +
				"<times />" +
				"<ci>b</ci>" +
				"<ci>x</ci>" +
				"</apply>" +
				"<ci>c</ci>" +
				"</apply>" +
				"</math>" +
				"</div>"
				;
			const string targetOutput5B = "<div>" +
				"<math>" +
				"<apply>" +
				"<plus/>" +
				"<apply>" +
				"<times/>" +
				"<ci>a</ci>" +
				"<apply>" +
				"<power/>" +
				"<ci>x</ci>" +
				"<cn>2</cn>" +
				"</apply>" +
				"</apply>" +
				"<apply>" +
				"<times/>" +
				"<ci>b</ci>" +
				"<ci>x</ci>" +
				"</apply>" +
				"<ci>c</ci>" +
				"</apply>" +
				"</math>" +
				"</div>"
				;
			const string targetOutput5C = targetOutput5A;

			// Act
			string output1A = emptyTagWithoutSlashMinifier.Minify(input1).MinifiedContent;
			string output1B = emptyTagWithSlashMinifier.Minify(input1).MinifiedContent;
			string output1C = emptyTagWithSpaceAndSlashMinifier.Minify(input1).MinifiedContent;

			string output2A = emptyTagWithoutSlashMinifier.Minify(input2).MinifiedContent;
			string output2B = emptyTagWithSlashMinifier.Minify(input2).MinifiedContent;
			string output2C = emptyTagWithSpaceAndSlashMinifier.Minify(input2).MinifiedContent;

			string output3A = emptyTagWithoutSlashMinifier.Minify(input3).MinifiedContent;
			string output3B = emptyTagWithSlashMinifier.Minify(input3).MinifiedContent;
			string output3C = emptyTagWithSpaceAndSlashMinifier.Minify(input3).MinifiedContent;

			string output4A = emptyTagWithoutSlashMinifier.Minify(input4).MinifiedContent;
			string output4B = emptyTagWithSlashMinifier.Minify(input4).MinifiedContent;
			string output4C = emptyTagWithSpaceAndSlashMinifier.Minify(input4).MinifiedContent;

			string output5A = emptyTagWithoutSlashMinifier.Minify(input5).MinifiedContent;
			string output5B = emptyTagWithSlashMinifier.Minify(input5).MinifiedContent;
			string output5C = emptyTagWithSpaceAndSlashMinifier.Minify(input5).MinifiedContent;

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

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);
		}
	}
}