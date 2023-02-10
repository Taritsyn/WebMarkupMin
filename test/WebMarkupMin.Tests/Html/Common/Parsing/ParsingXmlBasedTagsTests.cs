using System;

using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Parsing
{
	public class ParsingXmlBasedTagsTests : IDisposable
	{
		private HtmlMinifier _minifier;


		public ParsingXmlBasedTagsTests()
		{
			_minifier = new HtmlMinifier(new HtmlMinificationSettings(true));
		}


		[Fact]
		public void ParsingSvgTag()
		{
			// Arrange
			const string input = "<svg width=\"150\" height=\"100\" viewBox=\"0 0 3 2\">" +
				"<rect width=\"1\" height=\"2\" x=\"0\" fill=\"#008d46\" />" +
				"<rect width=\"1\" height=\"2\" x=\"1\" fill=\"#ffffff\" />" +
				"<rect width=\"1\" height=\"2\" x=\"2\" fill=\"#d2232c\" />" +
				"</svg>"
				;

			// Act
			string output = _minifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(input, output);
		}

		[Fact]
		public void ParsingNestedSvgTags()
		{
			// Arrange
			const string input = "<svg xmlns=\"http://www.w3.org/2000/svg\">\n" +
				"    <!-- some SVG content -->\n" +
				"    <svg>\n" +
				"        <!-- some inner SVG content -->\n" +
				"    </svg>\n\n" +
				"    <svg />\n\n" +
				"    <svg>\n" +
				"        <!-- other inner SVG content -->\n" +
				"    </svg>\n" +
				"</svg>"
				;

			// Act
			string output = _minifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(input, output);
		}

		[Fact]
		public void ParsingMathTag()
		{
			// Arrange
			const string input1 = "<math>" +
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
			const string input2 = "<math>" +
				"<ms><![CDATA[x<y]]></ms>" +
				"<mo>+</mo>" +
				"<mn>3</mn>" +
				"<mo>=</mo>" +
				"<ms><![CDATA[x<y3]]></ms>" +
				"</math>"
				;

			// Act
			string output1 = _minifier.Minify(input1).MinifiedContent;
			string output2 = _minifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(input2, output2);
		}

		#region IDisposable implementation

		public void Dispose()
		{
			_minifier = null;
		}

		#endregion
	}
}