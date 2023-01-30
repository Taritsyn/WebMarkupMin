using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Core.Tests.Xml.Minification
{
	public class EncodingAttributeValuesTests
	{
		[Fact]
		public void EncodingAttributeValues()
		{
			// Arrange
			var minifier = new XmlMinifier(new XmlMinificationSettings(true));

			const string input1 = "<product url=\"/product.asp?id=12&category=5&returnUrl=%2Fdefault.asp\"></product>";
			const string targetOutput1 = "<product url=\"/product.asp?id=12&amp;category=5&amp;returnUrl=%2Fdefault.asp\"></product>";

			const string input2 = "<product url='/product.asp?id=12&category=5&returnUrl=%2Fdefault.asp'></product>";
			const string targetOutput2 = "<product url=\"/product.asp?id=12&amp;category=5&amp;returnUrl=%2Fdefault.asp\"></product>";

			const string input3 = "<article description=\'Знаменитая статья Артемия Лебедева \"Паранойя оптимизатора\"\'/>";
			const string targetOutput3 = "<article description=\"Знаменитая статья Артемия Лебедева &quot;Паранойя оптимизатора&quot;\"/>";

			const string input4 = "<article description=\"Знаменитая статья Артемия Лебедева 'Паранойя оптимизатора'\"/>";
			const string targetOutput4 = input4;

			const string input5 = "<article description=\"Знаменитая статья Артемия Лебедева <Паранойя оптимизатора>\"/>";
			const string targetOutput5 = "<article description=\"Знаменитая статья Артемия Лебедева &lt;Паранойя оптимизатора&gt;\"/>";

			const string input6 = "<minifiers>\n" +
				"	<add displayName='Douglas Crockford&apos;s JS Minifier'/>\n" +
				"	<add displayName='Microsoft Ajax JS Minifier'/>\n" +
				"	<add displayName='YUI JS Minifier'/>\n" +
				"</minifiers>"
				;
			const string targetOutput6 = "<minifiers>\n" +
				"	<add displayName=\"Douglas Crockford's JS Minifier\"/>\n" +
				"	<add displayName=\"Microsoft Ajax JS Minifier\"/>\n" +
				"	<add displayName=\"YUI JS Minifier\"/>\n" +
				"</minifiers>"
				;

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			string output3 = minifier.Minify(input3).MinifiedContent;
			string output4 = minifier.Minify(input4).MinifiedContent;
			string output5 = minifier.Minify(input5).MinifiedContent;
			string output6 = minifier.Minify(input6).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
			Assert.Equal(targetOutput5, output5);
			Assert.Equal(targetOutput6, output6);
		}
	}
}