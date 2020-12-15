using Xunit;

namespace WebMarkupMin.Core.Tests.Xhtml.Angular1.Minification
{
	public class ProcessingBooleanAttributesTests
	{
		[Fact]
		public void ProcessingCustomBooleanAttributesIsCorrect()
		{
			// Arrange
			var minifier = new XhtmlMinifier(new XhtmlMinificationSettings(true));

			const string input1 = "<html ng-app>\n" +
				"	<body ng-controller=\"MyController\">\n" +
				"		<input ng-model=\"foo\" value=\"bar\" />\n" +
				"				<button ng-click=\"changeFoo()\">{{buttonText}}</button>\n" +
				"		<script src=\"angular.js\"></script>\n" +
				"	</body>\n" +
				"</html>"
				;
			const string targetOutput1 = "<html ng-app=\"\">\n" +
				"	<body ng-controller=\"MyController\">\n" +
				"		<input ng-model=\"foo\" value=\"bar\" />\n" +
				"				<button ng-click=\"changeFoo()\">{{buttonText}}</button>\n" +
				"		<script src=\"angular.js\"></script>\n" +
				"	</body>\n" +
				"</html>"
				;

			const string input2 = "<div ng-include src=\"views/sidepanel.html\"></div>";
			const string targetOutput2 = "<div ng-include=\"\" src=\"views/sidepanel.html\"></div>";

			const string input3 = "<div ng:include src=\"views/sidepanel.html\"></div>";
			const string targetOutput3 = "<div ng:include=\"\" src=\"views/sidepanel.html\"></div>";

			const string input4 = "<div ng_include src=\"views/sidepanel.html\"></div>";
			const string targetOutput4 = "<div ng_include=\"\" src=\"views/sidepanel.html\"></div>";

			const string input5 = "<div x-ng-include src=\"views/sidepanel.html\"></div>";
			const string targetOutput5 = "<div x-ng-include=\"\" src=\"views/sidepanel.html\"></div>";

			const string input6 = "<div data-ng-include src=\"views/sidepanel.html\"></div>";
			const string targetOutput6 = "<div data-ng-include=\"\" src=\"views/sidepanel.html\"></div>";

			const string input7 = "<div ng-include=\"\" src=\"views/sidepanel.html\"></div>";
			const string targetOutput7 = input7;

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			string output3 = minifier.Minify(input3).MinifiedContent;
			string output4 = minifier.Minify(input4).MinifiedContent;
			string output5 = minifier.Minify(input5).MinifiedContent;
			string output6 = minifier.Minify(input6).MinifiedContent;
			string output7 = minifier.Minify(input7).MinifiedContent;

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