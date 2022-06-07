using System.Collections.Generic;

using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class IgnoringFragmentsOfMarkupTests
	{
		[Fact]
		public void IgnoringFragmentsOfMarkupIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true)
			{
				WhitespaceMinificationMode = WhitespaceMinificationMode.Medium
			});

			const string input1 = "<!--wmm:ignore--><div class=\"class01\" style=\"color: firebrick\">\n" +
				"   Some  text...   <span> <input readonly/>  and some text... </span>\n\n" +
				"   </div><!--/wmm:ignore-->\n" +
				"<div class=\"class01\" style=\"color: firebrick\">\n" +
				"   Some text...   <span> <input readonly/>  and some text... </span>\n\n" +
				"   </div>"
				;
			const string targetOutput1 = "<div class=\"class01\" style=\"color: firebrick\">\n" +
				"   Some  text...   <span> <input readonly/>  and some text... </span>\n\n" +
				"   </div>" +
				"<div class=\"class01\" style=\"color: firebrick\">" +
				"Some text... <span> <input readonly=\"readonly\"> and some text... </span></div>"
				;

			const string input2 = "<!--wmm:ignore-->\n" +
				"<div class=\"mermaid\">sequenceDiagram\n\n" +
				"Participant Subscriber1 As Subscriber1\n" +
				"Participant Subscriber2 As Subscriber2\n" +
				"Subscriber1 ->> Publisher: Subscribe to Message1\n" +
				"Publisher ->> Persistence: Store \"Subscriber1 wants Message1\"\n" +
				"Subscriber2 ->> Publisher: Subscribe to Message1\n" +
				"Publisher ->> Persistence: Store \"Subscriber2 wants Message1\"\n" +
				"</div>\n" +
				"<!--/wmm:ignore-->"
				;
			const string targetOutput2 = "\n" +
				"<div class=\"mermaid\">sequenceDiagram\n\n" +
				"Participant Subscriber1 As Subscriber1\n" +
				"Participant Subscriber2 As Subscriber2\n" +
				"Subscriber1 ->> Publisher: Subscribe to Message1\n" +
				"Publisher ->> Persistence: Store \"Subscriber1 wants Message1\"\n" +
				"Subscriber2 ->> Publisher: Subscribe to Message1\n" +
				"Publisher ->> Persistence: Store \"Subscriber2 wants Message1\"\n" +
				"</div>\n"
				;

			const string input3 = "x\n<!--wmm:ignore-->y<!--/wmm:ignore-->";
			const string targetOutput3 = "x y";

			const string input4 = "<p>Hello <!--wmm:ignore--><span>\n\tworld!\n</span><!--/wmm:ignore-->.</p>";
			const string targetOutput4 = "<p>Hello <span>\n\tworld!\n</span>.</p>";

			const string input5 = "<!--wmm:ignore--><!--/wmm:ignore-->";
			const string targetOutput5 = "";

			const string input6 = "<!--wmm:ignore-->\n" +
				"<!DOCTYPE html>\n" +
				"<!--/wmm:ignore-->\n" +
				"    <p>   Hola mundo!!!   </p>"
				;
			const string targetOutput6 = "\n" +
				"<!DOCTYPE html>\n" +
				"<p>Hola mundo!!!</p>";

			const string input7 = "<!--wmm:ignore-->";

			const string input8 = "<p>Some text...</p>\n\n" +
				"    <!--wmm:ignore--><p>Any other text...</p>\n" +
				"<p>And some text...</p>"
				;

			const string input9 = "<!--/wmm:ignore-->";

			const string input10 = "<p>Some text...</p>\n" +
				"  <!--/wmm:ignore--><p>Any other text...</p>\n" +
				"<p>And some text...</p>"
				;

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			string output3 = minifier.Minify(input3).MinifiedContent;
			string output4 = minifier.Minify(input4).MinifiedContent;
			string output5 = minifier.Minify(input5).MinifiedContent;
			string output6 = minifier.Minify(input6).MinifiedContent;
			IList<MinificationErrorInfo> errors7 = minifier.Minify(input7).Errors;
			IList<MinificationErrorInfo> errors8 = minifier.Minify(input8).Errors;
			IList<MinificationErrorInfo> errors9 = minifier.Minify(input9).Errors;
			IList<MinificationErrorInfo> errors10 = minifier.Minify(input10).Errors;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
			Assert.Equal(targetOutput6, output6);

			Assert.Equal(1, errors7.Count);
			Assert.Equal(1, errors7[0].LineNumber);
			Assert.Equal(1, errors7[0].ColumnNumber);

			Assert.Equal(1, errors8.Count);
			Assert.Equal(3, errors8[0].LineNumber);
			Assert.Equal(5, errors8[0].ColumnNumber);

			Assert.Equal(1, errors9.Count);
			Assert.Equal(1, errors9[0].LineNumber);
			Assert.Equal(1, errors9[0].ColumnNumber);

			Assert.Equal(1, errors10.Count);
			Assert.Equal(2, errors10[0].LineNumber);
			Assert.Equal(3, errors10[0].ColumnNumber);
		}
	}
}