using System;
using System.Collections.Generic;
using System.IO;

using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Parsing
{
	public class ProcessingInvalidCodeTests : IDisposable
	{
		private const string HTML_FILES_DIRECTORY_PATH = @"Files/html/";

		private HtmlMinifier _minifier;


		public ProcessingInvalidCodeTests()
		{
			_minifier = new HtmlMinifier(new HtmlMinificationSettings(true));
		}


		[Fact]
		public void ProcessingInvalidCharactersInMetaStartTagIsCorrect()
		{
			// Arrange
			const string input = "<meta name=\"Description\" content=\"Информационно-аналитический портал про " +
				"недвижимость и квартиры в Москве и РФ \"Хата.ру\" – Аналитика и цены на недвижимость онлайн, " +
				"прогнозы, исследования, обзоры и статьи.\">"
				;

			// Act
			IList<MinificationErrorInfo> errors = _minifier.Minify(input).Errors;

			// Assert
			Assert.Equal(1, errors.Count);
			Assert.Equal("In the start tag <meta> found invalid characters.", errors[0].Message);
			Assert.Equal(1, errors[0].LineNumber);
			Assert.Equal(120, errors[0].ColumnNumber);
			Assert.Equal(
				"Line 1: … недвижимость и квартиры в Москве и РФ \"Хата.ру\" – Аналитика и цены на недвижимость онлайн, п…" + Environment.NewLine +
				"--------------------------------------------------------^" + Environment.NewLine,
				errors[0].SourceFragment
			);
		}

		[Fact]
		public void ProcessingInvalidCharactersInLinkStartTagIsCorrect()
		{
			// Arrange
			const string input = "<link id=\"favicon\" rel=?\"shortcut icon\" type=?\"image/?png\" href=?\"#\">";

			// Act
			IList<MinificationErrorInfo> errors = _minifier.Minify(input).Errors;

			// Assert
			Assert.Equal(1, errors.Count);
			Assert.Equal("In the start tag <link> found invalid characters.", errors[0].Message);
			Assert.Equal(1, errors[0].LineNumber);
			Assert.Equal(25, errors[0].ColumnNumber);
			Assert.Equal(
				"Line 1: <link id=\"favicon\" rel=?\"shortcut icon\" type=?\"image/?png\" href=?\"#\">" + Environment.NewLine +
				"--------------------------------^" + Environment.NewLine,
				errors[0].SourceFragment
			);
		}

		[Fact]
		public void ProcessingInvalidCharactersInHeaderStartTagIsCorrect()
		{
			// Arrange
			const string input = "<header\"></header>";

			// Act
			IList<MinificationErrorInfo> errors = _minifier.Minify(input).Errors;

			// Assert
			Assert.Equal(1, errors.Count);
			Assert.Equal("In the start tag <header> found invalid characters.", errors[0].Message);
			Assert.Equal(1, errors[0].LineNumber);
			Assert.Equal(8, errors[0].ColumnNumber);
			Assert.Equal(
				"Line 1: <header\"></header>" + Environment.NewLine +
				"---------------^" + Environment.NewLine,
				errors[0].SourceFragment
			);
		}

		[Fact]
		public void ProcessingInvalidCharactersInDivStartTagIsCorrect()
		{
			// Arrange
			const string input = "<div class=\"row\">\n" +
				"	<div class=\"span12>\n" +
				"		Level 1 of column\n" +
				"		<div class=\"row\">\n" +
				"			<div class=\"span6\">Level 2</div>\n" +
				"			<div class=\"span6\">Level 2</div>\n" +
				"		</div>\n" +
				"	</div>\n" +
				"</div>"
				;

			// Act
			IList<MinificationErrorInfo> errors = _minifier.Minify(input).Errors;

			// Assert
			Assert.Equal(1, errors.Count);
			Assert.Equal("In the start tag <div> found invalid characters.", errors[0].Message);
			Assert.Equal(4, errors[0].LineNumber);
			Assert.Equal(18, errors[0].ColumnNumber);
			Assert.Equal(
				"Line 3:         Level 1 of column" + Environment.NewLine +
				"Line 4:         <div class=\"row\">" + Environment.NewLine +
				"-------------------------------^" + Environment.NewLine +
				"Line 5:             <div class=\"span6\">Level 2</div>" + Environment.NewLine,
				errors[0].SourceFragment
			);

		}

		[Fact]
		public void ProcessingInvalidCharactersInSpanStartTagIsCorrect()
		{
			// Arrange
			const string input = "<span class=\"b-form-input_size_l i-bem\"\">Some text...</span>";

			// Act
			IList<MinificationErrorInfo> errors = _minifier.Minify(input).Errors;

			// Assert
			Assert.Equal(1, errors.Count);
			Assert.Equal("In the start tag <span> found invalid characters.", errors[0].Message);
			Assert.Equal(1, errors[0].LineNumber);
			Assert.Equal(40, errors[0].ColumnNumber);
			Assert.Equal(
				"Line 1: <span class=\"b-form-input_size_l i-bem\"\">Some text...</span>" + Environment.NewLine +
				"-----------------------------------------------^" + Environment.NewLine,
				errors[0].SourceFragment
			);
		}

		[Fact]
		public void ProcessingInvalidCharactersInAnchorStartTagIsCorrect()
		{
			// Arrange
			const string input1 = "<A HREF='/cgi-bin/user.cgi?user=O'Connor'>O'Connor</A>";
			const string input2 = "<li>\n" +
				"	<a data-bind=\"href:'@Url.Action(\"GetInvoices\", \"ListUsers\")'+'/?eMail='+ Login\">Invoices</a>\n" +
				"</li>"
				;

			// Act
			IList<MinificationErrorInfo> errors1 = _minifier.Minify(input1).Errors;
			IList<MinificationErrorInfo> errors2 = _minifier.Minify(input2).Errors;

			// Assert
			Assert.Equal(1, errors1.Count);
			Assert.Equal("In the start tag <A> found invalid characters.", errors1[0].Message);
			Assert.Equal(1, errors1[0].LineNumber);
			Assert.Equal(41, errors1[0].ColumnNumber);
			Assert.Equal(
				"Line 1: <A HREF='/cgi-bin/user.cgi?user=O'Connor'>O'Connor</A>" + Environment.NewLine +
				"------------------------------------------------^" + Environment.NewLine,
				errors1[0].SourceFragment
			);

			Assert.Equal(1, errors2.Count);
			Assert.Equal("In the start tag <a> found invalid characters.", errors2[0].Message);
			Assert.Equal(2, errors2[0].LineNumber);
			Assert.Equal(46, errors2[0].ColumnNumber);
			Assert.Equal(
				"Line 1: <li>" + Environment.NewLine +
				"Line 2:     <a data-bind=\"href:'@Url.Action(\"GetInvoices\", \"ListUsers\")'+'/?eMail='+ Login\">Invoices</a>" + Environment.NewLine +
				"--------------------------------------------------------^" + Environment.NewLine +
				"Line 3: </li>" + Environment.NewLine,
				errors2[0].SourceFragment
			);
		}

		[Fact]
		public void ProcessingInvalidCharactersInImageStartTagIsCorrect()
		{
			// Arrange
			const string input1 = "<img src=\"/images/39.gif\" width=80\" height=\"60\">";
			const string input2 = "<img src=\"http://files.example.com/img/88900173ct.jpg\" width=\"100\" " +
				"title=\"Программирование на COBOL, 14-е издание, I том (файл PDF) \" / " +
				"alt=\"Программирование на COBOL, 14-е издание, I том (файл PDF)\" />"
				;
			const string input3 = "<noscript>\r" +
				"	<div>\r" +
				"		<img src=\"//mk.tyndex.ru/watch/6740060\" style=\"position:absolute; left:-9999px;\" alt=\"\"\" />\r" +
				"	</div>\r" +
				"</noscript>"
				;
			const string input4 = "<table class='grayblock' style='width:100%'>\r\n" +
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

			// Act
			IList<MinificationErrorInfo> errors1 = _minifier.Minify(input1).Errors;
			IList<MinificationErrorInfo> errors2 = _minifier.Minify(input2).Errors;
			IList<MinificationErrorInfo> errors3 = _minifier.Minify(input3).Errors;
			IList<MinificationErrorInfo> errors4 = _minifier.Minify(input4).Errors;

			// Assert
			Assert.Equal(1, errors1.Count);
			Assert.Equal("In the start tag <img> found invalid characters.", errors1[0].Message);
			Assert.Equal(1, errors1[0].LineNumber);
			Assert.Equal(35, errors1[0].ColumnNumber);
			Assert.Equal(
				"Line 1: <img src=\"/images/39.gif\" width=80\" height=\"60\">" + Environment.NewLine +
				"------------------------------------------^" + Environment.NewLine,
				errors1[0].SourceFragment
			);

			Assert.Equal(1, errors2.Count);
			Assert.Equal("In the start tag <img> found invalid characters.", errors2[0].Message);
			Assert.Equal(1, errors2[0].LineNumber);
			Assert.Equal(135, errors2[0].ColumnNumber);
			Assert.Equal(
				"Line 1: …ние на COBOL, 14-е издание, I том (файл PDF) \" / alt=\"Программирование на COBOL, 14-е издание…" + Environment.NewLine +
				"--------------------------------------------------------^" + Environment.NewLine,
				errors2[0].SourceFragment
			);

			Assert.Equal(1, errors3.Count);
			Assert.Equal("In the start tag <img> found invalid characters.", errors3[0].Message);
			Assert.Equal(3, errors3[0].LineNumber);
			Assert.Equal(90, errors3[0].ColumnNumber);
			Assert.Equal(
				"Line 2:     <div>" + Environment.NewLine +
				"Line 3:         <img src=\"//mk.tyndex.ru/watch/6740060\" style=\"position:absolute; left:-9999px;\" alt=\"\"\" />" + Environment.NewLine +
				"-------------------------------------------------------------------------------------------------------^" + Environment.NewLine +
				"Line 4:     </div>" + Environment.NewLine,
				errors3[0].SourceFragment
			);

			Assert.Equal(1, errors4.Count);
			Assert.Equal("In the start tag <img> found invalid characters.", errors4[0].Message);
			Assert.Equal(4, errors4[0].LineNumber);
			Assert.Equal(135, errors4[0].ColumnNumber);
			Assert.Equal(
				"Line 3: " + Environment.NewLine +
				"Line 4: …му Общему собранию акционеров ОАО Рога и копыта' alt='Материалы к годовому Общему собранию ак…" + Environment.NewLine +
				"--------------------------------------------------------^" + Environment.NewLine +
				"Line 5: …та</a></td>" + Environment.NewLine,
				errors4[0].SourceFragment
			);
		}

		[Fact]
		public void ProcessingInvalidCharactersInTableEndTagIsCorrect()
		{
			// Arrange
			const string input = "<table>\n" +
				"	<tr>\n" +
				"		<td></td>\n" +
				"	</tr>\n" +
				"</table."
				;

			// Act
			IList<MinificationErrorInfo> errors = _minifier.Minify(input).Errors;

			// Assert
			Assert.Equal(1, errors.Count);
			Assert.Equal("During parsing of HTML-code error has occurred.", errors[0].Message);
			Assert.Equal(5, errors[0].LineNumber);
			Assert.Equal(1, errors[0].ColumnNumber);
			Assert.Equal(
				"Line 4:     </tr>" + Environment.NewLine +
				"Line 5: </table." + Environment.NewLine +
				"--------^" + Environment.NewLine,
				errors[0].SourceFragment
			);
		}

		[Fact]
		public void ProcessingInvalidHtmlCommentIsCorrect()
		{
			// Arrange
			const string input = "<!-->";

			// Act
			IList<MinificationErrorInfo> errors = _minifier.Minify(input).Errors;

			// Assert
			Assert.Equal(1, errors.Count);
			Assert.Equal("Comment is not closed.", errors[0].Message);
			Assert.Equal(1, errors[0].LineNumber);
			Assert.Equal(1, errors[0].ColumnNumber);
			Assert.Equal(
				"Line 1: <!-->" + Environment.NewLine +
				"--------^" + Environment.NewLine,
				errors[0].SourceFragment
			);
		}

		[Fact]
		public void ProcessingInvalidCodeGeneratedByFuzzerIsCorrect()
		{
			// Arrange
			string input = File.ReadAllText(
				Path.Combine(HTML_FILES_DIRECTORY_PATH, "html-document-with-invalid-characters.html"));

			// Act
			IList<MinificationErrorInfo> errors = _minifier.Minify(input).Errors;

			// Assert
			Assert.Equal(1, errors.Count);
			Assert.Equal("In the start tag <C> found invalid characters.", errors[0].Message);
			Assert.Equal(1, errors[0].LineNumber);
			Assert.Equal(9, errors[0].ColumnNumber);
			Assert.Equal(
				"Line 1: <C�\u0003�tmh</`\u0018\u001al@\0OCTYPE \0O|�\0\0\u0010\0htmh</`�\u0001\0\0h<+`\u0006OCTFPE ElE Ml@\0\0\u0010|J\u0017h\0\u0010h1?html" + Environment.NewLine +
				"----------------^" + Environment.NewLine,
				errors[0].SourceFragment
			);
		}

		#region IDisposable implementation

		public void Dispose()
		{
			_minifier = null;
		}

		#endregion
	}
}