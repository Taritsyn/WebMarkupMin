using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class ProcessingHtmlCommentsInScriptsAndStylesTests
	{
		[Fact]
		public void ProcessingHtmlCommentsInStyles()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));
			var removingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode= WhitespaceMinificationMode.Medium });
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlCommentsFromScriptsAndStyles = true });

			const string input1 = "<style type=\"text/css\"><!--\tp { color: #f00; } --></style>";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<style type=\"text/css\"><!--p { color: #f00; }--></style>";
			const string targetOutput1C = "<style type=\"text/css\">\tp { color: #f00; } </style>";


			const string input2 = "<style type=\"text/css\">\r\n" +
				"<!--\r\n" +
				"\tp { color: #f00; } " +
				"\r\n-->" +
				"\r\n</style>"
				;
			const string targetOutput2A = "<style type=\"text/css\">" +
				"<!--" +
				"\tp { color: #f00; } " +
				"-->" +
				"</style>"
				;
			const string targetOutput2B = "<style type=\"text/css\">" +
				"<!--" +
				"p { color: #f00; }" +
				"-->" +
				"</style>"
				;
			const string targetOutput2C = "<style type=\"text/css\">" +
				"\tp { color: #f00; } " +
				"</style>"
				;


			const string input3 = "<style type=\"text/css\">\tp::before { content: \"<!--\" } </style>";
			const string targetOutput3A = input3;
			const string targetOutput3B = "<style type=\"text/css\">p::before { content: \"<!--\" }</style>";
			const string targetOutput3C = input3;

			// Act
			string output1A = minifier.Minify(input1).MinifiedContent;
			string output1B = removingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;

			string output2A = minifier.Minify(input2).MinifiedContent;
			string output2B = removingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			string output3A = minifier.Minify(input3).MinifiedContent;
			string output3B = removingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3C = removingHtmlCommentsMinifier.Minify(input3).MinifiedContent;

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
		public void ProcessingHtmlCommentsInScripts()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));
			var removingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlCommentsFromScriptsAndStyles = true });

			const string input1 = "<script type=\"text/javascript\">\r\n" +
				"<!-- alert('Hooray!'); -->" +
				"\r\n</script>"
				;
			const string targetOutput1A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput1B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput1C = "<script type=\"text/javascript\"></script>";


			const string input2 = "<script type=\"text/javascript\">\r\n" +
				"<!-- alert('-->'); -->" +
				"\r\n</script>"
				;
			const string targetOutput2A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput2B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput2C = "<script type=\"text/javascript\"></script>";


			const string input3 = "<script type=\"text/javascript\">\r\n" +
				"<!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n-->" +
				"\r\n</script>"
				;
			const string targetOutput3A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput3B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput3C = "<script type=\"text/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input4 = "<script type=\"text/javascript\">\r\n" +
				"<!--\r\n" +
				"\talert('-->'); " +
				"\r\n-->" +
				"\r\n</script>"
				;
			const string targetOutput4A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('-->'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput4B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('-->');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput4C = "<script type=\"text/javascript\">" +
				"\talert('-->'); " +
				"</script>"
				;


			const string input5 = "<script type=\"text/javascript\">\r\n" +
				"<!-- alert('Test 1');\r\n" +
				"\talert('Test 2'); \r\n" +
				"\talert('Test 3'); -->" +
				"\r\n</script>"
				;
			const string targetOutput5A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('Test 2'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput5B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('Test 2');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput5C = "<script type=\"text/javascript\">" +
				"\talert('Test 2'); " +
				"</script>"
				;


			const string input6 = "<script type=\"text/javascript\">\r\n" +
				"<!-- alert('1 -->');\r\n" +
				"\talert('2 -->'); \r\n" +
				"\talert('3 -->'); -->" +
				"\r\n</script>"
				;
			const string targetOutput6A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('2 -->'); \r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput6B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('2 -->');\r\n" +
				"//-->" +
				"</script>"
				;
			const string targetOutput6C = "<script type=\"text/javascript\">" +
				"\talert('2 -->'); " +
				"</script>"
				;

			const string input7 = "<script type=\"text/javascript\">\r\n" +
				"<!--//\r\n" +
				"\talert('-->'); " +
				"\r\n//-->" +
				"\r\n</script>"
				;
			const string targetOutput7A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('-->'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput7B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('-->');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput7C = "<script type=\"text/javascript\">" +
				"\talert('-->'); " +
				"</script>"
				;


			const string input8 = "<script type=\"text/javascript\">\r\n" +
				"<!-- //\r\n" +
				"\talert('-->'); " +
				"\r\n// -->" +
				"\r\n</script>"
				;
			const string targetOutput8A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('-->'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput8B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('-->');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput8C = "<script type=\"text/javascript\">" +
				"\talert('-->'); " +
				"</script>"
				;


			const string input9 = "<script type=\"text/javascript\">\r\n" +
				"//<!--\r\n" +
				"\talert('-->'); " +
				"\r\n//-->" +
				"\r\n</script>"
				;
			const string targetOutput9A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('-->'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput9B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('-->');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput9C = "<script type=\"text/javascript\">" +
				"\talert('-->'); " +
				"</script>"
				;


			const string input10 = "<script type=\"text/javascript\">\r\n" +
				"// <!--\r\n" +
				"\talert('-->'); " +
				"\r\n// -->" +
				"\r\n</script>"
				;
			const string targetOutput10A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('-->'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput10B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('-->');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput10C = "<script type=\"text/javascript\">" +
				"\talert('-->'); " +
				"</script>"
				;


			const string input11 = "<script type=\"text/javascript\">\r\n" +
				"<!-- Earlier versions of browsers ignore this code\r\n" +
				"\talert('Hooray!'); " +
				"\r\nHiding code stops here -->\r\n" +
				"</script>"
				;
			const string targetOutput11A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput11B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput11C = "<script type=\"text/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input12 = "<script type=\"text/javascript\">\r\n" +
				"<!-- Earlier versions of browsers ignore this code\r\n" +
				"\talert('Hooray!'); " +
				"\r\n// Hiding code stops here -->\r\n" +
				"</script>"
				;
			const string targetOutput12A = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput12B = "<script type=\"text/javascript\">" +
				"<!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput12C = "<script type=\"text/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input13 = "<script type=\"text/ecmascript\">\r\n" +
				"<!-- // Earlier versions of browsers ignore this code\r\n" +
				"\talert('Hooray!'); " +
				"\r\n// Hiding code stops here -->\r\n" +
				"</script>"
				;
			const string targetOutput13A = "<script type=\"text/ecmascript\">" +
				"<!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput13B = "<script type=\"text/ecmascript\">" +
				"<!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput13C = "<script type=\"text/ecmascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input14 = "<script type=\"application/ecmascript\">\r\n" +
				"//<!-- Earlier versions of browsers ignore this code\r\n" +
				"\talert('Hooray!'); " +
				"\r\n// Hiding code stops here -->\r\n" +
				"</script>"
				;
			const string targetOutput14A = "<script type=\"application/ecmascript\">" +
				"<!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput14B = "<script type=\"application/ecmascript\">" +
				"<!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput14C = "<script type=\"application/ecmascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input15 = "<script type=\"application/javascript\">\r\n" +
				"// <!-- Earlier versions of browsers ignore this code\r\n" +
				"\talert('Hooray!'); " +
				"\r\n// Hiding code stops here -->\r\n" +
				"</script>"
				;
			const string targetOutput15A = "<script type=\"application/javascript\">" +
				"<!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput15B = "<script type=\"application/javascript\">" +
				"<!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//-->" +
				"</script>"
				;
			const string targetOutput15C = "<script type=\"application/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input16 = "<script type=\"text/vbscript\">\r\n" +
				"<!--\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"-->\r\n" +
				"</script>"
				;
			const string targetOutput16A = "<script type=\"text/vbscript\">" +
				"<!--\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"-->" +
				"</script>"
				;
			const string targetOutput16B = "<script type=\"text/vbscript\">" +
				"<!--\r\n" +
				"Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"-->" +
				"</script>"
				;
			const string targetOutput16C = "<script type=\"text/vbscript\">" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function" +
				"</script>"
				;


			const string input17 = "<script language=\"VBScript\">\r\n" +
				"<!--\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"-->\r\n" +
				"</script>"
				;
			const string targetOutput17A = "<script language=\"VBScript\">" +
				"<!--\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"-->" +
				"</script>"
				;
			const string targetOutput17B = "<script language=\"VBScript\">" +
				"<!--\r\n" +
				"Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"-->" +
				"</script>"
				;
			const string targetOutput17C = "<script language=\"VBScript\">" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function" +
				"</script>"
				;


			const string input18 = "<script type=\"application/json\">\r\n" +
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
			const string targetOutput18A = "<script type=\"application/json\">" +
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
			const string targetOutput18B = "<script type=\"application/json\">" +
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
			const string targetOutput18C = "<script type=\"application/json\">" +
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


			const string input19 = "<script type=\"application/ld+json\">\r\n" +
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
			const string targetOutput19A = "<script type=\"application/ld+json\">" +
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
			const string targetOutput19B = "<script type=\"application/ld+json\">" +
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
			const string targetOutput19C = "<script type=\"application/ld+json\">" +
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
			string output1A = minifier.Minify(input1).MinifiedContent;
			string output1B = removingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;

			string output2A = minifier.Minify(input2).MinifiedContent;
			string output2B = removingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			string output3A = minifier.Minify(input3).MinifiedContent;
			string output3B = removingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3C = removingHtmlCommentsMinifier.Minify(input3).MinifiedContent;

			string output4A = minifier.Minify(input4).MinifiedContent;
			string output4B = removingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4C = removingHtmlCommentsMinifier.Minify(input4).MinifiedContent;

			string output5A = minifier.Minify(input5).MinifiedContent;
			string output5B = removingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5C = removingHtmlCommentsMinifier.Minify(input5).MinifiedContent;

			string output6A = minifier.Minify(input6).MinifiedContent;
			string output6B = removingWhitespaceMinifier.Minify(input6).MinifiedContent;
			string output6C = removingHtmlCommentsMinifier.Minify(input6).MinifiedContent;

			string output7A = minifier.Minify(input7).MinifiedContent;
			string output7B = removingWhitespaceMinifier.Minify(input7).MinifiedContent;
			string output7C = removingHtmlCommentsMinifier.Minify(input7).MinifiedContent;

			string output8A = minifier.Minify(input8).MinifiedContent;
			string output8B = removingWhitespaceMinifier.Minify(input8).MinifiedContent;
			string output8C = removingHtmlCommentsMinifier.Minify(input8).MinifiedContent;

			string output9A = minifier.Minify(input9).MinifiedContent;
			string output9B = removingWhitespaceMinifier.Minify(input9).MinifiedContent;
			string output9C = removingHtmlCommentsMinifier.Minify(input9).MinifiedContent;

			string output10A = minifier.Minify(input10).MinifiedContent;
			string output10B = removingWhitespaceMinifier.Minify(input10).MinifiedContent;
			string output10C = removingHtmlCommentsMinifier.Minify(input10).MinifiedContent;

			string output11A = minifier.Minify(input11).MinifiedContent;
			string output11B = removingWhitespaceMinifier.Minify(input11).MinifiedContent;
			string output11C = removingHtmlCommentsMinifier.Minify(input11).MinifiedContent;

			string output12A = minifier.Minify(input12).MinifiedContent;
			string output12B = removingWhitespaceMinifier.Minify(input12).MinifiedContent;
			string output12C = removingHtmlCommentsMinifier.Minify(input12).MinifiedContent;

			string output13A = minifier.Minify(input13).MinifiedContent;
			string output13B = removingWhitespaceMinifier.Minify(input13).MinifiedContent;
			string output13C = removingHtmlCommentsMinifier.Minify(input13).MinifiedContent;

			string output14A = minifier.Minify(input14).MinifiedContent;
			string output14B = removingWhitespaceMinifier.Minify(input14).MinifiedContent;
			string output14C = removingHtmlCommentsMinifier.Minify(input14).MinifiedContent;

			string output15A = minifier.Minify(input15).MinifiedContent;
			string output15B = removingWhitespaceMinifier.Minify(input15).MinifiedContent;
			string output15C = removingHtmlCommentsMinifier.Minify(input15).MinifiedContent;

			string output16A = minifier.Minify(input16).MinifiedContent;
			string output16B = removingWhitespaceMinifier.Minify(input16).MinifiedContent;
			string output16C = removingHtmlCommentsMinifier.Minify(input16).MinifiedContent;

			string output17A = minifier.Minify(input17).MinifiedContent;
			string output17B = removingWhitespaceMinifier.Minify(input17).MinifiedContent;
			string output17C = removingHtmlCommentsMinifier.Minify(input17).MinifiedContent;

			string output18A = minifier.Minify(input18).MinifiedContent;
			string output18B = removingWhitespaceMinifier.Minify(input18).MinifiedContent;
			string output18C = removingHtmlCommentsMinifier.Minify(input18).MinifiedContent;

			string output19A = minifier.Minify(input19).MinifiedContent;
			string output19B = removingWhitespaceMinifier.Minify(input19).MinifiedContent;
			string output19C = removingHtmlCommentsMinifier.Minify(input19).MinifiedContent;

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

			Assert.Equal(targetOutput7A, output7A);
			Assert.Equal(targetOutput7B, output7B);
			Assert.Equal(targetOutput7C, output7C);

			Assert.Equal(targetOutput8A, output8A);
			Assert.Equal(targetOutput8B, output8B);
			Assert.Equal(targetOutput8C, output8C);

			Assert.Equal(targetOutput9A, output9A);
			Assert.Equal(targetOutput9B, output9B);
			Assert.Equal(targetOutput9C, output9C);

			Assert.Equal(targetOutput10A, output10A);
			Assert.Equal(targetOutput10B, output10B);
			Assert.Equal(targetOutput10C, output10C);

			Assert.Equal(targetOutput11A, output11A);
			Assert.Equal(targetOutput11B, output11B);
			Assert.Equal(targetOutput11C, output11C);

			Assert.Equal(targetOutput12A, output12A);
			Assert.Equal(targetOutput12B, output12B);
			Assert.Equal(targetOutput12C, output12C);

			Assert.Equal(targetOutput13A, output13A);
			Assert.Equal(targetOutput13B, output13B);
			Assert.Equal(targetOutput13C, output13C);

			Assert.Equal(targetOutput14A, output14A);
			Assert.Equal(targetOutput14B, output14B);
			Assert.Equal(targetOutput14C, output14C);

			Assert.Equal(targetOutput15A, output15A);
			Assert.Equal(targetOutput15B, output15B);
			Assert.Equal(targetOutput15C, output15C);

			Assert.Equal(targetOutput16A, output16A);
			Assert.Equal(targetOutput16B, output16B);
			Assert.Equal(targetOutput16C, output16C);

			Assert.Equal(targetOutput17A, output17A);
			Assert.Equal(targetOutput17B, output17B);
			Assert.Equal(targetOutput17C, output17C);

			Assert.Equal(targetOutput18A, output18A);
			Assert.Equal(targetOutput18B, output18B);
			Assert.Equal(targetOutput18C, output18C);

			Assert.Equal(targetOutput19A, output19A);
			Assert.Equal(targetOutput19B, output19B);
			Assert.Equal(targetOutput19C, output19C);
		}
	}
}