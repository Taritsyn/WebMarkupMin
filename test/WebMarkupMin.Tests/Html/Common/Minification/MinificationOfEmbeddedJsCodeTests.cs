using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class MinificationOfEmbeddedJsCodeTests
	{
		[Fact]
		public void MinificationOfEmbeddedJsCode()
		{
			// Arrange
			var keepingEmbeddedJsCodeMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyEmbeddedJsCode = false });
			var minifyingEmbeddedJsCodeMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyEmbeddedJsCode = true });

			const string input1 = "<script type=\"\">\n" +
				"	// Prints 'Hello, World' in the modal window\n" +
				"	alert( 'Hello World!' );\n" +
				"</script>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<script type=\"\">" +
				"alert('Hello World!');" +
				"</script>"
				;

			const string input2 = "<script type>\n" +
				"	// Prints 'Hello, World' in the modal window\n" +
				"	alert( 'Hello World!' );\n" +
				"</script>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = "<script type>" +
				"alert('Hello World!');" +
				"</script>"
				;

			const string input3 = "<script type=\"text/javascript\">\n" +
				"	// Prints 'Hello, World' in the modal window\n" +
				"	alert( 'Hello World!' );\n" +
				"</script>"
				;
			const string targetOutput3A = input3;
			const string targetOutput3B = "<script type=\"text/javascript\">" +
				"alert('Hello World!');" +
				"</script>"
				;

			const string input4 = "<script type=\"  text/javascript \">\n" +
				"	// Prints 'Hello, World' in the modal window\n" +
				"	alert( 'Hello World!' );\n" +
				"</script>"
				;
			const string targetOutput4A = input4;
			const string targetOutput4B = "<script type=\"  text/javascript \">" +
				"alert('Hello World!');" +
				"</script>"
				;

			const string input5 = "<script type=\"TeXt/JaVaScRiPt\">\n" +
				"	// Prints 'Hello, World' in the modal window\n" +
				"	alert( 'Hello World!' );\n" +
				"</script>"
				;
			const string targetOutput5A = input5;
			const string targetOutput5B = "<script type=\"TeXt/JaVaScRiPt\">" +
				"alert('Hello World!');" +
				"</script>"
				;

			const string input6 = "<script type=\"text/x-javascript\">\n" +
				"	// Prints 'Hello, World' in the modal window\n" +
				"	alert( 'Hello World!' );\n" +
				"</script>"
				;
			const string targetOutput6A = input6;
			const string targetOutput6B = "<script type=\"text/x-javascript\">" +
				"alert('Hello World!');" +
				"</script>"
				;

			const string input7 = "<script type=\"text/ecmascript\">\n" +
				"	// Prints 'Hello, World' in the modal window\n" +
				"	alert( 'Hello World!' );\n" +
				"</script>"
				;
			const string targetOutput7A = input7;
			const string targetOutput7B = "<script type=\"text/ecmascript\">" +
				"alert('Hello World!');" +
				"</script>"
				;

			const string input8 = "<script type=\"text/x-ecmascript\">\n" +
				"	// Prints 'Hello, World' in the modal window\n" +
				"	alert( 'Hello World!' );\n" +
				"</script>"
				;
			const string targetOutput8A = input8;
			const string targetOutput8B = "<script type=\"text/x-ecmascript\">" +
				"alert('Hello World!');" +
				"</script>"
				;

			const string input9 = "<script type=\"application/javascript\">\n" +
				"	// Prints 'Hello, World' in the modal window\n" +
				"	alert( 'Hello World!' );\n" +
				"</script>"
				;
			const string targetOutput9A = input9;
			const string targetOutput9B = "<script type=\"application/javascript\">" +
				"alert('Hello World!');" +
				"</script>"
				;

			const string input10 = "<script type=\"application/javascript;version=1.8\">\n" +
				"	document.addEventListener(\"click\", function() false, true);\n" +
				"</script>"
				;
			const string targetOutput10A = input10;
			const string targetOutput10B = "<script type=\"application/javascript;version=1.8\">" +
				"document.addEventListener(\"click\",function()false,true);" +
				"</script>"
				;

			const string input11 = "<script type=\"application/x-javascript\">\n" +
				"	// Prints 'Hello, World' in the modal window\n" +
				"	alert( 'Hello World!' );\n" +
				"</script>"
				;
			const string targetOutput11A = input11;
			const string targetOutput11B = "<script type=\"application/x-javascript\">" +
				"alert('Hello World!');" +
				"</script>"
				;

			const string input12 = "<script type=\"application/ecmascript\">\n" +
				"	// Prints 'Hello, World' in the modal window\n" +
				"	alert( 'Hello World!' );\n" +
				"</script>"
				;
			const string targetOutput12A = input12;
			const string targetOutput12B = "<script type=\"application/ecmascript\">" +
				"alert('Hello World!');" +
				"</script>"
				;

			const string input13 = "<script type=\"application/x-ecmascript\">\n" +
				"	// Prints 'Hello, World' in the modal window\n" +
				"	alert( 'Hello World!' );\n" +
				"</script>"
				;
			const string targetOutput13A = input13;
			const string targetOutput13B = "<script type=\"application/x-ecmascript\">" +
				"alert('Hello World!');" +
				"</script>"
				;

			const string input14 = "<script type=\"text/jscript\">\n" +
				"/*@cc_on\n" +
				"	/*@if (@_win32)\n" +
				"		document.write(\"OS is 32-bit, browser is IE.\");\n" +
				"	 @else @*/\n" +
				"		document.write(\"Browser is not IE or Browser is not 32 bit IE.\");\n" +
				"	/*@end\n" +
				"@*/\n" +
				"</script>"
				;

			const string input15 = "<script type=\"text/vbscript\">\n" +
				"	'Prints 'Hello, World' in the modal window\n" +
				"	MsgBox(\"Hello World!\")\n" +
				"</script>"
				;

			const string input16 = "<script type=\"module\">\n" +
				"	// Prints 'Hello, World' in the Browser Console\n" +
				"	console.log( 'Hello World!' );\n" +
				"</script>"
				;
			const string targetOutput16A = input16;
			const string targetOutput16B = "<script type=\"module\">" +
				"console.log('Hello World!');" +
				"</script>"
				;

			// Act
			string output1A = keepingEmbeddedJsCodeMinifier.Minify(input1).MinifiedContent;
			string output1B = minifyingEmbeddedJsCodeMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingEmbeddedJsCodeMinifier.Minify(input2).MinifiedContent;
			string output2B = minifyingEmbeddedJsCodeMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingEmbeddedJsCodeMinifier.Minify(input3).MinifiedContent;
			string output3B = minifyingEmbeddedJsCodeMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingEmbeddedJsCodeMinifier.Minify(input4).MinifiedContent;
			string output4B = minifyingEmbeddedJsCodeMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingEmbeddedJsCodeMinifier.Minify(input5).MinifiedContent;
			string output5B = minifyingEmbeddedJsCodeMinifier.Minify(input5).MinifiedContent;

			string output6A = keepingEmbeddedJsCodeMinifier.Minify(input6).MinifiedContent;
			string output6B = minifyingEmbeddedJsCodeMinifier.Minify(input6).MinifiedContent;

			string output7A = keepingEmbeddedJsCodeMinifier.Minify(input7).MinifiedContent;
			string output7B = minifyingEmbeddedJsCodeMinifier.Minify(input7).MinifiedContent;

			string output8A = keepingEmbeddedJsCodeMinifier.Minify(input8).MinifiedContent;
			string output8B = minifyingEmbeddedJsCodeMinifier.Minify(input8).MinifiedContent;

			string output9A = keepingEmbeddedJsCodeMinifier.Minify(input9).MinifiedContent;
			string output9B = minifyingEmbeddedJsCodeMinifier.Minify(input9).MinifiedContent;

			string output10A = keepingEmbeddedJsCodeMinifier.Minify(input10).MinifiedContent;
			string output10B = minifyingEmbeddedJsCodeMinifier.Minify(input10).MinifiedContent;

			string output11A = keepingEmbeddedJsCodeMinifier.Minify(input11).MinifiedContent;
			string output11B = minifyingEmbeddedJsCodeMinifier.Minify(input11).MinifiedContent;

			string output12A = keepingEmbeddedJsCodeMinifier.Minify(input12).MinifiedContent;
			string output12B = minifyingEmbeddedJsCodeMinifier.Minify(input12).MinifiedContent;

			string output13A = keepingEmbeddedJsCodeMinifier.Minify(input13).MinifiedContent;
			string output13B = minifyingEmbeddedJsCodeMinifier.Minify(input13).MinifiedContent;

			string output14A = keepingEmbeddedJsCodeMinifier.Minify(input14).MinifiedContent;
			string output14B = minifyingEmbeddedJsCodeMinifier.Minify(input14).MinifiedContent;

			string output15A = keepingEmbeddedJsCodeMinifier.Minify(input15).MinifiedContent;
			string output15B = minifyingEmbeddedJsCodeMinifier.Minify(input15).MinifiedContent;

			string output16A = keepingEmbeddedJsCodeMinifier.Minify(input16).MinifiedContent;
			string output16B = minifyingEmbeddedJsCodeMinifier.Minify(input16).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);

			Assert.Equal(targetOutput6A, output6A);
			Assert.Equal(targetOutput6B, output6B);

			Assert.Equal(targetOutput7A, output7A);
			Assert.Equal(targetOutput7B, output7B);

			Assert.Equal(targetOutput8A, output8A);
			Assert.Equal(targetOutput8B, output8B);

			Assert.Equal(targetOutput9A, output9A);
			Assert.Equal(targetOutput9B, output9B);

			Assert.Equal(targetOutput10A, output10A);
			Assert.Equal(targetOutput10B, output10B);

			Assert.Equal(targetOutput11A, output11A);
			Assert.Equal(targetOutput11B, output11B);

			Assert.Equal(targetOutput12A, output12A);
			Assert.Equal(targetOutput12B, output12B);

			Assert.Equal(targetOutput13A, output13A);
			Assert.Equal(targetOutput13B, output13B);

			Assert.Equal(input14, output14A);
			Assert.Equal(input14, output14B);

			Assert.Equal(input15, output15A);
			Assert.Equal(input15, output15B);

			Assert.Equal(targetOutput16A, output16A);
			Assert.Equal(targetOutput16B, output16B);
		}
	}
}