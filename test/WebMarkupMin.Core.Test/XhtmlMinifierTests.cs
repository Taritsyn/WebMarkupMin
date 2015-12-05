using System.Collections.Generic;

using Xunit;

namespace WebMarkupMin.Core.Test
{
	public class XhtmlMinifierTests : MarkupMinifierTestsBase
	{
		#region Processing XML nodes
		[Fact]
		public void ProcessingXmlNodesIsCorrect()
		{
			// Arrange
			var minifier = new XhtmlMinifier(new XhtmlMinificationSettings(true));

			const string input1 = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\n" +
				"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\"\n" +
				"  \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\n" +
				"<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\" " +
				"xmlns:bi=\"urn:schemas-microsoft-com:mscom:bi\">\n" +
				"	<head>\n" +
				"		<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\n" +
				"		<title>Some title...</title>\n" +
				"	</head>\n" +
				"	<body>\n" +
				"		<div id=\"content\">\n" +
				"			<p>Some text...</p>\n" +
				"		</div>\n" +
				"	</body>\n" +
				"</html>"
				;
			const string targetOutput1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
				"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" " +
				"\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\n" +
				"<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\" " +
				"xmlns:bi=\"urn:schemas-microsoft-com:mscom:bi\">\n" +
				"	<head>\n" +
				"		<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\n" +
				"		<title>Some title...</title>\n" +
				"	</head>\n" +
				"	<body>\n" +
				"		<div id=\"content\">\n" +
				"			<p>Some text...</p>\n" +
				"		</div>\n" +
				"	</body>\n" +
				"</html>"
				;

			const string input2 = "<svg version=\"1.1\" baseProfile=\"full\" " +
				"xmlns=\"http://www.w3.org/2000/svg\" " +
				"xmlns:xlink=\"http://www.w3.org/1999/xlink\" " +
				"xmlns:ev=\"http://www.w3.org/2001/xml-events\" " +
				"width=\"100%\" height=\"100%\">\n" +
				"	<rect fill=\"white\" x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" />\n" +
				"	<rect fill=\"silver\" x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" rx=\"1em\" />\n" +
				"</svg>"
				;

			const string input3 = "<math xmlns=\"http://www.w3.org/1998/Math/MathML\">\n" +
				"	<infinity />\n" +
				"</math>"
				;

			// Act
			MarkupMinificationResult result1 = minifier.Minify(input1);
			string output1 = result1.MinifiedContent;
			IList<MinificationErrorInfo> warnings1 = result1.Warnings;

			MarkupMinificationResult result2 = minifier.Minify(input2);
			string output2 = result2.MinifiedContent;
			IList<MinificationErrorInfo> warnings2 = result2.Warnings;

			MarkupMinificationResult result3 = minifier.Minify(input3);
			string output3 = result3.MinifiedContent;
			IList<MinificationErrorInfo> warnings3 = result3.Warnings;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(0, warnings1.Count);

			Assert.Equal(input2, output2);
			Assert.Equal(0, warnings2.Count);

			Assert.Equal(input3, output3);
			Assert.Equal(0, warnings3.Count);
		}
		#endregion

		#region Processing custom boolean attributes
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

			const string input8 = "<div class></div>";
			const string targetOutput8 = "<div class=\"\"></div>";

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			string output3 = minifier.Minify(input3).MinifiedContent;
			string output4 = minifier.Minify(input4).MinifiedContent;
			string output5 = minifier.Minify(input5).MinifiedContent;
			string output6 = minifier.Minify(input6).MinifiedContent;
			string output7 = minifier.Minify(input7).MinifiedContent;
			string output8 = minifier.Minify(input8).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
			Assert.Equal(targetOutput6, output6);
			Assert.Equal(targetOutput7, output7);
			Assert.Equal(targetOutput8, output8);
		}
		#endregion
	}
}