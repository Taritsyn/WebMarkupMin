using System;

using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Xhtml.Common.Minification
{
	public class NewLineStyleNormalizationTests
	{
		[Fact]
		public void NewLineStyleNormalizationInXhtmlDocument()
		{
			// Arrange
			string nativeNewLine = Environment.NewLine;

			const string input = " \n " +
				"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" " +
				"\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n" +
				"<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\" lang=\"en\">\n" +
				"    <head>\n" +
				"        <meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />\n" +
				"        <meta name=\"robots\" content=\"noindex, nofollow\" />\n" +
				"        \n" +
				"        <style type=\"text/css\">\r" +
				"        #box {\r" +
				"            width: 200px;\r" +
				"            border: 2px solid #969696;\r" +
				"            padding: 0.5em;\r" +
				"        }\r\r" +
				"        #box img {\r" +
				"            width: 200px;\r" +
				"        }\r\r" +
				"        </style>\n" +
				"        \n\r" +
				"        <title>\n" +
				"            HTMLImageElement.src - example - code sample\n" +
				"        </title>\n" +
				"    </head>\n" +
				"    <body>\n" +
				"        \n" +
				"        <div id=\"box\">\n" +
				"            <a href=\"#\" onclick=\"showImageSrc();\n\r" +
				"                                    return false\"><img src=\"/img/clock-demo.png\" " +
				"alt=\"Clock\" /></a>\n" +
				"        </div>\n" +
				" \n\r" +
				"        \n\r\n\r" +
				"        <script type=\"text/javascript\">\r\n" +
				"        function showImageSrc() {\r\n" +
				"            var image = event.target;\r\n\r\n" +
				"            alert(\"Image: \" + image.src);\r\n" +
				"        };\r\n\r\n" +
				"        </script>\n\r" +
				"        \n\r" +
				"    </body>\n" +
				"</html>\n\n"
				;
			const string targetOutputA = input;
			string targetOutputB = " " + nativeNewLine + " " +
				"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" " +
				"\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" + nativeNewLine +
				"<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\" lang=\"en\">" + nativeNewLine +
				"    <head>" + nativeNewLine +
				"        <meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />" + nativeNewLine +
				"        <meta name=\"robots\" content=\"noindex, nofollow\" />" + nativeNewLine +
				"        " + nativeNewLine +
				"        <style type=\"text/css\">" + nativeNewLine +
				"        #box {" + nativeNewLine +
				"            width: 200px;" + nativeNewLine +
				"            border: 2px solid #969696;" + nativeNewLine +
				"            padding: 0.5em;" + nativeNewLine +
				"        }" + nativeNewLine + nativeNewLine +
				"        #box img {" + nativeNewLine +
				"            width: 200px;" + nativeNewLine +
				"        }" + nativeNewLine + nativeNewLine +
				"        </style>" + nativeNewLine +
				"        " + nativeNewLine +
				"        <title>" + nativeNewLine +
				"            HTMLImageElement.src - example - code sample" + nativeNewLine +
				"        </title>" + nativeNewLine +
				"    </head>" + nativeNewLine +
				"    <body>" + nativeNewLine +
				"        " + nativeNewLine +
				"        <div id=\"box\">" + nativeNewLine +
				"            <a href=\"#\" onclick=\"showImageSrc();" + nativeNewLine +
				"                                    return false\"><img src=\"/img/clock-demo.png\" " +
				"alt=\"Clock\" /></a>" + nativeNewLine +
				"        </div>" + nativeNewLine +
				" " + nativeNewLine +
				"        " + nativeNewLine + nativeNewLine +
				"        <script type=\"text/javascript\">" + nativeNewLine +
				"        function showImageSrc() {" + nativeNewLine +
				"            var image = event.target;" + nativeNewLine + nativeNewLine +
				"            alert(\"Image: \" + image.src);" + nativeNewLine +
				"        };" + nativeNewLine + nativeNewLine +
				"        </script>" + nativeNewLine +
				"        " + nativeNewLine +
				"    </body>" + nativeNewLine +
				"</html>" + nativeNewLine + nativeNewLine
				;
			const string targetOutputC = " \r\n " +
				"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" " +
				"\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n" +
				"<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\" lang=\"en\">\r\n" +
				"    <head>\r\n" +
				"        <meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />\r\n" +
				"        <meta name=\"robots\" content=\"noindex, nofollow\" />\r\n" +
				"        \r\n" +
				"        <style type=\"text/css\">\r\n" +
				"        #box {\r\n" +
				"            width: 200px;\r\n" +
				"            border: 2px solid #969696;\r\n" +
				"            padding: 0.5em;\r\n" +
				"        }\r\n\r\n" +
				"        #box img {\r\n" +
				"            width: 200px;\r\n" +
				"        }\r\n\r\n" +
				"        </style>\r\n" +
				"        \r\n" +
				"        <title>\r\n" +
				"            HTMLImageElement.src - example - code sample\r\n" +
				"        </title>\r\n" +
				"    </head>\r\n" +
				"    <body>\r\n" +
				"        \r\n" +
				"        <div id=\"box\">\r\n" +
				"            <a href=\"#\" onclick=\"showImageSrc();\r\n" +
				"                                    return false\"><img src=\"/img/clock-demo.png\" " +
				"alt=\"Clock\" /></a>\r\n" +
				"        </div>\r\n" +
				" \r\n" +
				"        \r\n\r\n" +
				"        <script type=\"text/javascript\">\r\n" +
				"        function showImageSrc() {\r\n" +
				"            var image = event.target;\r\n\r\n" +
				"            alert(\"Image: \" + image.src);\r\n" +
				"        };\r\n\r\n" +
				"        </script>\r\n" +
				"        \r\n" +
				"    </body>\r\n" +
				"</html>\r\n\r\n"
				;
			const string targetOutputD = " \r " +
				"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" " +
				"\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r" +
				"<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\" lang=\"en\">\r" +
				"    <head>\r" +
				"        <meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />\r" +
				"        <meta name=\"robots\" content=\"noindex, nofollow\" />\r" +
				"        \r" +
				"        <style type=\"text/css\">\r" +
				"        #box {\r" +
				"            width: 200px;\r" +
				"            border: 2px solid #969696;\r" +
				"            padding: 0.5em;\r" +
				"        }\r\r" +
				"        #box img {\r" +
				"            width: 200px;\r" +
				"        }\r\r" +
				"        </style>\r" +
				"        \r" +
				"        <title>\r" +
				"            HTMLImageElement.src - example - code sample\r" +
				"        </title>\r" +
				"    </head>\r" +
				"    <body>\r" +
				"        \r" +
				"        <div id=\"box\">\r" +
				"            <a href=\"#\" onclick=\"showImageSrc();\r" +
				"                                    return false\"><img src=\"/img/clock-demo.png\" " +
				"alt=\"Clock\" /></a>\r" +
				"        </div>\r" +
				" \r" +
				"        \r\r" +
				"        <script type=\"text/javascript\">\r" +
				"        function showImageSrc() {\r" +
				"            var image = event.target;\r\r" +
				"            alert(\"Image: \" + image.src);\r" +
				"        };\r\r" +
				"        </script>\r" +
				"        \r" +
				"    </body>\r" +
				"</html>\r\r"
				;
			const string targetOutputE = " \n " +
				"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" " +
				"\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n" +
				"<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\" lang=\"en\">\n" +
				"    <head>\n" +
				"        <meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />\n" +
				"        <meta name=\"robots\" content=\"noindex, nofollow\" />\n" +
				"        \n" +
				"        <style type=\"text/css\">\n" +
				"        #box {\n" +
				"            width: 200px;\n" +
				"            border: 2px solid #969696;\n" +
				"            padding: 0.5em;\n" +
				"        }\n\n" +
				"        #box img {\n" +
				"            width: 200px;\n" +
				"        }\n\n" +
				"        </style>\n" +
				"        \n" +
				"        <title>\n" +
				"            HTMLImageElement.src - example - code sample\n" +
				"        </title>\n" +
				"    </head>\n" +
				"    <body>\n" +
				"        \n" +
				"        <div id=\"box\">\n" +
				"            <a href=\"#\" onclick=\"showImageSrc();\n" +
				"                                    return false\"><img src=\"/img/clock-demo.png\" " +
				"alt=\"Clock\" /></a>\n" +
				"        </div>\n" +
				" \n" +
				"        \n\n" +
				"        <script type=\"text/javascript\">\n" +
				"        function showImageSrc() {\n" +
				"            var image = event.target;\n\n" +
				"            alert(\"Image: \" + image.src);\n" +
				"        };\n\n" +
				"        </script>\n" +
				"        \n" +
				"    </body>\n" +
				"</html>\n\n"
				;

			var autoNewLineStyleMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true) { NewLineStyle = NewLineStyle.Auto });
			var nativeNewLineStyleMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true) { NewLineStyle = NewLineStyle.Native });
			var windowsNewLineStyleMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true) { NewLineStyle = NewLineStyle.Windows });
			var macNewLineStyleMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true) { NewLineStyle = NewLineStyle.Mac });
			var unixNewLineStyleMinifier = new XhtmlMinifier(
				new XhtmlMinificationSettings(true) { NewLineStyle = NewLineStyle.Unix });

			// Act
			string outputA = autoNewLineStyleMinifier.Minify(input).MinifiedContent;
			string outputB = nativeNewLineStyleMinifier.Minify(input).MinifiedContent;
			string outputC = windowsNewLineStyleMinifier.Minify(input).MinifiedContent;
			string outputD = macNewLineStyleMinifier.Minify(input).MinifiedContent;
			string outputE = unixNewLineStyleMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
			Assert.Equal(targetOutputE, outputE);
		}
	}
}