using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Parsing
{
	public class ParsingNonTrivialMarkupTests
	{
		[Fact]
		public void ParsingNonTrivialMarkupIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<p title=\"&lt;/p>\">Some text...</p>";
			const string input2 = "<p title=\" &lt;!-- Some comment... --> \">Some text...</p>";
			const string input3 = "<p title=\" &lt;![CDATA[\tSome data...\t]]> \">Some text...</p>";

			const string input4 = "<button data-dismiss=alert>Some text...</button>";
			const string targetOutput4 = "<button data-dismiss=\"alert\">Some text...</button>";

			const string input5 = "<option xml:lang=ru value=ru lang=ru>" +
				"&#x420;&#x43E;&#x441;&#x441;&#x438;&#x44F; (Russia)</option>";
			const string targetOutput5 = "<option xml:lang=\"ru\" value=\"ru\" lang=\"ru\">" +
				"&#x420;&#x43E;&#x441;&#x441;&#x438;&#x44F; (Russia)</option>";

			const string input6 = "<aside>" +
				"	<div id=\"about\">" +
				"		<div class=\"bl\">" +
				"			<div class=\"br\">" +
				"				<div class=\"tl\">" +
				"					<div class=\"tr\">" +
				"						<h4>Some header</h4>" +
				"						<p>Some text...</p>" +
				"					</div>" +
				"				</div>" +
				"			</div>" +
				"		</div>" +
				"	</div>" +
				"</aside>"
				;
			const string input7 = "<script>alert(\"<!--\")</script>";
			const string input8 = "<script>alert(\"<!-- Some text -->\")</script>";
			const string input9 = "<script>alert(\"-->\")</script>";

			const string input10 = "<a href=\"http://www.example.com/weather\"" +
				"class=\"b-link\"onclick=\"showWeatherWidget()\">Weather</a>";
			const string targetOutput10 = "<a href=\"http://www.example.com/weather\"" +
				" class=\"b-link\" onclick=\"showWeatherWidget()\">Weather</a>";

			const string input11 = "<p>Some text...";
			const string targetOutput11 = "<p>Some text...</p>";

			const string input12 = "<div class=></div>";
			const string targetOutput12 = "<div class=\"\"></div>";

			const string input13 = "<div class></div>";
			const string targetOutput13 = "<div class=\"\"></div>";

			const string input14 = "<g:plusone size=\"medium\" href=\"http://www.example.com/blog/WebWorkers.aspx\">" +
				"</g:plusone>";
			const string input15 = "<fb:like-box href=\"https://www.facebook.com/ExampleGroup\"" +
				" width=\"250\" height=\"168\" show_faces=\"true\" stream=\"false\" header=\"false\"></fb:like-box>";

			const string input16 = "<script type=\"text/javascript\"></script >";
			const string targetOutput16 = "<script type=\"text/javascript\"></script>";

			const string input17 = "<html>" +
				"<head>" +
				"<title>Some title…</title>" +
				"<body>" +
				"<p><strong>Welcome!</strong></p>"
				;
			const string targetOutput17 = "<html>" +
				"<head>" +
				"<title>Some title…</title>" +
				"</head>" +
				"<body>" +
				"<p><strong>Welcome!</strong></p>" +
				"</body>" +
				"</html>"
				;

			const string input18 = "<meta http-equiv=\"X-UA-Compatible\" content=\"chrome=1\">\n" +
				"<link href='http://example.com/rss' rel='alternate' type='application/rss+xml' " +
				"Вакансии — ФИГАЧИМ='Все вакансии'>\n" +
				"<title>Вакансии — ФИГАЧИМ</title>"
				;
			const string targetOutput18 = "<meta http-equiv=\"X-UA-Compatible\" content=\"chrome=1\">\n" +
				"<link href=\"http://example.com/rss\" rel=\"alternate\" type=\"application/rss+xml\" " +
				"вакансии — фигачим=\"Все вакансии\">\n" +
				"<title>Вакансии — ФИГАЧИМ</title>"
				;

			const string input19 = "<a href=\"/?source=upload-main\" class=\"b-link\"[object Object]>" +
				"<i class=\"b-ico b-ico-upload\"></i>" +
				"</a>"
				;
			const string targetOutput19 = "<a href=\"/?source=upload-main\" class=\"b-link\" [object object]>" +
				"<i class=\"b-ico b-ico-upload\"></i>" +
				"</a>"
				;

			const string input20 = "<form action='/s/ref=nb_sb_noss' method='get' role='search'" +
				" accept-charset='utf-8', class='nav-searchbar-inner'></form>"
				;
			const string targetOutput20 = "<form action=\"/s/ref=nb_sb_noss\" method=\"get\" role=\"search\"" +
				" accept-charset=\"utf-8\" , class=\"nav-searchbar-inner\"></form>"
				;

			const string input21 = "<head><td>m<head>l><html><body><h1>h1</h1><p>p</p></body></html>";
			const string targetOutput21 = "<head><td>m<head>l><html></html></head><body><h1>h1</h1>" +
				"<p>p</p></body></td></head>";

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			string output3 = minifier.Minify(input3).MinifiedContent;
			string output4 = minifier.Minify(input4).MinifiedContent;
			string output5 = minifier.Minify(input5).MinifiedContent;
			string output6 = minifier.Minify(input6).MinifiedContent;
			string output7 = minifier.Minify(input7).MinifiedContent;
			string output8 = minifier.Minify(input8).MinifiedContent;
			string output9 = minifier.Minify(input9).MinifiedContent;
			string output10 = minifier.Minify(input10).MinifiedContent;
			string output11 = minifier.Minify(input11).MinifiedContent;
			string output12 = minifier.Minify(input12).MinifiedContent;
			string output13 = minifier.Minify(input13).MinifiedContent;
			string output14 = minifier.Minify(input14).MinifiedContent;
			string output15 = minifier.Minify(input15).MinifiedContent;
			string output16 = minifier.Minify(input16).MinifiedContent;
			string output17 = minifier.Minify(input17).MinifiedContent;
			string output18 = minifier.Minify(input18).MinifiedContent;
			string output19 = minifier.Minify(input19).MinifiedContent;
			string output20 = minifier.Minify(input20).MinifiedContent;
			string output21 = minifier.Minify(input21).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(input2, output2);
			Assert.Equal(input3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
			Assert.Equal(input6, output6);
			Assert.Equal(input7, output7);
			Assert.Equal(input8, output8);
			Assert.Equal(input9, output9);
			Assert.Equal(targetOutput10, output10);
			Assert.Equal(targetOutput11, output11);
			Assert.Equal(targetOutput12, output12);
			Assert.Equal(targetOutput13, output13);
			Assert.Equal(input14, output14);
			Assert.Equal(input15, output15);
			Assert.Equal(targetOutput16, output16);
			Assert.Equal(targetOutput17, output17);
			Assert.Equal(targetOutput18, output18);
			Assert.Equal(targetOutput19, output19);
			Assert.Equal(targetOutput20, output20);
			Assert.Equal(targetOutput21, output21);
		}

		[Fact]
		public void ParsingNestedScriptTagsIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input = "<script type=\"text/html\"> \n" +
				"	<!--\n" +
				"	<script>alert(\"Hello!\");</script>\n" +
				"	-->\n\n" +
				"	<!--\n" +
				"		<script type=\"text/javascript\">\n" +
				"		var years = prompt('How old are you?', 100);\n" +
				"		alert('You are ' + years + ' years old!');\n" +
				"		</script>\n" +
				"	-->\n\n" +
				"	<!--\n" +
				"		<script type=\"application/javascript\">\n" +
				"		var isAdmin = confirm(\"Are you an administrator?\");\n" +
				"		alert( isAdmin );\n" +
				"		</script>\n" +
				"	--> \n" +
				" \n" +
				"</script>"
				;

			// Act
			string output = minifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(input, output);
		}
	}
}