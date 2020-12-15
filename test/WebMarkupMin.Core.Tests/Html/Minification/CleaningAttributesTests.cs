using Xunit;

namespace WebMarkupMin.Core.Tests.Html.Minification
{
	public class CleaningAttributesTests
	{
		[Fact]
		public void CleaningClassAttributesIsCorrect()
		{
			// Arrange
			var cleaningAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<div class=\" b-inline b-top-bar__item  \">Some text…</div>";
			const string targetOutput1 = "<div class=\"b-inline b-top-bar__item\">Some text…</div>";

			const string input2 = "<p class=\" b-sethome__popupa-text      \">Some text…</p>";
			const string targetOutput2 = "<p class=\"b-sethome__popupa-text\">Some text…</p>";

			const string input3 = "<p class=\"\n  \n b-sethome__popupa-text   \n\n\t  \t\n   \">Some text…</p>";
			const string targetOutput3 = "<p class=\"b-sethome__popupa-text\">Some text…</p>";

			const string input4 = "<div class=\"\n  \n b-dropdowna   \n\n\t  \t\n  i-bem b-dropdowna_is-bem_yes \">" +
				"Some text…</div>";
			const string targetOutput4 = "<div class=\"b-dropdowna i-bem b-dropdowna_is-bem_yes\">Some text…</div>";

			const string input5 = "<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\">\r\n" +
				"	<style type=\"text/css\"><![CDATA[\r\n" +
				"	circle.myGreen {\r\n" +
				"		stroke: #006600;\r\n" +
				"		fill: #00cc00;\r\n" +
				"	}\r\n" +
				"	circle.myRed {\r\n" +
				"		stroke: #660000;\r\n" +
				"		fill: #cc0000;\r\n" +
				"	}\r\n" +
				"]]></style>\r\n" +
				"	<circle class=\"\t  myGreen  \t\" cx=\"40\" cy=\"40\" r=\"24\" />\r\n" +
				"	<circle class=\"\t  myRed  \t\" cx=\"40\" cy=\"100\" r=\"24\" />\r\n" +
				"</svg>"
				;
			const string targetOutput5 = "<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\">\r\n" +
				"	<style type=\"text/css\"><![CDATA[" +
				"	circle.myGreen {\r\n" +
				"		stroke: #006600;\r\n" +
				"		fill: #00cc00;\r\n" +
				"	}\r\n" +
				"	circle.myRed {\r\n" +
				"		stroke: #660000;\r\n" +
				"		fill: #cc0000;\r\n" +
				"	}" +
				"]]></style>\r\n" +
				"	<circle class=\"myGreen\" cx=\"40\" cy=\"40\" r=\"24\" />\r\n" +
				"	<circle class=\"myRed\" cx=\"40\" cy=\"100\" r=\"24\" />\r\n" +
				"</svg>"
				;

			const string input6 = "<img class=\"item-image defer-image|http://www.example.com/rpsweb/othumb?id=NL$1452$PDF&s=e\">";
			const string targetOutput6 = "<img class=\"item-image defer-image|http://www.example.com/rpsweb/othumb?id=NL$1452$PDF&amp;s=e\">";

			// Act
			string output1 = cleaningAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = cleaningAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = cleaningAttributesMinifier.Minify(input3).MinifiedContent;
			string output4 = cleaningAttributesMinifier.Minify(input4).MinifiedContent;
			string output5 = cleaningAttributesMinifier.Minify(input5).MinifiedContent;
			string output6 = cleaningAttributesMinifier.Minify(input6).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
			Assert.Equal(targetOutput6, output6);
		}

		[Fact]
		public void CleaningStyleAttributesIsCorrect()
		{
			// Arrange
			var cleaningAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<p style=\"    color: red; background-color: rgba(255, 0, 0, 0.7);  \">Some text…</p>";
			const string targetOutput1 = "<p style=\"color: red; background-color: rgba(255, 0, 0, 0.7)\">Some text…</p>";

			const string input2 = "<p style=\"font-weight: bold  ; \">Some text…</p>";
			const string targetOutput2 = "<p style=\"font-weight: bold\">Some text…</p>";

			const string input3 = "<svg>\n" +
				"	<rect x=\"20\" y=\"20\" width=\"100\" height=\"100\" style=\"stroke: #009900;\n" +
				"			 stroke-width: 3;\n" +
				"			 stroke-dasharray: 10 5;\n" +
				"			 fill: none;\n" +
				"			\"\n" +
				"		/>\n" +
				"</svg>"
				;
			const string targetOutput3 = "<svg>\n" +
				"	<rect x=\"20\" y=\"20\" width=\"100\" height=\"100\" style=\"stroke: #009900;\n" +
				"			 stroke-width: 3;\n" +
				"			 stroke-dasharray: 10 5;\n" +
				"			 fill: none\" />\n" +
				"</svg>"
				;

			// Act
			string output1 = cleaningAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = cleaningAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = cleaningAttributesMinifier.Minify(input3).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
		}

