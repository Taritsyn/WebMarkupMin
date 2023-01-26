using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class ProcessingHtmlCommentsTests
	{
		[Fact]
		public void RemovingHtmlCommentsIsCorrect()
		{
			// Arrange
			var keepingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = false });
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = true });

			const string input1 = "<!---->";
			const string targetOutput1A = input1;
			const string targetOutput1B = "";

			const string input2 = "<!-- -->";
			const string targetOutput2A = input2;
			const string targetOutput2B = "";

			const string input3 = "<!-- Some comment... -->";
			const string targetOutput3A = input3;
			const string targetOutput3B = "";

			const string input4 = "<!-- Initial comment... -->" +
				"<div>Some text...</div>" +
				"<!-- Final comment\n\n Some other comments ... -->"
				;
			const string targetOutput4A = input4;
			const string targetOutput4B = "<div>Some text...</div>";

			const string input5 = "<p title=\"&lt;!-- Some comment... -->\">Some text...</p>";

			const string input6 = "<div>\n" +
				"\t<svg width=\"150\" height=\"100\" viewBox=\"0 0 3 2\">\n" +
				"\t\t<!-- SVG content -->\n" +
				"\t\t<rect width=\"1\" height=\"2\" x=\"0\" fill=\"#008d46\" />\n" +
				"\t\t<rect width=\"1\" height=\"2\" x=\"1\" fill=\"#ffffff\" />\n" +
				"\t\t<rect width=\"1\" height=\"2\" x=\"2\" fill=\"#d2232c\" />\n" +
				"\t\t<!-- /SVG content -->\n" +
				"\t</svg>\n" +
				"</div>"
				;
			const string targetOutput6A = input6;
			const string targetOutput6B = "<div>\n" +
				"\t<svg width=\"150\" height=\"100\" viewBox=\"0 0 3 2\">\n" +
				"\t\t\n" +
				"\t\t<rect width=\"1\" height=\"2\" x=\"0\" fill=\"#008d46\" />\n" +
				"\t\t<rect width=\"1\" height=\"2\" x=\"1\" fill=\"#ffffff\" />\n" +
				"\t\t<rect width=\"1\" height=\"2\" x=\"2\" fill=\"#d2232c\" />\n" +
				"\t\t\n" +
				"\t</svg>\n" +
				"</div>";

			const string input7 = "<div>\n" +
				"\t<math>\n" +
				"\t\t<!-- MathML content -->\n" +
				"\t\t<mrow>\n" +
				"\t\t\t<mrow>\n" +
				"\t\t\t\t<msup>\n" +
				"\t\t\t\t\t<mi>a</mi>\n" +
				"\t\t\t\t\t<mn>2</mn>\n" +
				"\t\t\t\t</msup>\n" +
				"\t\t\t\t<mo>+</mo>\n" +
				"\t\t\t\t<msup>\n" +
				"\t\t\t\t\t<mi>b</mi>\n" +
				"\t\t\t\t\t<mn>2</mn>\n" +
				"\t\t\t\t</msup>\n" +
				"\t\t\t</mrow>\n" +
				"\t\t\t<mo>=</mo>\n" +
				"\t\t\t<msup>\n" +
				"\t\t\t\t<mi>c</mi>\n" +
				"\t\t\t\t<mn>2</mn>\n" +
				"\t\t\t</msup>\n" +
				"\t\t</mrow>\n" +
				"\t\t<!-- /MathML content -->\n" +
				"\t</math>\n" +
				"</div>"
			;
			const string targetOutput7A = input7;
			const string targetOutput7B = "<div>\n" +
				"\t<math>\n" +
				"\t\t\n" +
				"\t\t<mrow>\n" +
				"\t\t\t<mrow>\n" +
				"\t\t\t\t<msup>\n" +
				"\t\t\t\t\t<mi>a</mi>\n" +
				"\t\t\t\t\t<mn>2</mn>\n" +
				"\t\t\t\t</msup>\n" +
				"\t\t\t\t<mo>+</mo>\n" +
				"\t\t\t\t<msup>\n" +
				"\t\t\t\t\t<mi>b</mi>\n" +
				"\t\t\t\t\t<mn>2</mn>\n" +
				"\t\t\t\t</msup>\n" +
				"\t\t\t</mrow>\n" +
				"\t\t\t<mo>=</mo>\n" +
				"\t\t\t<msup>\n" +
				"\t\t\t\t<mi>c</mi>\n" +
				"\t\t\t\t<mn>2</mn>\n" +
				"\t\t\t</msup>\n" +
				"\t\t</mrow>\n" +
				"\t\t\n" +
				"\t</math>\n" +
				"</div>"
			;

			const string input8 = "<!--wmm:ignore-->Some text...<!--/wmm:ignore-->";
			const string targetOutput8A = "Some text...";
			const string targetOutput8B = targetOutput8A;

			// Act
			string output1A = keepingHtmlCommentsMinifier.Minify(input1).MinifiedContent;
			string output1B = removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingHtmlCommentsMinifier.Minify(input2).MinifiedContent;
			string output2B = removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingHtmlCommentsMinifier.Minify(input3).MinifiedContent;
			string output3B = removingHtmlCommentsMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingHtmlCommentsMinifier.Minify(input4).MinifiedContent;
			string output4B = removingHtmlCommentsMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingHtmlCommentsMinifier.Minify(input5).MinifiedContent;
			string output5B = removingHtmlCommentsMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingHtmlCommentsMinifier.Minify(input6).MinifiedContent;
			string output6B = removingHtmlCommentsMinifier.Minify(input6).MinifiedContent;

			string output7A = keepingHtmlCommentsMinifier.Minify(input7).MinifiedContent;
			string output7B = removingHtmlCommentsMinifier.Minify(input7).MinifiedContent;

			string output8A = keepingHtmlCommentsMinifier.Minify(input8).MinifiedContent;
			string output8B = removingHtmlCommentsMinifier.Minify(input8).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);

			Assert.Equal(input5, output5A);
			Assert.Equal(input5, output5B);

			Assert.Equal(targetOutput6A, output6A);
			Assert.Equal(targetOutput6B, output6B);

			Assert.Equal(targetOutput7A, output7A);
			Assert.Equal(targetOutput7B, output7B);

			Assert.Equal(targetOutput8A, output8A);
			Assert.Equal(targetOutput8B, output8B);
		}

		[Fact]
		public void ProcessingNoindexCommentsIsCorrect()
		{
			// Arrange
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = true });

			const string input1 = "<div class=\"selector\">\n" +
				"<!--noindex--><a href=\"?orderBy=relevance\" rel=\"nofollow\">Sort by relevance</a><!--/noindex-->\n" +
				"</div>"
				;

			const string input2 = "<div class=\"selector\">\n" +
				"<!--NOINDEX--><a href=\"?orderBy=relevance\" rel=\"nofollow\">Sort by relevance</a><!--/NOINDEX-->\n" +
				"</div>"
				;
			const string targetOutput2 = "<div class=\"selector\">\n" +
				"<!--noindex--><a href=\"?orderBy=relevance\" rel=\"nofollow\">Sort by relevance</a><!--/noindex-->\n" +
				"</div>"
				;

			// Act
			string output1 = removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;
			string output2 = removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(targetOutput2, output2);
		}
	}
}