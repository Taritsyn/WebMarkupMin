using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class RemovingTypeAttributesTests
	{
		[Fact]
		public void RemovingJavaScriptTypeAttributes()
		{
			// Arrange
			var keepingAllJavaScriptTypeAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveJsTypeAttributes = false });
			var removingJavaScriptTypeAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveJsTypeAttributes = true });
			var keepingSomeJavaScriptTypeAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					RemoveJsTypeAttributes = true,
					PreservableAttributeList = "script[type]"
				}
			);

			const string input1 = "<script type=\"text/javascript\">alert(\"Hooray!\");</script>";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<script>alert(\"Hooray!\");</script>";
			const string targetOutput1C = input1;

			const string input2 = "<SCRIPT TYPE=\"  text/javascript \">alert(\"Hooray!\");</script>";
			const string targetOutput2A = "<script type=\"  text/javascript \">alert(\"Hooray!\");</script>";
			const string targetOutput2B = "<script>alert(\"Hooray!\");</script>";
			const string targetOutput2C = targetOutput2A;

			const string input3 = "<script type=\"text/ecmascript\">alert(\"Hooray!\");</script>";

			const string input4 = "<script type=\"text/vbscript\">MsgBox(\"Hooray!\")</script>";

			// Act
			string output1A = keepingAllJavaScriptTypeAttributesMinifier.Minify(input1).MinifiedContent;
			string output1B = removingJavaScriptTypeAttributesMinifier.Minify(input1).MinifiedContent;
			string output1C = keepingSomeJavaScriptTypeAttributesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingAllJavaScriptTypeAttributesMinifier.Minify(input2).MinifiedContent;
			string output2B = removingJavaScriptTypeAttributesMinifier.Minify(input2).MinifiedContent;
			string output2C = keepingSomeJavaScriptTypeAttributesMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingAllJavaScriptTypeAttributesMinifier.Minify(input3).MinifiedContent;
			string output3B = removingJavaScriptTypeAttributesMinifier.Minify(input3).MinifiedContent;
			string output3C = keepingSomeJavaScriptTypeAttributesMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingAllJavaScriptTypeAttributesMinifier.Minify(input4).MinifiedContent;
			string output4B = removingJavaScriptTypeAttributesMinifier.Minify(input4).MinifiedContent;
			string output4C = keepingSomeJavaScriptTypeAttributesMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(input3, output3A);
			Assert.Equal(input3, output3B);
			Assert.Equal(input3, output3C);

			Assert.Equal(input4, output4A);
			Assert.Equal(input4, output4B);
			Assert.Equal(input4, output4C);
		}

		[Fact]
		public void RemovingCssTypeAttributes()
		{
			// Arrange
			var keepingAllCssTypeAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveCssTypeAttributes = false });
			var removingCssTypeAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveCssTypeAttributes = true });
			var keepingSomeCssTypeAttributesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					RemoveCssTypeAttributes = true,
					PreservableAttributeList = "style[type],link[type=\"text/css\"]"
				}
			);

			const string input1 = "<style type=\"text/css\">.error { color: #b94a48; }</style>";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<style>.error { color: #b94a48; }</style>";
			const string targetOutput1C = input1;

			const string input2 = "<STYLE TYPE = \"  text/CSS \">body { font-size: 14px; }</style>";
			const string targetOutput2A = "<style type=\"  text/CSS \">body { font-size: 14px; }</style>";
			const string targetOutput2B = "<style>body { font-size: 14px; }</style>";
			const string targetOutput2C = targetOutput2A;

			const string input3 = "<style type=\"text/less\">\n" +
				"	@color: #4D926F;\n\n" +
				"	#header { color: @color; }\n" +
				"</style>"
				;

			const string input4 = "<link rel=\"stylesheet\" type=\"text/css\"" +
				" href=\"http://twitter.github.com/bootstrap/assets/css/bootstrap.css\">";
			const string targetOutput4A = input4;
			const string targetOutput4B = "<link rel=\"stylesheet\"" +
				" href=\"http://twitter.github.com/bootstrap/assets/css/bootstrap.css\">";
			const string targetOutput4C = input4;

			const string input5 = "<link rel=\"stylesheet/less\" type=\"text/css\" href=\"styles.less\">";

			// Act
			string output1A = keepingAllCssTypeAttributesMinifier.Minify(input1).MinifiedContent;
			string output1B = removingCssTypeAttributesMinifier.Minify(input1).MinifiedContent;
			string output1C = keepingSomeCssTypeAttributesMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingAllCssTypeAttributesMinifier.Minify(input2).MinifiedContent;
			string output2B = removingCssTypeAttributesMinifier.Minify(input2).MinifiedContent;
			string output2C = keepingSomeCssTypeAttributesMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingAllCssTypeAttributesMinifier.Minify(input3).MinifiedContent;
			string output3B = removingCssTypeAttributesMinifier.Minify(input3).MinifiedContent;
			string output3C = keepingSomeCssTypeAttributesMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingAllCssTypeAttributesMinifier.Minify(input4).MinifiedContent;
			string output4B = removingCssTypeAttributesMinifier.Minify(input4).MinifiedContent;
			string output4C = keepingSomeCssTypeAttributesMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingAllCssTypeAttributesMinifier.Minify(input5).MinifiedContent;
			string output5B = removingCssTypeAttributesMinifier.Minify(input5).MinifiedContent;
			string output5C = keepingSomeCssTypeAttributesMinifier.Minify(input5).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(input3, output3A);
			Assert.Equal(input3, output3B);
			Assert.Equal(input3, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);

			Assert.Equal(input5, output5A);
			Assert.Equal(input5, output5B);
			Assert.Equal(input5, output5C);
		}
	}
}