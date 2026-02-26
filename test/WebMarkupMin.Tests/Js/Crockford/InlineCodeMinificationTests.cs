using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Js.Crockford
{
	public class InlineCodeMinificationTests
	{
		[Fact]
		public void RegularExpressionLiteralsMinification()
		{
			// Arrange
			var minifier = new CrockfordJsMinifier();

			const string input1 = @"/^\//";
			const string input2 = @"/^\//.test(""/"")";

			// Act
			string output1 = minifier.Minify(input1, true).MinifiedContent;
			string output2 = minifier.Minify(input2, true).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(input2, output2);
		}
	}
}