		[Fact]
		public void CleaningUriBasedAttributesIsCorrect()
		{
			// Arrange
			var cleaningAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<a href=\"   http://example.com  \">Some text...</a>";
			const string targetOutput1 = "<a href=\"http://example.com\">Some text...</a>";

			const string input2 = "<a href=\"  \t\t  \n \t  \">Some text...</a>";
			const string targetOutput2 = "<a href=\"\">Some text...</a>";

			const string input3 = "<img src=\"   http://example.com/html.png  \" title=\"Some title...   \" " +
				"longdesc=\"  http://example.com/html.txt \n\n   \t \">";
			const string targetOutput3 = "<img src=\"http://example.com/html.png\" title=\"Some title...   \" " +
				"longdesc=\"http://example.com/html.txt\">";

			const string input4 = "<video src=\"/video/movie.mp4\" poster=\"   /video/poster.png  \"></video>";
			const string targetOutput4 = "<video src=\"/video/movie.mp4\" poster=\"/video/poster.png\"></video>";

			const string input5 = "<form action=\"  guestbook/add_record?user=monitor2002&language=russian     \"></form>";
			const string targetOutput5 = "<form action=\"guestbook/add_record?user=monitor2002&amp;language=russian\"></form>";

			const string input6 = "<BLOCKQUOTE cite=\" \n\n\n http://validator.w3.org/docs/help.html     \">" +
				"<P>This validator checks the markup validity of Web documents in HTML, XHTML, SMIL, MathML, etc.</P>" +
				"</BLOCKQUOTE>";
			const string targetOutput6 = "<blockquote cite=\"http://validator.w3.org/docs/help.html\">" +
				"<p>This validator checks the markup validity of Web documents in HTML, XHTML, SMIL, MathML, etc.</p>" +
				"</blockquote>";

			const string input7 = "<head profile=\"       http://gmpg.org/xfn/11    \"></head>";
			const string targetOutput7 = "<head profile=\"http://gmpg.org/xfn/11\"></head>";

			const string input8 = "<object codebase=\"   http://example.com  \"></object>";
			const string targetOutput8 = "<object codebase=\"http://example.com\"></object>";

			const string input9 = "<object type=\"application/x-shockwave-flash\" data=\"	 player_flv_mini.swf \">" +
				"<param name=\"movie\" value=\"	 player_flv_mini.swf \">" +
				"</object>";
			const string targetOutput9 = "<object type=\"application/x-shockwave-flash\" data=\"player_flv_mini.swf\">" +
				"<param name=\"movie\" value=\"player_flv_mini.swf\">" +
				"</object>";

			const string input10 = "<object data=\"  movie.mov	\" type=\"video/quicktime\">" +
				"<param name=\"pluginspage\" value=\"   http://quicktime.apple.com/	\">" +
				"</object>";
			const string targetOutput10 = "<object data=\"movie.mov\" type=\"video/quicktime\">" +
				"<param name=\"pluginspage\" value=\"http://quicktime.apple.com/\">" +
				"</object>";

			const string input11 = "<span profile=\"   6, 7, 8  \">Some text...</span>";
			const string input12 = "<div action=\"  one-two-three \">Some other text ...</div>";

			// Act
			string output1 = cleaningAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = cleaningAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = cleaningAttributesMinifier.Minify(input3).MinifiedContent;
			string output4 = cleaningAttributesMinifier.Minify(input4).MinifiedContent;
			string output5 = cleaningAttributesMinifier.Minify(input5).MinifiedContent;
			string output6 = cleaningAttributesMinifier.Minify(input6).MinifiedContent;
			string output7 = cleaningAttributesMinifier.Minify(input7).MinifiedContent;
			string output8 = cleaningAttributesMinifier.Minify(input8).MinifiedContent;
			string output9 = cleaningAttributesMinifier.Minify(input9).MinifiedContent;
			string output10 = cleaningAttributesMinifier.Minify(input10).MinifiedContent;
			string output11 = cleaningAttributesMinifier.Minify(input11).MinifiedContent;
			string output12 = cleaningAttributesMinifier.Minify(input12).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
			Assert.Equal(targetOutput6, output6);
			Assert.Equal(targetOutput7, output7);
			Assert.Equal(targetOutput8, output8);
			Assert.Equal(targetOutput9, output9);
			Assert.Equal(targetOutput10, output10);
			Assert.Equal(input11, output11);
			Assert.Equal(input12, output12);
		}

