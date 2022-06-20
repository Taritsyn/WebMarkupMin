using System;
using System.Collections.Generic;

using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class HandlingEmbeddedJsonDataMinificationErrorsTests : IDisposable
	{
		private HtmlMinifier _minifier;


		public HandlingEmbeddedJsonDataMinificationErrorsTests()
		{
			_minifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					MinifyEmbeddedJsonData = true
				}
			);
		}


		[Fact]
		public void HandlingMinificationErrorsInScriptTagIsCorrect()
		{
			// Arrange
			const string input = "<script type=\"application/json\">\n" +
				"{\n" +
				"  \"stream\": {\n" +
				"    \"bpos\": 2,\n" +
				"    \"clusterAdCount\": \"0,\n" +
				"    \"cpos\": 10,\n" +
				"    \"cposy\": 19\n" +
				"  \n" +
				"}\n" +
				"</script>"
				;

			// Act
			IList<MinificationErrorInfo> errors = _minifier.Minify(input).Errors;

			// Assert
			Assert.Equal(1, errors.Count);
			Assert.Equal("Unterminated string literal.", errors[0].Message);
			Assert.Equal(1, errors[0].LineNumber);
			Assert.Equal(33, errors[0].ColumnNumber);
		}

		[Fact]
		public void HandlingMinificationErrorsInScriptTagWithHtmlCommentIsCorrect()
		{
			// Arrange
			const string input = "<script type=\"application/json\">\n" +
				"<!--\n" +
				"{\n" +
				"  \"stream\": {\n" +
				"    \"bpos\": 2,\n" +
				"    \"clusterAdCount\": \"0,\n" +
				"    \"cpos\": 10,\n" +
				"    \"cposy\": 19\n" +
				"  \n" +
				"}\n" +
				"-->\n" +
				"</script>"
				;

			// Act
			IList<MinificationErrorInfo> errors = _minifier.Minify(input).Errors;

			// Assert
			Assert.Equal(1, errors.Count);
			Assert.Equal("Unterminated string literal.", errors[0].Message);
			Assert.Equal(3, errors[0].LineNumber);
			Assert.Equal(1, errors[0].ColumnNumber);
		}

		[Fact]
		public void HandlingMinificationErrorsInScriptTagWithCdataSectionIsCorrect()
		{
			// Arrange
			const string input = "<script type=\"application/json\">\n" +
				"<![CDATA[\n" +
				"{\n" +
				"  \"stream\": {\n" +
				"    \"bpos\": 2,\n" +
				"    \"clusterAdCount\": \"0,\n" +
				"    \"cpos\": 10,\n" +
				"    \"cposy\": 19\n" +
				"  \n" +
				"}\n" +
				"]]>\n" +
				"</script>"
				;

			// Act
			IList<MinificationErrorInfo> errors = _minifier.Minify(input).Errors;

			// Assert
			Assert.Equal(1, errors.Count);
			Assert.Equal("Unterminated string literal.", errors[0].Message);
			Assert.Equal(3, errors[0].LineNumber);
			Assert.Equal(1, errors[0].ColumnNumber);
		}

		#region IDisposable implementation

		public void Dispose()
		{
			_minifier = null;
		}

		#endregion
	}
}