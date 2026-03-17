using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Css.Kristensen
{
	public class EmbeddedCodeMinificationTests
	{
		[Fact]
		public void RemovingComments()
		{
			// Arrange
			var minifier = new KristensenCssMinifier();

			const string input1 = "body{font-size:62.5% /* 1em = 10px */}";
			const string targetOutput1 = "body{font-size:62.5%}";

			const string input2 = "h1 {\n" +
				"  font-size: 120%; /* Размер шрифта */\n" +
				"  font-family: Verdana, Arial, Helvetica, sans-serif; /* Семейство шрифта */\n" +
				"  color: #336 /* Цвет текста */\n" +
				"}"
				;
			const string targetOutput2 = "h1{" +
				"font-size:120%;" +
				"font-family:Verdana,Arial,Helvetica,sans-serif;" +
				"color:#336" +
				"}"
				;

			const string input3 = "* {\n" +
				"  margin: 0;\n" +
				"  padding: 0\n" +
				"}\n" +
				"/*\n\n" +
				"span {\n" +
				"  color: blue;\n" +
				"  font-size: 1.5em\n" +
				"}\n" +
				"*/\n\n" +
				"p {\n" +
				"   font-family: arial, helvetica, sans-serif\n" +
				"}"
				;
			const string targetOutput3 = "*{" +
				"margin:0;" +
				"padding:0" +
				"}" +
				"p{" +
				"font-family:arial,helvetica,sans-serif" +
				"}"
				;

			// Act
			string output1 = minifier.Minify(input1, false).MinifiedContent;
			string output2 = minifier.Minify(input2, false).MinifiedContent;
			string output3 = minifier.Minify(input3, false).MinifiedContent;

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

			const string input1 = "body { background-color: powderblue }\r\n" +
				"h1   { color: blue }\r\n" +
				"p    { color: red }"
				;
			const string targetOutput1 = "body{background-color:powderblue}" +
				"h1{color:blue}" +
				"p{color:red}"
				;

			const string input2 = "h1 {\n" +
				"	font-size:  120%;\n" +
				"	font-family: Verdana,	 Arial,   Helvetica,  sans-serif ;\n" +
				"	color : #336 \n" +
				"}\n"
				;
			const string targetOutput2 = "h1{" +
				"font-size:120%;" +
				"font-family:Verdana,Arial,Helvetica,sans-serif;" +
				"color:#336" +
				"}"
				;

			const string input3 = "  	p.note > b {\r " +
				"   color: red\r" +
				"}	  "
				;
			const string targetOutput3 = "p.note > b{" +
				"color:red" +
				"}"
				;

			const string input4 = "h1,\n" +
				"h2 {\n" +
				"	color: red;\n" +
				"	font-family: \"Times New Roman\", Georgia, Serif;\n" +
				"	line-height: 1.3em\n" +
				"}\n"
				;
			const string targetOutput4 = "h1," +
				"h2{" +
				"color:red;" +
				"font-family:\"Times New Roman\",Georgia,Serif;" +
				"line-height:1.3em" +
				"}"
				;

			// Act
			string output1 = minifier.Minify(input1, false).MinifiedContent;
			string output2 = minifier.Minify(input2, false).MinifiedContent;
			string output3 = minifier.Minify(input3, false).MinifiedContent;
			string output4 = minifier.Minify(input4, false).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
		}

		[Fact]
		public void RemovingTrailingSemicolons()
		{
			// Arrange
			var keepingSemicolonsMinifier = new KristensenCssMinifier(
				new KristensenCssMinificationSettings { RemoveTrailingSemicolons = false });
			var removingSemicolonsMinifier = new KristensenCssMinifier(
				new KristensenCssMinificationSettings { RemoveTrailingSemicolons = true });

			const string input1 = "a{color:blue;}";
			const string targetOutput1A = input1;
			const string targetOutput1B = "a{color:blue}";

			const string input2 = ".note{color:red;background-color:yellow;font-weight:bold;}";
			const string targetOutput2A = input2;
			const string targetOutput2B = ".note{color:red;background-color:yellow;font-weight:bold}";

			// Act
			string output1A = keepingSemicolonsMinifier.Minify(input1, false).MinifiedContent;
			string output1B = removingSemicolonsMinifier.Minify(input1, false).MinifiedContent;

			string output2A = keepingSemicolonsMinifier.Minify(input2, false).MinifiedContent;
			string output2B = removingSemicolonsMinifier.Minify(input2, false).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
		}

		[Fact]
		public void RemovingRedundantSelectors()
		{
			// Arrange
			var keepingRedundantSelectorsMinifier = new KristensenCssMinifier(
				new KristensenCssMinificationSettings { RemoveRedundantSelectors = false });
			var removingRedundantSelectorsMinifier = new KristensenCssMinifier(
				new KristensenCssMinificationSettings { RemoveRedundantSelectors = true });

			const string input1 = "div#idDiv{border:2px solid blue;color:red;margin-top:15px}";
			const string targetOutput1A = input1;
			const string targetOutput1B = "#idDiv{border:2px solid blue;color:red;margin-top:15px}";

			const string input2 = "#content.sectionA{background-color:yellow}";

			const string input3 = "* html #someblock{width:100px;padding:10px}";

			// Act
			string output1A = keepingRedundantSelectorsMinifier.Minify(input1, false).MinifiedContent;
			string output1B = removingRedundantSelectorsMinifier.Minify(input1, false).MinifiedContent;

			string output2A = keepingRedundantSelectorsMinifier.Minify(input2, false).MinifiedContent;
			string output2B = removingRedundantSelectorsMinifier.Minify(input2, false).MinifiedContent;

			string output3A = keepingRedundantSelectorsMinifier.Minify(input3, false).MinifiedContent;
			string output3B = removingRedundantSelectorsMinifier.Minify(input3, false).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(input2, output2A);
			Assert.Equal(input2, output2B);

			Assert.Equal(input3, output3A);
			Assert.Equal(input3, output3B);
		}

		[Fact]
		public void RemovingUnitsFromZeroValues()
		{
			// Arrange
			var keepingZeroUnitsMinifier = new KristensenCssMinifier(
				new KristensenCssMinificationSettings { RemoveUnitsFromZeroValues = false });
			var removingZeroUnitsMinifier = new KristensenCssMinifier(
				new KristensenCssMinificationSettings { RemoveUnitsFromZeroValues = true });

			const string input1 = "div{width:0px;margin:10px 0px}";
			const string targetOutput1A = input1;
			const string targetOutput1B = "div{width:0;margin:10px 0}";

			const string input2 = "div{width:0pt;margin:10pt 0pt}";
			const string targetOutput2A = input2;
			const string targetOutput2B = "div{width:0;margin:10pt 0}";

			const string input3 = "div{width:0pc;margin:10pc 0pc}";
			const string targetOutput3A = input3;
			const string targetOutput3B = "div{width:0;margin:10pc 0}";

			const string input4 = "div{width:0cm;margin:10cm 0cm}";
			const string targetOutput4A = input4;
			const string targetOutput4B = "div{width:0;margin:10cm 0}";

			const string input5 = "div{width:0mm;margin:10mm 0mm}";
			const string targetOutput5A = input5;
			const string targetOutput5B = "div{width:0;margin:10mm 0}";

			const string input6 = "div{width:0in;margin:10in 0in}";
			const string targetOutput6A = input6;
			const string targetOutput6B = "div{width:0;margin:10in 0}";

			const string input7 = "div{width:0%;margin:10% 0%}";

			const string input8 = "div{width:0em;margin:10em 0em}";
			const string targetOutput8A = input8;
			const string targetOutput8B = "div{width:0;margin:10em 0}";

			const string input9 = "div{width:0ex;margin:10ex 0ex}";
			const string targetOutput9A = input9;
			const string targetOutput9B = "div{width:0;margin:10ex 0}";

			const string input10 = "div{width:0ch;margin:10ch 0ch}";
			const string targetOutput10A = input10;
			const string targetOutput10B = "div{width:0;margin:10ch 0}";

			const string input11 = "div{width:0rem;margin:10rem 0rem}";
			const string targetOutput11A = input11;
			const string targetOutput11B = "div{width:0;margin:10rem 0}";

			const string input12 = "div{width:0vw;margin:10vw 0vw}";
			const string targetOutput12A = input12;
			const string targetOutput12B = "div{width:0;margin:10vw 0}";

			const string input13 = "div{width:0vh;margin:10vh 0vh}";
			const string targetOutput13A = input13;
			const string targetOutput13B = "div{width:0;margin:10vh 0}";

			const string input14 = "div{width:0vmin;margin:10vmin 0vmin}";
			const string targetOutput14A = input14;
			const string targetOutput14B = "div{width:0;margin:10vmin 0}";

			const string input15 = "div{width:0vmax;margin:10vmax 0vmax}";
			const string targetOutput15A = input15;
			const string targetOutput15B = "div{width:0;margin:10vmax 0}";

			const string input16 = "div{width:0vm;margin:10vm 0vm}";

			// Act
			string output1A = keepingZeroUnitsMinifier.Minify(input1, true).MinifiedContent;
			string output1B = removingZeroUnitsMinifier.Minify(input1, false).MinifiedContent;

			string output2A = keepingZeroUnitsMinifier.Minify(input2, true).MinifiedContent;
			string output2B = removingZeroUnitsMinifier.Minify(input2, false).MinifiedContent;

			string output3A = keepingZeroUnitsMinifier.Minify(input3, true).MinifiedContent;
			string output3B = removingZeroUnitsMinifier.Minify(input3, false).MinifiedContent;

			string output4A = keepingZeroUnitsMinifier.Minify(input4, true).MinifiedContent;
			string output4B = removingZeroUnitsMinifier.Minify(input4, false).MinifiedContent;

			string output5A = keepingZeroUnitsMinifier.Minify(input5, true).MinifiedContent;
			string output5B = removingZeroUnitsMinifier.Minify(input5, false).MinifiedContent;

			string output6A = keepingZeroUnitsMinifier.Minify(input6, true).MinifiedContent;
			string output6B = removingZeroUnitsMinifier.Minify(input6, false).MinifiedContent;

			string output7A = keepingZeroUnitsMinifier.Minify(input7, true).MinifiedContent;
			string output7B = removingZeroUnitsMinifier.Minify(input7, false).MinifiedContent;

			string output8A = keepingZeroUnitsMinifier.Minify(input8, true).MinifiedContent;
			string output8B = removingZeroUnitsMinifier.Minify(input8, false).MinifiedContent;

			string output9A = keepingZeroUnitsMinifier.Minify(input9, true).MinifiedContent;
			string output9B = removingZeroUnitsMinifier.Minify(input9, false).MinifiedContent;

			string output10A = keepingZeroUnitsMinifier.Minify(input10, true).MinifiedContent;
			string output10B = removingZeroUnitsMinifier.Minify(input10, false).MinifiedContent;

			string output11A = keepingZeroUnitsMinifier.Minify(input11, true).MinifiedContent;
			string output11B = removingZeroUnitsMinifier.Minify(input11, false).MinifiedContent;

			string output12A = keepingZeroUnitsMinifier.Minify(input12, true).MinifiedContent;
			string output12B = removingZeroUnitsMinifier.Minify(input12, false).MinifiedContent;

			string output13A = keepingZeroUnitsMinifier.Minify(input13, true).MinifiedContent;
			string output13B = removingZeroUnitsMinifier.Minify(input13, false).MinifiedContent;

			string output14A = keepingZeroUnitsMinifier.Minify(input14, true).MinifiedContent;
			string output14B = removingZeroUnitsMinifier.Minify(input14, false).MinifiedContent;

			string output15A = keepingZeroUnitsMinifier.Minify(input15, true).MinifiedContent;
			string output15B = removingZeroUnitsMinifier.Minify(input15, false).MinifiedContent;

			string output16A = keepingZeroUnitsMinifier.Minify(input16, true).MinifiedContent;
			string output16B = removingZeroUnitsMinifier.Minify(input16, false).MinifiedContent;

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

			Assert.Equal(input7, output7A);
			Assert.Equal(input7, output7B);

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

			Assert.Equal(input16, output16A);
			Assert.Equal(input16, output16B);
		}

		[Fact]
		public void ApplyingOfAllOptimizations()
		{
			// Arrange
			var minifierWithoutAdditionalOptimizations = new KristensenCssMinifier(
				new KristensenCssMinificationSettings
				{
					RemoveRedundantSelectors = false,
					RemoveTrailingSemicolons = false,
					RemoveUnitsFromZeroValues = false
				});
			var minifierWithAdditionalOptimizations = new KristensenCssMinifier(
				new KristensenCssMinificationSettings
				{
					RemoveRedundantSelectors = true,
					RemoveTrailingSemicolons = true,
					RemoveUnitsFromZeroValues = true
				});

			const string input = "  	div#idDiv\n" +
				"{\n" +
				"	/*max-width: 1020px;*/\n" +
				"	margin: 10px  0px;\n" +
				"	border: 4px  double   black;\n" +
				"	font-size:  120%;\n" +
				"	font-family: Verdana,	 Arial,   Helvetica,  sans-serif ;\n" +
				"	color : #336;\n" +
				"}\n" +
				"/*\n\n" +
				"span\n" +
				"{\n" +
				"	color: blue;\n" +
				"	font-size: 1.5em;\n" +
				"}\n" +
				"*/\n\n" +
				"a:active\n" +
				"{\n" +
				"	color: blue;\n" +
				"}\n\n" +
				"p::first-letter\n" +
				"{\n" +
				"	font-size: 32px;\n" +
				"}	  "
				;
			const string targetOutputA = "div#idDiv" +
				"{" +
				"margin:10px 0px;" +
				"border:4px double black;" +
				"font-size:120%;" +
				"font-family:Verdana,Arial,Helvetica,sans-serif;" +
				"color:#336;" +
				"}" +
				"a:active" +
				"{" +
				"color:blue;" +
				"}" +
				"p::first-letter" +
				"{" +
				"font-size:32px;" +
				"}"
				;
			const string targetOutputB = "#idDiv" +
				"{" +
				"margin:10px 0;" +
				"border:4px double black;" +
				"font-size:120%;" +
				"font-family:Verdana,Arial,Helvetica,sans-serif;" +
				"color:#336" +
				"}" +
				"a:active" +
				"{" +
				"color:blue" +
				"}" +
				"p::first-letter" +
				"{" +
				"font-size:32px" +
				"}"
				;

			// Act
			string outputA = minifierWithoutAdditionalOptimizations.Minify(input, false).MinifiedContent;
			string outputB = minifierWithAdditionalOptimizations.Minify(input, false).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
		}
	}
}