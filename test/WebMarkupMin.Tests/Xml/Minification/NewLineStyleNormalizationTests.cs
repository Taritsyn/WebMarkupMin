using System;

using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Xml.Minification
{
	public class NewLineStyleNormalizationTests
	{
		[Fact]
		public void NewLineStyleNormalizationInXmlDocumentIsCorrect()
		{
			// Arrange
			string nativeNewLine = Environment.NewLine;

			const string input = " \n " +
				"<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
				"<breakfast_menu>\r\n" +
				"	<food>\r\n" +
				"		<name alternative_name=\"Toasted white bread...\n" +
				"Dry\">French Toast</name>\r\n" +
				"		<price>$4.35</price>\r\n" +
				"		<description>Thick slices made from our homemade sourdough bread</description>\r\n" +
				"		<calories>600</calories>\r\n" +
				"	</food>\n\r\r" +
				"	<food>\r\n" +
				"		<name>Belgian Waffles</name>\r\n" +
				"		<price>$6.85</price>\r\n" +
				"		<description><![CDATA[\r" +
				"			Our famous Belgian Waffles with plenty of real maple syrup. 6 < 7\r\r" +
				"		]]></description>\r\n" +
				"		<calories>650</calories>\r\n" +
				"	</food>\r\n\n" +
				"</breakfast_menu>\r\n" +
				"	\n\r\n\r"
				;
			const string targetOutputA = input;
			string targetOutputB = " " + nativeNewLine + " " +
				"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + nativeNewLine +
				"<breakfast_menu>" + nativeNewLine +
				"	<food>" + nativeNewLine +
				"		<name alternative_name=\"Toasted white bread..." + nativeNewLine +
				"Dry\">French Toast</name>" + nativeNewLine +
				"		<price>$4.35</price>" + nativeNewLine +
				"		<description>Thick slices made from our homemade sourdough bread</description>" + nativeNewLine +
				"		<calories>600</calories>" + nativeNewLine +
				"	</food>" + nativeNewLine + nativeNewLine +
				"	<food>" + nativeNewLine +
				"		<name>Belgian Waffles</name>" + nativeNewLine +
				"		<price>$6.85</price>" + nativeNewLine +
				"		<description><![CDATA[" + nativeNewLine +
				"			Our famous Belgian Waffles with plenty of real maple syrup. 6 < 7" + nativeNewLine + nativeNewLine +
				"		]]></description>" + nativeNewLine +
				"		<calories>650</calories>" + nativeNewLine +
				"	</food>" + nativeNewLine + nativeNewLine +
				"</breakfast_menu>" + nativeNewLine +
				"	" + nativeNewLine + nativeNewLine
				;
			const string targetOutputC = " \r\n " +
				"<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
				"<breakfast_menu>\r\n" +
				"	<food>\r\n" +
				"		<name alternative_name=\"Toasted white bread...\r\n" +
				"Dry\">French Toast</name>\r\n" +
				"		<price>$4.35</price>\r\n" +
				"		<description>Thick slices made from our homemade sourdough bread</description>\r\n" +
				"		<calories>600</calories>\r\n" +
				"	</food>\r\n\r\n" +
				"	<food>\r\n" +
				"		<name>Belgian Waffles</name>\r\n" +
				"		<price>$6.85</price>\r\n" +
				"		<description><![CDATA[\r\n" +
				"			Our famous Belgian Waffles with plenty of real maple syrup. 6 < 7\r\n\r\n" +
				"		]]></description>\r\n" +
				"		<calories>650</calories>\r\n" +
				"	</food>\r\n\r\n" +
				"</breakfast_menu>\r\n" +
				"	\r\n\r\n"
				;
			const string targetOutputD = " \r " +
				"<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r" +
				"<breakfast_menu>\r" +
				"	<food>\r" +
				"		<name alternative_name=\"Toasted white bread...\r" +
				"Dry\">French Toast</name>\r" +
				"		<price>$4.35</price>\r" +
				"		<description>Thick slices made from our homemade sourdough bread</description>\r" +
				"		<calories>600</calories>\r" +
				"	</food>\r\r" +
				"	<food>\r" +
				"		<name>Belgian Waffles</name>\r" +
				"		<price>$6.85</price>\r" +
				"		<description><![CDATA[\r" +
				"			Our famous Belgian Waffles with plenty of real maple syrup. 6 < 7\r\r" +
				"		]]></description>\r" +
				"		<calories>650</calories>\r" +
				"	</food>\r\r" +
				"</breakfast_menu>\r" +
				"	\r\r"
				;
			const string targetOutputE = " \n " +
				"<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
				"<breakfast_menu>\n" +
				"	<food>\n" +
				"		<name alternative_name=\"Toasted white bread...\n" +
				"Dry\">French Toast</name>\n" +
				"		<price>$4.35</price>\n" +
				"		<description>Thick slices made from our homemade sourdough bread</description>\n" +
				"		<calories>600</calories>\n" +
				"	</food>\n\n" +
				"	<food>\n" +
				"		<name>Belgian Waffles</name>\n" +
				"		<price>$6.85</price>\n" +
				"		<description><![CDATA[\n" +
				"			Our famous Belgian Waffles with plenty of real maple syrup. 6 < 7\n\n" +
				"		]]></description>\n" +
				"		<calories>650</calories>\n" +
				"	</food>\n\n" +
				"</breakfast_menu>\n" +
				"	\n\n"
				;

			var autoNewLineStyleMinifier = new XmlMinifier(
				new XmlMinificationSettings(true) { NewLineStyle = NewLineStyle.Auto });
			var nativeNewLineStyleMinifier = new XmlMinifier(
				new XmlMinificationSettings(true) { NewLineStyle = NewLineStyle.Native });
			var windowsNewLineStyleMinifier = new XmlMinifier(
				new XmlMinificationSettings(true) { NewLineStyle = NewLineStyle.Windows });
			var macNewLineStyleMinifier = new XmlMinifier(
				new XmlMinificationSettings(true) { NewLineStyle = NewLineStyle.Mac });
			var unixNewLineStyleMinifier = new XmlMinifier(
				new XmlMinificationSettings(true) { NewLineStyle = NewLineStyle.Unix });

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