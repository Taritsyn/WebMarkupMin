using System;

using Xunit;

namespace WebMarkupMin.Core.Tests.Html.Minification
{
	public class NewLineStyleNormalizationTests
	{
		[Fact]
		public void NewLineStyleNormalizationInHtmlDocumentIsCorrect()
		{
			// Arrange
			string nativeNewLine = Environment.NewLine;

			const string input = "\r\n" +
				"<!DOCTYPE html>\r\n" +
				"<html>\r\n" +
				"	<head>\r\n" +
				"		<meta charset=\"utf-8\">\r\n" +
				"		<meta name=\"robots\" content=\"noindex, nofollow\">\r\n" +
				"		\r\n" +
				"		<style>\r" +
				"			.box {\r" +
				"				width: 200px;\r" +
				"				border: 2px solid rgba(150, 150, 150, 255);\r" +
				"				padding: 0.5em;\r" +
				"				word-break: break-all;\r" +
				"			}\r\r" +
				"			.box img {\r" +
				"				width: 200px;\r" +
				"			}\r\r" +
				"		</style>\r\n" +
				"		\n\r" +
				"		<title>\r\n" +
				"			HTMLImageElement.srcset - example - code sample\r\n" +
				"		</title>\r\n" +
				"	</head>\r\n" +
				"	<body>\r\n" +
				"		\r\n" +
				"		<div class=\"box\">\r\n" +
				"			<img src=\"/img/clock-demo-200px.png\" srcset=\"/img/clock-demo-200px.png 1x,\r\n" +
				"															/img/clock-demo-400px.png 2x\" " +
				"alt=\"Clock\">\r\n" +
				"		</div>\r\n" +
				" \n\r" +
				"		\n\r" +
				"		\n\r" +
				"		<script>\n" +
				"			window.addEventListener(\"load\", () => {\n" +
				"				let box = document.querySelector(\".box\");\n" +
				"				let image = box.querySelector(\"img\");\n\n" +
				"				let newElem = document.createElement(\"p\");\n" +
				"				newElem.innerHTML = `Image: <code>${image.currentSrc}</code>`;\n" +
				"				box.appendChild(newElem);\n" +
				"			});\n\n" +
				"		</script>\n\r" +
				"		\n\r" +
				"	</body>\r\n" +
				"</html>\r\n\r\n"
				;
			const string targetOutputA = input;
			string targetOutputB = nativeNewLine +
				"<!DOCTYPE html>" + nativeNewLine +
				"<html>" + nativeNewLine +
				"	<head>" + nativeNewLine +
				"		<meta charset=\"utf-8\">" + nativeNewLine +
				"		<meta name=\"robots\" content=\"noindex, nofollow\">" + nativeNewLine +
				"		" + nativeNewLine +
				"		<style>" + nativeNewLine +
				"			.box {" + nativeNewLine +
				"				width: 200px;" + nativeNewLine +
				"				border: 2px solid rgba(150, 150, 150, 255);" + nativeNewLine +
				"				padding: 0.5em;" + nativeNewLine +
				"				word-break: break-all;" + nativeNewLine +
				"			}" + nativeNewLine + nativeNewLine +
				"			.box img {" + nativeNewLine +
				"				width: 200px;" + nativeNewLine +
				"			}" + nativeNewLine + nativeNewLine +
				"		</style>" + nativeNewLine +
				"		" + nativeNewLine +
				"		<title>" + nativeNewLine +
				"			HTMLImageElement.srcset - example - code sample" + nativeNewLine +
				"		</title>" + nativeNewLine +
				"	</head>" + nativeNewLine +
				"	<body>" + nativeNewLine +
				"		" + nativeNewLine +
				"		<div class=\"box\">" + nativeNewLine +
				"			<img src=\"/img/clock-demo-200px.png\" srcset=\"/img/clock-demo-200px.png 1x," + nativeNewLine +
				"															/img/clock-demo-400px.png 2x\" " +
				"alt=\"Clock\">" + nativeNewLine +
				"		</div>" + nativeNewLine +
				" " + nativeNewLine +
				"		" + nativeNewLine +
				"		" + nativeNewLine +
				"		<script>" + nativeNewLine +
				"			window.addEventListener(\"load\", () => {" + nativeNewLine +
				"				let box = document.querySelector(\".box\");" + nativeNewLine +
				"				let image = box.querySelector(\"img\");" + nativeNewLine + nativeNewLine +
				"				let newElem = document.createElement(\"p\");" + nativeNewLine +
				"				newElem.innerHTML = `Image: <code>${image.currentSrc}</code>`;" + nativeNewLine +
				"				box.appendChild(newElem);" + nativeNewLine +
				"			});" + nativeNewLine + nativeNewLine +
				"		</script>" + nativeNewLine +
				"		" + nativeNewLine +
				"	</body>" + nativeNewLine +
				"</html>" + nativeNewLine + nativeNewLine
				;
			const string targetOutputC = "\r\n" +
				"<!DOCTYPE html>\r\n" +
				"<html>\r\n" +
				"	<head>\r\n" +
				"		<meta charset=\"utf-8\">\r\n" +
				"		<meta name=\"robots\" content=\"noindex, nofollow\">\r\n" +
				"		\r\n" +
				"		<style>\r\n" +
				"			.box {\r\n" +
				"				width: 200px;\r\n" +
				"				border: 2px solid rgba(150, 150, 150, 255);\r\n" +
				"				padding: 0.5em;\r\n" +
				"				word-break: break-all;\r\n" +
				"			}\r\n\r\n" +
				"			.box img {\r\n" +
				"				width: 200px;\r\n" +
				"			}\r\n\r\n" +
				"		</style>\r\n" +
				"		\r\n" +
				"		<title>\r\n" +
				"			HTMLImageElement.srcset - example - code sample\r\n" +
				"		</title>\r\n" +
				"	</head>\r\n" +
				"	<body>\r\n" +
				"		\r\n" +
				"		<div class=\"box\">\r\n" +
				"			<img src=\"/img/clock-demo-200px.png\" srcset=\"/img/clock-demo-200px.png 1x,\r\n" +
				"															/img/clock-demo-400px.png 2x\" " +
				"alt=\"Clock\">\r\n" +
				"		</div>\r\n" +
				" \r\n" +
				"		\r\n" +
				"		\r\n" +
				"		<script>\r\n" +
				"			window.addEventListener(\"load\", () => {\r\n" +
				"				let box = document.querySelector(\".box\");\r\n" +
				"				let image = box.querySelector(\"img\");\r\n\r\n" +
				"				let newElem = document.createElement(\"p\");\r\n" +
				"				newElem.innerHTML = `Image: <code>${image.currentSrc}</code>`;\r\n" +
				"				box.appendChild(newElem);\r\n" +
				"			});\r\n\r\n" +
				"		</script>\r\n" +
				"		\r\n" +
				"	</body>\r\n" +
				"</html>\r\n\r\n"
				;
			const string targetOutputD = "\r" +
				"<!DOCTYPE html>\r" +
				"<html>\r" +
				"	<head>\r" +
				"		<meta charset=\"utf-8\">\r" +
				"		<meta name=\"robots\" content=\"noindex, nofollow\">\r" +
				"		\r" +
				"		<style>\r" +
				"			.box {\r" +
				"				width: 200px;\r" +
				"				border: 2px solid rgba(150, 150, 150, 255);\r" +
				"				padding: 0.5em;\r" +
				"				word-break: break-all;\r" +
				"			}\r\r" +
				"			.box img {\r" +
				"				width: 200px;\r" +
				"			}\r\r" +
				"		</style>\r" +
				"		\r" +
				"		<title>\r" +
				"			HTMLImageElement.srcset - example - code sample\r" +
				"		</title>\r" +
				"	</head>\r" +
				"	<body>\r" +
				"		\r" +
				"		<div class=\"box\">\r" +
				"			<img src=\"/img/clock-demo-200px.png\" srcset=\"/img/clock-demo-200px.png 1x,\r" +
				"															/img/clock-demo-400px.png 2x\" " +
				"alt=\"Clock\">\r" +
				"		</div>\r" +
				" \r" +
				"		\r" +
				"		\r" +
				"		<script>\r" +
				"			window.addEventListener(\"load\", () => {\r" +
				"				let box = document.querySelector(\".box\");\r" +
				"				let image = box.querySelector(\"img\");\r\r" +
				"				let newElem = document.createElement(\"p\");\r" +
				"				newElem.innerHTML = `Image: <code>${image.currentSrc}</code>`;\r" +
				"				box.appendChild(newElem);\r" +
				"			});\r\r" +
				"		</script>\r" +
				"		\r" +
				"	</body>\r" +
				"</html>\r\r"
				;
			const string targetOutputE = "\n" +
				"<!DOCTYPE html>\n" +
				"<html>\n" +
				"	<head>\n" +
				"		<meta charset=\"utf-8\">\n" +
				"		<meta name=\"robots\" content=\"noindex, nofollow\">\n" +
				"		\n" +
				"		<style>\n" +
				"			.box {\n" +
				"				width: 200px;\n" +
				"				border: 2px solid rgba(150, 150, 150, 255);\n" +
				"				padding: 0.5em;\n" +
				"				word-break: break-all;\n" +
				"			}\n\n" +
				"			.box img {\n" +
				"				width: 200px;\n" +
				"			}\n\n" +
				"		</style>\n" +
				"		\n" +
				"		<title>\n" +
				"			HTMLImageElement.srcset - example - code sample\n" +
				"		</title>\n" +
				"	</head>\n" +
				"	<body>\n" +
				"		\n" +
				"		<div class=\"box\">\n" +
				"			<img src=\"/img/clock-demo-200px.png\" srcset=\"/img/clock-demo-200px.png 1x,\n" +
				"															/img/clock-demo-400px.png 2x\" " +
				"alt=\"Clock\">\n" +
				"		</div>\n" +
				" \n" +
				"		\n" +
				"		\n" +
				"		<script>\n" +
				"			window.addEventListener(\"load\", () => {\n" +
				"				let box = document.querySelector(\".box\");\n" +
				"				let image = box.querySelector(\"img\");\n\n" +
				"				let newElem = document.createElement(\"p\");\n" +
				"				newElem.innerHTML = `Image: <code>${image.currentSrc}</code>`;\n" +
				"				box.appendChild(newElem);\n" +
				"			});\n\n" +
				"		</script>\n" +
				"		\n" +
				"	</body>\n" +
				"</html>\n\n"
				;

			var autoNewLineStyleMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { NewLineStyle = NewLineStyle.Auto });
			var nativeNewLineStyleMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { NewLineStyle = NewLineStyle.Native });
			var windowsNewLineStyleMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { NewLineStyle = NewLineStyle.Windows });
			var macNewLineStyleMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { NewLineStyle = NewLineStyle.Mac });
			var unixNewLineStyleMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { NewLineStyle = NewLineStyle.Unix });

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