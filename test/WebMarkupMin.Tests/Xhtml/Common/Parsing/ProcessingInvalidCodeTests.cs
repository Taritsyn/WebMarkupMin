using System.Collections.Generic;

using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Xhtml.Common.Parsing
{
	public class ProcessingInvalidCodeTests
	{
		[Fact]
		public void ProcessingInvalidCharactersInXmlDeclaration()
		{
			// Arrange
			var minifier = new XhtmlMinifier(new XhtmlMinificationSettings(true));

			const string input1 = "\n\n\t<?xml version=\"1.0\" encoding=\"windows-1251 standalone=\"no\"?>\n";
			const string input2 = "\r\n  <?xml version=\"1.1\" ;$%? encoding=\"windows-1251\" ?>\r\n";

			// Act
			IList<MinificationErrorInfo> errors1 = minifier.Minify(input1).Errors;
			IList<MinificationErrorInfo> errors2 = minifier.Minify(input2).Errors;

			// Assert
			Assert.Equal(1, errors1.Count);
			Assert.Equal(3, errors1[0].LineNumber);
			Assert.Equal(57, errors1[0].ColumnNumber);

			Assert.Equal(1, errors2.Count);
			Assert.Equal(2, errors2[0].LineNumber);
			Assert.Equal(23, errors2[0].ColumnNumber);
		}
	}
}