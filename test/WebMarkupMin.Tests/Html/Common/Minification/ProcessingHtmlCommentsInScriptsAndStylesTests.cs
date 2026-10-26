using System;

using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class ProcessingHtmlCommentsInScriptsAndStylesTests : IDisposable
	{
		private HtmlMinifier _minifier;
		private HtmlMinifier _removingWhitespaceMinifier;
		private HtmlMinifier _removingHtmlCommentsMinifier;


		public ProcessingHtmlCommentsInScriptsAndStylesTests()
		{
			_minifier = new HtmlMinifier(new HtmlMinificationSettings(true));
			_removingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			_removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlCommentsFromScriptsAndStyles = true });
		}


		[Fact]
		public void ProcessingSinglelineHtmlCommentsInStyleTagsWithSimpleContent()
		{
			// Arrange
			const string input = "<style type=\"text/css\"><!--\tp { color: #f00; } --></style>";
			const string targetOutputA = input;
			const string targetOutputB = "<style type=\"text/css\"><!--p { color: #f00; }--></style>";
			const string targetOutputC = "<style type=\"text/css\">\tp { color: #f00; } </style>";

			// Act
			string outputA = _minifier.Minify(input).MinifiedContent;
			string outputB = _removingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = _removingHtmlCommentsMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
		}

		[Fact]
		public void ProcessingSinglelineHtmlCommentsInStyleTagsWithContentContainingCommentBeginPart()
		{
			// Arrange
			const string input = "<style type=\"text/css\">\tp::before { content: \"<!--\" } </style>";
			const string targetOutputA = input;
			const string targetOutputB = "<style type=\"text/css\">p::before { content: \"<!--\" }</style>";
			const string targetOutputC = input;

			// Act
			string outputA = _minifier.Minify(input).MinifiedContent;
			string outputB = _removingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = _removingHtmlCommentsMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
		}

		[Fact]
		public void ProcessingThreelineHtmlCommentsInStyleTagsWithSimpleContent()
		{
			// Arrange
			const string input = "<style type=\"text/css\">\r\n" +
				"<!--\r\n" +
				"	p { color: #f00; } \r\n" +
				"-->\r\n" +
				"</style>"
				;
			const string targetOutputA = "<style type=\"text/css\">" +
				"<!--" +
				"	p { color: #f00; } " +
				"-->" +
				"</style>"
				;
			const string targetOutputB = "<style type=\"text/css\">" +
				"<!--" +
				"p { color: #f00; }" +
				"-->" +
				"</style>"
				;
			const string targetOutputC = "<style type=\"text/css\">" +
				"	p { color: #f00; } " +
				"</style>"
				;

			// Act
			string outputA = _minifier.Minify(input).MinifiedContent;
			string outputB = _removingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = _removingHtmlCommentsMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
		}

		[Fact]
		public void ProcessingSinglelineHtmlCommentsInScriptTagsWithSimpleContent()
		{
			// Arrange
			const string input = "<script>\r\n" +
				"<!-- alert('Hooray!'); -->\r\n" +
				"</script>"
				;
			const string targetOutputA = "<script>" +
				"<!--\r\n" +
				"\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutputB = "<script>" +
				"<!--\r\n" +
				"\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutputC = "<script></script>";

			// Act
			string outputA = _minifier.Minify(input).MinifiedContent;
			string outputB = _removingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = _removingHtmlCommentsMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
		}

		[Fact]
		public void ProcessingSinglelineHtmlCommentsInScriptTagsWithContentContainingCommentEndPart()
		{
			// Arrange
			const string input = "<script>\r\n" +
				"<!-- alert('-->'); -->\r\n" +
				"</script>"
				;
			const string targetOutputA = "<script>" +
				"<!--\r\n" +
				"\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutputB = "<script>" +
				"<!--\r\n" +
				"\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutputC = "<script></script>";

			// Act
			string outputA = _minifier.Minify(input).MinifiedContent;
			string outputB = _removingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = _removingHtmlCommentsMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
		}

		[Fact]
		public void ProcessingTwolineHtmlCommentsInScriptTagsWithSimpleContent()
		{
			// Arrange
			const string input1 = "<script>\r\n" +
				"<!--\r\n" +
				"	alert('Hooray!'); -->\r\n" +
				"</script>"
				;
			const string targetOutput1A = "<script>" +
				"<!--\r\n" +
				"\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput1B = "<script>" +
				"<!--\r\n" +
				"\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput1C = "<script></script>";

			const string input2 = "<script>\r\n" +
				"<!--\r\n" +
				"	alert('Hooray!'); // -->\r\n" +
				"</script>"
				;
			const string targetOutput2A = "<script>" +
				"<!--\r\n" +
				"	alert('Hooray!'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput2B = "<script>" +
				"<!--\r\n" +
				"alert('Hooray!');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput2C = "<script>" +
				"	alert('Hooray!'); " +
				"</script>"
				;

			// Act
			string output1A = _minifier.Minify(input1).MinifiedContent;
			string output1B = _removingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = _removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;

			string output2A = _minifier.Minify(input2).MinifiedContent;
			string output2B = _removingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = _removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);
		}

		[Fact]
		public void ProcessingTwolineHtmlCommentsInScriptTagsWithContentContainingCommentEndPart()
		{
			// Arrange
			const string input1 = "<script>\r\n" +
				"<!--\r\n" +
				"	alert('-->'); -->\r\n" +
				"</script>"
				;
			const string targetOutput1A = "<script>" +
				"<!--\r\n" +
				"\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput1B = "<script>" +
				"<!--\r\n" +
				"\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput1C = "<script></script>";

			const string input2 = "<script>\r\n" +
				"<!--\r\n" +
				"	alert('-->'); // -->\r\n" +
				"</script>"
				;
			const string targetOutput2A = "<script>" +
				"<!--\r\n" +
				"	alert('-->'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput2B = "<script>" +
				"<!--\r\n" +
				"alert('-->');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput2C = "<script>" +
				"	alert('-->'); " +
				"</script>"
				;

			const string input3 = "<script>\r\n" +
				"<!--\r\n" +
				"	alert('-->'); // Shows message // -->\r\n" +
				"</script>"
				;
			const string targetOutput3A = "<script>" +
				"<!--\r\n" +
				"	alert('-->'); // Shows message \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput3B = "<script>" +
				"<!--\r\n" +
				"alert('-->'); // Shows message\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput3C = "<script>" +
				"	alert('-->'); // Shows message " +
				"</script>"
				;

			// Act
			string output1A = _minifier.Minify(input1).MinifiedContent;
			string output1B = _removingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = _removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;

			string output2A = _minifier.Minify(input2).MinifiedContent;
			string output2B = _removingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = _removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			string output3A = _minifier.Minify(input3).MinifiedContent;
			string output3B = _removingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3C = _removingHtmlCommentsMinifier.Minify(input3).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);
		}

		[Fact]
		public void ProcessingTwolineHtmlCommentsWithTextInScriptTags()
		{
			// Arrange
			const string input = "<script>\r\n" +
				"<!-- Броузеры ранних версий игнорируют этот код\r\n" +
				"	alert('Ура!'); // Здесь сокрытие кода прекращается -->\r\n" +
				"</script>"
				;
			const string targetOutputA = "<script>" +
				"<!--\r\n" +
				"	alert('Ура!'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutputB = "<script>" +
				"<!--\r\n" +
				"alert('Ура!');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutputC = "<script>" +
				"	alert('Ура!'); " +
				"</script>"
				;

			// Act
			string outputA = _minifier.Minify(input).MinifiedContent;
			string outputB = _removingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = _removingHtmlCommentsMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
		}

		[Fact]
		public void ProcessingThreelineHtmlCommentsInScriptTagsWithSimpleContent()
		{
			// Arrange
			const string input1 = "<script>\r\n" +
				"<!--\r\n" +
				"	alert('Hooray!'); \r\n" +
				"-->\r\n" +
				"</script>"
				;
			const string targetOutput1A = "<script>" +
				"<!--\r\n" +
				"	alert('Hooray!'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput1B = "<script>" +
				"<!--\r\n" +
				"alert('Hooray!');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput1C = "<script>" +
				"	alert('Hooray!'); " +
				"</script>"
				;

			const string input2 = "<script>\r\n" +
				"<!--//\r\n" +
				"	alert('Hooray!'); \r\n" +
				"//-->\r\n" +
				"</script>"
				;
			const string targetOutput2A = "<script>" +
				"<!--\r\n" +
				"	alert('Hooray!'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput2B = "<script>" +
				"<!--\r\n" +
				"alert('Hooray!');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput2C = "<script>" +
				"	alert('Hooray!'); " +
				"</script>"
				;

			const string input3 = "<script>\r\n" +
				"<!-- //\r\n" +
				"	alert('Hooray!'); \r\n" +
				"// -->\r\n" +
				"</script>"
				;
			const string targetOutput3A = "<script>" +
				"<!--\r\n" +
				"	alert('Hooray!'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput3B = "<script>" +
				"<!--\r\n" +
				"alert('Hooray!');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput3C = "<script>" +
				"	alert('Hooray!'); " +
				"</script>"
				;

			const string input4 = "<script>\r\n" +
				"//<!--\r\n" +
				"	alert('Hooray!'); \r\n" +
				"//-->\r\n" +
				"</script>"
				;
			const string targetOutput4A = "<script>" +
				"<!--\r\n" +
				"	alert('Hooray!'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput4B = "<script>" +
				"<!--\r\n" +
				"alert('Hooray!');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput4C = "<script>" +
				"	alert('Hooray!'); " +
				"</script>"
				;

			const string input5 = "<script>\r\n" +
				"// <!--\r\n" +
				"	alert('Hooray!'); \r\n" +
				"// -->\r\n" +
				"</script>"
				;
			const string targetOutput5A = "<script>" +
				"<!--\r\n" +
				"	alert('Hooray!'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput5B = "<script>" +
				"<!--\r\n" +
				"alert('Hooray!');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput5C = "<script>" +
				"	alert('Hooray!'); " +
				"</script>"
				;

			const string input6 = "<script>\r\n" +
				"<!-- alert('Test 1');\r\n" +
				"	alert('Test 2'); \r\n" +
				"	alert('Test 3'); -->\r\n" +
				"</script>"
				;
			const string targetOutput6A = "<script>" +
				"<!--\r\n" +
				"	alert('Test 2'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput6B = "<script>" +
				"<!--\r\n" +
				"alert('Test 2');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput6C = "<script>" +
				"	alert('Test 2'); " +
				"</script>"
				;

			// Act
			string output1A = _minifier.Minify(input1).MinifiedContent;
			string output1B = _removingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = _removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;

			string output2A = _minifier.Minify(input2).MinifiedContent;
			string output2B = _removingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = _removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			string output3A = _minifier.Minify(input3).MinifiedContent;
			string output3B = _removingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3C = _removingHtmlCommentsMinifier.Minify(input3).MinifiedContent;

			string output4A = _minifier.Minify(input4).MinifiedContent;
			string output4B = _removingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4C = _removingHtmlCommentsMinifier.Minify(input4).MinifiedContent;

			string output5A = _minifier.Minify(input5).MinifiedContent;
			string output5B = _removingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5C = _removingHtmlCommentsMinifier.Minify(input5).MinifiedContent;

			string output6A = _minifier.Minify(input6).MinifiedContent;
			string output6B = _removingWhitespaceMinifier.Minify(input6).MinifiedContent;
			string output6C = _removingHtmlCommentsMinifier.Minify(input6).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);

			Assert.Equal(targetOutput6A, output6A);
			Assert.Equal(targetOutput6B, output6B);
			Assert.Equal(targetOutput6C, output6C);
		}

		[Fact]
		public void ProcessingThreelineHtmlCommentsInScriptTagsWithContentContainingCommentEndPart()
		{
			// Arrange
			const string input1 = "<script>\r\n" +
				"<!--\r\n" +
				"	alert('-->'); \r\n" +
				"-->\r\n" +
				"</script>"
				;
			const string targetOutput1A = "<script>" +
				"<!--\r\n" +
				"	alert('-->'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput1B = "<script>" +
				"<!--\r\n" +
				"alert('-->');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput1C = "<script>" +
				"	alert('-->'); " +
				"</script>"
				;

			const string input2 = "<script>\r\n" +
				"<!-- alert('1 -->');\r\n" +
				"	alert('2 -->'); \r\n" +
				"	alert('3 -->'); -->\r\n" +
				"</script>"
				;
			const string targetOutput2A = "<script>" +
				"<!--\r\n" +
				"	alert('2 -->'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput2B = "<script>" +
				"<!--\r\n" +
				"alert('2 -->');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput2C = "<script>" +
				"	alert('2 -->'); " +
				"</script>"
				;

			// Act
			string output1A = _minifier.Minify(input1).MinifiedContent;
			string output1B = _removingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = _removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;

			string output2A = _minifier.Minify(input2).MinifiedContent;
			string output2B = _removingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = _removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);
		}

		[Fact]
		public void ProcessingThreelineHtmlCommentsWithTextInScriptTags()
		{
			// Arrange
			const string input1 = "<script>\r\n" +
				"<!-- Earlier versions of browsers ignore this code\r\n" +
				"	alert('Hooray!'); \r\n" +
				"Hiding code stops here -->\r\n" +
				"</script>"
				;
			const string targetOutput1A = "<script>" +
				"<!--\r\n" +
				"	alert('Hooray!'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput1B = "<script>" +
				"<!--\r\n" +
				"alert('Hooray!');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput1C = "<script>" +
				"	alert('Hooray!'); " +
				"</script>"
				;

			const string input2 = "<script>\r\n" +
				"<!-- Earlier versions of browsers ignore this code\r\n" +
				"	alert('Hooray!'); \r\n" +
				"// Hiding code stops here -->\r\n" +
				"</script>"
				;
			const string targetOutput2A = "<script>" +
				"<!--\r\n" +
				"	alert('Hooray!'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput2B = "<script>" +
				"<!--\r\n" +
				"alert('Hooray!');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput2C = "<script>" +
				"	alert('Hooray!'); " +
				"</script>"
				;

			const string input3 = "<script>\r\n" +
				"<!-- // Earlier versions of browsers ignore this code\r\n" +
				"	alert('Hooray!'); \r\n" +
				"// Hiding code stops here -->\r\n" +
				"</script>"
				;
			const string targetOutput3A = "<script>" +
				"<!--\r\n" +
				"	alert('Hooray!'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput3B = "<script>" +
				"<!--\r\n" +
				"alert('Hooray!');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput3C = "<script>" +
				"	alert('Hooray!'); " +
				"</script>"
				;

			const string input4 = "<script>\r\n" +
				"//<!-- Earlier versions of browsers ignore this code\r\n" +
				"	alert('Hooray!'); \r\n" +
				"// Hiding code stops here -->\r\n" +
				"</script>"
				;
			const string targetOutput4A = "<script>" +
				"<!--\r\n" +
				"	alert('Hooray!'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput4B = "<script>" +
				"<!--\r\n" +
				"alert('Hooray!');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput4C = "<script>" +
				"	alert('Hooray!'); " +
				"</script>"
				;

			const string input5 = "<script>\r\n" +
				"// <!-- Earlier versions of browsers ignore this code\r\n" +
				"	alert('Hooray!'); \r\n" +
				"// Hiding code stops here -->\r\n" +
				"</script>"
				;
			const string targetOutput5A = "<script>" +
				"<!--\r\n" +
				"	alert('Hooray!'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput5B = "<script>" +
				"<!--\r\n" +
				"alert('Hooray!');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput5C = "<script>" +
				"	alert('Hooray!'); " +
				"</script>"
				;

			// Act
			string output1A = _minifier.Minify(input1).MinifiedContent;
			string output1B = _removingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = _removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;

			string output2A = _minifier.Minify(input2).MinifiedContent;
			string output2B = _removingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = _removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			string output3A = _minifier.Minify(input3).MinifiedContent;
			string output3B = _removingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3C = _removingHtmlCommentsMinifier.Minify(input3).MinifiedContent;

			string output4A = _minifier.Minify(input4).MinifiedContent;
			string output4B = _removingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4C = _removingHtmlCommentsMinifier.Minify(input4).MinifiedContent;

			string output5A = _minifier.Minify(input5).MinifiedContent;
			string output5B = _removingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5C = _removingHtmlCommentsMinifier.Minify(input5).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);

			Assert.Equal(targetOutput5A, output5A);
			Assert.Equal(targetOutput5B, output5B);
			Assert.Equal(targetOutput5C, output5C);
		}

		[Fact]
		public void ProcessingHtmlCommentsInScriptTagsWithJsContentTypes()
		{
			// Arrange
			const string input1 = "<script type=\"text/javascript\">\r\n" +
				"<!--\r\n" +
				"	alert('Ура!'); \r\n" +
				"-->\r\n" +
				"</script>"
				;
			const string targetOutput1A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"	alert('Ура!'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput1B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('Ура!');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput1C = "<script type=\"text/javascript\">" +
				"	alert('Ура!'); " +
				"</script>"
				;

			const string input2 = "<script type=\"text/ecmascript\">\r\n" +
				"<!--\r\n" +
				"	alert('Ура!'); \r\n" +
				"-->\r\n" +
				"</script>"
				;
			const string targetOutput2A = "<script type=\"text/ecmascript\">" +
				"<!--\r\n" +
				"	alert('Ура!'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput2B = "<script type=\"text/ecmascript\">" +
				"<!--\r\n" +
				"alert('Ура!');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput2C = "<script type=\"text/ecmascript\">" +
				"	alert('Ура!'); " +
				"</script>"
				;

			const string input3 = "<script type=\"application/ecmascript\">\r\n" +
				"<!--\r\n" +
				"	alert('Ура!'); \r\n" +
				"-->\r\n" +
				"</script>"
				;
			const string targetOutput3A = "<script type=\"application/ecmascript\">" +
				"<!--\r\n" +
				"	alert('Ура!'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput3B = "<script type=\"application/ecmascript\">" +
				"<!--\r\n" +
				"alert('Ура!');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput3C = "<script type=\"application/ecmascript\">" +
				"	alert('Ура!'); " +
				"</script>"
				;

			const string input4 = "<script type=\"application/javascript\">\r\n" +
				"<!--\r\n" +
				"	alert('Ура!'); \r\n" +
				"-->\r\n" +
				"</script>"
				;
			const string targetOutput4A = "<script type=\"application/javascript\">" +
				"<!--\r\n" +
				"	alert('Ура!'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput4B = "<script type=\"application/javascript\">" +
				"<!--\r\n" +
				"alert('Ура!');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput4C = "<script type=\"application/javascript\">" +
				"	alert('Ура!'); " +
				"</script>"
				;

			// Act
			string output1A = _minifier.Minify(input1).MinifiedContent;
			string output1B = _removingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = _removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;

			string output2A = _minifier.Minify(input2).MinifiedContent;
			string output2B = _removingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = _removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			string output3A = _minifier.Minify(input3).MinifiedContent;
			string output3B = _removingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3C = _removingHtmlCommentsMinifier.Minify(input3).MinifiedContent;

			string output4A = _minifier.Minify(input4).MinifiedContent;
			string output4B = _removingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4C = _removingHtmlCommentsMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);
			Assert.Equal(targetOutput3C, output3C);

			Assert.Equal(targetOutput4A, output4A);
			Assert.Equal(targetOutput4B, output4B);
			Assert.Equal(targetOutput4C, output4C);
		}

		[Fact]
		public void ProcessingHtmlCommentsInScriptTagsWithVbsLanguage()
		{
			// Arrange
			const string input = "<script language=\"VBScript\">\r\n" +
				"<!--\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"-->\r\n" +
				"</script>"
				;
			const string targetOutputA = "<script language=\"VBScript\">" +
				"<!--\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"-->" +
				"</script>"
				;
			const string targetOutputB = "<script language=\"VBScript\">" +
				"<!--\r\n" +
				"Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"-->" +
				"</script>"
				;
			const string targetOutputC = "<script language=\"VBScript\">" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function" +
				"</script>"
				;

			// Act
			string outputA = _minifier.Minify(input).MinifiedContent;
			string outputB = _removingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = _removingHtmlCommentsMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
		}

		[Fact]
		public void ProcessingHtmlCommentsInScriptTagsWithVbsContentType()
		{
			// Arrange
			const string input = "<script type=\"text/vbscript\">\r\n" +
				"<!--\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"-->\r\n" +
				"</script>"
				;
			const string targetOutputA = "<script type=\"text/vbscript\">" +
				"<!--\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"-->" +
				"</script>"
				;
			const string targetOutputB = "<script type=\"text/vbscript\">" +
				"<!--\r\n" +
				"Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"-->" +
				"</script>"
				;
			const string targetOutputC = "<script type=\"text/vbscript\">" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function" +
				"</script>"
				;

			// Act
			string outputA = _minifier.Minify(input).MinifiedContent;
			string outputB = _removingWhitespaceMinifier.Minify(input).MinifiedContent;
			string outputC = _removingHtmlCommentsMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
		}

		[Fact]
		public void ProcessingHtmlCommentsInScriptTagsWithJsonContentTypes()
		{
			// Arrange
			const string input1 = "<script type=\"application/json\">\r\n" +
				"<!--\r\n" +
				"{\r\n" +
				"  \"vars\" : {\r\n" +
				"    \"gtag_id\": \"UA-39595497-1\",\r\n" +
				"    \"config\" : {\r\n" +
				"      \"UA-39595497-1\": { \"groups\": \"default\" }\r\n" +
				"    }\r\n" +
				"  }\r\n" +
				"}\r\n" +
				"-->\r\n" +
				"</script>"
				;
			const string targetOutput1A = "<script type=\"application/json\">" +
				"<!--\r\n" +
				"{\r\n" +
				"  \"vars\" : {\r\n" +
				"    \"gtag_id\": \"UA-39595497-1\",\r\n" +
				"    \"config\" : {\r\n" +
				"      \"UA-39595497-1\": { \"groups\": \"default\" }\r\n" +
				"    }\r\n" +
				"  }\r\n" +
				"}\r\n" +
				"-->" +
				"</script>"
				;
			const string targetOutput1B = "<script type=\"application/json\">" +
				"<!--\r\n" +
				"{\r\n" +
				"  \"vars\" : {\r\n" +
				"    \"gtag_id\": \"UA-39595497-1\",\r\n" +
				"    \"config\" : {\r\n" +
				"      \"UA-39595497-1\": { \"groups\": \"default\" }\r\n" +
				"    }\r\n" +
				"  }\r\n" +
				"}\r\n" +
				"-->" +
				"</script>"
				;
			const string targetOutput1C = "<script type=\"application/json\">" +
				"{\r\n" +
				"  \"vars\" : {\r\n" +
				"    \"gtag_id\": \"UA-39595497-1\",\r\n" +
				"    \"config\" : {\r\n" +
				"      \"UA-39595497-1\": { \"groups\": \"default\" }\r\n" +
				"    }\r\n" +
				"  }\r\n" +
				"}" +
				"</script>"
				;

			const string input2 = "<script type=\"application/ld+json\">\r\n" +
				"<!--\r\n" +
				"{\r\n" +
				"	\"@context\": \"http:\\/\\/schema.org\",\r\n" +
				"	\"@type\": \"Article\",\r\n" +
				"	\"mainEntityOfPage\": {\r\n" +
				"		\"@type\": \"WebPage\",\r\n" +
				"		\"@id\": \"https:\\/\\/habr.com\\/ru\\/post\\/306484\\/\"\r\n" +
				"	},\r\n" +
				"	\"headline\": \"Переходим на WebMarkupMin 2.X\",\r\n" +
				"	\"datePublished\": \"2016-07-26T22:47:45+03:00\",\r\n" +
				"	\"dateModified\": \"2016-07-27T09:40:50+03:00\",\r\n" +
				"	\"author\": {\r\n" +
				"		\"@type\": \"Person\",\r\n" +
				"		\"name\": \"Андрей Тарицын\"\r\n" +
				"	},\r\n" +
				"	\"publisher\": {\r\n" +
				"		\"@type\": \"Organization\",\r\n" +
				"		\"name\": \"Habr\",\r\n" +
				"		\"logo\": {\r\n" +
				"			\"@type\": \"ImageObject\",\r\n" +
				"			\"url\": \"https:\\/\\/dr.habracdn.net\\/habrcom\\/images\\/habr_google.png\"\r\n" +
				"		}\r\n" +
				"	},\r\n" +
				"	\"description\": \"Весной прошлого года, когда ASP.NET 5 был еще в стадии Beta 3, " +
				"я начал получать от пользователей письма с просьбами сделать WebMarkupMin совместимым " +
				"с DNX 4.5.1...\",\r\n" +
				"	\"image\": [\r\n" +
				"		\"https:\\/\\/habrastorage.org\\/getpro\\/habr\\/post_images\\/560\\/f32\\/1b2\\/560f321b29f23528ac5ccd18035783d1.png\"\r\n" +
				"	]\r\n" +
				"}\r\n" +
				"-->\r\n" +
				"</script>"
				;
			const string targetOutput2A = "<script type=\"application/ld+json\">" +
				"<!--\r\n" +
				"{\r\n" +
				"	\"@context\": \"http:\\/\\/schema.org\",\r\n" +
				"	\"@type\": \"Article\",\r\n" +
				"	\"mainEntityOfPage\": {\r\n" +
				"		\"@type\": \"WebPage\",\r\n" +
				"		\"@id\": \"https:\\/\\/habr.com\\/ru\\/post\\/306484\\/\"\r\n" +
				"	},\r\n" +
				"	\"headline\": \"Переходим на WebMarkupMin 2.X\",\r\n" +
				"	\"datePublished\": \"2016-07-26T22:47:45+03:00\",\r\n" +
				"	\"dateModified\": \"2016-07-27T09:40:50+03:00\",\r\n" +
				"	\"author\": {\r\n" +
				"		\"@type\": \"Person\",\r\n" +
				"		\"name\": \"Андрей Тарицын\"\r\n" +
				"	},\r\n" +
				"	\"publisher\": {\r\n" +
				"		\"@type\": \"Organization\",\r\n" +
				"		\"name\": \"Habr\",\r\n" +
				"		\"logo\": {\r\n" +
				"			\"@type\": \"ImageObject\",\r\n" +
				"			\"url\": \"https:\\/\\/dr.habracdn.net\\/habrcom\\/images\\/habr_google.png\"\r\n" +
				"		}\r\n" +
				"	},\r\n" +
				"	\"description\": \"Весной прошлого года, когда ASP.NET 5 был еще в стадии Beta 3, " +
				"я начал получать от пользователей письма с просьбами сделать WebMarkupMin совместимым " +
				"с DNX 4.5.1...\",\r\n" +
				"	\"image\": [\r\n" +
				"		\"https:\\/\\/habrastorage.org\\/getpro\\/habr\\/post_images\\/560\\/f32\\/1b2\\/560f321b29f23528ac5ccd18035783d1.png\"\r\n" +
				"	]\r\n" +
				"}\r\n" +
				"-->" +
				"</script>"
				;
			const string targetOutput2B = "<script type=\"application/ld+json\">" +
				"<!--\r\n" +
				"{\r\n" +
				"	\"@context\": \"http:\\/\\/schema.org\",\r\n" +
				"	\"@type\": \"Article\",\r\n" +
				"	\"mainEntityOfPage\": {\r\n" +
				"		\"@type\": \"WebPage\",\r\n" +
				"		\"@id\": \"https:\\/\\/habr.com\\/ru\\/post\\/306484\\/\"\r\n" +
				"	},\r\n" +
				"	\"headline\": \"Переходим на WebMarkupMin 2.X\",\r\n" +
				"	\"datePublished\": \"2016-07-26T22:47:45+03:00\",\r\n" +
				"	\"dateModified\": \"2016-07-27T09:40:50+03:00\",\r\n" +
				"	\"author\": {\r\n" +
				"		\"@type\": \"Person\",\r\n" +
				"		\"name\": \"Андрей Тарицын\"\r\n" +
				"	},\r\n" +
				"	\"publisher\": {\r\n" +
				"		\"@type\": \"Organization\",\r\n" +
				"		\"name\": \"Habr\",\r\n" +
				"		\"logo\": {\r\n" +
				"			\"@type\": \"ImageObject\",\r\n" +
				"			\"url\": \"https:\\/\\/dr.habracdn.net\\/habrcom\\/images\\/habr_google.png\"\r\n" +
				"		}\r\n" +
				"	},\r\n" +
				"	\"description\": \"Весной прошлого года, когда ASP.NET 5 был еще в стадии Beta 3, " +
				"я начал получать от пользователей письма с просьбами сделать WebMarkupMin совместимым " +
				"с DNX 4.5.1...\",\r\n" +
				"	\"image\": [\r\n" +
				"		\"https:\\/\\/habrastorage.org\\/getpro\\/habr\\/post_images\\/560\\/f32\\/1b2\\/560f321b29f23528ac5ccd18035783d1.png\"\r\n" +
				"	]\r\n" +
				"}\r\n" +
				"-->" +
				"</script>"
				;
			const string targetOutput2C = "<script type=\"application/ld+json\">" +
				"{\r\n" +
				"	\"@context\": \"http:\\/\\/schema.org\",\r\n" +
				"	\"@type\": \"Article\",\r\n" +
				"	\"mainEntityOfPage\": {\r\n" +
				"		\"@type\": \"WebPage\",\r\n" +
				"		\"@id\": \"https:\\/\\/habr.com\\/ru\\/post\\/306484\\/\"\r\n" +
				"	},\r\n" +
				"	\"headline\": \"Переходим на WebMarkupMin 2.X\",\r\n" +
				"	\"datePublished\": \"2016-07-26T22:47:45+03:00\",\r\n" +
				"	\"dateModified\": \"2016-07-27T09:40:50+03:00\",\r\n" +
				"	\"author\": {\r\n" +
				"		\"@type\": \"Person\",\r\n" +
				"		\"name\": \"Андрей Тарицын\"\r\n" +
				"	},\r\n" +
				"	\"publisher\": {\r\n" +
				"		\"@type\": \"Organization\",\r\n" +
				"		\"name\": \"Habr\",\r\n" +
				"		\"logo\": {\r\n" +
				"			\"@type\": \"ImageObject\",\r\n" +
				"			\"url\": \"https:\\/\\/dr.habracdn.net\\/habrcom\\/images\\/habr_google.png\"\r\n" +
				"		}\r\n" +
				"	},\r\n" +
				"	\"description\": \"Весной прошлого года, когда ASP.NET 5 был еще в стадии Beta 3, " +
				"я начал получать от пользователей письма с просьбами сделать WebMarkupMin совместимым " +
				"с DNX 4.5.1...\",\r\n" +
				"	\"image\": [\r\n" +
				"		\"https:\\/\\/habrastorage.org\\/getpro\\/habr\\/post_images\\/560\\/f32\\/1b2\\/560f321b29f23528ac5ccd18035783d1.png\"\r\n" +
				"	]\r\n" +
				"}" +
				"</script>"
				;

			// Act
			string output1A = _minifier.Minify(input1).MinifiedContent;
			string output1B = _removingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = _removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;

			string output2A = _minifier.Minify(input2).MinifiedContent;
			string output2B = _removingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = _removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
			Assert.Equal(targetOutput1C, output1C);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
			Assert.Equal(targetOutput2C, output2C);
		}


		#region IDisposable implementation

		public void Dispose()
		{
			_minifier = null;
			_removingWhitespaceMinifier = null;
			_removingHtmlCommentsMinifier = null;
		}

		#endregion
	}
}