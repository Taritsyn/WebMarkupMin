using System.Collections.Generic;

using Xunit;

namespace WebMarkupMin.Core.Test.Xml.Minification
{
	public class IgnoringFragmentsOfMarkupTests
	{
		[Fact]
		public void IgnoringFragmentsOfMarkupIsCorrect()
		{
			// Arrange
			var minifier = new XmlMinifier(new XmlMinificationSettings(true)
			{
				MinifyWhitespace = true
			});

			const string input1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n"+
				"<menu>\n" +
				"	<dish>\n" +
				"		<name>Belgian Waffles</name>\n" +
				"		<price>$6.20</price>\n" +
				"		<description>Two of our famous Belgian Waffles with plenty of real maple syrup</description>\n" +
				"	</dish>\n" +
				"<!--wmm:ignore-->\n" +
				"	<dish>\n" +
				"		<name>Berry-Berry Belgian Waffles</name>\n" +
				"		<price>$9.20</price>\n" +
				"		<description>Light Belgian waffles covered with an assortment of fresh berries and whipped cream</description>\n" +
				"	</dish>\n" +
				"	<dish>\n" +
				"		<name>French Toast</name>\n" +
				"		<price>$5.50</price>\n" +
				"		<description>Thick slices made from our homemade sourdough bread</description>\n" +
				"	</dish>\n" +
				"<!--/wmm:ignore-->\n" +
				"	<dish>\n" +
				"		<name>Homestyle Breakfast</name>\n" +
				"		<price>$7.20</price>\n" +
				"		<description>Two eggs, bacon or sausage, toast, and our ever-popular hash browns</description>\n" +
				"	</dish>\n" +
				"<!--wmm:ignore-->\n" +
				"	<dish>\n" +
				"		<name>Strawberry Belgian Waffles</name>\n" +
				"		<price>$8.20</price>\n" +
				"		<description>Light Belgian waffles covered with strawberries and whipped cream</description>\n" +
				"	</dish>\n" +
				"<!--/wmm:ignore-->\n" +
				"</menu>"
				;
			const string targetOutput1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
				"<menu>" +
				"<dish>" +
				"<name>Belgian Waffles</name>" +
				"<price>$6.20</price>" +
				"<description>Two of our famous Belgian Waffles with plenty of real maple syrup</description>" +
				"</dish>\n" +
				"	<dish>\n" +
				"		<name>Berry-Berry Belgian Waffles</name>\n" +
				"		<price>$9.20</price>\n" +
				"		<description>Light Belgian waffles covered with an assortment of fresh berries and whipped cream</description>\n" +
				"	</dish>\n" +
				"	<dish>\n" +
				"		<name>French Toast</name>\n" +
				"		<price>$5.50</price>\n" +
				"		<description>Thick slices made from our homemade sourdough bread</description>\n" +
				"	</dish>\n" +
				"<dish>" +
				"<name>Homestyle Breakfast</name>" +
				"<price>$7.20</price>" +
				"<description>Two eggs, bacon or sausage, toast, and our ever-popular hash browns</description>" +
				"</dish>\n" +
				"	<dish>\n" +
				"		<name>Strawberry Belgian Waffles</name>\n" +
				"		<price>$8.20</price>\n" +
				"		<description>Light Belgian waffles covered with strawberries and whipped cream</description>\n" +
				"	</dish>\n\n" +
				"</menu>"
				;

			const string input2 = "<!--wmm:ignore--><!--/wmm:ignore-->";
			const string targetOutput2 = "";

			const string input3 = "<!--wmm:ignore-->";

			const string input4 = "<row RoleId=\"4\" RoleName=\"Administrator\"/>\n" +
				"<!--wmm:ignore--><row RoleId=\"5\" RoleName=\"Contributor\"/>\n" +
				"<row RoleId=\"6\" RoleName=\"Editor\"/>"
				;

			const string input5 = "<!--/wmm:ignore-->";

			const string input6 = "<row RoleId=\"4\" RoleName=\"Administrator\"/>\n" +
				"<row RoleId=\"5\" RoleName=\"Contributor\"/><!--/wmm:ignore-->\n" +
				"<row RoleId=\"6\" RoleName=\"Editor\"/>"
				;

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			IList<MinificationErrorInfo> errors3 = minifier.Minify(input3).Errors;
			IList<MinificationErrorInfo> errors4 = minifier.Minify(input4).Errors;
			IList<MinificationErrorInfo> errors5 = minifier.Minify(input5).Errors;
			IList<MinificationErrorInfo> errors6 = minifier.Minify(input6).Errors;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);

			Assert.Equal(1, errors3.Count);
			Assert.Equal(1, errors3[0].LineNumber);
			Assert.Equal(1, errors3[0].ColumnNumber);

			Assert.Equal(1, errors4.Count);
			Assert.Equal(2, errors4[0].LineNumber);
			Assert.Equal(1, errors4[0].ColumnNumber);

			Assert.Equal(1, errors5.Count);
			Assert.Equal(1, errors5[0].LineNumber);
			Assert.Equal(1, errors5[0].ColumnNumber);

			Assert.Equal(1, errors6.Count);
			Assert.Equal(2, errors6[0].LineNumber);
			Assert.Equal(41, errors6[0].ColumnNumber);
		}
	}
}