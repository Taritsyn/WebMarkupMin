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

			const string input1 = "<script type=\"\">alert(\"Hooray!\");</script>";

			const string input2 = "<script type>alert(\"Hooray!\");</script>";

			const string input3 = "<script type=\"text/javascript\">alert(\"Hooray!\");</script>";
			const string targetOutput3A = input3;
			const string targetOutput3B = "<script>alert(\"Hooray!\");</script>";
			const string targetOutput3C = input3;

			const string input4 = "<SCRIPT TYPE=\"  text/javascript \">alert(\"Hooray!\");</script>";
			const string targetOutput4A = "<script type=\"  text/javascript \">alert(\"Hooray!\");</script>";
			const string targetOutput4B = "<script>alert(\"Hooray!\");</script>";
			const string targetOutput4C = targetOutput4A;

			const string input5 = "<script type=\"TeXt/JaVaScRiPt\">alert(\"Hooray!\");</script>";
			const string targetOutput5A = input5;
			const string targetOutput5B = "<script>alert(\"Hooray!\");</script>";
			const string targetOutput5C = input5;

			const string input6 = "<script type=\"text/x-javascript\">alert(\"Hooray!\");</script>";

			const string input7 = "<script type=\"text/ecmascript\">alert(\"Hooray!\");</script>";

			const string input8 = "<script type=\"text/x-ecmascript\">alert(\"Hooray!\");</script>";

			const string input9 = "<script type=\"application/javascript\">alert(\"Hooray!\");</script>";

			const string input10 = "<script type=\"application/javascript;version=1.8\">alert(\"Hooray!\");</script>";

			const string input11 = "<script type=\"application/x-javascript\">alert(\"Hooray!\");</script>";

			const string input12 = "<script type=\"application/ecmascript\">alert(\"Hooray!\");</script>";

			const string input13 = "<script type=\"application/x-ecmascript\">alert(\"Hooray!\");</script>";

			const string input14 = "<script type=\"text/jscript\">alert(\"Hooray!\");</script>";

			const string input15 = "<script type=\"text/vbscript\">MsgBox(\"Hooray!\")</script>";

			const string input16 = "<script type=\"module\">console.log(\"Hooray!\");</script>";

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

			string output5A = keepingAllJavaScriptTypeAttributesMinifier.Minify(input5).MinifiedContent;
			string output5B = removingJavaScriptTypeAttributesMinifier.Minify(input5).MinifiedContent;
			string output5C = keepingSomeJavaScriptTypeAttributesMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingAllJavaScriptTypeAttributesMinifier.Minify(input6).MinifiedContent;
			string output6B = removingJavaScriptTypeAttributesMinifier.Minify(input6).MinifiedContent;
			string output6C = keepingSomeJavaScriptTypeAttributesMinifier.Minify(input6).MinifiedContent;

			string output7A = keepingAllJavaScriptTypeAttributesMinifier.Minify(input7).MinifiedContent;
			string output7B = removingJavaScriptTypeAttributesMinifier.Minify(input7).MinifiedContent;
			string output7C = keepingSomeJavaScriptTypeAttributesMinifier.Minify(input7).MinifiedContent;

			string output8A = keepingAllJavaScriptTypeAttributesMinifier.Minify(input8).MinifiedContent;
			string output8B = removingJavaScriptTypeAttributesMinifier.Minify(input8).MinifiedContent;
			string output8C = keepingSomeJavaScriptTypeAttributesMinifier.Minify(input8).MinifiedContent;

			string output9A = keepingAllJavaScriptTypeAttributesMinifier.Minify(input9).MinifiedContent;
			string output9B = removingJavaScriptTypeAttributesMinifier.Minify(input9).MinifiedContent;
			string output9C = keepingSomeJavaScriptTypeAttributesMinifier.Minify(input9).MinifiedContent;

			string output10A = keepingAllJavaScriptTypeAttributesMinifier.Minify(input10).MinifiedContent;
			string output10B = removingJavaScriptTypeAttributesMinifier.Minify(input10).MinifiedContent;
			string output10C = keepingSomeJavaScriptTypeAttributesMinifier.Minify(input10).MinifiedContent;

			string output11A = keepingAllJavaScriptTypeAttributesMinifier.Minify(input11).MinifiedContent;
			string output11B = removingJavaScriptTypeAttributesMinifier.Minify(input11).MinifiedContent;
			string output11C = keepingSomeJavaScriptTypeAttributesMinifier.Minify(input11).MinifiedContent;

			string output12A = keepingAllJavaScriptTypeAttributesMinifier.Minify(input12).MinifiedContent;
			string output12B = removingJavaScriptTypeAttributesMinifier.Minify(input12).MinifiedContent;
			string output12C = keepingSomeJavaScriptTypeAttributesMinifier.Minify(input12).MinifiedContent;

			string output13A = keepingAllJavaScriptTypeAttributesMinifier.Minify(input13).MinifiedContent;
			string output13B = removingJavaScriptTypeAttributesMinifier.Minify(input13).MinifiedContent;
			string output13C = keepingSomeJavaScriptTypeAttributesMinifier.Minify(input13).MinifiedContent;

			string output14A = keepingAllJavaScriptTypeAttributesMinifier.Minify(input14).MinifiedContent;
			string output14B = removingJavaScriptTypeAttributesMinifier.Minify(input14).MinifiedContent;
			string output14C = keepingSomeJavaScriptTypeAttributesMinifier.Minify(input14).MinifiedContent;

			string output15A = keepingAllJavaScriptTypeAttributesMinifier.Minify(input15).MinifiedContent;
			string output15B = removingJavaScriptTypeAttributesMinifier.Minify(input15).MinifiedContent;
			string output15C = keepingSomeJavaScriptTypeAttributesMinifier.Minify(input15).MinifiedContent;

			string output16A = keepingAllJavaScriptTypeAttributesMinifier.Minify(input16).MinifiedContent;
			string output16B = removingJavaScriptTypeAttributesMinifier.Minify(input16).MinifiedContent;
			string output16C = keepingSomeJavaScriptTypeAttributesMinifier.Minify(input16).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1A);
			Assert.Equal(input1, output1B);
			Assert.Equal(input1, output1C);

			Assert.Equal(input2, output2A);
			Assert.Equal(input2, output2B);
			Assert.Equal(input2, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);

			Assert.Equal(input6, output6A);
			Assert.Equal(input6, output6B);
			Assert.Equal(input6, output6C);

			Assert.Equal(input7, output7A);
			Assert.Equal(input7, output7B);
			Assert.Equal(input7, output7C);

			Assert.Equal(input8, output8A);
			Assert.Equal(input8, output8B);
			Assert.Equal(input8, output8C);

			Assert.Equal(input9, output9A);
			Assert.Equal(input9, output9B);
			Assert.Equal(input9, output9C);

			Assert.Equal(input10, output10A);
			Assert.Equal(input10, output10B);
			Assert.Equal(input10, output10C);

			Assert.Equal(input11, output11A);
			Assert.Equal(input11, output11B);
			Assert.Equal(input11, output11C);

			Assert.Equal(input12, output12A);
			Assert.Equal(input12, output12B);
			Assert.Equal(input12, output12C);

			Assert.Equal(input13, output13A);
			Assert.Equal(input13, output13B);
			Assert.Equal(input13, output13C);

			Assert.Equal(input14, output14A);
			Assert.Equal(input14, output14B);
			Assert.Equal(input14, output14C);

			Assert.Equal(input15, output15A);
			Assert.Equal(input15, output15B);
			Assert.Equal(input15, output15C);

			Assert.Equal(input16, output16A);
			Assert.Equal(input16, output16B);
			Assert.Equal(input16, output16C);
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

			const string input1 = "<style type=\"\">.content-wrapper { margin: 0 auto; max-width: 960px; }</style>";

			const string input2 = "<style type>h1 { font-size: 2em; }</style>";

			const string input3 = "<style type=\"text/css\">.error { color: #b94a48; }</style>";
			const string targetOutput3A = input3;
			const string targetOutput3B = "<style>.error { color: #b94a48; }</style>";
			const string targetOutput3C = input3;

			const string input4 = "<STYLE TYPE=\"  text/css \">body { font-size: 14px; }</style>";
			const string targetOutput4A = "<style type=\"  text/css \">body { font-size: 14px; }</style>";
			const string targetOutput4B = "<style>body { font-size: 14px; }</style>";
			const string targetOutput4C = targetOutput4A;

			const string input5 = "<style type=\"text/CSS\">input { width: 90%; }</style>";
			const string targetOutput5A = "<style type=\"text/CSS\">input { width: 90%; }</style>";
			const string targetOutput5B = "<style>input { width: 90%; }</style>";
			const string targetOutput5C = targetOutput5A;

			const string input6 = "<style type=\"text/less\">\n" +
				"	@color: #4D926F;\n\n" +
				"	#header { color: @color; }\n" +
				"</style>"
				;

			const string input7 = "<link rel=\"stylesheet\" type=\"text/css\"" +
				" href=\"http://twitter.github.com/bootstrap/assets/css/bootstrap.css\">";
			const string targetOutput7A = input7;
			const string targetOutput7B = "<link rel=\"stylesheet\"" +
				" href=\"http://twitter.github.com/bootstrap/assets/css/bootstrap.css\">";
			const string targetOutput7C = input7;

			const string input8 = "<link rel=\"stylesheet/less\" type=\"text/css\" href=\"styles.less\">";

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

			string output6A = keepingAllCssTypeAttributesMinifier.Minify(input6).MinifiedContent;
			string output6B = removingCssTypeAttributesMinifier.Minify(input6).MinifiedContent;
			string output6C = keepingSomeCssTypeAttributesMinifier.Minify(input6).MinifiedContent;

			string output7A = keepingAllCssTypeAttributesMinifier.Minify(input7).MinifiedContent;
			string output7B = removingCssTypeAttributesMinifier.Minify(input7).MinifiedContent;
			string output7C = keepingSomeCssTypeAttributesMinifier.Minify(input7).MinifiedContent;

			string output8A = keepingAllCssTypeAttributesMinifier.Minify(input8).MinifiedContent;
			string output8B = removingCssTypeAttributesMinifier.Minify(input8).MinifiedContent;
			string output8C = keepingSomeCssTypeAttributesMinifier.Minify(input8).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1A);
			Assert.Equal(input1, output1B);
			Assert.Equal(input1, output1C);

			Assert.Equal(input2, output2A);
			Assert.Equal(input2, output2B);
			Assert.Equal(input2, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);

			Assert.Equal(input6, output6A);
			Assert.Equal(input6, output6B);
			Assert.Equal(input6, output6C);

			Assert.Equal(targetOutput7A, output7A);
			Assert.Equal(targetOutput7B, output7B);
			Assert.Equal(targetOutput7C, output7C);

			Assert.Equal(input8, output8A);
			Assert.Equal(input8, output8B);
			Assert.Equal(input8, output8C);
		}
	}
}