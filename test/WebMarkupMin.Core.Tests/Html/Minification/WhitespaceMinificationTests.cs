using Xunit;

namespace WebMarkupMin.Core.Tests.Html.Minification
{
	public class WhitespaceMinificationTests
	{
		[Fact]
		public void WhitespaceMinificationInStyleTagIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.None });
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Safe });
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive });

			const string input = "<style>cite { quotes: \" «\" \"» \"; }    </style>";
			const string targetOutputA = input;
			const string targetOutputB = "<style>cite { quotes: \" «\" \"» \"; }</style>";
			const string targetOutputC = targetOutputB;
			const string targetOutputD = targetOutputB;

			// Act
			string outputA = keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
		}

		[Fact]
		public void WhitespaceMinificationInScriptTagIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.None });
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Safe });
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive });

			const string input = "<script>alert(\"Hello,     world!\");    </script>";
			const string targetOutputA = input;
			const string targetOutputB = "<script>alert(\"Hello,     world!\");</script>";
			const string targetOutputC = targetOutputB;
			const string targetOutputD = targetOutputB;

			// Act
			string outputA = keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
		}

		[Fact]
		public void WhitespaceMinificationInParagraphTagIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.None });
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Safe });
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive });

			const string input = "<p>  New  \n  Release	\n</p>";
			const string targetOutputA = input;
			const string targetOutputB = "<p> New Release </p>";
			const string targetOutputC = "<p>New Release</p>";
			const string targetOutputD = targetOutputC;

			// Act
			string output9A = keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string output9B = safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string output9C = mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string output9D = aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, output9A);
			Assert.Equal(targetOutputB, output9B);
			Assert.Equal(targetOutputC, output9C);
			Assert.Equal(targetOutputD, output9D);
		}

		[Fact]
		public void WhitespaceMinificationInUnorderedListTagIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.None });
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Safe });
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive });

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
			const string targetOutputB = "<ul>" +
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
			const string targetOutputC = "<ul>" +
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
			const string targetOutputD = targetOutputC;

			// Act
			string outputA = keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
		}

		[Fact]
		public void WhitespaceMinificationInDescriptionListTagIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.None });
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Safe });
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive });

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
			const string targetOutputB = "<dl>" +
				"<dt> Name: </dt> " +
				"<dd> John Doe </dd> " +
				"<dt> Gender: </dt> " +
				"<dd> Male </dd> " +
				"<dt> Day of Birth: </dt> " +
				"<dd> Unknown </dd>" +
				"</dl>"
				;
			const string targetOutputC = "<dl>" +
				"<dt>Name:</dt>" +
				"<dd>John Doe</dd>" +
				"<dt>Gender:</dt>" +
				"<dd>Male</dd>" +
				"<dt>Day of Birth:</dt>" +
				"<dd>Unknown</dd>" +
				"</dl>"
				;
			const string targetOutputD = targetOutputC;

			// Act
			string outputA = keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
		}

		[Fact]
		public void WhitespaceMinificationInRubyTagIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.None });
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Safe });
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive });

			const string input1 = "<ruby>\n" +
				"	漢  <rt>  Kan  </rt>\n" +
				"	字  <rt>  ji  </rt>\n" +
				"</ruby>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<ruby> " +
				"漢 <rt> Kan </rt> " +
				"字 <rt> ji </rt> " +
				"</ruby>"
				;
			const string targetOutput1C = targetOutput1B;
			const string targetOutput1D = "<ruby>" +
				"漢 <rt>Kan</rt> " +
				"字 <rt>ji</rt>" +
				"</ruby>"
				;

			const string input2 = "<ruby>\n" +
				"	漢  <rp>  (</rp>  <rt>  Kan  </rt>  <rp>)  </rp>\n" +
				"	字  <rp>  (</rp>  <rt>  ji  </rt>  <rp>)  </rp>\n" +
				"</ruby>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = "<ruby> " +
				"漢 <rp> (</rp> <rt> Kan </rt> <rp>) </rp> " +
				"字 <rp> (</rp> <rt> ji </rt> <rp>) </rp> " +
				"</ruby>"
				;
			const string targetOutput2C = targetOutput2B;
			const string targetOutput2D = "<ruby>" +
				"漢 <rp>(</rp> <rt>Kan</rt> <rp>)</rp> " +
				"字 <rp>(</rp> <rt>ji</rt> <rp>)</rp>" +
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
			const string targetOutput3B = "<ruby> " +
				"東 " +
				"<rb> 京 </rb> " +
				"<rt> とう </rt> " +
				"<rt> きょう </rt> " +
				"</ruby>";
			const string targetOutput3C = targetOutput3B;
			const string targetOutput3D = "<ruby>" +
				"東 " +
				"<rb>京</rb> " +
				"<rt>とう</rt> " +
				"<rt>きょう</rt>" +
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
			const string targetOutput4B = "<ruby> " +
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
			const string targetOutput4C = targetOutput4B;
			const string targetOutput4D = "<ruby>" +
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

			const string input5 = "<ruby>\n" +
				"	<rb>  旧  </rb>  <rb>  金  </rb>  <rb>  山  </rb>\n" +
				"	<rt>  jiù  </rt>  <rt>  jīn  </rt> <rt>  shān  </rt>\n" +
				"	<rtc>  Сан-Франциско  </rtc>\n" +
				"</ruby>"
				;
			const string targetOutput5A = input5;
			const string targetOutput5B = "<ruby> " +
				"<rb> 旧 </rb> <rb> 金 </rb> <rb> 山 </rb> " +
				"<rt> jiù </rt> <rt> jīn </rt> <rt> shān </rt> " +
				"<rtc> Сан-Франциско </rtc> " +
				"</ruby>"
				;
			const string targetOutput5C = targetOutput5B;
			const string targetOutput5D = "<ruby>" +
				"<rb>旧</rb> <rb>金</rb> <rb>山</rb> " +
				"<rt>jiù</rt> <rt>jīn</rt> <rt>shān</rt> " +
				"<rtc>Сан-Франциско</rtc>" +
				"</ruby>"
				;

			// Act
			string output1A = keepingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1B = safeRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = mediumRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1D = aggressiveRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2B = safeRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = mediumRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2D = aggressiveRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3B = safeRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3C = mediumRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3D = aggressiveRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4B = safeRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4C = mediumRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4D = aggressiveRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5B = safeRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5C = mediumRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5D = aggressiveRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);
			Assert.Equal(targetOutput1D, output1D);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);
			Assert.Equal(targetOutput2D, output2D);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);
			Assert.Equal(targetOutput3D, output3D);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);
			Assert.Equal(targetOutput4D, output4D);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);
			Assert.Equal(targetOutput5D, output5D);
		}

		[Fact]
		public void WhitespaceMinificationInFigureTagIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.None });
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Safe });
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive });

			const string input = "<figure>\n" +
				"	<img src=\"libsass-logo.png\" alt=\"LibSass logo\" width=\"640\" height=\"320\">\n" +
				"	<figcaption>  Fig 1.  -  LibSass logo. \n" +
				"</figcaption>\n" +
				"</figure>"
				;
			const string targetOutputA = input;
			const string targetOutputB = "<figure>" +
				" <img src=\"libsass-logo.png\" alt=\"LibSass logo\" width=\"640\" height=\"320\"> " +
				"<figcaption> Fig 1. - LibSass logo. </figcaption>" +
				"</figure>"
				;
			const string targetOutputC = "<figure>" +
				"<img src=\"libsass-logo.png\" alt=\"LibSass logo\" width=\"640\" height=\"320\">" +
				"<figcaption>Fig 1. - LibSass logo.</figcaption>" +
				"</figure>"
				;
			const string targetOutputD = targetOutputC;

			// Act
			string outputA = keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
		}

		[Fact]
		public void WhitespaceMinificationInTableTagIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.None });
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Safe });
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive });

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
			const string targetOutputB = "<table class=\"table\">" +
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
			const string targetOutputC = "<table class=\"table\">" +
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
			const string targetOutputD = targetOutputC;

			// Act
			string outputA = keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
		}

		[Fact]
		public void WhitespaceMinificationInFormAndFieldSetTagsIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.None });
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Safe });
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive });

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
			const string targetOutputB = "<form> " +
				"<fieldset>" +
				"<legend> Personal data </legend>" +
				"Name: <input type=\"text\" size=\"50\"><br> " +
				"Email: <input type=\"text\" size=\"50\"><br> " +
				"Date of birth: <input type=\"text\" size=\"10\"> " +
				"</fieldset> " +
				"</form>"
				;
			const string targetOutputC = "<form>" +
				"<fieldset>" +
				"<legend>Personal data</legend>" +
				"Name: <input type=\"text\" size=\"50\"><br> " +
				"Email: <input type=\"text\" size=\"50\"><br> " +
				"Date of birth: <input type=\"text\" size=\"10\">" +
				"</fieldset>" +
				"</form>"
				;
			const string targetOutputD = targetOutputC;

			// Act
			string outputA = keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
		}

		[Fact]
		public void WhitespaceMinificationInLabelAndTextareaTagsIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.None });
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Safe });
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive });

			const string input = "<label>		Text:  \n </label> \n\t  " +
				"<textarea> THERE IS NO     KNOWLEDGE \n\n   – \t    THAT IS NOT POWER </textarea> \t\n  ";
			const string targetOutputA = input;
			const string targetOutputB = "<label> Text: </label> " +
				"<textarea> THERE IS NO     KNOWLEDGE \n\n   – \t    THAT IS NOT POWER </textarea>";
			const string targetOutputC = targetOutputB;
			const string targetOutputD = "<label>Text:</label> " +
				"<textarea> THERE IS NO     KNOWLEDGE \n\n   – \t    THAT IS NOT POWER </textarea>";

			// Act
			string outputA = keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
		}

		[Fact]
		public void WhitespaceMinificationInSelectTagIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.None });
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Safe });
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive });

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
			const string targetOutputB = "<select name=\"preprocessors\">" +
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
			const string targetOutputC = targetOutputB;
			const string targetOutputD = "<select name=\"preprocessors\">" +
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

			// Act
			string outputA = keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
		}

		[Fact]
		public void WhitespaceMinificationInMenuTagIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.None });
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Safe });
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive });

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
			const string targetOutput1B = "<menu>" +
				"<menuitem label=\"New\" icon=\"icons/new.png\" onclick=\"new()\"> </menuitem>" +
				"<menuitem label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\"> </menuitem>" +
				"<menuitem label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\"> </menuitem>" +
				"</menu>"
				;
			const string targetOutput1C = "<menu>" +
				"<menuitem label=\"New\" icon=\"icons/new.png\" onclick=\"new()\"></menuitem>" +
				"<menuitem label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\"></menuitem>" +
				"<menuitem label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\"></menuitem>" +
				"</menu>"
				;
			const string targetOutput1D = targetOutput1C;


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
			const string targetOutput2B = "<menu>" +
				"<command type=\"command\" label=\"New\" icon=\"icons/new.png\" onclick=\"new()\"> </command>" +
				"<command type=\"command\" label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\"> </command>" +
				"<command type=\"command\" label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\"> </command>" +
				"</menu>"
				;
			const string targetOutput2C = "<menu>" +
				"<command type=\"command\" label=\"New\" icon=\"icons/new.png\" onclick=\"new()\"></command>" +
				"<command type=\"command\" label=\"Open\" icon=\"icons/open.png\" onclick=\"open()\"></command>" +
				"<command type=\"command\" label=\"Save\" icon=\"icons/save.png\" onclick=\"save()\"></command>" +
				"</menu>"
				;
			const string targetOutput2D = targetOutput2C;

			// Act
			string output1A = keepingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1B = safeRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = mediumRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1D = aggressiveRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2B = safeRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = mediumRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2D = aggressiveRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);
			Assert.Equal(targetOutput1D, output1D);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);
			Assert.Equal(targetOutput2D, output2D);
		}

		[Fact]
		public void WhitespaceMinificationInVideoTagIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.None });
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Safe });
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive });

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
			const string targetOutputB = "<video width=\"320\" height=\"240\" poster=\"video/poster.png\" controls=\"controls\">" +
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
			const string targetOutputC = targetOutputB;
			const string targetOutputD = "<video width=\"320\" height=\"240\" poster=\"video/poster.png\" controls=\"controls\">" +
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

			// Act
			string outputA = keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
		}

		[Fact]
		public void WhitespaceMinificationInSvgTagIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.None });
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Safe });
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive });

			const string input1 = "<div>\n" +
				"	<svg width=\"150\" height=\"100\" viewBox=\"0 0 3 2\">\n" +
				"		<rect width=\"1\" height=\"2\" x=\"0\" fill=\"#008d46\" />\n" +
				"		<rect width=\"1\" height=\"2\" x=\"1\" fill=\"#ffffff\" />\n" +
				"		<rect width=\"1\" height=\"2\" x=\"2\" fill=\"#d2232c\" />\n" +
				"	</svg>\n" +
				"</div>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<div> " +
				"<svg width=\"150\" height=\"100\" viewBox=\"0 0 3 2\">" +
				"<rect width=\"1\" height=\"2\" x=\"0\" fill=\"#008d46\" />" +
				"<rect width=\"1\" height=\"2\" x=\"1\" fill=\"#ffffff\" />" +
				"<rect width=\"1\" height=\"2\" x=\"2\" fill=\"#d2232c\" />" +
				"</svg> " +
				"</div>"
				;
			const string targetOutput1C = "<div>" +
				"<svg width=\"150\" height=\"100\" viewBox=\"0 0 3 2\">" +
				"<rect width=\"1\" height=\"2\" x=\"0\" fill=\"#008d46\" />" +
				"<rect width=\"1\" height=\"2\" x=\"1\" fill=\"#ffffff\" />" +
				"<rect width=\"1\" height=\"2\" x=\"2\" fill=\"#d2232c\" />" +
				"</svg>" +
				"</div>"
				;
			const string targetOutput1D = targetOutput1C;

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
			const string targetOutput2B = "<div> " +
				"<svg>" +
				"<text x=\"20\" y=\"40\" " +
				"transform=\"rotate(30, 20,40)\" " +
				"style=\"stroke: none; fill: #000000\">\n" +
				"\t\t  Rotated  SVG  text  \t\n" +
				"		</text>" +
				"</svg> " +
				"</div>"
				;
			const string targetOutput2C = "<div>" +
				"<svg>" +
				"<text x=\"20\" y=\"40\" " +
				"transform=\"rotate(30, 20,40)\" " +
				"style=\"stroke: none; fill: #000000\">\n" +
				"\t\t  Rotated  SVG  text  \t\n" +
				"		</text>" +
				"</svg>" +
				"</div>"
				;
			const string targetOutput2D = targetOutput2C;

			const string input3 = "<svg width=\"300\" height=\"50\" viewBox=\"0 0 300 50\">\n" +
				"	<rect x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" fill=\"#f6f8fa\" />\n" +
				"	<text x=\"50%\" y=\"50%\" fill=\"#24292e\" font-size=\"16\" text-anchor=\"middle\">\n" +
				"	<![CDATA[\n" +
				"	Vasya Pupkin <vasya-pupkin@mail.ru>\n" +
				"	]]>\n" +
				"	</text>\n" +
				"</svg>"
				;
			const string targetOutput3A = input3;
			const string targetOutput3B = "<svg width=\"300\" height=\"50\" viewBox=\"0 0 300 50\">" +
				"<rect x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" fill=\"#f6f8fa\" />" +
				"<text x=\"50%\" y=\"50%\" fill=\"#24292e\" font-size=\"16\" text-anchor=\"middle\">\n" +
				"	<![CDATA[\n" +
				"	Vasya Pupkin <vasya-pupkin@mail.ru>\n" +
				"	]]>\n" +
				"	</text>" +
				"</svg>"
				;
			const string targetOutput3C = targetOutput3B;
			const string targetOutput3D = targetOutput3C;

			// Act
			string output1A = keepingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1B = safeRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = mediumRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1D = aggressiveRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2B = safeRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = mediumRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2D = aggressiveRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3B = safeRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3C = mediumRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3D = aggressiveRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);
			Assert.Equal(targetOutput1D, output1D);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);
			Assert.Equal(targetOutput2D, output2D);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);
			Assert.Equal(targetOutput3D, output3D);
		}

		[Fact]
		public void WhitespaceMinificationInMathTagIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.None });
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Safe });
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive });

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
			const string targetOutputB = "<div> " +
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
			const string targetOutputC = "<div>" +
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
			const string targetOutputD = targetOutputC;

			// Act
			string outputA = keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
		}

		[Fact]
		public void WhitespaceMinificationInHtmlFragmentsWithTypographicalTagsIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.None });
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Safe });
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive });

			const string input1 = "<p>	 one  </p>    \n" +
				"<p>  two	 </p>\n\n    \n\t\t  " +
				"<div title=\"Some title...\">  three	 </div>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<p> one </p> " +
				"<p> two </p> " +
				"<div title=\"Some title...\"> three </div>"
				;
			const string targetOutput1C = "<p>one</p>" +
				"<p>two</p>" +
				"<div title=\"Some title...\">three</div>"
				;
			const string targetOutput1D = targetOutput1C;

			const string input2 = "<span>Some text...</span> \n\t  " +
				"<pre title=\"Some title...\">   Some     text </pre> \t\n  " +
				"<span>Some text...</span>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = "<span>Some text...</span> " +
				"<pre title=\"Some title...\">   Some     text </pre> " +
				"<span>Some text...</span>"
				;
			const string targetOutput2C = "<span>Some text...</span>" +
				"<pre title=\"Some title...\">   Some     text </pre>" +
				"<span>Some text...</span>"
				;
			const string targetOutput2D = targetOutput2C;

			const string input3 = "<p>Some text...</p> \n\t  " +
				"<pre title=\"Some title...\">	<code>   Some     text </code>\n</pre> \t\n  " +
				"<p>Some text...</p>"
				;
			const string targetOutput3A = input3;
			const string targetOutput3B = "<p>Some text...</p> " +
				"<pre title=\"Some title...\">	<code>   Some     text </code>\n</pre> " +
				"<p>Some text...</p>"
				;
			const string targetOutput3C = "<p>Some text...</p>" +
				"<pre title=\"Some title...\">	<code>   Some     text </code>\n</pre>" +
				"<p>Some text...</p>"
				;
			const string targetOutput3D = targetOutput3C;

			const string input4 = "<p> I'll   tell  you   my  story  with    <span>  5   slides </span> -  " +
				"<img src=\"\"> <span>	!  </span>	</p>";
			const string targetOutput4A = input4;
			const string targetOutput4B = "<p> I'll tell you my story with <span> 5 slides </span> - " +
				"<img src=\"\"> <span> ! </span> </p>";
			const string targetOutput4C = "<p>I'll tell you my story with <span> 5 slides </span> - " +
				"<img src=\"\"> <span> ! </span></p>";
			const string targetOutput4D = "<p>I'll tell you my story with <span>5 slides</span> - " +
				"<img src=\"\"> <span>!</span></p>";

			const string input5 = "<p>  An  <del>	 old \n</del>  <ins>	new  \n </ins>  embedded flash animation:  \n " +
				"<embed src=\"helloworld.swf\">	 !  </p>";
			const string targetOutput5A = input5;
			const string targetOutput5B = "<p> An <del> old </del> <ins> new </ins> embedded flash animation: " +
				"<embed src=\"helloworld.swf\"> ! </p>";
			const string targetOutput5C = "<p>An <del> old </del> <ins> new </ins> embedded flash animation: " +
				"<embed src=\"helloworld.swf\"> !</p>";
			const string targetOutput5D = "<p>An <del>old</del> <ins>new</ins> embedded flash animation: " +
				"<embed src=\"helloworld.swf\"> !</p>";

			// Act
			string output1A = keepingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1B = safeRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = mediumRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1D = aggressiveRemovingWhitespaceMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2B = safeRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = mediumRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2D = aggressiveRemovingWhitespaceMinifier.Minify(input2).MinifiedContent;

			string output3A = keepingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3B = safeRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3C = mediumRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3D = aggressiveRemovingWhitespaceMinifier.Minify(input3).MinifiedContent;

			string output4A = keepingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4B = safeRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4C = mediumRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4D = aggressiveRemovingWhitespaceMinifier.Minify(input4).MinifiedContent;

			string output5A = keepingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5B = safeRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5C = mediumRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5D = aggressiveRemovingWhitespaceMinifier.Minify(input5).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);
			Assert.Equal(targetOutput1D, output1D);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);
			Assert.Equal(targetOutput2D, output2D);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);
			Assert.Equal(targetOutput3D, output3D);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);
			Assert.Equal(targetOutput4D, output4D);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);
			Assert.Equal(targetOutput5D, output5D);
		}

		[Fact]
		public void WhitespaceMinificationInHtmlDocumentIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.None });
			var safeRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Safe });
			var mediumRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var aggressiveRemovingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive });

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
				"		<p>Some text...</p>\n" +
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
			const string targetOutputB = "<!-- meta name=\"GENERATOR\" content=\"Microsoft FrontPage 1.0\" -->" +
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
			const string targetOutputC = targetOutputB;
			const string targetOutputD = targetOutputB;

			// Act
			string outputA = keepingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputB = safeRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = mediumRemovingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputD = aggressiveRemovingWhitespaceMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
		}
	}
}