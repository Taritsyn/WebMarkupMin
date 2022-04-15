using System;

using Xunit;

namespace WebMarkupMin.Core.Tests.Html.Minification
{
	public class WhitespaceMinificationTests : IDisposable
	{
		private HtmlMinifier _keepingWhitespaceMinifier;
		private HtmlMinifier _keepingWhitespaceAndNewLinesMinifier;
		private HtmlMinifier _safeRemovingWhitespaceMinifier;
		private HtmlMinifier _safeRemovingWhitespaceExceptForNewLinesMinifier;
		private HtmlMinifier _mediumRemovingWhitespaceMinifier;
		private HtmlMinifier _mediumRemovingWhitespaceExceptForNewLinesMinifier;
		private HtmlMinifier _aggressiveRemovingWhitespaceMinifier;
		private HtmlMinifier _aggressiveRemovingWhitespaceExceptForNewLinesMinifier;


		public WhitespaceMinificationTests()
		{
			_keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.None,
					PreserveNewLines = false
				});
			_keepingWhitespaceAndNewLinesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.None,
					PreserveNewLines = true
				});
			_safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Safe,
					PreserveNewLines = false
				});
			_safeRemovingWhitespaceExceptForNewLinesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Safe,
					PreserveNewLines = true
				});
			_mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Medium,
					PreserveNewLines = false
				});
			_mediumRemovingWhitespaceExceptForNewLinesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Medium,
					PreserveNewLines = true
				});
			_aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive,
					PreserveNewLines = false
				});
			_aggressiveRemovingWhitespaceExceptForNewLinesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive,
					PreserveNewLines = true
				});
		}


		[Fact]
		public void WhitespaceMinificationInStyleTagIsCorrect()
		{
			// Arrange
			const string input1 = "<style>cite { quotes: \" «\" \"» \"; }    </style>";
			const string targetOutput1A = input1;
			const string targetOutput1B = input1;
			const string targetOutput1C = "<style>cite { quotes: \" «\" \"» \"; }</style>";
			const string targetOutput1D = targetOutput1C;
			const string targetOutput1E = targetOutput1C;
			const string targetOutput1F = targetOutput1C;
			const string targetOutput1G = targetOutput1C;
			const string targetOutput1H = targetOutput1C;

			const string input2 = "<style>\r\n" +
				".item-list > span::after {\r\n" +
				"  content: \"\\a\";\r\n" +
				"  white-space: pre;\r\n" +
				"}\r\n\r\n" +
				"</style>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = input2;
			const string targetOutput2C = "<style>" +
				".item-list > span::after {\r\n" +
				"  content: \"\\a\";\r\n" +
				"  white-space: pre;\r\n" +
				"}" +
				"</style>"
				;
			const string targetOutput2D = "<style>\r\n" +
				".item-list > span::after {\r\n" +
				"  content: \"\\a\";\r\n" +
				"  white-space: pre;\r\n" +
				"}\r\n" +
				"</style>"
				;
			const string targetOutput2E = targetOutput2C;
			const string targetOutput2F = targetOutput2D;
			const string targetOutput2G = targetOutput2C;
			const string targetOutput2H = targetOutput2D;

			// Act
			string output1A = _keepingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1B = _keepingWhitespaceAndNewLinesMinifier.Minify(input1).MinifiedContent;
			string output1C = _safeRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input1).MinifiedContent;
			string output1E = _mediumRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input1).MinifiedContent;
			string output1G = _aggressiveRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input1).MinifiedContent;

			string output2A = _keepingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2B = _keepingWhitespaceAndNewLinesMinifier.Minify(input2).MinifiedContent;
			string output2C = _safeRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input2).MinifiedContent;
			string output2E = _mediumRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input2).MinifiedContent;
			string output2G = _aggressiveRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);
			Assert.Equal(targetOutput1D, output1D);
			Assert.Equal(targetOutput1E, output1E);
			Assert.Equal(targetOutput1F, output1F);
			Assert.Equal(targetOutput1G, output1G);
			Assert.Equal(targetOutput1H, output1H);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);
			Assert.Equal(targetOutput2D, output2D);
			Assert.Equal(targetOutput2E, output2E);
			Assert.Equal(targetOutput2F, output2F);
			Assert.Equal(targetOutput2G, output2G);
			Assert.Equal(targetOutput2H, output2H);
		}

		[Fact]
		public void WhitespaceMinificationInScriptTagIsCorrect()
		{
			// Arrange
			const string input1 = "<script>alert(\"Hello,     world!\");    </script>";
			const string targetOutput1A = input1;
			const string targetOutput1B = input1;
			const string targetOutput1C = "<script>alert(\"Hello,     world!\");</script>";
			const string targetOutput1D = targetOutput1C;
			const string targetOutput1E = targetOutput1C;
			const string targetOutput1F = targetOutput1C;
			const string targetOutput1G = targetOutput1C;
			const string targetOutput1H = targetOutput1C;

			const string input2 = "<script>\r\r" +
				"    var userName = prompt(\"Good time of the day!\\r\\rPlease enter your name:\", \"anonymous\");\r" +
				"    alert(\"Glad to see you, \" + userName + \"!\");\r" +
				"</script>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = input2;
			const string targetOutput2C = "<script>" +
				"var userName = prompt(\"Good time of the day!\\r\\rPlease enter your name:\", \"anonymous\");\r" +
				"    alert(\"Glad to see you, \" + userName + \"!\");" +
				"</script>"
				;
			const string targetOutput2D = "<script>\r" +
				"var userName = prompt(\"Good time of the day!\\r\\rPlease enter your name:\", \"anonymous\");\r" +
				"    alert(\"Glad to see you, \" + userName + \"!\");\r" +
				"</script>"
				;
			const string targetOutput2E = targetOutput2C;
			const string targetOutput2F = targetOutput2D;
			const string targetOutput2G = targetOutput2C;
			const string targetOutput2H = targetOutput2D;

			// Act
			string output1A = _keepingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1B = _keepingWhitespaceAndNewLinesMinifier.Minify(input1).MinifiedContent;
			string output1C = _safeRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input1).MinifiedContent;
			string output1E = _mediumRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input1).MinifiedContent;
			string output1G = _aggressiveRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input1).MinifiedContent;

			string output2A = _keepingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2B = _keepingWhitespaceAndNewLinesMinifier.Minify(input2).MinifiedContent;
			string output2C = _safeRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input2).MinifiedContent;
			string output2E = _mediumRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input2).MinifiedContent;
			string output2G = _aggressiveRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);
			Assert.Equal(targetOutput1D, output1D);
			Assert.Equal(targetOutput1E, output1E);
			Assert.Equal(targetOutput1F, output1F);
			Assert.Equal(targetOutput1G, output1G);
			Assert.Equal(targetOutput1H, output1H);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);
			Assert.Equal(targetOutput2D, output2D);
			Assert.Equal(targetOutput2E, output2E);
			Assert.Equal(targetOutput2F, output2F);
			Assert.Equal(targetOutput2G, output2G);
			Assert.Equal(targetOutput2H, output2H);
		}

		[Fact]
		public void WhitespaceMinificationInParagraphTagIsCorrect()
		{
			// Arrange
			const string input = "<p>  New  \n  Release	\n</p>";
			const string targetOutputA = input;
			const string targetOutputB = input;
			const string targetOutputC = "<p> New Release </p>";
			const string targetOutputD = "<p> New\nRelease\n</p>";
			const string targetOutputE = "<p>New Release</p>";
			const string targetOutputF = "<p>New\nRelease\n</p>";
			const string targetOutputG = targetOutputE;
			const string targetOutputH = targetOutputF;

			// Act
			string outputA = _keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = _keepingWhitespaceAndNewLinesMinifier.Minify(input).MinifiedContent;
			string outputC = _safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputE = _mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputF = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputG = _aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputH = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
			Assert.Equal(targetOutputE, outputE);
			Assert.Equal(targetOutputF, outputF);
			Assert.Equal(targetOutputG, outputG);
			Assert.Equal(targetOutputH, outputH);
		}

		[Fact]
		public void WhitespaceMinificationInUnorderedListTagIsCorrect()
		{
			// Arrange
			const string input = "<ul>\n" +
				"	<li>	 Item 1 \n</li>\n" +
				"	<li>	 Item 2\n" +
				"		<ul>\n" +
				"			<li>	 Item 21 \n</li>\n" +
				"			<li>	 Item 22 \n</li>\n" +
				"		</ul>\n" +
				"	</li>\n" +
				"	<li>	 Item 3 \n</li>\n" +
				"</ul>"
				;
			const string targetOutputA = input;
			const string targetOutputB = input;
			const string targetOutputC = "<ul>" +
				"<li> Item 1 </li> " +
				"<li> Item 2 " +
				"<ul>" +
				"<li> Item 21 </li> " +
				"<li> Item 22 </li>" +
				"</ul> " +
				"</li> " +
				"<li> Item 3 </li>" +
				"</ul>"
				;
			const string targetOutputD = "<ul>\n" +
				"<li> Item 1\n</li>\n" +
				"<li> Item 2\n" +
				"<ul>\n" +
				"<li> Item 21\n</li>\n" +
				"<li> Item 22\n</li>\n" +
				"</ul>\n" +
				"</li>\n" +
				"<li> Item 3\n</li>\n" +
				"</ul>"
				;
			const string targetOutputE = "<ul>" +
				"<li>Item 1</li>" +
				"<li>Item 2" +
				"<ul>" +
				"<li>Item 21</li>" +
				"<li>Item 22</li>" +
				"</ul>" +
				"</li>" +
				"<li>Item 3</li>" +
				"</ul>"
				;
			const string targetOutputF = "<ul>\n" +
				"<li>Item 1\n</li>\n" +
				"<li>Item 2\n" +
				"<ul>\n" +
				"<li>Item 21\n</li>\n" +
				"<li>Item 22\n</li>\n" +
				"</ul>\n" +
				"</li>\n" +
				"<li>Item 3\n</li>\n" +
				"</ul>"
				;
			const string targetOutputG = targetOutputE;
			const string targetOutputH = targetOutputF;

			// Act
			string outputA = _keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = _keepingWhitespaceAndNewLinesMinifier.Minify(input).MinifiedContent;
			string outputC = _safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputE = _mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputF = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputG = _aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputH = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
			Assert.Equal(targetOutputE, outputE);
			Assert.Equal(targetOutputF, outputF);
			Assert.Equal(targetOutputG, outputG);
			Assert.Equal(targetOutputH, outputH);
		}

		[Fact]
		public void WhitespaceMinificationInDescriptionListTagIsCorrect()
		{
			// Arrange
			const string input = "<dl>\n" +
				"	<dt>  Name:  </dt>\n" +
				"	<dd>  John Doe  \n" +
				"</dd>\n\n" +
				"	<dt>  Gender:  </dt>\n" +
				"	<dd>  Male  \n" +
				"</dd>\n\n" +
				"	<dt>  Day  of  Birth:  </dt>\n" +
				"	<dd>  Unknown  \n" +
				"</dd>\n" +
				"</dl>"
				;
			const string targetOutputA = input;
			const string targetOutputB = input;
			const string targetOutputC = "<dl>" +
				"<dt> Name: </dt> " +
				"<dd> John Doe </dd> " +
				"<dt> Gender: </dt> " +
				"<dd> Male </dd> " +
				"<dt> Day of Birth: </dt> " +
				"<dd> Unknown </dd>" +
				"</dl>"
				;
			const string targetOutputD = "<dl>\n" +
				"<dt> Name: </dt>\n" +
				"<dd> John Doe\n" +
				"</dd>\n" +
				"<dt> Gender: </dt>\n" +
				"<dd> Male\n" +
				"</dd>\n" +
				"<dt> Day of Birth: </dt>\n" +
				"<dd> Unknown\n" +
				"</dd>\n" +
				"</dl>"
				;
			const string targetOutputE = "<dl>" +
				"<dt>Name:</dt>" +
				"<dd>John Doe</dd>" +
				"<dt>Gender:</dt>" +
				"<dd>Male</dd>" +
				"<dt>Day of Birth:</dt>" +
				"<dd>Unknown</dd>" +
				"</dl>"
				;
			const string targetOutputF = "<dl>\n" +
				"<dt>Name:</dt>\n" +
				"<dd>John Doe\n" +
				"</dd>\n" +
				"<dt>Gender:</dt>\n" +
				"<dd>Male\n" +
				"</dd>\n" +
				"<dt>Day of Birth:</dt>\n" +
				"<dd>Unknown\n" +
				"</dd>\n" +
				"</dl>"
				;
			const string targetOutputG = targetOutputE;
			const string targetOutputH = targetOutputF;

			// Act
			string outputA = _keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = _keepingWhitespaceAndNewLinesMinifier.Minify(input).MinifiedContent;
			string outputC = _safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputE = _mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputF = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputG = _aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputH = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
			Assert.Equal(targetOutputE, outputE);
			Assert.Equal(targetOutputF, outputF);
			Assert.Equal(targetOutputG, outputG);
			Assert.Equal(targetOutputH, outputH);
		}

		[Fact]
		public void WhitespaceMinificationInRubyTagIsCorrect()
		{
			// Arrange
			const string input1 = "<ruby>\n" +
				"	漢  <rt>  Kan  </rt>\n" +
				"	字  <rt>  ji  </rt>\n" +
				"</ruby>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = input1;
			const string targetOutput1C = "<ruby> " +
				"漢 <rt> Kan </rt> " +
				"字 <rt> ji </rt> " +
				"</ruby>"
				;
			const string targetOutput1D = "<ruby>\n" +
				"漢 <rt> Kan </rt>\n" +
				"字 <rt> ji </rt>\n" +
				"</ruby>"
				;
			const string targetOutput1E = targetOutput1C;
			const string targetOutput1F = targetOutput1D;
			const string targetOutput1G = "<ruby>" +
				"漢 <rt>Kan</rt> " +
				"字 <rt>ji</rt>" +
				"</ruby>"
				;
			const string targetOutput1H = "<ruby>\n" +
				"漢 <rt>Kan</rt>\n" +
				"字 <rt>ji</rt>\n" +
				"</ruby>"
				;

			const string input2 = "<ruby>\n" +
				"	漢  <rp>  (</rp>  <rt>  Kan  </rt>  <rp>)  </rp>\n" +
				"	字  <rp>  (</rp>  <rt>  ji  </rt>  <rp>)  </rp>\n" +
				"</ruby>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = input2;
			const string targetOutput2C = "<ruby> " +
				"漢 <rp> (</rp> <rt> Kan </rt> <rp>) </rp> " +
				"字 <rp> (</rp> <rt> ji </rt> <rp>) </rp> " +
				"</ruby>"
				;
			const string targetOutput2D = "<ruby>\n" +
				"漢 <rp> (</rp> <rt> Kan </rt> <rp>) </rp>\n" +
				"字 <rp> (</rp> <rt> ji </rt> <rp>) </rp>\n" +
				"</ruby>"
				;
			const string targetOutput2E = targetOutput2C;
			const string targetOutput2F = targetOutput2D;
			const string targetOutput2G = "<ruby>" +
				"漢 <rp>(</rp> <rt>Kan</rt> <rp>)</rp> " +
				"字 <rp>(</rp> <rt>ji</rt> <rp>)</rp>" +
				"</ruby>"
				;
			const string targetOutput2H = "<ruby>\n" +
				"漢 <rp>(</rp> <rt>Kan</rt> <rp>)</rp>\n" +
				"字 <rp>(</rp> <rt>ji</rt> <rp>)</rp>\n" +
				"</ruby>"
				;

			const string input3 = "<ruby>\n" +
				"	東\n" +
				"	<rb>  京  </rb>\n" +
				"	<rt>  とう  </rt>\n" +
				"	<rt>  きょう  </rt>\n" +
				"</ruby>"
				;
			const string targetOutput3A = input3;
			const string targetOutput3B = input3;
			const string targetOutput3C = "<ruby> " +
				"東 " +
				"<rb> 京 </rb> " +
				"<rt> とう </rt> " +
				"<rt> きょう </rt> " +
				"</ruby>";
			const string targetOutput3D = "<ruby>\n" +
				"東\n" +
				"<rb> 京 </rb>\n" +
				"<rt> とう </rt>\n" +
				"<rt> きょう </rt>\n" +
				"</ruby>"
				;
			const string targetOutput3E = targetOutput3C;
			const string targetOutput3F = targetOutput3D;
			const string targetOutput3G = "<ruby>" +
				"東 " +
				"<rb>京</rb> " +
				"<rt>とう</rt> " +
				"<rt>きょう</rt>" +
				"</ruby>"
				;
			const string targetOutput3H = "<ruby>\n" +
				"東\n" +
				"<rb>京</rb>\n" +
				"<rt>とう</rt>\n" +
				"<rt>きょう</rt>\n" +
				"</ruby>"
				;

			const string input4 = "<ruby>\n" +
				"	♥\n" +
				"	<rp>:  </rp>\n" +
				"	<rt>  Heart  </rt>\n" +
				"	<rp>,  </rp>\n" +
				"	<rtc>\n" +
				"		<rt lang=\"ru\">  Сердце  </rt>\n" +
				"	</rtc>\n" +
				"	<rp>.  </rp>\n" +
				"	☘\n" +
				"	<rp>:  </rp>\n" +
				"	<rt>  Shamrock  </rt>\n" +
				"	<rp>,  </rp>\n" +
				"	<rtc>\n" +
				"		<rt lang=\"ru\">  Трилистник  </rt>\n" +
				"	</rtc>\n" +
				"	<rp>.  </rp>\n" +
				"	✶\n" +
				"	<rp>:  </rp>\n" +
				"	<rt>  Star  </rt>\n" +
				"	<rp>,  </rp>\n" +
				"	<rtc>\n" +
				"		<rt lang=\"ru\">  Звезда  </rt>\n" +
				"	</rtc>\n" +
				"	<rp>.  </rp>\n" +
				"</ruby>"
				;
			const string targetOutput4A = input4;
			const string targetOutput4B = input4;
			const string targetOutput4C = "<ruby> " +
				"♥ " +
				"<rp>: </rp> " +
				"<rt> Heart </rt> " +
				"<rp>, </rp> " +
				"<rtc> " +
				"<rt lang=\"ru\"> Сердце </rt> " +
				"</rtc> " +
				"<rp>. </rp> " +
				"☘ " +
				"<rp>: </rp> " +
				"<rt> Shamrock </rt> " +
				"<rp>, </rp> " +
				"<rtc> " +
				"<rt lang=\"ru\"> Трилистник </rt> " +
				"</rtc> " +
				"<rp>. </rp> " +
				"✶ " +
				"<rp>: </rp> " +
				"<rt> Star </rt> " +
				"<rp>, </rp> " +
				"<rtc> " +
				"<rt lang=\"ru\"> Звезда </rt> " +
				"</rtc> " +
				"<rp>. </rp> " +
				"</ruby>"
				;
			const string targetOutput4D = "<ruby>\n" +
				"♥\n" +
				"<rp>: </rp>\n" +
				"<rt> Heart </rt>\n" +
				"<rp>, </rp>\n" +
				"<rtc>\n" +
				"<rt lang=\"ru\"> Сердце </rt>\n" +
				"</rtc>\n" +
				"<rp>. </rp>\n" +
				"☘\n" +
				"<rp>: </rp>\n" +
				"<rt> Shamrock </rt>\n" +
				"<rp>, </rp>\n" +
				"<rtc>\n" +
				"<rt lang=\"ru\"> Трилистник </rt>\n" +
				"</rtc>\n" +
				"<rp>. </rp>\n" +
				"✶\n" +
				"<rp>: </rp>\n" +
				"<rt> Star </rt>\n" +
				"<rp>, </rp>\n" +
				"<rtc>\n" +
				"<rt lang=\"ru\"> Звезда </rt>\n" +
				"</rtc>\n" +
				"<rp>. </rp>\n" +
				"</ruby>"
				;
			const string targetOutput4E = targetOutput4C;
			const string targetOutput4F = targetOutput4D;
			const string targetOutput4G = "<ruby>" +
				"♥ " +
				"<rp>:</rp> " +
				"<rt>Heart</rt> " +
				"<rp>,</rp> " +
				"<rtc>" +
				"<rt lang=\"ru\">Сердце</rt>" +
				"</rtc> " +
				"<rp>.</rp> " +
				"☘ " +
				"<rp>:</rp> " +
				"<rt>Shamrock</rt> " +
				"<rp>,</rp> " +
				"<rtc>" +
				"<rt lang=\"ru\">Трилистник</rt>" +
				"</rtc> " +
				"<rp>.</rp> " +
				"✶ " +
				"<rp>:</rp> " +
				"<rt>Star</rt> " +
				"<rp>,</rp> " +
				"<rtc>" +
				"<rt lang=\"ru\">Звезда</rt>" +
				"</rtc> " +
				"<rp>.</rp>" +
				"</ruby>"
				;
			const string targetOutput4H = "<ruby>\n" +
				"♥\n" +
				"<rp>:</rp>\n" +
				"<rt>Heart</rt>\n" +
				"<rp>,</rp>\n" +
				"<rtc>\n" +
				"<rt lang=\"ru\">Сердце</rt>\n" +
				"</rtc>\n" +
				"<rp>.</rp>\n" +
				"☘\n" +
				"<rp>:</rp>\n" +
				"<rt>Shamrock</rt>\n" +
				"<rp>,</rp>\n" +
				"<rtc>\n" +
				"<rt lang=\"ru\">Трилистник</rt>\n" +
				"</rtc>\n" +
				"<rp>.</rp>\n" +
				"✶\n" +
				"<rp>:</rp>\n" +
				"<rt>Star</rt>\n" +
				"<rp>,</rp>\n" +
				"<rtc>\n" +
				"<rt lang=\"ru\">Звезда</rt>\n" +
				"</rtc>\n" +
				"<rp>.</rp>\n" +
				"</ruby>"
				;

			const string input5 = "<ruby>\n" +
				"	<rb>  旧  </rb>  <rb>  金  </rb>  <rb>  山  </rb>\n" +
				"	<rt>  jiù  </rt>  <rt>  jīn  </rt> <rt>  shān  </rt>\n" +
				"	<rtc>  Сан-Франциско  </rtc>\n" +
				"</ruby>"
				;
			const string targetOutput5A = input5;
			const string targetOutput5B = input5;
			const string targetOutput5C = "<ruby> " +
				"<rb> 旧 </rb> <rb> 金 </rb> <rb> 山 </rb> " +
				"<rt> jiù </rt> <rt> jīn </rt> <rt> shān </rt> " +
				"<rtc> Сан-Франциско </rtc> " +
				"</ruby>"
				;
			const string targetOutput5D = "<ruby>\n" +
				"<rb> 旧 </rb> <rb> 金 </rb> <rb> 山 </rb>\n" +
				"<rt> jiù </rt> <rt> jīn </rt> <rt> shān </rt>\n" +
				"<rtc> Сан-Франциско </rtc>\n" +
				"</ruby>"
				;
			const string targetOutput5E = targetOutput5C;
			const string targetOutput5F = targetOutput5D;
			const string targetOutput5G = "<ruby>" +
				"<rb>旧</rb> <rb>金</rb> <rb>山</rb> " +
				"<rt>jiù</rt> <rt>jīn</rt> <rt>shān</rt> " +
				"<rtc>Сан-Франциско</rtc>" +
				"</ruby>"
				;
			const string targetOutput5H = "<ruby>\n" +
				"<rb>旧</rb> <rb>金</rb> <rb>山</rb>\n" +
				"<rt>jiù</rt> <rt>jīn</rt> <rt>shān</rt>\n" +
				"<rtc>Сан-Франциско</rtc>\n" +
				"</ruby>"
				;

			// Act
			string output1A = _keepingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1B = _keepingWhitespaceAndNewLinesMinifier.Minify(input1).MinifiedContent;
			string output1C = _safeRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input1).MinifiedContent;
			string output1E = _mediumRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input1).MinifiedContent;
			string output1G = _aggressiveRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input1).MinifiedContent;

			string output2A = _keepingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2B = _keepingWhitespaceAndNewLinesMinifier.Minify(input2).MinifiedContent;
			string output2C = _safeRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input2).MinifiedContent;
			string output2E = _mediumRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input2).MinifiedContent;
			string output2G = _aggressiveRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input2).MinifiedContent;

			string output3A = _keepingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3B = _keepingWhitespaceAndNewLinesMinifier.Minify(input3).MinifiedContent;
			string output3C = _safeRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input3).MinifiedContent;
			string output3E = _mediumRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input3).MinifiedContent;
			string output3G = _aggressiveRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input3).MinifiedContent;

			string output4A = _keepingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4B = _keepingWhitespaceAndNewLinesMinifier.Minify(input4).MinifiedContent;
			string output4C = _safeRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input4).MinifiedContent;
			string output4E = _mediumRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input4).MinifiedContent;
			string output4G = _aggressiveRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input4).MinifiedContent;

			string output5A = _keepingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5B = _keepingWhitespaceAndNewLinesMinifier.Minify(input5).MinifiedContent;
			string output5C = _safeRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input5).MinifiedContent;
			string output5E = _mediumRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input5).MinifiedContent;
			string output5G = _aggressiveRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input5).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);
			Assert.Equal(targetOutput1D, output1D);
			Assert.Equal(targetOutput1E, output1E);
			Assert.Equal(targetOutput1F, output1F);
			Assert.Equal(targetOutput1G, output1G);
			Assert.Equal(targetOutput1H, output1H);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);
			Assert.Equal(targetOutput2D, output2D);
			Assert.Equal(targetOutput2E, output2E);
			Assert.Equal(targetOutput2F, output2F);
			Assert.Equal(targetOutput2G, output2G);
			Assert.Equal(targetOutput2H, output2H);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);
			Assert.Equal(targetOutput3D, output3D);
			Assert.Equal(targetOutput3E, output3E);
			Assert.Equal(targetOutput3F, output3F);
			Assert.Equal(targetOutput3G, output3G);
			Assert.Equal(targetOutput3H, output3H);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);
			Assert.Equal(targetOutput4D, output4D);
			Assert.Equal(targetOutput4E, output4E);
			Assert.Equal(targetOutput4F, output4F);
			Assert.Equal(targetOutput4G, output4G);
			Assert.Equal(targetOutput4H, output4H);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);
			Assert.Equal(targetOutput5D, output5D);
			Assert.Equal(targetOutput5E, output5E);
			Assert.Equal(targetOutput5F, output5F);
			Assert.Equal(targetOutput5G, output5G);
			Assert.Equal(targetOutput5H, output5H);
		}

		[Fact]
		public void WhitespaceMinificationInFigureTagIsCorrect()
		{
			// Arrange
			const string input = "<figure>\n" +
				"	<img src=\"libsass-logo.png\" alt=\"LibSass logo\" width=\"640\" height=\"320\">\n" +
				"	<figcaption>  Fig 1.  -  LibSass logo. \n" +
				"</figcaption>\n" +
				"</figure>"
				;
			const string targetOutputA = input;
			const string targetOutputB = input;
			const string targetOutputC = "<figure>" +
				" <img src=\"libsass-logo.png\" alt=\"LibSass logo\" width=\"640\" height=\"320\"> " +
				"<figcaption> Fig 1. - LibSass logo. </figcaption>" +
				"</figure>"
				;
			const string targetOutputD = "<figure>\n" +
				"<img src=\"libsass-logo.png\" alt=\"LibSass logo\" width=\"640\" height=\"320\">\n" +
				"<figcaption> Fig 1. - LibSass logo.\n" +
				"</figcaption>\n" +
				"</figure>"
				;
			const string targetOutputE = "<figure>" +
				"<img src=\"libsass-logo.png\" alt=\"LibSass logo\" width=\"640\" height=\"320\">" +
				"<figcaption>Fig 1. - LibSass logo.</figcaption>" +
				"</figure>"
				;
			const string targetOutputF = "<figure>\n" +
				"<img src=\"libsass-logo.png\" alt=\"LibSass logo\" width=\"640\" height=\"320\">\n" +
				"<figcaption>Fig 1. - LibSass logo.\n" +
				"</figcaption>\n" +
				"</figure>"
				;
			const string targetOutputG = targetOutputE;
			const string targetOutputH = targetOutputF;

			// Act
			string outputA = _keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = _keepingWhitespaceAndNewLinesMinifier.Minify(input).MinifiedContent;
			string outputC = _safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputE = _mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputF = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputG = _aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputH = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
			Assert.Equal(targetOutputE, outputE);
			Assert.Equal(targetOutputF, outputF);
			Assert.Equal(targetOutputG, outputG);
			Assert.Equal(targetOutputH, outputH);
		}

		[Fact]
		public void WhitespaceMinificationInTableTagIsCorrect()
		{
			// Arrange
			const string input = "<table class=\"table\">\n" +
				"	<caption>	 Monthly savings \n</caption>\n" +
				"	<colgroup>\n" +
				"		<col style=\"text-align: left\">\n" +
				"		<col style=\"text-align: right\">\n" +
				"	</colgroup>\n" +
				"	<thead>\n" +
				"		<tr>\n" +
				"			<th>	 Month \n</th>\n" +
				"			<th>	 Savings \n</th>\n" +
				"		</tr>\n" +
				"	</thead>\n" +
				"	<tbody>\n" +
				"		<tr>\n" +
				"			<td>	 Jul \n</td>\n" +
				"			<td>	 $2900 \n</td>\n" +
				"		</tr>\n" +
				"		<tr>\n" +
				"			<td>	 Oct \n</td>\n" +
				"			<td>	 $3120 \n</td>\n" +
				"		</tr>\n" +
				"	</tbody>\n" +
				"	<tfoot>\n" +
				"		<tr>\n" +
				"			<td>	 Total \n</td>\n" +
				"			<td>	 $6250 \n</td>\n" +
				"		</tr>\n" +
				"	</tfoot>\n" +
				"</table>"
				;
			const string targetOutputA = input;
			const string targetOutputB = input;
			const string targetOutputC = "<table class=\"table\">" +
				"<caption> Monthly savings </caption>" +
				"<colgroup>" +
				"<col style=\"text-align: left\">" +
				"<col style=\"text-align: right\">" +
				"</colgroup>" +
				"<thead>" +
				"<tr>" +
				"<th> Month </th>" +
				"<th> Savings </th>" +
				"</tr>" +
				"</thead>" +
				"<tbody>" +
				"<tr>" +
				"<td> Jul </td>" +
				"<td> $2900 </td>" +
				"</tr>" +
				"<tr>" +
				"<td> Oct </td>" +
				"<td> $3120 </td>" +
				"</tr>" +
				"</tbody>" +
				"<tfoot>" +
				"<tr>" +
				"<td> Total </td>" +
				"<td> $6250 </td>" +
				"</tr>" +
				"</tfoot>" +
				"</table>"
				;
			const string targetOutputD = "<table class=\"table\">\n" +
				"<caption> Monthly savings\n</caption>\n" +
				"<colgroup>\n" +
				"<col style=\"text-align: left\">\n" +
				"<col style=\"text-align: right\">\n" +
				"</colgroup>\n" +
				"<thead>\n" +
				"<tr>\n" +
				"<th> Month\n</th>\n" +
				"<th> Savings\n</th>\n" +
				"</tr>\n" +
				"</thead>\n" +
				"<tbody>\n" +
				"<tr>\n" +
				"<td> Jul\n</td>\n" +
				"<td> $2900\n</td>\n" +
				"</tr>\n" +
				"<tr>\n" +
				"<td> Oct\n</td>\n" +
				"<td> $3120\n</td>\n" +
				"</tr>\n" +
				"</tbody>\n" +
				"<tfoot>\n" +
				"<tr>\n" +
				"<td> Total\n</td>\n" +
				"<td> $6250\n</td>\n" +
				"</tr>\n" +
				"</tfoot>\n" +
				"</table>"
				;
			const string targetOutputE = "<table class=\"table\">" +
				"<caption>Monthly savings</caption>" +
				"<colgroup>" +
				"<col style=\"text-align: left\">" +
				"<col style=\"text-align: right\">" +
				"</colgroup>" +
				"<thead>" +
				"<tr>" +
				"<th>Month</th>" +
				"<th>Savings</th>" +
				"</tr>" +
				"</thead>" +
				"<tbody>" +
				"<tr>" +
				"<td>Jul</td>" +
				"<td>$2900</td>" +
				"</tr>" +
				"<tr>" +
				"<td>Oct</td>" +
				"<td>$3120</td>" +
				"</tr>" +
				"</tbody>" +
				"<tfoot>" +
				"<tr>" +
				"<td>Total</td>" +
				"<td>$6250</td>" +
				"</tr>" +
				"</tfoot>" +
				"</table>"
				;
			const string targetOutputF = "<table class=\"table\">\n" +
				"<caption>Monthly savings\n</caption>\n" +
				"<colgroup>\n" +
				"<col style=\"text-align: left\">\n" +
				"<col style=\"text-align: right\">\n" +
				"</colgroup>\n" +
				"<thead>\n" +
				"<tr>\n" +
				"<th>Month\n</th>\n" +
				"<th>Savings\n</th>\n" +
				"</tr>\n" +
				"</thead>\n" +
				"<tbody>\n" +
				"<tr>\n" +
				"<td>Jul\n</td>\n" +
				"<td>$2900\n</td>\n" +
				"</tr>\n" +
				"<tr>\n" +
				"<td>Oct\n</td>\n" +
				"<td>$3120\n</td>\n" +
				"</tr>\n" +
				"</tbody>\n" +
				"<tfoot>\n" +
				"<tr>\n" +
				"<td>Total\n</td>\n" +
				"<td>$6250\n</td>\n" +
				"</tr>\n" +
				"</tfoot>\n" +
				"</table>"
				;
			const string targetOutputG = targetOutputE;
			const string targetOutputH = targetOutputF;

			// Act
			string outputA = _keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = _keepingWhitespaceAndNewLinesMinifier.Minify(input).MinifiedContent;
			string outputC = _safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputE = _mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputF = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputG = _aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputH = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
			Assert.Equal(targetOutputE, outputE);
			Assert.Equal(targetOutputF, outputF);
			Assert.Equal(targetOutputG, outputG);
			Assert.Equal(targetOutputH, outputH);
		}

		[Fact]
		public void WhitespaceMinificationInFormAndFieldSetTagsIsCorrect()
		{
			// Arrange
			const string input = "<form>\n" +
				"	<fieldset>\n" +
				"		<legend>  Personal data \n" +
				"</legend>\n" +
				"		Name: <input type=\"text\" size=\"50\"><br>\n" +
				"		Email: <input type=\"text\" size=\"50\"><br>\n" +
				"		Date of birth: <input type=\"text\" size=\"10\">\n" +
				"	</fieldset>\n" +
				"</form>"
				;
			const string targetOutputA = input;
			const string targetOutputB = input;
			const string targetOutputC = "<form> " +
				"<fieldset>" +
				"<legend> Personal data </legend>" +
				"Name: <input type=\"text\" size=\"50\"><br> " +
				"Email: <input type=\"text\" size=\"50\"><br> " +
				"Date of birth: <input type=\"text\" size=\"10\"> " +
				"</fieldset> " +
				"</form>"
				;
			const string targetOutputD = "<form>\n" +
				"<fieldset>\n" +
				"<legend> Personal data\n" +
				"</legend>\n" +
				"Name: <input type=\"text\" size=\"50\"><br>\n" +
				"Email: <input type=\"text\" size=\"50\"><br>\n" +
				"Date of birth: <input type=\"text\" size=\"10\">\n" +
				"</fieldset>\n" +
				"</form>"
				;
			const string targetOutputE = "<form>" +
				"<fieldset>" +
				"<legend>Personal data</legend>" +
				"Name: <input type=\"text\" size=\"50\"><br> " +
				"Email: <input type=\"text\" size=\"50\"><br> " +
				"Date of birth: <input type=\"text\" size=\"10\">" +
				"</fieldset>" +
				"</form>"
				;
			const string targetOutputF = "<form>\n" +
				"<fieldset>\n" +
				"<legend>Personal data\n" +
				"</legend>\n" +
				"Name: <input type=\"text\" size=\"50\"><br>\n" +
				"Email: <input type=\"text\" size=\"50\"><br>\n" +
				"Date of birth: <input type=\"text\" size=\"10\">\n" +
				"</fieldset>\n" +
				"</form>"
				;
			const string targetOutputG = targetOutputE;
			const string targetOutputH = targetOutputF;

			// Act
			string outputA = _keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = _keepingWhitespaceAndNewLinesMinifier.Minify(input).MinifiedContent;
			string outputC = _safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputE = _mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputF = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputG = _aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputH = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
			Assert.Equal(targetOutputE, outputE);
			Assert.Equal(targetOutputF, outputF);
			Assert.Equal(targetOutputG, outputG);
			Assert.Equal(targetOutputH, outputH);
		}

		[Fact]
		public void WhitespaceMinificationInLabelAndTextareaTagsIsCorrect()
		{
			// Arrange
			const string input = "<label>		Text:  \n </label> \n\t  " +
				"<textarea> THERE IS NO     KNOWLEDGE \n\n   – \t    THAT IS NOT POWER </textarea> \t\n  ";
			const string targetOutputA = input;
			const string targetOutputB = input;
			const string targetOutputC = "<label> Text: </label> " +
				"<textarea> THERE IS NO     KNOWLEDGE \n\n   – \t    THAT IS NOT POWER </textarea>";
			const string targetOutputD = "<label> Text:\n</label>\n" +
				"<textarea> THERE IS NO     KNOWLEDGE \n\n   – \t    THAT IS NOT POWER </textarea>";
			const string targetOutputE = targetOutputC;
			const string targetOutputF = targetOutputD;
			const string targetOutputG = "<label>Text:</label> " +
				"<textarea> THERE IS NO     KNOWLEDGE \n\n   – \t    THAT IS NOT POWER </textarea>";
			const string targetOutputH = "<label>Text:\n</label>\n" +
				"<textarea> THERE IS NO     KNOWLEDGE \n\n   – \t    THAT IS NOT POWER </textarea>";

			// Act
			string outputA = _keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = _keepingWhitespaceAndNewLinesMinifier.Minify(input).MinifiedContent;
			string outputC = _safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputE = _mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputF = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputG = _aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputH = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
			Assert.Equal(targetOutputE, outputE);
			Assert.Equal(targetOutputF, outputF);
			Assert.Equal(targetOutputG, outputG);
			Assert.Equal(targetOutputH, outputH);
		}

		[Fact]
		public void WhitespaceMinificationInSelectTagIsCorrect()
		{
			// Arrange
			const string input = "<select name=\"preprocessors\">\n" +
				"	<optgroup label=\"Styles\">\n" +
				"		<option value=\"sass\">	 Sass \n</option>\n" +
				"		<option value=\"less\">	 LESS \n</option>\n" +
				"		<option value=\"stylus\">	 Stylus \n</option>\n" +
				"	</optgroup>\n" +
				"	<optgroup label=\"Scripts\">\n" +
				"		<option value=\"coffeescript\">	 CoffeeScript \n</option>\n" +
				"		<option value=\"typescript\">	 TypeScript \n</option>\n" +
				"		<option value=\"kaffeine\">	 Kaffeine \n</option>\n" +
				"	</optgroup>\n" +
				"	<option value=\"dart\">	 Dart \n</option>\n" +
				"</select>"
				;
			const string targetOutputA = input;
			const string targetOutputB = input;
			const string targetOutputC = "<select name=\"preprocessors\">" +
				"<optgroup label=\"Styles\">" +
				"<option value=\"sass\"> Sass </option>" +
				"<option value=\"less\"> LESS </option>" +
				"<option value=\"stylus\"> Stylus </option>" +
				"</optgroup>" +
				"<optgroup label=\"Scripts\">" +
				"<option value=\"coffeescript\"> CoffeeScript </option>" +
				"<option value=\"typescript\"> TypeScript </option>" +
				"<option value=\"kaffeine\"> Kaffeine </option>" +
				"</optgroup>" +
				"<option value=\"dart\"> Dart </option>" +
				"</select>"
				;
			const string targetOutputD = "<select name=\"preprocessors\">\n" +
				"<optgroup label=\"Styles\">\n" +
				"<option value=\"sass\"> Sass\n</option>\n" +
				"<option value=\"less\"> LESS\n</option>\n" +
				"<option value=\"stylus\"> Stylus\n</option>\n" +
				"</optgroup>\n" +
				"<optgroup label=\"Scripts\">\n" +
				"<option value=\"coffeescript\"> CoffeeScript\n</option>\n" +
				"<option value=\"typescript\"> TypeScript\n</option>\n" +
				"<option value=\"kaffeine\"> Kaffeine\n</option>\n" +
				"</optgroup>\n" +
				"<option value=\"dart\"> Dart\n</option>\n" +
				"</select>"
				;
			const string targetOutputE = targetOutputC;
			const string targetOutputF = targetOutputD;
			const string targetOutputG = "<select name=\"preprocessors\">" +
				"<optgroup label=\"Styles\">" +
				"<option value=\"sass\">Sass</option>" +
				"<option value=\"less\">LESS</option>" +
				"<option value=\"stylus\">Stylus</option>" +
				"</optgroup>" +
				"<optgroup label=\"Scripts\">" +
				"<option value=\"coffeescript\">CoffeeScript</option>" +
				"<option value=\"typescript\">TypeScript</option>" +
				"<option value=\"kaffeine\">Kaffeine</option>" +
				"</optgroup>" +
				"<option value=\"dart\">Dart</option>" +
				"</select>"
				;
			const string targetOutputH = "<select name=\"preprocessors\">\n" +
				"<optgroup label=\"Styles\">\n" +
				"<option value=\"sass\">Sass\n</option>\n" +
				"<option value=\"less\">LESS\n</option>\n" +
				"<option value=\"stylus\">Stylus\n</option>\n" +
				"</optgroup>\n" +
				"<optgroup label=\"Scripts\">\n" +
				"<option value=\"coffeescript\">CoffeeScript\n</option>\n" +
				"<option value=\"typescript\">TypeScript\n</option>\n" +
				"<option value=\"kaffeine\">Kaffeine\n</option>\n" +
				"</optgroup>\n" +
				"<option value=\"dart\">Dart\n</option>\n" +
				"</select>"
				;

			// Act
			string outputA = _keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = _keepingWhitespaceAndNewLinesMinifier.Minify(input).MinifiedContent;
			string outputC = _safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputE = _mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputF = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputG = _aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputH = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
			Assert.Equal(targetOutputE, outputE);
			Assert.Equal(targetOutputF, outputF);
			Assert.Equal(targetOutputG, outputG);
			Assert.Equal(targetOutputH, outputH);
		}

		[Fact]
		public void WhitespaceMinificationInMenuTagIsCorrect()
		{
			// Arrange
			const string input1 = "<menu>\n" +
				"	<menuitem label=\"New\" icon=\"icons/new.png\" onclick=\"new()\">\n" +
				"	</menuitem>\n" +
				"	<menuitem label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\">\n" +
				"	</menuitem>\n" +
				"	<menuitem label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\">\n" +
				"	</menuitem>\n" +
				"</menu>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = input1;
			const string targetOutput1C = "<menu>" +
				"<menuitem label=\"New\" icon=\"icons/new.png\" onclick=\"new()\"> </menuitem>" +
				"<menuitem label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\"> </menuitem>" +
				"<menuitem label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\"> </menuitem>" +
				"</menu>"
				;
			const string targetOutput1D = "<menu>\n" +
				"<menuitem label=\"New\" icon=\"icons/new.png\" onclick=\"new()\">\n" +
				"</menuitem>\n" +
				"<menuitem label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\">\n" +
				"</menuitem>\n" +
				"<menuitem label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\">\n" +
				"</menuitem>\n" +
				"</menu>"
				;
			const string targetOutput1E = "<menu>" +
				"<menuitem label=\"New\" icon=\"icons/new.png\" onclick=\"new()\"></menuitem>" +
				"<menuitem label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\"></menuitem>" +
				"<menuitem label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\"></menuitem>" +
				"</menu>"
				;
			const string targetOutput1F = targetOutput1D;
			const string targetOutput1G = targetOutput1E;
			const string targetOutput1H = targetOutput1D;

			const string input2 = "<menu>\n" +
				"	<command type=\"command\" label=\"New\" icon=\"icons/new.png\" onclick=\"new()\">\n" +
				"	</command>\n" +
				"	<command type=\"command\" label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\">\n" +
				"	</command>\n" +
				"	<command type=\"command\" label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\">\n" +
				"	</command>\n" +
				"</menu>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = input2;
			const string targetOutput2C = "<menu>" +
				"<command type=\"command\" label=\"New\" icon=\"icons/new.png\" onclick=\"new()\"> </command>" +
				"<command type=\"command\" label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\"> </command>" +
				"<command type=\"command\" label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\"> </command>" +
				"</menu>"
				;
			const string targetOutput2D = "<menu>\n" +
				"<command type=\"command\" label=\"New\" icon=\"icons/new.png\" onclick=\"new()\">\n" +
				"</command>\n" +
				"<command type=\"command\" label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\">\n" +
				"</command>\n" +
				"<command type=\"command\" label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\">\n" +
				"</command>\n" +
				"</menu>"
				;
			const string targetOutput2E = "<menu>" +
				"<command type=\"command\" label=\"New\" icon=\"icons/new.png\" onclick=\"new()\"></command>" +
				"<command type=\"command\" label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\"></command>" +
				"<command type=\"command\" label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\"></command>" +
				"</menu>"
				;
			const string targetOutput2F = targetOutput2D;
			const string targetOutput2G = targetOutput2E;
			const string targetOutput2H = targetOutput2D;

			// Act
			string output1A = _keepingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1B = _keepingWhitespaceAndNewLinesMinifier.Minify(input1).MinifiedContent;
			string output1C = _safeRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input1).MinifiedContent;
			string output1E = _mediumRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input1).MinifiedContent;
			string output1G = _aggressiveRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input1).MinifiedContent;

			string output2A = _keepingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2B = _keepingWhitespaceAndNewLinesMinifier.Minify(input2).MinifiedContent;
			string output2C = _safeRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input2).MinifiedContent;
			string output2E = _mediumRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input2).MinifiedContent;
			string output2G = _aggressiveRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);
			Assert.Equal(targetOutput1D, output1D);
			Assert.Equal(targetOutput1E, output1E);
			Assert.Equal(targetOutput1F, output1F);
			Assert.Equal(targetOutput1G, output1G);
			Assert.Equal(targetOutput1H, output1H);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);
			Assert.Equal(targetOutput2D, output2D);
			Assert.Equal(targetOutput2E, output2E);
			Assert.Equal(targetOutput2F, output2F);
			Assert.Equal(targetOutput2G, output2G);
			Assert.Equal(targetOutput2H, output2H);
		}

		[Fact]
		public void WhitespaceMinificationInVideoTagIsCorrect()
		{
			// Arrange
			const string input = "<video width=\"320\" height=\"240\" poster=\"video/poster.png\" controls=\"controls\">\n" +
				"	<source src=\"video/ie6.ogv\" type=\"video/ogg\">\n" +
				"	<source src=\"video/ie6.mp4\" type=\"video/mp4\">\n" +
				"	<object type=\"application/x-shockwave-flash\" data=\"player_flv_mini.swf\" " +
				"width=\"320\" height=\"240\">\n" +
				"		<param name=\"movie\" value=\"player_flv_mini.swf\">\n" +
				"		<param name=\"wmode\" value=\"opaque\">\n" +
				"		<param name=\"allowScriptAccess\" value=\"sameDomain\">\n" +
				"		<param name=\"quality\" value=\"high\">\n" +
				"		<param name=\"menu\" value=\"true\">\n" +
				"		<param name=\"autoplay\" value=\"false\">\n" +
				"		<param name=\"autoload\" value=\"false\">\n" +
				"		<param name=\"FlashVars\" value=\"flv=video/ie6.flv&amp;width=320&amp;height=240&amp;buffer=5\">\n" +
				"		<a href=\"video/ie6.flv\">Скачать видео-файл</a>\n" +
				"	</object>\n" +
				"</video>"
				;
			const string targetOutputA = input;
			const string targetOutputB = input;
			const string targetOutputC = "<video width=\"320\" height=\"240\" poster=\"video/poster.png\" controls=\"controls\">" +
				"<source src=\"video/ie6.ogv\" type=\"video/ogg\">" +
				"<source src=\"video/ie6.mp4\" type=\"video/mp4\">" +
				"<object type=\"application/x-shockwave-flash\" data=\"player_flv_mini.swf\" " +
				"width=\"320\" height=\"240\">" +
				"<param name=\"movie\" value=\"player_flv_mini.swf\">" +
				"<param name=\"wmode\" value=\"opaque\">" +
				"<param name=\"allowScriptAccess\" value=\"sameDomain\">" +
				"<param name=\"quality\" value=\"high\">" +
				"<param name=\"menu\" value=\"true\">" +
				"<param name=\"autoplay\" value=\"false\">" +
				"<param name=\"autoload\" value=\"false\">" +
				"<param name=\"FlashVars\" value=\"flv=video/ie6.flv&amp;width=320&amp;height=240&amp;buffer=5\">" +
				"<a href=\"video/ie6.flv\">Скачать видео-файл</a> " +
				"</object> " +
				"</video>"
				;
			const string targetOutputD = "<video width=\"320\" height=\"240\" poster=\"video/poster.png\" controls=\"controls\">\n" +
				"<source src=\"video/ie6.ogv\" type=\"video/ogg\">\n" +
				"<source src=\"video/ie6.mp4\" type=\"video/mp4\">\n" +
				"<object type=\"application/x-shockwave-flash\" data=\"player_flv_mini.swf\" " +
				"width=\"320\" height=\"240\">\n" +
				"<param name=\"movie\" value=\"player_flv_mini.swf\">\n" +
				"<param name=\"wmode\" value=\"opaque\">\n" +
				"<param name=\"allowScriptAccess\" value=\"sameDomain\">\n" +
				"<param name=\"quality\" value=\"high\">\n" +
				"<param name=\"menu\" value=\"true\">\n" +
				"<param name=\"autoplay\" value=\"false\">\n" +
				"<param name=\"autoload\" value=\"false\">\n" +
				"<param name=\"FlashVars\" value=\"flv=video/ie6.flv&amp;width=320&amp;height=240&amp;buffer=5\">\n" +
				"<a href=\"video/ie6.flv\">Скачать видео-файл</a>\n" +
				"</object>\n" +
				"</video>"
				;
			const string targetOutputE = targetOutputC;
			const string targetOutputF = targetOutputD;
			const string targetOutputG = "<video width=\"320\" height=\"240\" poster=\"video/poster.png\" controls=\"controls\">" +
				"<source src=\"video/ie6.ogv\" type=\"video/ogg\">" +
				"<source src=\"video/ie6.mp4\" type=\"video/mp4\">" +
				"<object type=\"application/x-shockwave-flash\" data=\"player_flv_mini.swf\" " +
				"width=\"320\" height=\"240\">" +
				"<param name=\"movie\" value=\"player_flv_mini.swf\">" +
				"<param name=\"wmode\" value=\"opaque\">" +
				"<param name=\"allowScriptAccess\" value=\"sameDomain\">" +
				"<param name=\"quality\" value=\"high\">" +
				"<param name=\"menu\" value=\"true\">" +
				"<param name=\"autoplay\" value=\"false\">" +
				"<param name=\"autoload\" value=\"false\">" +
				"<param name=\"FlashVars\" value=\"flv=video/ie6.flv&amp;width=320&amp;height=240&amp;buffer=5\">" +
				"<a href=\"video/ie6.flv\">Скачать видео-файл</a>" +
				"</object>" +
				"</video>"
				;
			const string targetOutputH = targetOutputD;

			// Act
			string outputA = _keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = _keepingWhitespaceAndNewLinesMinifier.Minify(input).MinifiedContent;
			string outputC = _safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputE = _mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputF = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputG = _aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputH = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
			Assert.Equal(targetOutputE, outputE);
			Assert.Equal(targetOutputF, outputF);
			Assert.Equal(targetOutputG, outputG);
			Assert.Equal(targetOutputH, outputH);
		}

		[Fact]
		public void WhitespaceMinificationInSvgTagIsCorrect()
		{
			// Arrange
			const string input1 = "<div>\n" +
				"	<svg width=\"150\" height=\"100\" viewBox=\"0 0 3 2\">\n" +
				"		<rect width=\"1\" height=\"2\" x=\"0\" fill=\"#008d46\" />\n" +
				"		<rect width=\"1\" height=\"2\" x=\"1\" fill=\"#ffffff\" />\n" +
				"		<rect width=\"1\" height=\"2\" x=\"2\" fill=\"#d2232c\" />\n" +
				"	</svg>\n" +
				"</div>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = input1;
			const string targetOutput1C = "<div> " +
				"<svg width=\"150\" height=\"100\" viewBox=\"0 0 3 2\">" +
				"<rect width=\"1\" height=\"2\" x=\"0\" fill=\"#008d46\" />" +
				"<rect width=\"1\" height=\"2\" x=\"1\" fill=\"#ffffff\" />" +
				"<rect width=\"1\" height=\"2\" x=\"2\" fill=\"#d2232c\" />" +
				"</svg> " +
				"</div>"
				;
			const string targetOutput1D = "<div>\n" +
				"<svg width=\"150\" height=\"100\" viewBox=\"0 0 3 2\">\n" +
				"<rect width=\"1\" height=\"2\" x=\"0\" fill=\"#008d46\" />\n" +
				"<rect width=\"1\" height=\"2\" x=\"1\" fill=\"#ffffff\" />\n" +
				"<rect width=\"1\" height=\"2\" x=\"2\" fill=\"#d2232c\" />\n" +
				"</svg>\n" +
				"</div>"
				;
			const string targetOutput1E = "<div>" +
				"<svg width=\"150\" height=\"100\" viewBox=\"0 0 3 2\">" +
				"<rect width=\"1\" height=\"2\" x=\"0\" fill=\"#008d46\" />" +
				"<rect width=\"1\" height=\"2\" x=\"1\" fill=\"#ffffff\" />" +
				"<rect width=\"1\" height=\"2\" x=\"2\" fill=\"#d2232c\" />" +
				"</svg>" +
				"</div>"
				;
			const string targetOutput1F = targetOutput1D;
			const string targetOutput1G = targetOutput1E;
			const string targetOutput1H = targetOutput1D;

			const string input2 = "<div>\n" +
				"	<svg>\n" +
				"		<text x=\"20\" y=\"40\" " +
				"transform=\"rotate(30, 20,40)\" " +
				"style=\"stroke: none; fill: #000000\">\n" +
				"\t\t  Rotated  SVG  text  \t\n" +
				"		</text>\n" +
				"	</svg>\n" +
				"</div>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = input2;
			const string targetOutput2C = "<div> " +
				"<svg>" +
				"<text x=\"20\" y=\"40\" " +
				"transform=\"rotate(30, 20,40)\" " +
				"style=\"stroke: none; fill: #000000\">\n" +
				"\t\t  Rotated  SVG  text  \t\n" +
				"		</text>" +
				"</svg> " +
				"</div>"
				;
			const string targetOutput2D = "<div>\n" +
				"<svg>\n" +
				"<text x=\"20\" y=\"40\" " +
				"transform=\"rotate(30, 20,40)\" " +
				"style=\"stroke: none; fill: #000000\">\n" +
				"\t\t  Rotated  SVG  text  \t\n" +
				"		</text>\n" +
				"</svg>\n" +
				"</div>"
				;
			const string targetOutput2E = "<div>" +
				"<svg>" +
				"<text x=\"20\" y=\"40\" " +
				"transform=\"rotate(30, 20,40)\" " +
				"style=\"stroke: none; fill: #000000\">\n" +
				"\t\t  Rotated  SVG  text  \t\n" +
				"		</text>" +
				"</svg>" +
				"</div>"
				;
			const string targetOutput2F = targetOutput2D;
			const string targetOutput2G = targetOutput2E;
			const string targetOutput2H = targetOutput2D;

			const string input3 = "<svg width=\"100\" height=\"100\">\n\r\n\r" +
				"  <path d=\"M 10 10 H 90 V 90 H 10 L 10 10\" />\n\r\n\r" +
				"  <!-- Points -->\n\r" +
				"  <circle cx=\"10\" cy=\"10\" r=\"2\" fill=\"red\" />\n\r" +
				"  <circle cx=\"90\" cy=\"90\" r=\"2\" fill=\"red\" />\n\r" +
				"  <circle cx=\"90\" cy=\"10\" r=\"2\" fill=\"red\" />\n\r" +
				"  <circle cx=\"10\" cy=\"90\" r=\"2\" fill=\"red\" />\n\r" +
				"</svg>"
				;
			const string targetOutput3A = input3;
			const string targetOutput3B = input3;
			const string targetOutput3C = "<svg width=\"100\" height=\"100\">" +
				"<path d=\"M 10 10 H 90 V 90 H 10 L 10 10\" />" +
				"<!-- Points -->" +
				"<circle cx=\"10\" cy=\"10\" r=\"2\" fill=\"red\" />" +
				"<circle cx=\"90\" cy=\"90\" r=\"2\" fill=\"red\" />" +
				"<circle cx=\"90\" cy=\"10\" r=\"2\" fill=\"red\" />" +
				"<circle cx=\"10\" cy=\"90\" r=\"2\" fill=\"red\" />" +
				"</svg>"
				;
			const string targetOutput3D = "<svg width=\"100\" height=\"100\">\n\r" +
				"<path d=\"M 10 10 H 90 V 90 H 10 L 10 10\" />\n\r" +
				"<!-- Points -->\n\r" +
				"<circle cx=\"10\" cy=\"10\" r=\"2\" fill=\"red\" />\n\r" +
				"<circle cx=\"90\" cy=\"90\" r=\"2\" fill=\"red\" />\n\r" +
				"<circle cx=\"90\" cy=\"10\" r=\"2\" fill=\"red\" />\n\r" +
				"<circle cx=\"10\" cy=\"90\" r=\"2\" fill=\"red\" />\n\r" +
				"</svg>"
				;
			const string targetOutput3E = targetOutput3C;
			const string targetOutput3F = targetOutput3D;
			const string targetOutput3G = targetOutput3C;
			const string targetOutput3H = targetOutput3D;

			const string input4 = "<svg width=\"100%\" height=\"100%\" version=\"1.1\">\r" +
				"  <defs>\r" +
				"    <font id=\"Font2\" horiz-adv-x=\"1000\">\r" +
				"      <font-face font-family=\"Super Sans\" font-weight=\"normal\" font-style=\"italic\" " +
				"units-per-em=\"1000\" cap-height=\"600\" x-height=\"400\" " +
				"ascent=\"700\" descent=\"300\" " +
				"alphabetic=\"0\" mathematical=\"350\" ideographic=\"400\" hanging=\"500\">\r" +
				"        <font-face-src>\r" +
				"          <font-face-name name=\"Super Sans Italic\" />\r" +
				"        </font-face-src>\r" +
				"      </font-face>\r" +
				"      <missing-glyph>\r" +
				"        <path d=\"M0,0h200v200h-200z\" />\r" +
				"      </missing-glyph>\r" +
				"      <glyph unicode=\"!\" horiz-adv-x=\"300\">\r" +
				"        <!-- Outline of exclam. pt. glyph -->\r" +
				"      </glyph>\r" +
				"      <glyph unicode=\"@\">\r" +
				"        <!-- Outline of @ glyph -->\r" +
				"      </glyph>\r" +
				"      <!-- more glyphs -->\r" +
				"    </font>\r" +
				"  </defs>\r" +
				"</svg>"
				;
			const string targetOutput4A = input4;
			const string targetOutput4B = input4;
			const string targetOutput4C = "<svg width=\"100%\" height=\"100%\" version=\"1.1\">" +
				"<defs>" +
				"<font id=\"Font2\" horiz-adv-x=\"1000\">" +
				"<font-face font-family=\"Super Sans\" font-weight=\"normal\" font-style=\"italic\" " +
				"units-per-em=\"1000\" cap-height=\"600\" x-height=\"400\" " +
				"ascent=\"700\" descent=\"300\" " +
				"alphabetic=\"0\" mathematical=\"350\" ideographic=\"400\" hanging=\"500\">" +
				"<font-face-src>" +
				"<font-face-name name=\"Super Sans Italic\" />" +
				"</font-face-src>" +
				"</font-face>" +
				"<missing-glyph>" +
				"<path d=\"M0,0h200v200h-200z\" />" +
				"</missing-glyph>" +
				"<glyph unicode=\"!\" horiz-adv-x=\"300\">\r" +
				"        <!-- Outline of exclam. pt. glyph -->\r" +
				"      </glyph>" +
				"<glyph unicode=\"@\">\r" +
				"        <!-- Outline of @ glyph -->\r" +
				"      </glyph>" +
				"<!-- more glyphs -->\r" +
				"    </font>" +
				"</defs>" +
				"</svg>"
				;
			const string targetOutput4D = "<svg width=\"100%\" height=\"100%\" version=\"1.1\">\r" +
				"<defs>\r" +
				"<font id=\"Font2\" horiz-adv-x=\"1000\">\r" +
				"<font-face font-family=\"Super Sans\" font-weight=\"normal\" font-style=\"italic\" " +
				"units-per-em=\"1000\" cap-height=\"600\" x-height=\"400\" " +
				"ascent=\"700\" descent=\"300\" " +
				"alphabetic=\"0\" mathematical=\"350\" ideographic=\"400\" hanging=\"500\">\r" +
				"<font-face-src>\r" +
				"<font-face-name name=\"Super Sans Italic\" />\r" +
				"</font-face-src>\r" +
				"</font-face>\r" +
				"<missing-glyph>\r" +
				"<path d=\"M0,0h200v200h-200z\" />\r" +
				"</missing-glyph>\r" +
				"<glyph unicode=\"!\" horiz-adv-x=\"300\">\r" +
				"        <!-- Outline of exclam. pt. glyph -->\r" +
				"      </glyph>\r" +
				"<glyph unicode=\"@\">\r" +
				"        <!-- Outline of @ glyph -->\r" +
				"      </glyph>\r" +
				"<!-- more glyphs -->\r" +
				"    </font>\r" +
				"</defs>\r" +
				"</svg>"
				;
			const string targetOutput4E = targetOutput4C;
			const string targetOutput4F = targetOutput4D;
			const string targetOutput4G = targetOutput4C;
			const string targetOutput4H = targetOutput4D;

			const string input5 = "<svg width=\"300\" height=\"50\" viewBox=\"0 0 300 50\">\r\n" +
				"	<rect x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" fill=\"#f6f8fa\" />\r\n" +
				"	<text x=\"50%\" y=\"50%\" fill=\"#24292e\" font-size=\"16\" text-anchor=\"middle\">\r\n" +
				"	<![CDATA[\r\n" +
				"	Vasya Pupkin <vasya-pupkin@mail.ru>\r\n" +
				"	]]>\r\n" +
				"	</text>\r\n" +
				"</svg>"
				;
			const string targetOutput5A = input5;
			const string targetOutput5B = input5;
			const string targetOutput5C = "<svg width=\"300\" height=\"50\" viewBox=\"0 0 300 50\">" +
				"<rect x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" fill=\"#f6f8fa\" />" +
				"<text x=\"50%\" y=\"50%\" fill=\"#24292e\" font-size=\"16\" text-anchor=\"middle\">\r\n" +
				"	<![CDATA[\r\n" +
				"	Vasya Pupkin <vasya-pupkin@mail.ru>\r\n" +
				"	]]>\r\n" +
				"	</text>" +
				"</svg>"
				;
			const string targetOutput5D = "<svg width=\"300\" height=\"50\" viewBox=\"0 0 300 50\">\r\n" +
				"<rect x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" fill=\"#f6f8fa\" />\r\n" +
				"<text x=\"50%\" y=\"50%\" fill=\"#24292e\" font-size=\"16\" text-anchor=\"middle\">\r\n" +
				"	<![CDATA[\r\n" +
				"	Vasya Pupkin <vasya-pupkin@mail.ru>\r\n" +
				"	]]>\r\n" +
				"	</text>\r\n" +
				"</svg>"
				;
			const string targetOutput5E = targetOutput5C;
			const string targetOutput5F = targetOutput5D;
			const string targetOutput5G = targetOutput5E;
			const string targetOutput5H = targetOutput5D;

			// Act
			string output1A = _keepingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1B = _keepingWhitespaceAndNewLinesMinifier.Minify(input1).MinifiedContent;
			string output1C = _safeRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input1).MinifiedContent;
			string output1E = _mediumRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input1).MinifiedContent;
			string output1G = _aggressiveRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input1).MinifiedContent;

			string output2A = _keepingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2B = _keepingWhitespaceAndNewLinesMinifier.Minify(input2).MinifiedContent;
			string output2C = _safeRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input2).MinifiedContent;
			string output2E = _mediumRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input2).MinifiedContent;
			string output2G = _aggressiveRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input2).MinifiedContent;

			string output3A = _keepingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3B = _keepingWhitespaceAndNewLinesMinifier.Minify(input3).MinifiedContent;
			string output3C = _safeRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input3).MinifiedContent;
			string output3E = _mediumRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input3).MinifiedContent;
			string output3G = _aggressiveRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input3).MinifiedContent;

			string output4A = _keepingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4B = _keepingWhitespaceAndNewLinesMinifier.Minify(input4).MinifiedContent;
			string output4C = _safeRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input4).MinifiedContent;
			string output4E = _mediumRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input4).MinifiedContent;
			string output4G = _aggressiveRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input4).MinifiedContent;

			string output5A = _keepingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5B = _keepingWhitespaceAndNewLinesMinifier.Minify(input5).MinifiedContent;
			string output5C = _safeRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input5).MinifiedContent;
			string output5E = _mediumRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input5).MinifiedContent;
			string output5G = _aggressiveRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input5).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);
			Assert.Equal(targetOutput1D, output1D);
			Assert.Equal(targetOutput1E, output1E);
			Assert.Equal(targetOutput1F, output1F);
			Assert.Equal(targetOutput1G, output1G);
			Assert.Equal(targetOutput1H, output1H);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);
			Assert.Equal(targetOutput2D, output2D);
			Assert.Equal(targetOutput2E, output2E);
			Assert.Equal(targetOutput2F, output2F);
			Assert.Equal(targetOutput2G, output2G);
			Assert.Equal(targetOutput2H, output2H);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);
			Assert.Equal(targetOutput3D, output3D);
			Assert.Equal(targetOutput3E, output3E);
			Assert.Equal(targetOutput3F, output3F);
			Assert.Equal(targetOutput3G, output3G);
			Assert.Equal(targetOutput3H, output3H);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);
			Assert.Equal(targetOutput4D, output4D);
			Assert.Equal(targetOutput4E, output4E);
			Assert.Equal(targetOutput4F, output4F);
			Assert.Equal(targetOutput4G, output4G);
			Assert.Equal(targetOutput4H, output4H);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);
			Assert.Equal(targetOutput5D, output5D);
			Assert.Equal(targetOutput5E, output5E);
			Assert.Equal(targetOutput5F, output5F);
			Assert.Equal(targetOutput5G, output5G);
			Assert.Equal(targetOutput5H, output5H);
		}

		[Fact]
		public void WhitespaceMinificationInMathTagIsCorrect()
		{
			// Arrange
			const string input = "<div>\n" +
				"	<math>\n" +
				"		<mrow>\n" +
				"			<mrow>\n" +
				"				<msup>\n" +
				"					<mi>a</mi>\n" +
				"					<mn>2</mn>\n" +
				"				</msup>\n" +
				"				<mo>+</mo>\n" +
				"				<msup>\n" +
				"					<mi>b</mi>\n" +
				"					<mn>2</mn>\n" +
				"				</msup>\n" +
				"			</mrow>\n" +
				"			<mo>=</mo>\n" +
				"			<msup>\n" +
				"				<mi>c</mi>\n" +
				"				<mn>2</mn>\n" +
				"			</msup>\n" +
				"		</mrow>\n" +
				"	</math>\n" +
				"</div>"
				;
			const string targetOutputA = input;
			const string targetOutputB = input;
			const string targetOutputC = "<div> " +
				"<math>" +
				"<mrow>" +
				"<mrow>" +
				"<msup>" +
				"<mi>a</mi>" +
				"<mn>2</mn>" +
				"</msup>" +
				"<mo>+</mo>" +
				"<msup>" +
				"<mi>b</mi>" +
				"<mn>2</mn>" +
				"</msup>" +
				"</mrow>" +
				"<mo>=</mo>" +
				"<msup>" +
				"<mi>c</mi>" +
				"<mn>2</mn>" +
				"</msup>" +
				"</mrow>" +
				"</math> " +
				"</div>"
				;
			const string targetOutputD = "<div>\n" +
				"<math>\n" +
				"<mrow>\n" +
				"<mrow>\n" +
				"<msup>\n" +
				"<mi>a</mi>\n" +
				"<mn>2</mn>\n" +
				"</msup>\n" +
				"<mo>+</mo>\n" +
				"<msup>\n" +
				"<mi>b</mi>\n" +
				"<mn>2</mn>\n" +
				"</msup>\n" +
				"</mrow>\n" +
				"<mo>=</mo>\n" +
				"<msup>\n" +
				"<mi>c</mi>\n" +
				"<mn>2</mn>\n" +
				"</msup>\n" +
				"</mrow>\n" +
				"</math>\n" +
				"</div>"
				;
			const string targetOutputE = "<div>" +
				"<math>" +
				"<mrow>" +
				"<mrow>" +
				"<msup>" +
				"<mi>a</mi>" +
				"<mn>2</mn>" +
				"</msup>" +
				"<mo>+</mo>" +
				"<msup>" +
				"<mi>b</mi>" +
				"<mn>2</mn>" +
				"</msup>" +
				"</mrow>" +
				"<mo>=</mo>" +
				"<msup>" +
				"<mi>c</mi>" +
				"<mn>2</mn>" +
				"</msup>" +
				"</mrow>" +
				"</math>" +
				"</div>"
				;
			const string targetOutputF = targetOutputD;
			const string targetOutputG = targetOutputE;
			const string targetOutputH = targetOutputD;

			// Act
			string outputA = _keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = _keepingWhitespaceAndNewLinesMinifier.Minify(input).MinifiedContent;
			string outputC = _safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputE = _mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputF = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputG = _aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputH = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
			Assert.Equal(targetOutputE, outputE);
			Assert.Equal(targetOutputF, outputF);
			Assert.Equal(targetOutputG, outputG);
			Assert.Equal(targetOutputH, outputH);
		}

		[Fact]
		public void WhitespaceMinificationInHtmlFragmentsWithTypographicalTagsIsCorrect()
		{
			// Arrange
			const string input1 = "<p>	 one  </p>    \n" +
				"<p>  two	 </p>\n\n    \n\t\t  " +
				"<div title=\"Some title...\">  three	 </div>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = input1;
			const string targetOutput1C = "<p> one </p> " +
				"<p> two </p> " +
				"<div title=\"Some title...\"> three </div>"
				;
			const string targetOutput1D = "<p> one </p>\n" +
				"<p> two </p>\n" +
				"<div title=\"Some title...\"> three </div>"
				;
			const string targetOutput1E = "<p>one</p>" +
				"<p>two</p>" +
				"<div title=\"Some title...\">three</div>"
				;
			const string targetOutput1F = "<p>one</p>\n" +
				"<p>two</p>\n" +
				"<div title=\"Some title...\">three</div>"
				;
			const string targetOutput1G = targetOutput1E;
			const string targetOutput1H = targetOutput1F;

			const string input2 = "<span>Some text...</span> \n\t  " +
				"<pre title=\"Some title...\">   Some     text </pre> \t\n  " +
				"<span>Some text...</span>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = input2;
			const string targetOutput2C = "<span>Some text...</span> " +
				"<pre title=\"Some title...\">   Some     text </pre> " +
				"<span>Some text...</span>"
				;
			const string targetOutput2D = "<span>Some text...</span>\n" +
				"<pre title=\"Some title...\">   Some     text </pre>\n" +
				"<span>Some text...</span>"
				;
			const string targetOutput2E = "<span>Some text...</span>" +
				"<pre title=\"Some title...\">   Some     text </pre>" +
				"<span>Some text...</span>"
				;
			const string targetOutput2F = targetOutput2D;
			const string targetOutput2G = targetOutput2E;
			const string targetOutput2H = targetOutput2D;

			const string input3 = "<p>Some text...</p> \n\t  " +
				"<pre title=\"Some title...\">	<code>   Some     text </code>\n</pre> \t\n  " +
				"<p>Some text...</p>"
				;
			const string targetOutput3A = input3;
			const string targetOutput3B = input3;
			const string targetOutput3C = "<p>Some text...</p> " +
				"<pre title=\"Some title...\">	<code>   Some     text </code>\n</pre> " +
				"<p>Some text...</p>"
				;
			const string targetOutput3D = "<p>Some text...</p>\n" +
				"<pre title=\"Some title...\">	<code>   Some     text </code>\n</pre>\n" +
				"<p>Some text...</p>"
				;
			const string targetOutput3E = "<p>Some text...</p>" +
				"<pre title=\"Some title...\">	<code>   Some     text </code>\n</pre>" +
				"<p>Some text...</p>"
				;
			const string targetOutput3F = targetOutput3D;
			const string targetOutput3G = targetOutput3E;
			const string targetOutput3H = targetOutput3D;

			const string input4 = "<p> I'll   tell  you   my  story  with    <span>  5   slides </span> -  " +
				"<img src=\"\"> <span>	!  </span>	</p>";
			const string targetOutput4A = input4;
			const string targetOutput4B = input4;
			const string targetOutput4C = "<p> I'll tell you my story with <span> 5 slides </span> - " +
				"<img src=\"\"> <span> ! </span> </p>";
			const string targetOutput4D = targetOutput4C;
			const string targetOutput4E = "<p>I'll tell you my story with <span> 5 slides </span> - " +
				"<img src=\"\"> <span> ! </span></p>";
			const string targetOutput4F = targetOutput4E;
			const string targetOutput4G = "<p>I'll tell you my story with <span>5 slides</span> - " +
				"<img src=\"\"> <span>!</span></p>";
			const string targetOutput4H = targetOutput4G;

			const string input5 = "<p>  An  <del>	 old \n</del>  <ins>	new  \n </ins>  embedded flash animation:  \n " +
				"<embed src=\"helloworld.swf\">	 !  </p>";
			const string targetOutput5A = input5;
			const string targetOutput5B = input5;
			const string targetOutput5C = "<p> An <del> old </del> <ins> new </ins> embedded flash animation: " +
				"<embed src=\"helloworld.swf\"> ! </p>";
			const string targetOutput5D = "<p> An <del> old\n</del> <ins> new\n</ins> embedded flash animation:\n" +
				"<embed src=\"helloworld.swf\"> ! </p>";
			const string targetOutput5E = "<p>An <del> old </del> <ins> new </ins> embedded flash animation: " +
				"<embed src=\"helloworld.swf\"> !</p>";
			const string targetOutput5F = "<p>An <del> old\n</del> <ins> new\n</ins> embedded flash animation:\n" +
				"<embed src=\"helloworld.swf\"> !</p>";
			const string targetOutput5G = "<p>An <del>old</del> <ins>new</ins> embedded flash animation: " +
				"<embed src=\"helloworld.swf\"> !</p>";
			const string targetOutput5H = "<p>An <del>old\n</del> <ins>new\n</ins> embedded flash animation:\n" +
				"<embed src=\"helloworld.swf\"> !</p>";

			// Act
			string output1A = _keepingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1B = _keepingWhitespaceAndNewLinesMinifier.Minify(input1).MinifiedContent;
			string output1C = _safeRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input1).MinifiedContent;
			string output1E = _mediumRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input1).MinifiedContent;
			string output1G = _aggressiveRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input1).MinifiedContent;

			string output2A = _keepingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2B = _keepingWhitespaceAndNewLinesMinifier.Minify(input2).MinifiedContent;
			string output2C = _safeRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input2).MinifiedContent;
			string output2E = _mediumRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input2).MinifiedContent;
			string output2G = _aggressiveRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input2).MinifiedContent;

			string output3A = _keepingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3B = _keepingWhitespaceAndNewLinesMinifier.Minify(input3).MinifiedContent;
			string output3C = _safeRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input3).MinifiedContent;
			string output3E = _mediumRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input3).MinifiedContent;
			string output3G = _aggressiveRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input3).MinifiedContent;

			string output4A = _keepingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4B = _keepingWhitespaceAndNewLinesMinifier.Minify(input4).MinifiedContent;
			string output4C = _safeRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input4).MinifiedContent;
			string output4E = _mediumRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input4).MinifiedContent;
			string output4G = _aggressiveRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input4).MinifiedContent;

			string output5A = _keepingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5B = _keepingWhitespaceAndNewLinesMinifier.Minify(input5).MinifiedContent;
			string output5C = _safeRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5D = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input5).MinifiedContent;
			string output5E = _mediumRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5F = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input5).MinifiedContent;
			string output5G = _aggressiveRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5H = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input5).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);
			Assert.Equal(targetOutput1D, output1D);
			Assert.Equal(targetOutput1E, output1E);
			Assert.Equal(targetOutput1F, output1F);
			Assert.Equal(targetOutput1G, output1G);
			Assert.Equal(targetOutput1H, output1H);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);
			Assert.Equal(targetOutput2D, output2D);
			Assert.Equal(targetOutput2E, output2E);
			Assert.Equal(targetOutput2F, output2F);
			Assert.Equal(targetOutput2G, output2G);
			Assert.Equal(targetOutput2H, output2H);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);
			Assert.Equal(targetOutput3D, output3D);
			Assert.Equal(targetOutput3E, output3E);
			Assert.Equal(targetOutput3F, output3F);
			Assert.Equal(targetOutput3G, output3G);
			Assert.Equal(targetOutput3H, output3H);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);
			Assert.Equal(targetOutput4D, output4D);
			Assert.Equal(targetOutput4E, output4E);
			Assert.Equal(targetOutput4F, output4F);
			Assert.Equal(targetOutput4G, output4G);
			Assert.Equal(targetOutput4H, output4H);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);
			Assert.Equal(targetOutput5D, output5D);
			Assert.Equal(targetOutput5E, output5E);
			Assert.Equal(targetOutput5F, output5F);
			Assert.Equal(targetOutput5G, output5G);
			Assert.Equal(targetOutput5H, output5H);
		}

		[Fact]
		public void WhitespaceMinificationInHtmlDocumentIsCorrect()
		{
			// Arrange
			const string input = " \n \n\t <!-- meta name=\"GENERATOR\" content=\"Microsoft FrontPage 1.0\" --> \n  \n\t\n" +
				"<!DOCTYPE html>\n" +
				"<html>\n" +
				"	<head>\n" +
				"		<meta charset=\"utf-8\">\n" +
				"		<title> \t  Some  title...  \t  </title>\n" +
				"		<base href=\"http://www.example.com/\" target=\"_blank\">\n" +
				"		<link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\">\n" +
				"		<meta name=\"viewport\" content=\"width=device-width\">\n" +
				"		<link href=\"/Bundles/CommonStyles\" rel=\"stylesheet\">\n" +
				"		<style type=\"text/css\">\n" +
				"			.ie table.min-width-content {\n" +
				"				table-layout: auto !important;\n" +
				"			}\n" +
				"		</style>\n" +
				"		<script src=\"/Bundles/Modernizr\"></script>\n" +
				"	</head>\n" +
				"	<body>\n" +
				"		<p>Some text...</p>\n\n" +
				"		<script src=\"http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js\"></script>\n" +
				"		<script>\n" +
				"			(window.jquery) || document.write('<script src=\"/Bundles/Jquery\"><\\/script>');  \n" +
				"</script>\n" +
				"	    <script src=\"/Bundles/CommonScripts\"></script>\n" +
				"	</body>\n" +
				"</html>\n\n\t \n " +
				"<!-- MEOW -->\t \n\n  \n "
				;
			const string targetOutputA = input;
			const string targetOutputB = input;
			const string targetOutputC = "<!-- meta name=\"GENERATOR\" content=\"Microsoft FrontPage 1.0\" -->" +
				"<!DOCTYPE html>" +
				"<html>" +
				"<head>" +
				"<meta charset=\"utf-8\">" +
				"<title>Some title...</title>" +
				"<base href=\"http://www.example.com/\" target=\"_blank\">" +
				"<link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\">" +
				"<meta name=\"viewport\" content=\"width=device-width\">" +
				"<link href=\"/Bundles/CommonStyles\" rel=\"stylesheet\">" +
				"<style type=\"text/css\">" +
				".ie table.min-width-content {\n" +
				"				table-layout: auto !important;\n" +
				"			}" +
				"</style>" +
				"<script src=\"/Bundles/Modernizr\"></script>" +
				"</head>" +
				"<body>" +
				"<p>Some text...</p>" +
				"<script src=\"http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js\"></script>" +
				"<script>(window.jquery) || document.write('<script src=\"/Bundles/Jquery\"><\\/script>');</script>" +
				"<script src=\"/Bundles/CommonScripts\"></script>" +
				"</body>" +
				"</html>" +
				"<!-- MEOW -->"
				;
			const string targetOutputD = "<!-- meta name=\"GENERATOR\" content=\"Microsoft FrontPage 1.0\" -->\n" +
				"<!DOCTYPE html>\n" +
				"<html>\n" +
				"<head>\n" +
				"<meta charset=\"utf-8\">\n" +
				"<title>Some title...</title>\n" +
				"<base href=\"http://www.example.com/\" target=\"_blank\">\n" +
				"<link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\">\n" +
				"<meta name=\"viewport\" content=\"width=device-width\">\n" +
				"<link href=\"/Bundles/CommonStyles\" rel=\"stylesheet\">\n" +
				"<style type=\"text/css\">\n" +
				".ie table.min-width-content {\n" +
				"				table-layout: auto !important;\n" +
				"			}\n" +
				"</style>\n" +
				"<script src=\"/Bundles/Modernizr\"></script>\n" +
				"</head>\n" +
				"<body>\n" +
				"<p>Some text...</p>\n" +
				"<script src=\"http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js\"></script>\n" +
				"<script>\n" +
				"(window.jquery) || document.write('<script src=\"/Bundles/Jquery\"><\\/script>');\n" +
				"</script>\n" +
				"<script src=\"/Bundles/CommonScripts\"></script>\n" +
				"</body>\n" +
				"</html>\n" +
				"<!-- MEOW -->"
				;
			const string targetOutputE = targetOutputC;
			const string targetOutputF = targetOutputD;
			const string targetOutputG = targetOutputC;
			const string targetOutputH = targetOutputD;

			// Act
			string outputA = _keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = _keepingWhitespaceAndNewLinesMinifier.Minify(input).MinifiedContent;
			string outputC = _safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = _safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputE = _mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputF = _mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputG = _aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputH = _aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
			Assert.Equal(targetOutputE, outputE);
			Assert.Equal(targetOutputF, outputF);
			Assert.Equal(targetOutputG, outputG);
			Assert.Equal(targetOutputH, outputH);
		}

		[Fact]
		public void WhitespaceMinificationWithRemovingOptionalEndTagsInHtmlDocumentIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.None,
					PreserveNewLines = false,
					RemoveOptionalEndTags = true
				});
			var keepingWhitespaceAndNewLinesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.None,
					PreserveNewLines = true,
					RemoveOptionalEndTags = true
				});
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Safe,
					PreserveNewLines = false,
					RemoveOptionalEndTags = true
				});
			var safeRemovingWhitespaceExceptForNewLinesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Safe,
					PreserveNewLines = true,
					RemoveOptionalEndTags = true
				});
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Medium,
					PreserveNewLines = false,
					RemoveOptionalEndTags = true
				});
			var mediumRemovingWhitespaceExceptForNewLinesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Medium,
					PreserveNewLines = true,
					RemoveOptionalEndTags = true
				});
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive,
					PreserveNewLines = false,
					RemoveOptionalEndTags = true
				});
			var aggressiveRemovingWhitespaceExceptForNewLinesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive,
					PreserveNewLines = true,
					RemoveOptionalEndTags = true
				});

			const string input = "<!DOCTYPE html>\r" +
				"<html lang=\"en\">\r" +
				"    <head>\r" +
				"        <title>London | Largest cities in the world</title>\r" +
				"        <meta charset=\"utf-8\">\r" +
				"        <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r" +
				"        <link href=\"/css/style.css\" rel=\"stylesheet\">\r" +
				"    </head>\r" +
				"    <body>\r\r" +
				"        <header>\r" +
				"            <div class=\"support-old-browsers\">\r" +
				"                <p>\r" +
				"                    We stop supporting Internet Explorer. <br>Other browsers are more stable.\r" +
				"                </p>\r" +
				"            </div>\r" +
				"            <div class=\"header\">\r" +
				"                <h1 class=\"logo\">\r" +
				"                    <a href=\"/\">Cities</a>\r" +
				"                </h1>\r" +
				"                <nav>\r" +
				"                    <ul>\r" +
				"                        <li><a href=\"/tokyo\">Tokyo</a></li>\r" +
				"                        <li><a href=\"/moscow\">Moscow</a></li>\r" +
				"                        <li><a href=\"/london\">London</a></li>\r" +
				"                    </ul>\r" +
				"                </nav>\r" +
				"            </div>\r" +
				"        </header>\r\r" +
				"        <main>\r" +
				"            <h2>London</h2>\r\r" +
				"            <p>London is the capital of  Great Britain, its political, economic and commercial\r" +
				"            centre. It is one of the largest cities in the world and the largest city in Europe.\r" +
				"            Its population is about 8 million.</p>\r" +
				"            <p>London is one of the oldest and most interesting cities in the world.</p>\r" +
				"            <p>Traditionally, it is divided into several parts: the City, Westminster, \r" +
				"            the West End and the East End. They are very different from each other and seem to\r" +
				"            belong to different towns and epochs.</p>\r" +
				"        </main>\r" +
				"        <aside>\r" +
				"            <a href=\"/banner.do?id=160803\" target=\"_blank\">" +
				"<img src=\"/images/banner-160803.png\" width=\"300\" height=\"600\" alt=\"Rent a coworking in London\">" +
				"</a>" +
				"        </aside>\r\r" +
				"        <footer>\r" +
				"            <p>&copy; 2022 All rights reserved</p>\r" +
				"        </footer>\r" +
				"    </body>\r" +
				"</html>"
				;
			const string targetOutputA = "<!DOCTYPE html>\r" +
				"<html lang=\"en\">\r" +
				"    <head>\r" +
				"        <title>London | Largest cities in the world</title>\r" +
				"        <meta charset=\"utf-8\">\r" +
				"        <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r" +
				"        <link href=\"/css/style.css\" rel=\"stylesheet\">\r" +
				"    \r" +
				"    <body>\r\r" +
				"        <header>\r" +
				"            <div class=\"support-old-browsers\">\r" +
				"                <p>\r" +
				"                    We stop supporting Internet Explorer. <br>Other browsers are more stable.\r" +
				"                \r" +
				"            </div>\r" +
				"            <div class=\"header\">\r" +
				"                <h1 class=\"logo\">\r" +
				"                    <a href=\"/\">Cities</a>\r" +
				"                </h1>\r" +
				"                <nav>\r" +
				"                    <ul>\r" +
				"                        <li><a href=\"/tokyo\">Tokyo</a>\r" +
				"                        <li><a href=\"/moscow\">Moscow</a>\r" +
				"                        <li><a href=\"/london\">London</a>\r" +
				"                    </ul>\r" +
				"                </nav>\r" +
				"            </div>\r" +
				"        </header>\r\r" +
				"        <main>\r" +
				"            <h2>London</h2>\r\r" +
				"            <p>London is the capital of  Great Britain, its political, economic and commercial\r" +
				"            centre. It is one of the largest cities in the world and the largest city in Europe.\r" +
				"            Its population is about 8 million.\r" +
				"            <p>London is one of the oldest and most interesting cities in the world.\r" +
				"            <p>Traditionally, it is divided into several parts: the City, Westminster, \r" +
				"            the West End and the East End. They are very different from each other and seem to\r" +
				"            belong to different towns and epochs.\r" +
				"        </main>\r" +
				"        <aside>\r" +
				"            <a href=\"/banner.do?id=160803\" target=\"_blank\">" +
				"<img src=\"/images/banner-160803.png\" width=\"300\" height=\"600\" alt=\"Rent a coworking in London\">" +
				"</a>" +
				"        </aside>\r\r" +
				"        <footer>\r" +
				"            <p>&copy; 2022 All rights reserved\r" +
				"        </footer>\r" +
				"    \r"
				;
			const string targetOutputB = targetOutputA;
			const string targetOutputC = "<!DOCTYPE html>" +
				"<html lang=\"en\">" +
				"<head>" +
				"<title>London | Largest cities in the world</title>" +
				"<meta charset=\"utf-8\">" +
				"<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">" +
				"<link href=\"/css/style.css\" rel=\"stylesheet\">" +
				"<body>" +
				"<header> " +
				"<div class=\"support-old-browsers\"> " +
				"<p> " +
				"We stop supporting Internet Explorer. <br>Other browsers are more stable. " +
				"</div> " +
				"<div class=\"header\"> " +
				"<h1 class=\"logo\"> " +
				"<a href=\"/\">Cities</a> " +
				"</h1> " +
				"<nav> " +
				"<ul>" +
				"<li><a href=\"/tokyo\">Tokyo</a> " +
				"<li><a href=\"/moscow\">Moscow</a> " +
				"<li><a href=\"/london\">London</a>" +
				"</ul> " +
				"</nav> " +
				"</div> " +
				"</header> " +
				"<main> " +
				"<h2>London</h2> " +
				"<p>London is the capital of Great Britain, its political, economic and commercial " +
				"centre. It is one of the largest cities in the world and the largest city in Europe. " +
				"Its population is about 8 million. " +
				"<p>London is one of the oldest and most interesting cities in the world. " +
				"<p>Traditionally, it is divided into several parts: the City, Westminster, " +
				"the West End and the East End. They are very different from each other and seem to " +
				"belong to different towns and epochs. " +
				"</main> " +
				"<aside> " +
				"<a href=\"/banner.do?id=160803\" target=\"_blank\">" +
				"<img src=\"/images/banner-160803.png\" width=\"300\" height=\"600\" alt=\"Rent a coworking in London\">" +
				"</a> " +
				"</aside> " +
				"<footer> " +
				"<p>&copy; 2022 All rights reserved " +
				"</footer>"
				;
			const string targetOutputD = "<!DOCTYPE html>\r" +
				"<html lang=\"en\">\r" +
				"<head>\r" +
				"<title>London | Largest cities in the world</title>\r" +
				"<meta charset=\"utf-8\">\r" +
				"<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r" +
				"<link href=\"/css/style.css\" rel=\"stylesheet\">\r" +
				"<body>\r" +
				"<header>\r" +
				"<div class=\"support-old-browsers\">\r" +
				"<p>\r" +
				"We stop supporting Internet Explorer. <br>Other browsers are more stable.\r" +
				"</div>\r" +
				"<div class=\"header\">\r" +
				"<h1 class=\"logo\">\r" +
				"<a href=\"/\">Cities</a>\r" +
				"</h1>\r" +
				"<nav>\r" +
				"<ul>\r" +
				"<li><a href=\"/tokyo\">Tokyo</a>\r" +
				"<li><a href=\"/moscow\">Moscow</a>\r" +
				"<li><a href=\"/london\">London</a>\r" +
				"</ul>\r" +
				"</nav>\r" +
				"</div>\r" +
				"</header>\r" +
				"<main>\r" +
				"<h2>London</h2>\r" +
				"<p>London is the capital of Great Britain, its political, economic and commercial\r" +
				"centre. It is one of the largest cities in the world and the largest city in Europe.\r" +
				"Its population is about 8 million.\r" +
				"<p>London is one of the oldest and most interesting cities in the world.\r" +
				"<p>Traditionally, it is divided into several parts: the City, Westminster,\r" +
				"the West End and the East End. They are very different from each other and seem to\r" +
				"belong to different towns and epochs.\r" +
				"</main>\r" +
				"<aside>\r" +
				"<a href=\"/banner.do?id=160803\" target=\"_blank\">" +
				"<img src=\"/images/banner-160803.png\" width=\"300\" height=\"600\" alt=\"Rent a coworking in London\">" +
				"</a> " +
				"</aside>\r" +
				"<footer>\r" +
				"<p>&copy; 2022 All rights reserved\r" +
				"</footer>"
				;
			const string targetOutputE = "<!DOCTYPE html>" +
				"<html lang=\"en\">" +
				"<head>" +
				"<title>London | Largest cities in the world</title>" +
				"<meta charset=\"utf-8\">" +
				"<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">" +
				"<link href=\"/css/style.css\" rel=\"stylesheet\">" +
				"<body>" +
				"<header>" +
				"<div class=\"support-old-browsers\">" +
				"<p>" +
				"We stop supporting Internet Explorer. <br>Other browsers are more stable." +
				"</div>" +
				"<div class=\"header\">" +
				"<h1 class=\"logo\">" +
				"<a href=\"/\">Cities</a>" +
				"</h1>" +
				"<nav>" +
				"<ul>" +
				"<li><a href=\"/tokyo\">Tokyo</a>" +
				"<li><a href=\"/moscow\">Moscow</a>" +
				"<li><a href=\"/london\">London</a>" +
				"</ul>" +
				"</nav>" +
				"</div>" +
				"</header>" +
				"<main>" +
				"<h2>London</h2>" +
				"<p>London is the capital of Great Britain, its political, economic and commercial " +
				"centre. It is one of the largest cities in the world and the largest city in Europe. " +
				"Its population is about 8 million." +
				"<p>London is one of the oldest and most interesting cities in the world." +
				"<p>Traditionally, it is divided into several parts: the City, Westminster, " +
				"the West End and the East End. They are very different from each other and seem to " +
				"belong to different towns and epochs." +
				"</main>" +
				"<aside>" +
				"<a href=\"/banner.do?id=160803\" target=\"_blank\">" +
				"<img src=\"/images/banner-160803.png\" width=\"300\" height=\"600\" alt=\"Rent a coworking in London\">" +
				"</a>" +
				"</aside>" +
				"<footer>" +
				"<p>&copy; 2022 All rights reserved" +
				"</footer>"
				;
			const string targetOutputF = "<!DOCTYPE html>\r" +
				"<html lang=\"en\">\r" +
				"<head>\r" +
				"<title>London | Largest cities in the world</title>\r" +
				"<meta charset=\"utf-8\">\r" +
				"<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r" +
				"<link href=\"/css/style.css\" rel=\"stylesheet\">\r" +
				"<body>\r" +
				"<header>\r" +
				"<div class=\"support-old-browsers\">\r" +
				"<p>\r" +
				"We stop supporting Internet Explorer. <br>Other browsers are more stable.\r" +
				"</div>\r" +
				"<div class=\"header\">\r" +
				"<h1 class=\"logo\">\r" +
				"<a href=\"/\">Cities</a>\r" +
				"</h1>\r" +
				"<nav>\r" +
				"<ul>\r" +
				"<li><a href=\"/tokyo\">Tokyo</a>\r" +
				"<li><a href=\"/moscow\">Moscow</a>\r" +
				"<li><a href=\"/london\">London</a>\r" +
				"</ul>\r" +
				"</nav>\r" +
				"</div>\r" +
				"</header>\r" +
				"<main>\r" +
				"<h2>London</h2>\r" +
				"<p>London is the capital of Great Britain, its political, economic and commercial\r" +
				"centre. It is one of the largest cities in the world and the largest city in Europe.\r" +
				"Its population is about 8 million.\r" +
				"<p>London is one of the oldest and most interesting cities in the world.\r" +
				"<p>Traditionally, it is divided into several parts: the City, Westminster,\r" +
				"the West End and the East End. They are very different from each other and seem to\r" +
				"belong to different towns and epochs.\r" +
				"</main>\r" +
				"<aside>\r" +
				"<a href=\"/banner.do?id=160803\" target=\"_blank\">" +
				"<img src=\"/images/banner-160803.png\" width=\"300\" height=\"600\" alt=\"Rent a coworking in London\">" +
				"</a>" +
				"</aside>\r" +
				"<footer>\r" +
				"<p>&copy; 2022 All rights reserved\r" +
				"</footer>"
				;
			const string targetOutputG = "<!DOCTYPE html>" +
				"<html lang=\"en\">" +
				"<head>" +
				"<title>London | Largest cities in the world</title>" +
				"<meta charset=\"utf-8\">" +
				"<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">" +
				"<link href=\"/css/style.css\" rel=\"stylesheet\">" +
				"<body>" +
				"<header>" +
				"<div class=\"support-old-browsers\">" +
				"<p>" +
				"We stop supporting Internet Explorer. <br>Other browsers are more stable." +
				"</div>" +
				"<div class=\"header\">" +
				"<h1 class=\"logo\">" +
				"<a href=\"/\">Cities</a>" +
				"</h1>" +
				"<nav>" +
				"<ul>" +
				"<li><a href=\"/tokyo\">Tokyo</a>" +
				"<li><a href=\"/moscow\">Moscow</a>" +
				"<li><a href=\"/london\">London</a>" +
				"</ul>" +
				"</nav>" +
				"</div>" +
				"</header>" +
				"<main>" +
				"<h2>London</h2>" +
				"<p>London is the capital of Great Britain, its political, economic and commercial " +
				"centre. It is one of the largest cities in the world and the largest city in Europe. " +
				"Its population is about 8 million." +
				"<p>London is one of the oldest and most interesting cities in the world." +
				"<p>Traditionally, it is divided into several parts: the City, Westminster, " +
				"the West End and the East End. They are very different from each other and seem to " +
				"belong to different towns and epochs." +
				"</main>" +
				"<aside>" +
				"<a href=\"/banner.do?id=160803\" target=\"_blank\">" +
				"<img src=\"/images/banner-160803.png\" width=\"300\" height=\"600\" alt=\"Rent a coworking in London\">" +
				"</a>" +
				"</aside>" +
				"<footer>" +
				"<p>&copy; 2022 All rights reserved" +
				"</footer>"
				;
			const string targetOutputH = targetOutputF;

			// Act
			string outputA = keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = keepingWhitespaceAndNewLinesMinifier.Minify(input).MinifiedContent;
			string outputC = safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputE = mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputF = mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputG = aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputH = aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
			Assert.Equal(targetOutputE, outputE);
			Assert.Equal(targetOutputF, outputF);
			Assert.Equal(targetOutputG, outputG);
			Assert.Equal(targetOutputH, outputH);
		}



		[Fact]
		public void WhitespaceMinificationWithRemovingHtmlCommentsInHtmlDocumentIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.None,
					PreserveNewLines = false,
					RemoveHtmlComments = true
				});
			var keepingWhitespaceAndNewLinesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.None,
					PreserveNewLines = true,
					RemoveHtmlComments = true
				});
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Safe,
					PreserveNewLines = false,
					RemoveHtmlComments = true
				});
			var safeRemovingWhitespaceExceptForNewLinesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Safe,
					PreserveNewLines = true,
					RemoveHtmlComments = true
				});
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Medium,
					PreserveNewLines = false,
					RemoveHtmlComments = true
				});
			var mediumRemovingWhitespaceExceptForNewLinesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Medium,
					PreserveNewLines = true,
					RemoveHtmlComments = true
				});
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive,
					PreserveNewLines = false,
					RemoveHtmlComments = true
				});
			var aggressiveRemovingWhitespaceExceptForNewLinesMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive,
					PreserveNewLines = true,
					RemoveHtmlComments = true
				});

			const string input = "<!DOCTYPE html>\r\n" +
				"<html lang=\"en\">\r\n" +
				"    <head>\r\n" +
				"        <title>London | Largest cities in the world</title>\r\n" +
				"        <meta charset=\"utf-8\">\r\n" +
				"        <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n" +
				"        <link href=\"/css/style.css\" rel=\"stylesheet\">\r\n" +
				"    </head>\r\n" +
				"    <body>\r\n\r\n" +
				"        <!-- header START -->\r\n" +
				"        <header>\r\n" +
				"            <h1 class=\"logo\">\r\n" +
				"                <a href=\"/\">Cities</a>\r\n" +
				"            </h1>\r\n" +
				"            <!-- navigation START -->\r\n" +
				"            <nav>\r\n" +
				"                <!-- menu START -->\r\n" +
				"                <ul>\r\n" +
				"                    <li><a href=\"/tokyo\">Tokyo</a></li>\r\n" +
				"                    <li><a href=\"/moscow\">Moscow</a></li>\r\n" +
				"                    <li><a href=\"/london\">London</a></li>\r\n" +
				"                </ul>\r\n" +
				"                <!-- menu END -->\r\n" +
				"            </nav>\r\n" +
				"            <!-- navigation END -->\r\n" +
				"        </header>\r\n" +
				"        <!-- header END -->\r\n\r\n" +
				"        <!-- content START -->\r\n" +
				"        <!-- main START -->\r\n" +
				"        <main>\r\n" +
				"            <h2>London</h2>\r\n\r\n" +
				"            <p>London is the capital of  Great Britain, its political, economic and commercial\r\n" +
				"            centre. It is one of the largest cities in the world and the largest city in Europe.\r\n" +
				"            Its population is about 8 million.</p>\r\n" +
				"            <p>London is one of the oldest and most interesting cities in the world.</p>\r\n" +
				"            <p>Traditionally, it is divided into several parts: the City, Westminster, \r\n" +
				"            the West End and the East End. They are very different from each other and seem to\r\n" +
				"            belong to different towns and epochs.</p>\r\n" +
				"        </main>\r\n" +
				"        <!-- main END -->\r\n" +
				"        <!-- sidebar START -->\r\n" +
				"        <!--noindex-->\r\n" +
				"        <aside>\r\n" +
				"            <!-- banner START -->\r\n" +
				"            <a href=\"/banner.do?id=160803\" target=\"_blank\">" +
				"<img src=\"/images/banner-160803.png\" width=\"300\" height=\"600\" alt=\"Rent a coworking in London\">" +
				"</a>\r\n" +
				"            <!-- banner END -->\r\n" +
				"        </aside>\r\n" +
				"        <!--/noindex-->\r\n" +
				"        <!-- sidebar END -->\r\n" +
				"        <!-- content END -->\r\n\r\n" +
				"        <!-- footer START -->\r\n" +
				"        <footer>\r\n" +
				"            <p>&copy; 2022 All rights reserved</p>\r\n" +
				"        </footer>\r\n" +
				"        <!-- footer END -->\r\n\r\n" +
				"    </body>\r\n" +
				"</html>\r\n" +
				"<!--\r\n" +
				"    generated 3 seconds ago\r\n" +
				"    generated in 0.246 seconds\r\n" +
				"    served from batcache in 0.004 seconds\r\n" +
				"    expires in 297 seconds\r\n" +
				"-->"
				;
			const string targetOutputA = "<!DOCTYPE html>\r\n" +
				"<html lang=\"en\">\r\n" +
				"    <head>\r\n" +
				"        <title>London | Largest cities in the world</title>\r\n" +
				"        <meta charset=\"utf-8\">\r\n" +
				"        <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n" +
				"        <link href=\"/css/style.css\" rel=\"stylesheet\">\r\n" +
				"    </head>\r\n" +
				"    <body>\r\n\r\n" +
				"        \r\n" +
				"        <header>\r\n" +
				"            <h1 class=\"logo\">\r\n" +
				"                <a href=\"/\">Cities</a>\r\n" +
				"            </h1>\r\n" +
				"            \r\n" +
				"            <nav>\r\n" +
				"                \r\n" +
				"                <ul>\r\n" +
				"                    <li><a href=\"/tokyo\">Tokyo</a></li>\r\n" +
				"                    <li><a href=\"/moscow\">Moscow</a></li>\r\n" +
				"                    <li><a href=\"/london\">London</a></li>\r\n" +
				"                </ul>\r\n" +
				"                \r\n" +
				"            </nav>\r\n" +
				"            \r\n" +
				"        </header>\r\n" +
				"        \r\n\r\n" +
				"        \r\n" +
				"        \r\n" +
				"        <main>\r\n" +
				"            <h2>London</h2>\r\n\r\n" +
				"            <p>London is the capital of  Great Britain, its political, economic and commercial\r\n" +
				"            centre. It is one of the largest cities in the world and the largest city in Europe.\r\n" +
				"            Its population is about 8 million.</p>\r\n" +
				"            <p>London is one of the oldest and most interesting cities in the world.</p>\r\n" +
				"            <p>Traditionally, it is divided into several parts: the City, Westminster, \r\n" +
				"            the West End and the East End. They are very different from each other and seem to\r\n" +
				"            belong to different towns and epochs.</p>\r\n" +
				"        </main>\r\n" +
				"        \r\n" +
				"        \r\n" +
				"        <!--noindex-->\r\n" +
				"        <aside>\r\n" +
				"            \r\n" +
				"            <a href=\"/banner.do?id=160803\" target=\"_blank\">" +
				"<img src=\"/images/banner-160803.png\" width=\"300\" height=\"600\" alt=\"Rent a coworking in London\">" +
				"</a>\r\n" +
				"            \r\n" +
				"        </aside>\r\n" +
				"        <!--/noindex-->\r\n" +
				"        \r\n" +
				"        \r\n\r\n" +
				"        \r\n" +
				"        <footer>\r\n" +
				"            <p>&copy; 2022 All rights reserved</p>\r\n" +
				"        </footer>\r\n" +
				"        \r\n\r\n" +
				"    </body>\r\n" +
				"</html>\r\n"
				;
			const string targetOutputB = targetOutputA;
			const string targetOutputC = "<!DOCTYPE html>" +
				"<html lang=\"en\">" +
				"<head>" +
				"<title>London | Largest cities in the world</title>" +
				"<meta charset=\"utf-8\">" +
				"<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">" +
				"<link href=\"/css/style.css\" rel=\"stylesheet\">" +
				"</head>" +
				"<body>" +
				"<header> " +
				"<h1 class=\"logo\"> " +
				"<a href=\"/\">Cities</a> " +
				"</h1> " +
				"<nav> " +
				"<ul>" +
				"<li><a href=\"/tokyo\">Tokyo</a></li> " +
				"<li><a href=\"/moscow\">Moscow</a></li> " +
				"<li><a href=\"/london\">London</a></li>" +
				"</ul> " +
				"</nav> " +
				"</header> " +
				"<main> " +
				"<h2>London</h2> " +
				"<p>London is the capital of Great Britain, its political, economic and commercial " +
				"centre. It is one of the largest cities in the world and the largest city in Europe. " +
				"Its population is about 8 million.</p> " +
				"<p>London is one of the oldest and most interesting cities in the world.</p> " +
				"<p>Traditionally, it is divided into several parts: the City, Westminster, " +
				"the West End and the East End. They are very different from each other and seem to " +
				"belong to different towns and epochs.</p> " +
				"</main> " +
				"<!--noindex--> " +
				"<aside> " +
				"<a href=\"/banner.do?id=160803\" target=\"_blank\">" +
				"<img src=\"/images/banner-160803.png\" width=\"300\" height=\"600\" alt=\"Rent a coworking in London\">" +
				"</a> " +
				"</aside> " +
				"<!--/noindex--> " +
				"<footer> " +
				"<p>&copy; 2022 All rights reserved</p> " +
				"</footer>" +
				"</body>" +
				"</html>"
				;
			const string targetOutputD = "<!DOCTYPE html>\r\n" +
				"<html lang=\"en\">\r\n" +
				"<head>\r\n" +
				"<title>London | Largest cities in the world</title>\r\n" +
				"<meta charset=\"utf-8\">\r\n" +
				"<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n" +
				"<link href=\"/css/style.css\" rel=\"stylesheet\">\r\n" +
				"</head>\r\n" +
				"<body>\r\n" +
				"<header>\r\n" +
				"<h1 class=\"logo\">\r\n" +
				"<a href=\"/\">Cities</a>\r\n" +
				"</h1>\r\n" +
				"<nav>\r\n" +
				"<ul>\r\n" +
				"<li><a href=\"/tokyo\">Tokyo</a></li>\r\n" +
				"<li><a href=\"/moscow\">Moscow</a></li>\r\n" +
				"<li><a href=\"/london\">London</a></li>\r\n" +
				"</ul>\r\n" +
				"</nav>\r\n" +
				"</header>\r\n" +
				"<main>\r\n" +
				"<h2>London</h2>\r\n" +
				"<p>London is the capital of Great Britain, its political, economic and commercial\r\n" +
				"centre. It is one of the largest cities in the world and the largest city in Europe.\r\n" +
				"Its population is about 8 million.</p>\r\n" +
				"<p>London is one of the oldest and most interesting cities in the world.</p>\r\n" +
				"<p>Traditionally, it is divided into several parts: the City, Westminster,\r\n" +
				"the West End and the East End. They are very different from each other and seem to\r\n" +
				"belong to different towns and epochs.</p>\r\n" +
				"</main>\r\n" +
				"<!--noindex-->\r\n" +
				"<aside>\r\n" +
				"<a href=\"/banner.do?id=160803\" target=\"_blank\">" +
				"<img src=\"/images/banner-160803.png\" width=\"300\" height=\"600\" alt=\"Rent a coworking in London\">" +
				"</a>\r\n" +
				"</aside>\r\n" +
				"<!--/noindex-->\r\n" +
				"<footer>\r\n" +
				"<p>&copy; 2022 All rights reserved</p>\r\n" +
				"</footer>\r\n" +
				"</body>\r\n" +
				"</html>"
				;
			const string targetOutputE = "<!DOCTYPE html>" +
				"<html lang=\"en\">" +
				"<head>" +
				"<title>London | Largest cities in the world</title>" +
				"<meta charset=\"utf-8\">" +
				"<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">" +
				"<link href=\"/css/style.css\" rel=\"stylesheet\">" +
				"</head>" +
				"<body>" +
				"<header>" +
				"<h1 class=\"logo\">" +
				"<a href=\"/\">Cities</a>" +
				"</h1>" +
				"<nav>" +
				"<ul>" +
				"<li><a href=\"/tokyo\">Tokyo</a></li>" +
				"<li><a href=\"/moscow\">Moscow</a></li>" +
				"<li><a href=\"/london\">London</a></li>" +
				"</ul>" +
				"</nav>" +
				"</header>" +
				"<main>" +
				"<h2>London</h2>" +
				"<p>London is the capital of Great Britain, its political, economic and commercial " +
				"centre. It is one of the largest cities in the world and the largest city in Europe. " +
				"Its population is about 8 million.</p>" +
				"<p>London is one of the oldest and most interesting cities in the world.</p>" +
				"<p>Traditionally, it is divided into several parts: the City, Westminster, " +
				"the West End and the East End. They are very different from each other and seem to " +
				"belong to different towns and epochs.</p>" +
				"</main>" +
				"<!--noindex-->" +
				"<aside>" +
				"<a href=\"/banner.do?id=160803\" target=\"_blank\">" +
				"<img src=\"/images/banner-160803.png\" width=\"300\" height=\"600\" alt=\"Rent a coworking in London\">" +
				"</a>" +
				"</aside>" +
				"<!--/noindex-->" +
				"<footer>" +
				"<p>&copy; 2022 All rights reserved</p>" +
				"</footer>" +
				"</body>" +
				"</html>"
				;
			const string targetOutputF = "<!DOCTYPE html>\r\n" +
				"<html lang=\"en\">\r\n" +
				"<head>\r\n" +
				"<title>London | Largest cities in the world</title>\r\n" +
				"<meta charset=\"utf-8\">\r\n" +
				"<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n" +
				"<link href=\"/css/style.css\" rel=\"stylesheet\">\r\n" +
				"</head>\r\n" +
				"<body>\r\n" +
				"<header>\r\n" +
				"<h1 class=\"logo\">\r\n" +
				"<a href=\"/\">Cities</a>\r\n" +
				"</h1>\r\n" +
				"<nav>\r\n" +
				"<ul>\r\n" +
				"<li><a href=\"/tokyo\">Tokyo</a></li>\r\n" +
				"<li><a href=\"/moscow\">Moscow</a></li>\r\n" +
				"<li><a href=\"/london\">London</a></li>\r\n" +
				"</ul>\r\n" +
				"</nav>\r\n" +
				"</header>\r\n" +
				"<main>\r\n" +
				"<h2>London</h2>\r\n" +
				"<p>London is the capital of Great Britain, its political, economic and commercial\r\n" +
				"centre. It is one of the largest cities in the world and the largest city in Europe.\r\n" +
				"Its population is about 8 million.</p>\r\n" +
				"<p>London is one of the oldest and most interesting cities in the world.</p>\r\n" +
				"<p>Traditionally, it is divided into several parts: the City, Westminster,\r\n" +
				"the West End and the East End. They are very different from each other and seem to\r\n" +
				"belong to different towns and epochs.</p>\r\n" +
				"</main>\r\n" +
				"<!--noindex-->\r\n" +
				"<aside>\r\n" +
				"<a href=\"/banner.do?id=160803\" target=\"_blank\">" +
				"<img src=\"/images/banner-160803.png\" width=\"300\" height=\"600\" alt=\"Rent a coworking in London\">" +
				"</a>\r\n" +
				"</aside>\r\n" +
				"<!--/noindex-->\r\n" +
				"<footer>\r\n" +
				"<p>&copy; 2022 All rights reserved</p>\r\n" +
				"</footer>\r\n" +
				"</body>\r\n" +
				"</html>"
				;
			const string targetOutputG = targetOutputE;
			const string targetOutputH = targetOutputF;

			// Act
			string outputA = keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = keepingWhitespaceAndNewLinesMinifier.Minify(input).MinifiedContent;
			string outputC = safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = safeRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputE = mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputF = mediumRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;
			string outputG = aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputH = aggressiveRemovingWhitespaceExceptForNewLinesMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
			Assert.Equal(targetOutputE, outputE);
			Assert.Equal(targetOutputF, outputF);
			Assert.Equal(targetOutputG, outputG);
			Assert.Equal(targetOutputH, outputH);
		}

		#region IDisposable implementation

		public void Dispose()
		{
			_keepingWhitespaceMinifier = null;
			_keepingWhitespaceAndNewLinesMinifier = null;
			_safeRemovingWhitespaceMinifier = null;
			_safeRemovingWhitespaceExceptForNewLinesMinifier = null;
			_mediumRemovingWhitespaceMinifier = null;
			_mediumRemovingWhitespaceExceptForNewLinesMinifier = null;
			_aggressiveRemovingWhitespaceMinifier = null;
			_aggressiveRemovingWhitespaceExceptForNewLinesMinifier = null;
		}

		#endregion
	}
}