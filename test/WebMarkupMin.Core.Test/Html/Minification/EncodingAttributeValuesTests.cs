using Xunit;

namespace WebMarkupMin.Core.Test.Html.Minification
{
	public class EncodingAttributeValuesTests
	{
		[Fact]
		public void EncodingAttributeValuesIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input1 = "<input value='<a href=\"/product.asp?id=12&category=5&returnUrl=%2Fdefault.asp\">" +
				"Show product</a>'>";
			const string targetOutput1 = "<input value=\"&lt;a href=&#34;/product.asp?id=12&amp;category=5" +
				"&amp;returnUrl=%2Fdefault.asp&#34;>Show product&lt;/a>\">";

			const string input2 = "<input value=\"<a href='/product.asp?id=12&category=5&returnUrl=%2Fdefault.asp'>" +
				"Show product</a>\">";
			const string targetOutput2 = "<input value=\"&lt;a href='/product.asp?id=12&amp;category=5&amp;returnUrl=%2Fdefault.asp'>" +
				"Show product&lt;/a>\">";

			const string input3 = "<select>\n" +
				"	<option value='Douglas Crockford&#39;s JS Minifier'>Douglas Crockford's JS Minifier</option>\n" +
				"	<option value='Microsoft Ajax JS Minifier'>Microsoft Ajax JS Minifier</option>\n" +
				"	<option value='YUI JS Minifier'>YUI JS Minifier</option>\n" +
				"</select>"
				;
			const string targetOutput3 = "<select>\n" +
				"	<option value=\"Douglas Crockford's JS Minifier\">Douglas Crockford's JS Minifier</option>\n" +
				"	<option value=\"Microsoft Ajax JS Minifier\">Microsoft Ajax JS Minifier</option>\n" +
				"	<option value=\"YUI JS Minifier\">YUI JS Minifier</option>\n" +
				"</select>"
				;

			const string input4 = "<input type=\"button\" value=\"Remove article &quot;Паранойя оптимизатора&quot;\">";
			const string targetOutput4 = "<input type=\"button\" value=\"Remove article &#34;Паранойя оптимизатора&#34;\">";

			// Act
			string output1 = minifier.Minify(input1).MinifiedContent;
			string output2 = minifier.Minify(input2).MinifiedContent;
			string output3 = minifier.Minify(input3).MinifiedContent;
			string output4 = minifier.Minify(input4).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
			Assert.Equal(targetOutput3, output3);
			Assert.Equal(targetOutput4, output4);
		}
	}
}