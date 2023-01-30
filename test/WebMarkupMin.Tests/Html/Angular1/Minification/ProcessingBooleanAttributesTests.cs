using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Angular1.Minification
{
	public class ProcessingBooleanAttributesTests
	{
		[Fact]
		public void ProcessingCustomBooleanAttributes()
		{
			// Arrange
			var keepingBooleanAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { CollapseBooleanAttributes = false });
			var collapsingBooleanAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { CollapseBooleanAttributes = true });

			const string input1 = "<html ng-app>\n" +
				"	<body ng-controller=\"MyController\">\n" +
				"		<input ng-model=\"foo\" value=\"bar\">\n" +
				"				<button ng-click=\"changeFoo()\">{{buttonText}}</button>\n" +
				"		<script src=\"angular.js\"></script>\n" +
				"	</body>\n" +
				"</html>"
				;
			const string input2 = "<div ng-include src=\"views/sidepanel.html\"></div>";
			const string input3 = "<div ng:include src=\"views/sidepanel.html\"></div>";
			const string input4 = "<div ng_include src=\"views/sidepanel.html\"></div>";
			const string input5 = "<div x-ng-include src=\"views/sidepanel.html\"></div>";
			const string input6 = "<div data-ng-include src=\"views/sidepanel.html\"></div>";
			const string input7 = "<div ng-include=\"\" src=\"views/sidepanel.html\"></div>";

			// Act
			string output1A = keepingBooleanAttributesMinifier.Minify(input1).MinifiedContent;
			string output1B = collapsingBooleanAttributesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingBooleanAttributesMinifier.Minify(input2).MinifiedContent;
			string output2B = collapsingBooleanAttributesMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingBooleanAttributesMinifier.Minify(input3).MinifiedContent;
			string output3B = collapsingBooleanAttributesMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingBooleanAttributesMinifier.Minify(input4).MinifiedContent;
			string output4B = collapsingBooleanAttributesMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingBooleanAttributesMinifier.Minify(input5).MinifiedContent;
			string output5B = collapsingBooleanAttributesMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingBooleanAttributesMinifier.Minify(input6).MinifiedContent;
			string output6B = collapsingBooleanAttributesMinifier.Minify(input6).MinifiedContent;

			string output7A = keepingBooleanAttributesMinifier.Minify(input7).MinifiedContent;
			string output7B = collapsingBooleanAttributesMinifier.Minify(input7).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1A);
			Assert.Equal(input1, output1B);

			Assert.Equal(input2, output2A);
			Assert.Equal(input2, output2B);

			Assert.Equal(input3, output3A);
			Assert.Equal(input3, output3B);

			Assert.Equal(input4, output4A);
			Assert.Equal(input4, output4B);

			Assert.Equal(input5, output5A);
			Assert.Equal(input5, output5B);

			Assert.Equal(input6, output6A);
			Assert.Equal(input6, output6B);

			Assert.Equal(input7, output7A);
			Assert.Equal(input7, output7B);
		}
	}
}