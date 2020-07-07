using System.Collections.Generic;

using Xunit;

namespace WebMarkupMin.Core.Test.Html.Minification
{
	public class RemovingTagsWithoutContentTests
	{
		[Fact]
		public void RemovingTagsWithoutContentIsCorrect()
		{
			// Arrange
			var removingTagsWithoutContentMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveTagsWithoutContent = true });

			const string input1 = "<p>Some text...</p>";

			const string input2 = "<p></p>";
			const string targetOutput2 = "";

			const string input3 = "<p>Some text...<span>Some other text...</span><span></span></p>";
			const string targetOutput3 = "<p>Some text...<span>Some other text...</span></p>";

			const string input4 = "<a href=\"http://example/com\" title=\"Some title...\"></a>";
			const string targetOutput4 = "";

			const string input5 = "<textarea cols=\"80\" rows=\"10\"></textarea>";
			const string input6 = "<div>Hello, <span>%Username%</span>!</div>";

			const string input7 = "<div>one<div>two <div>three</div><div></div>four</div>five</div>";
			const string targetOutput7 = "<div>one<div>two <div>three</div>four</div>five</div>";

			const string input8 = "<img src=\"\">";
			const string input9 = "<p><!-- Some comment... --></p>";
			const string input10 = "<p>Some text...<span title=\"Some title...\" class=\"caret\"></span></p>";

			const string input11 = "<p>Some text...<span title=\"Some title...\" class=\"\"></span></p>";
			const string targetOutput11 = "<p>Some text...</p>";

			const string input12 = "<p>Some text...<span title=\"Some title...\" id=\"label\"></span></p>";

			const string input13 = "<p>Some text...<span title=\"Some title...\" id=\"\"></span></p>";
			const string targetOutput13 = "<p>Some text...</p>";

			const string input14 = "<p>Some text...<a title=\"Some title...\" name=\"anchor\"></a></p>";

			const string input15 = "<p>Some text...<a title=\"Some title...\" name=\"\"></a></p>";
			const string targetOutput15 = "<p>Some text...</p>";

			const string input16 = "<p>Some text...<span title=\"Some title...\" data-description=\"Some description...\"></span></p>";
			const string input17 = "<p>Some text...<span title=\"Some title...\" data-description=\"\"></span></p>";
			const string input18 = "<div>" +
				"<svg width=\"220\" height=\"220\">" +
				"<circle cx=\"110\" cy=\"110\" r=\"100\" fill=\"#fafaa2\" stroke=\"#000\" />" +
				"</svg>" +
				"</div>"
				;
			const string input19 = "<div>" +
				"<math><infinity /></math>" +
				"</div>"
				;
			const string input20 = "<div role=\"contentinfo\"></div>";

			const string input21 = "<div role=\"\"></div>";
			const string targetOutput21 = "";

			const string input22 = "<div custom-attribute=\"\"></div>";

			const string input23 = "<div>\n" +
				"	<p>	\n  </p>\n" +
				"</div>"
				;
			const string targetOutput23 = "<div>\n" +
				"	\n" +
				"</div>"
				;

			const string input24 = "<style></style>";
			const string targetOutput24 = "";

			const string input25 = "<script></script>";
			const string targetOutput25 = "";

			const string input26 = "<div>\n" +
				"	<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"100\" height=\"50\"></svg>\n" +
				"</div>"
				;
			const string targetOutput26 = "<div>\n" +
				"	\n" +
				"</div>"
				;

			const string input27 = "<div>\n" +
				"	<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"100\" height=\"50\">\n" +
				"		<text x=\"10\" y=\"20\" transform=\"rotate(30 20,40)\"></text>\n" +
				"	</svg>\n" +
				"</div>"
				;

			// Act
			string output1 = removingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;
			string output2 = removingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;
			string output3 = removingTagsWithoutContentMinifier.Minify(input3).MinifiedContent;
			string output4 = removingTagsWithoutContentMinifier.Minify(input4).MinifiedContent;
			string output5 = removingTagsWithoutContentMinifier.Minify(input5).MinifiedContent;
			string output6 = removingTagsWithoutContentMinifier.Minify(input6).MinifiedContent;
			string output7 = removingTagsWithoutContentMinifier.Minify(input7).MinifiedContent;
			string output8 = removingTagsWithoutContentMinifier.Minify(input8).MinifiedContent;
			string output9 = removingTagsWithoutContentMinifier.Minify(input9).MinifiedContent;
			string output10 = removingTagsWithoutContentMinifier.Minify(input10).MinifiedContent;
			string output11 = removingTagsWithoutContentMinifier.Minify(input11).MinifiedContent;
			string output12 = removingTagsWithoutContentMinifier.Minify(input12).MinifiedContent;
			string output13 = removingTagsWithoutContentMinifier.Minify(input13).MinifiedContent;
			string output14 = removingTagsWithoutContentMinifier.Minify(input14).MinifiedContent;
			string output15 = removingTagsWithoutContentMinifier.Minify(input15).MinifiedContent;
			string output16 = removingTagsWithoutContentMinifier.Minify(input16).MinifiedContent;
			string output17 = removingTagsWithoutContentMinifier.Minify(input17).MinifiedContent;
			string output18 = removingTagsWithoutContentMinifier.Minify(input18).MinifiedContent;
			string output19 = removingTagsWithoutContentMinifier.Minify(input19).MinifiedContent;
			string output20 = removingTagsWithoutContentMinifier.Minify(input20).MinifiedContent;
			string output21 = removingTagsWithoutContentMinifier.Minify(input21).MinifiedContent;
			string output22 = removingTagsWithoutContentMinifier.Minify(input22).MinifiedContent;
			string output23 = removingTagsWithoutContentMinifier.Minify(input23).MinifiedContent;
			string output24 = removingTagsWithoutContentMinifier.Minify(input24).MinifiedContent;
			string output25 = removingTagsWithoutContentMinifier.Minify(input25).MinifiedContent;
			string output26 = removingTagsWithoutContentMinifier.Minify(input26).MinifiedContent;
			string output27 = removingTagsWithoutContentMinifier.Minify(input27).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(input5, output5);
			Assert.Equal(input6, output6);
			Assert.Equal(targetOutput7, output7);
			Assert.Equal(input8, output8);
			Assert.Equal(input9, output9);
			Assert.Equal(input10, output10);
			Assert.Equal(targetOutput11, output11);
			Assert.Equal(input12, output12);
			Assert.Equal(targetOutput13, output13);
			Assert.Equal(input14, output14);
			Assert.Equal(targetOutput15, output15);
			Assert.Equal(input16, output16);
			Assert.Equal(input17, output17);
			Assert.Equal(input18, output18);
			Assert.Equal(input19, output19);
			Assert.Equal(input20, output20);
			Assert.Equal(targetOutput21, output21);
			Assert.Equal(input22, output22);
			Assert.Equal(targetOutput23, output23);
			Assert.Equal(targetOutput24, output24);
			Assert.Equal(targetOutput25, output25);
			Assert.Equal(targetOutput26, output26);
			Assert.Equal(input27, output27);
		}
	}
}