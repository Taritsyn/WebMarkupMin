using Xunit;

namespace WebMarkupMin.Core.Test.Html.Aurelia
{
	public class ParsingTests
	{
		[Fact]
		public void ParsingIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<input type=\"text\" value.bind=\"firstName\">";
			const string input2 = "<img src.bind=\"\t  typeof data.thumbnail !== 'undefined' ? data.thumbnail  :  '';  \t\">";
			const string input3 = "<ul>\n" +
				"	<li repeat.for=\"post of posts\">\n" +
				"		<img src.bind=\"post.data.thumbnail\">\n" +
				"	</li>\n" +
				"</ul>"
				;
			const string input4 = "<form submit.trigger=\"\t  submit();  \t\"></form>";
			const string input5 = "<ul>\n" +
				"	<li repeat.for=\"navItem of router.navigation\" class=\"${\t  navItem.isActive ? 'active'  :  '';  \t}\">\n" +
				"		<a href=\"http://mysite.com${\t  navItem.href  \t}\">\n" +
				"			${\t  navItem.title  \t}\n" +
				"		</a>\n" +
				"	</li>\n" +
				"</ul>"
				;

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			string output3 = minifier.Minify(input3).MinifiedContent;
			string output4 = minifier.Minify(input4).MinifiedContent;
			string output5 = minifier.Minify(input5).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(input2, output2);
			Assert.Equal(input3, output3);
			Assert.Equal(input4, output4);
			Assert.Equal(input5, output5);
		}
	}
}