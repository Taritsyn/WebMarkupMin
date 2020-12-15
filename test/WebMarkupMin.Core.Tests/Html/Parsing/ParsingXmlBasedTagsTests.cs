using Xunit;

namespace WebMarkupMin.Core.Tests.Html.Parsing
{
	public class ParsingXmlBasedTagsTests
	{
		[Fact]
		public void ParsingXmlBasedTagsIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<svg width=\"150\" height=\"100\" viewBox=\"0 0 3 2\">" +
				"<rect width=\"1\" height=\"2\" x=\"0\" fill=\"#008d46\" />" +
				"<rect width=\"1\" height=\"2\" x=\"1\" fill=\"#ffffff\" />" +
				"<rect width=\"1\" height=\"2\" x=\"2\" fill=\"#d2232c\" />" +
				"</svg>"
				;
			const string input2 = "<math>" +
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
				"</math>"
				;
			const string input3 = "<math>" +
				"<ms><![CDATA[x<y]]></ms>" +
				"<mo>+</mo>" +
				"<mn>3</mn>" +
				"<mo>=</mo>" +
				"<ms><![CDATA[x<y3]]></ms>" +
				"</math>"
				;

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			string output3 = minifier.Minify(input3).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(input2, output2);
			Assert.Equal(input3, output3);
		}
	}
}