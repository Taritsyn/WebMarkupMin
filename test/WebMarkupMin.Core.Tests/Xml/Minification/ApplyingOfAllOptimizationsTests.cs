using Xunit;

namespace WebMarkupMin.Core.Tests.Xml.Minification
{
	public class ApplyingOfAllOptimizationsTests
	{
		[Fact]
		public void ApplyingOfAllOptimizationsIsCorrect()
		{
			// Arrange
			var emptyTagWithSlashMinifier = new XmlMinifier(
				new XmlMinificationSettings(true)
				{
					MinifyWhitespace = true,
					RemoveXmlComments = true,
					CollapseTagsWithoutContent = true,
					RenderEmptyTagsWithSpace = false
				});
			var emptyTagWithSpaceAndSlashMinifier = new XmlMinifier(
				new XmlMinificationSettings(true)
				{
					MinifyWhitespace = true,
					RemoveXmlComments = true,
					CollapseTagsWithoutContent = true,
					RenderEmptyTagsWithSpace = true
				});

			const string input1 = "  \n	\n  <?xml	 version=\"1.0\"	encoding=\'utf-8\' \n ?>\n" +
				"<xsl:stylesheet  xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\"	version=\'1.0\'>\n" +
				"	<xsl:output   method=\"xml\"   indent=\"yes\"   />\n\n" +
				"	<!-- List of persons template -->\n" +
				"	<xsl:template   match=\"/persons\"  >\n" +
				"		<root>\n" +
				"			<xsl:apply-templates  select=\"person\"><!-- Apply the Person template --></xsl:apply-templates>\n" +
				"		</root>\n" +
				"	</xsl:template>\n" +
				"	<!-- /List of persons template -->\n\n" +
				"	<!-- Person template -->\n" +
				"	<xsl:template    match=\"person\"  >\n" +
				"		<name   username=\"{@username}\"  >\n" +
				"			<!-- Name of person -->\n" +
				"			<xsl:value-of     select=\"name\"   />\n" +
				"			<!-- /Name of person -->\n" +
				"			<xsl:if    test=\"position() != last()\"  >\n" +
				"				<xsl:text>, </xsl:text>\n" +
				"			</xsl:if>\n" +
				"		</name>\n" +
				"	</xsl:template>\n" +
				"	<!-- /Person template -->\n\n" +
				"</xsl:stylesheet>  \n	 \n"
				;
			const string targetOutput1A = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
				"<xsl:stylesheet xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" version=\"1.0\">" +
				"<xsl:output method=\"xml\" indent=\"yes\"/>" +
				"<xsl:template match=\"/persons\">" +
				"<root>" +
				"<xsl:apply-templates select=\"person\"/>" +
				"</root>" +
				"</xsl:template>" +
				"<xsl:template match=\"person\">" +
				"<name username=\"{@username}\">" +
				"<xsl:value-of select=\"name\"/>" +
				"<xsl:if test=\"position() != last()\">" +
				"<xsl:text>, </xsl:text>" +
				"</xsl:if>" +
				"</name>" +
				"</xsl:template>" +
				"</xsl:stylesheet>"
				;
			const string targetOutput1B = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
				"<xsl:stylesheet xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" version=\"1.0\">" +
				"<xsl:output method=\"xml\" indent=\"yes\" />" +
				"<xsl:template match=\"/persons\">" +
				"<root>" +
				"<xsl:apply-templates select=\"person\" />" +
				"</root>" +
				"</xsl:template>" +
				"<xsl:template match=\"person\">" +
				"<name username=\"{@username}\">" +
				"<xsl:value-of select=\"name\" />" +
				"<xsl:if test=\"position() != last()\">" +
				"<xsl:text>, </xsl:text>" +
				"</xsl:if>" +
				"</name>" +
				"</xsl:template>" +
				"</xsl:stylesheet>"
				;

			// Act
			string output1A = emptyTagWithSlashMinifier.Minify(input1).MinifiedContent;
			string output1B = emptyTagWithSpaceAndSlashMinifier.Minify(input1).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1A, output1A);
			Assert.Equal(targetOutput1B, output1B);
		}
	}
}