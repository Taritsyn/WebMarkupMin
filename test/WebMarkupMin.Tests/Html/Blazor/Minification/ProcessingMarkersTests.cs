using System;

using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Blazor.Minification
{
	public class ProcessingMarkersTests : IDisposable
	{
		private HtmlMinifier _removingHtmlCommentsMinifier;


		public ProcessingMarkersTests()
		{
			_removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = true }
			);
		}


		[Fact]
		public void ProcessingPrerenderedServerComponentMarkers()
		{
			// Arrange
			const string input1 = "    <app>\n" +
				"        <!--Blazor:{\"sequence\":0,\"type\":\"server\"," +
				"\"prerenderId\":\"06e9ae1e55c3437eb41a4c1d63cd9dc8\"," +
				"\"descriptor\":\"CfDJ8NK75dCQyrlElvJvfLSrrguC1bsLxXsAuW0q1g/H0zl" +
				"\u002Bp\u002B\u002B/TqOa8setXFvAD\u002Bz9A9MdprpBvSN557bPtkPK5YlCmXwocCGGDrR3O3QN" +
				"\u002BcwXhn0sAYeIUqaj2\u002BYyQXKeetXKhsX4j5OCYaH\u002B3UJAguu8" +
				"\u002B\u002BxsuGS8CUGjJwwhbfUC5wcmo4LziUz7XMrTYXaYEOMmzFA7Eq5ved31kdZZaOFECuhYfpY" +
				"\u002BgsmXo1c\u002BTiyf6ZX2vCKycTBtAPxWMT3O01Bh8171scgfPRUWlSm0iV5WgTc9hkv9x/bpQtOz0LcC" +
				"\u002BDH/MVr3fKNRagRNX/azUfUei45AhSBobLliNugCSYAPBn1v9vtoD9nqXy0ciFLn\"}-->\n" +
				"    <div>\n" +
				"        <h1>Counter</h1>\n\n" +
				"<p>Current count: 0</p>\n\n" +
				"<button class=\"btn btn-primary\">Click me</button>\n" +
				"    </div>\n" +
				"    <!--Blazor:{\"prerenderId\":\"06e9ae1e55c3437eb41a4c1d63cd9dc8\"}-->\n" +
				"    </app>"
				;
			const string input2 = "<main b-j65y7q72mt>\r\n" +
				"	<article class=\"content px-4\" b-j65y7q72mt>\r\n" +
				"		<!-- Blazor:{\"type\":\"server\"," +
				"\"prerenderId\":\"94be537a035c4ff795625471538fa4d9\"," +
				"\"key\":{\"locationHash\":\"7E0B9CD317B666EFE1B6FA61FAD6E10DCE0D0FFEAAF51FBA430B540DE5DB4B34:0\"," +
				"\"formattedComponentKey\":\"\"}," +
				"\"sequence\":0," +
				"\"descriptor\":\"CfDJ8BHIeHq60AFHs07HbcrL\\u002B8ZhD7a78WBF3H7mID3que6Tr9S5HnyBvKn1aiqAoN" +
				"\\u002BXeAca\\u002BVAel0juJl98ACnB9KJXxg38SZRVVPxFdXtSjY8ypdLGWY6p8PSFceQbTk9XCy5EVuPewhW" +
				"C0A3BbjcYoMeQ2833OSlz0y55aJjnjXUASNxQX53bImHWg/QFR9Za4LG/3rBevnsbh" +
				"\\u002BehLlGEhLGtMPFKKHx7eu2laULdY/60KjAmfqr36YYs44qRXTfXRs44g5D" +
				"\\u002BHw8lsAecWODdgjoc5ISONrzwkQKbIJZLXaZyVq9LVAmrNctvho/H3f5/kBnjgMNNv/k2nc7QTJyYfvjy8IX/k" +
				"\\u002BBy4hY6NUfQtr2HVFW8kTj\\u002Blt7dnjJOuEbvKLvuBiMRiswKGvxkn35uZRJSlgXC7HtUysHFFawJ8S10l" +
				"tCyJFTkK4E9TXmEHwXE8piRmBtjVe/tS8MP41XnOuMq5tRKIa8OHsSdesnI7tDD0LoBVJsteWXTOlDyog8leU9MZ9WUn" +
				"eAPwXK1ALsp3I8=\"} -->\r\n\r\n" +
				"		<h1>Counter</h1>\r\n\r\n" +
				"		<p role=\"status\">Current count: 0</p>\r\n\r\n" +
				"		<button class=\"btn btn-primary\">Click me</button>\r\n" +
				"		<!-- Blazor:{\"prerenderId\":\"94be537a035c4ff795625471538fa4d9\"} -->\r\n" +
				"	</article>\r\n" +
				"</main>"
				;

			// Act
			string output1 = _removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;
			string output2 = _removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(input1, output1);
			Assert.Equal(input2, output2);
		}

		[Fact]
		public void ProcessingServerComponentMarkers()
		{
			// Arrange
			const string input = "    <app>\n" +
				"        <!--Blazor:{\"sequence\":0,\"type\":\"server\"," +
				"\"descriptor\":\"CfDJ8NK75dCQyrlElvJvfLSrrgsusP7rGOvnC/" +
				"WXFUytyBYGcQSY7IMzMiXKdOuDQ/fhxDAKzXeNdf3NeExaW2C" +
				"\u002BqOKNVNTyILq87lxEbrgxr3Gh\u002BKW\u002BL0SYKtGgqMs4CGa" +
				"\u002ByvFM5bTecRi8uEEs/szWltI3muNRxRuleATXIqrUXr" +
				"\u002B2SGcyuMnqgdXIaqyiyUPdeXn5bgTxKO9gqFoYIJHXvA8YRNUoY7lv40l1oUMsRNVVD/" +
				"GHXWOwRhpsHehXvx2Vbz/eAbvSDOjFJ2UuVMuIPXOxN1GUj7gEpzvLNK9uRfarj" +
				"\u002BoV9ajUx3lz4Bk0o9uRRR2aDgevrG7hvTOXmg7bNt1CrsRW38pGmXs1vAvo\u002BI3G\"}-->\n" +
				"    </app>"
				;

			// Act
			string output = _removingHtmlCommentsMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(input, output);
		}

		[Fact]
		public void ProcessingServerPersistedStateMarkers()
		{
			// Arrange
			const string input = "<!--Blazor-Server-Component-State:CfDJ8BHIeHq" +
				"60AFHs07HbcrL+8Z237okHrDNhCV4LnykC89Z+MrgCnKtCI7eMr/aopu/QZZMp" +
				"4BLKGIj9HbzpGCdSe+j9R2oiZDW2sG2g0HubJooIt6wMnexSveJ3vRqMP0xf+S" +
				"z/zVyntGRKboq+Tbn8tcohJeO6Zyv0SnKsbkezIU5oszGiG38CyIIzzeepLeKH" +
				"uy+VOJAhjekx40k3DZ0DhzclTeJq6njtxX+6wd/DWSLP1eRwqejr7JJOWHbNFE" +
				"qYpEv8M5O4uYg7ZYjgH+V/2gLwDDfydVVhOBqszaBpwI4Lr88LEx5xb8hCz6Ox" +
				"Hg1bbPDyx43MReLrSE3IFgPfoq8/rw81Xk16nrsuK5UjVoX+Fue6awfJlrSURj" +
				"+bWeHMS7p0iZgYdkn1AdJpGfC/CQZL4GwtbUmF5E8PVjfCfmXJ9ZI/Evfbc0pNU" +
				"3mJlXHAvo5OEWyOpt2z+sOsiYfYyNKI14HI6FR8iaOSSEI1ejBXKWWc/ou8Gfui" +
				"MlBq65wDq2B9X6j0nI5WD2MWOPO1fPQi3azr3V5aJwLuKRQDvn4-->"
				;

			// Act
			string output = _removingHtmlCommentsMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(input, output);
		}

		[Fact]
		public void ProcessingWebAssemblyPersistedStateMarkers()
		{
			// Arrange
			const string input = "<!--Blazor-WebAssembly-Component-State:eyJfX2" +
				"ludGVybmFsX19BbnRpZm9yZ2VyeVJlcXVlc3RUb2tlbiI6ImV5SjJZV3gxWlNJ" +
				"NklrTm1SRW80VERaTGVEZHpaRnAzUWtWclpVZGhhVzFKVnpoMVNrdG5TM2hYVV" +
				"hoWU1tcDBjVEpHWVhWRFYwTXlXbmQ0ZHpreWFWSkRkbUZYUVY4d1ExQkVWVkpw" +
				"YkhWUFoxWm9SRlpHWHpaSlJXZG9lamx5Ymw5VFFrcFNPRmRRV21aVk5XeHlXRT" +
				"FaVGt0T1pVaFFOa2g2YkhGMmVHTnFZbUpYT0RKTFEwUnFjSGRvYzE5dU1sZGhi" +
				"WEZOVlZFMU1FNWhlbWxxWVd4RVJUSkpJaXdpWm05eWJVWnBaV3hrVG1GdFpTST" +
				"ZJbDlmVW1WeGRXVnpkRlpsY21sbWFXTmhkR2x2YmxSdmEyVnVJbjA9In0=-->"
				;

			// Act
			string output = _removingHtmlCommentsMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(input, output);
		}

		[Fact]
		public void ProcessingWebInitializerMarkers()
		{
			// Arrange
			const string input = "<!--Blazor-Web-Initializers:Ww0KICAiQmxhem9yU" +
				"1NSX1dlYk1hcmt1cE1pbi5saWIubW9kdWxlLmpzIg0KXQ==-->"
				;

			// Act
			string output = _removingHtmlCommentsMinifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(input, output);
		}

		#region IDisposable implementation

		public void Dispose()
		{
			_removingHtmlCommentsMinifier = null;
		}

		#endregion
	}
}