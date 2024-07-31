using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class ProcessingHtmlCommentsTests
	{
		[Fact]
		public void RemovingHtmlComments()
		{
			// Arrange
			var keepingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = false });
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = true });
			var keepingSomeHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					RemoveHtmlComments = true,
					PreservableHtmlCommentList = @"
						/^\s*saved from url=\(\d+\)/i, 
						/^\s*Site-Version:\s*\d{4}\.\d{1,2}\.\d{1,2}\.\d{1,3}\s*$/, 
						/^\s*Site\.UI-Version: \d+\.\d+\.\d+\.\d+\s*$/,
						/^\s*Last Published:/, 
						/^\/?\$$/, 
						/^[\[\]]$/
						"
				}
			);

			const string input1 = "<!---->";
			const string targetOutput1A = input1;
			const string targetOutput1B = "";
			const string targetOutput1C = "";

			const string input2 = "<!-- -->";
			const string targetOutput2A = input2;
			const string targetOutput2B = "";
			const string targetOutput2C = "";

			const string input3 = "<!-- Some comment... -->";
			const string targetOutput3A = input3;
			const string targetOutput3B = "";
			const string targetOutput3C = "";

			const string input4 = "<!-- Initial comment... -->" +
				"<div>Some text...</div>" +
				"<!-- Final comment\n\n Some other comments ... -->"
				;
			const string targetOutput4A = input4;
			const string targetOutput4B = "<div>Some text...</div>";
			const string targetOutput4C = targetOutput4B;

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
				"</div>"
				;
			const string targetOutput6C = targetOutput6B;

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
			const string targetOutput7C = targetOutput7B;

			const string input8 = "<!-- saved from URL=(0029)https://html.spec.whatwg.org/ -->";
			const string targetOutput8A = input8;
			const string targetOutput8B = "";
			const string targetOutput8C = input8;

			const string input9 = "<!-- Site-Version: 2024.7.2.510 -->";
			const string targetOutput9A = input9;
			const string targetOutput9B = "";
			const string targetOutput9C = input9;

			const string input10 = "<!-- Site.UI-Version: 6.1.1.0 -->";
			const string targetOutput10A = input10;
			const string targetOutput10B = "";
			const string targetOutput10C = input10;

			const string input11 = "<!-- Last Published: Sun May 19 2024 19:49:58 GMT+0000 (Coordinated Universal Time) -->";
			const string targetOutput11A = input11;
			const string targetOutput11B = "";
			const string targetOutput11C = input11;

			const string input12 = "<!--$-->\n" +
				"<section class=\"SimpleSectionWidgetBody-sc-1hnj5vy-0 gRcNTb\">\n" +
				"    <!--$-->\n" +
				"    <div class=\"ContainerWidgetBody-sc-1afjli6-1 fFpMyW\"></div>\n" +
				"    <!--/$-->\n" +
				"</section>\n" +
				"<!--/$-->"
				;
			const string targetOutput12A = input12;
			const string targetOutput12B = "\n" +
				"<section class=\"SimpleSectionWidgetBody-sc-1hnj5vy-0 gRcNTb\">\n" +
				"    \n" +
				"    <div class=\"ContainerWidgetBody-sc-1afjli6-1 fFpMyW\"></div>\n" +
				"    \n" +
				"</section>\n"
				;
			const string targetOutput12C = input12;

			const string input13 = "<!--[--><span>Read more &rarr;</span><!--]-->";
			const string targetOutput13A = input13;
			const string targetOutput13B = "<span>Read more &rarr;</span>";
			const string targetOutput13C = input13;

			// Act
			string output1A = keepingHtmlCommentsMinifier.Minify(input1).MinifiedContent;
			string output1B = removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;
			string output1C = keepingSomeHtmlCommentsMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingHtmlCommentsMinifier.Minify(input2).MinifiedContent;
			string output2B = removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;
			string output2C = keepingSomeHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingHtmlCommentsMinifier.Minify(input3).MinifiedContent;
			string output3B = removingHtmlCommentsMinifier.Minify(input3).MinifiedContent;
			string output3C = keepingSomeHtmlCommentsMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingHtmlCommentsMinifier.Minify(input4).MinifiedContent;
			string output4B = removingHtmlCommentsMinifier.Minify(input4).MinifiedContent;
			string output4C = keepingSomeHtmlCommentsMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingHtmlCommentsMinifier.Minify(input5).MinifiedContent;
			string output5B = removingHtmlCommentsMinifier.Minify(input5).MinifiedContent;
			string output5C = keepingSomeHtmlCommentsMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingHtmlCommentsMinifier.Minify(input6).MinifiedContent;
			string output6B = removingHtmlCommentsMinifier.Minify(input6).MinifiedContent;
			string output6C = keepingSomeHtmlCommentsMinifier.Minify(input6).MinifiedContent;

			string output7A = keepingHtmlCommentsMinifier.Minify(input7).MinifiedContent;
			string output7B = removingHtmlCommentsMinifier.Minify(input7).MinifiedContent;
			string output7C = keepingSomeHtmlCommentsMinifier.Minify(input7).MinifiedContent;

			string output8A = keepingHtmlCommentsMinifier.Minify(input8).MinifiedContent;
			string output8B = removingHtmlCommentsMinifier.Minify(input8).MinifiedContent;
			string output8C = keepingSomeHtmlCommentsMinifier.Minify(input8).MinifiedContent;

			string output9A = keepingHtmlCommentsMinifier.Minify(input9).MinifiedContent;
			string output9B = removingHtmlCommentsMinifier.Minify(input9).MinifiedContent;
			string output9C = keepingSomeHtmlCommentsMinifier.Minify(input9).MinifiedContent;

			string output10A = keepingHtmlCommentsMinifier.Minify(input10).MinifiedContent;
			string output10B = removingHtmlCommentsMinifier.Minify(input10).MinifiedContent;
			string output10C = keepingSomeHtmlCommentsMinifier.Minify(input10).MinifiedContent;

			string output11A = keepingHtmlCommentsMinifier.Minify(input11).MinifiedContent;
			string output11B = removingHtmlCommentsMinifier.Minify(input11).MinifiedContent;
			string output11C = keepingSomeHtmlCommentsMinifier.Minify(input11).MinifiedContent;

			string output12A = keepingHtmlCommentsMinifier.Minify(input12).MinifiedContent;
			string output12B = removingHtmlCommentsMinifier.Minify(input12).MinifiedContent;
			string output12C = keepingSomeHtmlCommentsMinifier.Minify(input12).MinifiedContent;

			string output13A = keepingHtmlCommentsMinifier.Minify(input13).MinifiedContent;
			string output13B = removingHtmlCommentsMinifier.Minify(input13).MinifiedContent;
			string output13C = keepingSomeHtmlCommentsMinifier.Minify(input13).MinifiedContent;

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

			Assert.Equal(targetOutput7A, output7A);
			Assert.Equal(targetOutput7B, output7B);
			Assert.Equal(targetOutput7C, output7C);

			Assert.Equal(targetOutput8A, output8A);
			Assert.Equal(targetOutput8B, output8B);
			Assert.Equal(targetOutput8C, output8C);

			Assert.Equal(targetOutput9A, output9A);
			Assert.Equal(targetOutput9B, output9B);
			Assert.Equal(targetOutput9C, output9C);

			Assert.Equal(targetOutput10A, output10A);
			Assert.Equal(targetOutput10B, output10B);
			Assert.Equal(targetOutput10C, output10C);

			Assert.Equal(targetOutput11A, output11A);
			Assert.Equal(targetOutput11B, output11B);
			Assert.Equal(targetOutput11C, output11C);

			Assert.Equal(targetOutput12A, output12A);
			Assert.Equal(targetOutput12B, output12B);
			Assert.Equal(targetOutput12C, output12C);

			Assert.Equal(targetOutput13A, output13A);
			Assert.Equal(targetOutput13B, output13B);
			Assert.Equal(targetOutput13C, output13C);
		}

		[Fact]
		public void ProcessingIgnoringCommentTags()
		{
			// Arrange
			var keepingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = false });
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = true });

			const string input = "<!--wmm:ignore-->Some text...<!--/wmm:ignore-->";
			const string targetOutputA = "Some text...";
			const string targetOutputB = targetOutputA;

			// Act
			string outputA = keepingHtmlCommentsMinifier.Minify(input).MinifiedContent;
			string outputB = removingHtmlCommentsMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
		}

		[Fact]
		public void ProcessingNoindexComments()
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

		[Fact]
		public void ProcessingBlazorWasmPreRenderingBuildMarkers()
		{
			// Arrange
			var keepingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = false });
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = true });
			var keepingSomeHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					RemoveHtmlComments = true,
					PreservableHtmlCommentList = @" / %%-PRERENDERING(?:-HEADOUTLET)?-(?:BEGIN|END)-%% / "
				}
			);

			const string input1 = "<!-- %%-PRERENDERING-HEADOUTLET-BEGIN-%% -->\n" +
				"<title>Home | Blazor Wasm App 0</title>" +
				"<meta name=\"description\" content=\"This is a meta description.\">\n" +
				"<!-- %%-PRERENDERING-HEADOUTLET-END-%% -->"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "\n" +
				"<title>Home | Blazor Wasm App 0</title>" +
				"<meta name=\"description\" content=\"This is a meta description.\">\n"
				;
			const string targetOutput1C = input1;

			const string input2 = "<app>\n" +
				"    <div class=\"loading\">Loading...</div>\n" +
				"    \n" +
				"<!-- %%-PRERENDERING-BEGIN-%% -->\n" +
				"<div style=\"opacity: 0; position: fixed; z-index: -1; top: 0; left: 0; bottom: 0; right: 0\">\n" +
				"    \n\n\n" +
				"<h1>Home</h1>\n\n" +
				"<div><a href=\"/about\">about</a></div>\n" +
				"<div><a href=\"/lazy-loading-page\">lazy loading page</a></div>\n\n" +
				"<div class=\"environment\">\n" +
				"    Environment: Prerendering</div>\n\n" +
				"<div class=\"my-component\" b-2fr94ogw6o>    This component is defined in the " +
				"<strong b-2fr94ogw6o>RazorClassLib1</strong> library.\n" +
				"</div>\n\n" +
				"</div>\n" +
				"<!-- %%-PRERENDERING-END-%% -->\n" +
				"</app>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = "<app>\n" +
				"    <div class=\"loading\">Loading...</div>\n" +
				"    \n\n" +
				"<div style=\"opacity: 0; position: fixed; z-index: -1; top: 0; left: 0; bottom: 0; right: 0\">\n" +
				"    \n\n\n" +
				"<h1>Home</h1>\n\n" +
				"<div><a href=\"/about\">about</a></div>\n" +
				"<div><a href=\"/lazy-loading-page\">lazy loading page</a></div>\n\n" +
				"<div class=\"environment\">\n" +
				"    Environment: Prerendering</div>\n\n" +
				"<div class=\"my-component\" b-2fr94ogw6o>    This component is defined in the " +
				"<strong b-2fr94ogw6o>RazorClassLib1</strong> library.\n" +
				"</div>\n\n" +
				"</div>\n\n" +
				"</app>"
				;
			const string targetOutput2C = input2;

			// Act
			string output1A = keepingHtmlCommentsMinifier.Minify(input1).MinifiedContent;
			string output1B = removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;
			string output1C = keepingSomeHtmlCommentsMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingHtmlCommentsMinifier.Minify(input2).MinifiedContent;
			string output2B = removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;
			string output2C = keepingSomeHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);
		}
	}
}