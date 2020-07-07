using System.Collections.Generic;

using Xunit;

namespace WebMarkupMin.Core.Test.Xhtml.Minification
{
	public class ProcessingBooleanAttributesTests
	{
		[Fact]
		public void ProcessingCustomBooleanAttributesIsCorrect()
		{
			// Arrange
			var minifier = new XhtmlMinifier(new XhtmlMinificationSettings(true));

			const string input1 = "<div custom-attribute></div>";
			const string targetOutput1 = "<div custom-attribute=\"\"></div>";

			const string input2 = "<div class></div>";
			const string targetOutput2 = "<div class=\"\"></div>";

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
		}
	}
}