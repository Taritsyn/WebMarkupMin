using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Xhtml.Common.Minification
{
	public class WhitespaceMinificationTests
	{
		[Fact]
		public void WhitespaceMinificationInXhtmlDocumentIsCorrect()
		{
			// Arrange
			var keepingWhitespaceMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.None,
					PreserveNewLines = false
				});
			var keepingWhitespaceAndNewLinesMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.None,
					PreserveNewLines = true
				});
			var safeRemovingWhitespaceMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Safe,
					PreserveNewLines = false
				});
			var safeRemovingWhitespaceExceptForNewLinesMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Safe,
					PreserveNewLines = true
				});
			var mediumRemovingWhitespaceMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Medium,
					PreserveNewLines = false
				});
			var mediumRemovingWhitespaceExceptForNewLinesMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Medium,
					PreserveNewLines = true
				});
			var aggressiveRemovingWhitespaceMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive,
					PreserveNewLines = false
				});
			var aggressiveRemovingWhitespaceExceptForNewLinesMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true)
				{
					WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive,
					PreserveNewLines = true
				});

			const string input = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\"\r\n" +
				"   \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\r\n" +
				"<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n" +
				"	<head>\r\n" +
				"		<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\" />\r\n" +
				"		<title>\r\n" +
				"			IIS Windows\r\n" +
				"		</title>\r\n\r\n" +
				"		<style type=\"text/css\">\r\n" +
				"		<!--\r\n" +
				"		body {\r\n" +
				"			color:#000000;\r\n" +
				"			background-color:#0072C6;\r\n" +
				"			margin:0;\r\n" +
				"		}\r\n\r\n" +
				"		#container {\r\n" +
				"			margin-left:auto;\r\n" +
				"			margin-right:auto;\r\n" +
				"			text-align:center;\r\n" +
				"			}\r\n\r\n" +
				"		a img {\r\n" +
				"			border:none;\r\n" +
				"		}\r\n\r\n" +
				"		-->\r\n" +
				"		</style>\r\n\r\n" +
				"	</head>\r\n" +
				"	<body>\r\n\r\n" +
				"		<div id=\"container\">\r\n" +
				"			<a href=\"http://go.microsoft.com/fwlink/?linkid=66138&amp;clcid=0x409\">" +
				"<img src=\"iisstart.png\" alt=\"IIS\" width=\"960\" height=\"600\" />" +
				"</a>\r\n" +
				"		</div>\r\n\r\n" +
				"	</body>\r\n" +
				"</html>\r\n"
				;
			const string targetOutputA = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" " +
				"\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\r\n" +
				"<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n" +
				"	<head>\r\n" +
				"		<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\" />\r\n" +
				"		<title>\r\n" +
				"			IIS Windows\r\n" +
				"		</title>\r\n\r\n" +
				"		<style type=\"text/css\">" +
				"<!--" +
				"		body {\r\n" +
				"			color:#000000;\r\n" +
				"			background-color:#0072C6;\r\n" +
				"			margin:0;\r\n" +
				"		}\r\n\r\n" +
				"		#container {\r\n" +
				"			margin-left:auto;\r\n" +
				"			margin-right:auto;\r\n" +
				"			text-align:center;\r\n" +
				"			}\r\n\r\n" +
				"		a img {\r\n" +
				"			border:none;\r\n" +
				"		}\r\n" +
				"-->" +
				"</style>\r\n\r\n" +
				"	</head>\r\n" +
				"	<body>\r\n\r\n" +
				"		<div id=\"container\">\r\n" +
				"			<a href=\"http://go.microsoft.com/fwlink/?linkid=66138&amp;clcid=0x409\">" +
				"<img src=\"iisstart.png\" alt=\"IIS\" width=\"960\" height=\"600\" />" +
				"</a>\r\n" +
				"		</div>\r\n\r\n" +
				"	</body>\r\n" +
				"</html>\r\n";
			const string targetOutputB = targetOutputA;
			const string targetOutputC = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" " +
				"\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">" +
				"<html xmlns=\"http://www.w3.org/1999/xhtml\">" +
				"<head>" +
				"<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\" />" +
				"<title>" +
				"IIS Windows" +
				"</title>" +
				"<style type=\"text/css\">" +
				"<!--" +
				"body {\r\n" +
				"			color:#000000;\r\n" +
				"			background-color:#0072C6;\r\n" +
				"			margin:0;\r\n" +
				"		}\r\n\r\n" +
				"		#container {\r\n" +
				"			margin-left:auto;\r\n" +
				"			margin-right:auto;\r\n" +
				"			text-align:center;\r\n" +
				"			}\r\n\r\n" +
				"		a img {\r\n" +
				"			border:none;\r\n" +
				"		}" +
				"-->" +
				"</style>" +
				"</head>" +
				"<body>" +
				"<div id=\"container\"> " +
				"<a href=\"http://go.microsoft.com/fwlink/?linkid=66138&amp;clcid=0x409\">" +
				"<img src=\"iisstart.png\" alt=\"IIS\" width=\"960\" height=\"600\" />" +
				"</a> " +
				"</div>" +
				"</body>" +
				"</html>"
				;
			const string targetOutputD = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" " +
				"\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\r\n" +
				"<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n" +
				"<head>\r\n" +
				"<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\" />\r\n" +
				"<title>\r\n" +
				"IIS Windows\r\n" +
				"</title>\r\n" +
				"<style type=\"text/css\">" +
				"<!--" +
				"body {\r\n" +
				"			color:#000000;\r\n" +
				"			background-color:#0072C6;\r\n" +
				"			margin:0;\r\n" +
				"		}\r\n\r\n" +
				"		#container {\r\n" +
				"			margin-left:auto;\r\n" +
				"			margin-right:auto;\r\n" +
				"			text-align:center;\r\n" +
				"			}\r\n\r\n" +
				"		a img {\r\n" +
				"			border:none;\r\n" +
				"		}\r\n" +
				"-->" +
				"</style>\r\n" +
				"</head>\r\n" +
				"<body>\r\n" +
				"<div id=\"container\">\r\n" +
				"<a href=\"http://go.microsoft.com/fwlink/?linkid=66138&amp;clcid=0x409\">" +
				"<img src=\"iisstart.png\" alt=\"IIS\" width=\"960\" height=\"600\" />" +
				"</a>\r\n" +
				"</div>\r\n" +
				"</body>\r\n" +
				"</html>"
				;
			const string targetOutputE = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" " +
				"\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">" +
				"<html xmlns=\"http://www.w3.org/1999/xhtml\">" +
				"<head>" +
				"<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\" />" +
				"<title>" +
				"IIS Windows" +
				"</title>" +
				"<style type=\"text/css\">" +
				"<!--" +
				"body {\r\n" +
				"			color:#000000;\r\n" +
				"			background-color:#0072C6;\r\n" +
				"			margin:0;\r\n" +
				"		}\r\n\r\n" +
				"		#container {\r\n" +
				"			margin-left:auto;\r\n" +
				"			margin-right:auto;\r\n" +
				"			text-align:center;\r\n" +
				"			}\r\n\r\n" +
				"		a img {\r\n" +
				"			border:none;\r\n" +
				"		}" +
				"-->" +
				"</style>" +
				"</head>" +
				"<body>" +
				"<div id=\"container\">" +
				"<a href=\"http://go.microsoft.com/fwlink/?linkid=66138&amp;clcid=0x409\">" +
				"<img src=\"iisstart.png\" alt=\"IIS\" width=\"960\" height=\"600\" />" +
				"</a>" +
				"</div>" +
				"</body>" +
				"</html>"
				;
			const string targetOutputF = targetOutputD;
			const string targetOutputG = targetOutputE;
			const string targetOutputH = targetOutputD;

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
	}
}