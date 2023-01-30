using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Xml.Minification
{
	public class CollapsingTagsWithoutContentTests
	{
		[Fact]
		public void CollapsingTagsWithoutContent()
		{
			// Arrange
			var notCollapsingMinifier = new XmlMinifier(
				new XmlMinificationSettings(true) { CollapseTagsWithoutContent = false });
			var collapsingMinifier = new XmlMinifier(
				new XmlMinificationSettings(true) { CollapseTagsWithoutContent = true });

			const string input1 = "<node></node>";
			const string targetOutput1A = input1;
			const string targetOutput1B = "<node/>";

			const string input2 = "<node>  \t \n  \n</node>";

			const string input3 = "<row RoleId=\"4\" RoleName=\"Administrator\"></row>\n" +
				"<row RoleId=\"5\" RoleName=\"Contributor\"></row>\n" +
				"<row RoleId=\"6\" RoleName=\"Editor\"></row>"
				;
			const string targetOutput3A = input3;
			const string targetOutput3B = "<row RoleId=\"4\" RoleName=\"Administrator\"/>\n" +
				"<row RoleId=\"5\" RoleName=\"Contributor\"/>\n" +
				"<row RoleId=\"6\" RoleName=\"Editor\"/>"
				;

			const string input4 = "<row RoleId=\"4\" RoleName=\"Administrator\"> </row>\n" +
				"<row RoleId=\"5\" RoleName=\"Contributor\">  \t  </row>\n" +
				"<row RoleId=\"6\" RoleName=\"Editor\"> \n  \n </row>"
				;

			// Act
			string output1A = notCollapsingMinifier.Minify(input1).MinifiedContent;
			string output1B = collapsingMinifier.Minify(input1).MinifiedContent;

			string output2A = notCollapsingMinifier.Minify(input2).MinifiedContent;
			string output2B = collapsingMinifier.Minify(input2).MinifiedContent;

			string output3A = notCollapsingMinifier.Minify(input3).MinifiedContent;
			string output3B = collapsingMinifier.Minify(input3).MinifiedContent;

			string output4A = notCollapsingMinifier.Minify(input4).MinifiedContent;
			string output4B = collapsingMinifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);

			Assert.Equal(input2, output2A);
			Assert.Equal(input2, output2B);

			Assert.Equal(targetOutput3A, output3A);
			Assert.Equal(targetOutput3B, output3B);

			Assert.Equal(input4, output4A);
			Assert.Equal(input4, output4B);
		}
	}
}