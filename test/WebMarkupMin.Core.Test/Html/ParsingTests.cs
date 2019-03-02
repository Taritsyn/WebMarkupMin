using System.Collections.Generic;

using Xunit;

namespace WebMarkupMin.Core.Test.Html
{
	public class ParsingTests
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

		[Fact]
		public void ProcessingInvalidCharactersInStartTagIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<img src=\"/images/39.gif\" width=80\" height=\"60\">";
			const string input2 = "<span class=\"b-form-input_size_l i-bem\"\">Some text...</span>";
			const string input3 = "<meta name=\"Description\" content=\"Информационно-аналитический портал про " +
				"недвижимость и квартиры в Москве и РФ \"Хата.ру\" – Аналитика и цены на недвижимость онлайн, " +
				"прогнозы, исследования, обзоры и статьи.\">"
				;
			const string input4 = "<img src=\"http://files.example.com/img/88900173ct.jpg\" width=\"100\" " +
				"title=\"Программирование на COBOL, 14-е издание, I том (файл PDF) \" / " +
				"alt=\"Программирование на COBOL, 14-е издание, I том (файл PDF)\" />"
				;
			const string input5 = "<A HREF='/cgi-bin/user.cgi?user=O'Connor'>O'Connor</A>";
			const string input6 = "<noscript>\r" +
				"	<div>\r" +
				"		<img src=\"//mk.tyndex.ru/watch/6740060\" style=\"position:absolute; left:-9999px;\" alt=\"\"\" />\r" +
				"	</div>\r" +
				"</noscript>"
				;
			const string input7 = "<table class='grayblock' style='width:100%'>\r\n" +
				"	<tbody>\r\n" +
				"		<tr valign='middle'>\r\n" +
				"			<td><a href='/r7165'><img width='55' border='0' height='55' " +
				"title=Материалы к годовому Общему собранию акционеров ОАО Рога и копыта' " +
				"alt='Материалы к годовому Общему собранию акционеров ОАО Рога и копыта'" +
				"src='/img/728/left-1.jpg' /></a></td>\r\n" +
				"			<td><a href='/r7165'>Материалы к годовому Общему собранию акционеров ОАО Рога и копыта</a></td>\r\n" +
				"		</tr>\r\n" +
				"	</tbody>\r\n" +
				"</table>"
				;
			const string input8 = "<li>\n" +
				"	<a data-bind=\"href:'@Url.Action(\"GetInvoices\", \"ListUsers\")'+'/?eMail='+ Login\">Invoices</a>\n" +
				"</li>"
				;
			const string input9 = "<header\"></header>";
			const string input10 = "<table>\n" +
				"	<tr>\n" +
				"		<td></td>\n" +
				"	</tr>\n" +
				"</table."
				;
			const string input11 = "<link id=\"favicon\" rel=?\"shortcut icon\" type=?\"image/?png\" href=?\"#\">";

			// Act
			IList<MinificationErrorInfo> errors1 = minifier.Minify(input1).Errors;
			IList<MinificationErrorInfo> errors2 = minifier.Minify(input2).Errors;
			IList<MinificationErrorInfo> errors3 = minifier.Minify(input3).Errors;
			IList<MinificationErrorInfo> errors4 = minifier.Minify(input4).Errors;
			IList<MinificationErrorInfo> errors5 = minifier.Minify(input5).Errors;
			IList<MinificationErrorInfo> errors6 = minifier.Minify(input6).Errors;
			IList<MinificationErrorInfo> errors7 = minifier.Minify(input7).Errors;
			IList<MinificationErrorInfo> errors8 = minifier.Minify(input8).Errors;
			IList<MinificationErrorInfo> errors9 = minifier.Minify(input9).Errors;
			IList<MinificationErrorInfo> errors10 = minifier.Minify(input10).Errors;
			IList<MinificationErrorInfo> errors11 = minifier.Minify(input11).Errors;

			// Assert
			Assert.Equal(1, errors1.Count);
			Assert.Equal(1, errors1[0].LineNumber);
			Assert.Equal(35, errors1[0].ColumnNumber);

			Assert.Equal(1, errors2.Count);
			Assert.Equal(1, errors2[0].LineNumber);
			Assert.Equal(40, errors2[0].ColumnNumber);

			Assert.Equal(1, errors3.Count);
			Assert.Equal(1, errors3[0].LineNumber);
			Assert.Equal(205, errors3[0].ColumnNumber);

			Assert.Equal(1, errors4.Count);
			Assert.Equal(1, errors4[0].LineNumber);
			Assert.Equal(135, errors4[0].ColumnNumber);

			Assert.Equal(1, errors5.Count);
			Assert.Equal(1, errors5[0].LineNumber);
			Assert.Equal(41, errors5[0].ColumnNumber);

			Assert.Equal(1, errors6.Count);
			Assert.Equal(3, errors6[0].LineNumber);
			Assert.Equal(90, errors6[0].ColumnNumber);

			Assert.Equal(1, errors7.Count);
			Assert.Equal(4, errors7[0].LineNumber);
			Assert.Equal(135, errors7[0].ColumnNumber);

			Assert.Equal(1, errors8.Count);
			Assert.Equal(2, errors8[0].LineNumber);
			Assert.Equal(80, errors8[0].ColumnNumber);

			Assert.Equal(1, errors9.Count);
			Assert.Equal(1, errors9[0].LineNumber);
			Assert.Equal(8, errors9[0].ColumnNumber);

			Assert.Equal(1, errors10.Count);
			Assert.Equal(5, errors10[0].LineNumber);
			Assert.Equal(1, errors10[0].ColumnNumber);

			Assert.Equal(1, errors11.Count);
			Assert.Equal(1, errors11[0].LineNumber);
			Assert.Equal(68, errors11[0].ColumnNumber);
		}

		[Fact]
		public void ProcessingInvalidHtmlCommentsIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<!-->";

			// Act
			IList<MinificationErrorInfo> errors1 = minifier.Minify(input1).Errors;

			// Assert
			Assert.Equal(1, errors1.Count);
			Assert.Equal(1, errors1[0].LineNumber);
			Assert.Equal(1, errors1[0].ColumnNumber);
		}
	}
}