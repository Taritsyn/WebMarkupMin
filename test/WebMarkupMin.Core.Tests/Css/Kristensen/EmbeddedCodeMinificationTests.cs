using Xunit;

namespace WebMarkupMin.Core.Tests.Css.Kristensen
{
	public class EmbeddedCodeMinificationTests
	{
		[Fact]
		public void RemovingCommentsIsCorrect()
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
		public void WhitespaceMinificationIsCorrect()
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
		public void RemovingLastSemicolonsIsCorrect()
		{
			// Arrange
			var minifier = new KristensenCssMinifier();

			const string input1 = "a{color:blue;}";
			const string targetOutput1 = "a{color:blue}";

			const string input2 = ".note{color:red;background-color:yellow;font-weight:bold;}";
			const string targetOutput2 = ".note{color:red;background-color:yellow;font-weight:bold}";

			// Act
			string output1 = minifier.Minify(input1, false).MinifiedContent;
			string output2 = minifier.Minify(input2, false).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
		}

		[Fact]
		public void RemovingRedundantSelectorsIsCorrect()
		{
			// Arrange
			var minifier = new KristensenCssMinifier();

			const string input1 = "div#idDiv{border:2px solid blue;color:red;margin-top:15px}";
			const string targetOutput1 = "#idDiv{border:2px solid blue;color:red;margin-top:15px}";

			const string input2 = "#content.sectionA{background-color:yellow}";
			const string input3 = "* html #someblock{width:100px;padding:10px}";

			// Act
			string output1 = minifier.Minify(input1, false).MinifiedContent;
			string output2 = minifier.Minify(input2, false).MinifiedContent;
			string output3 = minifier.Minify(input3, false).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(input2, output2);
			Assert.Equal(input3, output3);
		}

		[Fact]
		public void RemovingUnitsFromZeroValuesIsCorrect()
		{
			// Arrange
			var minifier = new KristensenCssMinifier();

			const string input1 = "div{width:0px;margin:10px 0px}";
			const string targetOutput1 = "div{width:0;margin:10px 0}";

			const string input2 = "div{width:0em;margin:10em 0em}";
			const string targetOutput2 = "div{width:0;margin:10em 0}";

			const string input3 = "div{width:0ex;margin:10ex 0ex}";
			const string targetOutput3 = "div{width:0;margin:10ex 0}";

			const string input4 = "div{width:0cm;margin:10cm 0cm}";
			const string targetOutput4 = "div{width:0;margin:10cm 0}";

			const string input5 = "div{width:0mm;margin:10mm 0mm}";
			const string targetOutput5 = "div{width:0;margin:10mm 0}";

			const string input6 = "div{width:0in;margin:10in 0in}";
			const string targetOutput6 = "div{width:0;margin:10in 0}";

			const string input7 = "div{width:0pt;margin:10pt 0pt}";
			const string targetOutput7 = "div{width:0;margin:10pt 0}";

			const string input8 = "div{width:0pc;margin:10pc 0pc}";
			const string targetOutput8 = "div{width:0;margin:10pc 0}";

			const string input9 = "div{width:0%;margin:10% 0%}";
			const string targetOutput9 = "div{width:0;margin:10% 0}";

			const string input10 = "div{width:0ch;margin:10ch 0ch}";
			const string targetOutput10 = "div{width:0;margin:10ch 0}";

			const string input11 = "div{width:0rem;margin:10rem 0rem}";
			const string targetOutput11 = "div{width:0;margin:10rem 0}";

			const string input12 = "div{width:0vh;margin:10vh 0vh}";
			const string targetOutput12 = "div{width:0;margin:10vh 0}";

			const string input13 = "div{width:0vmax;margin:10vmax 0vmax}";
			const string targetOutput13 = "div{width:0;margin:10vmax 0}";

			const string input14 = "div{width:0vmin;margin:10vmin 0vmin}";
			const string targetOutput14 = "div{width:0;margin:10vmin 0}";

			const string input15 = "div{width:0vm;margin:10vm 0vm}";
			const string targetOutput15 = "div{width:0;margin:10vm 0}";

			const string input16 = "div{width:0vw;margin:10vw 0vw}";
			const string targetOutput16 = "div{width:0;margin:10vw 0}";

			// Act
			string output1 = minifier.Minify(input1, false).MinifiedContent;
			string output2 = minifier.Minify(input2, false).MinifiedContent;
			string output3 = minifier.Minify(input3, false).MinifiedContent;
			string output4 = minifier.Minify(input4, false).MinifiedContent;
			string output5 = minifier.Minify(input5, false).MinifiedContent;
			string output6 = minifier.Minify(input6, false).MinifiedContent;
			string output7 = minifier.Minify(input7, false).MinifiedContent;
			string output8 = minifier.Minify(input8, false).MinifiedContent;
			string output9 = minifier.Minify(input9, false).MinifiedContent;
			string output10 = minifier.Minify(input10, false).MinifiedContent;
			string output11 = minifier.Minify(input11, false).MinifiedContent;
			string output12 = minifier.Minify(input12, false).MinifiedContent;
			string output13 = minifier.Minify(input13, false).MinifiedContent;
			string output14 = minifier.Minify(input14, false).MinifiedContent;
			string output15 = minifier.Minify(input15, false).MinifiedContent;
			string output16 = minifier.Minify(input16, false).MinifiedContent;

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
			const string targetOutput = "#idDiv" +
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
			string output = minifier.Minify(input, false).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput, output);
		}
	}
}