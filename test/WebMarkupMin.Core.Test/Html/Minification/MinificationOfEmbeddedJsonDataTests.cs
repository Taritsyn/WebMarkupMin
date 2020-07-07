using Xunit;

namespace WebMarkupMin.Core.Test.Html.Minification
{
	public class MinificationOfEmbeddedJsonDataTests
	{
		[Fact]
		public void MinificationOfEmbeddedJsonDataIsCorrect()
		{
			// Arrange
			var keepingEmbeddedJsonDataMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyEmbeddedJsonData = false });
			var minifyingEmbeddedJsonDataMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyEmbeddedJsonData = true });

			const string input1 = "<script type=\"application/json\">\r\n" +
				"{\r\n" +
				"  \"vars\" : {\r\n" +
				"    \"gtag_id\": \"UA-39595497-1\",\r\n" +
				"    \"config\" : {\r\n" +
				"      \"UA-39595497-1\": { \"groups\": \"default\" }\r\n" +
				"    }\r\n" +
				"  }\r\n" +
				"}\r\n" +
				"</script>"
				;
			const string targetOutput1A = input1;
			const string targetOutput1B = "<script type=\"application/json\">" +
				"{" +
				"\"vars\":{" +
				"\"gtag_id\":\"UA-39595497-1\"," +
				"\"config\":{" +
				"\"UA-39595497-1\":{\"groups\":\"default\"}" +
				"}" +
				"}" +
				"}" +
				"</script>"
				;

			const string input2 = "<script type=\"application/ld+json\">\r\n" +
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
				"</script>"
				;
			const string targetOutput2A = input2;
			const string targetOutput2B = "<script type=\"application/ld+json\">" +
				"{" +
				"\"@context\":\"http:\\/\\/schema.org\"," +
				"\"@type\":\"Article\"," +
				"\"mainEntityOfPage\":{" +
				"\"@type\":\"WebPage\"," +
				"\"@id\":\"https:\\/\\/habr.com\\/ru\\/post\\/306484\\/\"" +
				"}," +
				"\"headline\":\"Переходим на WebMarkupMin 2.X\"," +
				"\"datePublished\":\"2016-07-26T22:47:45+03:00\"," +
				"\"dateModified\":\"2016-07-27T09:40:50+03:00\"," +
				"\"author\":{" +
				"\"@type\":\"Person\"," +
				"\"name\":\"Андрей Тарицын\"" +
				"}," +
				"\"publisher\":{" +
				"\"@type\":\"Organization\"," +
				"\"name\":\"Habr\"," +
				"\"logo\":{" +
				"\"@type\":\"ImageObject\"," +
				"\"url\":\"https:\\/\\/dr.habracdn.net\\/habrcom\\/images\\/habr_google.png\"" +
				"}" +
				"}," +
				"\"description\":\"Весной прошлого года, когда ASP.NET 5 был еще в стадии Beta 3, " +
				"я начал получать от пользователей письма с просьбами сделать WebMarkupMin совместимым " +
				"с DNX 4.5.1...\"," +
				"\"image\":[" +
				"\"https:\\/\\/habrastorage.org\\/getpro\\/habr\\/post_images\\/560\\/f32\\/1b2\\/560f321b29f23528ac5ccd18035783d1.png\"" +
				"]" +
				"}" +
				"</script>"
				;

			// Act
			string output1A = keepingEmbeddedJsonDataMinifier.Minify(input1).MinifiedContent;
			string output1B = minifyingEmbeddedJsonDataMinifier.Minify(input1).MinifiedContent;

			string output2A = keepingEmbeddedJsonDataMinifier.Minify(input2).MinifiedContent;
			string output2B = minifyingEmbeddedJsonDataMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(targetOutput2A, output2A);
			Assert.Equal(targetOutput2B, output2B);
		}
	}
}