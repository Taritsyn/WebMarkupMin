using System;

using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class ProcessingConditionalCommentsTests : IDisposable
	{
		private HtmlMinifier _removingHtmlCommentsMinifier;


		public ProcessingConditionalCommentsTests()
		{
			_removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = true });
		}


		[Fact]
		public void ProcessingHiddenConditionalCommentsIsCorrect()
		{
			// Arrange
			const string input1 = "<!--[if lt IE 9]>" +
				"<script src=\"Scripts/html5shiv.js\"></script>" +
				"<![endif]-->"
				;
			const string targetOutput1 = input1;

			const string input2 = "<!--[if IE 7]>\n" +
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
			const string targetOutput2 = input2;

			const string input3 = "<!--[if IE 7 ]>\n" +
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
			const string targetOutput3 = "<!--[if IE 7]>\n" +
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

			const string input4 = "<picture>\n" +
				"	<!--[if IE 9]><video style=\"display: none\"><![endif]-->\n" +
				"	<source srcset=\"examples/images/extralarge.jpg\" media=\"(min-width: 1000px)\">\n" +
				"	<source srcset=\"examples/images/large.jpg\" media=\"(min-width: 800px)\">\n" +
				"	<source srcset=\"examples/images/medium.jpg\">\n" +
				"	<!--[if IE 9]></video><![endif]-->\n" +
				"	<img srcset=\"examples/images/medium.jpg\">\n" +
				"</picture>"
				;
			const string targetOutput4 = input4;

			const string input5 = "<div><table><tr><td>\r\n\r\n" +
				"    <!--[if mso]>\r\n" +
				"    <div>\r\n" +
				"        test1\r\n" +
				"    <![endif]-->\r\n" +
				"    test2\r\n" +
				"    <!--[if mso]>\r\n" +
				"    </div>\r\n" +
				"    <![endif]-->\r\n\r\n" +
				"    <!--[if mso]>\r\n" +
				"    <div>\r\n" +
				"        test1\r\n" +
				"    <![endif]-->\r\n" +
				"    test2\r\n" +
				"    <!--[if mso]>\r\n" +
				"    </div>\r\n" +
				"    <![endif]-->\r\n" +
				"</td></tr></table></div>\r\n"
				;
			const string targetOutput5 = input5;

			const string input6 = "<!--[if mso]>\r" +
				"	<style>\r" +
				"		body {\r" +
				"			font-family: Arial, sans serif;\r" +
				"			font-size: 14px;\r" +
				"			color: #163e79;\r" +
				"		}\r" +
				"	</style>\r" +
				"<![endif]-->"
				;
			const string targetOutput6 = input6;

			const string input7 = "<!--[if lte mso 14]>\n" +
				"<table width=\"100%\" style=\"border: 1px solid #3f3f3f\"><tr><td>\n" +
				"    <p>First row of table content.</p>\n" +
				"</td></tr>\n" +
				"<tr style=\"page-break-before: always\"><td>\n" +
				"    <p>This code will force a page break at table row.</p>\n" +
				"</td></tr>\n" +
				"<tr><td>\n" +
				"    <p>Last row of table content.</p>\n" +
				"</td></tr></table>\n" +
				"<![endif]-->\n\n" +
				"<!--[if gt mso 14]>\n" +
				"<table width=\"100%\" style=\"border: 1px solid #3f3f3f\"><tr><td>\n" +
				"    <p>First row of table content.</p>\n" +
				"</td></tr>\n" +
				"<tr><td>\n" +
				"    <p>This is just a normal row of content now.</p>\n" +
				"</td></tr>\n" +
				"<tr><td>\n" +
				"    <p>Last row of table content.</p>\n" +
				"</td></tr></table>\n" +
				"<![endif]-->"
				;
			const string targetOutput7 = input7;

			const string input8 = "<!--[if mso]>\r\n" +
				"<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\">\r\n" +
				"<tr>\r\n" +
				"<td width=\"340\">\r\n" +
				"<![endif]-->\r\n" +
				"    <div style=\"display: inline-block; width: 100%; min-width: 200px; max-width: 340px\">\r\n" +
				"        <p>Here’s the table cell content. Since we’ve declared a fixed width for the\r\n" +
				"        table cell in the MSO conditionalstatement, we’ll be able to use a static width\r\n" +
				"        for Outlook while preserving fluid table design in other email clients that\r\n" +
				"        respect div styles.</p>\r\n" +
				"    </div>\r\n" +
				"<!--[if mso]>\r\n" +
				"</td>\r\n" +
				"</tr>\r\n" +
				"</table>\r\n" +
				"<![endif]-->"
				;
			const string targetOutput8 = input8;

			const string input9 = "<!--[if (IE) ]>\n" +
				"<p>Этот контент видно только в версиях Outlook, базирующихся на Internet Explorer.</p>\n" +
				"<![endif]-->"
				;
			const string targetOutput9 = "<!--[if (IE)]>\n" +
				"<p>Этот контент видно только в версиях Outlook, базирующихся на Internet Explorer.</p>\n" +
				"<![endif]-->"
				;

			const string input10 = "<!--[if (gte mso 9)|(IE)]>\r\n" +
				"<table width=\"600\" align=\"center\">\r\n" +
				"<tr>\r\n" +
				"<td>\r\n" +
				"<![endif]-->\r\n" +
				"<table class=\"outer\" align=\"center\">\r\n" +
				"	<tr>\r\n" +
				"		<td>\r\n" +
				"			Какой-то рыбный текст.\r\n" +
				"		</td>\r\n" +
				"	</tr>\r\n" +
				"</table>\r\n" +
				"<!--[if (gte mso 9) | (IE) ]>\r\n" +
				"</td>\r\n" +
				"</tr>\r\n" +
				"</table>\r\n" +
				"<![endif]-->"
				;
			const string targetOutput10 = "<!--[if (gte mso 9)|(IE)]>\r\n" +
				"<table width=\"600\" align=\"center\">\r\n" +
				"<tr>\r\n" +
				"<td>\r\n" +
				"<![endif]-->\r\n" +
				"<table class=\"outer\" align=\"center\">\r\n" +
				"	<tr>\r\n" +
				"		<td>\r\n" +
				"			Какой-то рыбный текст.\r\n" +
				"		</td>\r\n" +
				"	</tr>\r\n" +
				"</table>\r\n" +
				"<!--[if (gte mso 9) | (IE)]>\r\n" +
				"</td>\r\n" +
				"</tr>\r\n" +
				"</table>\r\n" +
				"<![endif]-->"
				;

			// Act
			string output1 = _removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;
			string output2 = _removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;
			string output3 = _removingHtmlCommentsMinifier.Minify(input3).MinifiedContent;
			string output4 = _removingHtmlCommentsMinifier.Minify(input4).MinifiedContent;
			string output5 = _removingHtmlCommentsMinifier.Minify(input5).MinifiedContent;
			string output6 = _removingHtmlCommentsMinifier.Minify(input6).MinifiedContent;
			string output7 = _removingHtmlCommentsMinifier.Minify(input7).MinifiedContent;
			string output8 = _removingHtmlCommentsMinifier.Minify(input8).MinifiedContent;
			string output9 = _removingHtmlCommentsMinifier.Minify(input9).MinifiedContent;
			string output10 = _removingHtmlCommentsMinifier.Minify(input10).MinifiedContent;

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
		}

		[Fact]
		public void ProcessingRevealedConditionalCommentsIsCorrect()
		{
			// Arrange
			const string input1 = "<![if IE 5.0000]>\n" +
				"	<p>Welcome to Internet Explorer 5.0!</p>\n" +
				"<![endif \t  ]>\n" +
				"<![if IE 5]>\n" +
				"	<p>Welcome to any incremental version of Internet Explorer 5!</p>\n" +
				"<![endif  \n ]>\n" +
				"<![if lt IE 8]>\n" +
				"	<p class=\"text-warning\">Please upgrade to Internet Explorer version 8.</p>\n" +
				"<![endif]>"
				;
			const string targetOutput1 = "<![if IE 5.0000]>\n" +
				"	<p>Welcome to Internet Explorer 5.0!</p>\n" +
				"<![endif]>\n" +
				"<![if IE 5]>\n" +
				"	<p>Welcome to any incremental version of Internet Explorer 5!</p>\n" +
				"<![endif]>\n" +
				"<![if lt IE 8]>\n" +
				"	<p class=\"text-warning\">Please upgrade to Internet Explorer version 8.</p>\n" +
				"<![endif]>"
				;

			const string input2 = "<![if IE 5.0000 ]>\n" +
				"	<p>Welcome to Internet Explorer 5.0!</p>\n" +
				"<![endif \t  ]>\n" +
				"<![if IE 5 ]>\n" +
				"	<p>Welcome to any incremental version of Internet Explorer 5!</p>\n" +
				"<![endif  \n ]>\n" +
				"<![if lt IE 8]>\n" +
				"	<p class=\"text-warning\">Please upgrade to Internet Explorer version 8.</p>\n" +
				"<![endif]>"
				;
			const string targetOutput2 = "<![if IE 5.0000]>\n" +
				"	<p>Welcome to Internet Explorer 5.0!</p>\n" +
				"<![endif]>\n" +
				"<![if IE 5]>\n" +
				"	<p>Welcome to any incremental version of Internet Explorer 5!</p>\n" +
				"<![endif]>\n" +
				"<![if lt IE 8]>\n" +
				"	<p class=\"text-warning\">Please upgrade to Internet Explorer version 8.</p>\n" +
				"<![endif]>"
				;

			// Act
			string output1 = _removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;
			string output2 = _removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
		}

		[Fact]
		public void ProcessingRevealedValidatingConditionalCommentsIsCorrect()
		{
			// Arrange
			const string input1 = "<!--[if IEMobile 7]> <html class=\"no-js iem7\"> <![endif \t  ]-->\n" +
				"<!--[if lt IE 9 ]> <html class=\"no-js lte-ie8\"> <![endif  \n ]-->\n" +
				"<!--[if (gt IE 8)|(gt IEMobile 7)|!(IEMobile)|!(IE)]> \t\n  <!  \t\n --> <html class=\"no-js\" lang=\"en\"> <!-- \t\n  <![endif  \t\n ]-->\n"
				;
			const string targetOutput1 = "<!--[if IEMobile 7]> <html class=\"no-js iem7\"> <![endif]-->\n" +
				"<!--[if lt IE 9]> <html class=\"no-js lte-ie8\"> <![endif]-->\n" +
				"<!--[if (gt IE 8)|(gt IEMobile 7)|!(IEMobile)|!(IE)]><!--> <html class=\"no-js\" lang=\"en\"> <!--<![endif]-->\n" +
				"</html>"
				;

			const string input2 = "<!--[if IEMobile 7 ]> <html class=\"no-js iem7\"> <![endif]-->\n" +
				"<!--[if lt IE 9]> <html class=\"no-js lte-ie8\"> <![endif]-->\n" +
				"<!--[if (gt IE 8)|(gt IEMobile 7)|!(IEMobile)|!(IE)]><!--> <html class=\"no-js\" lang=\"en\"> <!--<![endif]-->\n"
				;
			const string targetOutput2 = "<!--[if IEMobile 7]> <html class=\"no-js iem7\"> <![endif]-->\n" +
				"<!--[if lt IE 9]> <html class=\"no-js lte-ie8\"> <![endif]-->\n" +
				"<!--[if (gt IE 8)|(gt IEMobile 7)|!(IEMobile)|!(IE)]><!--> <html class=\"no-js\" lang=\"en\"> <!--<![endif]-->\n" +
				"</html>"
				;

			const string input3 = "<!--[if lt IE 9]>\n" +
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
			const string targetOutput3 = input3;

			const string input4 = "<!--[if lte mso 14 ]>\r" +
				"<table width=\"100%\" style=\"border: 1px solid #3f3f3f\"><tr><td>\r" +
				"    <p>This information will display only in Microsoft Outlook 2010 and older.</p>\r" +
				"</td></tr></table>\r" +
				"<![endif]-->\r" +
				"<!--[if gt mso 14 ]>\r" +
				"<table width=\"80%\" style=\"border: 2px solid #000\"><tr><td>\r" +
				"    <p>This information will display only in Microsoft Outlook 2013 and newer.</p>\r" +
				"</td></tr></table>\r" +
				"<![endif]-->\r" +
				"<!--[if !mso ]><!-->\r" +
				"<table width=\"100%\" style=\"border: 3px solid #163e79\"><tr><td>\r" +
				"    <p>This information will display only if the client is not Microsoft Outlook.</p>\r" +
				"</td></tr></table>\r" +
				"<!--<![endif]-->"
				;
			const string targetOutput4 = "<!--[if lte mso 14]>\r" +
				"<table width=\"100%\" style=\"border: 1px solid #3f3f3f\"><tr><td>\r" +
				"    <p>This information will display only in Microsoft Outlook 2010 and older.</p>\r" +
				"</td></tr></table>\r" +
				"<![endif]-->\r" +
				"<!--[if gt mso 14]>\r" +
				"<table width=\"80%\" style=\"border: 2px solid #000\"><tr><td>\r" +
				"    <p>This information will display only in Microsoft Outlook 2013 and newer.</p>\r" +
				"</td></tr></table>\r" +
				"<![endif]-->\r" +
				"<!--[if !mso]><!-->\r" +
				"<table width=\"100%\" style=\"border: 3px solid #163e79\"><tr><td>\r" +
				"    <p>This information will display only if the client is not Microsoft Outlook.</p>\r" +
				"</td></tr></table>\r" +
				"<!--<![endif]-->"
				;

			const string input5 = "<!--[if true]>\r\n" +
				"<table role=\"presentation\" width=\"100%\" style=\"all: unset; opacity: 0\">\r\n" +
				"    <tr>\r\n" +
				"<![endif]-->\r\n" +
				"<!--[if false]></td></tr></table><![endif]-->\r\n" +
				"<div style=\"display: table; width: 100%\">\r\n" +
				"    <!--[if true ]>\r\n" +
				"        <td width=\"100%\">\r\n" +
				"    <![endif]-->\r\n" +
				"    <!--[if !true]><!-->\r\n" +
				"        <div style=\"display: table-cell; width:100%\">\r\n" +
				"    <!--<![endif]-->\r\n" +
				"            Column content\r\n" +
				"    <!--[if !true ]><!-->\r\n" +
				"        </div>\r\n" +
				"    <!--<![endif]-->\r\n" +
				"    <!--[if true]>\r\n" +
				"        </td>\r\n" +
				"    <![endif]-->\r\n" +
				"    </div>\r\n" +
				"<!--[if true]>\r\n" +
				"    </tr>\r\n" +
				"</table>\r\n" +
				"<![endif]-->"
				;
			const string targetOutput5 = "<!--[if true]>\r\n" +
				"<table role=\"presentation\" width=\"100%\" style=\"all: unset; opacity: 0\">\r\n" +
				"    <tr>\r\n" +
				"<![endif]-->\r\n" +
				"<!--[if false]></td></tr></table><![endif]-->\r\n" +
				"<div style=\"display: table; width: 100%\">\r\n" +
				"    <!--[if true]>\r\n" +
				"        <td width=\"100%\">\r\n" +
				"    <![endif]-->\r\n" +
				"    <!--[if !true]><!-->\r\n" +
				"        <div style=\"display: table-cell; width:100%\">\r\n" +
				"    <!--<![endif]-->\r\n" +
				"            Column content\r\n" +
				"    <!--[if !true]><!-->\r\n" +
				"        </div>\r\n" +
				"    <!--<![endif]-->\r\n" +
				"    <!--[if true]>\r\n" +
				"        </td>\r\n" +
				"    <![endif]-->\r\n" +
				"    </div>\r\n" +
				"<!--[if true]>\r\n" +
				"    </tr>\r\n" +
				"</table>\r\n" +
				"<![endif]-->"
				;

			// Act
			string output1 = _removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;
			string output2 = _removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;
			string output3 = _removingHtmlCommentsMinifier.Minify(input3).MinifiedContent;
			string output4 = _removingHtmlCommentsMinifier.Minify(input4).MinifiedContent;
			string output5 = _removingHtmlCommentsMinifier.Minify(input5).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
		}

		[Fact]
		public void ProcessingRevealedValidatingSimplifiedConditionalCommentsIsCorrect()
		{
			// Arrange
			const string input1 = "<!--[if lt IE 7 ]> <html class=\"no-js ie6 ielt8\"> <![endif]-->\n" +
				"<!--[if IE 7 ]> <html class=\"no-js ie7 ielt8\"> <![endif]-->\n" +
				"<!--[if IE 8 ]> <html class=\"no-js ie8\"> <![endif]-->\n" +
				"<!--[if IE 9 ]> <html class=\"no-js ie9\"> <![endif]-->\n" +
				"<!--[if (gt IE 9)|!(IE)]> \t\n  --> <html class=\"no-js\"> <!--  \t\n <![endif \t\n  ]-->\n"
				;
			const string targetOutput1 = "<!--[if lt IE 7]> <html class=\"no-js ie6 ielt8\"> <![endif]-->\n" +
				"<!--[if IE 7]> <html class=\"no-js ie7 ielt8\"> <![endif]-->\n" +
				"<!--[if IE 8]> <html class=\"no-js ie8\"> <![endif]-->\n" +
				"<!--[if IE 9]> <html class=\"no-js ie9\"> <![endif]-->\n" +
				"<!--[if (gt IE 9)|!(IE)]>--> <html class=\"no-js\"> <!--<![endif]-->\n" +
				"</html>"
				;

			const string input2 = "<!--[if lt IE 7]> <html class=\"no-js ie6 oldie\" " +
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
			const string targetOutput2 = "<!--[if lt IE 7]> <html class=\"no-js ie6 oldie\" " +
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

			// Act
			string output1 = _removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;
			string output2 = _removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
		}

		#region IDisposable implementation

		public void Dispose()
		{
			_removingHtmlCommentsMinifier = null;
		}

		#endregion
	}
}