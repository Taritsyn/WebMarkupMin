using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Xml.Minification
{
	public class AttributeQuotesRenderingTests
	{
		[Fact]
		public void AttributeQuotesRendering()
		{
			// Arrange
			const string input = "<book author=\"Vasya &quot;Monster&quot; Pupkin\" " +
				"title='HTML: Using the &apos; and &quot; characters' " +
				"publishingHouse=\'O&apos;Hare Media\'/>";
			const string targetOutputA = "<book author=\"Vasya &#34;Monster&#34; Pupkin\" " +
				"title='HTML: Using the &#39; and \" characters' " +
				"publishingHouse='O&#39;Hare Media'/>";
			const string targetOutputB = "<book author='Vasya \"Monster\" Pupkin' " +
				"title='HTML: Using the &#39; and \" characters' " +
				"publishingHouse=\"O'Hare Media\"/>";
			const string targetOutputC = "<book author='Vasya \"Monster\" Pupkin' " +
				"title='HTML: Using the &#39; and \" characters' " +
				"publishingHouse='O&#39;Hare Media'/>";
			const string targetOutputD = "<book author=\"Vasya &#34;Monster&#34; Pupkin\" " +
				"title=\"HTML: Using the ' and &#34; characters\" " +
				"publishingHouse=\"O'Hare Media\"/>";

			var autoAttributeQuoteStyleMinifier = new XmlMinifier(
				new XmlMinificationSettings(true) { AttributeQuotesStyle = XmlAttributeQuotesStyle.Auto });
			var optimalAttributeQuoteStyleMinifier = new XmlMinifier(
				new XmlMinificationSettings(true) { AttributeQuotesStyle = XmlAttributeQuotesStyle.Optimal });
			var singleAttributeQuoteStyleMinifier = new XmlMinifier(
				new XmlMinificationSettings(true) { AttributeQuotesStyle = XmlAttributeQuotesStyle.Single });
			var doubleAttributeQuoteStyleMinifier = new XmlMinifier(
				new XmlMinificationSettings(true) { AttributeQuotesStyle = XmlAttributeQuotesStyle.Double });

			// Act
			string outputA = autoAttributeQuoteStyleMinifier.Minify(input).MinifiedContent;
			string outputB = optimalAttributeQuoteStyleMinifier.Minify(input).MinifiedContent;
			string outputC = singleAttributeQuoteStyleMinifier.Minify(input).MinifiedContent;
			string outputD = doubleAttributeQuoteStyleMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(targetOutputA, outputA);
			Assert.Equal(targetOutputB, outputB);
			Assert.Equal(targetOutputC, outputC);
			Assert.Equal(targetOutputD, outputD);
		}
	}
}