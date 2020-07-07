using Xunit;

namespace WebMarkupMin.Core.Test.Html.Angular1.Minification
{
	public class ProcessingCommentDirectivesTests
	{
		[Fact]
		public void ProcessingCommentDirectivesIsCorrect()
		{
			// Arrange
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = true });

			const string input1 = "<div ng-app=\"superhero\">\n" +
				"	<!--\t directive: superman \t-->\n" +
				"</div>"
				;
			const string targetOutput1 = "<div ng-app=\"superhero\">\n" +
				"	<!--directive:superman -->\n" +
				"</div>"
				;

			const string input2 = "<body ng-controller=\"MainCtrl\">\n" +
				"	<!--directive:date-picker -->\n" +
				"</body>"
				;
			const string targetOutput2 = input2;

			const string input3 = "<!-- directive: my-dir exp -->";
			const string targetOutput3 = "<!--directive:my-dir exp-->";

			// Act
			string output1 = removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;
			string output2 = removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;
			string output3 = removingHtmlCommentsMinifier.Minify(input3).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
		}
	}
}