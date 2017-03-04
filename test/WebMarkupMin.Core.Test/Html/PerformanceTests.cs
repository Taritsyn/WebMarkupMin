using System;
using System.Text;

using Xunit;

namespace WebMarkupMin.Core.Test.Html
{
	public class PerformanceTests
	{
		[Fact]
		public void ProcessingHtmlCommentsInScriptsIsFast()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true), jsMinifier: new NullJsMinifier());

			var largeJsonDataBuilder = new StringBuilder();
			largeJsonDataBuilder.Append("{");

			for (int num = 1; num <= 30; num++)
			{
				if (num > 1)
				{
					largeJsonDataBuilder.Append(",");
				}
				largeJsonDataBuilder.AppendFormat(@"""product{0}"":" +
					@"{{""ProductAttributes"":[],""ProductCodes"":[],""Attributes"":[]," +
					@"""maximumQuantity"":2147483647,""productId"":{0},""productName"":""Изделие №{0}""," +
					@"""hasInStockCodes"":false,""hasRequestStockCodes"":false,""inStock"":true," +
					@"""basketQuantity"":6}}", num);
			}

			largeJsonDataBuilder.Append("}");

			string largeJsonData = largeJsonDataBuilder.ToString();

			string correctInput = "<script>\n" +
				"/* Comment */\n" +
				"var products = " + largeJsonData + ";\n" +
				"</script>"
				;
			string wrongInput = "<script>\n" +
				"<!-- Comment -->\n" +
				"var products = " + largeJsonData + ";\n" +
				"</script>"
				;

			// Act
			int correctDuration = minifier.Minify(correctInput, true).Statistics.MinificationDuration;
			int wrongDuration = minifier.Minify(wrongInput, true).Statistics.MinificationDuration;

			// Assert
			Assert.True(Math.Abs(wrongDuration - correctDuration) <= 50);
		}
	}
}