using Xunit;

namespace WebMarkupMin.Core.Tests.Html.Minification
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
		public void ProcessingConditionalCommentsIsCorrect()
		{
			// Arrange
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = true });

			const string input1 = "<!--[if lt IE 9]>" +
				"<script src=\"Scripts/html5shiv.js\"></script>" +
				"<![endif]-->"
				;

			const string input2 = "<!--[if IEMobile 7]> <html class=\"no-js iem7\"> <![endif \t  ]-->\n" +
				"<!--[if lt IE 9 ]> <html class=\"no-js lte-ie8\"> <![endif  \n ]-->\n" +
				"<!--[if (gt IE 8)|(gt IEMobile 7)|!(IEMobile)|!(IE)]> \t\n  <!  \t\n --> <html class=\"no-js\" lang=\"en\"> <!-- \t\n  <![endif  \t\n ]-->\n"
				;
			const string targetOutput2 = "<!--[if IEMobile 7]> <html class=\"no-js iem7\"> <![endif]-->\n" +
				"<!--[if lt IE 9]> <html class=\"no-js lte-ie8\"> <![endif]-->\n" +
				"<!--[if (gt IE 8)|(gt IEMobile 7)|!(IEMobile)|!(IE)]><!--> <html class=\"no-js\" lang=\"en\"> <!--<![endif]-->\n" +
				"</html>"
				;

			const string input3 = "<!--[if IEMobile 7 ]> <html class=\"no-js iem7\"> <![endif]-->\n" +
				"<!--[if lt IE 9]> <html class=\"no-js lte-ie8\"> <![endif]-->\n" +
				"<!--[if (gt IE 8)|(gt IEMobile 7)|!(IEMobile)|!(IE)]><!--> <html class=\"no-js\" lang=\"en\"> <!--<![endif]-->\n"
				;
			const string targetOutput3 = "<!--[if IEMobile 7]> <html class=\"no-js iem7\"> <![endif]-->\n" +
				"<!--[if lt IE 9]> <html class=\"no-js lte-ie8\"> <![endif]-->\n" +
				"<!--[if (gt IE 8)|(gt IEMobile 7)|!(IEMobile)|!(IE)]><!--> <html class=\"no-js\" lang=\"en\"> <!--<![endif]-->\n" +
				"</html>"
				;

			const string input4 = "<!--[if lt IE 7 ]> <html class=\"no-js ie6 ielt8\"> <![endif]-->\n" +
				"<!--[if IE 7 ]> <html class=\"no-js ie7 ielt8\"> <![endif]-->\n" +
				"<!--[if IE 8 ]> <html class=\"no-js ie8\"> <![endif]-->\n" +
				"<!--[if IE 9 ]> <html class=\"no-js ie9\"> <![endif]-->\n" +
				"<!--[if (gt IE 9)|!(IE)]> \t\n  --> <html class=\"no-js\"> <!--  \t\n <![endif \t\n  ]-->\n"
				;
			const string targetOutput4 = "<!--[if lt IE 7]> <html class=\"no-js ie6 ielt8\"> <![endif]-->\n" +
				"<!--[if IE 7]> <html class=\"no-js ie7 ielt8\"> <![endif]-->\n" +
				"<!--[if IE 8]> <html class=\"no-js ie8\"> <![endif]-->\n" +
				"<!--[if IE 9]> <html class=\"no-js ie9\"> <![endif]-->\n" +
				"<!--[if (gt IE 9)|!(IE)]>--> <html class=\"no-js\"> <!--<![endif]-->\n" +
				"</html>"
				;

			const string input5 = "<!--[if lt IE 7]> <html class=\"no-js ie6 oldie\" " +
				"data-text-direction=\"left-to-right\">\n" +
				"<![endif  \t ]-->\n" +
				"<!--[if IE 7]> <html class=\"no-js ie7 oldie\" " +
				"data-text-direction=\"left-to-right\">\n" +
				"<![endif \n  ]-->\n" +
				"<!--[if IE 8]> <html class=\"no-js ie8 oldie\" " +
				"data-text-direction=\"left-to-right\">\n" +
				"<![endif]-->\n" +
				"<!--[if gt IE 8]>--> <html class=\"no-js\"" +
				"data-text-direction=\"left-to-right\">\n" +
				"<!--<![endif]-->\n"
				;
			const string targetOutput5 = "<!--[if lt IE 7]> <html class=\"no-js ie6 oldie\" " +
				"data-text-direction=\"left-to-right\">\n" +
				"<![endif]-->\n" +
				"<!--[if IE 7]> <html class=\"no-js ie7 oldie\" " +
				"data-text-direction=\"left-to-right\">\n" +
				"<![endif]-->\n" +
				"<!--[if IE 8]> <html class=\"no-js ie8 oldie\" " +
				"data-text-direction=\"left-to-right\">\n" +
				"<![endif]-->\n" +
				"<!--[if gt IE 8]>--> <html class=\"no-js\" " +
				"data-text-direction=\"left-to-right\">\n" +
				"<!--<![endif]-->\n" +
				"</html>"
				;

			const string input6 = "<!--[if IE 7]>\n" +
				"<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/site_ie7.css\" media=\"all\">\n" +
				"	<![endif]-->\n\n" +
				"<!--[if IE 6]>\n" +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/site_ie6.css\" media=\"all\">\n " +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/ie6_site_extras.css\" media=\"all\">\n" +
				"<![endif]-->\n\n" +
				"<!--[if IE 5]>\n" +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/site_ie5.css\" media=\"all\">\n" +
				"<![endif]-->"
				;

			const string input7 = "<!--[if IE 7 ]>\n" +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/site_ie7.css\" media=\"all\"/>\n" +
				"<![endif]-->\n\n" +
				"<!--[if IE 6 ]>\n" +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/site_ie6.css\" media=\"all\"/>\n" +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/ie6_site_extras.css\" media=\"all\"/>\n" +
				"<![endif]-->\n\n" +
				"<!--[if IE 5 ]>\n" +
				"	<link  \trel=\"stylesheet\" type=\"text/css\"  href=\"/Content/site_ie5.css\" media=\"all\"/>\n" +
				"<![endif]-->"
				;
			const string targetOutput7 = "<!--[if IE 7]>\n" +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/site_ie7.css\" media=\"all\">\n" +
				"<![endif]-->\n\n" +
				"<!--[if IE 6]>\n" +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/site_ie6.css\" media=\"all\">\n" +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/ie6_site_extras.css\" media=\"all\">\n" +
				"<![endif]-->\n\n" +
				"<!--[if IE 5]>\n" +
				"	<link rel=\"stylesheet\" type=\"text/css\" href=\"/Content/site_ie5.css\" media=\"all\">\n" +
				"<![endif]-->"
				;

			const string input8 = "<![if IE 5.0000]>\n" +
				"	<p>Welcome to Internet Explorer 5.0!</p>\n" +
				"<![endif \t  ]>\n" +
				"<![if IE 5]>\n" +
				"	<p>Welcome to any incremental version of Internet Explorer 5!</p>\n" +
				"<![endif  \n ]>\n" +
				"<![if lt IE 8]>\n" +
				"	<p class=\"text-warning\">Please upgrade to Internet Explorer version 8.</p>\n" +
				"<![endif]>"
				;
			const string targetOutput8 = "<![if IE 5.0000]>\n" +
				"	<p>Welcome to Internet Explorer 5.0!</p>\n" +
				"<![endif]>\n" +
				"<![if IE 5]>\n" +
				"	<p>Welcome to any incremental version of Internet Explorer 5!</p>\n" +
				"<![endif]>\n" +
				"<![if lt IE 8]>\n" +
				"	<p class=\"text-warning\">Please upgrade to Internet Explorer version 8.</p>\n" +
				"<![endif]>"
				;

			const string input9 = "<![if IE 5.0000 ]>\n" +
				"	<p>Welcome to Internet Explorer 5.0!</p>\n" +
				"<![endif \t  ]>\n" +
				"<![if IE 5 ]>\n" +
				"	<p>Welcome to any incremental version of Internet Explorer 5!</p>\n" +
				"<![endif  \n ]>\n" +
				"<![if lt IE 8]>\n" +
				"	<p class=\"text-warning\">Please upgrade to Internet Explorer version 8.</p>\n" +
				"<![endif]>"
				;
			const string targetOutput9 = "<![if IE 5.0000]>\n" +
				"	<p>Welcome to Internet Explorer 5.0!</p>\n" +
				"<![endif]>\n" +
				"<![if IE 5]>\n" +
				"	<p>Welcome to any incremental version of Internet Explorer 5!</p>\n" +
				"<![endif]>\n" +
				"<![if lt IE 8]>\n" +
				"	<p class=\"text-warning\">Please upgrade to Internet Explorer version 8.</p>\n" +
				"<![endif]>"
				;

			const string input10 = "<picture>\n" +
				"	<!--[if IE 9]><video style=\"display: none\"><![endif]-->\n" +
				"	<source srcset=\"examples/images/extralarge.jpg\" media=\"(min-width: 1000px)\">\n" +
				"	<source srcset=\"examples/images/large.jpg\" media=\"(min-width: 800px)\">\n" +
				"	<source srcset=\"examples/images/medium.jpg\">\n" +
				"	<!--[if IE 9]></video><![endif]-->\n" +
				"	<img srcset=\"examples/images/medium.jpg\">\n" +
				"</picture>"
				;

			const string input11 = "<!--[if lt IE 9]>\n" +
				"<script src=\"//ajax.aspnetcdn.com/ajax/jquery/jquery-1.11.1.min.js\"></script>\n" +
				"<script>\n" +
				"	window.jQuery || document.write('<script src=\"/scripts/jquery-1.11.1.min.js\"><\\/script>')\n" +
				"</script>\n" +
				"<![endif]-->\n" +
				"<!--[if gte IE 9]><!-->\n" +
				"<script src=\"//ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.1.min.js\"></script>\n" +
				"<script>\n" +
				"	window.jQuery || document.write('<script src=\"/scripts/jquery-2.1.1.min.js\"><\\/script>')\n" +
				"</script>\n" +
				"<!--<![endif]-->"
				;

			// Act
			string output1 = removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;
			string output2 = removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;
			string output3 = removingHtmlCommentsMinifier.Minify(input3).MinifiedContent;
			string output4 = removingHtmlCommentsMinifier.Minify(input4).MinifiedContent;
			string output5 = removingHtmlCommentsMinifier.Minify(input5).MinifiedContent;
			string output6 = removingHtmlCommentsMinifier.Minify(input6).MinifiedContent;
			string output7 = removingHtmlCommentsMinifier.Minify(input7).MinifiedContent;
			string output8 = removingHtmlCommentsMinifier.Minify(input8).MinifiedContent;
			string output9 = removingHtmlCommentsMinifier.Minify(input9).MinifiedContent;
			string output10 = removingHtmlCommentsMinifier.Minify(input10).MinifiedContent;
			string output11 = removingHtmlCommentsMinifier.Minify(input11).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
			Assert.Equal(input6, output6);
			Assert.Equal(targetOutput7, output7);
			Assert.Equal(targetOutput8, output8);
			Assert.Equal(targetOutput9, output9);
			Assert.Equal(input10, output10);
			Assert.Equal(input11, output11);
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