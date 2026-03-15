using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Css.Kristensen
{
	public class InlineCodeMinificationTests
	{
		[Fact]
		public void RemovingComments()
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
		public void WhitespaceMinification()
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
		public void RemovingLastSemicolons()
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
		public void RemovingUnitsFromZeroValues()
		{
			// Arrange
			var keepingZeroUnitsMinifier = new KristensenCssMinifier(
				new KristensenCssMinificationSettings
				{
					RemoveRedundantSelectors = false,
					RemoveUnitsFromZeroValues = false
				});
			var removingZeroUnitsMinifier = new KristensenCssMinifier(
				new KristensenCssMinificationSettings
				{
					RemoveRedundantSelectors = true,
					RemoveUnitsFromZeroValues = true
				});

			const string input1 = "width:0px;margin:10px 0px";
			const string targetOutput1A = input1;
			const string targetOutput1B = "width:0;margin:10px 0";

			const string input2 = "width:0em;margin:10em 0em";
			const string targetOutput2A = input2;
			const string targetOutput2B = "width:0;margin:10em 0";

			const string input3 = "width:0ex;margin:10ex 0ex";
			const string targetOutput3A = input3;
			const string targetOutput3B = "width:0;margin:10ex 0";

			const string input4 = "width:0cm;margin:10cm 0cm";
			const string targetOutput4A = input4;
			const string targetOutput4B = "width:0;margin:10cm 0";

			const string input5 = "width:0mm;margin:10mm 0mm";
			const string targetOutput5A = input5;
			const string targetOutput5B = "width:0;margin:10mm 0";

			const string input6 = "width:0in;margin:10in 0in";
			const string targetOutput6A = input6;
			const string targetOutput6B = "width:0;margin:10in 0";

			const string input7 = "width:0pt;margin:10pt 0pt";
			const string targetOutput7A = input7;
			const string targetOutput7B = "width:0;margin:10pt 0";

			const string input8 = "width:0pc;margin:10pc 0pc";
			const string targetOutput8A = input8;
			const string targetOutput8B = "width:0;margin:10pc 0";

			const string input9 = "width:0%;margin:10% 0%";
			const string targetOutput9A = input9;
			const string targetOutput9B = "width:0;margin:10% 0";

			const string input10 = "width:0ch;margin:10ch 0ch";
			const string targetOutput10A = input10;
			const string targetOutput10B = "width:0;margin:10ch 0";

			const string input11 = "width:0rem;margin:10rem 0rem";
			const string targetOutput11A = input11;
			const string targetOutput11B = "width:0;margin:10rem 0";

			const string input12 = "width:0vh;margin:10vh 0vh";
			const string targetOutput12A = input12;
			const string targetOutput12B = "width:0;margin:10vh 0";

			const string input13 = "width:0vmax;margin:10vmax 0vmax";
			const string targetOutput13A = input13;
			const string targetOutput13B = "width:0;margin:10vmax 0";

			const string input14 = "width:0vmin;margin:10vmin 0vmin";
			const string targetOutput14A = input14;
			const string targetOutput14B = "width:0;margin:10vmin 0";

			const string input15 = "width:0vm;margin:10vm 0vm";
			const string targetOutput15A = input15;
			const string targetOutput15B = "width:0;margin:10vm 0";

			const string input16 = "width:0vw;margin:10vw 0vw";
			const string targetOutput16A = input16;
			const string targetOutput16B = "width:0;margin:10vw 0";

			// Act
			string output1A = keepingZeroUnitsMinifier.Minify(input1, true).MinifiedContent;
			string output1B = removingZeroUnitsMinifier.Minify(input1, true).MinifiedContent;

			string output2A = keepingZeroUnitsMinifier.Minify(input2, true).MinifiedContent;
			string output2B = removingZeroUnitsMinifier.Minify(input2, true).MinifiedContent;

			string output3A = keepingZeroUnitsMinifier.Minify(input3, true).MinifiedContent;
			string output3B = removingZeroUnitsMinifier.Minify(input3, true).MinifiedContent;

			string output4A = keepingZeroUnitsMinifier.Minify(input4, true).MinifiedContent;
			string output4B = removingZeroUnitsMinifier.Minify(input4, true).MinifiedContent;

			string output5A = keepingZeroUnitsMinifier.Minify(input5, true).MinifiedContent;
			string output5B = removingZeroUnitsMinifier.Minify(input5, true).MinifiedContent;

			string output6A = keepingZeroUnitsMinifier.Minify(input6, true).MinifiedContent;
			string output6B = removingZeroUnitsMinifier.Minify(input6, true).MinifiedContent;

			string output7A = keepingZeroUnitsMinifier.Minify(input7, true).MinifiedContent;
			string output7B = removingZeroUnitsMinifier.Minify(input7, true).MinifiedContent;

			string output8A = keepingZeroUnitsMinifier.Minify(input8, true).MinifiedContent;
			string output8B = removingZeroUnitsMinifier.Minify(input8, true).MinifiedContent;

			string output9A = keepingZeroUnitsMinifier.Minify(input9, true).MinifiedContent;
			string output9B = removingZeroUnitsMinifier.Minify(input9, true).MinifiedContent;

			string output10A = keepingZeroUnitsMinifier.Minify(input10, true).MinifiedContent;
			string output10B = removingZeroUnitsMinifier.Minify(input10, true).MinifiedContent;

			string output11A = keepingZeroUnitsMinifier.Minify(input11, true).MinifiedContent;
			string output11B = removingZeroUnitsMinifier.Minify(input11, true).MinifiedContent;

			string output12A = keepingZeroUnitsMinifier.Minify(input12, true).MinifiedContent;
			string output12B = removingZeroUnitsMinifier.Minify(input12, true).MinifiedContent;

			string output13A = keepingZeroUnitsMinifier.Minify(input13, true).MinifiedContent;
			string output13B = removingZeroUnitsMinifier.Minify(input13, true).MinifiedContent;

			string output14A = keepingZeroUnitsMinifier.Minify(input14, true).MinifiedContent;
			string output14B = removingZeroUnitsMinifier.Minify(input14, true).MinifiedContent;

			string output15A = keepingZeroUnitsMinifier.Minify(input15, true).MinifiedContent;
			string output15B = removingZeroUnitsMinifier.Minify(input15, true).MinifiedContent;

			string output16A = keepingZeroUnitsMinifier.Minify(input16, true).MinifiedContent;
			string output16B = removingZeroUnitsMinifier.Minify(input16, true).MinifiedContent;

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

			Assert.Equal(targetOutput14A, output14A);
			Assert.Equal(targetOutput14B, output14B);

			Assert.Equal(targetOutput15A, output15A);
			Assert.Equal(targetOutput15B, output15B);

			Assert.Equal(targetOutput16A, output16A);
			Assert.Equal(targetOutput16B, output16B);
		}

		[Fact]
		public void ApplyingOfAllOptimizations()
		{
			// Arrange
			var minifierWithoutAdditionalOptimizations = new KristensenCssMinifier(
				new KristensenCssMinificationSettings { RemoveUnitsFromZeroValues = false });
			var minifierWithAdditionalOptimizations = new KristensenCssMinifier(
				new KristensenCssMinificationSettings { RemoveUnitsFromZeroValues = true });

			const string input = "  	width:	640px; " +
				"/*max-width: 1020px; */" +
				"margin: 10px  0px; " +
				"border: 4px  double   black; " +
				"font-size:  120%; " +
				"font-family: Verdana,	 Arial,   Helvetica,  sans-serif ; " +
				"color : #336	 ; "
				;
			const string targetOutputA = "width:640px;" +
				"margin:10px 0px;" +
				"border:4px double black;" +
				"font-size:120%;" +
				"font-family:Verdana,Arial,Helvetica,sans-serif;" +
				"color:#336"
				;
			const string targetOutputB = "width:640px;" +
				"margin:10px 0;" +
				"border:4px double black;" +
				"font-size:120%;" +
				"font-family:Verdana,Arial,Helvetica,sans-serif;" +
				"color:#336"
				;

			// Act
			string outputA = minifierWithoutAdditionalOptimizations.Minify(input, true).MinifiedContent;
			string outputB = minifierWithAdditionalOptimizations.Minify(input, true).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
		}
	}
}