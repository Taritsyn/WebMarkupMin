using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Common.Minification
{
	public class ProcessingCdataSectionsInScriptsAndStylesTests
	{
		[Fact]
		public void ProcessingCdataSectionsInStylesIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));
			var removingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var removingCdataSectionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveCdataSectionsFromScriptsAndStyles = true });

			const string input1 = "<style type=\"text/css\">\r\n" +
				"/*<![CDATA[*/\r\n" +
				"\tp { color: #f00; } " +
				"\r\n/*]]>*/" +
				"\r\n</style>"
				;
			const string targetOutput1A = "<style type=\"text/css\">" +
				"/*<![CDATA[*/" +
				"\tp { color: #f00; } " +
				"/*]]>*/" +
				"</style>"
				;
			const string targetOutput1B = "<style type=\"text/css\">" +
				"/*<![CDATA[*/" +
				"p { color: #f00; }" +
				"/*]]>*/" +
				"</style>"
				;
			const string targetOutput1C = "<style type=\"text/css\">" +
				"\tp { color: #f00; } " +
				"</style>"
				;


			const string input2 = "<style type=\"text/css\">\r\n" +
				"/* <![CDATA[ */\r\n" +
				"\tp { color: #f00; } " +
				"\r\n/* ]]> */" +
				"\r\n</style>"
				;
			const string targetOutput2A = "<style type=\"text/css\">" +
				"/*<![CDATA[*/" +
				"\tp { color: #f00; } " +
				"/*]]>*/" +
				"</style>"
				;
			const string targetOutput2B = "<style type=\"text/css\">" +
				"/*<![CDATA[*/" +
				"p { color: #f00; }" +
				"/*]]>*/" +
				"</style>"
				;
			const string targetOutput2C = "<style type=\"text/css\">" +
				"\tp { color: #f00; } " +
				"</style>"
				;


			const string input3 = "<style type=\"text/css\">\r\n" +
				"<!--/*--><![CDATA[/*><!--*/\r\n" +
				"\tp { color: #f00; } " +
				"\r\n/*]]>*/-->" +
				"\r\n</style>"
				;
			const string targetOutput3A = "<style type=\"text/css\">" +
				"<!--/*--><![CDATA[/*><!--*/" +
				"\tp { color: #f00; } " +
				"/*]]>*/-->" +
				"</style>"
				;
			const string targetOutput3B = "<style type=\"text/css\">" +
				"<!--/*--><![CDATA[/*><!--*/" +
				"p { color: #f00; }" +
				"/*]]>*/-->" +
				"</style>"
				;
			const string targetOutput3C = "<style type=\"text/css\">" +
				"\tp { color: #f00; } " +
				"</style>"
				;


			const string input4 = "<style type=\"text/css\">\r\n" +
				"<!-- /* --><![CDATA[ /* ><!-- */\r\n" +
				"\tp { color: #f00; } " +
				"\r\n/* ]]> */ -->" +
				"\r\n</style>"
				;
			const string targetOutput4A = "<style type=\"text/css\">" +
				"<!--/*--><![CDATA[/*><!--*/" +
				"\tp { color: #f00; } " +
				"/*]]>*/-->" +
				"</style>"
				;
			const string targetOutput4B = "<style type=\"text/css\">" +
				"<!--/*--><![CDATA[/*><!--*/" +
				"p { color: #f00; }" +
				"/*]]>*/-->" +
				"</style>"
				;
			const string targetOutput4C = "<style type=\"text/css\">" +
				"\tp { color: #f00; } " +
				"</style>"
				;


			const string input5 = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 100\">\r\n" +
				"	<style>\r\n" +
				"	/* <![CDATA[ */\r\n" +
				"	circle {\r\n" +
				"		fill: orange;\r\n" +
				"		stroke: black;\r\n" +
				"		stroke-width: 10px; // Note that the value of a pixel depend on the viewBox\r\n" +
				"	}\r\n" +
				"	/* ]]> */\r\n" +
				"	</style>\r\n" +
				"	<circle cx=\"50\" cy=\"50\" r=\"40\"/>\r\n" +
				"</svg>"
				;
			const string targetOutput5A = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 100\">\r\n" +
				"	<style>" +
				"/*<![CDATA[*/" +
				"	circle {\r\n" +
				"		fill: orange;\r\n" +
				"		stroke: black;\r\n" +
				"		stroke-width: 10px; // Note that the value of a pixel depend on the viewBox\r\n" +
				"	}" +
				"/*]]>*/" +
				"</style>\r\n" +
				"	<circle cx=\"50\" cy=\"50\" r=\"40\" />\r\n" +
				"</svg>"
				;
			const string targetOutput5B = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 100\">" +
				"<style>" +
				"/*<![CDATA[*/" +
				"circle {\r\n" +
				"		fill: orange;\r\n" +
				"		stroke: black;\r\n" +
				"		stroke-width: 10px; // Note that the value of a pixel depend on the viewBox\r\n" +
				"	}" +
				"/*]]>*/" +
				"</style>" +
				"<circle cx=\"50\" cy=\"50\" r=\"40\" />" +
				"</svg>"
				;
			const string targetOutput5C = targetOutput5A;


			const string input6 = "<svg version=\"1.1\" width=\"10cm\" height=\"5cm\" viewBox=\"0 0 1000 500\">\r\n" +
				"	<defs>\r\n" +
				"		<style type=\"text/css\">\r\n" +
				"<![CDATA[" +
				"		rect {\r\n" +
				"			fill: red;\r\n" +
				"			stroke: blue;\r\n" +
				"			stroke-width: 3;\r\n" +
				"		}\r\n" +
				"]]>" +
				"</style>\r\n" +
				"	</defs>\r\n" +
				"	<rect x=\"200\" y=\"100\" width=\"600\" height=\"300\" />\r\n" +
				"</svg>"
				;
			const string targetOutput6A = "<svg version=\"1.1\" width=\"10cm\" height=\"5cm\" viewBox=\"0 0 1000 500\">\r\n" +
				"	<defs>\r\n" +
				"		<style type=\"text/css\">" +
				"<![CDATA[" +
				"		rect {\r\n" +
				"			fill: red;\r\n" +
				"			stroke: blue;\r\n" +
				"			stroke-width: 3;\r\n" +
				"		}" +
				"]]>" +
				"</style>\r\n" +
				"	</defs>\r\n" +
				"	<rect x=\"200\" y=\"100\" width=\"600\" height=\"300\" />\r\n" +
				"</svg>"
				;
			const string targetOutput6B = "<svg version=\"1.1\" width=\"10cm\" height=\"5cm\" viewBox=\"0 0 1000 500\">" +
				"<defs>" +
				"<style type=\"text/css\">" +
				"<![CDATA[" +
				"rect {\r\n" +
				"			fill: red;\r\n" +
				"			stroke: blue;\r\n" +
				"			stroke-width: 3;\r\n" +
				"		}" +
				"]]>" +
				"</style>" +
				"</defs>" +
				"<rect x=\"200\" y=\"100\" width=\"600\" height=\"300\" />" +
				"</svg>"
				;
			const string targetOutput6C = targetOutput6A;

			// Act
			string output1A = minifier.Minify(input1).MinifiedContent;
			string output1B = removingWhitespaceMinifier.Minify(input1).MinifiedContent;
			string output1C = removingCdataSectionsMinifier.Minify(input1).MinifiedContent;

			string output2A = minifier.Minify(input2).MinifiedContent;
			string output2B = removingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = removingCdataSectionsMinifier.Minify(input2).MinifiedContent;

			string output3A = minifier.Minify(input3).MinifiedContent;
			string output3B = removingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3C = removingCdataSectionsMinifier.Minify(input3).MinifiedContent;

			string output4A = minifier.Minify(input4).MinifiedContent;
			string output4B = removingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4C = removingCdataSectionsMinifier.Minify(input4).MinifiedContent;

			string output5A = minifier.Minify(input5).MinifiedContent;
			string output5B = removingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5C = removingCdataSectionsMinifier.Minify(input5).MinifiedContent;

			string output6A = minifier.Minify(input6).MinifiedContent;
			string output6B = removingWhitespaceMinifier.Minify(input6).MinifiedContent;
			string output6C = removingCdataSectionsMinifier.Minify(input6).MinifiedContent;

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
		public void ProcessingCdataSectionsInScriptsIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));
			var removingWhitespaceMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { WhitespaceMinificationMode = WhitespaceMinificationMode.Medium });
			var removingCdataSectionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveCdataSectionsFromScriptsAndStyles = true });

			const string input1 = "<script type=\"text/javascript\">\r\n" +
				"//<![CDATA[\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//]]>" +
				"\r\n</script>"
				;
			const string targetOutput1A = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput1B = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"alert('Hooray!');" +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput1C = "<script type=\"text/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input2 = "<script type=\"text/javascript\">\r\n" +
				"// <![CDATA[\r\n" +
				"\talert('Hooray!'); " +
				"\r\n// ]]>" +
				"\r\n</script>"
				;
			const string targetOutput2A = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput2B = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"alert('Hooray!');" +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput2C = "<script type=\"text/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input3 = "<script type=\"text/javascript\">\r\n" +
				"// <![CDATA[ alert('Test 1'); \r\n" +
				"\talert('Test 2'); " +
				"\r\n//\talert('Test 3'); ]]>" +
				"\r\n</script>"
				;
			const string targetOutput3A = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"\talert('Test 2'); " +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput3B = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"alert('Test 2');" +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput3C = "<script type=\"text/javascript\">" +
				"\talert('Test 2'); " +
				"</script>"
				;


			const string input4 = "<script type=\"text/javascript\">\r\n" +
				"/*<![CDATA[*/\r\n" +
				"\talert('Hooray!'); " +
				"\r\n/*]]>*" +
				"/\r\n</script>"
				;
			const string targetOutput4A = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput4B = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"alert('Hooray!');" +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput4C = "<script type=\"text/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input5 = "<script type=\"text/javascript\">\r\n" +
				"/* <![CDATA[ */\r\n" +
				"\talert('Hooray!'); " +
				"\r\n/* ]]> */" +
				"\r\n</script>"
				;
			const string targetOutput5A = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput5B = "<script type=\"text/javascript\">" +
				"//<![CDATA[\r\n" +
				"alert('Hooray!');" +
				"\r\n//]]>" +
				"</script>"
				;
			const string targetOutput5C = "<script type=\"text/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input6 = "<script type=\"text/javascript\">\r\n" +
				"<!--//--><![CDATA[//><!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//--><!]]>" +
				"\r\n</script>"
				;
			const string targetOutput6A = "<script type=\"text/javascript\">" +
				"<!--//--><![CDATA[//><!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//--><!]]>" +
				"</script>"
				;
			const string targetOutput6B = "<script type=\"text/javascript\">" +
				"<!--//--><![CDATA[//><!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//--><!]]>" +
				"</script>"
				;
			const string targetOutput6C = "<script type=\"text/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input7 = "<script type=\"text/ecmascript\">\r\n" +
				"<!-- // --><![CDATA[ // ><!-- \r\n" +
				"\talert('Hooray!'); " +
				"\r\n // --><!]]>" +
				"\r\n</script>"
				;
			const string targetOutput7A = "<script type=\"text/ecmascript\">" +
				"<!--//--><![CDATA[//><!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//--><!]]>" +
				"</script>"
				;
			const string targetOutput7B = "<script type=\"text/ecmascript\">" +
				"<!--//--><![CDATA[//><!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//--><!]]>" +
				"</script>"
				;
			const string targetOutput7C = "<script type=\"text/ecmascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input8 = "<script type=\"application/ecmascript\">\r\n" +
				"<!--/*--><![CDATA[/*><!--*/\r\n" +
				"\talert('Hooray!'); " +
				"\r\n/*]]>*/-->" +
				"\r\n</script>"
				;
			const string targetOutput8A = "<script type=\"application/ecmascript\">" +
				"<!--//--><![CDATA[//><!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//--><!]]>" +
				"</script>"
				;
			const string targetOutput8B = "<script type=\"application/ecmascript\">" +
				"<!--//--><![CDATA[//><!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//--><!]]>" +
				"</script>"
				;
			const string targetOutput8C = "<script type=\"application/ecmascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input9 = "<script type=\"application/javascript\">\r\n" +
				"<!-- /* --><![CDATA[ /* ><!-- */ \r\n" +
				"\talert('Hooray!'); " +
				"\r\n/* ]]> */ -->" +
				"\r\n</script>"
				;
			const string targetOutput9A = "<script type=\"application/javascript\">" +
				"<!--//--><![CDATA[//><!--\r\n" +
				"\talert('Hooray!'); " +
				"\r\n//--><!]]>" +
				"</script>"
				;
			const string targetOutput9B = "<script type=\"application/javascript\">" +
				"<!--//--><![CDATA[//><!--\r\n" +
				"alert('Hooray!');" +
				"\r\n//--><!]]>" +
				"</script>"
				;
			const string targetOutput9C = "<script type=\"application/javascript\">" +
				"\talert('Hooray!'); " +
				"</script>"
				;


			const string input10 = "<script type=\"text/javascript\">\r\n" +
				"	<![CDATA[\r\n" +
				"	// is the variable a non-negative integer less than 15?\r\n" +
				"	if (variable < 15 && variable >= 0) {\r\n" +
				"		doSomething();\r\n" +
				"	}\r\n" +
				"	]]>\r\n" +
				"</script>"
				;
			const string targetOutput10A = "<script type=\"text/javascript\">" +
				"<![CDATA[\r\n" +
				"	// is the variable a non-negative integer less than 15?\r\n" +
				"	if (variable < 15 && variable >= 0) {\r\n" +
				"		doSomething();\r\n" +
				"	}\r\n" +
				"]]>" +
				"</script>"
				;
			const string targetOutput10B = "<script type=\"text/javascript\">" +
				"<![CDATA[\r\n" +
				"// is the variable a non-negative integer less than 15?\r\n" +
				"	if (variable < 15 && variable >= 0) {\r\n" +
				"		doSomething();\r\n" +
				"	}\r\n" +
				"]]>" +
				"</script>"
				;
			const string targetOutput10C = "<script type=\"text/javascript\">" +
				"<![CDATA[\r\n" +
				"	// is the variable a non-negative integer less than 15?\r\n" +
				"	if (variable < 15 && variable >= 0) {\r\n" +
				"		doSomething();\r\n" +
				"	}\r\n" +
				"]]>" +
				"</script>"
				;


			const string input11 = "<script type=\"text/vbscript\">\r\n" +
				"<![CDATA[\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"]]>\r\n" +
				"</script>"
				;
			const string targetOutput11A = "<script type=\"text/vbscript\">" +
				"<![CDATA[\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"]]>" +
				"</script>"
				;
			const string targetOutput11B = "<script type=\"text/vbscript\">" +
				"<![CDATA[\r\n" +
				"Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"]]>" +
				"</script>"
				;
			const string targetOutput11C = "<script type=\"text/vbscript\">" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function" +
				"</script>";


			const string input12 = "<script language=\"VBScript\">\r\n" +
				"<![CDATA[\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"]]>\r\n" +
				"</script>"
				;
			const string targetOutput12A = "<script language=\"VBScript\">" +
				"<![CDATA[\r\n" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"]]>" +
				"</script>"
				;
			const string targetOutput12B = "<script language=\"VBScript\">" +
				"<![CDATA[\r\n" +
				"Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function\r\n" +
				"]]>" +
				"</script>"
				;
			const string targetOutput12C = "<script language=\"VBScript\">" +
				"	Function CanDeliver(Dt)\r\n" +
				"		CanDeliver = (CDate(Dt) - Now()) > 2\r\n" +
				"	End Function" +
				"</script>";


			const string input13 = "<script type=\"application/json\">\r\n" +
				"<![CDATA[\r\n" +
				"{\r\n" +
				"  \"vars\" : {\r\n" +
				"    \"gtag_id\": \"UA-39595497-1\",\r\n" +
				"    \"config\" : {\r\n" +
				"      \"UA-39595497-1\": { \"groups\": \"default\" }\r\n" +
				"    }\r\n" +
				"  }\r\n" +
				"}\r\n" +
				"]]>\r\n" +
				"</script>"
				;
			const string targetOutput13A = "<script type=\"application/json\">" +
				"<![CDATA[\r\n" +
				"{\r\n" +
				"  \"vars\" : {\r\n" +
				"    \"gtag_id\": \"UA-39595497-1\",\r\n" +
				"    \"config\" : {\r\n" +
				"      \"UA-39595497-1\": { \"groups\": \"default\" }\r\n" +
				"    }\r\n" +
				"  }\r\n" +
				"}\r\n" +
				"]]>" +
				"</script>"
				;
			const string targetOutput13B = "<script type=\"application/json\">" +
				"<![CDATA[\r\n" +
				"{\r\n" +
				"  \"vars\" : {\r\n" +
				"    \"gtag_id\": \"UA-39595497-1\",\r\n" +
				"    \"config\" : {\r\n" +
				"      \"UA-39595497-1\": { \"groups\": \"default\" }\r\n" +
				"    }\r\n" +
				"  }\r\n" +
				"}\r\n" +
				"]]>" +
				"</script>"
				;
			const string targetOutput13C = "<script type=\"application/json\">" +
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


			const string input14 = "<script type=\"application/ld+json\">\r\n" +
				"<![CDATA[\r\n" +
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
				"]]>\r\n" +
				"</script>"
				;
			const string targetOutput14A = "<script type=\"application/ld+json\">" +
				"<![CDATA[\r\n" +
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
				"]]>" +
				"</script>"
				;
			const string targetOutput14B = "<script type=\"application/ld+json\">" +
				"<![CDATA[\r\n" +
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
				"]]>" +
				"</script>"
				;
			const string targetOutput14C = "<script type=\"application/ld+json\">" +
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
			string output1C = removingCdataSectionsMinifier.Minify(input1).MinifiedContent;

			string output2A = minifier.Minify(input2).MinifiedContent;
			string output2B = removingWhitespaceMinifier.Minify(input2).MinifiedContent;
			string output2C = removingCdataSectionsMinifier.Minify(input2).MinifiedContent;

			string output3A = minifier.Minify(input3).MinifiedContent;
			string output3B = removingWhitespaceMinifier.Minify(input3).MinifiedContent;
			string output3C = removingCdataSectionsMinifier.Minify(input3).MinifiedContent;

			string output4A = minifier.Minify(input4).MinifiedContent;
			string output4B = removingWhitespaceMinifier.Minify(input4).MinifiedContent;
			string output4C = removingCdataSectionsMinifier.Minify(input4).MinifiedContent;

			string output5A = minifier.Minify(input5).MinifiedContent;
			string output5B = removingWhitespaceMinifier.Minify(input5).MinifiedContent;
			string output5C = removingCdataSectionsMinifier.Minify(input5).MinifiedContent;

			string output6A = minifier.Minify(input6).MinifiedContent;
			string output6B = removingWhitespaceMinifier.Minify(input6).MinifiedContent;
			string output6C = removingCdataSectionsMinifier.Minify(input6).MinifiedContent;

			string output7A = minifier.Minify(input7).MinifiedContent;
			string output7B = removingWhitespaceMinifier.Minify(input7).MinifiedContent;
			string output7C = removingCdataSectionsMinifier.Minify(input7).MinifiedContent;

			string output8A = minifier.Minify(input8).MinifiedContent;
			string output8B = removingWhitespaceMinifier.Minify(input8).MinifiedContent;
			string output8C = removingCdataSectionsMinifier.Minify(input8).MinifiedContent;

			string output9A = minifier.Minify(input9).MinifiedContent;
			string output9B = removingWhitespaceMinifier.Minify(input9).MinifiedContent;
			string output9C = removingCdataSectionsMinifier.Minify(input9).MinifiedContent;

			string output10A = minifier.Minify(input10).MinifiedContent;
			string output10B = removingWhitespaceMinifier.Minify(input10).MinifiedContent;
			string output10C = removingCdataSectionsMinifier.Minify(input10).MinifiedContent;

			string output11A = minifier.Minify(input11).MinifiedContent;
			string output11B = removingWhitespaceMinifier.Minify(input11).MinifiedContent;
			string output11C = removingCdataSectionsMinifier.Minify(input11).MinifiedContent;

			string output12A = minifier.Minify(input12).MinifiedContent;
			string output12B = removingWhitespaceMinifier.Minify(input12).MinifiedContent;
			string output12C = removingCdataSectionsMinifier.Minify(input12).MinifiedContent;

			string output13A = minifier.Minify(input13).MinifiedContent;
			string output13B = removingWhitespaceMinifier.Minify(input13).MinifiedContent;
			string output13C = removingCdataSectionsMinifier.Minify(input13).MinifiedContent;

			string output14A = minifier.Minify(input14).MinifiedContent;
			string output14B = removingWhitespaceMinifier.Minify(input14).MinifiedContent;
			string output14C = removingCdataSectionsMinifier.Minify(input14).MinifiedContent;

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
		}
	}
}