		[Fact]
		public void CleaningNumericAttributesIsCorrect()
		{
			// Arrange
			var cleaningAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<button tabindex=\"   1  \">Save</button> <a href=\"#\" tabindex=\"   2  \">Cancel</a>";
			const string targetOutput1 = "<button tabindex=\"1\">Save</button> <a href=\"#\" tabindex=\"2\">Cancel</a>";

			const string input2 = "<input value=\"\" maxlength=\"     5 \">";
			const string targetOutput2 = "<input value=\"\" maxlength=\"5\">";

			const string input3 = "<select size=\"  15   \t\t \"><option>Some text…</option></select>";
			const string targetOutput3 = "<select size=\"15\"><option>Some text…</option></select>";

			const string input4 = "<textarea rows=\"   10  \" cols=\"  40      \"></textarea>";
			const string targetOutput4 = "<textarea rows=\"10\" cols=\"40\"></textarea>";

			const string input5 = "<COLGROUP span=\"   20  \"><COL span=\"  19 \"></COLGROUP>";
			const string targetOutput5 = "<colgroup span=\"20\"><col span=\"19\"></colgroup>";

			const string input6 = "<tr><td colspan=\"    2   \">Some text…</td><td rowspan=\"   3 \"></td></tr>";
			const string targetOutput6 = "<tr><td colspan=\"2\">Some text…</td><td rowspan=\"3\"></td></tr>";

			// Act
			string output1 = cleaningAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = cleaningAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = cleaningAttributesMinifier.Minify(input3).MinifiedContent;
			string output4 = cleaningAttributesMinifier.Minify(input4).MinifiedContent;
			string output5 = cleaningAttributesMinifier.Minify(input5).MinifiedContent;
			string output6 = cleaningAttributesMinifier.Minify(input6).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
			Assert.Equal(targetOutput6, output6);
		}

		[Fact]
		public void CleaningEventAttributesIsCorrect()
		{
			// Arrange
			var cleaningAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<button type=\"button\" onmouseover=\" \n\n alert('Hooray!')  \t \n\t  \">Some text…</button>";
			const string targetOutput1 = "<button type=\"button\" onmouseover=\"alert('Hooray!')\">Some text…</button>";

			const string input2 = "<a href=\"http://example.com/login.php\"" +
				" onclick=\"  window.open('http://example.com/login.php','_blank','toolbar=0,location=0,status=0," +
				"width=520,height=270,scrollbars=0,resizable=1');return false; \">" +
				"Some text…</a>"
				;
			const string targetOutput2 = "<a href=\"http://example.com/login.php\"" +
				" onclick=\"window.open('http://example.com/login.php','_blank','toolbar=0,location=0,status=0," +
				"width=520,height=270,scrollbars=0,resizable=1');return false\">" +
				"Some text…</a>"
				;

			const string input3 = "<body onload=\"  initStatistics();   initForms() ;  \"><p>Some text…</p></body>";
			const string targetOutput3 = "<body onload=\"initStatistics();   initForms()\"><p>Some text…</p></body>";

			const string input4 = "<svg width=\"500\" height=\"100\">\n" +
				"	<rect x=\"10\" y=\"10\" width=\"100\" height=\"75\"" +
				" onmouseover=\"  this.style.stroke = '#ff0000'; this.style['stroke-width'] = 5;  \"" +
				" onmouseout=\"  this.style.stroke = '#000000'; this.style['stroke-width'] = 1;  \" />\n" +
				"</svg>"
				;
			const string targetOutput4 = "<svg width=\"500\" height=\"100\">\n" +
				"	<rect x=\"10\" y=\"10\" width=\"100\" height=\"75\"" +
				" onmouseover=\"this.style.stroke = '#ff0000'; this.style['stroke-width'] = 5\"" +
				" onmouseout=\"this.style.stroke = '#000000'; this.style['stroke-width'] = 1\" />\n" +
				"</svg>"
				;

			// Act
			string output1 = cleaningAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = cleaningAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = cleaningAttributesMinifier.Minify(input3).MinifiedContent;
			string output4 = cleaningAttributesMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
		}

		[Fact]
		public void CleaningOtherAttributesIsCorrect()
		{
			// Arrange
			var cleaningAttributesMinifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input = "<meta name=\"keywords\" content=\"	HTML5, CSS3, ECMAScript 5, \">";
			const string targetOutput = "<meta name=\"keywords\" content=\"HTML5,CSS3,ECMAScript 5\">";

			// Act
			string output = cleaningAttributesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput, output);
		}
	}
}