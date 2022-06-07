using System.Collections.Generic;

using Xunit;

using WebMarkupMin.Core;

namespace WebMarkupMin.Tests.Html.Knockout.Minification
{
	public class HandlingBindingExpressionMinificationErrorsTests
	{
		[Fact]
		public void HandlingMinificationErrorsInDataBindAttributesIsCorrect()
		{
			// Arrange
			var minifyingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyKnockoutBindingExpressions = true });

			const string input = "<label for=\"ddlCountry\">Country:</label>\n" +
				"<select id=\"ddlCountry\" name=\"country\" data-bind=\"\n" +
				"	options: availableCountries,\n" +
				"	optionsText: 'countryName,\n" +
				"	value: selectedCountry,\n" +
				"	optionsCaption: 'Choose...'\"></select>"
				;

			// Act
			IList<MinificationErrorInfo> errors = minifyingExpressionsMinifier.Minify(input).Errors;

			// Assert
			Assert.Equal(1, errors.Count);
			Assert.Equal(2, errors[0].LineNumber);
			Assert.Equal(51, errors[0].ColumnNumber);
		}

		[Fact]
		public void HandlingMinificationErrorsInContainerlessCommentsIsCorrect()
		{
			// Arrange
			var minifyingExpressionsMinifier = new HtmlMinifier(
				new HtmlMinificationSettings(true) { MinifyKnockoutBindingExpressions = true });

			const string input = "<header>\n" +
				"    <!--ko compose: { view: 'nav } --><!--/ko-->\n" +
				"</header>\n" +
				"<section id=\"content\" class=\"main container-fluid\">\n" +
				"    <!--ko compose: { model: router.activeItem, \n" +
				"        afterCompose: router.afterCompose, \n" +
				"        transition: 'entrance'' } -->\n" +
				"    <!--/ko-->\n" +
				"</section>\n" +
				"<footer>\n" +
				"    <!--ko compose: { view: footer' } --><!--/ko-->\n" +
				"</footer>"
				;

			// Act
			IList<MinificationErrorInfo> errors = minifyingExpressionsMinifier.Minify(input).Errors;

			// Assert
			Assert.Equal(3, errors.Count);
			Assert.Equal(2, errors[0].LineNumber);
			Assert.Equal(12, errors[0].ColumnNumber);
			Assert.Equal(5, errors[1].LineNumber);
			Assert.Equal(12, errors[1].ColumnNumber);
			Assert.Equal(11, errors[2].LineNumber);
			Assert.Equal(12, errors[2].ColumnNumber);
		}
	}
}