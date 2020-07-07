using System.Collections.Generic;
using System.IO;

using Xunit;

namespace WebMarkupMin.Core.Test.Html.Parsing
{
	public class ProcessingInvalidCodeTests : FileSystemTestsBase
	{
		private readonly string _htmlFilesDirectoryPath;


		public ProcessingInvalidCodeTests()
		{
			_htmlFilesDirectoryPath = Path.GetFullPath(Path.Combine(_baseDirectoryPath, @"../SharedFiles/html/"));
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
			string input12 = File.ReadAllText(
				Path.Combine(_htmlFilesDirectoryPath, "html-document-with-invalid-characters.html"));

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
			IList<MinificationErrorInfo> errors12 = minifier.Minify(input12).Errors;

			// Assert
			Assert.Equal(1, errors1.Count);
			Assert.Equal(1, errors1[0].LineNumber);
			Assert.Equal(35, errors1[0].ColumnNumber);

			Assert.Equal(1, errors2.Count);
			Assert.Equal(1, errors2[0].LineNumber);
			Assert.Equal(40, errors2[0].ColumnNumber);

			Assert.Equal(1, errors3.Count);
			Assert.Equal(1, errors3[0].LineNumber);
			Assert.Equal(120, errors3[0].ColumnNumber);

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
			Assert.Equal(46, errors8[0].ColumnNumber);

			Assert.Equal(1, errors9.Count);
			Assert.Equal(1, errors9[0].LineNumber);
			Assert.Equal(8, errors9[0].ColumnNumber);

			Assert.Equal(1, errors10.Count);
			Assert.Equal(5, errors10[0].LineNumber);
			Assert.Equal(1, errors10[0].ColumnNumber);

			Assert.Equal(1, errors11.Count);
			Assert.Equal(1, errors11[0].LineNumber);
			Assert.Equal(25, errors11[0].ColumnNumber);

			Assert.Equal(1, errors12.Count);
			Assert.Equal(1, errors12[0].LineNumber);
			Assert.Equal(9, errors12[0].ColumnNumber);
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