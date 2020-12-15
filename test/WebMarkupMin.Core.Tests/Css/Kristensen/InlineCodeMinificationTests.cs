using Xunit;

namespace WebMarkupMin.Core.Tests.Css.Kristensen
{
	public class InlineCodeMinificationTests
	{
		[Fact]
		public void RemovingCommentsIsCorrect()
		{
			// Arrange
			var minifier = new KristensenCssMinifier();

			const string input1 = "font-size:62.5%/* 1em = 10px */";
			const string targetOutput1 = "font-size:62.5%";

			const string input2 = "font-size:120%;/* Размер шрифта */" +
				"font-family:Verdana,Arial,Helvetica,sans-serif;/* Семейство шрифта */" +
				"color:#336/* Цвет текста */"
				;
			const string targetOutput2 = "font-size:120%;" +
				"font-family:Verdana,Arial,Helvetica,sans-serif;" +
				"color:#336"
				;

			const string input3 = "width:300px/*;margin-left:20px*/;margin-right:20px";
			const string targetOutput3 = "width:300px;margin-right:20px";

			// Act
			string output1 = minifier.Minify(input1, true).MinifiedContent;
			string output2 = minifier.Minify(input2, true).MinifiedContent;
			string output3 = minifier.Minify(input3, true).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
		}

		[Fact]
		public void WhitespaceMinificationIsCorrect()
		{
			// Arrange
			var minifier = new KristensenCssMinifier();

			const string input1 = "background-image:	url('/images/products/392.jpg')";
			const string targetOutput1 = "background-image:url('/images/products/392.jpg')";

			const string input2 = "  	font-size:	120%;  " +
				"font-family: Verdana,	Arial,   Helvetica,  sans-serif ;" +
				"color : #336	  "
				;
			const string targetOutput2 = "font-size:120%;" +
				"font-family:Verdana,Arial,Helvetica,sans-serif;" +
				"color:#336";

			// Act
			string output1 = minifier.Minify(input1, true).MinifiedContent;
			string output2 = minifier.Minify(input2, true).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
		}

		[Fact]
		public void RemovingLastSemicolonsIsCorrect()
		{
			// Arrange
			var minifier = new KristensenCssMinifier();

			const string input1 = "color:blue;";
			const string targetOutput1 = "color:blue";

			const string input2 = "color:red;background-color:yellow;font-weight:bold;";
			const string targetOutput2 = "color:red;background-color:yellow;font-weight:bold";

			// Act
			string output1 = minifier.Minify(input1, true).MinifiedContent;
			string output2 = minifier.Minify(input2, true).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
		}

		[Fact]
		public void RemovingUnitsFromZeroValuesIsCorrect()
		{
			// Arrange
			var minifier = new KristensenCssMinifier();

			const string input1 = "width:0px;margin:10px 0px";
			const string targetOutput1 = "width:0;margin:10px 0";

			const string input2 = "width:0em;margin:10em 0em";
			const string targetOutput2 = "width:0;margin:10em 0";

			const string input3 = "width:0ex;margin:10ex 0ex";
			const string targetOutput3 = "width:0;margin:10ex 0";

			const string input4 = "width:0cm;margin:10cm 0cm";
			const string targetOutput4 = "width:0;margin:10cm 0";

			const string input5 = "width:0mm;margin:10mm 0mm";
			const string targetOutput5 = "width:0;margin:10mm 0";

			const string input6 = "width:0in;margin:10in 0in";
			const string targetOutput6 = "width:0;margin:10in 0";

			const string input7 = "width:0pt;margin:10pt 0pt";
			const string targetOutput7 = "width:0;margin:10pt 0";

			const string input8 = "width:0pc;margin:10pc 0pc";
			const string targetOutput8 = "width:0;margin:10pc 0";

			const string input9 = "width:0%;margin:10% 0%";
			const string targetOutput9 = "width:0;margin:10% 0";

			const string input10 = "width:0ch;margin:10ch 0ch";
			const string targetOutput10 = "width:0;margin:10ch 0";

			const string input11 = "width:0rem;margin:10rem 0rem";
			const string targetOutput11 = "width:0;margin:10rem 0";

			const string input12 = "width:0vh;margin:10vh 0vh";
			const string targetOutput12 = "width:0;margin:10vh 0";

			const string input13 = "width:0vmax;margin:10vmax 0vmax";
			const string targetOutput13 = "width:0;margin:10vmax 0";

			const string input14 = "width:0vmin;margin:10vmin 0vmin";
			const string targetOutput14 = "width:0;margin:10vmin 0";

			const string input15 = "width:0vm;margin:10vm 0vm";
			const string targetOutput15 = "width:0;margin:10vm 0";

			const string input16 = "width:0vw;margin:10vw 0vw";
			const string targetOutput16 = "width:0;margin:10vw 0";

			// Act
			string output1 = minifier.Minify(input1, true).MinifiedContent;
			string output2 = minifier.Minify(input2, true).MinifiedContent;
			string output3 = minifier.Minify(input3, true).MinifiedContent;
			string output4 = minifier.Minify(input4, true).MinifiedContent;
			string output5 = minifier.Minify(input5, true).MinifiedContent;
			string output6 = minifier.Minify(input6, true).MinifiedContent;
			string output7 = minifier.Minify(input7, true).MinifiedContent;
			string output8 = minifier.Minify(input8, true).MinifiedContent;
			string output9 = minifier.Minify(input9, true).MinifiedContent;
			string output10 = minifier.Minify(input10, true).MinifiedContent;
			string output11 = minifier.Minify(input11, true).MinifiedContent;
			string output12 = minifier.Minify(input12, true).MinifiedContent;
			string output13 = minifier.Minify(input13, true).MinifiedContent;
			string output14 = minifier.Minify(input14, true).MinifiedContent;
			string output15 = minifier.Minify(input15, true).MinifiedContent;
			string output16 = minifier.Minify(input16, true).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
			Assert.Equal(targetOutput6, output6);
			Assert.Equal(targetOutput7, output7);
			Assert.Equal(targetOutput8, output8);
			Assert.Equal(targetOutput9, output9);
			Assert.Equal(targetOutput10, output10);
			Assert.Equal(targetOutput11, output11);
			Assert.Equal(targetOutput12, output12);
			Assert.Equal(targetOutput13, output13);
			Assert.Equal(targetOutput14, output14);
			Assert.Equal(targetOutput15, output15);
			Assert.Equal(targetOutput16, output16);
		}

		[Fact]
		public void ApplyingOfAllOptimizationsIsCorrect()
		{
			// Arrange
			var minifier = new KristensenCssMinifier();

			const string input = "  	width:	640px; " +
				"/*max-width: 1020px; */" +
				"margin: 10px  0px; " +
				"border: 4px  double   black; " +
				"font-size:  120%; " +
				"font-family: Verdana,	 Arial,   Helvetica,  sans-serif ; " +
				"color : #336	 ; "
				;
			const string targetOutput = "width:640px;" +
				"margin:10px 0;" +
				"border:4px double black;" +
				"font-size:120%;" +
				"font-family:Verdana,Arial,Helvetica,sans-serif;" +
				"color:#336"
				;

			// Act
			string output = minifier.Minify(input, true).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput, output);
		}
	}
}