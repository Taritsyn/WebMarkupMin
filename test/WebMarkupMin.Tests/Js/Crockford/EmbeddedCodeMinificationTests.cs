using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Js.Crockford
{
	public class EmbeddedCodeMinificationTests
	{
		[Fact]
		public void RegularExpressionLiteralsMinification()
		{
			// Arrange
			var minifier = new CrockfordJsMinifier();

			const string input1 = "\n    /^\\//\n";
			const string targetOutput1 = "/^\\//";

			const string input2 = "\r    /^\\// // backslash regular expression\r";
			const string targetOutput2 = "/^\\//";

			const string input3 = "\r\n\t/^\\//.test('/')\r\n";
			const string targetOutput3 = "/^\\//.test('/')";

			const string input4 = "\n    var backSlashRegEx = /^\\//\n";
			const string targetOutput4 = "var backSlashRegEx=/^\\//";

			const string input5 = "\r\tvar result = /^\\//.test(\"/\")\r";
			const string targetOutput5 = "var result=/^\\//.test(\"/\")";

			const string input6 = "\r\n" +
				"if (/^\\//.test(\"/\")) {\r\n" +
				"    alert(\"Works!\");\r\n" +
				"}\r\n"
				;
			const string targetOutput6 = "if(/^\\//.test(\"/\")){alert(\"Works!\");}";

			// Act
			string output1 = minifier.Minify(input1, false).MinifiedContent;
			string output2 = minifier.Minify(input2, false).MinifiedContent;
			string output3 = minifier.Minify(input3, false).MinifiedContent;
			string output4 = minifier.Minify(input4, false).MinifiedContent;
			string output5 = minifier.Minify(input5, false).MinifiedContent;
			string output6 = minifier.Minify(input6, false).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
			Assert.Equal(targetOutput6, output6);
		}
	}
}