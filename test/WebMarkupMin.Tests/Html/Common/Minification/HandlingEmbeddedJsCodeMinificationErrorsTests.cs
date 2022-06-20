using System;
using System.Collections.Generic;

using Xunit;

using WebMarkupMin.Core;
using WebMarkupMin.MsAjax;
using WebMarkupMin.NUglify;
using WebMarkupMin.Yui;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class HandlingEmbeddedJsCodeMinificationErrorsTests : IDisposable
	{
		private HtmlMinifier _nullMinifier;
		private HtmlMinifier _crockfordMinifier;
		private HtmlMinifier _msAjaxMinifier;
		private HtmlMinifier _nuglifyMinifier;
		private HtmlMinifier _yuiMinifier;


		public HandlingEmbeddedJsCodeMinificationErrorsTests()
		{
			var settings = new HtmlMinificationSettings(true) { MinifyEmbeddedJsCode = true };

			_nullMinifier = new HtmlMinifier(settings, jsMinifier: new NullJsMinifier());
			_crockfordMinifier = new HtmlMinifier(settings, jsMinifier: new CrockfordJsMinifier());
			_msAjaxMinifier = new HtmlMinifier(settings, jsMinifier: new MsAjaxJsMinifier());
			_nuglifyMinifier = new HtmlMinifier(settings, jsMinifier: new NUglifyJsMinifier());
			_yuiMinifier = new HtmlMinifier(settings, jsMinifier: new YuiJsMinifier());
		}


		[Fact]
		public void HandlingMinificationErrorsInScriptTagIsCorrect()
		{
			// Arrange
			const string input = "<script type=\"text/javascript\">\n" +
				"    var customWindow = window.open(\"\", \"hello\", \"width=640,height=480\");\n" +
				"    customWindow.document.write(\"Hello, World!);\n" +
				"    customWindow.print(;\n" +
				"</script>"
				;

			// Act
			IList<MinificationErrorInfo> errorsA = _nullMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsB = _crockfordMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsC = _msAjaxMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsD = _nuglifyMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsE = _yuiMinifier.Minify(input).Errors;

			// Assert
			Assert.Equal(0, errorsA.Count);

			Assert.Equal(1, errorsB.Count);
			Assert.Equal("Unterminated string literal.", errorsB[0].Message);
			Assert.Equal(1, errorsB[0].LineNumber);
			Assert.Equal(32, errorsB[0].ColumnNumber);

			Assert.Equal(4, errorsC.Count);
			Assert.Equal("Unterminated string constant: \"Hello, World!);", errorsC[0].Message);
			Assert.Equal(3, errorsC[0].LineNumber);
			Assert.Equal(33, errorsC[0].ColumnNumber);
			Assert.Equal("Expected ')': customWindow", errorsC[1].Message);
			Assert.Equal(4, errorsC[1].LineNumber);
			Assert.Equal(5, errorsC[1].ColumnNumber);
			Assert.Equal("Expected expression: ;", errorsC[2].Message);
			Assert.Equal(4, errorsC[2].LineNumber);
			Assert.Equal(24, errorsC[2].ColumnNumber);
			Assert.Equal("Expected ')': ;", errorsC[3].Message);
			Assert.Equal(4, errorsC[3].LineNumber);
			Assert.Equal(24, errorsC[3].ColumnNumber);

			Assert.Equal(4, errorsD.Count);
			Assert.Equal("Unterminated string constant: \"Hello, World!);", errorsD[0].Message);
			Assert.Equal(3, errorsD[0].LineNumber);
			Assert.Equal(33, errorsD[0].ColumnNumber);
			Assert.Equal("Expected ')': customWindow", errorsD[1].Message);
			Assert.Equal(4, errorsD[1].LineNumber);
			Assert.Equal(5, errorsD[1].ColumnNumber);
			Assert.Equal("Expected expression: ;", errorsD[2].Message);
			Assert.Equal(4, errorsD[2].LineNumber);
			Assert.Equal(24, errorsD[2].ColumnNumber);
			Assert.Equal("Expected ')': ;", errorsD[3].Message);
			Assert.Equal(4, errorsD[3].LineNumber);
			Assert.Equal(24, errorsD[3].ColumnNumber);

			Assert.Equal(3, errorsE.Count);
			Assert.Equal("unterminated string literal", errorsE[0].Message);
			Assert.Equal(3, errorsE[0].LineNumber);
			Assert.Equal(48, errorsE[0].ColumnNumber);
			Assert.Equal("syntax error", errorsE[1].Message);
			Assert.Equal(3, errorsE[1].LineNumber);
			Assert.Equal(48, errorsE[1].ColumnNumber);
			Assert.Equal("syntax error", errorsE[2].Message);
			Assert.Equal(4, errorsE[2].LineNumber);
			Assert.Equal(18, errorsE[2].ColumnNumber);
		}

		[Fact]
		public void HandlingMinificationErrorsInScriptTagWithHtmlCommentIsCorrect()
		{
			// Arrange
			const string input = "<script type=\"text/javascript\">\n" +
				"    <!--\n" +
				"    var customWindow = window.open(\"\", \"hello\", \"width=640,height=480\");\n" +
				"    customWindow.document.write(\"Hello, World!);\n" +
				"    customWindow.print(;\n" +
				"    //-->\n" +
				"</script>"
				;

			// Act
			IList<MinificationErrorInfo> errorsA = _nullMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsB = _crockfordMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsC = _msAjaxMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsD = _nuglifyMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsE = _yuiMinifier.Minify(input).Errors;

			// Assert
			Assert.Equal(0, errorsA.Count);

			Assert.Equal(1, errorsB.Count);
			Assert.Equal("Unterminated string literal.", errorsB[0].Message);
			Assert.Equal(3, errorsB[0].LineNumber);
			Assert.Equal(1, errorsB[0].ColumnNumber);

			Assert.Equal(4, errorsC.Count);
			Assert.Equal("Unterminated string constant: \"Hello, World!);", errorsC[0].Message);
			Assert.Equal(4, errorsC[0].LineNumber);
			Assert.Equal(33, errorsC[0].ColumnNumber);
			Assert.Equal("Expected ')': customWindow", errorsC[1].Message);
			Assert.Equal(5, errorsC[1].LineNumber);
			Assert.Equal(5, errorsC[1].ColumnNumber);
			Assert.Equal("Expected expression: ;", errorsC[2].Message);
			Assert.Equal(5, errorsC[2].LineNumber);
			Assert.Equal(24, errorsC[2].ColumnNumber);
			Assert.Equal("Expected ')': ;", errorsC[3].Message);
			Assert.Equal(5, errorsC[3].LineNumber);
			Assert.Equal(24, errorsC[3].ColumnNumber);

			Assert.Equal(4, errorsD.Count);
			Assert.Equal("Unterminated string constant: \"Hello, World!);", errorsD[0].Message);
			Assert.Equal(4, errorsD[0].LineNumber);
			Assert.Equal(33, errorsD[0].ColumnNumber);
			Assert.Equal("Expected ')': customWindow", errorsD[1].Message);
			Assert.Equal(5, errorsD[1].LineNumber);
			Assert.Equal(5, errorsD[1].ColumnNumber);
			Assert.Equal("Expected expression: ;", errorsD[2].Message);
			Assert.Equal(5, errorsD[2].LineNumber);
			Assert.Equal(24, errorsD[2].ColumnNumber);
			Assert.Equal("Expected ')': ;", errorsD[3].Message);
			Assert.Equal(5, errorsD[3].LineNumber);
			Assert.Equal(24, errorsD[3].ColumnNumber);

			Assert.Equal(3, errorsE.Count);
			Assert.Equal("unterminated string literal", errorsE[0].Message);
			Assert.Equal(4, errorsE[0].LineNumber);
			Assert.Equal(48, errorsE[0].ColumnNumber);
			Assert.Equal("syntax error", errorsE[1].Message);
			Assert.Equal(4, errorsE[1].LineNumber);
			Assert.Equal(48, errorsE[1].ColumnNumber);
			Assert.Equal("syntax error", errorsE[2].Message);
			Assert.Equal(5, errorsE[2].LineNumber);
			Assert.Equal(18, errorsE[2].ColumnNumber);
		}

		[Fact]
		public void HandlingMinificationErrorsInScriptTagWithCdataSectionIsCorrect()
		{
			// Arrange
			const string input = "<script type=\"text/javascript\">\n" +
				"    //<![CDATA[\n" +
				"    var customWindow = window.open(\"\", \"hello\", \"width=640,height=480\");\n" +
				"    customWindow.document.write(\"Hello, World!);\n" +
				"    customWindow.print(;\n" +
				"    //]]>\n" +
				"</script>"
				;

			// Act
			IList<MinificationErrorInfo> errorsA = _nullMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsB = _crockfordMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsC = _msAjaxMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsD = _nuglifyMinifier.Minify(input).Errors;
			IList<MinificationErrorInfo> errorsE = _yuiMinifier.Minify(input).Errors;

			// Assert
			Assert.Equal(0, errorsA.Count);

			Assert.Equal(1, errorsB.Count);
			Assert.Equal("Unterminated string literal.", errorsB[0].Message);
			Assert.Equal(3, errorsB[0].LineNumber);
			Assert.Equal(1, errorsB[0].ColumnNumber);

			Assert.Equal(4, errorsC.Count);
			Assert.Equal("Unterminated string constant: \"Hello, World!);", errorsC[0].Message);
			Assert.Equal(4, errorsC[0].LineNumber);
			Assert.Equal(33, errorsC[0].ColumnNumber);
			Assert.Equal("Expected ')': customWindow", errorsC[1].Message);
			Assert.Equal(5, errorsC[1].LineNumber);
			Assert.Equal(5, errorsC[1].ColumnNumber);
			Assert.Equal("Expected expression: ;", errorsC[2].Message);
			Assert.Equal(5, errorsC[2].LineNumber);
			Assert.Equal(24, errorsC[2].ColumnNumber);
			Assert.Equal("Expected ')': ;", errorsC[3].Message);
			Assert.Equal(5, errorsC[3].LineNumber);
			Assert.Equal(24, errorsC[3].ColumnNumber);

			Assert.Equal(4, errorsD.Count);
			Assert.Equal("Unterminated string constant: \"Hello, World!);", errorsD[0].Message);
			Assert.Equal(4, errorsD[0].LineNumber);
			Assert.Equal(33, errorsD[0].ColumnNumber);
			Assert.Equal("Expected ')': customWindow", errorsD[1].Message);
			Assert.Equal(5, errorsD[1].LineNumber);
			Assert.Equal(5, errorsD[1].ColumnNumber);
			Assert.Equal("Expected expression: ;", errorsD[2].Message);
			Assert.Equal(5, errorsD[2].LineNumber);
			Assert.Equal(24, errorsD[2].ColumnNumber);
			Assert.Equal("Expected ')': ;", errorsD[3].Message);
			Assert.Equal(5, errorsD[3].LineNumber);
			Assert.Equal(24, errorsD[3].ColumnNumber);

			Assert.Equal(3, errorsE.Count);
			Assert.Equal("unterminated string literal", errorsE[0].Message);
			Assert.Equal(4, errorsE[0].LineNumber);
			Assert.Equal(48, errorsE[0].ColumnNumber);
			Assert.Equal("syntax error", errorsE[1].Message);
			Assert.Equal(4, errorsE[1].LineNumber);
			Assert.Equal(48, errorsE[1].ColumnNumber);
			Assert.Equal("syntax error", errorsE[2].Message);
			Assert.Equal(5, errorsE[2].LineNumber);
			Assert.Equal(18, errorsE[2].ColumnNumber);
		}

		#region IDisposable implementation

		public void Dispose()
		{
			_nullMinifier = null;
			_crockfordMinifier = null;
			_msAjaxMinifier = null;
			_nuglifyMinifier = null;
			_yuiMinifier = null;
		}

		#endregion
	}
}