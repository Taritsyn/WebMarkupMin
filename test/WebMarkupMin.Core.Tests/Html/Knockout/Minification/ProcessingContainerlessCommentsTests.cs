using Xunit;

namespace WebMarkupMin.Core.Tests.Html.Knockout.Minification
{
	public class ProcessingContainerlessCommentsTests
	{
		[Fact]
		public void ProcessingContainerlessCommentsIsCorrect()
		{
			// Arrange
			var removingHtmlCommentsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { RemoveHtmlComments = true });

			const string input1 = "<div>\n" +
				"	<header>\n" +
				"		<!--ko compose: {view: 'nav'}--><!--/ko-->\n" +
				"	</header>\n" +
				"	<section id=\"content\" class=\"main container-fluid\">\n" +
				"		<!-- ko compose: {model: router.activeItem,\n" +
				"		afterCompose: router.afterCompose,\n" +
				"		transition: 'entrance'} -->\n" +
				"		<!-- /ko -->\n" +
				"	</section>\n" +
				"	<footer>\n" +
				"		<!--\n\tko\t\ncompose: {view: 'footer'}\t\n--><!--\n\t/ko\n\t-->\n" +
				"	</footer>\n" +
				"</div>"
				;
			const string targetOutput1 = "<div>\n" +
				"	<header>\n" +
				"		<!--ko compose: {view: 'nav'}--><!--/ko-->\n" +
				"	</header>\n" +
				"	<section id=\"content\" class=\"main container-fluid\">\n" +
				"		<!--ko compose: {model: router.activeItem,\n" +
				"		afterCompose: router.afterCompose,\n" +
				"		transition: 'entrance'}-->\n" +
				"		<!--/ko-->\n" +
				"	</section>\n" +
				"	<footer>\n" +
				"		<!--ko compose: {view: 'footer'}--><!--/ko-->\n" +
				"	</footer>\n" +
				"</div>"
				;

			const string input2 = "<ul>\n" +
				"	<!-- ko foreach: items -->\n" +
				"		<!-- ko template: { name: 'productTemplate'} -->\n" +
				"		<!-- /ko -->\n" +
				"	<!-- /ko -->\n" +
				"</ul>"
				;
			const string targetOutput2 = "<ul>\n" +
				"	<!--ko foreach: items-->\n" +
				"		<!--ko template: { name: 'productTemplate'}-->\n" +
				"		<!--/ko-->\n" +
				"	<!--/ko-->\n" +
				"</ul>"
				;

			// Act
			string output1 = removingHtmlCommentsMinifier.Minify(input1).MinifiedContent;
			string output2 = removingHtmlCommentsMinifier.Minify(input2).MinifiedContent;

			// Assert
			Assert.Equal(targetOutput1, output1);
			Assert.Equal(targetOutput2, output2);
		}
	}
}