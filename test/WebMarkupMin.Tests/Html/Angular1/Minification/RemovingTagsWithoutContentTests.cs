using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Angular1.Minification
{
	public class RemovingTagsWithoutContentTests
	{
		[Fact]
		public void RemovingTagsWithoutContent()
		{
			// Arrange
			var removingTagsWithoutContentMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveTagsWithoutContent = true });

			const string input = "<div ng-app></div>";

			// Act
			string output = removingTagsWithoutContentMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(input, output);
		}
	}
}