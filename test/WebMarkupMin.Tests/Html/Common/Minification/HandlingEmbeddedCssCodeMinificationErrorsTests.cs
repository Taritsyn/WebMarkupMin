using System;
using System.Collections.Generic;

using Xunit;

using WebMarkupMin.Core;
using WebMarkupMin.MsAjax;
using WebMarkupMin.NUglify;
using WebMarkupMin.Yui;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class HandlingEmbeddedCssCodeMinificationErrorsTests : IDisposable
	{
		private HtmlMinifier _nullMinifier;
		private HtmlMinifier _kristensenMinifier;
		private HtmlMinifier _msAjaxMinifier;
		private HtmlMinifier _nuglifyMinifier;
		private HtmlMinifier _yuiMinifier;


		public HandlingEmbeddedCssCodeMinificationErrorsTests()
		{
			var settings = new HtmlMinificationSettings(true) { MinifyEmbeddedCssCode = true };

			_nullMinifier = new HtmlMinifier(settings, cssMinifier: new NullCssMinifier());
			_kristensenMinifier = new HtmlMinifier(settings, cssMinifier: new KristensenCssMinifier());
			_msAjaxMinifier = new HtmlMinifier(settings, cssMinifier: new MsAjaxCssMinifier());
			_nuglifyMinifier = new HtmlMinifier(settings, cssMinifier: new NUglifyCssMinifier());
			_yuiMinifier = new HtmlMinifier(settings, cssMinifier: new YuiCssMinifier());
		}


		[Fact]
		public void HandlingMinificationErrorsInStyleTag()
		{
			// Arrange
			const string input = "<style type=\"text/css\">\n" +
				"    .main-content a:visited::before {\n" +
				"        content: \"\\2713 ;\n" +
				"    }\n\n" +
				"    .footer {\n" +
				"        background-color: #eee;\n" +
				"        rgba(85, 85, 85, 1;\n" +
				"        text-decoration: none;\n" +
				"    }\n" +
				"</style>"
				;

			// Act
			IList<MinificationErrorInfo> errorsA = _nullMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsB = _kristensenMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsC = _msAjaxMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsD = _nuglifyMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsE = _yuiMinifier.Minify(input).Errors;

			// Assert
			Assert.Equal(0, errorsA.Count);

			Assert.Equal(0, errorsB.Count);

			Assert.Equal(3, errorsC.Count);
			Assert.Equal("Unterminated string: \"✓;", errorsC[0].Message);
			Assert.Equal(4, errorsC[0].LineNumber);
			Assert.Equal(0, errorsC[0].ColumnNumber);
			Assert.Equal("Expected expression, found '\"✓;\r\n'", errorsC[1].Message);
			Assert.Equal(3, errorsC[1].LineNumber);
			Assert.Equal(18, errorsC[1].ColumnNumber);
			Assert.Equal("Expected semicolon or closing curly-brace, found 'rgba('", errorsC[2].Message);
			Assert.Equal(8, errorsC[2].LineNumber);
			Assert.Equal(9, errorsC[2].ColumnNumber);

			Assert.Equal(3, errorsD.Count);
			Assert.Equal("Unterminated string: \"✓;", errorsD[0].Message);
			Assert.Equal(3, errorsD[0].LineNumber);
			Assert.Equal(25, errorsD[0].ColumnNumber);
			Assert.Equal("Expected expression, found '\"✓;\r\n'", errorsD[1].Message);
			Assert.Equal(3, errorsD[1].LineNumber);
			Assert.Equal(18, errorsD[1].ColumnNumber);
			Assert.Equal("Expected semicolon or closing curly-brace, found 'rgba('", errorsD[2].Message);
			Assert.Equal(8, errorsD[2].LineNumber);
			Assert.Equal(9, errorsD[2].ColumnNumber);

			Assert.Equal(0, errorsE.Count);
		}

		[Fact]
		public void HandlingMinificationErrorsInStyleTagWithHtmlComment()
		{
			// Arrange
			const string input = "<style type=\"text/css\">\n" +
				"    <!--\n" +
				"    .main-content a:visited::before {\n" +
				"        content: \"\\2713 ;\n" +
				"    }\n\n" +
				"    .footer {\n" +
				"        background-color: #eee;\n" +
				"        rgba(85, 85, 85, 1;\n" +
				"        text-decoration: none;\n" +
				"    }\n" +
				"    -->\n" +
				"</style>"
				;

			// Act
			IList<MinificationErrorInfo> errorsA = _nullMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsB = _kristensenMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsC = _msAjaxMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsD = _nuglifyMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsE = _yuiMinifier.Minify(input).Errors;

			// Assert
			Assert.Equal(0, errorsA.Count);

			Assert.Equal(0, errorsB.Count);

			Assert.Equal(3, errorsC.Count);
			Assert.Equal("Unterminated string: \"✓;", errorsC[0].Message);
			Assert.Equal(5, errorsC[0].LineNumber);
			Assert.Equal(0, errorsC[0].ColumnNumber);
			Assert.Equal("Expected expression, found '\"✓;\r\n'", errorsC[1].Message);
			Assert.Equal(4, errorsC[1].LineNumber);
			Assert.Equal(18, errorsC[1].ColumnNumber);
			Assert.Equal("Expected semicolon or closing curly-brace, found 'rgba('", errorsC[2].Message);
			Assert.Equal(9, errorsC[2].LineNumber);
			Assert.Equal(9, errorsC[2].ColumnNumber);

			Assert.Equal(3, errorsD.Count);
			Assert.Equal("Unterminated string: \"✓;", errorsD[0].Message);
			Assert.Equal(4, errorsD[0].LineNumber);
			Assert.Equal(25, errorsD[0].ColumnNumber);
			Assert.Equal("Expected expression, found '\"✓;\r\n'", errorsD[1].Message);
			Assert.Equal(4, errorsD[1].LineNumber);
			Assert.Equal(18, errorsD[1].ColumnNumber);
			Assert.Equal("Expected semicolon or closing curly-brace, found 'rgba('", errorsD[2].Message);
			Assert.Equal(9, errorsD[2].LineNumber);
			Assert.Equal(9, errorsD[2].ColumnNumber);

			Assert.Equal(0, errorsE.Count);
		}

		[Fact]
		public void HandlingMinificationErrorsInStyleTagWithCdataSection()
		{
			// Arrange
			const string input = "<style type=\"text/css\">\n" +
				"    /*<![CDATA[*/\n" +
				"    .main-content a:visited::before {\n" +
				"        content: \"\\2713 ;\n" +
				"    }\n\n" +
				"    .footer {\n" +
				"        background-color: #eee;\n" +
				"        rgba(85, 85, 85, 1;\n" +
				"        text-decoration: none;\n" +
				"    }\n" +
				"    /*]]>*/\n" +
				"</style>"
				;

			// Act
			IList<MinificationErrorInfo> errorsA = _nullMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsB = _kristensenMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsC = _msAjaxMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsD = _nuglifyMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsE = _yuiMinifier.Minify(input).Errors;

			// Assert
			Assert.Equal(0, errorsA.Count);

			Assert.Equal(0, errorsB.Count);

			Assert.Equal(3, errorsC.Count);
			Assert.Equal("Unterminated string: \"✓;", errorsC[0].Message);
			Assert.Equal(5, errorsC[0].LineNumber);
			Assert.Equal(0, errorsC[0].ColumnNumber);
			Assert.Equal("Expected expression, found '\"✓;\r\n'", errorsC[1].Message);
			Assert.Equal(4, errorsC[1].LineNumber);
			Assert.Equal(18, errorsC[1].ColumnNumber);
			Assert.Equal("Expected semicolon or closing curly-brace, found 'rgba('", errorsC[2].Message);
			Assert.Equal(9, errorsC[2].LineNumber);
			Assert.Equal(9, errorsC[2].ColumnNumber);

			Assert.Equal(3, errorsD.Count);
			Assert.Equal("Unterminated string: \"✓;", errorsD[0].Message);
			Assert.Equal(4, errorsD[0].LineNumber);
			Assert.Equal(25, errorsD[0].ColumnNumber);
			Assert.Equal("Expected expression, found '\"✓;\r\n'", errorsD[1].Message);
			Assert.Equal(4, errorsD[1].LineNumber);
			Assert.Equal(18, errorsD[1].ColumnNumber);
			Assert.Equal("Expected semicolon or closing curly-brace, found 'rgba('", errorsD[2].Message);
			Assert.Equal(9, errorsD[2].LineNumber);
			Assert.Equal(9, errorsD[2].ColumnNumber);

			Assert.Equal(0, errorsE.Count);
		}

		#region IDisposable implementation

		public void Dispose()
		{
			_nullMinifier = null;
			_kristensenMinifier = null;
			_msAjaxMinifier = null;
			_nuglifyMinifier = null;
			_yuiMinifier = null;
		}

		#endregion
	}
}