using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class CollapsingEnumeratedAttributesTests
	{
		[Fact]
		public void CollapsingHiddenAttributes()
		{
			// Arrange
			var collapsingBooleanAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { CollapseBooleanAttributes = true });

			const string input1 = "<span hidden></span>";
			const string targetOutput1 = "<span hidden></span>";

			const string input2 = "<span hidden=\"\"></span>";
			const string targetOutput2 = "<span hidden></span>";

			const string input3 = "<span hidden=\"hidden\"></span>";
			const string targetOutput3 = "<span hidden></span>";

			const string input4 = "<span hidden=\"until-found\"></span>";
			const string targetOutput4 = "<span hidden=\"until-found\"></span>";

			const string input5 = "<span hidden=\"  until-found  \"></span>";
			const string targetOutput5 = "<span hidden></span>";

			const string input6 = "<span hidden=\"UNTIL-FOUND\"></span>";
			const string targetOutput6 = "<span hidden=\"UNTIL-FOUND\"></span>";

			const string input7 = "<span hidden=\"some-value\"></span>";
			const string targetOutput7 = "<span hidden></span>";

			// Act
			string output1 = collapsingBooleanAttributesMinifier.Minify(input1).MinifiedContent;
			string output2 = collapsingBooleanAttributesMinifier.Minify(input2).MinifiedContent;
			string output3 = collapsingBooleanAttributesMinifier.Minify(input3).MinifiedContent;
			string output4 = collapsingBooleanAttributesMinifier.Minify(input4).MinifiedContent;
			string output5 = collapsingBooleanAttributesMinifier.Minify(input5).MinifiedContent;
			string output6 = collapsingBooleanAttributesMinifier.Minify(input6).MinifiedContent;
			string output7 = collapsingBooleanAttributesMinifier.Minify(input7).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
			Assert.Equal(targetOutput6, output6);
			Assert.Equal(targetOutput7, output7);
		}
	}
}