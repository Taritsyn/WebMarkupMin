using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Aurelia.Minification
{
	public class RemovingTagsWithoutContentTests
	{
		[Fact]
		public void RemovingTagsWithoutContentIsCorrect()
		{
			// Arrange
			var removingTagsWithoutContentMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveTagsWithoutContent = true });

			const string input1 = "<div aurelia-app></div>";
			const string input2 = "<router-view></router-view>";

			// Act
			string output1 = removingTagsWithoutContentMinifier.Minify(input1).MinifiedContent;
			string output2 = removingTagsWithoutContentMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(input2, output2);
		}
	}
}