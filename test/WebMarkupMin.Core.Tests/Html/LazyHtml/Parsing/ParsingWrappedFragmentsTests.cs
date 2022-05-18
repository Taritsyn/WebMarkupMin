using Xunit;

namespace WebMarkupMin.Core.Tests.Html.LazyHtml.Parsing
{
	public class ParsingWrappedFragmentsTests
	{
		[Fact]
		public void ParsingOfWrappedAdSenseCodeIsCorrect()
		{
			// Arrange
			var minifier = new HtmlMinifier(new HtmlMinificationSettings(true));

			const string input = "<div class=\"lazyhtml\" data-lazyhtml onvisible>\n" +
				"	<script type=\"text/lazyhtml\">\n" +
				"		<!--\n" +
				"		<ins class=\"adsbygoogle\"\n" +
				"			style=\"display:block\"\n" +
				"			data-ad-client=\"ca-pub-012345\"\n" +
				"			data-ad-slot=\"0987\"\n" +
				"			data-ad-format=\"auto\"\n" +
				"			data-full-width-responsive=\"true\">\n" +
				"		</ins>\n" +
				"		<script>\n" +
				"			(adsbygoogle = window.adsbygoogle || []).push({});\n" +
				"		</script>\n" +
				"		-->\n" +
				"	</script>\n" +
				"</div>"
				;

			// Act
			string output = minifier.Minify(input).MinifiedContent;

			// Assert
			Assert.Equal(input, output);
		}
	}
}